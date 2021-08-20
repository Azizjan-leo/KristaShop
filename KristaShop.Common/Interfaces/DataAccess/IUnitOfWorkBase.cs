using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace KristaShop.Common.Interfaces.DataAccess {
    public interface IUnitOfWorkBase : IDisposable {
        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        void CommitTransaction();
        void RollbackTransaction();
        bool Save();
        [Obsolete("This method is deprecated. Use SaveChangesAsync instead")]
        Task<bool> SaveAsync();
        Task<int> SaveChangesAsync();
    }
}
