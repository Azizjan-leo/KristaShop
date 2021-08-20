using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Module.Media.Business.DTOs;

namespace Module.Media.Business.Interfaces {
    public interface IDynamicPagesService {
        Task<List<DynamicPageDTO>> GetPagesAsync(bool openOnly = false, bool visibleInMenuOnly = false);
        Task<List<DynamicPageDTO>> GetPageByControllerAsync(string controller, bool openOnly = false,
            bool visibleInMenuOnly = false);
        Task<List<DynamicPageDTO>> GetPageByUrlsAsync(List<string> urls);
        Task<DynamicPageDTO> GetPageByIdAsync(Guid id);
        Task<DynamicPageDTO> GetPageByUrlAsync(string url);
        Task<OperationResult> InsertPageAsync(DynamicPageDTO dynamicPage);
        Task<OperationResult> UpdatePageAsync(DynamicPageDTO dynamicPage);
        Task<OperationResult> DeletePageAsync(Guid id, string fileDirectoryPath);
        Task<OperationResult> UpdatePageOrderAsync(Guid id, int newOrder);
        Task<OperationResult> RestorePageOrderAsync();
    }
}