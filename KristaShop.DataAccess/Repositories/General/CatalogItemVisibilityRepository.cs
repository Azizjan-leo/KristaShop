using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.General {
    public class CatalogItemVisibilityRepository : Repository<CatalogItemVisibility, Guid>, ICatalogItemVisibilityRepository {
        public CatalogItemVisibilityRepository(DbContext context) : base(context) { }

        public async Task<CatalogItemVisibility> CreateOrUpdateAsync(string articul, int modelId, SizeValue size, int colorId, CatalogType catalogId, bool isVisible) {
            var item = await All.Where(x => x.ModelId.Equals(modelId) && x.SizeValue.Equals(size.Value) && x.ColorId.Equals(colorId))
                .FirstOrDefaultAsync();
            if (item == null) {
                item = new CatalogItemVisibility( articul, modelId, size, colorId, catalogId, isVisible);
                await AddAsync(item, true);
            } else {
                item.IsVisible = isVisible;
                Update(item);
            }

            return item;
        }
    }
}