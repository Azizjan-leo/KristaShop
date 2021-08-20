using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddBadgeColumnToMenuItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "badge_target",
                table: "menu_items",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("46ad49f1-21f3-48d9-bfa2-68137af8900b"),
                column: "badge_target",
                value: "clientActionsTotal");

            migrationBuilder.UpdateData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("aefe9999-19b7-457c-babc-c96eee8d3dc0"),
                column: "badge_target",
                value: "newUsers");

            migrationBuilder.UpdateData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("cedfe1cf-4c35-4ed0-a576-f05ab5e01414"),
                column: "badge_target",
                value: "feedbackTotal");

            migrationBuilder.UpdateData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("df4f2c8a-8414-49c2-a4a3-31454b55936c"),
                column: "badge_target",
                value: "newFeedback");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "badge_target",
                table: "menu_items");
        }
    }
}
