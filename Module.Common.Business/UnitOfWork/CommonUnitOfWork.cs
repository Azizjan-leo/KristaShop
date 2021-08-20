using System;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.CacheRepositories.General;
using KristaShop.DataAccess.Domain;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using KristaShop.DataAccess.Repositories.General;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace Module.Common.Business.UnitOfWork {
    public class CommonUnitOfWork : UnitOfWorkBase, ICommonUnitOfWork {
        private readonly Lazy<IRepository<Settings, Guid>> _settingsRepository;
        private readonly Lazy<IModelsRepository> _modelsRepository;
        private readonly Lazy<IRepository<Color, int>> _colorsRepository;
        private readonly Lazy<IRepository<Manager, int>> _managersRepository;
        private readonly Lazy<ICollectionsRepository> _collectionsRepository;
        private readonly Lazy<ICitiesRepository> _citiesRepository;

        public IRepository<Settings, Guid> Settings => _settingsRepository.Value;
        public IModelsRepository Models => _modelsRepository.Value;
        public IRepository<Color, int> Colors => _colorsRepository.Value;
        public IRepository<Manager, int> Managers => _managersRepository.Value;
        public ICollectionsRepository Collections => _collectionsRepository.Value;
        public ICitiesRepository Cities => _citiesRepository.Value;

        public CommonUnitOfWork(KristaShopDbContext context, IMemoryCache memoryCache, ILogger logger) : base(context, logger) {
            _settingsRepository = new Lazy<IRepository<Settings, Guid>>(() => new Repository<Settings, Guid>(context));
            _modelsRepository = new Lazy<IModelsRepository>(() => new ModelsCacheRepository(memoryCache, new ModelsRepository(context)));
            _colorsRepository = new Lazy<IRepository<Color, int>>(() => new Repository<Color, int>(context));
            _managersRepository = new Lazy<IRepository<Manager, int>>(() => new Repository<Manager, int>(context));
            _collectionsRepository = new Lazy<ICollectionsRepository>(() => new CollectionRepository(context));
            _citiesRepository = new Lazy<ICitiesRepository>(() => new CitiesRepository(context));
        }
    }
}