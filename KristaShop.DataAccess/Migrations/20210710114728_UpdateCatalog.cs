using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class UpdateCatalog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "nom_catalog_1c",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int(6)");

            migrationBuilder.AlterColumn<int>(
                name: "catalog_id",
                table: "nom_catalog_1c",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int(8)");

            migrationBuilder.AlterColumn<string>(
                name: "size_value",
                table: "catalog_item_visibility",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_model_catalogs_invisibility_articul_catalog_id",
                table: "model_catalogs_invisibility",
                columns: new[] { "articul", "catalog_id" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_dict_category_category_id_1c",
                table: "dict_category",
                column: "category_id_1c");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_dict_catalogs_catalog_id_1c",
                table: "dict_catalogs",
                column: "catalog_id_1c");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_catalog_item_visibility_model_id_color_id_size_value_catalog~",
                table: "catalog_item_visibility",
                columns: new[] { "model_id", "color_id", "size_value", "catalog_id" });

            migrationBuilder.CreateIndex(
                name: "IX_model_photos_1c_color_id",
                table: "model_photos_1c",
                column: "color_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_model_photos_1c_color_id",
                table: "model_photos_1c");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_model_catalogs_invisibility_articul_catalog_id",
                table: "model_catalogs_invisibility");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_dict_category_category_id_1c",
                table: "dict_category");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_dict_catalogs_catalog_id_1c",
                table: "dict_catalogs");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_catalog_item_visibility_model_id_color_id_size_value_catalog~",
                table: "catalog_item_visibility");

            migrationBuilder.AlterColumn<int>(
                name: "order",
                table: "nom_catalog_1c",
                type: "int(6)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "catalog_id",
                table: "nom_catalog_1c",
                type: "int(8)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "size_value",
                table: "catalog_item_visibility",
                type: "varchar(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
