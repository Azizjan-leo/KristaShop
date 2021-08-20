using System.Collections.Generic;
using System.Linq;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Configurations.DataFrom1C;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Interfaces.Repositories {
    public interface IBarcodeRepository : IRepository<Barcode, int> {
        IQueryable<BarcodeSqlView> GetBarcodes();
        IQueryable<BarcodeSqlView> GetBarcodes(IEnumerable<string> barcodes);
        IQueryable<BarcodeSqlView> GetBarcodes(IEnumerable<int> modelIds);
    }
}
