using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KristaShop.Common.Enums;
using KristaShop.Common.Extensions;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Views;
using Microsoft.EntityFrameworkCore;
using Module.Common.Business.Models;
using Module.Order.Business.Interfaces;
using Module.Order.Business.Models;
using Module.Order.Business.Models.Adapters;

namespace Module.Order.Business.Services {
    public class OrderReportService : IOrderReportService {
        private readonly KristaShopDbContext _context;
        private readonly IMapper _mapper;

        public OrderReportService(KristaShopDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReportTotalsDTO> GetOrderTotals(bool isProcessedOnly = true, List<string> articuls = null,
            List<int> catalogIds = null, List<int> userIds = null, List<int> colorIds = null,
            List<int> cityIds = null, List<int> managerIds = null) {

            var whereStatement = "";

            if (isProcessedOnly) {
                whereStatement = " ((`details`.`catalog_id` = 3 AND `orders`.`is_processed_preorder` = 0) OR (`details`.`catalog_id` != 3 AND `orders`.`is_processed_retail` = 0))";
            }

            if (catalogIds != null && catalogIds.Any()) {
                whereStatement += (string.IsNullOrEmpty(whereStatement) ? "" : " AND ") + $"`details`.`catalog_id` in ({string.Join(",", catalogIds)})";
            }

            if (articuls != null && articuls.Any()) {
                whereStatement += (string.IsNullOrEmpty(whereStatement) ? "" : " AND ") + $"`models`.`articul` IN ('{string.Join("' ,'", articuls)}')";
            }

            if (userIds != null && userIds.Any()) {
                whereStatement += (string.IsNullOrEmpty(whereStatement) ? "" : " AND ") + $"`orders`.`user_id` in ({string.Join(",", userIds)})";
            }

            if (colorIds != null && colorIds.Any()) {
                whereStatement += (string.IsNullOrEmpty(whereStatement) ? "" : " AND ") + $"`details`.`color_id` IN ({string.Join(",", colorIds)})";
            }

            if (cityIds != null && cityIds.Any()) {
                whereStatement += (string.IsNullOrEmpty(whereStatement) ? "" : " AND ") + $"`cities`.`id` IN ({string.Join(",", cityIds)})";
            }

            if (managerIds != null && managerIds.Any()) {
                whereStatement += (string.IsNullOrEmpty(whereStatement) ? "" : " AND ") + $"`managers`.`id` IN ({string.Join(",", managerIds)})";
            }

            if (!string.IsNullOrEmpty(whereStatement)) {
                whereStatement = "WHERE " + whereStatement;
            }

            var query = @$"SELECT sum(`details`.`amount`) AS `total_amount`,
sum(`details`.`price` * `details`.`amount`) AS `total_price`,
sum(`details`.`price_in_rub` * `details`.`amount`) AS `total_price_rub`, `details`.`catalog_id`
FROM `for1c_order_details` AS `details`
LEFT JOIN `for1c_orders` AS `orders` ON `details`.`order_id` = `orders`.`id`
LEFT JOIN `1c_clients` AS `users` ON `orders`.`user_id` = `users`.`id`
LEFT OUTER JOIN `1c_manager` AS `managers` ON `users`.`manager` = `managers`.`id`
LEFT OUTER JOIN `1c_city` AS `cities` ON `users`.`city` = `cities`.`id`
LEFT JOIN `1c_models` AS `models` ON `details`.`model_id` = `models`.`id`
{whereStatement}
GROUP BY `details`.`catalog_id`"
                .ToFormattable();

            var orderTotals = await _context.Set<ReportTotals>().FromSqlInterpolated(query).ToListAsync();
            return new OrderTotalsAdapter().Convert(orderTotals);
        }

        public async Task<List<ClientOrdersTotalsDTO>> GetClientOrdersTotals(bool unprocessedOnly,
            bool onlyNotEmptyUserCarts, List<string> articuls = null, List<int> catalogIds = null,
            List<int> userIds = null, List<int> colorIds = null, List<int> cityIds = null, List<int> managerIds = null) {
            var whereStatement = " `cities`.`id` > 0 ";
            var innerWhereStatement = string.Empty;

            if (articuls != null && articuls.Any()) {
                innerWhereStatement += (!string.IsNullOrEmpty(innerWhereStatement) ? " AND " : "") + $"`models`.`articul` IN ('{string.Join("' ,'", articuls)}')";
            }

            if (catalogIds != null && catalogIds.Any()) {
                innerWhereStatement += (!string.IsNullOrEmpty(innerWhereStatement) ? " AND " : "") + $"`details`.`catalog_id` IN ({string.Join(",", catalogIds)})";
            }

            if (colorIds != null && colorIds.Any()) {
                innerWhereStatement += (!string.IsNullOrEmpty(innerWhereStatement) ? " AND " : "") + $"`details`.`color_id` IN ({string.Join(",", colorIds)})";
            }

            if (userIds != null && userIds.Any()) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + $"`users`.`id` IN ({string.Join(",", userIds)})";
            }

            if (cityIds != null && cityIds.Any()) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + $"`cities`.`id` IN ({string.Join(",", cityIds)})";
            }

            if (managerIds != null && managerIds.Any()) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + $"`managers`.`id` IN ({string.Join(",", managerIds)})";
            }

            if (onlyNotEmptyUserCarts) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + "`cart`.`has_cart_items` = 1";
            }

            if (unprocessedOnly) {
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + "(`order_totals`.`preorder_amount` > 0 AND `order_totals`.`is_processed_preorder` = 0) OR (`order_totals`.`instock_amount` > 0 AND `order_totals`.`is_processed_retail` = 0)";
            }

            if (!string.IsNullOrEmpty(innerWhereStatement)) {
                innerWhereStatement = "WHERE " + innerWhereStatement;
                whereStatement += (!string.IsNullOrEmpty(whereStatement) ? " AND " : "") + "(`order_totals`.`preorder_amount` > 0 OR `order_totals`.`instock_amount` > 0)";
            }

            if (!string.IsNullOrEmpty(whereStatement)) {
                whereStatement = "WHERE " + whereStatement;
            }

            var query = $@"SELECT `users`.`id` AS `user_id`, `users`.`fullname` AS `user_fullname`,
IFNULL(`managers`.`id`, 0) AS `manager_id`, IFNULL(`managers`.`name`, '') AS `manager_name`,
IFNULL(`cities`.`id`, 0) AS `city_id`, IFNULL(`cities`.`name`, '') AS `city_name`, 
IFNULL(`cart`.`has_cart_items`, 0) as `has_cart_items`,
SUM(IFNULL(`order_totals`.`preorder_total_sum`, 0)) AS `preorder_total_sum`,
SUM(IFNULL(`order_totals`.`instock_total_sum`, 0)) AS `instock_total_sum`, 
SUM(IFNULL(`order_totals`.`preorder_total_sum_rub`, 0)) AS `preorder_total_sum_rub`,
SUM(IFNULL(`order_totals`.`instock_total_sum_rub`, 0)) AS `instock_total_sum_rub`, 
SUM(IFNULL(`order_totals`.`preorder_amount`, 0)) AS `preorder_amount`,
SUM(IFNULL(`order_totals`.`instock_amount`, 0)) AS `instock_amount`
FROM `1c_clients` AS `users`
LEFT JOIN `1c_city` AS `cities` ON `users`.`city` = `cities`.`id` 
LEFT JOIN `1c_manager` AS `managers` ON `users`.`manager` = `managers`.`id`
LEFT JOIN (
    SELECT `items`.`user_id`, IF(count(`items`.`id`) > 0, 1, 0) AS `has_cart_items`
    FROM `cart_items_1c` AS `items`
    GROUP BY `items`.`user_id`
) AS `cart` ON `users`.`id` = `cart`.`user_id`
LEFT JOIN (
	SELECT `orders`.`id`, `orders`.`is_processed_preorder`, `orders`.`is_processed_retail`, `orders`.`user_id`,
	SUM(IF(`details`.`catalog_id` = {(int)CatalogType.Preorder}, `details`.`price` * `details`.`amount`, 0)) AS `preorder_total_sum`,
    SUM(IF(`details`.`catalog_id` != {(int)CatalogType.Preorder}, `details`.`price` * `details`.`amount`, 0)) AS `instock_total_sum`,
    SUM(IF(`details`.`catalog_id` = {(int)CatalogType.Preorder}, `details`.`price_in_rub` * `details`.`amount`, 0)) AS `preorder_total_sum_rub`,
    SUM(IF(`details`.`catalog_id` != {(int)CatalogType.Preorder}, `details`.`price_in_rub` * `details`.`amount`, 0)) AS `instock_total_sum_rub`,
    SUM(IF(`details`.`catalog_id` = {(int)CatalogType.Preorder}, `details`.`amount`, 0)) AS `preorder_amount`, 
    SUM(IF(`details`.`catalog_id` != {(int)CatalogType.Preorder}, `details`.`amount`, 0)) AS `instock_amount`
	FROM `for1c_order_details` AS `details`
    LEFT JOIN `for1c_orders` AS `orders` ON `details`.`order_id` = `orders`.`id`
    LEFT JOIN `1c_models` AS `models` ON `details`.`model_id` = `models`.`id`
    JOIN `1c_colors` AS `colors` ON `details`.`color_id` = `colors`.`id`
    {innerWhereStatement}
	GROUP BY `orders`.`id`
) AS `order_totals` ON `users`.`id` = `order_totals`.`user_id` 
{whereStatement}
GROUP BY `users`.`id`, `users`.`fullname`, `managers`.`id`, `managers`.`name`, `cities`.`id`, `cities`.`name`
ORDER BY `cities`.`name` ASC;"
                .ToFormattable();

            var ordersList = await _context.Set<ClientOrdersTotalsSqlView>().FromSqlInterpolated(query).ToListAsync();
            return _mapper.Map<List<ClientOrdersTotalsDTO>>(ordersList);
        }

        public async Task<List<string>> GetUnprocessedOrdersArticulsAsync(bool processedOnly) {
            var whereSection = string.Empty;
            if (processedOnly) {
                whereSection = "WHERE (`details`.`catalog_id` = 3 AND `orders`.`is_processed_preorder` = 0) OR (`details`.`catalog_id` != 3 AND `orders`.`is_processed_retail` = 0)";
            }

            var query = $@"SELECT `models`.`articul` as `value`
FROM `for1c_order_details` AS `details`
LEFT JOIN `for1c_orders` AS `orders` ON `details`.`order_id` = `orders`.`id`
JOIN `1c_models` AS `models` ON `details`.`model_id` = `models`.`id`
{whereSection}
GROUP BY `models`.`articul`"
                .ToFormattable();

            var articuls = await _context.Set<Scalar>().FromSqlInterpolated(query).ToListAsync();
            return articuls.Select(x => x.Value).ToList();
        }

        public async Task<List<LookUpItem<int, string>>> GetUnprocessedOrdersUsersAsync(bool processedOnly) {
            var whereSection = string.Empty;
            if (processedOnly) {
                whereSection = "WHERE (`details`.`catalog_id` = 3 AND `orders`.`is_processed_preorder` = 0) OR (`details`.`catalog_id` != 3 AND `orders`.`is_processed_retail` = 0)";
            }

            var query = $@"SELECT `users`.`id` AS `key`, `users`.`fullname` AS `value`
FROM `for1c_order_details` AS `details`
LEFT JOIN `for1c_orders` AS `orders` ON `details`.`order_id` = `orders`.`id`
LEFT JOIN `1c_clients` AS `users` ON `orders`.`user_id` = `users`.`id`
{whereSection}
GROUP BY `orders`.`user_id`"
                .ToFormattable();

            return await _context.Set<LookUpItem<int, string>>().FromSqlInterpolated(query).ToListAsync();
        }
    }
}