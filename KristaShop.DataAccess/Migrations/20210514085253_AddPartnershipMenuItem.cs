using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddPartnershipMenuItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[] { new Guid("909a2640-00af-4b7e-9c5f-f85597e93f69"), "Index", "Admin", null, "Partnership", "krista-crown", 999, 17, null, null, "Партнеры" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("909a2640-00af-4b7e-9c5f-f85597e93f69"));
        }
    }
}
