using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;

namespace Module.Common.Business.Interfaces {
    public interface ICityService {
        Task<List<LookUpItem<int, string>>> GetCitiesLookupListAsync();
    }
}
