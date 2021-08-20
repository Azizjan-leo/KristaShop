using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddVideoGalleryEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "gallery_video",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    preview_path = table.Column<string>(maxLength: 256, nullable: false),
                    video_path = table.Column<string>(maxLength: 256, nullable: false),
                    title = table.Column<string>(maxLength: 64, nullable: false),
                    description = table.Column<string>(maxLength: 2000, nullable: true),
                    is_visible = table.Column<bool>(nullable: false),
                    order = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gallery_video", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "video_gallery",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    title = table.Column<string>(maxLength: 128, nullable: false),
                    description = table.Column<string>(maxLength: 2048, type:"varchar(2000)", nullable: true),
                    is_visible = table.Column<bool>(nullable: false),
                    name = table.Column<string>(maxLength: 128, nullable: false),
                    order = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_video_gallery", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "video_gallery_videos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    gallery_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    video_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_video_gallery_videos", x => x.id);
                    table.ForeignKey(
                        name: "FK_video_gallery_videos_video_gallery_gallery_id",
                        column: x => x.gallery_id,
                        principalTable: "video_gallery",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_video_gallery_videos_gallery_video_video_id",
                        column: x => x.video_id,
                        principalTable: "gallery_video",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "controller_name", "icon", "menu_type", "order", "parameters", "title", "url" },
                values: new object[] { new Guid("1784ba0c-c201-4a9c-8afe-526b78101242"), "Index", "Admin/VideoGallery", "fa-film", 999, 13, null, "Видеогалерея", null });

            migrationBuilder.CreateIndex(
                name: "IX_video_gallery_videos_gallery_id",
                table: "video_gallery_videos",
                column: "gallery_id");

            migrationBuilder.CreateIndex(
                name: "IX_video_gallery_videos_video_id",
                table: "video_gallery_videos",
                column: "video_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "video_gallery_videos");

            migrationBuilder.DropTable(
                name: "video_gallery");

            migrationBuilder.DropTable(
                name: "gallery_video");

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("1784ba0c-c201-4a9c-8afe-526b78101242"));
        }
    }
}
