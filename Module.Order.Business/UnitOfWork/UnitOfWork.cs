using System;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.CacheRepositories.General;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Interfaces.Repositories;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using KristaShop.DataAccess.Repositories;
using KristaShop.DataAccess.Repositories.General;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace Module.Order.Business.UnitOfWork {
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork {
        private readonly Lazy<ICartRepository> _cartsRepository;
        private readonly Lazy<IManagerAccessRepository<ManagerAccess, Guid>> _managerAccessRepository;
        private readonly Lazy<IRepository<ManagerDetails, int>> _managerDetailsRepository;
        private readonly Lazy<IUserRepository> _usersRepository;
        private readonly Lazy<ICatalogItemRepository> _catalogItemsRepository;
        private readonly Lazy<IRequestItemsRepository> _requestItemsRepository;
        private readonly Lazy<IManufactureItemsRepository> _manufactureItemsRepository;
        private readonly Lazy<IReservationItemsRepository> _reservationItemsRepository;
        private readonly Lazy<IShipmentRepository> _shipmentRepository;
        private readonly Lazy<IOrderRepository> _orderRepository;
        private readonly Lazy<IOrdersHistoryRepository> _ordersHistoryRepository;
        private readonly Lazy<IMoneyDocumentsRepository> _moneyDocumentsRepository;
        private readonly Lazy<IMoneyDocumentsTotalsRepository> _moneyDocumentsTotalsRepository;
        private readonly Lazy<IOrderTotalsRepository> _orderTotalsRepository;
        private readonly Lazy<ICollectionsRepository> _collectionsRepository;

        public ICartRepository Carts => _cartsRepository.Value;
        public IManagerAccessRepository<ManagerAccess, Guid> ManagerAccess => _managerAccessRepository.Value;
        public IRepository<ManagerDetails, int> ManagerDetails => _managerDetailsRepository.Value;
        public IUserRepository Users => _usersRepository.Value;
        public ICatalogItemRepository CatalogItems => _catalogItemsRepository.Value;
        public IRequestItemsRepository RequestItems => _requestItemsRepository.Value;
        public IManufactureItemsRepository ManufactureItems => _manufactureItemsRepository.Value;
        public IReservationItemsRepository ReservationItems => _reservationItemsRepository.Value;
        public IShipmentRepository Shipments => _shipmentRepository.Value;
        public IOrderRepository Orders => _orderRepository.Value;
        public IOrdersHistoryRepository OrdersHistory => _ordersHistoryRepository.Value;
        public IMoneyDocumentsRepository MoneyDocuments => _moneyDocumentsRepository.Value;
        public IMoneyDocumentsTotalsRepository MoneyDocumentsTotals => _moneyDocumentsTotalsRepository.Value;
        public IOrderTotalsRepository OrderTotals => _orderTotalsRepository.Value;
        public ICollectionsRepository Collections => _collectionsRepository.Value;

        public UnitOfWork(KristaShopDbContext context, IMemoryCache memoryCache, ILogger logger) : base(context, logger) {
            _cartsRepository = new Lazy<ICartRepository>(() => new CartRepository(context));
            _managerAccessRepository = new Lazy<IManagerAccessRepository<ManagerAccess, Guid>>(() => new ManagerAccessRepository(context));
            _managerDetailsRepository = new Lazy<IRepository<ManagerDetails, int>>(() => new Repository<ManagerDetails, int>(context));
            _usersRepository = new Lazy<IUserRepository>(() => new UserCacheRepository(memoryCache, new UserRepository(context)));
            _catalogItemsRepository = new Lazy<ICatalogItemRepository>(() => new CatalogItemRepository(context));
            _requestItemsRepository = new Lazy<IRequestItemsRepository>(() => new RequestItemsRepository(Context));
            _manufactureItemsRepository = new Lazy<IManufactureItemsRepository>(() => new ManufactureItemsRepository(Context));
            _reservationItemsRepository = new Lazy<IReservationItemsRepository>(() => new ReservationItemsRepository(Context));
            _shipmentRepository = new Lazy<IShipmentRepository>(() => new ShipmentRepository(Context));
            _orderRepository = new Lazy<IOrderRepository>(() => new OrderRepository(Context));
            _ordersHistoryRepository = new Lazy<IOrdersHistoryRepository>(() => new OrdersHistoryRepository(Context));
            _moneyDocumentsRepository = new Lazy<IMoneyDocumentsRepository>(() => new MoneyDocumentsRepository(Context));
            _moneyDocumentsTotalsRepository = new Lazy<IMoneyDocumentsTotalsRepository>(() => new MoneyDocumentsTotalsRepository(Context));
            _orderTotalsRepository = new Lazy<IOrderTotalsRepository>(() => new OrderTotalsRepository(Context));
            _collectionsRepository = new Lazy<ICollectionsRepository>(() => new CollectionRepository(Context));
        }
    }
}