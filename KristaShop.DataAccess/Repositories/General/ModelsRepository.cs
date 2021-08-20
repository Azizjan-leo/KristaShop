using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Helpers;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.Common.Models.Structs;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.General {
    public class ModelsRepository : Repository<Model, int>, IModelsRepository {
        public ModelsRepository(DbContext context) : base(context) { }
        
        public async Task<IReadOnlyList<string>> GetAllSizesAsync() {
            var sizes = await All
                .Select(x => new SizeValue(x.SizeLine))
                .ToListAsync();

            return sizes.SelectMany(x => x.Values)
                .Distinct()
                .Concat(sizes.Select(x => x.Line).Distinct())
                .Distinct()
                .OrderBy(x => x, new SizeStringComparer())
                .ToList();
        }

        public async Task<IReadOnlyList<string>> GetAllArticulsAsync() {
            return await All.Select(x => x.Articul).Distinct().ToListAsync();
        }
    }
}