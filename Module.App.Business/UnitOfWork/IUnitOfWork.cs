using System;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFor1C;
using KristaShop.DataAccess.Interfaces.Repositories;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using KristaShop.DataAccess.Interfaces.Repositories.Partners;

namespace Module.App.Business.UnitOfWork {
    public interface IUnitOfWork : IUnitOfWorkBase {
         IRoleAccessRepository RoleAccesses { get; }
         IRepository<Role, Guid> Roles { get; }
         IModelCatalogOrderRepository ModelCatalogOrder { get; }
         IRepository<Feedback, Guid> Feedback { get; }
         IRepository<FeedbackFile, Guid> FeedbackFiles { get; }
         IRepository<MenuItem, Guid> MenuItems { get; }
         IRepository<Manager, int> Managers { get; }
         IRepository<ManagerDetails, int> ManagerDetails { get; }
         IManagerAccessRepository<ManagerAccess, Guid> ManagerAccess { get; }
    }
}