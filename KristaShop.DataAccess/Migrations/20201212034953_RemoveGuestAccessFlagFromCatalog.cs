using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class RemoveGuestAccessFlagFromCatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_guest_access",
                table: "dict_catalogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_guest_access",
                table: "dict_catalogs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
