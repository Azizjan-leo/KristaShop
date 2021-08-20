using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class ModelCatalogsInvisibility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "model_catalogs_invisibility",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(8)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    articul = table.Column<string>(type: "varchar(64)", nullable: false),
                    catalog_id = table.Column<int>(type: "int(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_model_catalogs_invisibility", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "model_catalogs_invisibility");
        }
    }
}
