#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.Common.Implementation.DataAccess {
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class where TKey : notnull {
        protected readonly DbContext Context;

        public Repository(DbContext context) {
            Context = context;
        }

        public virtual IQueryable<TEntity> All => Context.Set<TEntity>();
        public virtual IOrderedQueryable<TEntity> AllOrdered => null;

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync() {
            return await All.ToListAsync();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, bool generateKey = false) {
            await GenerateKeyAsync(entity, generateKey);
            await Context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public virtual TEntity Update(TEntity entity) {
            Context.Set<TEntity>().Update(entity);
            return entity;
        }

        public virtual TEntity Delete(TEntity entity) {
            Context.Set<TEntity>().Remove(entity);
            return entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities) {
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities) {
            Context.Set<TEntity>().UpdateRange(entities);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities) {
            Context.Set<TEntity>().RemoveRange(entities);
        }
        
        public virtual async Task<TEntity?> GetByIdAsync(TKey id) {
            return await Context.FindAsync<TEntity>(id);
        }

        public virtual async Task<TEntity?> DeleteAsync(TKey id) {
            var entity = await Context.FindAsync<TEntity>(id);
            if (entity != null) {
                Context.Set<TEntity>().Remove(entity);
            }

            return entity;
        }
        
        private async Task GenerateKeyAsync(TEntity entity, bool generateKey) {
            if (generateKey) {
                if (entity is IEntityKeyGeneratable entityGeneratable) {
                    _generateKey(entityGeneratable);
                } else if(entity is IIdentityKeyGeneratable identityGeneratable) {
                    await _generateKeyAsync(identityGeneratable);
                }
            }
        }
        
        private void _generateKey(IEntityKeyGeneratable entity) {
            entity.GenerateKey();
        }

        private async Task _generateKeyAsync(IIdentityKeyGeneratable entity) {
            var key = await All.MaxAsync(x => EF.Property<int>(x, "Id"));
            entity.SetPrimaryKey(++key);
        }
    }
}   
