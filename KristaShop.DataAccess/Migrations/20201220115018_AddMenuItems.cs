using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddMenuItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("cc811614-fdf0-4d4a-89c8-e23f9be12dc7"),
                column: "controller_name",
                value: "ModelsCatalog");

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[] { new Guid("df4f2c8a-8414-49c2-a4a3-31454b55936c"), "Index", "Admin", "Feedback", "", 1, 1, null, new Guid("cedfe1cf-4c35-4ed0-a576-f05ab5e01414"), "Сообщения" });

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[] { new Guid("aefe9999-19b7-457c-babc-c96eee8d3dc0"), "Index", "Admin", "Identity", "", 1, 1, null, new Guid("46ad49f1-21f3-48d9-bfa2-68137af8900b"), "Клиенты" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("aefe9999-19b7-457c-babc-c96eee8d3dc0"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("df4f2c8a-8414-49c2-a4a3-31454b55936c"));

            migrationBuilder.UpdateData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("cc811614-fdf0-4d4a-89c8-e23f9be12dc7"),
                column: "controller_name",
                value: "CModel");
        }
    }
}
