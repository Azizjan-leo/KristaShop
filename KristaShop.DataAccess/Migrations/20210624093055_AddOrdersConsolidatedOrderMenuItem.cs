using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddOrdersConsolidatedOrderMenuItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parent_id", "title" },
                values: new object[] { new Guid("b185169c-2738-4664-b5ce-0c8a98f0f227"), "ConsolidatedRequest", "Admin", "", "Orders", "", 1, 26, new Guid("46ad49f1-21f3-48d9-bfa2-68137af8900b"), "Сводная заявка" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("b185169c-2738-4664-b5ce-0c8a98f0f227"));
        }
    }
}
