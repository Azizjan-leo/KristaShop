using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Interfaces.Repositories {
    public interface IShipmentRepository : IRepository<Shipment, int> {
        Task<IEnumerable<ShipmentsSqlView>> GetShipments();
        Task<IEnumerable<ShipmentsSqlView>> GetItemsByUserAsync(int userId, DateTime shipmentDate = default);

        Task<IEnumerable<BarcodeShipmentsSqlView>> GetNotIncomedItemsWithBarcodes(int userId, DateTime shipmentDate = default);
        Task<IEnumerable<ShipmentsSqlView>> GetItemsByUserForLastMonthsAsync(int userId, int months);
    }
}
