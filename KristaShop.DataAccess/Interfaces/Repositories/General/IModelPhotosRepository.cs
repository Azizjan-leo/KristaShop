using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface IModelPhotosRepository : IRepository<ModelPhoto1C, int> {
        Task<IEnumerable<ModelPhoto1C>> GetAllAsync(IEnumerable<string> articuls);
        Task<IEnumerable<ModelPhoto1C>> GetAllByArticulAsync(string articul);
        Task<int> GetMaxOrderNumberAsync(string articul);
        Task UpdatePhotoPosition(int id, int newPosition);
        Task UpdatePhotoColor(int id, int colorId);
        Task<string> UpdatePhotoPath(int id, string path);
    }
}