using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Configurations.DataFrom1C;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories {
    public class BarcodeRepository : Repository<Barcode, int>, IBarcodeRepository {
        private readonly MySqlCompiler _compiler;

        public BarcodeRepository(DbContext context) : base(context) {
            _compiler = new MySqlCompiler();
        }

        public IQueryable<BarcodeSqlView> GetBarcodes() {
            var compiledSql = _compiler.Compile(_createBarcodesQuery());
            return Context.Set<BarcodeSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray());
        }

        public IQueryable<BarcodeSqlView> GetBarcodes(IEnumerable<string> barcodes) {
            var compiledSql = _compiler.Compile(_createBarcodes(barcodes));
            return Context.Set<BarcodeSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray());
        }

        public IQueryable<BarcodeSqlView> GetBarcodes(IEnumerable<int> modelIds) {
            var compiledSql = _compiler.Compile(_createBarcodes(modelIds));
            return Context.Set<BarcodeSqlView>().FromSqlRaw(compiledSql.Sql, compiledSql.Bindings.ToArray());
        }

        private Query _createBarcodesQuery() {
            return new Query("1c_barcodes AS barcodes")
                .Select("barcodes.id", "barcodes.model", "barcodes.color", "barcodes.barcode",
                    "models.articul", "models.price")
                .SelectRaw("IF(`barcodes`.`razmer` IS NULL OR `barcodes`.`razmer` = '', `models`.`line`, `barcodes`.`razmer`) AS `razmer`")
                .Join("1c_models AS models", "models.id", "barcodes.model");
        }

        private Query _createBarcodes(IEnumerable<string> barcodes) {
            return _createBarcodesQuery().WhereIn("barcodes.barcode", barcodes);
        }

        private Query _createBarcodes(IEnumerable<int> modelIds) {
            return _createBarcodesQuery().WhereIn("barcodes.model", modelIds);
        }
    }
}
