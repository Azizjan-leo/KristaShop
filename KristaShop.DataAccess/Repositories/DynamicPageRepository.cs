using System;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.Media;
using KristaShop.DataAccess.Interfaces.Repositories.Media;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories {
    public class DynamicPageRepository : Repository<DynamicPage, Guid>, IDynamicPageRepository<DynamicPage, Guid> {
        public DynamicPageRepository(DbContext context) : base(context) { }

        public override IOrderedQueryable<DynamicPage> AllOrdered => All.OrderBy(x => x.Order);

        public IQueryable<DynamicPage> GetAllOrderedOpenAndVisibleInMenuOnly(bool openOnly, bool visibleInMenuOnly) {
            var request = visibleInMenuOnly ? AllOrdered.Where(x => x.IsVisibleInMenu) : AllOrdered;
            return openOnly ? request.Where(x => x.IsOpen) : request;
        }

        public async Task<int> GetNewOrderValueAsync() {
            return await All.MaxAsync(x => (int?) x.Order) + 1 ?? 1;
        }
    }
}
