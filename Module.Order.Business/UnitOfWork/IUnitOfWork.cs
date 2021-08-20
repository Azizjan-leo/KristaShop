using System;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Interfaces.Repositories;
using KristaShop.DataAccess.Interfaces.Repositories.General;

namespace Module.Order.Business.UnitOfWork {
    public interface IUnitOfWork : IUnitOfWorkBase {
        ICartRepository Carts { get; }
        IManagerAccessRepository<ManagerAccess, Guid> ManagerAccess { get; }
        IRepository<ManagerDetails, int> ManagerDetails { get; }
        IUserRepository Users { get; }
        ICatalogItemRepository CatalogItems { get; }
        IRequestItemsRepository RequestItems { get; }
        IManufactureItemsRepository ManufactureItems { get; }
        IReservationItemsRepository ReservationItems { get; }
        IShipmentRepository Shipments { get; }
        IOrderRepository Orders { get; }
        IOrdersHistoryRepository OrdersHistory { get; }
        IMoneyDocumentsRepository MoneyDocuments { get; }
        IMoneyDocumentsTotalsRepository MoneyDocumentsTotals { get; }
        IOrderTotalsRepository OrderTotals { get; }
        ICollectionsRepository Collections { get; }

    }
}