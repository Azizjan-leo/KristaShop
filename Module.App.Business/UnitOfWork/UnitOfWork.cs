using System;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.CacheRepositories;
using KristaShop.DataAccess.CacheRepositories.General;
using KristaShop.DataAccess.CacheRepositories.Partners;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Interfaces.Repositories;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using KristaShop.DataAccess.Interfaces.Repositories.Partners;
using KristaShop.DataAccess.Repositories;
using KristaShop.DataAccess.Repositories.General;
using KristaShop.DataAccess.Repositories.Partners;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace Module.App.Business.UnitOfWork {
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork {
        private readonly Lazy<IRoleAccessRepository> _roleAccessesRepository;
        private readonly Lazy<IRepository<Role, Guid>> _rolesRepository;
        private readonly Lazy<IModelCatalogOrderRepository> _modelCatalogOrderRepository;
        private readonly Lazy<IRepository<Feedback, Guid>> _feedbackRepository;
        private readonly Lazy<IRepository<FeedbackFile, Guid>> _feedbackFilesRepository;
        private readonly Lazy<IRepository<MenuItem, Guid>> _menuItemsRepository;
        private readonly Lazy<IRepository<Manager, int>> _managersRepository;
        private readonly Lazy<IRepository<ManagerDetails, int>> _managerDetailsRepository;
        private readonly Lazy<IManagerAccessRepository<ManagerAccess, Guid>> _managerAccessRepository;

        public IRoleAccessRepository RoleAccesses => _roleAccessesRepository.Value;
        public IRepository<Role, Guid> Roles => _rolesRepository.Value;
        public IModelCatalogOrderRepository ModelCatalogOrder => _modelCatalogOrderRepository.Value;
        public IRepository<Feedback, Guid> Feedback => _feedbackRepository.Value;
        public IRepository<FeedbackFile, Guid> FeedbackFiles => _feedbackFilesRepository.Value;
        public IRepository<MenuItem, Guid> MenuItems => _menuItemsRepository.Value;
        public IRepository<Manager, int> Managers => _managersRepository.Value;
        public IRepository<ManagerDetails, int> ManagerDetails => _managerDetailsRepository.Value;
        public IManagerAccessRepository<ManagerAccess, Guid> ManagerAccess => _managerAccessRepository.Value;
        
         public UnitOfWork(KristaShopDbContext context, IMemoryCache memoryCache, ILogger logger) : base(context, logger) {
            _roleAccessesRepository = new Lazy<IRoleAccessRepository>(() => new RoleAccessCacheRepository(memoryCache, new RoleAccessRepository(context)));
            _rolesRepository = new Lazy<IRepository<Role, Guid>>(() => new Repository<Role, Guid>(context));
            _modelCatalogOrderRepository = new Lazy<IModelCatalogOrderRepository>(() => new ModelCatalogOrderRepository(context));
            _feedbackRepository = new Lazy<IRepository<Feedback, Guid>>(() => new Repository<Feedback, Guid>(context));
            _feedbackFilesRepository = new Lazy<IRepository<FeedbackFile, Guid>>(() => new Repository<FeedbackFile, Guid>(context));
            _menuItemsRepository = new Lazy<IRepository<MenuItem, Guid>>(() => new Repository<MenuItem, Guid>(context));
            _managersRepository = new Lazy<IRepository<Manager, int>>(() => new Repository<Manager, int>(context));
            _managerDetailsRepository = new Lazy<IRepository<ManagerDetails, int>>(() => new Repository<ManagerDetails, int>(context));
            _managerAccessRepository = new Lazy<IManagerAccessRepository<ManagerAccess, Guid>>(() => new ManagerAccessRepository(context));
         }
    }
}