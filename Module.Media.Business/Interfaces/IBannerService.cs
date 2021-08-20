using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Module.Media.Business.DTOs;

namespace Module.Media.Business.Interfaces {
    public interface IBannerService {
        Task<OperationResult> SwitchBannerVisabilityAsync(Guid id);
        Task<List<BannerItemDTO>> GetBannersAsync(bool visibleOnly = false);
        Task<BannerItemDTO> GetBannerAsync(Guid id);
        Task<BannerItemDTO> GetBannerNoTrackAsync(Guid id);
        Task<OperationResult> InsertBannerAsync(BannerItemDTO banner);
        Task<OperationResult> UpdateBannerAsync(BannerItemDTO banner);
        Task<OperationResult> DeleteBanner(Guid id, string fileDirectoryPath);
    }
}