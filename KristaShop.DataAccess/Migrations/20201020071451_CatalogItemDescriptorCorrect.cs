using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class CatalogItemDescriptorCorrect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "add_aate",
                table: "catalog_item_descriptor",
                newName: "add_date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "add_date",
                table: "catalog_item_descriptor",
                newName: "add_aate");
        }
    }
}
