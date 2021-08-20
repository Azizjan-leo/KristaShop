using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Module.Catalogs.Business.Models;

namespace Module.Catalogs.Business.Interfaces {
    public interface ICategoryService {
        Task<List<CategoryDTO>> GetCategoriesAsync();
        Task<CategoryDTO> GetCategoryAsync(Guid id);
        Task<CategoryDTO> GetCategoryAsync(int id1C);
        Task<OperationResult> InsertCategoryAsync(CategoryDTO dto);
        Task<OperationResult> UpdateCategoryAsync(CategoryDTO dto);
        Task<OperationResult> DeleteCategory(Guid id);
        Task<IEnumerable<Category1CDTO>> GetAllCategoriesAsync();
    }
}