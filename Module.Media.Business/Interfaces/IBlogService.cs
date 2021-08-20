using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Module.Media.Business.DTOs;
using P.Pager;

namespace Module.Media.Business.Interfaces {
    public interface IBlogService {
        Task<OperationResult> SwitchBlogVisabilityAsync(Guid id);
        Task<List<BlogItemDTO>> GetBlogsAsync();
        Task<IPager<BlogItemDTO>> GetBlogsForPageAsync(int page, int modelInPage);
        Task<List<BlogItemDTO>> GetTopBlogsAsync(int count);
        Task<BlogItemDTO> GetBlogAsync(Guid id);
        Task<BlogItemDTO> GetBlogNoTrackAsync(Guid id);
        Task<OperationResult> InsertBlogAsync(BlogItemDTO blogItem);
        Task<OperationResult> UpdateBlogAsync(BlogItemDTO blogItem);
        Task<OperationResult> DeleteBlogAsync(Guid id);
    }
}