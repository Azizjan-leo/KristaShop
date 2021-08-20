using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddStaffToMenuItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("ada79b75-9681-41c5-80d5-13d47cb8c522"));

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[] { new Guid("2f7c02f6-3b20-4c61-a371-946d9a2b5764"), "Index", "Admin", null, "Staff", "krista-staff", 999, 75, null, null, "Персонал" });

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[] { new Guid("6f8df031-accf-4c6d-90d0-52ebb21c6bff"), "Index", "Admin", null, "Access", "", 1, 20, null, new Guid("c29da4a1-ea6d-4649-88a6-134fecb7bc24"), "Права доступов" });

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[] { new Guid("647c76d8-9d6a-4167-928f-fdcc0e3e3ab2"), "Index", "Admin", "", "Staff", "", 1, 1, null, new Guid("2f7c02f6-3b20-4c61-a371-946d9a2b5764"), "Менеджеры" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("647c76d8-9d6a-4167-928f-fdcc0e3e3ab2"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("6f8df031-accf-4c6d-90d0-52ebb21c6bff"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("2f7c02f6-3b20-4c61-a371-946d9a2b5764"));

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[] { new Guid("ada79b75-9681-41c5-80d5-13d47cb8c522"), "Index", "Admin", null, "UrlAcl", "", 1, 20, null, new Guid("c29da4a1-ea6d-4649-88a6-134fecb7bc24"), "Доступ по URL" });
        }
    }
}
