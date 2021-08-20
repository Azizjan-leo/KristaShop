using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Enums;
using Module.Catalogs.Business.Models;

namespace Module.Catalogs.Business.Interfaces {
    public interface ICatalogItemUpdateService {
        Task UpdateModelDescriptionAsync(UpdateCatalogItemDescriptorDTO descriptor);
        Task UpdateModelPhotoPositionAsync(int id, int newPosition);
        Task UpdateModelMainPhotoAsync(string articul, string path);
        Task ReorderModelPhotosAsync(string articul, int photoId, int newPosition);
        Task SetPhotoColorAsync(int photoId, int colorId);
        Task<string> UpdateModelPhotoAsync(int photoId, string path);
        Task UpdateModelPositionInCatalogAsync(CatalogType catalog, string id, int toPosition);
        Task<List<string>> DeleteModelPhotoAsync(int photoId);
    }
}