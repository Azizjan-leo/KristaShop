#nullable enable
using System;
using System.Data;
using System.Threading.Tasks;
using KristaShop.Common.Interfaces;
using KristaShop.Common.Interfaces.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace KristaShop.Common.Implementation.DataAccess {
    public abstract class UnitOfWorkBase : IUnitOfWorkBase {
        protected readonly BaseDbContext Context;
        private readonly IClaimsManager? _claimsManager;
        protected readonly ILogger Logger;

        protected UnitOfWorkBase(BaseDbContext context, ILogger logger, IClaimsManager? claimsManager = default) {
            Context = context;
            _claimsManager = claimsManager;
            Logger = logger;
        }

        public IDbContextTransaction BeginTransaction() {
            Context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
            return Context.Database.CurrentTransaction;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync() {
            return await Context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }

        public async Task CommitTransactionAsync() {
            await Context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync() {
            await Context.Database.RollbackTransactionAsync();
        }

        public void CommitTransaction() {
            Context.Database.CommitTransaction();
        }

        public void RollbackTransaction() {
            Context.Database.RollbackTransaction();
        }

        [Obsolete]
        public virtual bool Save() {
            try {
                return Context.SaveChanges() > 0;
            } catch (Exception ex) {
                if (Context.Database.CurrentTransaction != null) {
                    Context.Database.RollbackTransaction();
                }
                Logger.Error(ex, "Failed to commit changes {message}", ex.Message);
                return false;
            }
        }

        public virtual async Task<bool> SaveAsync() {
            try {
                return await Context.SaveChangesAsync() > 0;
            } catch (Exception ex) {
                if (Context.Database.CurrentTransaction != null) {
                    await Context.Database.RollbackTransactionAsync();
                }
                Logger.Error(ex, "Failed to commit changes {message}", ex.Message);
                return false;
            }
        }
        
        public virtual async Task<int> SaveChangesAsync() {
            try {
                return await Context.SaveChangesAsync(true, _claimsManager?.Session);
            } catch (Exception ex) {
                Logger.Error(ex, "Failed to commit changes {message}", ex.Message);
                throw;
            }
        }

        #region IDisposable implementation

        private bool _disposed;

        protected void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    _dispose();
                }
            }

            _disposed = true;
        }

        protected virtual void _dispose() {
            Context.Dispose();
        }

        public void Dispose() {
            Dispose(true);
        }

        #endregion
    }
}
