using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using KristaShop.Common.Models;
using KristaShop.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Module.Catalogs.Business.Interfaces;
using Module.Catalogs.Business.Models;
using Module.Catalogs.Business.UnitOfWork;

namespace Module.Catalogs.Business.Services {
    public class CategoryService : ICategoryService {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<CategoryDTO>> GetCategoriesAsync() {
            return await _uow.Categories.All
                .OrderBy(x => x.Order)
                .ProjectTo<CategoryDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<CategoryDTO> GetCategoryAsync(Guid id) {
            return _mapper.Map<CategoryDTO>(await _uow.Categories.GetByIdAsync(id));
        }

        public async Task<CategoryDTO> GetCategoryAsync(int id1C) {
            return _mapper.Map<CategoryDTO>(await _uow.Categories.All.FirstOrDefaultAsync(x => x.CategoryId1C == id1C));
        }

        public async Task<OperationResult> InsertCategoryAsync(CategoryDTO dto) {
            var entity = _mapper.Map<Category>(dto);
            var lastOrder = _uow.Categories.All
                .OrderByDescending(s => s.Order)
                .FirstOrDefault()?.Order;
            entity.Order = lastOrder + 1 ?? 1;
            entity.Id = Guid.NewGuid();
            entity.IsVisible = true;
            await _uow.Categories.AddAsync(entity);
            await _uow.SaveChangesAsync();
            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdateCategoryAsync(CategoryDTO dto) {
            var entity = _mapper.Map<Category>(dto);
            var existCat = await _uow.Categories.GetByIdAsync(dto.Id);
            if (existCat != null) {
                existCat.ImagePath = entity.ImagePath ?? existCat.ImagePath;
                existCat.IsVisible = entity.IsVisible;
                existCat.Name = entity.Name;
                existCat.Description = entity.Description;
                existCat.Order = entity.Order;
                existCat.CategoryId1C = entity.CategoryId1C;
                _uow.Categories.Update(existCat);
                await _uow.SaveChangesAsync();
                return OperationResult.Success();
            }

            return OperationResult.Failure("Категория не найдена в БД");
        }

        public async Task<OperationResult> DeleteCategory(Guid id) {
            await _uow.Categories.DeleteAsync(id);
            await _uow.SaveChangesAsync();
            return OperationResult.Success();
        }
        
        public async Task<IEnumerable<Category1CDTO>> GetAllCategoriesAsync() {
            return _mapper.Map<IEnumerable<Category1CDTO>>(await _uow.Categories.GetAllCategories1C());
        }
    }
}