using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddCatalogItemVisibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_limited",
                table: "catalog_item_descriptor",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "catalog_item_visibility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    articul = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    model_id = table.Column<int>(type: "int", nullable: false),
                    size_value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    color_id = table.Column<int>(type: "int", nullable: false),
                    catalog_id = table.Column<int>(type: "int", nullable: false),
                    is_visible = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_item_visibility", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "catalog_item_visibility");

            migrationBuilder.DropColumn(
                name: "is_limited",
                table: "catalog_item_descriptor");
        }
    }
}
