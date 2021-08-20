using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.General {
    public class CitiesRepository : Repository<City, int>, ICitiesRepository {
        private static readonly HashSet<string> _excludeCities = new() {"Поставщики", "Покупатели", "Реализаторы"};
        public CitiesRepository(DbContext context) : base(context) { }
        
        public override async Task<IEnumerable<City>> GetAllAsync() {
            var result = await All.OrderBy(x => x.Name).ToListAsync();
            result.RemoveAll(x => _excludeCities.Contains(x.Name));
            return result;
        }
    }
}