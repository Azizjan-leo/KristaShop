using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class RemoveGroupsColumnsFromUrlAccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "access_groups_json",
                table: "url_access");

            migrationBuilder.DropColumn(
                name: "acl",
                table: "url_access");

            migrationBuilder.DropColumn(
                name: "denied_groups_json",
                table: "url_access");

            migrationBuilder.AddColumn<bool>(
                name: "for_manager",
                table: "url_access",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "for_root",
                table: "url_access",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "controller_name", "icon", "menu_type", "order", "parameters", "title", "url" },
                values: new object[] { new Guid("40e2e2ec-3032-4f85-87d5-0537e4a6189d"), "Execute", "Admin/Import", "fas fa-file-import", 999, 999, null, "Импорт", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("40e2e2ec-3032-4f85-87d5-0537e4a6189d"));

            migrationBuilder.DropColumn(
                name: "for_manager",
                table: "url_access");

            migrationBuilder.DropColumn(
                name: "for_root",
                table: "url_access");

            migrationBuilder.AddColumn<string>(
                name: "access_groups_json",
                table: "url_access",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "acl",
                table: "url_access",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "denied_groups_json",
                table: "url_access",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
