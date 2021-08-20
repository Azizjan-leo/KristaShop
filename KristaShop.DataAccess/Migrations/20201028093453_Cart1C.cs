using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class Cart1C : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "category_id_1c",
                table: "dict_category",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "catalog_id_1c",
                table: "dict_catalogs",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "cart_items_1c",
                columns: table => new
                {
                    id = table.Column<int>(type: "int(8)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int(8)", nullable: false),
                    catalog_id = table.Column<int>(type: "int(8)", nullable: false),
                    articul = table.Column<string>(type: "varchar(64)", nullable: false),
                    model_id = table.Column<int>(type: "int(8)", nullable: false),
                    nomenclature_id = table.Column<int>(type: "int(8)", nullable: false),
                    color_id = table.Column<int>(type: "int(8)", nullable: false),
                    SizeValue = table.Column<string>(type: "varchar(32)", nullable: false),
                    price = table.Column<double>(type: "double", nullable: false),
                    price_rub = table.Column<double>(type: "double", nullable: false),
                    amount = table.Column<int>(type: "int(6)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart_items_1c", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cart_items_1c");

            migrationBuilder.AlterColumn<long>(
                name: "category_id_1c",
                table: "dict_category",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "catalog_id_1c",
                table: "dict_catalogs",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
