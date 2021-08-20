using System;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.CacheRepositories.General;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using KristaShop.DataAccess.Repositories.General;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace Module.Catalogs.Business.UnitOfWork {
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork {
        private readonly Lazy<IUserRepository> _userRepository;
        private readonly Lazy<ICatalogRepository> _catalogRepository;
        private readonly Lazy<ICatalogItemRepository> _catalogItemsRepository;
        private readonly Lazy<IModelPhotosRepository> _modelPhotosRepository;
        private readonly Lazy<IModelCatalogInvisibilitiesRepository> _modelCatalogInvisibilitiesRepository;
        private readonly Lazy<ICatalogItemVisibilityRepository> _catalogItemVisibilityRepository;
        private readonly Lazy<ICatalogItemDescriptorRepository> _catalogItemDescriptorRepository;
        private readonly Lazy<IModelCatalogOrderRepository> _modelCatalogOrderRepository;
        private readonly Lazy<IRepository<CatalogExtraCharge, Guid>> _catalogExtraChargesRepository;
        private readonly Lazy<ICategoriesRepository> _categoriesRepository;

        public IUserRepository Users => _userRepository.Value;
        public ICatalogRepository Catalogs => _catalogRepository.Value;
        public ICatalogItemRepository CatalogItems => _catalogItemsRepository.Value;
        public IModelPhotosRepository ModelPhotos => _modelPhotosRepository.Value;
        public IModelCatalogInvisibilitiesRepository ModelCatalogInvisibilities => _modelCatalogInvisibilitiesRepository.Value;
        public ICatalogItemVisibilityRepository CatalogItemsVisibility => _catalogItemVisibilityRepository.Value;
        public ICatalogItemDescriptorRepository CatalogItemDescriptors => _catalogItemDescriptorRepository.Value;
        public IModelCatalogOrderRepository ModelCatalogOrder => _modelCatalogOrderRepository.Value;
        public IRepository<CatalogExtraCharge, Guid> CatalogExtraCharges => _catalogExtraChargesRepository.Value;
        public ICategoriesRepository Categories => _categoriesRepository.Value;

        public UnitOfWork(KristaShopDbContext context, IMemoryCache memoryCache, ILogger logger) : base(context, logger) {
            _userRepository = new Lazy<IUserRepository>(() => new UserCacheRepository(memoryCache, new UserRepository(context)));
            _catalogRepository = new Lazy<ICatalogRepository>(() => new CatalogRepository(context));
            _catalogItemsRepository = new Lazy<ICatalogItemRepository>(() => new CatalogItemRepository(context));
            _modelPhotosRepository = new Lazy<IModelPhotosRepository>(() => new ModelPhotosRepository(context));
            _catalogExtraChargesRepository = new Lazy<IRepository<CatalogExtraCharge, Guid>>(() => new Repository<CatalogExtraCharge, Guid>(context));
            _catalogItemVisibilityRepository = new Lazy<ICatalogItemVisibilityRepository>(() => new CatalogItemVisibilityRepository(context));
            _modelCatalogInvisibilitiesRepository = new Lazy<IModelCatalogInvisibilitiesRepository>(() => new ModelCatalogInvisibilitiesRepository(context));
            _modelCatalogOrderRepository = new Lazy<IModelCatalogOrderRepository>(() => new ModelCatalogOrderRepository(context));
            _catalogItemDescriptorRepository = new Lazy<ICatalogItemDescriptorRepository>(() => new CatalogItemDescriptorRepository(context));
            _categoriesRepository = new Lazy<ICategoriesRepository>(() => new CategoriesRepository(context));
        }
       
    }
}