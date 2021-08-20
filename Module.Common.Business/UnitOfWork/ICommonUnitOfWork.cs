using System;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories.General;

namespace Module.Common.Business.UnitOfWork {
    public interface ICommonUnitOfWork : IUnitOfWorkBase {
        IRepository<Settings, Guid> Settings { get; } 
        IModelsRepository Models { get; }
        IRepository<Color, int> Colors { get; }
        IRepository<Manager, int> Managers { get; }
        ICollectionsRepository Collections { get; }
        ICitiesRepository Cities { get; }
    }
}