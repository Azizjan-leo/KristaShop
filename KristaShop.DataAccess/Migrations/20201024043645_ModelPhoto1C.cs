using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class ModelPhoto1C : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "model_photos_1c",
                columns: table => new
                {
                    articul = table.Column<string>(type: "varchar(64)", nullable: false),
                    id = table.Column<int>(type: "int(8)", nullable: false),
                    photo_path = table.Column<string>(type: "varchar(128)", nullable: false),
                    old_photo_path = table.Column<string>(type: "varchar(128)", nullable: false),
                    color_id = table.Column<int>(type: "int(8)", nullable: true),
                    order = table.Column<int>(type: "int(6)", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_model_photos_1c", x => new { x.articul, x.id });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "model_photos_1c");
        }
    }
}
