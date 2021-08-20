using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class Catalog1COrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nom_catalog_1c",
                columns: table => new
                {
                    articul = table.Column<string>(type: "varchar(64)", nullable: false),
                    catalog_id = table.Column<int>(type: "int(8)", nullable: false),
                    order = table.Column<int>(type: "int(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nom_catalog_1c", x => new { x.articul, x.catalog_id });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "nom_catalog_1c");
        }
    }
}
