using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddFaqSectionContentFileEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentUrl",
                table: "faq_section_content");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "faq_section_content");

            migrationBuilder.DropColumn(
                name: "IsDocument",
                table: "faq_section_content");

            migrationBuilder.DropColumn(
                name: "IsImage",
                table: "faq_section_content");

            migrationBuilder.CreateTable(
                name: "FaqSectionContentFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    FaqSectionContentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqSectionContentFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqSectionContentFile_faq_section_content_FaqSectionContentId",
                        column: x => x.FaqSectionContentId,
                        principalTable: "faq_section_content",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FaqSectionContentFile_FaqSectionContentId",
                table: "FaqSectionContentFile",
                column: "FaqSectionContentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaqSectionContentFile");

            migrationBuilder.AddColumn<string>(
                name: "DocumentUrl",
                table: "faq_section_content",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "faq_section_content",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDocument",
                table: "faq_section_content",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsImage",
                table: "faq_section_content",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
