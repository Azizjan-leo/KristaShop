using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Module.Media.Business.DTOs;

namespace Module.Media.Business.Interfaces {
    public interface IVideoGalleryService : IVideoService {
        Task<List<VideoGalleryDTO>> GetGalleriesAsync(bool openOnly = false, bool onlyVisible = true);
        Task<VideoGalleryDTO> GetGalleryAsync(Guid id, bool openOnly = false, bool onlyVisible = true);
        Task<VideoGalleryDTO> GetGalleryAsync(string slug, bool openOnly = false, bool onlyVisible = true);
        Task<List<VideoGalleryWithVideosDTO>> GetGalleriesWithVideoAsync(int videosQuantity, bool openOnly = false, bool onlyVisible = true);
        Task<VideoGalleryWithVideosDTO> GetGalleryWithVideoAsync(Guid id, int videosQuantity, bool openOnly = false, bool onlyVisible = true);
        Task<VideoGalleryWithVideosDTO> GetGalleryWithVideoAsync(string slug, int videosQuantity, bool openOnly = false, bool onlyVisible = true);
        Task<VideoGalleryWithVideosDTO> GetFirstGalleryWithVideosAsync(int videosQuantity, bool openOnly = false, bool onlyVisible = true);
        Task<OperationResult> InsertGalleryAsync(VideoGalleryDTO gallery);
        Task<OperationResult> UpdateGalleryAsync(VideoGalleryDTO gallery);
        Task<OperationResult> DeleteGalleryAsync(Guid id, string filesDirectoryPath);
        Task<OperationResult> RestoreGalleriesOrderAsync();
        Task<OperationResult> SwitchGalleryVisabilityAsync(Guid id);
        Task<OperationResult> SwitchVideoVisabilityAsync(Guid id);
    }
}
