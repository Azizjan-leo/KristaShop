using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddFaqEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "faq",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    title = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faq", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "faq_section",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    title = table.Column<string>(maxLength: 64, nullable: false),
                    faq_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faq_section", x => x.id);
                    table.ForeignKey(
                        name: "FK_faq_section_faq_faq_id",
                        column: x => x.faq_id,
                        principalTable: "faq",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "faq_section_content",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    content = table.Column<string>(maxLength: 5000, type: "varchar(5000)", nullable: false),
                    IsDocument = table.Column<bool>(nullable: false),
                    faq_section_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faq_section_content", x => x.id);
                    table.ForeignKey(
                        name: "FK_faq_section_content_faq_section_faq_section_id",
                        column: x => x.faq_section_id,
                        principalTable: "faq_section",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_faq_section_faq_id",
                table: "faq_section",
                column: "faq_id");

            migrationBuilder.CreateIndex(
                name: "IX_faq_section_content_faq_section_id",
                table: "faq_section_content",
                column: "faq_section_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "faq_section_content");

            migrationBuilder.DropTable(
                name: "faq_section");

            migrationBuilder.DropTable(
                name: "faq");
        }
    }
}
