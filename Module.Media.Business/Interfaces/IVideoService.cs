using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Module.Media.Business.DTOs;
using P.Pager;

namespace Module.Media.Business.Interfaces {
    public interface IVideoService {
        Task<List<VideoDTO>> GetVideosAsync(bool onlyVisible = true);
        Task<IPager<VideoDTO>> GetVideosOnPageAsync(int page, int quantity, bool onlyVisible = true);
        Task<List<VideoDTO>> GetVideosByGalleryAsync(Guid galleryId, bool onlyVisible = true);
        Task<IPager<VideoDTO>> GetVideosByGalleryOnPageAsync(Guid galleryId, int page, int quantity = 20, bool onlyVisible = true);
        Task<VideoDTO> GetVideoAsync(Guid id, bool onlyVisible = true);
        Task<OperationResult> InsertVideoAsync(VideoDTO video);
        Task<OperationResult> UpdateVideoAsync(VideoDTO video);
        Task<OperationResult> DeleteVideoAsync(Guid id, string fileDirectoryPath);
        Task<OperationResult> UpdateVideoOrder(Guid galleryId, Guid videoId, int order);
        Task<OperationResult> RestoreVideosOrderAsync();
    }
}