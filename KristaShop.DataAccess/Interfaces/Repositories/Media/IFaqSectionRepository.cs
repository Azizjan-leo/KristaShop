using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;

namespace KristaShop.DataAccess.Interfaces.Repositories.Media
{
    public interface IFaqSectionRepository<T, in TU> : IRepository<T, TU> where T : class
    {
        FaqSection GetFaqSectionByIdIncluding(Guid id);
        Task<List<FaqSection>> GetFaqSections(Guid id);
    }
}
