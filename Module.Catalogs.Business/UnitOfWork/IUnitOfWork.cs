using System;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Interfaces.Repositories.General;

namespace Module.Catalogs.Business.UnitOfWork {
    public interface IUnitOfWork : IUnitOfWorkBase {
        IUserRepository Users { get; }
        ICatalogRepository Catalogs { get; }
        ICatalogItemRepository CatalogItems { get; }
        IModelPhotosRepository ModelPhotos { get; }
        IModelCatalogInvisibilitiesRepository ModelCatalogInvisibilities { get; }
        ICatalogItemVisibilityRepository CatalogItemsVisibility { get; }
        ICatalogItemDescriptorRepository CatalogItemDescriptors { get; }
        IModelCatalogOrderRepository ModelCatalogOrder {get; }
        IRepository<CatalogExtraCharge, Guid> CatalogExtraCharges { get; }
        ICategoriesRepository Categories { get; }
    }
}