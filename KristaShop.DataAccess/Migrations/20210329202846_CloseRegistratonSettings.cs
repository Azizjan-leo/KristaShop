using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class CloseRegistratonSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "dict_settings",
                columns: new[] { "id", "description", "key", "value" },
                values: new object[] { new Guid("eac3bc5a-63d7-47c0-9e1c-55eb2a5ec864"), "Открытие регистрация. Значения: True - открыта. False - закрыта.", "IsRegistrationActive", "True" });

            migrationBuilder.InsertData(
                table: "dict_settings",
                columns: new[] { "id", "description", "key", "value" },
                values: new object[] { new Guid("ff028c22-ed10-4fc0-b48c-db3efdeb7ffa"), "Сообщение для пользователя, когда регистрация закрыта", "InactiveRegistrationMessage", "В данный момент регистрация на сайте закрыта" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("eac3bc5a-63d7-47c0-9e1c-55eb2a5ec864"));

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("ff028c22-ed10-4fc0-b48c-db3efdeb7ffa"));
        }
    }
}
