using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddNewMenuFaq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "controller_name", "icon", "menu_type", "order", "parameters", "title", "url" },
                values: new object[] { new Guid("1d56e74b-9c90-4000-b673-66a6fb90fb9a"), "Index", "Admin/Faq", "fa-info", 999, 140, null, "Воронка", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("1d56e74b-9c90-4000-b673-66a6fb90fb9a"));
        }
    }
}
