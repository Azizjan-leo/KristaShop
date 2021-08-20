using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddPartnerStorehouseItemEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "part_storehouse_items",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    articul = table.Column<string>(nullable: false),
                    model_id = table.Column<int>(nullable: false),
                    color_id = table.Column<int>(nullable: false),
                    size_value = table.Column<string>(nullable: false),
                    amount = table.Column<int>(nullable: false),
                    price = table.Column<double>(nullable: false),
                    price_rub = table.Column<double>(nullable: false),
                    order_type = table.Column<int>(nullable: false),
                    income_date = table.Column<DateTimeOffset>(nullable: false),
                    reservation_key = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part_storehouse_items", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_part_storehouse_items_user_id",
                table: "part_storehouse_items",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "part_storehouse_items");
        }
    }
}
