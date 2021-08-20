using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Implementation.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;
using KristaShop.DataAccess.Interfaces.Repositories.General;
using Microsoft.EntityFrameworkCore;
using SqlKata;
using SqlKata.Compilers;

namespace KristaShop.DataAccess.Repositories.General {
    public class CategoriesRepository : Repository<Category, Guid>, ICategoriesRepository {
        public CategoriesRepository(DbContext context) : base(context) { }
        
        public async Task<IEnumerable<Category1C>> GetAllCategories1C() {
            return await Context.Set<Category1C>().ToListAsync();
        }
    }
}