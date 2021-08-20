using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddLinkTextAndTitleColumnsForGalleryItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "link_text",
                table: "gallery_items",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "gallery_items",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "link_text",
                table: "gallery_items");

            migrationBuilder.DropColumn(
                name: "title",
                table: "gallery_items");
        }
    }
}
