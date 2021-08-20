using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Structs;
using KristaShop.Common.Models.Filters;
using Module.Common.Business.Models;
using Module.Order.Business.Interfaces;
using Module.Order.Business.Models;
using Module.Order.Business.Models.Adapters;
using Module.Order.Business.UnitOfWork;

namespace Module.Order.Business.Services {
    public class ClientReportService : IClientReportService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ClientReportService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ItemsGrouped<ItemsGroupedWithDate<RequestsItemDTO>>> GetGroupedUserRequestItemsAsync(int userId) {
            var requestsItems = _mapper.Map<List<RequestsItemDTO>>(await _uow.RequestItems.GetUserItemsAsync(userId));

            var resultItems = requestsItems
                .GroupBy(
                    x => x.CreateDate.ToBasicString(),
                    (_, groupedItems) => {
                        var items = groupedItems.ToList();
                        return new ItemsGroupedWithDate<RequestsItemDTO> {
                            CreateDate = items.First().CreateDate,
                            Totals = new ReportTotalInfo(items.Sum(c => c.Amount),
                                items.Sum(c => c.TotalPrice),
                                items.Sum(c => c.TotalPriceInRub)),
                            Items = items
                                .OrderBy(o => o.Articul)
                                .ThenBy(o => o.Size.Value)
                                .ThenBy(o => o.ColorName)
                                .ToList()
                        };
                    })
                .OrderByDescending(o => o.CreateDate)
                .ToList();

            return new ItemsGrouped<ItemsGroupedWithDate<RequestsItemDTO>> {
                Items = resultItems,
                Totals = new ReportTotalInfo(requestsItems.Sum(x => x.Amount),
                    requestsItems.Sum(x => x.TotalPrice),
                    requestsItems.Sum(x => x.TotalPriceInRub))
            };
        }
        public async Task<ItemsGrouped<ItemsGrouped<ManufacturingItemDTO>>> GetGroupedUserManufactureItemsAsync(int userId) {
            var manufactureItems = new ManufactureAdapter().Convert(await _uow.ManufactureItems.GetUserItemsAsync(userId)).ToList();

            var resultItems = new List<ItemsGrouped<ManufacturingItemDTO>> {
                new() {
                    Totals = new ReportTotalInfo(manufactureItems.Sum(c => c.Amount),
                        manufactureItems.Sum(c => c.TotalPrice),
                        manufactureItems.Sum(c => c.TotalPriceInRub)),
                    Items = manufactureItems.OrderBy(o => o.Articul)
                        .ThenBy(o => o.Size.Value)
                        .ThenBy(o => o.ColorName)
                        .ToList()
                }
            };

            return new ItemsGrouped<ItemsGrouped<ManufacturingItemDTO>> {
                Items = resultItems,
                Totals = new ReportTotalInfo(manufactureItems.Sum(x => x.Amount),
                    manufactureItems.Sum(x => x.TotalPrice),
                    manufactureItems.Sum(x => x.TotalPriceInRub))
            };
        }

        public async Task<ItemsGrouped<ItemsGroupedWithDate<ReservationsItemDTO>>> GetGroupedUserReservationsAsync(int userId) {
            var reservationsItems = _mapper.Map<List<ReservationsItemDTO>>(await _uow.ReservationItems.GetUserItemsAsync(userId));

            var resultItems = reservationsItems
                .GroupBy(
                    x => x.CreateDate.ToBasicString(),
                    (_, groupedItems) => {
                        var items = groupedItems.ToList();
                        return new ItemsGroupedWithDate<ReservationsItemDTO> {
                            CreateDate = items.First().CreateDate,
                            Totals = new ReportTotalInfo(items.Sum(c => c.Amount),
                                items.Sum(c => c.TotalPrice),
                                items.Sum(c => c.TotalPriceInRub)),
                            Items = items.OrderBy(o => o.Articul)
                                .ThenBy(o => o.Size.Value)
                                .ThenBy(o => o.ColorName)
                                .ToList()
                        };
                    })
                .OrderByDescending(o => o.CreateDate)
                .ToList();

            return new ItemsGrouped<ItemsGroupedWithDate<ReservationsItemDTO>> {
                Items = resultItems,
                Totals = new ReportTotalInfo(reservationsItems.Sum(x => x.Amount),
                    reservationsItems.Sum(x => x.TotalPrice),
                    reservationsItems.Sum(x => x.TotalPriceInRub))
            };
        }

        public async Task<ItemsGrouped<ItemsGroupedWithDateName<ShipmentsItemDTO>>> GetGroupedUserShipmentsAsync(int userId, DateTime shipmentDate = default) {
            var shipmentsItems = _mapper.Map<List<ShipmentsItemDTO>>(await _uow.Shipments.GetItemsByUserAsync(userId, shipmentDate));

            var resultItems = shipmentsItems
                .GroupBy(
                    x => new {CreateDate = x.CreateDate.Date, x.DocumentsFolder},
                    (key, groupedItems) => {
                        var items = groupedItems.ToList();
                        return new ItemsGroupedWithDateName<ShipmentsItemDTO> {
                            CreateDate = key.CreateDate,
                            Name = key.DocumentsFolder,
                            Totals = new ReportTotalInfo(items.Sum(c => c.Amount),
                                items.Sum(c => c.TotalPrice),
                                items.Sum(c => c.TotalPriceInRub)),
                            Items = items.OrderBy(o => o.Articul)
                                .ThenBy(o => o.Size.Value)
                                .ThenBy(o => o.ColorName)
                                .ToList()
                        };
                    })
                .OrderByDescending(o => o.CreateDate)
                .ToList();

            return new ItemsGrouped<ItemsGroupedWithDateName<ShipmentsItemDTO>> {
                Items = resultItems,
                Totals = new ReportTotalInfo(shipmentsItems.Sum(x => x.Amount),
                    shipmentsItems.Sum(x => x.TotalPrice),
                    shipmentsItems.Sum(x => x.TotalPriceInRub))
            };
        }
        public async Task<UnprocessedItemsGrouped<UnprocessedOrderItemDTO>> GetUserUnprocessedItemsAsync(int userId) {
            var resultItems = _mapper.Map<List<UnprocessedOrderItemDTO>>(await _uow.Orders.GetUserUnprocessedOrderItemsAsync(userId));
            
            var result = new UnprocessedItemsGrouped<UnprocessedOrderItemDTO> {
                Items = resultItems,
                PreorderTotals = resultItems.Where(x => x.CatalogId == (int) CatalogType.Preorder)
                    .GroupBy(_ => 1)
                    .Select(x => new ReportTotalInfo(x.Sum(c => c.Amount), x.Sum(c => c.TotalPrice), x.Sum(c => c.TotalPriceInRub), x.DefaultIfEmpty(new UnprocessedOrderItemDTO()).Average(c => c.PrepayPercent)))
                    .DefaultIfEmpty(new ReportTotalInfo(0, 0, 0))
                    .First(),
                InStockTotals = resultItems.Where(x => x.CatalogId != (int) CatalogType.Preorder)
                    .GroupBy(_ => 1)
                    .Select(x => new ReportTotalInfo(x.Sum(c => c.Amount), x.Sum(c => c.TotalPrice), x.Sum(c => c.TotalPriceInRub), x.DefaultIfEmpty(new UnprocessedOrderItemDTO()).Average(c => c.PrepayPercent)))
                    .DefaultIfEmpty(new ReportTotalInfo(0, 0, 0))
                    .First()
            };
            result.Totals = result.PreorderTotals + result.InStockTotals;

            return result;
        }

        public async Task<ItemsGrouped<ItemsGroupedWithDate<OrderHistoryItemDTO>>> GetUserHistoryItemsAsync(int userId) {
            var historyItems = _mapper.Map<List<OrderHistoryItemDTO>>(await _uow.OrdersHistory.GetUserItemsAsync(userId));

            var resultItems = _mapper.Map<List<OrderHistoryItemDTO>>(historyItems)
                .GroupBy(
                    x => x.CreateDate.ToBasicString(),
                    (_, groupedItems) => {
                        var items = groupedItems.ToList();
                        return new ItemsGroupedWithDate<OrderHistoryItemDTO> {
                            CreateDate = items.First().CreateDate,
                            Totals = new ReportTotalInfo(items.Sum(c => c.Amount),
                                items.Sum(c => c.TotalPrice),
                                items.Sum(c => c.TotalPriceInRub)),
                            Items = items.OrderBy(o => o.Articul)
                                .ThenBy(o => o.Size.Value)
                                .ThenBy(o => o.ColorName)
                                .ToList()
                        };
                    }
                )
                .OrderByDescending(o => o.CreateDate)
                .ToList();

            return new ItemsGrouped<ItemsGroupedWithDate<OrderHistoryItemDTO>> {
                Items = resultItems,
                Totals = new ReportTotalInfo(historyItems.Sum(x => x.Amount),
                    historyItems.Sum(x => x.TotalPrice),
                    0)
            };
        }

        public async Task<IEnumerable<MoneyDocumentDTO>> GetUserMoneyReportsAsync(int userId) {
            return _mapper.Map<List<MoneyDocumentDTO>>(await _uow.MoneyDocuments.GetUserItemsDetailedAsync(userId));
        }

        public async Task<MoneyDocumentsTotalDTO> GetUserMoneyTotalsAsync(int userId) {
            return _mapper.Map<MoneyDocumentsTotalDTO>(await _uow.MoneyDocumentsTotals.GetUserTotals(userId)) ??
                   new MoneyDocumentsTotalDTO {InitialDate = new DateTime(DateTime.Now.Year, 1, 1), FinalDate = DateTime.Now};
        }

        public async Task<UserComplexOrderDTO> GetUserComplexOrderAsync(int userId) {
            var result = new UserComplexOrderDTO {
                ManufactureItems = await _getGroupedByCollectionUserManufactureItemsAsync(userId),
                Reservations = await _getGroupedByCollectionUserReservationsAsync(userId),
                Shipments = await _getGroupedByCollectionUserShipmentsAsync(userId)
            };
            
            var byCollections = result.ManufactureItems.Items.Select(x => x.Id)
                .Concat(result.Reservations.Items.Select(x => x.Id))
                .Concat(result.Shipments.Items.Select(x => x.Id));

            result.OrderedItems = await _getGroupedByCollectionUserOrderHistoryItemsAsync(userId, byCollections);
            result.ProcessedItems = _getGroupedByCollectionUserProcessedItemsAsync(result);
            return result;
        }

        public async Task<Dictionary<OrderTotalsReportType, TotalSum>> GetUserOrdersTotalsAsync(int userId) {
            var result = (await _uow.OrderTotals.GetUserTotalsAsync(userId))
                .ToDictionary(k => k.Type, v => new TotalSum(v.Sum, v.SumInRub, v.PrepayPercent));

            result.Add(OrderTotalsReportType.TotalProcessing, result[OrderTotalsReportType.Request] + result[OrderTotalsReportType.Manufacture]);
            result.Add(OrderTotalsReportType.ClientBalanceAfterPrepay, new TotalSum(
                    result[OrderTotalsReportType.Balance].TotalPrice - result[OrderTotalsReportType.TotalProcessing].GetTotalPricePrepay(),
                    result[OrderTotalsReportType.Balance].TotalPriceInRub - result[OrderTotalsReportType.TotalProcessing].GetTotalPriceInRubPrepay()));
            result.Add(OrderTotalsReportType.TotalToPay, result[OrderTotalsReportType.Reservation] - result[OrderTotalsReportType.ClientBalanceAfterPrepay]);
            return result;
        }

        public async Task<List<SimpleGroupedModelDTO<RequestsItemDTO>>> GetAllOrdersAsync(ReportsFilter filter) {
            var source = await _uow.OrdersHistory.GetAllItemsAsync(filter);
            var list = source.GroupBy(x => x.ModelId);
            var result = new List<SimpleGroupedModelDTO<RequestsItemDTO>>();

            foreach (var item in list) {
                var first = item.First();

                var items = item.GroupBy(x => new {
                    x.ModelId,
                    x.ColorId,
                    x.Size
                }).Select(x => {
                        var first = x.First();
                        first.Amount = x.Sum(c => c.Amount);
                        first.Price *= x.Sum(c => c.Amount);
                        return first;
                }).ToList().OrderBy(x => x.ColorName).ThenBy(x => x.Size.Value).ToList();

                result.Add(new SimpleGroupedModelDTO<RequestsItemDTO> {
                    ModelKey = item.Key.ToString(),
                    ModelInfo = new ModelInfoDTO() {
                        Name = first.ModelName,
                        Articul = first.Articul,
                        MainPhoto = first.MainPhoto,
                        Size = first.Size
                    },
                    Date = first.OrderDate,
                    TotalAmount = items.Sum(x => x.Amount),
                    TotalSum = items.Sum(x => x.Price),
                    Items = _mapper.Map<List<RequestsItemDTO>>(items)
                });
            }

            return result;
        }

        #region build complex report
        private async Task<ItemsGrouped<ItemsGroupedWithName<OrderHistoryItemDTO>>> _getGroupedByCollectionUserOrderHistoryItemsAsync(int userId, IEnumerable<int> collectionIds) {
            var historyItems = _mapper.Map<List<OrderHistoryItemDTO>>(await _uow.OrdersHistory.GetUserItemsAsync(userId, collectionIds));

            var resultItems = _mapper.Map<List<OrderHistoryItemDTO>>(historyItems)
                .GroupBy(
                    x => new { x.CollectionId, x.CollectionName },
                    (key, groupedItems) => {
                        var items = groupedItems.ToList();
                        return new ItemsGroupedWithName<OrderHistoryItemDTO> {
                            Id = key.CollectionId,
                            Name = key.CollectionName,
                            Totals = new ReportTotalInfo(items.Sum(c => c.Amount),
                                items.Sum(c => c.TotalPrice),
                                items.Sum(c => c.TotalPriceInRub),
                                items.DefaultIfEmpty(new OrderHistoryItemDTO()).Average(c => c.PrepayPercent)),
                            Items = items.OrderBy(o => o.Articul)
                                .ThenBy(o => o.Size.Value)
                                .ThenBy(o => o.ColorName)
                                .ToList()
                        };
                    }
                )
                .OrderByDescending(o => o.Id)
                .ToList();

            return new ItemsGrouped<ItemsGroupedWithName<OrderHistoryItemDTO>> {
                Items = resultItems,
                Totals = new ReportTotalInfo(historyItems.Sum(x => x.Amount),
                    historyItems.Sum(x => x.TotalPrice),
                    0,
                    historyItems.DefaultIfEmpty(new OrderHistoryItemDTO()).Average(x => x.PrepayPercent))
            };
        }

        private async Task<ItemsGrouped<ManufactureItemsGrouped<ManufacturingItemDTO>>> _getGroupedByCollectionUserManufactureItemsAsync(int userId) {
            var manufactureItems = new ManufactureAdapter().Convert(await _uow.ManufactureItems.GetUserItemsAsync(userId)).ToList();

            var resultItems = manufactureItems.GroupBy(
                    x => new { x.CollectionId, x.CollectionName },
                    (key, groupedItems) => {
                        var items = groupedItems.ToList();
                        return new ManufactureItemsGrouped<ManufacturingItemDTO> {
                            Id = key.CollectionId,
                            Name = key.CollectionName,
                            Totals = new ReportTotalInfo(items.Sum(c => c.Amount),
                                items.Sum(c => c.TotalPrice),
                                items.Sum(c => c.TotalPriceInRub),
                                items.DefaultIfEmpty(new ManufacturingItemDTO()).Average(c => c.PrepayPercent)),
                            Items = items
                                .OrderBy(o => o.Articul)
                                .ThenBy(o => o.Size.Value)
                                .ThenBy(o => o.ColorName)
                                .ToList(),
                            TotalByState = items.GroupBy(x => x.State, (iKey, iGroupedItems) => {
                                var iItems = iGroupedItems.ToList();
                                return new LookUpItem<ManufactureState, ReportTotalInfo>(iKey,
                                    new ReportTotalInfo(iItems.Sum(c => c.Amount),
                                        iItems.Sum(c => c.TotalPrice),
                                        iItems.Sum(c => c.TotalPriceInRub),
                                        iItems.DefaultIfEmpty(new ManufacturingItemDTO()).Average(c => c.PrepayPercent)));
                            }).ToDictionary(k => k.Key, v => v.Value)
                        };
                    })
                .OrderByDescending(o => o.Id)
                .ToList();

            return new ItemsGrouped<ManufactureItemsGrouped<ManufacturingItemDTO>> {
                Items = resultItems,
                Totals = new ReportTotalInfo(manufactureItems.Sum(x => x.Amount),
                    manufactureItems.Sum(x => x.TotalPrice),
                    manufactureItems.Sum(x => x.TotalPriceInRub),
                    manufactureItems.DefaultIfEmpty(new ManufacturingItemDTO()).Average(x => x.PrepayPercent))
            };
        }

        private async Task<ItemsGrouped<ItemsGroupedWithName<ReservationsItemDTO>>> _getGroupedByCollectionUserReservationsAsync(int userId) {
            var reservationsItems = _mapper.Map<List<ReservationsItemDTO>>(await _uow.ReservationItems.GetUserItemsAsync(userId));

            var resultItems = reservationsItems
                .GroupBy(
                    x => new { x.CollectionId, x.CollectionName },
                    (key, groupedItems) => {
                        var items = groupedItems.ToList();
                        return new ItemsGroupedWithName<ReservationsItemDTO> {
                            Id = key.CollectionId,
                            Name = key.CollectionName,
                            Totals = new ReportTotalInfo(items.Sum(c => c.Amount),
                                items.Sum(c => c.TotalPrice),
                                items.Sum(c => c.TotalPriceInRub),
                                items.DefaultIfEmpty(new ReservationsItemDTO()).Average(c => c.PrepayPercent)),
                            Items = items.OrderBy(o => o.Articul)
                                .ThenBy(o => o.Size.Value)
                                .ThenBy(o => o.ColorName)
                                .ToList()
                        };
                    })
                .OrderByDescending(o => o.Id)
                .ToList();

            return new ItemsGrouped<ItemsGroupedWithName<ReservationsItemDTO>> {
                Items = resultItems,
                Totals = new ReportTotalInfo(reservationsItems.Sum(x => x.Amount),
                    reservationsItems.Sum(x => x.TotalPrice),
                    reservationsItems.Sum(x => x.TotalPriceInRub),
                    reservationsItems.DefaultIfEmpty(new ReservationsItemDTO()).Average(x => x.PrepayPercent))
            };
        }

        private async Task<ItemsGrouped<ItemsGroupedWithName<ItemsGroupedWithDateName<ShipmentsItemDTO>>>> _getGroupedByCollectionUserShipmentsAsync(int userId) {
            var shipmentsItems = _mapper.Map<List<ShipmentsItemDTO>>(await _uow.Shipments.GetItemsByUserForLastMonthsAsync(userId, 3));

            var resultItems = shipmentsItems
                .GroupBy(
                    x => new { x.CollectionId, x.CollectionName },
                    (key, groupedItems) => {
                        var items = groupedItems.ToList();
                        return new ItemsGroupedWithName<ItemsGroupedWithDateName<ShipmentsItemDTO>> {
                            Id = key.CollectionId,
                            Name = key.CollectionName,
                            Totals = new ReportTotalInfo(items.Sum(c => c.Amount),
                                items.Sum(c => c.TotalPrice),
                                items.Sum(c => c.TotalPriceInRub),
                                items.DefaultIfEmpty(new ShipmentsItemDTO()).Average(c => c.PrepayPercent)),
                            Items = items.GroupBy(i => new {i.CreateDate.Date, i.DocumentsFolder},
                                (iKey, iGroupedItems) => {
                                var iItems = iGroupedItems.ToList();
                                return new ItemsGroupedWithDateName<ShipmentsItemDTO> {
                                    CreateDate = iKey.Date,
                                    Name = iKey.DocumentsFolder,
                                    Totals = new ReportTotalInfo(iItems.Sum(c => c.Amount),
                                        iItems.Sum(c => c.TotalPrice),
                                        iItems.Sum(c => c.TotalPriceInRub),
                                        iItems.DefaultIfEmpty(new ShipmentsItemDTO()).Average(c => c.PrepayPercent)),
                                    Items = iItems.OrderBy(o => o.Articul)
                                        .ThenBy(o => o.Size.Value)
                                        .ThenBy(o => o.ColorName)
                                        .ToList()
                                };
                            }).ToList()
                        };
                    })
                .OrderByDescending(o => o.Id)
                .ToList();

            return new ItemsGrouped<ItemsGroupedWithName<ItemsGroupedWithDateName<ShipmentsItemDTO>>> {
                Items = resultItems,
                Totals = new ReportTotalInfo(shipmentsItems.Sum(x => x.Amount),
                    shipmentsItems.Sum(x => x.TotalPrice),
                    shipmentsItems.Sum(x => x.TotalPriceInRub),
                    shipmentsItems.DefaultIfEmpty(new ShipmentsItemDTO()).Average(x => x.PrepayPercent))
            };
        }

        private static ItemsGrouped<ItemsGroupedWithDateName<BasicOrderItemDTO>> _getGroupedByCollectionUserProcessedItemsAsync(UserComplexOrderDTO complexOrder) {
            var processedItems = complexOrder.OrderedItems.Items.SelectMany(x => x.Items.OfType<BasicOrderItemDTO>())
                .Concat(complexOrder.ManufactureItems.Items.SelectMany(x => x.Items))
                .Concat(complexOrder.Reservations.Items.SelectMany(x => x.Items))
                .Concat(complexOrder.Shipments.Items.SelectMany(x => x.Items.SelectMany(i => i.Items)))
                .ToList();

            return new ItemsGrouped<ItemsGroupedWithDateName<BasicOrderItemDTO>> {
                Items = processedItems.GroupBy(x => new {x.CollectionId, x.CollectionName},
                        (key, groupedItems) => {
                            var items = groupedItems.ToList();
                            return new ItemsGroupedWithDateName<BasicOrderItemDTO> {
                                Id = key.CollectionId,
                                Name = key.CollectionName,
                                CreateDate = items.Where(x => x.CreateDate != DateTime.MinValue)
                                    .Select(x => x.CreateDate)
                                    .DefaultIfEmpty(DateTime.Today)
                                    .Min(),
                                Totals = complexOrder.OrderedItems.Items
                                             .FirstOrDefault(x => x.Id == key.CollectionId)
                                             ?.Totals ??
                                         new ReportTotalInfo(items.Sum(c => c.Amount),
                                             items.Sum(c => c.TotalPrice),
                                             items.Sum(c => c.TotalPriceInRub),
                                             items.DefaultIfEmpty(new ManufacturingItemDTO()).Average(c => c.PrepayPercent)),
                                Items = items.GroupBy(c => c.ModelKey)
                                    .Select(c => {
                                        var copy = (BasicOrderItemDTO) c.First().Clone();
                                        copy.Amount = c.Sum(x => x.Amount);
                                        copy.CreateDate = c.Where(x => x.CreateDate != DateTime.MinValue)
                                            .Select(x => x.CreateDate)
                                            .DefaultIfEmpty(DateTime.Today)
                                            .Min();
                                        return copy;
                                    })
                                    .OrderBy(o => o.Articul)
                                    .ThenBy(o => o.Size.Value)
                                    .ThenBy(o => o.ColorName)
                                    .ToList()
                            };
                        })
                    .OrderByDescending(x => x.Id)
                    .ToList(),
                Totals = new ReportTotalInfo(processedItems.Sum(x => x.Amount),
                    processedItems.Sum(x => x.TotalPrice),
                    processedItems.Sum(x => x.TotalPriceInRub),
                    processedItems.DefaultIfEmpty(new ManufacturingItemDTO()).Average(x => x.PrepayPercent))
            };
        }

        public async Task<List<LookUpItem<int, string>>> GetOrderersLookupAsync() => await _uow.OrdersHistory.GetAllOrderersAsync();
        #endregion
    }
}