#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KristaShop.Common.Interfaces.DataAccess {
    public interface IRepository {

    }

    public interface IRepository<TEntity, in TKey> : IRepository where TEntity : class {
        IQueryable<TEntity> All { get; }
        IOrderedQueryable<TEntity> AllOrdered { get; }

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> AddAsync(TEntity entity, bool generateKey = false);
        TEntity Update(TEntity entity);
        TEntity Delete(TEntity entity);
        
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void UpdateRange(IEnumerable<TEntity> entities);
        void DeleteRange(IEnumerable<TEntity> entities);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<TEntity?> DeleteAsync(TKey id);
    }
}
