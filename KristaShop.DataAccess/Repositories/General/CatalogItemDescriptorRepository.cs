using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Exceptions;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories.General {
    public class CatalogItemDescriptorRepository : Repository<CatalogItemDescriptor, string>, ICatalogItemDescriptorRepository {
        private MySqlCompiler _compiler;
        public CatalogItemDescriptorRepository(DbContext context) : base(context) {
            _compiler = new MySqlCompiler();
        }

        public async Task<Dictionary<string, CatalogItemDescriptor>> GetByArticulsAsDictionaryAsync(IEnumerable<string> articuls) {
            return await All.AsNoTracking()
                .Where(e => articuls.Contains(e.Articul))
                .ToDictionaryAsync(k => k.Articul, v => v);
        }

        public async Task UpdateDescriptorPhotoAsync(string articul, string path) {
            var entity = await GetByIdAsync(articul);
            if (entity == null) {
                throw new EntityNotFoundException($"Model descriptor not found {articul}");
            }

            entity.MainPhoto = path;
            Update(entity);
        }
    }
}