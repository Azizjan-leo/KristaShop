using KristaShop.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations {
    [DbContext(typeof(KristaShopDbContext))]
    [Migration("20200625063802_Init1cTables")]
    public partial class Init1cTables {
        protected override void BuildTargetModel(ModelBuilder modelBuilder) {
            
        }
    }
}