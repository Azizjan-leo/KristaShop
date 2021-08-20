using System;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.Partners;
using KristaShop.DataAccess.Interfaces.Repositories.Partners;
using KristaShop.DataAccess.Views.Partners;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories.Partners {
    public class PartnerStorehouseItemRepository : Repository<PartnerStorehouseItem, Guid>, IPartnerStorehouseItemRepository {
        private readonly MySqlCompiler _compiler;

        public PartnerStorehouseItemRepository(DbContext context) : base(context) {
            _compiler = new MySqlCompiler();
        }

        public async Task<PartnerStorehouseItem> GetStorehouseItemAsync(int modelId, int colorId, string sizeValue, int userId) {
            var item = await All
                .Where(x => x.UserId == userId &&
                            x.ModelId == modelId &&
                            x.ColorId == colorId &&
                            x.SizeValue == sizeValue &&
                            x.Amount > 0)
                .OrderByDescending(x => x.IncomeDate)
                .FirstOrDefaultAsync();

            if (item == null) {
                throw new StorehouseItemNotFound(userId, modelId, colorId, sizeValue);
            }

            return item;
        }

        public IQueryable<PartnerStorehouseItemSqlView> GetStorehouseItems(int userId) {
            var query = _createStorehouseItemsWithBarcodesQuery()
                .Where("items.user_id", userId);

            var compiledSql = _compiler.Compile(query);
            return Context.Set<PartnerStorehouseItemSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray());
        }

        public async Task<PartnerStorehouseItemSqlView> GetStorehouseItemAsync(string barcode, int userId) {
            var query = _createStorehouseItemsByBarcodeQuery(barcode)
                .Where("barcodes.barcodes", barcode)
                .Where("items.user_id", userId)
                .Limit(1);

            var compiledSql = _compiler.Compile(query);
            return await Context.Set<PartnerStorehouseItemSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).FirstOrDefaultAsync();
        }

        private Query _createStorehouseItemsQuery() {
            return new Query("part_storehouse_items as items")
                .Select("items.user_id", "models.name", "items.articul", "items.model_id", "items.color_id",
                    "items.order_type", "colors.name AS color_name", "matcolors.photo AS color_photo",
                    "color_groups.name AS color_group_name", "color_groups.rgb AS color_code", "descriptor.main_photo")
                .SelectRaw("concat(`items`.`size_value`, '|', `models`.`line`) AS `size_value`")
                .SelectRaw("SUM(`items`.`amount`) AS `amount`, AVG(`items`.`price`) AS `price`, AVG(`items`.`price_rub`) AS `price_rub`")
                .LeftJoin("1c_models AS models", "items.model_id", "models.id")
                .Join("1c_colors AS colors", "colors.id", "items.color_id")
                .LeftJoin("1c_colors_group AS color_groups", "color_groups.id", "colors.group")
                .LeftJoin("catalog_item_descriptor AS descriptor", "models.articul", "descriptor.articul")
                .LeftJoin("1c_materialcolors AS matcolors",
                    x => x.On("matcolors.color", "items.color_id").On("models.material", "matcolors.material"))
                .OrderBy("items.articul", "models.line", "colors.name", "items.size_value");
        }

        private Query _createStorehouseItemsQueryGrouped() {
            return _createStorehouseItemsQuery()
                .GroupBy("items.user_id", "items.articul", "items.model_id", "items.color_id", "items.size_value",
                    "items.order_type");
        }

        private Query _createStorehouseItemsWithBarcodesQuery() => _storehouseItemsWithBarcodeQuery(_createBarcodeQuery());

        private Query _createStorehouseItemsByBarcodeQuery(string barcode) {
            var barcodesQuery = _createBarcodeQuery().Where("inner_barcodes.barcode", barcode);
            return _storehouseItemsWithBarcodeQuery(barcodesQuery);
        }

        private Query _storehouseItemsWithBarcodeQuery(Query barcodesQuery) {
            return _createStorehouseItemsQuery()
                .Select("barcodes.barcodes")
                .Join(barcodesQuery.As("barcodes"), x => x.On("barcodes.model", "items.model_id")
                    .On("barcodes.color", "items.color_id")
                    .On("barcodes.razmer", "items.size_value"))
                .GroupBy("items.user_id", "models.name", "items.articul", "items.model_id", "items.color_id",
                    "items.order_type", "colors.name", "matcolors.photo", "color_groups.name", "color_groups.rgb",
                    "size_value");
        }

        private Query _createBarcodeQuery() {
            return new Query("1c_barcodes AS inner_barcodes")
                .Select("inner_barcodes.model", "inner_barcodes.color", "inner_barcodes.razmer")
                .SelectRaw("GROUP_CONCAT(`barcode` SEPARATOR ',') AS `barcodes`")
                .GroupBy("inner_barcodes.model", "inner_barcodes.color", "inner_barcodes.razmer");
        }
    }
}