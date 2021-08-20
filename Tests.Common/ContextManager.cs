using System;
using System.IO;
using System.Threading.Tasks;
using KristaShop.Common.Extensions;
using KristaShop.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;

namespace Tests.Common {
    public static class ContextManager {
        public static async Task<KristaShopDbContext> InitializeTestContextWithMigrationsAsync(string connection) {
            var context = InitializeTestContext(string.Format(connection, Guid.NewGuid().ToString("N")));
            await context.Database.MigrateAsync();
            return context;
        }

        public static KristaShopDbContext DuplicateContextWithNewConnection(KristaShopDbContext context) {
            return InitializeTestContext(context.Database.GetConnectionString());
        }
        
        private static KristaShopDbContext InitializeTestContext(string connection) {
            var optionsBuilder = new DbContextOptionsBuilder<KristaShopDbContext>()
                .UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 25)));

            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddDebug(); }));
            optionsBuilder.ReplaceService<IMigrationsModelDiffer, MigrationsModelDifferWithoutForeignKey>();
            
            return new KristaShopDbContext(optionsBuilder.Options);
        }
    }
}