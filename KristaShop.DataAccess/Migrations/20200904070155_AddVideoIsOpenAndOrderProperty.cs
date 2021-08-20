using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddVideoIsOpenAndOrderProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order",
                table: "gallery_video");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "video_gallery",
                newName: "slug");

            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "video_gallery_videos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_open",
                table: "video_gallery",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order",
                table: "video_gallery_videos");

            migrationBuilder.DropColumn(
                name: "is_open",
                table: "video_gallery");

            migrationBuilder.RenameColumn(
                name: "slug",
                table: "video_gallery",
                newName: "name");

            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "gallery_video",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
