using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using KristaShop.Common.Models.Structs;
using Module.Common.Business.Models;

namespace Module.Common.Business.Interfaces {
    public interface ICollectionService {
        Task<PrepayValue> GetCurrentPrepayPercentAsync();
        Task<List<LookUpItem<int, string>>> GetCollectionsLookupListAsync();
        Task<CollectionDTO> GetLastCollectionAsync();
    }
}