#nullable enable
using System.Threading;
using System.Threading.Tasks;
using KristaShop.Common.Models.Session;
using Microsoft.EntityFrameworkCore;

namespace KristaShop.Common.Implementation.DataAccess {
    public abstract class BaseDbContext : DbContext {
        protected BaseDbContext (DbContextOptions options) : base(options) { }

        public abstract Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, UserSession? session,
            CancellationToken cancellationToken = default);
    }
}