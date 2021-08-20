using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddNomenclatureDiscountsCatalogEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nom_discounts_catalogs",
                columns: table => new
                {
                    nom_discount_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    catalog_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nom_discounts_catalogs", x => new { x.nom_discount_id, x.catalog_id });
                    table.ForeignKey(
                        name: "FK_nom_discounts_catalogs_dict_catalogs_catalog_id",
                        column: x => x.catalog_id,
                        principalTable: "dict_catalogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_nom_discounts_catalogs_nom_discounts_nom_discount_id",
                        column: x => x.nom_discount_id,
                        principalTable: "nom_discounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_nom_discounts_catalogs_catalog_id",
                table: "nom_discounts_catalogs",
                column: "catalog_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nom_discounts_catalogs");
        }
    }
}
