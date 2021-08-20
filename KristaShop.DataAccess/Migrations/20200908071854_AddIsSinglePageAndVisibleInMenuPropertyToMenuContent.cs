using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddIsSinglePageAndVisibleInMenuPropertyToMenuContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_single_page",
                table: "menu_contents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_visible_in_menu",
                table: "menu_contents",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_single_page",
                table: "menu_contents");

            migrationBuilder.DropColumn(
                name: "is_visible_in_menu",
                table: "menu_contents");
        }
    }
}
