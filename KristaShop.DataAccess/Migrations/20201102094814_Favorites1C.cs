using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class Favorites1C : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "favorite_items_1c",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int(8)", nullable: false),
                    articul = table.Column<string>(type: "varchar(64)", nullable: false),
                    catalog_id = table.Column<int>(type: "int(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorite_items_1c", x => new { x.user_id, x.articul });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favorite_items_1c");
        }
    }
}
