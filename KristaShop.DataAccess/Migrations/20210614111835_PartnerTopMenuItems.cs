using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class PartnerTopMenuItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("909a2640-00af-4b7e-9c5f-f85597e93f69"),
                column: "badge_target",
                value: "requestsTotal");

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parent_id", "title" },
                values: new object[,]
                {
                    { new Guid("2b102b13-986e-422a-a058-c2dd4ee4d713"), "Index", "Admin", "", "Partnership", "", 1, 1, new Guid("909a2640-00af-4b7e-9c5f-f85597e93f69"), "Партнеры" },
                    { new Guid("68d7f82c-8a8d-4650-a47b-7725c5e56d86"), "PaymentsHistory", "Admin", "", "Partnership", "", 1, 10, new Guid("909a2640-00af-4b7e-9c5f-f85597e93f69"), "История взаиморасчетов" },
                    { new Guid("326c52ed-1831-49ab-8a9b-c449f7bcfaba"), "SalesReport", "Admin", "", "Partnership", "", 1, 20, new Guid("909a2640-00af-4b7e-9c5f-f85597e93f69"), "Отчет по продажам" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("2b102b13-986e-422a-a058-c2dd4ee4d713"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("326c52ed-1831-49ab-8a9b-c449f7bcfaba"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("68d7f82c-8a8d-4650-a47b-7725c5e56d86"));

            migrationBuilder.UpdateData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("909a2640-00af-4b7e-9c5f-f85597e93f69"),
                column: "badge_target",
                value: null);
        }
    }
}
