using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class UpdateMenuItemsForOrderReports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("1e32f8c8-c857-4f37-bd30-39bbe6864dc1"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("fd5ef88c-b1fe-4ee5-9df5-2661debdc381"));

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[] { new Guid("db0581b2-bdcd-400a-96ed-8fbfed3bcef4"), "UserUnprocessedOrdersReport", "Admin", null, "OrderReports", "", 1, 10, null, new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c"), "Сводный отчет по необработанным заказам" });

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[] { new Guid("b7b8bc14-43ce-40d6-b728-cedaa80386b1"), "CitiesOrdersReport", "Admin", null, "OrderReports", "", 1, 15, null, new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c"), "Отчет по городам" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("b7b8bc14-43ce-40d6-b728-cedaa80386b1"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("db0581b2-bdcd-400a-96ed-8fbfed3bcef4"));

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[] { new Guid("1e32f8c8-c857-4f37-bd30-39bbe6864dc1"), "CartsReport", "Admin", null, "Cart", "", 1, 10, null, new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c"), "Расшифровка по моделям в корзинах" });

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[] { new Guid("fd5ef88c-b1fe-4ee5-9df5-2661debdc381"), "TotalPreOrderReport", "Admin", null, "OrderReports", "", 1, 20, null, new Guid("02f3a86a-1ff5-40f8-9ca5-ab76b2db1d7c"), "Сводный отчет по предзаказу" });
        }
    }
}
