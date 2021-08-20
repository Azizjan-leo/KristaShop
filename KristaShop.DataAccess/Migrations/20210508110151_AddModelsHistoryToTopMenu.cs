using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddModelsHistoryToTopMenu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "badge_target", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[] { new Guid("71c09087-a52b-4506-86ec-2677bd824ebf"), "History", "Admin", null, "ModelsCatalog", "", 1, 25, null, new Guid("54297317-28ed-4abf-bd7f-bf3be9edac79"), "Все модели" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("71c09087-a52b-4506-86ec-2677bd824ebf"));
        }
    }
}
