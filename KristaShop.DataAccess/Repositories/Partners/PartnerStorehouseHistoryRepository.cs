using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.Partners;
using KristaShop.DataAccess.Interfaces.Repositories.Partners;
using KristaShop.DataAccess.Views.Partners;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories.Partners {
    public class PartnerStorehouseHistoryRepository : Repository<PartnerStorehouseHistoryItem, Guid>,
        IPartnerStorehouseHistoryRepository {
        private readonly MySqlCompiler _compiler;

        public PartnerStorehouseHistoryRepository(DbContext context) : base(context) {
            _compiler = new MySqlCompiler();
        }

        public async Task<IEnumerable<PartnerStorehouseHistoryItemSqlView>> GetHistoryItemsAsync(int? userId,
            DateTimeOffset date = default, MovementDirection movementDirection = MovementDirection.None,
            MovementType movementType = MovementType.None, bool isAmountPositive = false) {
            var amountToPositive = isAmountPositive ? -1 : 1;
            var compiledSql = _compiler.Compile(_createHistoryItemsQuery(userId, amountToPositive, date, movementDirection, movementType));
            return await Context.Set<PartnerStorehouseHistoryItemSqlView>()
                .FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }

        public async Task<IEnumerable<PartnerStorehouseHistoryItemSqlView>> GetGroupedHistoryItems(int? userId,
            DateTimeOffset date = default, MovementDirection movementDirection = MovementDirection.None,
            MovementType movementType = MovementType.None, bool isAmountPositive = false) {
            var amountToPositive = isAmountPositive ? -1 : 1;
            var compiledSql = _compiler.Compile(_createHistoryGroupedItemsQuery(userId, amountToPositive, date, movementDirection, movementType));
            return await Context.Set<PartnerStorehouseHistoryItemSqlView>()
                .FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }
        
        private Query _createHistoryItemsQuery(int? userId, int amountMultiplier, DateTimeOffset date = default,
            MovementDirection movementDirection = MovementDirection.None,
            MovementType movementType = MovementType.None) {
            var query = _createHistoryItemsQuery()
                .Select("models.name AS model_name", "colors.name AS color_name", "color_groups.rgb AS color_code",
                    "matcolors.photo AS color_photo", "items.create_date AS item_create_date", "descriptor.main_photo")
                .SelectRaw($"`items`.`price` * {amountMultiplier} AS `price`, `items`.`price_rub` * {amountMultiplier} AS `price_rub`")
                .SelectRaw("concat(`items`.`size_value`, '|', `models`.`line`) AS `size_value`")
                .SelectRaw($"`items`.`amount` * {amountMultiplier} AS `amount`")
                .LeftJoin("1c_models AS models", "items.model_id", "models.id")
                .Join("1c_colors AS colors", "colors.id", "items.color_id")
                .LeftJoin("1c_colors_group AS color_groups", "color_groups.id", "colors.group")
                .Join("catalog_item_descriptor AS descriptor", "descriptor.articul", "models.articul")
                .LeftJoin("1c_materialcolors AS matcolors",
                    x => x.On("matcolors.color", "items.color_id").On("models.material", "matcolors.material"));

            return _applyFilter(userId, date, movementDirection, movementType, query);
        }

        private Query _createHistoryGroupedItemsQuery(int? userId, int amountMultiplier, DateTimeOffset date = default,
            MovementDirection movementDirection = MovementDirection.None,
            MovementType movementType = MovementType.None) {
            var query = _createHistoryItemsQuery()
                .Select("models.name AS model_name", "colors.name AS color_name", "color_groups.rgb AS color_code",
                    "matcolors.photo AS color_photo", "descriptor.main_photo")
                .SelectRaw("concat(`items`.`size_value`, '|', `models`.`line`) AS `size_value`")
                .SelectRaw($"SUM(`items`.`amount`) * {amountMultiplier} AS `amount`")
                .SelectRaw($"AVG(`items`.`price`) * {amountMultiplier} AS `price`")
                .SelectRaw($"AVG(`items`.`price_rub`) * {amountMultiplier} AS `price_rub`")
                .SelectRaw("DATE(`items`.`create_date`) AS `item_create_date`")
                .LeftJoin("1c_models AS models", "items.model_id", "models.id")
                .Join("1c_colors AS colors", "colors.id", "items.color_id")
                .LeftJoin("1c_colors_group AS color_groups", "color_groups.id", "colors.group")
                .Join("catalog_item_descriptor AS descriptor", "descriptor.articul", "models.articul")
                .LeftJoin("1c_materialcolors AS matcolors",
                    x => x.On("matcolors.color", "items.color_id").On("models.material", "matcolors.material"));

            query = _applyFilter(userId, date, movementDirection, movementType, query);

            return query.GroupBy("items.user_id", "items.movement_direction", "items.movement_type",
                "items.articul", "items.model_id", "items.color_id", "size_value", "model_name", "color_name",
                "color_code", "color_photo", "item_create_date");
        }

        private static Query _applyFilter(int? userId, DateTimeOffset date, MovementDirection movementDirection,
            MovementType movementType, Query query) {
            if (userId != null)
                query = query.Where("items.user_id", userId);

            if (date != default) {
                query = query.Where("items.create_date", "<", date.Date.AddDays(1))
                    .Where("items.create_date", ">", date.Date.AddDays(-1));
            }

            if (movementDirection != MovementDirection.None) {
                query = query.Where("items.movement_direction", movementDirection);
            }

            if (movementType != MovementType.None) {
                query = query.Where("items.movement_type", movementType);
            }
            
            return query;
        }

        private Query _createHistoryItemsQuery() {
            return new Query("part_storehouse_history_items AS items")
                .Select("items.user_id", "items.movement_direction", "items.movement_type", "items.articul",
                    "items.model_id", "items.color_id");
        }
    }
}