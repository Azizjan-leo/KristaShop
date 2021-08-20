using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories.General {
    public class OrderRepository : IOrderRepository {
        private readonly DbContext _context;
        private readonly MySqlCompiler _compiler;

        public OrderRepository(DbContext context) {
            _context = context;
            _compiler = new MySqlCompiler();
        }

        public async Task<IEnumerable<OrderDetailsFull>> GetUserUnprocessedOrderItemsAsync(int userId) {
            var compiledSql = _compiler.Compile(_createUnprocessedOrderItemsQueryForUserId(userId));
            return await _context.Set<OrderDetailsFull>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }

        private Query _createOrderItemsQuery() {
            return new Query("for1c_order_details AS details")
                .Select(
                    "details.id", "details.catalog_id", "details.model_id",
                    "details.nomenclature_id", "details.color_id", "details.size_value",
                    "details.price", "details.price_in_rub", "models.articul", "catalogs.name AS catalog_name",
                    "colors.name AS color_name", "matcolors.photo AS color_photo",
                    "colors_group.name AS color_group_name", "colors_group.rgb AS color_group_rgb_value",
                    "photos.photo_path AS photo_by_color", "catalog_item.main_photo")
                .SelectRaw("0 AS `order_id`, SUM(`details`.`amount`) AS `amount`")
                .Join("for1c_orders AS orders", "details.order_id", "orders.id")
                .Join("1c_models AS models", "details.model_id", "models.id")
                .LeftJoin("dict_catalogs AS catalogs", "details.catalog_id", "catalogs.catalog_id_1c")
                .LeftJoin("1c_material AS materials", "models.material", "materials.id")
                .Join("1c_colors AS colors", "details.color_id", "colors.id")
                .LeftJoin("1c_materialcolors AS matcolors",
                    x => x.On("matcolors.color", "details.color_id").On("models.material", "matcolors.material"))
                .LeftJoin("1c_colors_group AS colors_group", "colors.group", "colors_group.id")
                .LeftJoin("catalog_item_descriptor AS catalog_item", "models.articul", "catalog_item.articul")
                .LeftJoin(GenericQueries.GetModelPhotoByColorInnerQuery().As("photos"),
                    x => x.On("photos.articul", "models.articul").On("photos.color_id", "details.color_id"))
                .JoinModelCollection("details", "collection_id")
                .GroupBy("details.catalog_id", "details.model_id", "details.nomenclature_id", "details.color_id")
                .GroupByRaw("FORMAT(ROUND(`details`.`price`*1000), 0), FORMAT(ROUND(`details`.`price_in_rub`*1000), 0)");
        }

        private Query _createUnprocessedOrderItemsQuery() {
            return _createOrderItemsQuery()
                .Where(x => x.Where(q => q.Where("details.catalog_id", (int)CatalogType.Preorder).Where("orders.is_processed_preorder", 0))
                    .OrWhere(q => q.WhereNot("details.catalog_id", (int)CatalogType.Preorder).Where("orders.is_processed_retail", 0)));
        }

        private Query _createUnprocessedOrderItemsQueryForUserId(int userId) {
            return _createUnprocessedOrderItemsQuery()
                .Where("orders.user_id", userId);
        }
    }
}