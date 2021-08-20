using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface ICatalogItemDescriptorRepository : IRepository<CatalogItemDescriptor, string> {
        Task<Dictionary<string, CatalogItemDescriptor>> GetByArticulsAsDictionaryAsync(IEnumerable<string> articuls);
        Task UpdateDescriptorPhotoAsync(string articul, string path);
    }
}