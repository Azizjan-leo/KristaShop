using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface IModelsRepository : IRepository<Model, int> {
        Task<IReadOnlyList<string>> GetAllSizesAsync();
        Task<IReadOnlyList<string>> GetAllArticulsAsync();
    }
}