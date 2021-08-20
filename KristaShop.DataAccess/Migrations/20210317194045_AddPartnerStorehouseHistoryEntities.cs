using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddPartnerStorehouseHistoryEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "part_excess_and_deficiency_history_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    articul = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    model_id = table.Column<int>(type: "int", nullable: false),
                    size_value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    color_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "double", nullable: false),
                    price_rub = table.Column<double>(type: "double", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    resource = table.Column<int>(type: "int", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part_excess_and_deficiency_history_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "part_storehouse_history_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    movement_direction = table.Column<int>(type: "int", nullable: false),
                    movement_type = table.Column<int>(type: "int", nullable: false),
                    articul = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    model_id = table.Column<int>(type: "int", nullable: false),
                    size_value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    color_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "double", nullable: false),
                    price_rub = table.Column<double>(type: "double", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part_storehouse_history_items", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_part_excess_and_deficiency_history_items_user_id",
                table: "part_excess_and_deficiency_history_items",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_storehouse_history_items_user_id",
                table: "part_storehouse_history_items",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "part_excess_and_deficiency_history_items");

            migrationBuilder.DropTable(
                name: "part_storehouse_history_items");
        }
    }
}
