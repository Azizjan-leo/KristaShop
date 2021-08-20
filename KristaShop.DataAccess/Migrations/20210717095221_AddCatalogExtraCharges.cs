using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddCatalogExtraCharges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "catalog_discounts");

            migrationBuilder.CreateTable(
                name: "catalog_extra_charges",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    catalog_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "double", nullable: false),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_extra_charges", x => x.id);
                    table.ForeignKey(
                        name: "FK_catalog_extra_charges_dict_catalogs_catalog_id",
                        column: x => x.catalog_id,
                        principalTable: "dict_catalogs",
                        principalColumn: "catalog_id_1c",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_extra_charges_catalog_id",
                table: "catalog_extra_charges",
                column: "catalog_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "catalog_extra_charges");

            migrationBuilder.CreateTable(
                name: "catalog_discounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    catalog_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    discount_price = table.Column<double>(type: "double", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_discounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_catalog_discounts_dict_catalogs_catalog_id",
                        column: x => x.catalog_id,
                        principalTable: "dict_catalogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_discounts_catalog_id",
                table: "catalog_discounts",
                column: "catalog_id");
        }
    }
}
