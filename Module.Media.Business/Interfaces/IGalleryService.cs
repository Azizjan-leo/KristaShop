using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Module.Media.Business.DTOs;
using P.Pager;

namespace Module.Media.Business.Interfaces {
    public interface IGalleryService {
        Task<OperationResult> SwitchGalleryVisabilityAsync(Guid id);
        Task<List<GalleryItemDTO>> GetGalleriesAsync();
        Task<IPager<GalleryItemDTO>> GetGalleryItemsForPageAsync(int page, int modelInPage);
        Task<List<GalleryItemDTO>> GetTopGalleryItemsAsync(int quantity);
        Task<List<GalleryItemDTO>> GetTopNRandomItemsAsync(int quantity);
        Task<GalleryItemDTO> GetGalleryItemAsync(Guid id);
        Task<GalleryItemDTO> GetGalleryItemNoTrackAsync(Guid id);
        Task<OperationResult> InsertGalleryAsync(GalleryItemDTO galleryItem);
        Task<OperationResult> UpdateGalleryAsync(GalleryItemDTO galleryItem);
        Task<OperationResult> DeleteGalleryAsync(Guid id);
    }
}