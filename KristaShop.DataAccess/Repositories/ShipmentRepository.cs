using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories {
    public class ShipmentRepository : Repository<Shipment, int>, IShipmentRepository {
        private readonly MySqlCompiler _compiler;

        public ShipmentRepository(DbContext context) : base(context) {
            _compiler = new MySqlCompiler();
        }
        
        public async Task<IEnumerable<ShipmentsSqlView>> GetShipments() {
            var query = All.Include(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Model).ThenInclude(x => x.Barcodes)
                .Include(x => x.Model).ThenInclude(x => x.Collection)
                .Include(x => x.Color).ThenInclude(x => x.Group);

            
            return await _selectSqlView(query).ToListAsync();
        }

        public async Task<IEnumerable<ShipmentsSqlView>> GetItemsByUserAsync(int userId, DateTime shipmentDate = default) {
            var query = All.Include(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Model).ThenInclude(x => x.Barcodes)
                .Include(x => x.Model).ThenInclude(x => x.Collection)
                .Include(x => x.Color).ThenInclude(x => x.Group)
                .Where(x => x.UserId == userId);

            if (shipmentDate != default) {
                query = query.Where(x => x.ShipmentDate == shipmentDate);
            }
            
            return await _selectSqlView(query).ToListAsync();
        }

        public async Task<IEnumerable<ShipmentsSqlView>> GetItemsByUserForLastMonthsAsync(int userId, int months) {
            var query = All.Include(x => x.Model).ThenInclude(x => x.Descriptor)
                .Include(x => x.Model).ThenInclude(x => x.Barcodes)
                .Include(x => x.Model).ThenInclude(x => x.Collection)
                .Include(x => x.Color).ThenInclude(x => x.Group)
                .Where(x => x.UserId == userId && x.ShipmentDate > DateTime.Now.AddMonths(-months));

            return await _selectSqlView(query).ToListAsync();
        }
        
        public async Task<IEnumerable<BarcodeShipmentsSqlView>> GetNotIncomedItemsWithBarcodes(int userId, DateTime shipmentDate = default) {
            var query = _createShipmentsWithBarcodesQuery()
                .LeftJoin("part_documents AS documents", x => x.WhereRaw("`shipments`.`klient` = `documents`.`user_id` AND `shipments`.`datav` = DATE(`documents`.`execution_date`)"))
                .Join("partners AS partner", x => x.WhereRaw("`shipments`.`klient` = `partner`.`user_id` AND `shipments`.`datav` >= DATE(`partner`.`date_approved`)"))
                .WhereNull("documents.id")
                .Where("shipments.klient", userId);
            query = _createQueryShipmentsQueryByDate(query, shipmentDate);

            var compiledSql = _compiler.Compile(query);
            return await Context.Set<BarcodeShipmentsSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray()).ToListAsync();
        }

        private IQueryable<ShipmentsSqlView> _selectSqlView(IQueryable<Shipment> shipments) {
            return shipments.Select(x => new ShipmentsSqlView {
                Id = x.Id,
                UserId = x.UserId,
                Articul = x.Model.Articul,
                Size = new SizeValue(x.SizeValue, x.Model.SizeLine),
                MainPhoto = x.Model.Descriptor.MainPhoto,
                ColorId = x.ColorId,
                ColorName = x.Color.Name,
                ColorPhoto = "",
                ColorValue = x.Color.Group.Hex ?? "",
                Amount = x.Amount,
                PartsCount = x.Model.Parts,
                SaleDate = x.ShipmentDate,
                Price = x.Price,
                PriceInRub = x.PriceInRub,
                CollectionId = x.CollectionId ?? 0,
                CollectionName = x.Collection.Name ?? "Коллекция №1",
                CollectionPrepayPercent = x.Collection != null
                    ? x.Collection.PercentValue
                    : GlobalConstant.GeneralPrepayPercentValue,
                DocumentsFolder = x.AttachmentsNumber.ToString()
            });
        }

        private Query _createShipmentsQuery() {
            return new Query("1c_prodagi_klientov AS shipments")
                .Select("shipments.id", "shipments.klient AS user_id", "models.articul", "shipments.model AS model_id",
                    "shipments.color AS color_id", "shipments.kolichestvo AS amount", "shipments.datav AS sale_date",
                    "shipments.cena", "shipments.cena_rub", "models.razmerov AS parts_count", "models.name AS name",
                    "colors.name AS color_name", "matcolors.photo AS color_photo", "color_groups.name AS color_group_name",
                    "color_groups.rgb AS color_group_rgb_value", "photos.photo_path AS photo_by_color", "item_descriptor.main_photo")
                .SelectRaw("IF(`shipments`.`razmer` IS NULL OR `shipments`.`razmer` = '', `models`.`line`, TRIM(`shipments`.`razmer`)) AS `size_value`")
                .SelectRaw("CAST(shipments.schet AS NCHAR) AS schet")
                .Join("1c_models AS models", "models.id", "shipments.model")
                .LeftJoin("1c_material AS materials", "materials.id", "models.material")
                .Join("1c_colors AS colors", "colors.id", "shipments.color")
                .LeftJoin("1c_materialcolors AS matcolors",
                    x => x.On("matcolors.color", "shipments.color").On("models.material", "matcolors.material"))
                .LeftJoin("1c_colors_group AS color_groups", "color_groups.id", "colors.group")
                .LeftJoin("catalog_item_descriptor AS item_descriptor", "models.articul", "item_descriptor.articul")
                .LeftJoin(GenericQueries.GetModelPhotoByColorInnerQuery().As("photos"), x => x.On("photos.articul", "models.articul").On("photos.color_id", "shipments.color"))
                .JoinModelCollection("shipments")
                .OrderBy("shipments.datav", "models.articul", "models.line", "size_value", "color_name");
        }

        private Query _createShipmentsWithBarcodesQuery() {
            var barcodesQuery = new Query("1c_barcodes AS inner_barcodes")
                .Select("inner_barcodes.model", "inner_barcodes.color", "inner_barcodes.razmer")
                .SelectRaw("GROUP_CONCAT(`barcode` SEPARATOR ',') AS `barcodes`")
                .GroupBy("inner_barcodes.model", "inner_barcodes.color", "inner_barcodes.razmer");
            
            return _createShipmentsQuery()
                .Select("barcodes.barcodes")
                .LeftJoin(barcodesQuery.As("barcodes"), x => x .On("barcodes.model", "shipments.model")
                    .On("barcodes.color", "shipments.color")
                    .On("barcodes.razmer", "shipments.razmer"))
                .GroupBy("shipments.id", "shipments.klient", "models.articul", "shipments.model", "shipments.color",
                    "shipments.kolichestvo", "shipments.datav", "shipments.cena", "shipments.cena_rub", "models.razmerov",
                    "colors.name", "matcolors.photo", "color_groups.name", "color_groups.rgb", "photos.photo_path",
                    "item_descriptor.main_photo");
        }
        
        private Query _createQueryShipmentsQueryByDate(Query query, DateTime shipmentDate) {
            return shipmentDate != default ? query.Where("shipments.datav", shipmentDate) : query;
        }
    }
}
