using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddReportsMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[] { new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c"), "UserCartsReport", "Admin", null, "Cart", "krista-doc", 999, 15, null, null, "Отчеты" });

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[,]
                {
                    { new Guid("07f0cd59-50a0-405b-9d3a-9a7134da7ef4"), "UserCartsReport", "Admin", null, "Cart", "", 1, 1, null, new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c"), "Отчет по корзинам клиентов" },
                    { new Guid("1e32f8c8-c857-4f37-bd30-39bbe6864dc1"), "CartsReport", "Admin", null, "Cart", "", 1, 10, null, new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c"), "Расшифровка по моделям в корзинах" },
                    { new Guid("15aa05a9-6c1c-4a04-b403-90f1ad7a4a66"), "TotalOrdersReport", "Admin", null, "OrderReports", "", 1, 20, null, new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c"), "Сводный отчет по заказам" },
                    { new Guid("fd5ef88c-b1fe-4ee5-9df5-2661debdc381"), "TotalPreOrderReport", "Admin", null, "OrderReports", "", 1, 20, null, new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c"), "Сводный отчет по предзаказу" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("07f0cd59-50a0-405b-9d3a-9a7134da7ef4"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("15aa05a9-6c1c-4a04-b403-90f1ad7a4a66"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("1e32f8c8-c857-4f37-bd30-39bbe6864dc1"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("fd5ef88c-b1fe-4ee5-9df5-2661debdc381"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c"));
        }
    }
}
