using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;

namespace KristaShop.DataAccess.Interfaces.Repositories.Media
{
    public interface IFaqSectionContentRepository<T, in TU> : IRepository<T, TU> where T : class
    {
        Task<List<FaqSectionContent>> GetFaqSectionContent(Guid id);
    }
}
