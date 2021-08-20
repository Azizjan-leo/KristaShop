using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddCartSuccessToSettingsSeeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "dict_settings",
                columns: new[] { "id", "description", "key", "value" },
                values: new object[] { new Guid("dfc70e24-ad7d-4283-9ad1-e9580af64ada"), "Сообщение при успешном совершениии покупки", "CartSuccess", "Спасибо за покупку" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("dfc70e24-ad7d-4283-9ad1-e9580af64ada"));
        }
    }
}
