using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.DataAccess.Repositories.General {
    public class ModelPhotosRepository : Repository<ModelPhoto1C, int>, IModelPhotosRepository {
        public ModelPhotosRepository(DbContext context) : base(context) { }

        public async Task<IEnumerable<ModelPhoto1C>> GetAllAsync(IEnumerable<string> articuls) {
            return await All
                .Where(x => articuls.Contains(x.Articul))
                .Include(x => x.Color)
                .ToListAsync();
        }

        public async Task<IEnumerable<ModelPhoto1C>> GetAllByArticulAsync(string articul) {
            return await All
                .Where(x => x.Articul.Equals(articul))
                .Include(x => x.Color)
                .OrderBy(x => x.Order)
                .ToListAsync();
        }

        public async Task<int> GetMaxOrderNumberAsync(string articul) {
            return (await All.Where(x => x.Articul == articul).MaxAsync(x => (int?) x.Order)) ?? 0;
        }

        public async Task UpdatePhotoPosition(int id, int newPosition) {
            var entity = await GetByIdAsync(id);
            if (entity == null) {
                throw new EntityNotFoundException($"Model photo not found {id}");
            }

            entity.Order = newPosition;
            Update(entity);
        }

        public async Task UpdatePhotoColor(int id, int colorId) {
            var entity = await GetByIdAsync(id);
            if (entity == null) {
                throw new EntityNotFoundException($"Model photo not found {id}");
            }

            entity.ColorId = colorId;
            Update(entity);
        }

        public async Task<string> UpdatePhotoPath(int id, string path) {
            var entity = await GetByIdAsync(id);
            if (entity == null) {
                throw new EntityNotFoundException($"Model photo not found {id}");
            }

            var result = entity.PhotoPath;
            entity.PhotoPath = path;
            Update(entity);
            return result;
        }
    }
}