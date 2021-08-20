using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddFeedbackFilesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "feedback_files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    parent_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    filename = table.Column<string>(maxLength: 64, nullable: false),
                    virtual_path = table.Column<string>(maxLength: 256, nullable: false),
                    create_date = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "current_timestamp()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedback_files", x => x.id);
                    table.ForeignKey(
                        name: "FK_feedback_files_feedback_items_parent_id",
                        column: x => x.parent_id,
                        principalTable: "feedback_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_feedback_files_parent_id",
                table: "feedback_files",
                column: "parent_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feedback_files");
        }
    }
}
