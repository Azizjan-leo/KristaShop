using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class ModelPhoto1CCgangeIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "model_photos_1c");

            migrationBuilder.CreateTable(
                name: "model_photos_1c",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(8)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    articul = table.Column<string>(type: "varchar(64)", nullable: false),
                    photo_path = table.Column<string>(type: "varchar(128)", nullable: false),
                    old_photo_path = table.Column<string>(type: "varchar(128)", nullable: false),
                    color_id = table.Column<int>(type: "int(8)", nullable: true),
                    order = table.Column<int>(type: "int(6)", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_model_photos_1c", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_model_photos_1c_articul_order",
                table: "model_photos_1c",
                columns: new[] { "articul", "order" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "model_photos_1c");
        }
    }
}
