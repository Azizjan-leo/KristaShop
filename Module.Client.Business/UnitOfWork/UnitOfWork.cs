using System;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Interfaces.DataAccess;
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

namespace Module.Client.Business.UnitOfWork {
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork {
        private readonly Lazy<INewUsersRepository> _newUsersRepository;
        private readonly Lazy<IUserRepository> _usersRepository;
        private readonly Lazy<ICitiesRepository> _citiesRepository;
        private readonly Lazy<ICartRepository> _cartsRepository;
        private readonly Lazy<IRepository<NewUsersCounter, Guid>> _newUsersCounterRepository;
        private readonly Lazy<IRepository<UserNewPassword, int>> _userNewPasswordsRepository;
        private readonly Lazy<IRepository<UserData, int>> _userDataRepository;
        private readonly Lazy<IRepository<AuthorizationLink, Guid>> _authorizationLinksRepository;
        private readonly Lazy<IManagerAccessRepository<ManagerAccess, Guid>> _managerAccessRepository;
        private readonly Lazy<IRepository<ManagerDetails, int>> _managerDetailsRepository;
        private readonly Lazy<IPartnersRepository> _partnersRepository;

        public INewUsersRepository NewUsers => _newUsersRepository.Value;
        public IUserRepository Users => _usersRepository.Value;
        public ICitiesRepository Cities => _citiesRepository.Value;
        public ICartRepository Carts => _cartsRepository.Value;
        public IRepository<NewUsersCounter, Guid> NewUsersCounter => _newUsersCounterRepository.Value;
        public IRepository<UserNewPassword, int> UserNewPasswords => _userNewPasswordsRepository.Value;
        public IRepository<UserData, int> UserData => _userDataRepository.Value;
        public IRepository<AuthorizationLink, Guid> AuthorizationLinks => _authorizationLinksRepository.Value;
        public IManagerAccessRepository<ManagerAccess, Guid> ManagerAccess => _managerAccessRepository.Value;
        public IRepository<ManagerDetails, int> ManagerDetails => _managerDetailsRepository.Value;
        public IPartnersRepository Partners => _partnersRepository.Value;


        public UnitOfWork(KristaShopDbContext context, IMemoryCache memoryCache, ILogger logger) : base(context, logger) {
            _newUsersRepository = new Lazy<INewUsersRepository>(() => new NewUsersRepository(context));
            _usersRepository = new Lazy<IUserRepository>(() => new UserCacheRepository(memoryCache, new UserRepository(context)));
            _citiesRepository = new Lazy<ICitiesRepository>(() => new CitiesRepository(context));
            _cartsRepository = new Lazy<ICartRepository>(() => new CartRepository(context));
            _newUsersCounterRepository = new Lazy<IRepository<NewUsersCounter, Guid>>(() => new Repository<NewUsersCounter, Guid>(context));
            _userNewPasswordsRepository = new Lazy<IRepository<UserNewPassword, int>>(() => new Repository<UserNewPassword, int>(context));
            _userDataRepository = new Lazy<IRepository<UserData, int>>(() => new Repository<UserData, int>(context));
            _authorizationLinksRepository = new Lazy<IRepository<AuthorizationLink, Guid>>(() => new Repository<AuthorizationLink, Guid>(context));
            _managerAccessRepository = new Lazy<IManagerAccessRepository<ManagerAccess, Guid>>(() => new ManagerAccessRepository(context));
            _managerDetailsRepository = new Lazy<IRepository<ManagerDetails, int>>(() => new Repository<ManagerDetails, int>(context));
            _partnersRepository = new Lazy<IPartnersRepository>(() => new PartnersCacheRepository(memoryCache, new PartnersRepository(context)));
        }
    }
}