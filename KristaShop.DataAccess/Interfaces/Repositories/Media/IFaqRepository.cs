using System;
using KristaShop.Common.Interfaces.DataAccess;
using KristaShop.DataAccess.Entities;
using KristaShop.DataAccess.Entities.Media;

namespace KristaShop.DataAccess.Interfaces.Repositories.Media
{
    public interface IFaqRepository<T, in TU> : IRepository<T, TU> where T : class
    {
        Faq GetFaqByIdIncluding(Guid id);
    }
}
