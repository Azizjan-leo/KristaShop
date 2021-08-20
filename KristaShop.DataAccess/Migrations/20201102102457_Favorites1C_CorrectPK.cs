using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class Favorites1C_CorrectPK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_favorite_items_1c",
                table: "favorite_items_1c");

            migrationBuilder.AddPrimaryKey(
                name: "PK_favorite_items_1c",
                table: "favorite_items_1c",
                columns: new[] { "user_id", "articul", "catalog_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_favorite_items_1c",
                table: "favorite_items_1c");

            migrationBuilder.AddPrimaryKey(
                name: "PK_favorite_items_1c",
                table: "favorite_items_1c",
                columns: new[] { "user_id", "articul" });
        }
    }
}
