using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Extensions;
using KristaShop.Common.Helpers;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Session;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Views;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Module.Common.Business.Interfaces;
using Module.Common.Business.Models;
using Module.Order.Business.Interfaces;
using Module.Order.Business.Models;
using Module.Order.Business.UnitOfWork;
using Serilog;

namespace Module.Order.Business.Services {
    public class Order1CService : IOrder1CService {
        private readonly KristaShopDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;
        private readonly IUnitOfWork _uow;
        private readonly UrlSetting _urlSettings;
        private readonly GlobalSettings _globalSettings;

        public Order1CService(KristaShopDbContext context, IMapper mapper, ILogger logger,
            IEmailService emailService, IOptions<UrlSetting> urlSettings, IOptions<GlobalSettings> globalSettings,
            IFileService fileService, IWebHostEnvironment env, IUnitOfWork uow) {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _emailService = emailService;
            _fileService = fileService;
            _env = env;
            _uow = uow;
            _urlSettings = urlSettings.Value;
            _globalSettings = globalSettings.Value;
        }

        public async Task<OperationResult> CreateOrderAsync(CreateOrderDTO createOrder, UserSession user) {
            try {
                await _context.Database.BeginTransactionAsync();

                var orderDetails = _mapper.Map<List<OrderDetails>>(createOrder.CartItems.Where(x => x.Amount > 0).ToList());

                if (orderDetails.Count == 0) {
                    return OperationResult.Failure("Отсутствуют позиции в корзине для оформления заказа.");
                }

                var entitiesOrderDetails = await _createOrderDetailsFromStorehouseRests(orderDetails);

                var entityOrder = _mapper.Map<KristaShop.DataAccess.Entities.DataFor1C.Order>(createOrder);
                entityOrder.CreateDate = DateTime.Now;
                entityOrder.IsProcessedPreorder = false;
                entityOrder.Description = string.IsNullOrEmpty(createOrder.Description) ? string.Empty : createOrder.Description;
                _context.Orders1C.Add(entityOrder);
                await _context.SaveChangesAsync();

                foreach (var orderDetail in entitiesOrderDetails) {
                    orderDetail.OrderId = entityOrder.Id;
                }
                
                await _context.Order1CDetails.AddRangeAsync(entitiesOrderDetails);
                
                await _uow.Carts.ClearUserCartAsync(createOrder.UserId);
                await _uow.SaveChangesAsync();
                
                _context.Database.CommitTransaction();

                await _sendEmailsAsync(createOrder, entityOrder.Id, user);

                return OperationResult.Success($"Заказ успешно оформлен");
            } catch (ExceptionBase ex) {
                _context.Database.RollbackTransaction();
                _logger.Error(ex, "Failed to create order. {message}", ex.Message);
                return OperationResult.Failure(ex.ReadableMessage);
            }
            catch (Exception) {
                _context.Database.RollbackTransaction();
                throw;
            }
        }

        public async Task<OperationResult> CreateGuestOrderAsync(string fullName, string phone, GuestSession userSession) {
            try {
                await _context.Database.BeginTransactionAsync();

                var entity = new NewUser();
                entity.FullName = fullName;
                entity.Phone = phone;
                entity.MallAddress = "";
                entity.Login = entity.CreateLoginFromName();
                entity.Password = HashHelper.TransformPassword(Generator.NewString(10));
                entity.ManagerId = 0;
                entity.UserId = userSession.UserId;

                _context.Set<NewUser>().Add(entity);
                await _context.SaveChangesAsync();

                _context.Database.CommitTransaction();

                //await _sendEmailsAsync(createOrder.CartItems, entityOrder.Id, user); // TO DO ???

                return OperationResult.Success($"Заказ успешно оформлен! В ближайшее время с Вами свяжется наш менеджер и уточнит детали.");
            } catch (ExceptionBase ex) {
                _context.Database.RollbackTransaction();
                _logger.Error(ex, "Failed to create guest order. {message}", ex.Message);
                return OperationResult.Failure(ex.ReadableMessage);
            } catch (Exception ex) {
                _context.Database.RollbackTransaction();
                throw;
            }
        }

        private async Task<List<OrderDetails>> _createOrderDetailsFromStorehouseRests(IReadOnlyCollection<OrderDetails> orderDetails) {
            var catalogIds = orderDetails.Select(x => x.CatalogId).Distinct().ToList();
            var catalogs = await _context.Catalogs.Where(x => catalogIds.Contains(x.Id)).ToDictionaryAsync(k => k.Id, v => v);

            var modelIds = orderDetails.Select(x => x.ModelId).Distinct().ToList();
            var nomenclatureIds = orderDetails.Select(x => x.NomenclatureId).Distinct().ToList();
            var rests = await _getStorehouseRestsAsync(modelIds, nomenclatureIds);
            var catalogRests = await _getCatalogItemsAsync(modelIds, nomenclatureIds);

            var currentCollection = await _uow.Collections.GetCurrentCollectionAsync();
            var result = new List<OrderDetails>();
            foreach (var details in orderDetails) {
                details.CollectionId = currentCollection.Id;
                if (!catalogs[details.CatalogId].NeedCheckStorehouseRests()) {

                    var modelCatalogRests = catalogRests.Where(x =>
                        x.CatalogId == (int) details.CatalogId && x.NomenclatureId == details.NomenclatureId &&
                        x.ModelId == details.ModelId && x.ColorId == details.ColorId).ToList();

                    var totalRestsAmountForPreorderModel = modelCatalogRests.Sum(x => x.Amount);
                    if (details.Amount > totalRestsAmountForPreorderModel) {
                        throw new ExceptionBase($"Not enough preorder catalog rests for position modelId: {details.ModelId}, nomenclatureId: {details.NomenclatureId}, colorId: {details.ColorId}. Required {details.Amount} but found {totalRestsAmountForPreorderModel}",
                            $"Невозможно оформить заказ на позицию {details.Articul} {details.SizeValue} {details.ColorName}. Количество в заказе {details.Amount}, доступное количество {totalRestsAmountForPreorderModel}");
                    }

                    var toSplitPreorder = details.Amount;
                    foreach (var catalogRest in modelCatalogRests) {
                        if(toSplitPreorder <= 0) break;

                        if (toSplitPreorder - catalogRest.Amount < 0) {
                            var newDetail = (OrderDetails) details.Clone();
                            newDetail.Amount = toSplitPreorder;
                            newDetail.StorehouseId = 0;
                            result.Add(newDetail);

                            await _subtractFromStorehouseRestsAmountInCatalogAsync(details.ModelId, details.NomenclatureId, details.ColorId, details.CatalogId, toSplitPreorder);

                            catalogRest.Amount -= toSplitPreorder;
                            toSplitPreorder = 0;
                            break;
                        }

                        if (toSplitPreorder - catalogRest.Amount >= 0) {
                            var newDetail = (OrderDetails) details.Clone();
                            newDetail.Amount = catalogRest.Amount;
                            newDetail.StorehouseId = 0;
                            result.Add(newDetail);

                            await _subtractFromStorehouseRestsAmountInCatalogAsync(details.ModelId, details.NomenclatureId, details.ColorId, details.CatalogId, catalogRest.Amount);

                            toSplitPreorder -= catalogRest.Amount;
                            catalogRest.Amount = 0;
                        }
                    }

                    continue;
                }

                List<StorehouseRests> modelRests;
                if (details.NomenclatureId > 0) {
                    modelRests = rests
                        .Where(x => x.ModelId == details.ModelId
                                    && x.NomenclatureId == details.NomenclatureId
                                    && x.ColorId == details.ColorId
                                    && !x.IsLine
                                    && x.Amount > 0)
                        .OrderBy(x => x.StorehousePriority)
                        .ToList();
                } else {
                    modelRests = rests
                        .Where(x => x.ModelId == details.ModelId
                                    && x.ColorId == details.ColorId
                                    && x.IsLine
                                    && x.Amount > 0)
                        .OrderBy(x => x.StorehousePriority)
                        .ToList();
                }

                var totalRestsAmountForModel = modelRests.Sum(x => x.Amount);
                if (details.Amount > totalRestsAmountForModel) {
                    throw new ExceptionBase($"Not enough storehouse rests for position modelId: {details.ModelId}, nomenclatureId: {details.NomenclatureId}, colorId: {details.ColorId}. Required {details.Amount} but found {totalRestsAmountForModel}",
                        $"Невозможно оформить заказ на позицию {details.Articul} {details.SizeValue} {details.ColorName}. Количество в заказе {details.Amount}, найдено на складе {totalRestsAmountForModel}");
                }

                var toSplit = details.Amount;
                foreach (var rest in modelRests) {
                    if(toSplit <= 0) break;

                    if (toSplit - rest.Amount < 0) {
                        var newDetail = (OrderDetails) details.Clone();
                        newDetail.Amount = toSplit;
                        newDetail.StorehouseId = rest.StorehouseId;
                        newDetail.NomenclatureId = rest.NomenclatureId;
                        result.Add(newDetail);

                        await _setStorehouseRestsAmountAsync(rest.Id, rest.Amount - toSplit);
                        await _subtractFromStorehouseRestsAmountInCatalogAsync(details.ModelId, details.NomenclatureId, details.ColorId, details.CatalogId, toSplit);

                        rest.Amount -= toSplit;
                        toSplit = 0;
                        break;
                    }

                    if (toSplit - rest.Amount >= 0) {
                        var newDetail = (OrderDetails) details.Clone();
                        newDetail.Amount = rest.Amount;
                        newDetail.StorehouseId = rest.StorehouseId;
                        newDetail.NomenclatureId = rest.NomenclatureId;
                        result.Add(newDetail);

                        await _setStorehouseRestsAmountAsync(rest.Id, 0);
                        await _subtractFromStorehouseRestsAmountInCatalogAsync(details.ModelId, details.NomenclatureId, details.ColorId, details.CatalogId, rest.Amount);

                        toSplit -= rest.Amount;
                        rest.Amount = 0;
                    }
                }
            }

            return result;
        }

        private async Task<List<StorehouseRests>> _getStorehouseRestsAsync(List<int> modelIds, List<int> nomenclatureIds) {

            var sqlStr = $@"SELECT `rests`.`id`, `rests`.`model`, `rests`.`color`, `rests`.`kolichestvo`, `rests`.`sklad`, `rests`.`nomenklatura`,
`storehouse`.`name`, `storehouse`.`obsch`, `storehouse`.`mesto`, `rests`.`line` 
FROM `1c_sklad` AS `rests`
LEFT JOIN `1c_sklady` AS `storehouse` ON `rests`.`sklad` = `storehouse`.`id`
WHERE (`rests`.`model` IN ({string.Join(", ", modelIds)}) OR `rests`.`nomenklatura` IN ({string.Join(", ", nomenclatureIds)})) AND `rests`.`kolichestvo` > 0
ORDER BY `storehouse`.`mesto` DESC;";

            var result = await _context.Set<StorehouseRests>().FromSqlRaw(sqlStr).ToListAsync();

            return result;
        }

        private async Task<List<CatalogItem1CAmount>> _getCatalogItemsAsync(List<int> modelIds, List<int> nomenclatureIds) {
            var sqlStr = $@"SELECT `catalog`.`id`, `catalog`.`model`, `catalog`.`color`, `catalog`.`razmer`, `catalog`.`nomenklatura`,
`catalog`.`kolichestvo`, `catalog`.`razdel`, `catalog`.`price`, `catalog`.`price_rub`
FROM `1c_catalog` AS `catalog`
WHERE `catalog`.`model` IN ({string.Join(", ", modelIds)}) OR `catalog`.`nomenklatura` IN ({string.Join(", ", nomenclatureIds)});";
            var result = await _context.Set<CatalogItem1CAmount>().FromSqlRaw(sqlStr).ToListAsync();

            return result;
        }

        private async Task _setStorehouseRestsAmountAsync(int restsId, int amount) {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"UPDATE `1c_sklad` SET `kolichestvo` = {amount} WHERE `id` = {restsId};");
            await _context.SaveChangesAsync();
        }

        private async Task _subtractFromStorehouseRestsAmountInCatalogAsync(int modelId, int nomenclatureId, int colorId, CatalogType catalogId, int amount) {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"UPDATE `1c_catalog` SET `kolichestvo` = IF(`kolichestvo` - {amount} < 0, 0, `kolichestvo` - {amount}) WHERE `model` = {modelId} AND `nomenklatura` = {nomenclatureId} AND `color` = {colorId} AND `razdel` = {(int) catalogId};");
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetModelDescriptionAsync(int modelId, int colorId, string mode = null) {
            var resultDescription = string.Empty;

            try {
                var sqlStr = $"SELECT CONCAT_WS(' ', `articul`, `line`) AS `value` FROM `1c_models` WHERE `id`={modelId}";
                var result = await _context.Set<Scalar>().FromSqlRaw(sqlStr).FirstOrDefaultAsync();
                if (result == null) {
                    return string.Empty;
                } else {
                    resultDescription = result.Value;
                }

                sqlStr = $"SELECT `name` AS `value` FROM `1c_colors` WHERE `id`={colorId}";
                result = await _context.Set<Scalar>().FromSqlRaw(sqlStr).FirstOrDefaultAsync();
                if (result == null) {
                    return string.Empty;
                } else {
                    resultDescription += " " + result.Value + (string.IsNullOrEmpty(mode) ? " (предзаказ)" : "");
                }

                return resultDescription;
            } catch (Exception) {
                return string.Empty;
            }
        }

        public async Task<List<OrderAdminDTO>> GetAllOrdersAsync(int orderId = 0, int managerId = 0, int modelId = 0,
            int colorId = 0, int[] ordersIDs = null, string catalogsMode = null, bool unprocessedOnly = false, bool includeUserCartCheck = false,
            bool onlyNotEmptyUserCarts = false, List<string> articuls = null, List<int> catalogIds = null,
            List<int> userIds = null, List<int> colorIds = null, List<int> cityIds = null, List<int> managerIds = null, bool onlyVisibleForManager = false) {
            var whereStatement = string.Empty;
            var havingStatement = string.Empty;
            if (orderId > 0) {
                whereStatement = (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + $"`orders`.`id` = {orderId}";
            }

            if (managerId > 0) {
                if (onlyVisibleForManager) {
                    managerIds = await _uow.ManagerAccess.GetManagerIdsAccessesFor(managerId, ManagerAccessToType.Orders).ToListAsync();
                } else {
                    whereStatement = (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + $"(`managers`.`id` = {managerId} OR `managers`.`id` IS NULL)";
                }
            }

            if (modelId > 0 && colorId > 0) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + $"`details`.`model_id` = {modelId} AND `details`.`color_id` = {colorId}";
                if (string.IsNullOrEmpty(catalogsMode)) {
                    whereStatement += $" AND `details`.`catalog_id` = {(int)CatalogType.Preorder} AND `orders`.`is_processed_preorder`= 0";
                }
            }

            if (unprocessedOnly) {
                havingStatement += (!string.IsNullOrEmpty(havingStatement) ? " AND " : "") + "(`preorder_amount` > 0 AND `orders`.`is_processed_preorder` = 0) OR (`retail_amount` > 0 AND `orders`.`is_processed_retail` = 0)";
            }

            if (ordersIDs != null && ordersIDs.Length > 0) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + $"`orders`.`id` IN ({string.Join(',', ordersIDs)})";
            }

            if (catalogIds != null && catalogIds.Any()) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + $"`details`.`catalog_id` IN ({string.Join(",", catalogIds)})";
            }

            if (articuls != null && articuls.Any()) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + $"`models`.`articul` IN ('{string.Join("' ,'", articuls)}')";
            }

            if (userIds != null && userIds.Any()) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + $"`orders`.`user_id` IN ({string.Join(",", userIds)})";
            }

            if (colorIds != null && colorIds.Any()) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + $"`details`.`color_id` IN ({string.Join(",", colorIds)})";
            }

            if (cityIds != null && cityIds.Any()) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + $"`cities`.`id` IN ({string.Join(",", cityIds)})";
            }

            if (managerIds != null && managerIds.Any()) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + $"`managers`.`id` IN ({string.Join(",", managerIds)})";
                if (managerIds.Contains(0)) {
                    whereStatement += " OR `managers`.`id` is null";
                }
            }

            var selectCartItemsField = " 0 AS `has_cart_items`, ";
            var joinCartItems = string.Empty;

            if (includeUserCartCheck || onlyNotEmptyUserCarts) {
                selectCartItemsField = " ifnull(`cart`.`has_cart_items`, 0) as `has_cart_items`, ";
                joinCartItems = @"LEFT JOIN (SELECT `items`.`user_id`, IF(count(`items`.`id`) > 0, 1, 0) AS `has_cart_items`
                                FROM `cart_items_1c` AS `items`
                                GROUP BY `items`.`user_id`) AS `cart` ON `orders`.`user_id` = `cart`.`user_id`";
            }

            if (onlyNotEmptyUserCarts) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + "`cart`.`has_cart_items` = 1";
            }

            if (!string.IsNullOrEmpty(whereStatement)) {
                whereStatement = "WHERE " + whereStatement;
            }

            if (!string.IsNullOrEmpty(havingStatement)) {
                havingStatement = "HAVING " + havingStatement;
            }


            var query = $@"SELECT `orders`.`id`, `orders`.`create_date`, `orders`.`user_id`, `orders`.`has_extra_pack`, `orders`.`description`,
 `orders`.`is_processed_preorder`, `orders`.`is_processed_retail`, `orders`.`is_reviewed`, `users`.`fullname` AS `client_name`,
IFNULL(`managers`.`id`, 0) AS `manager_id`, IFNULL(`managers`.`name`, '') AS `manager_name`,
IFNULL(`cities`.`id`, 0) AS `city_id`, IFNULL(`cities`.`name`, '') AS `city_name`, {selectCartItemsField}
SUM(IF(`details`.`catalog_id` = {(int)CatalogType.Preorder}, `details`.`price` * `details`.`amount`, 0)) AS `preorder_total_sum`,
SUM(IF(`details`.`catalog_id` != {(int)CatalogType.Preorder}, `details`.`price` * `details`.`amount`, 0)) AS `retail_total_sum`,
SUM(IF(`details`.`catalog_id` = {(int)CatalogType.Preorder}, `details`.`price_in_rub` * `details`.`amount`, 0)) AS `preorder_total_sum_rub`,
SUM(IF(`details`.`catalog_id` != {(int)CatalogType.Preorder}, `details`.`price_in_rub` * `details`.`amount`, 0)) AS `retail_total_sum_rub`,
SUM(IF(`details`.`catalog_id` = {(int)CatalogType.Preorder}, `details`.`amount`, 0)) AS `preorder_amount`, 
SUM(IF(`details`.`catalog_id` != {(int)CatalogType.Preorder}, `details`.`amount`, 0)) AS `retail_amount`
FROM `for1c_orders` AS `orders`
JOIN `1c_clients` AS `users` ON `orders`.`user_id` = `users`.`id`
LEFT JOIN `1c_city` AS `cities` ON `users`.`city` = `cities`.`id` 
LEFT JOIN `1c_manager` AS `managers` ON `users`.`manager` = `managers`.`id`
LEFT JOIN `for1c_order_details` AS `details` ON `orders`.`id` = `details`.`order_id`
LEFT JOIN `1c_models` AS `models` ON `details`.`model_id` = `models`.`id`
{joinCartItems}
{whereStatement}
GROUP BY `orders`.`id`, `orders`.`create_date`, `orders`.`user_id`, `orders`.`is_processed_preorder`,
	`orders`.`is_processed_retail`, `orders`.`has_extra_pack`, `orders`.`description`, `users`.`fullname`,
    `managers`.`id`, `managers`.`name`, `cities`.`id`, `cities`.`name`
{havingStatement}
ORDER BY `orders`.`id` DESC;"
                .ToFormattable();

            var ordersList = await _context.Set<OrderAdmin>().FromSqlInterpolated(query).ToListAsync();
            return _mapper.Map<List<OrderAdminDTO>>(ordersList);
        }

        public async Task<OrderAdminDTO> GetOrderAdminAsync(int orderId) {
            var result = (await GetAllOrdersAsync(orderId)).FirstOrDefault();
            if (result != null) {
                result.Details = await _getOrderDetailsAsync(orderId);
            }

            return result;
        }

        public async Task<OrderDTO> GetOrderAsync(int userId, int orderId) {
            var order = await _context.Orders1C
                .Where(x => x.UserId == userId && x.Id == orderId)
                .Select(x => new OrderDTO {
                    Id = x.Id,
                    UserId = x.UserId,
                    UserLogin = x.UserLogin,
                    CreateDate = x.CreateDate,
                    Description = x.Description,
                    HasExtraPack = x.HasExtraPack,
                    IsProcessedPreorder = x.IsProcessedPreorder,
                    TotalPrice = x.Details.Sum(c => c.Price * c.Amount),
                    TotalPriceInRub = x.Details.Sum(c => c.PriceInRub * c.Amount),
                    TotalAmount = x.Details.Sum(c => c.Amount)
                })
                .FirstOrDefaultAsync();

            var result = _mapper.Map<OrderDTO>(order);
            result.Details = await _getOrderDetailsAsync(orderId);
            return result;
        }

        public async Task<OperationResult> DeletePositionAsync(int orderId, int id, bool needTransaction = true) {
            var orderPosition = await _context.Order1CDetails.Where(x => x.Id == id && x.OrderId == orderId).FirstOrDefaultAsync();
            if (orderPosition == null) {
                return OperationResult.Failure("Позиция не найдена в заказе.");
            }

            try {
                if (needTransaction) _context.Database.BeginTransaction();

                var sqlStr = string.Empty;
                var whereStr = string.Empty;
                var paramsList = new List<object>();

                if (orderPosition.CatalogId != CatalogType.Preorder) {
                    paramsList = new List<object> { orderPosition.StorehouseId, orderPosition.ModelId, orderPosition.ColorId, orderPosition.NomenclatureId };
                    whereStr = "WHERE `sklad` = {0} AND `model` = {1} AND `color` = {2} AND `nomenklatura` = {3}";
                    sqlStr = $@"UPDATE `1c_sklad` SET `kolichestvo` = `kolichestvo` + {orderPosition.Amount} {whereStr} LIMIT 1;";
                    await _context.Database.ExecuteSqlRawAsync(sqlStr, paramsList.ToArray());
                }

                paramsList = new List<object> { orderPosition.ModelId, orderPosition.ColorId, orderPosition.CatalogId };
                whereStr = "WHERE `model` = {0} AND `color` = {1} AND `razdel` = {2} ";
                if (orderPosition.CatalogId == CatalogType.InStockParts) {
                    whereStr += " AND `nomenklatura` = {3}";
                    paramsList.Add(orderPosition.NomenclatureId);
                }             

                sqlStr = $@"UPDATE `1c_catalog` SET `kolichestvo` = `kolichestvo` + {orderPosition.Amount} {whereStr} LIMIT 1;";
                await _context.Database.ExecuteSqlRawAsync(sqlStr, paramsList.ToArray());

                _context.Order1CDetails.Remove(orderPosition);
                await _context.SaveChangesAsync();

                if (needTransaction) _context.Database.CommitTransaction();

                return OperationResult.Success("Позиция успешно удалена из заказа.");
            } catch (Exception ex) {
                if (needTransaction) _context.Database.RollbackTransaction();

                _logger.Error(ex, "Failed to Delete Order Position (OrderId: {orderId}, Id: {id}). {message}", orderId, id, ex.Message);

                return OperationResult.Failure("Ошибка при удалении позиции из заказе в БД.");
            }
        }

        public async Task<OperationResult> DeleteOrderAsync(int id, string mode = "") {
            var order = await _context.Orders1C.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (order == null) {
                return OperationResult.Failure("Заказ не найден в БД.");
            }

            try {
                _context.Database.BeginTransaction();

                var orderPositions = await _context.Order1CDetails.Where(x => x.OrderId == id).ToListAsync();
                int posCount = orderPositions.Count;
                int deletedCount = 0;
                foreach (var pos in orderPositions) {
                    if ((mode == "preorder" && pos.CatalogId != CatalogType.Preorder) ||
                        (mode == "retail" && pos.CatalogId == CatalogType.Preorder)) continue;

                    var or = await DeletePositionAsync(id, pos.Id, false);
                    if (!or.IsSuccess) {
                        throw new Exception("Failed to delete order position");
                    }
                    deletedCount++;
                }

                if (deletedCount >= posCount) {
                    _context.Orders1C.Remove(order);
                }
                await _context.SaveChangesAsync();

                _context.Database.CommitTransaction();

                return OperationResult.Success("Заказ успешно удален.");
            } catch (Exception ex) {
                _context.Database.RollbackTransaction();

                _logger.Error(ex, "Failed to Delete Order (OrderId: {orderId}). {message}", id, ex.Message);

                return OperationResult.Failure("Ошибка при удалении заказа из БД.");
            }
        }

        private string _getDictKeyForOrderAdmin(OrderAdminDTO value) {
            return $"{value.UserId}_{value.IsProcessedPreorder}_{value.IsProcessedRetail}";
        }

        public async Task<OperationResult> CombineOrdersAsync(int[] ordersIDs) {
            var ordersList = await GetAllOrdersAsync(0, 0, 0, 0, ordersIDs);
            if (ordersList.Count == 0) {
                return OperationResult.Failure("Заказы не найдены в БД.");
            }

            var ordersDict = new Dictionary<string, List<int>>();
            foreach (var order in ordersList) {
                var oKey = _getDictKeyForOrderAdmin(order);
                if (!ordersDict.ContainsKey(oKey)) {
                    ordersDict.Add(oKey, new List<int>());
                }
                ordersDict[oKey].Add(order.Id);
            }
            foreach (var item in ordersDict) {
                item.Value.Sort();
            }

            try {
                _context.Database.BeginTransaction();

                foreach (var item in ordersDict) {
                    if (item.Value.Count == 1) continue;

                    for (int i = 1; i < item.Value.Count; i++) {
                        await _context.Database.ExecuteSqlRawAsync("UPDATE `for1c_order_details` SET `order_id`={0} WHERE `order_id`={1};", new object[] { item.Value[0], item.Value[i] });
                        await _context.Database.ExecuteSqlRawAsync("DELETE FROM `for1c_orders` WHERE `id`={0};", new object[] { item.Value[i] });
                    }
                }

                _context.Database.CommitTransaction();

                return OperationResult.Success("Заказы успешно объединены.");
            } catch (Exception ex) {
                _context.Database.RollbackTransaction();

                _logger.Error(ex, "Failed to Combine Orders. {message}", ex.Message);

                return OperationResult.Failure("Ошибка при объединении заказов в БД.");
            }
        }

        private async Task<CatalogItem1CAmount> _getCatalogItemAsync(int catalogId, int modelId, int colorId, int nomenclatureId = 0) {
            var sqlStr = string.Empty;
            if (catalogId == (int)CatalogType.InStockParts) {
                sqlStr = $@"
SELECT `catalog`.`id`, `catalog`.`model`, `catalog`.`color`, `catalog`.`razmer`, `catalog`.`nomenklatura`,
`catalog`.`kolichestvo`, `catalog`.`razdel`, `catalog`.`price`, `catalog`.`price_rub`
FROM `1c_catalog` AS `catalog` 
WHERE `catalog`.`razdel` = {catalogId} AND `catalog`.`model` = {modelId} AND `catalog`.`color` = {colorId};";
            } else {
                sqlStr = $@"
SELECT `catalog`.`id`, `catalog`.`model`, `catalog`.`color`, `mod`.`line` AS `razmer`, `catalog`.`nomenklatura`,
`catalog`.`kolichestvo`, `catalog`.`razdel`, `catalog`.`price`, `catalog`.`price_rub`
FROM `1c_catalog` AS `catalog` 
JOIN `1c_models` AS `mod` ON `catalog`.`model` = `mod`.`id`
WHERE `catalog`.`razdel` = {catalogId} AND `catalog`.`model` = {modelId} AND `catalog`.`color` = {colorId};";
            }
            var resultList = await _context.Set<CatalogItem1CAmount>().FromSqlRaw(sqlStr).ToListAsync();

            CatalogItem1CAmount result = null;

            if (resultList.Count == 0) return null;

            if (nomenclatureId > 0) {
                foreach (var item in resultList) {
                    if (item.NomenclatureId == nomenclatureId) {
                        result = item;
                        break;
                    }
                }
            } else {
                foreach (var item in resultList) {
                    if (item.NomenclatureId == 0) {
                        result = item;
                        break;
                    }
                }
            }

            return result;
        }

        private async Task<int> _getStorehouseRestsAsync(int storehouseId, int modelId, int colorId, int nomenclatureId) {
            var sqlStr = $@"
SELECT `rests`.`id`, `rests`.`model`, `rests`.`color`, `rests`.`kolichestvo`, `rests`.`sklad`, `rests`.`nomenklatura`, `rests`.`line`, 
`storehouse`.`name`, `storehouse`.`obsch`, `storehouse`.`mesto` 
FROM `1c_sklad` AS `rests` 
LEFT JOIN `1c_sklady` AS `storehouse` ON `rests`.`sklad` = `storehouse`.`id` 
WHERE `rests`.`sklad` = {storehouseId} AND `rests`.`model` = {modelId} AND `rests`.`color` = {colorId} AND `rests`.`nomenklatura` = {nomenclatureId} AND `rests`.`kolichestvo` > 0
ORDER BY `rests`.`kolichestvo` DESC 
LIMIT 1;";
            var result = await _context.Set<StorehouseRests>().FromSqlRaw(sqlStr).ToListAsync();

            if (result.Count == 0) {
                return 0;
            }

            return result[0].Amount;
        }

        public async Task<int> GetMaxItemAmountAsync(OrderDetailsDTO orderItem) {
            CatalogItem1CAmount catalogAmount = null;
            if (orderItem.CatalogId != (int)CatalogType.Preorder) {
                if (orderItem.CatalogId == (int)CatalogType.InStockParts) {
                    catalogAmount = await _getCatalogItemAsync(orderItem.CatalogId, orderItem.ModelId, orderItem.ColorId, orderItem.NomenclatureId);
                } else {
                    catalogAmount = await _getCatalogItemAsync(orderItem.CatalogId, orderItem.ModelId, orderItem.ColorId);
                }
            } else {
                catalogAmount = await _getCatalogItemAsync(orderItem.CatalogId, orderItem.ModelId, orderItem.ColorId);
            }

            if (catalogAmount == null) {
                return orderItem.Amount;
            } else {
                return orderItem.Amount + catalogAmount.Amount;
            }
        }

        public async Task<OperationResult> AddModelToOrderAsync(int orderId, int catalogId, int modelId, int colorId, int nomenclatureId, int amount) {
            try {
                var catalogItem = await _getCatalogItemAsync(catalogId, modelId, colorId, nomenclatureId);
                if (catalogItem == null) {
                    return OperationResult.Failure("Модель не найдена в каталоге.");
                }

                await _context.Database.BeginTransactionAsync();

                var orderDetails = new List<OrderDetails>();
                orderDetails.Add(
                    new OrderDetails() {
                        CatalogId = catalogId.ToProductCatalog1CId(),
                        ModelId = modelId,
                        ColorId = colorId,
                        Amount = amount,
                        NomenclatureId = catalogItem.NomenclatureId,
                        SizeValue = catalogItem.SizeValue,
                        Price = catalogItem.Price,
                        PriceInRub = catalogItem.PriceInRub
                    }
                );
                var entitiesOrderDetails = await _createOrderDetailsFromStorehouseRests(orderDetails);

                foreach (var orderDetail in entitiesOrderDetails) {
                    orderDetail.OrderId = orderId;
                }

                await _context.Order1CDetails.AddRangeAsync(entitiesOrderDetails);
                await _context.SaveChangesAsync();

                _context.Database.CommitTransaction();

                return OperationResult.Success("Модель успешно добавлена в заказ.");
            } catch (Exception ex) {
                _context.Database.RollbackTransaction();

                _logger.Error(ex, "Failed to Add Model to Order Position (Id: {id}). {message}", orderId, ex.Message);

                return OperationResult.Failure("Ошибка при добавлении модели в заказ в БД.");
            }
        }

        public async Task<OperationResult> UpdateOrderItemAmountAsync(OrderDetailsDTO orderItem, int linesCount) {
            var orderPosition = await _context.Order1CDetails.Where(x => x.Id == orderItem.Id).FirstOrDefaultAsync();
            if (orderPosition == null) {
                return OperationResult.Failure("Позиция не найдена в заказе.");
            }

            try {
                _context.Database.BeginTransaction();

                var sqlStr = string.Empty;
                var whereStr = string.Empty;
                var paramsList = new List<object>();

                var delta = linesCount * orderItem.PartsCount - orderItem.Amount;
                if (delta < 0) {
                    delta = -delta;
                    if (orderPosition.CatalogId != CatalogType.Preorder) {
                        paramsList = new List<object> { orderPosition.StorehouseId, orderPosition.ModelId, orderPosition.ColorId, orderPosition.NomenclatureId };
                        whereStr = "WHERE `sklad` = {0} AND `model` = {1} AND `color` = {2} AND `nomenklatura` = {3}";
                        sqlStr = $@"UPDATE `1c_sklad` SET `kolichestvo` = `kolichestvo` + {delta} {whereStr} LIMIT 1;";
                        await _context.Database.ExecuteSqlRawAsync(sqlStr, paramsList.ToArray());
                    }

                    paramsList = new List<object> { orderPosition.ModelId, orderPosition.ColorId, orderPosition.CatalogId };
                    whereStr = "WHERE `model` = {0} AND `color` = {1} AND `razdel` = {2} ";
                    if (orderPosition.CatalogId == CatalogType.InStockParts) {
                        whereStr += " AND `nomenklatura` = {3}";
                        paramsList.Add(orderPosition.NomenclatureId);
                    }

                    sqlStr = $@"UPDATE `1c_catalog` SET `kolichestvo` = `kolichestvo` + {delta} {whereStr} LIMIT 1;";
                    await _context.Database.ExecuteSqlRawAsync(sqlStr, paramsList.ToArray());
                } else {
                    var catalogAmount = await GetMaxItemAmountAsync(orderItem);
                    if (catalogAmount < linesCount * orderItem.PartsCount) {
                        _context.Database.RollbackTransaction();
                        return OperationResult.Failure("Недостаточное количество товара в каталоге.");
                    }

                    if (orderPosition.CatalogId != CatalogType.Preorder) {
                        int rest = await _getStorehouseRestsAsync(orderPosition.StorehouseId, orderPosition.ModelId, orderPosition.ColorId, orderPosition.NomenclatureId);
                        if (rest < delta) {
                            _context.Database.RollbackTransaction();
                            return OperationResult.Failure("Недостаточное количество товара на складе.");
                        }

                        paramsList = new List<object> { orderPosition.StorehouseId, orderPosition.ModelId, orderPosition.ColorId, orderPosition.NomenclatureId };
                        whereStr = "WHERE `sklad` = {0} AND `model` = {1} AND `color` = {2} AND `nomenklatura` = {3}";
                        sqlStr = $@"UPDATE `1c_sklad` SET `kolichestvo` = `kolichestvo` - {delta} {whereStr} LIMIT 1;";
                        await _context.Database.ExecuteSqlRawAsync(sqlStr, paramsList.ToArray());
                    }

                    paramsList = new List<object> { orderPosition.ModelId, orderPosition.ColorId, orderPosition.CatalogId };
                    whereStr = "WHERE `model` = {0} AND `color` = {1} AND `razdel` = {2} ";
                    if (orderPosition.CatalogId == CatalogType.InStockParts) {
                        whereStr += " AND `nomenklatura` = {3}";
                        paramsList.Add(orderPosition.NomenclatureId);
                    }

                    sqlStr = $@"UPDATE `1c_catalog` SET `kolichestvo` = `kolichestvo` - {delta} {whereStr} LIMIT 1;";
                    await _context.Database.ExecuteSqlRawAsync(sqlStr, paramsList.ToArray());
                }

                orderPosition.Amount = linesCount * orderItem.PartsCount;

                _context.Order1CDetails.Update(orderPosition);
                await _context.SaveChangesAsync();

                _context.Database.CommitTransaction();

                return OperationResult.Success("Позиция успешно обновлена в заказе.");
            } catch (Exception ex) {
                _context.Database.RollbackTransaction();

                _logger.Error(ex, "Failed to Update Amount in Order Position (Id: {id}). {message}", orderItem.Id, ex.Message);

                return OperationResult.Failure("Ошибка при обновлении позиции заказа в БД.");
            }
        }

        private async Task<List<OrderDetailsDTO>> _getOrderDetailsAsync(int orderId) {
            var orderItems = await _context.OrderDetailsFull.FromSqlInterpolated($@"SELECT 
`details`.`id`, `details`.`order_id`, `details`.`catalog_id`, `details`.`model_id`, `details`.`nomenclature_id`, `details`.`color_id`,
`details`.`size_value`, `details`.`price`, `details`.`price_in_rub`, `details`.`amount`,
`models`.`articul`, `models`.`razmerov` as `parts_count`, `catalogs`.`name` AS `catalog_name`, 
`colors`.`name` AS `color_name`, `matcolors`.`photo` AS `color_photo`,
`colors_group`.`name` AS `color_group_name`, `colors_group`.`rgb` AS `color_group_rgb_value`, 
`model_photo`.`photo_path` AS `photo_by_color`,
`catalog_item`.`main_photo`,
ifnull(`collection`.`id`, 0) AS `collection_id`, ifnull(`collection`.`name`, 'Коллекция №1') AS `collection_name`,
if(`collection`.`procent` IS NULL OR `collection`.`procent` = 0, {GlobalConstant.GeneralPrepayPercentValue}, `collection`.`procent`) AS `collection_percent`
FROM `for1c_order_details` AS `details`
JOIN `1c_models` AS `models` ON `details`.`model_id` = `models`.`id`
LEFT OUTER JOIN `1c_material` AS `materials` ON `models`.`material` = `materials`.`id`
JOIN `1c_colors` AS `colors` ON `details`.`color_id` = `colors`.`id`
LEFT OUTER JOIN `1c_materialcolors` AS `matcolors` ON `matcolors`.`color` = `details`.`color_id` AND `matcolors`.`material` = `models`.`material`
LEFT OUTER JOIN `1c_colors_group` AS `colors_group` ON `colors`.`group` = `colors_group`.`id` 
LEFT JOIN `dict_catalogs` AS `catalogs` ON `details`.`catalog_id` = `catalogs`.`catalog_id_1c` 
LEFT JOIN `catalog_item_descriptor` AS `catalog_item` ON `models`.`articul` = `catalog_item`.`articul` 
LEFT JOIN (SELECT ti.`articul`, ti.`photo_path`, ti.`color_id` FROM `model_photos_1c` AS ti WHERE ti.`color_id` IS NOT NULL  GROUP BY ti.`articul`, ti.`color_id`) AS `model_photo` ON `models`.`articul` = `model_photo`.`articul` AND `details`.`color_id` = `model_photo`.`color_id`
LEFT JOIN `1c_collection` AS `collection` ON `models`.`collection` = `collection`.`id` 
WHERE `details`.`order_id` = {orderId}
ORDER BY `details`.`catalog_id`, `models`.`articul`, `color_name`;").ToListAsync();

            return _mapper.Map<List<OrderDetailsDTO>>(orderItems);
        }

        public async Task<List<OrderDetailsDTO>> GetPreorderTotalReportAsync() {
            var resultList = new List<OrderDetailsDTO>();

            var orderItems = await _context.OrderDetailsFull.FromSqlInterpolated($@"SELECT 
`details`.`id`, `details`.`order_id`, `details`.`catalog_id`, `details`.`model_id`, `details`.`nomenclature_id`, `details`.`color_id`,
`details`.`size_value`, AVG(`details`.`price`) AS `price`, AVG(`details`.`price_in_rub`) AS `price_in_rub`, SUM(`details`.`amount`) AS `amount`,
`models`.`articul`, `models`.`razmerov` as `parts_count`, `catalogs`.`name` AS `catalog_name`, 
`colors`.`name` AS `color_name`, `matcolors`.`photo` AS `color_photo`,
`colors_group`.`name` AS `color_group_name`, `colors_group`.`rgb` AS `color_group_rgb_value`, 
`model_photo`.`photo_path` AS `photo_by_color`,
`catalog_item`.`main_photo`
FROM `for1c_order_details` AS `details`
JOIN `1c_models` AS `models` ON `details`.`model_id` = `models`.`id`
LEFT OUTER JOIN `1c_material` AS `materials` ON `models`.`material` = `materials`.`id`
JOIN `1c_colors` AS `colors` ON `details`.`color_id` = `colors`.`id`
LEFT OUTER JOIN `1c_materialcolors` AS `matcolors` ON `matcolors`.`color` = `details`.`color_id` AND `matcolors`.`material` = `models`.`material`
LEFT OUTER JOIN `1c_colors_group` AS `colors_group` ON `colors`.`group` = `colors_group`.`id` 
LEFT JOIN `dict_catalogs` AS `catalogs` ON `details`.`catalog_id` = `catalogs`.`catalog_id_1c` 
LEFT JOIN `catalog_item_descriptor` AS `catalog_item` ON `models`.`articul` = `catalog_item`.`articul` 
LEFT JOIN (SELECT ti.`articul`, ti.`photo_path`, ti.`color_id` FROM `model_photos_1c` AS ti WHERE ti.`color_id` IS NOT NULL  GROUP BY ti.`articul`, ti.`color_id`) AS `model_photo` ON `models`.`articul` = `model_photo`.`articul` AND `details`.`color_id` = `model_photo`.`color_id` 
WHERE `details`.`order_id` IN (SELECT ti.`id` FROM `for1c_orders` AS ti WHERE ti.`is_processed_preorder` = 0) AND `details`.`catalog_id` = {(int)CatalogType.Preorder} 
GROUP BY `details`.`catalog_id`, `details`.`model_id`, `details`.`color_id` 
ORDER BY `details`.`catalog_id`, `models`.`articul`, `details`.`size_value`, `color_name`;").ToListAsync();

            return _mapper.Map<List<OrderDetailsDTO>>(orderItems);
        }

        public async Task<List<string>> GetAllSizesValuesFromOrdersAsync() {
            var sqlStr = @"SELECT DISTINCT t1.`size_value` AS `value` FROM `for1c_order_details` AS t1;";

            var sizesList = await _context.Set<Scalar>().FromSqlRaw(sqlStr).ToListAsync();

            return sizesList.Select(i => i.Value).ToList();
        }

        public async Task<List<Color1CDTO>> GetAllColorsValuesFromOrdersAsync() {
            var sqlStr = @"SELECT DISTINCT t1.`color_id` AS `id`, t2.`name`, 0 AS `group` 
FROM `for1c_order_details` AS t1
JOIN `1c_colors` AS t2 ON t1.`color_id` = t2.`id`
ORDER BY t2.`name`;";

            var colorsList = await _context.Set<Color>().FromSqlRaw(sqlStr).ToListAsync();

            return _mapper.Map<List<Color1CDTO>>(colorsList);
        }

        public async Task<List<OrderDetailsDTO>> GetOrdersTotalReportAsync(OrdersTotalFilterDTO filterValues) {
            var resultList = new List<OrderDetailsDTO>();

            var ordersWhereList = new List<string>();
            var ordersWhere = string.Empty;
            if (filterValues.OrderDateFrom != null) {
                ordersWhereList.Add($"ti.`create_date` > '{((DateTime)filterValues.OrderDateFrom).ToString("yyyy-MM-dd HH:mm")}'");
            }
            if (filterValues.OrderDateTo != null) {
                ordersWhereList.Add($"ti.`create_date` <= '{((DateTime)filterValues.OrderDateTo).ToString("yyyy-MM-dd HH:mm")}'");
            }
            if (filterValues.ProcessedOnly) {
                if (filterValues.PreorderOnly) {
                    ordersWhereList.Add($"ti.`is_processed_preorder` = 1");
                } else if (filterValues.InStockOnly) {
                    ordersWhereList.Add($"ti.`is_processed_retail` = 1");
                } else {
                    ordersWhereList.Add($"(ti.`is_processed_preorder` = 1 OR ti.`is_processed_retail` = 1)");
                }
            }
            if (filterValues.UnProcessedOnly) {
                if (filterValues.PreorderOnly) {
                    ordersWhereList.Add($"ti.`is_processed_preorder` = 0");
                } else if (filterValues.InStockOnly) {
                    ordersWhereList.Add($"ti.`is_processed_retail` = 0");
                } else {
                    ordersWhereList.Add($"(ti.`is_processed_preorder` = 0 AND ti.`is_processed_retail` = 0)");
                }
                
            }
            if (ordersWhereList.Count > 0) {
                ordersWhere = $"WHERE {string.Join(" AND ", ordersWhereList)}";
            }
            var ordersSelect = $"SELECT ti.`id` FROM `for1c_orders` AS ti {ordersWhere}";

            var paramsList = new List<object>();
            ordersWhereList.Clear();

            int paramsIndex = 0;
            if (!string.IsNullOrEmpty(filterValues.Articul)) {
                if (!string.IsNullOrEmpty(filterValues.Articul.Trim())) {
                    paramsList.Add("%" + filterValues.Articul.Trim() + "%");
                    ordersWhereList.Add($"`models`.`articul` LIKE {{{paramsIndex}}}");
                    paramsIndex++;
                }
            }
            if (filterValues.ColorId.HasValue && filterValues.ColorId > 0) {
                paramsList.Add((int)filterValues.ColorId);
                ordersWhereList.Add($"`details`.`color_id` = {{{paramsIndex}}}");
                paramsIndex++;
            }
            if (!string.IsNullOrEmpty(filterValues.SizeValue)) {
                paramsList.Add(filterValues.SizeValue);
                ordersWhereList.Add($"`details`.`size_value` = {{{paramsIndex}}}");
                paramsIndex++;
            }
            if (filterValues.InStockOnly) {
                paramsList.Add((int)CatalogType.Preorder);
                ordersWhereList.Add($"`details`.`catalog_id` <> {{{paramsIndex}}}");
                paramsIndex++;
            }
            if (filterValues.PreorderOnly) {
                paramsList.Add((int)CatalogType.Preorder);
                ordersWhereList.Add($"`details`.`catalog_id` = {{{paramsIndex}}}");
                paramsIndex++;
            }

            var outWhereStr = string.Join(" AND ", ordersWhereList);
            if (!string.IsNullOrEmpty(outWhereStr)) {
                outWhereStr = $" AND {outWhereStr}";
            }


            var sqlStr = $@"SELECT 
`details`.`id`, `details`.`order_id`, `details`.`catalog_id`, `details`.`model_id`, `details`.`nomenclature_id`, `details`.`color_id`,
`details`.`size_value`, SUM(`details`.`price` * `details`.`amount`) / SUM(`details`.`amount`) AS `price`, AVG(`details`.`price_in_rub`) AS `price_in_rub`, SUM(`details`.`amount`) AS `amount`,
`models`.`articul`, `models`.`razmerov` as `parts_count`, `catalogs`.`name` AS `catalog_name`, 
`colors`.`name` AS `color_name`, `matcolors`.`photo` AS `color_photo`,
`colors_group`.`name` AS `color_group_name`, `colors_group`.`rgb` AS `color_group_rgb_value`, 
`model_photo`.`photo_path` AS `photo_by_color`,
`catalog_item`.`main_photo`,
ifnull(`collection`.`id`, 0) AS `collection_id`, ifnull(`collection`.`name`, 'Коллекция №1') AS `collection_name`,
ifnull(`collection`.`procent`, {GlobalConstant.GeneralPrepayPercentValue}) AS `collection_percent`
FROM `for1c_order_details` AS `details`
JOIN `1c_models` AS `models` ON `details`.`model_id` = `models`.`id`
LEFT OUTER JOIN `1c_material` AS `materials` ON `models`.`material` = `materials`.`id`
JOIN `1c_colors` AS `colors` ON `details`.`color_id` = `colors`.`id`
LEFT OUTER JOIN `1c_materialcolors` AS `matcolors` ON `matcolors`.`color` = `details`.`color_id` AND `matcolors`.`material` = `models`.`material`
LEFT OUTER JOIN `1c_colors_group` AS `colors_group` ON `colors`.`group` = `colors_group`.`id` 
LEFT JOIN `dict_catalogs` AS `catalogs` ON `details`.`catalog_id` = `catalogs`.`catalog_id_1c` 
LEFT JOIN `catalog_item_descriptor` AS `catalog_item` ON `models`.`articul` = `catalog_item`.`articul` 
LEFT JOIN (SELECT ti.`articul`, ti.`photo_path`, ti.`color_id` FROM `model_photos_1c` AS ti WHERE ti.`color_id` IS NOT NULL  GROUP BY ti.`articul`, ti.`color_id`) AS `model_photo` ON `models`.`articul` = `model_photo`.`articul` AND `details`.`color_id` = `model_photo`.`color_id`
LEFT JOIN `1c_collection` AS `collection` ON `models`.`collection` = `collection`.`id` 
WHERE `details`.`order_id` IN ({ordersSelect}) {outWhereStr} 
GROUP BY `details`.`model_id`, `details`.`color_id`, `details`.`nomenclature_id` 
ORDER BY `models`.`articul`, `details`.`size_value`, `color_name`;";

            var orderItems = await _context.OrderDetailsFull.FromSqlRaw(sqlStr, paramsList.ToArray()).ToListAsync();

            return _mapper.Map<List<OrderDetailsDTO>>(orderItems);
        }

        private async Task _sendEmailsAsync(CreateOrderDTO order, int orderNumber, UserSession user) {
            await _sendEmailToManagerAsync(orderNumber, user, order);
            await _sendEmailToClientAsync(orderNumber, user, order.CartItems);
        }

        private async Task _sendEmailToManagerAsync(int orderNumber, UserSession user, CreateOrderDTO order) {
            try {
                var managers = await _uow.ManagerDetails.All.Include(x => x.Manager).Include(x => x.Accesses)
                  .Where(x => x.ManagerId == user.ManagerId || (x.SendNewOrderNotification == true && x.Accesses.Any(a => a.AccessToManagerId == user.ManagerId)))
                  .GroupBy(x => x.NotificationsEmail, x => new { x.Manager.Name }).Select(x => new { Email = x.Key, Name = x.Min(x => x.Name) }).ToListAsync();

                if (!managers.Any()) {
                    managers.Add(new { Email = _globalSettings.DefaultManagerEmail, Name = "Менеджер" });
                }

                var subject = $"Заказ №{orderNumber} от пользователя {user.User.FullName}";
                var content = await _makeCartItemHtmlAsync(order.CartItems, orderNumber) + $"<br/><br/>Комментарий к заказу: {order.Description}<br/><br/>Телефон заказчика: <a href=\"https://api.whatsapp.com/send?phone={user.User.Phone}\">{user.User.Phone}</a><br/>Email: <b><a href=\"mailto:{user.User.Email}\">{user.User.Email}</a></b>";
                
                foreach (var manager in managers) {
                    await _emailService.SendEmailAsync(new EmailMessage(manager.Email, subject, content), manager.Name);
                }
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to send order email to manager. {message}", ex.Message);
            }
        }

        private async Task _sendEmailToClientAsync(int orderNumber, UserSession user, List<CartItem1CDTO> cartItems) {
            try {
                if (string.IsNullOrEmpty(user.User.Email)) {
                    return;
                }

                var content = await _makeCartItemHtmlAsync(cartItems, orderNumber);
                await _emailService.SendEmailAsync(new EmailMessage(user.User.Email, $"Заказ №{orderNumber} оформлен", content), user.User.FullName);
            } catch (Exception ex) {
                _logger.Error(ex, "Failed to send order email to client. {message}", ex.Message);
            }
        }

        private async Task<string> _makeCartItemHtmlAsync(List<CartItem1CDTO> cartItems, int orderNumber) {
            var emailTemplateBuffer = _env.IsDevelopment() ?
                await _fileService.GetFileAsync(Path.Combine(_env.ContentRootPath, "..", "Module.Common.WebUI", "wwwroot"), "/email-order-template.html")
                : await _fileService.GetFileAsync(_env.WebRootPath, "/common/email-order-template.html");
            string template;
            await using (var stream = new MemoryStream(emailTemplateBuffer)) {
                using var reader = new StreamReader(stream); 
                template = await reader.ReadToEndAsync();
            }
            
            var body = new StringBuilder();
            const string bgColor = "bgcolor='#f8f9fa'";
            var cartItemsGrouped = cartItems.GroupBy(x => x.IsPreorder);
            foreach (var cartItemsGroup in cartItemsGrouped) {
                body.Append($@"<tr style='color: white;'>
<td colspan='4' valign='center' bgcolor='black' style='padding: 7px 20px;'>{(cartItemsGroup.Key ? "Предзаказ" : "Наличие")}</td>
<td colspan='5' nowrap valign='center' align='right' bgcolor='black' style='padding: 7px 20px;'>На сумму {cartItemsGroup.Sum(x => x.TotalPrice).ToTwoDecimalPlaces()} $</td>
</tr>");

                var cartItemsList = cartItemsGroup.ToList();
                for (var i = 0; i < cartItemsList.Count; i++) {
                    body.Append($@"
<tr {(i % 2 == 0 ? bgColor : "")}>
    <td align='center' valign='center' style='padding: 5px 0;'><img src='{_urlSettings.KristaShopUrl}{cartItemsList[i].Image}?width=100' alt='Фото {cartItemsList[i].Articul}'></td>
    <td align='center' valign='center'>{cartItemsList[i].Articul}</td>
    <td align='center' valign='center'>{cartItemsList[i].ColorName}</td>
    <td align='center' valign='center'>{cartItemsList[i].SizeValue}</td>
    <td align='center' valign='center'>{cartItemsList[i].Amount / cartItemsList[i].PartsCount}</td>
    <td align='center' valign='center'>{cartItemsList[i].Amount}</td>
    <td nowrap align='center' valign='center'>{cartItemsList[i].Price.ToTwoDecimalPlaces()} $</td>
    <td nowrap align='center' valign='center'>{((double)cartItemsList[i].Amount / cartItemsList[i].PartsCount * cartItemsList[i].Price).ToTwoDecimalPlaces()} $</td>
    <td nowrap align='center' valign='center'>{cartItemsList[i].TotalPrice.ToTwoDecimalPlaces()} $</td>
</tr>");
                }
            }

            body.Append($@"<tr style='color: white;'>
<td colspan='4' valign='center' bgcolor='black' style='padding: 7px 20px;'>Итого</td>
<td colspan='5' nowrap valign='center' align='right' bgcolor='black' style='padding: 7px 20px;'><div>Единиц {cartItems.Sum(x => x.Amount)}</div><div>На сумму {cartItems.Sum(x => x.TotalPrice).ToTwoDecimalPlaces()} $</div></td>
</tr>");

            var result = string.Format(template, orderNumber, body);
            return result;
        }

        public async Task<List<RequestAdminDTO>> getAllRequestsAsync() {
            var sqlStr = @"
SELECT t1.`klient` AS `user_id`, SUM(t1.`kolichestvo`) AS `tot_amount`, SUM(t1.`cena` * t1.`kolichestvo`) AS `tot_price`, SUM(t1.`cena_rub` * t1.`kolichestvo`) AS `tot_price_rub`,
t2.`fullname` AS `user_name`, IFNULL(t3.`name`, '') AS `city_name`, IFNULL(t4.`id`, 0) AS `manager_id`, IFNULL(t4.`name`, '---') AS `manager_name`, t1.`datav` AS `request_date`
FROM `1c_zayavka_klientov` AS t1 
JOIN `1c_clients` AS t2 ON t1.`klient` = t2.`id` 
LEFT JOIN `1c_city` AS t3 ON t2.`city` = t3.`id`
LEFT JOIN `1c_manager` AS t4 ON t2.`manager` = t4.`id`
GROUP BY t1.`klient`
ORDER BY `user_name`;";

            return _mapper.Map<List<RequestAdminDTO>>(await _context.Set<RequestAdmin>().FromSqlRaw(sqlStr).ToListAsync());
        }

        public async Task<List<ManufactureAdminDTO>> getAllManufactureAsync() {
            var sqlStr = @"
SELECT t1.`klient` AS `user_id`, SUM((t1.`kroitsya` + t1.`gotovkroy` + t1.`zapusk` + t1.`vposhive` + t1.`skladgp`)) AS `tot_amount`, 
SUM(t1.`cena` * (t1.`kroitsya` + t1.`gotovkroy` + t1.`zapusk` + t1.`vposhive` + t1.`skladgp`)) AS `tot_price`, 
SUM(t1.`cena_rub` * (t1.`kroitsya` + t1.`gotovkroy` + t1.`zapusk` + t1.`vposhive` + t1.`skladgp`)) AS `tot_price_rub`,
t2.`fullname` AS `user_name`, IFNULL(t3.`name`, '') AS `city_name`, IFNULL(t4.`id`, 0) AS `manager_id`, IFNULL(t4.`name`, '---') AS `manager_name` 
FROM `1c_klient_proizvodstvo` AS t1 
JOIN `1c_clients` AS t2 ON t1.`klient` = t2.`id` 
LEFT JOIN `1c_city` AS t3 ON t2.`city` = t3.`id`
LEFT JOIN `1c_manager` AS t4 ON t2.`manager` = t4.`id`
GROUP BY t1.`klient`
ORDER BY `user_name`;";

            return _mapper.Map<List<ManufactureAdminDTO>>(await _context.Set<ManufactureAdmin>().FromSqlRaw(sqlStr).ToListAsync());
        }

        public async Task<List<ReservationAdminDTO>> getAllReservationsAsync() {
            var sqlStr = @"
SELECT t1.`klient` AS `user_id`, SUM(t1.`kolichestvo`) AS `tot_amount`, SUM(t1.`cena` * t1.`kolichestvo`) AS `tot_price`, SUM(t1.`cena_rub` * t1.`kolichestvo`) AS `tot_price_rub`, MIN(t1.`datav`) AS `first_item_date`, 
t2.`fullname` AS `user_name`, IFNULL(t3.`name`, '') AS `city_name`, IFNULL(t4.`id`, 0) AS `manager_id`, IFNULL(t4.`name`, '---') AS `manager_name` 
FROM `1c_rezervy_klientov` AS t1 
JOIN `1c_clients` AS t2 ON t1.`klient` = t2.`id` 
LEFT JOIN `1c_city` AS t3 ON t2.`city` = t3.`id`
LEFT JOIN `1c_manager` AS t4 ON t2.`manager` = t4.`id`
GROUP BY t1.`klient`
ORDER BY `user_name`;";

            return _mapper.Map<List<ReservationAdminDTO>>(await _context.Set<ReservationAdmin>().FromSqlRaw(sqlStr).ToListAsync());
        }

        public async Task<List<ShipingAdminDTO>> getAllShipingsAsync() {
            var sqlStr = @"
SELECT t1.`klient` AS `user_id`, SUM(t1.`kolichestvo`) AS `tot_amount`, SUM(t1.`cena` * t1.`kolichestvo`) AS `tot_price`, SUM(t1.`cena_rub` * t1.`kolichestvo`) AS `tot_price_rub`, 
t2.`fullname` AS `user_name`, IFNULL(t3.`name`, '') AS `city_name`, IFNULL(t4.`id`, 0) AS `manager_id`, IFNULL(t4.`name`, '---') AS `manager_name` 
FROM `1c_prodagi_klientov` AS t1 
JOIN `1c_clients` AS t2 ON t1.`klient` = t2.`id` 
LEFT JOIN `1c_city` AS t3 ON t2.`city` = t3.`id`
LEFT JOIN `1c_manager` AS t4 ON t2.`manager` = t4.`id`
GROUP BY t1.`klient`
ORDER BY `user_name`;";

            return _mapper.Map<List<ShipingAdminDTO>>(await _context.Set<ShipingAdmin>().FromSqlRaw(sqlStr).ToListAsync());
        }

        public async Task<(double pCount, double pSum, double sCount, double sSum, int oCount)> getUnprocessedOrdersStatsAsync(int? managerId = null) {
            var whereStatement = "";
            if (managerId.HasValue) {
                var managerIds = await _uow.ManagerAccess.GetManagerIdsAccessesFor(managerId.Value, ManagerAccessToType.Orders).ToListAsync();
                if (managerIds.Any()) {
                    whereStatement = @$" WHERE `users`.`manager` IN ('{string.Join("' ,'", managerIds)}')";
                }
            }
           
            var sqlStr = @$"
SELECT COUNT(tin.`order_id`) AS `orders_count`, 
IFNULL(SUM(`tot_sum_preorder`), 0) AS `tot_sum_preorder`, IFNULL(SUM(`tot_sum_retail`), 0) AS `tot_sum_retail`, 
IFNULL(SUM(`tot_amount_preorder`), 0) AS `tot_amount_preorder`, IFNULL(SUM(`tot_amount_retail`), 0) AS `tot_amount_retail` 
FROM 
(
	SELECT t1.`order_id`, t2.`is_processed_preorder`, t2.`is_processed_retail`, 
	SUM(IF(t1.`catalog_id` = {{0}}, t1.`amount`, 0)) AS `tot_amount_preorder`, SUM(IF(t1.`catalog_id` <> {{0}}, t1.`amount`, 0)) AS `tot_amount_retail`,  
	SUM(IF(t1.`catalog_id` = {{0}}, t1.`amount` * t1.`price`, 0)) AS `tot_sum_preorder`, SUM(IF(t1.`catalog_id` <> {{0}}, t1.`amount` * t1.`price`, 0)) AS `tot_sum_retail` 
	FROM `for1c_order_details` AS t1
	JOIN `for1c_orders` AS t2 ON t1.`order_id` = t2.`id`
    JOIN `1c_clients` AS `users` ON t2.`user_id` = `users`.`id`
    {whereStatement}
	GROUP BY t1.`order_id`
) AS tin
WHERE (tin.`tot_amount_preorder` > 0 AND tin.`is_processed_preorder` = 0) OR (tin.`tot_amount_retail` > 0 AND tin.`is_processed_retail` = 0)";

            var totOrdersStats = await _context.Set<OrderStats>().FromSqlRaw(sqlStr, new object[] { (int)CatalogType.Preorder }).FirstOrDefaultAsync();

     

            return (
                pCount: totOrdersStats?.TotAmountPreorder ?? 0,
                pSum: totOrdersStats?.TotSumPreorder ?? 0.0d,
                sCount: totOrdersStats?.TotAmountRetail ?? 0,
                sSum: totOrdersStats?.TotSumRetail ?? 0.0d,
                oCount: totOrdersStats?.OrdersCount ?? 0
            );
        }

        public async Task<List<InvoiceDTO>> GetAllInvoicesListAsync() {
            var sqlStr = _getBaseInvoiceSqlStr("");

            var invoicesList = await _context.Set<InvoiceSql>().FromSqlRaw(sqlStr).ToListAsync();

            return _mapper.Map<List<InvoiceDTO>>(invoicesList);
        }

        public async Task<List<InvoiceDTO>> GetInvoicesListByUserIdAsync(int userId, bool includePayedInvoices = true) {
            var sqlStr = _getBaseInvoiceSqlStr(includePayedInvoices ? $"t1.`klient` = {userId}" : $"t1.`klient` = {userId} AND t1.`oplachen` = 0");

            var invoicesList = await _context.Set<InvoiceSql>().FromSqlRaw(sqlStr).ToListAsync();

            var resultList = _mapper.Map<List<InvoiceDTO>>(invoicesList);
            foreach (var item in resultList) {
                item.setBaseDirectory(_globalSettings.ClientInvoicesFilesPath);
            }
            return resultList;
        }

        public async Task<InvoiceDTO> GetInvoiceByIdAsync(int id) {
            var sqlStr = _getBaseInvoiceSqlStr($"t1.`id` = {id}");
            var invoice = await _context.Set<InvoiceSql>().FromSqlRaw(sqlStr.TrimEnd(';')).FirstOrDefaultAsync();

            if (invoice == null) return null;

            var result = _mapper.Map<InvoiceDTO>(invoice);

            result.Lines = await _getInvoiceLinesAsync(id);
            foreach (var line in result.Lines) {
                line.setParentInvoice(result);
            }

            result.setBaseDirectory(_globalSettings.ClientInvoicesFilesPath);

            return result;
        }

        private string _getBaseInvoiceSqlStr(string whereStr = "") {

            if (!string.IsNullOrEmpty(whereStr)) {
                whereStr = $"WHERE {whereStr} ";
            }

            var sqlStr = $@"
SELECT t1.`id`, t1.`klient`, t1.`pklient`, t1.`datadok`, t1.`nomerdok`, CAST(t1.`ispavans` AS double) AS `ispavans`, CAST(t1.`koplate` AS double) AS `koplate`, t1.`val`, t1.`kurs_rub`, t1.`oplachen`,  
t2.`fullname` AS `user_name`, IFNULL(t3.`name`, '') AS `city_name`, IFNULL(t4.`id`, 0) AS `manager_id`, IFNULL(t4.`name`, '---') AS `manager_name`, 
IFNULL(tpp.`is_prepay`, 0) AS `is_prepay` 
FROM `1c_scheta_klientov_dok` AS t1 
JOIN `1c_clients` AS t2 ON t1.`klient` = t2.`id` 
LEFT JOIN `1c_city` AS t3 ON t2.`city` = t3.`id`
LEFT JOIN `1c_manager` AS t4 ON t2.`manager` = t4.`id` 
LEFT JOIN (SELECT tin.`id_dok` AS `invoice_id`, MAX(tin.`flstr`) AS `is_prepay` FROM `1c_scheta_klientov` AS tin GROUP BY tin.`id_dok`) AS tpp ON tpp.`invoice_id` = t1.`id` 
{whereStr} 
ORDER BY t1.`datadok` DESC, t1.`id` DESC;";

            return sqlStr;
        }

        private async Task<List<InvoiceLineDTO>> _getInvoiceLinesAsync(int invoiceId) {

            var sqlStr = @"
SELECT t1.`id`, t1.`id_dok`, t1.`klient`, IFNULL(t1.`model`, 0) AS `model`, IFNULL(t1.`color`, 0) AS `color`, TRIM(IFNULL(t1.`razmer`, '')) AS `razmer`, t1.`osnovanie`, t1.`flstr`, t1.`kolichestvo`, t1.`cena`, 
`models`.`articul`, IFNULL(`models`.`razmerov`, 0) as `parts_count`,  `models`.`line` AS `size_line`,  
`colors`.`name` AS `color_name`, `matcolors`.`photo` AS `color_photo`,
`colors_group`.`name` AS `color_group_name`, `colors_group`.`rgb` AS `color_group_rgb_value`, 
`model_photo`.`photo_path` AS `photo_by_color`,
`catalog_item`.`main_photo`
FROM `1c_scheta_klientov` AS t1
LEFT OUTER JOIN `1c_models` AS `models` ON t1.`model` = `models`.`id`
LEFT OUTER JOIN `1c_material` AS `materials` ON `models`.`material` = `materials`.`id`
LEFT OUTER JOIN `1c_colors` AS `colors` ON t1.`color` = `colors`.`id`
LEFT OUTER JOIN `1c_materialcolors` AS `matcolors` ON `matcolors`.`color` = t1.`color` AND `matcolors`.`material` = `models`.`material`
LEFT OUTER JOIN `1c_colors_group` AS `colors_group` ON `colors`.`group` = `colors_group`.`id` 
LEFT JOIN `catalog_item_descriptor` AS `catalog_item` ON `models`.`articul` = `catalog_item`.`articul` 
LEFT OUTER JOIN (SELECT ti.`articul`, ti.`photo_path`, ti.`color_id` FROM `model_photos_1c` AS ti WHERE ti.`color_id` IS NOT NULL  GROUP BY ti.`articul`, ti.`color_id`) AS `model_photo` ON `models`.`articul` = `model_photo`.`articul` AND t1.`color` = `model_photo`.`color_id` 
WHERE t1.`id_dok` = {0}
ORDER BY t1.`id`;";

            var invoicesLines = await _context.Set<InvoiceLineSql>().FromSqlRaw(sqlStr, new object[] { invoiceId }).ToListAsync();

            return _mapper.Map<List<InvoiceLineDTO>>(invoicesLines);
        }

        public async Task<bool> CheckAsReviewedAsync(int orderId, UserSession user) {
            var managersCondition = "";
            if(user.IsManager && !user.IsRoot) {
                var managerIds = await _uow.ManagerAccess.GetManagerIdsAccessesFor(user.ManagerId, ManagerAccessToType.Orders).ToListAsync();
                managersCondition = $"AND `client`.`manager` in ({string.Join(", ", managerIds)})";
            }

            var result = await _context.Database.ExecuteSqlRawAsync(@$"UPDATE `for1c_orders` AS `order`
LEFT JOIN `1c_clients` AS `client` ON `order`.`user_id` = `client`.`id`
SET `is_reviewed` = 1 
WHERE `order`.`id` = {{0}} {managersCondition}", orderId);

            return result > 0;
        }
    }
}