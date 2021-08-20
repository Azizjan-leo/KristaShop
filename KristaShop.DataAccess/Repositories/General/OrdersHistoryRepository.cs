using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Filters;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories.General {
    public class OrdersHistoryRepository : IOrdersHistoryRepository {
        private readonly DbContext _context;
        private readonly MySqlCompiler _compiler;

        public OrdersHistoryRepository(DbContext context) {
            _context = context;
            _compiler = new MySqlCompiler();
        }

        public async Task<IEnumerable<OrderHistorySqlView>> GetUserItemsAsync(int userId) {
            var compiledSql = _compiler.Compile(_createOrderHistoryItemsQueryByUserId(userId));
            return await _context.Set<OrderHistorySqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }

        public async Task<IEnumerable<OrderHistorySqlView>> GetUserItemsAsync(int userId, IEnumerable<int> collectionIds) {
            var compiledSql = _compiler.Compile(_createOrderHistoryItemsQueryByUserIdAndCollection(userId, collectionIds));
            return await _context.Set<OrderHistorySqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }

        public async Task<IEnumerable<OrderHistorySqlView>> GetAllItemsAsync(ReportsFilter filter) {
            var query = _createOrdersHistoryItemsQuery();
            if (filter.SelectedCityIds?.Any() is true)
                query = query.WhereIn("users.city", filter.SelectedCityIds);

            if (filter.SelectedArticuls?.Any() is true)
                query = query.WhereIn("models.articul", filter.SelectedArticuls);

            if (filter.SelectedCollectionIds?.Any() is true) 
                query = query.WhereIn("collection.id", filter.SelectedCollectionIds); 

            if (filter.SelectedColorIds?.Any() is true)
                query = query.WhereIn("orders.color", filter.SelectedColorIds);

            if (filter.SelectedUserIds?.Any() is true)
                query = query.WhereIn("users.id", filter.SelectedUserIds);

            if (filter.SelectedManagerIds?.Any() is true)
                query = query.WhereIn("users.manager", filter.SelectedManagerIds);

            if (filter.Date.From != null)
                query = query.WhereDate("orders.datez", ">=", filter.Date.From);

            if (filter.Date.To != null)
                query = query.WhereDate("orders.datez", "<=", filter.Date.To);

            var compiledSql = _compiler.Compile(query);
            return await _context.Set<OrderHistorySqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }

        private Query _createOrdersHistoryItemsQuery() {
            return new Query("1c_zakaz_history AS orders")
                .Select(@"orders.datez AS order_date", "users.id AS user_id", "users.manager", "models.articul",
                    "models.id AS model_id", "models.name AS model_name", "orders.color AS color_id", "models.line AS size_value",
                    "models.razmerov AS parts_count", "colors.name AS color_name",
                    "matcolors.photo AS color_photo", "color_groups.rgb AS color_group_rgb_value",
                    "photos.photo_path AS photo_by_color", "item_descriptor.main_photo")
                .SelectRaw(@"AVG(`models`.`price`) AS `price`, SUM(`orders`.`kolichestvo`) AS `amount`")
                .LeftJoin("1c_clients AS users", "orders.klient", "users.id")
                .LeftJoin("1c_models AS models", "orders.model", "models.id")
                .Join("1c_colors AS colors", "orders.color", "colors.id")
                .LeftJoin("1c_materialcolors AS matcolors",
                    x => x.On("matcolors.color", "orders.color").On("models.material", "matcolors.material"))
                .LeftJoin("1c_colors_group AS color_groups", "color_groups.id", "colors.group")
                .LeftJoin("catalog_item_descriptor AS item_descriptor", "models.articul", "item_descriptor.articul")
                .LeftJoin(GenericQueries.GetModelPhotoByColorInnerQuery().As("photos"),
                    x => x.On("photos.articul", "models.articul").On("photos.color_id", "orders.color"))
                .JoinModelCollection("orders")
                .GroupBy("orders.datez", "users.id", "models.articul", "models.id",
                    "orders.color", "models.line", "models.razmerov", "colors.name",
                    "matcolors.photo", "color_groups.rgb", "photos.photo_path",
                    "item_descriptor.main_photo");
        }

        private Query _createOrderHistoryItemsQueryByUserId(int userId) {
            return _createOrdersHistoryItemsQuery()
                .Where("users.id", userId);
        }

        private Query _createOrderHistoryItemsQueryByUserIdAndCollection(int userId, IEnumerable<int> collectionIds) {
            return _createOrderHistoryItemsQueryByUserId(userId)
                .WhereIn("collection.id", collectionIds);
        }

        public async Task<List<LookUpItem<int, string>>> GetAllOrderersAsync() {
           var query = new Query("1c_zakaz_history AS orders")
                 .Select(@"users.id AS key", "users.fullname as value")
                 .Join("1c_clients AS users", "orders.klient", "users.id");

            var compiledSql = _compiler.Compile(query);
            return await _context.Set<LookUpItem<int, string>>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }
    }
}
