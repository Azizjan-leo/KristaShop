using System;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Interfaces.Repositories;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using KristaShop.DataAccess.Interfaces.Repositories.Partners;

namespace Module.Client.Business.UnitOfWork {
    public interface IUnitOfWork : IUnitOfWorkBase {
        INewUsersRepository NewUsers { get; }
        IUserRepository Users { get; }
        ICitiesRepository Cities { get; }
        ICartRepository Carts { get; }
        IRepository<NewUsersCounter, Guid> NewUsersCounter { get; }
        IRepository<UserNewPassword, int> UserNewPasswords { get; }
        IRepository<UserData, int> UserData { get; }
        IRepository<AuthorizationLink, Guid> AuthorizationLinks { get; }
        IManagerAccessRepository<ManagerAccess, Guid> ManagerAccess { get; }
        IRepository<ManagerDetails, int> ManagerDetails { get; }
        IPartnersRepository Partners { get; }
    }
}