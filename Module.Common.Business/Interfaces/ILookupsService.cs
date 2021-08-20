using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;

namespace Module.Common.Business.Interfaces {
    public interface ILookupsService {
        Task<List<LookUpItem<int, string>>> GetColorsLookupListAsync();
        Task<IReadOnlyList<string>> GetSizesListAsync();
        Task<IReadOnlyList<string>> GetArticulsListAsync();
        Task<IReadOnlyList<LookUpItem<int, string>>> GetManagersLookupListAsync();
    }
}
