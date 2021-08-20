using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class FixViewsForNet5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // builder.ToTable("", t => t.ExcludeFromMigrations()) were added for all views to support ef core 5
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
