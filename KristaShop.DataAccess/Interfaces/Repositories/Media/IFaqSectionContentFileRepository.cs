using KristaShop.Common.Interfaces.DataAccess;

namespace KristaShop.DataAccess.Interfaces.Repositories.Media
{
    public interface IFaqSectionContentFileRepository<T, in TU> : IRepository<T, TU> where T : class{
    }
}
