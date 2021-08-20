using System;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.General {
    public class ModelCatalogOrderRepository : Repository<ModelCatalogOrder, object>, IModelCatalogOrderRepository {
        public ModelCatalogOrderRepository(DbContext context) : base(context) {
        }

        public async Task CheckAllModels() {
            var lackEntities = (await Context.Set<CatalogItem>()
                .Where(x => x.ModelCatalogOrder == null)
                .Select(x => new ModelCatalogOrder(x.Articul, x.CatalogId))
                .Distinct()
                .ToListAsync())
                .GroupBy(x => x.CatalogId)
                .ToList();

            if (!lackEntities.Any()) {
                return;
            }

            var maxOrders = await Context.Set<ModelCatalogOrder>()
                .Where(x => lackEntities.Select(e => e.Key).Contains(x.CatalogId))
                .GroupBy(x => x.CatalogId)
                .Select(x => new Tuple<CatalogType, int>(x.Key, x.Max(c => c.Order)))
                .ToListAsync();

            foreach (var group in lackEntities) {
                var catalogMaxOrder = maxOrders.FirstOrDefault(x => x.Item1 == group.Key)?.Item2 ?? 0;

                foreach (var item in group) {
                    item.Order = ++catalogMaxOrder;
                }
                await AddRangeAsync(group.ToList());
            }
        }

        public async Task<ModelCatalogOrder> CreateOrUpdateAsync(string articul, CatalogType catalogId, int position) {
            var entity = await Context.Set<ModelCatalogOrder>().FindAsync(articul, catalogId);
            if (entity != null) {
                entity.Order = position;
                Update(entity);
                return entity;
            }

            entity = new ModelCatalogOrder {
                Articul = articul,
                CatalogId = catalogId,
                Order = position
            };
            await AddAsync(entity);
            return entity;
        }

    }
}