using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.General {
    public class CollectionRepository : Repository<Collection, int>, ICollectionsRepository {
        public CollectionRepository(DbContext context) : base(context) { }
        
        public async Task<Collection> GetCurrentCollectionAsync() {
            var collection = await All.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return collection ?? Collection.Default;
        }
    }
}