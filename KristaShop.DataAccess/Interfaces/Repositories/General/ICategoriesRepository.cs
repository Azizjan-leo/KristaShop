using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.DataFrom1C;

namespace KristaShop.DataAccess.Interfaces.Repositories.General {
    public interface ICategoriesRepository : IRepository<Category, Guid> {
        Task<IEnumerable<Category1C>> GetAllCategories1C();
    }
}