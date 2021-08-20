using System;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface ICatalogItemVisibilityRepository : IRepository<CatalogItemVisibility, Guid> {
        Task<CatalogItemVisibility> CreateOrUpdateAsync(string articul, int modelId, SizeValue size, int colorId, CatalogType catalogId, bool isVisible);
    }
}