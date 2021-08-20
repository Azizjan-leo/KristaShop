using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class MenuContentAddOrderColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("17c0a425-5e81-4c0c-ac40-55ffc62b04e0"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("3bb4efa1-1e25-4eda-a6f8-ae114e46bc00"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("653953b4-da30-4d0f-a338-3f54f46df6d5"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("6760ed7a-6f76-4408-b430-4e26544c9747"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("68e20f29-b595-4c0b-9282-65f816808f9a"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("767e9420-954d-48f8-8988-af226f9d7740"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("9b133739-eac7-4f24-ade4-1f623b29f442"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("c88d494d-46e7-4834-91d7-453222d27c41"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("c9736b11-9195-4416-aa69-1933d1f71ff0"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("f02c5904-b479-4eb6-8420-5c296b0d9078"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("f56b75c2-1a31-434f-adb7-32155de47959"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("fb2f4e69-6741-4ba6-a65a-c04319845ad1"));

            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "menu_contents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "controller_name", "icon", "menu_type", "order", "parameters", "title", "url" },
                values: new object[,]
                {
                    { new Guid("a47d3c84-89ae-4c5f-81c8-7eee5553b295"), "Index", "Admin/Home", "fa-home", 999, 1, null, "Главная", null },
                    { new Guid("2e4a0373-7cda-413a-89f3-5941306a4375"), "Index", "Admin/Menu", "fa-bars", 999, 2, null, "Пункты меню", null },
                    { new Guid("1764a5c3-16b3-4c45-bfe3-db39b649860f"), "Index", "Admin/MBody", "fa-file-code", 999, 3, null, "Контент страниц", null },
                    { new Guid("bc786175-d50e-4dd2-aa23-9743a35dda31"), "Index", "Admin/Catalog", "fa-th", 999, 4, null, "Каталоги", null },
                    { new Guid("efd960d1-712a-4ced-a81b-8731b962c449"), "Index", "Admin/Category", "fa-tags", 999, 5, null, "Категории", null },
                    { new Guid("404a0ee9-c74e-4800-a7bc-74988c8a04fd"), "Index", "Admin/CModel", "fa-eye", 999, 6, null, "Модели", null },
                    { new Guid("6b825fa8-f34a-4436-9383-518a72689c1e"), "Index", "Admin/Discount", "fa-percent", 999, 7, null, "Скидки", null },
                    { new Guid("4fa79f33-d6e4-4eaf-a2b1-53ff46bb5f5c"), "Index", "Admin/Blog", "fa-blog", 999, 8, null, "Блог", null },
                    { new Guid("9d8a80b9-d2f0-4f09-b04b-eb813631b8b4"), "Index", "Admin/Gallery", "fa-images", 999, 9, null, "Галерея", null },
                    { new Guid("6975bb16-ed47-432c-9cad-31354af52b2b"), "Index", "Admin/Feedback", "fa-mail-bulk", 999, 10, null, "Сообщения", null },
                    { new Guid("48056cc3-e894-4e12-bd11-f5c6b67f1c6e"), "Index", "Admin/Banner", "fa-bullhorn", 999, 12, null, "Баннер", null },
                    { new Guid("cee1e9c9-94c0-4f5c-9ad8-40d65f8ba3ab"), "Index", "Admin/UrlAcl", "fa-link", 999, 100, null, "Доступ по URL", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("1764a5c3-16b3-4c45-bfe3-db39b649860f"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("2e4a0373-7cda-413a-89f3-5941306a4375"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("404a0ee9-c74e-4800-a7bc-74988c8a04fd"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("48056cc3-e894-4e12-bd11-f5c6b67f1c6e"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("4fa79f33-d6e4-4eaf-a2b1-53ff46bb5f5c"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("6975bb16-ed47-432c-9cad-31354af52b2b"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("6b825fa8-f34a-4436-9383-518a72689c1e"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("9d8a80b9-d2f0-4f09-b04b-eb813631b8b4"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("a47d3c84-89ae-4c5f-81c8-7eee5553b295"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("bc786175-d50e-4dd2-aa23-9743a35dda31"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("cee1e9c9-94c0-4f5c-9ad8-40d65f8ba3ab"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("efd960d1-712a-4ced-a81b-8731b962c449"));

            migrationBuilder.DropColumn(
                name: "order",
                table: "menu_contents");

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "controller_name", "icon", "menu_type", "order", "parameters", "title", "url" },
                values: new object[,]
                {
                    { new Guid("f02c5904-b479-4eb6-8420-5c296b0d9078"), "Index", "Admin/Home", "fa-home", 999, 1, null, "Главная", null },
                    { new Guid("17c0a425-5e81-4c0c-ac40-55ffc62b04e0"), "Index", "Admin/Menu", "fa-bars", 999, 2, null, "Пункты меню", null },
                    { new Guid("f56b75c2-1a31-434f-adb7-32155de47959"), "Index", "Admin/MBody", "fa-file-code", 999, 3, null, "Контент страниц", null },
                    { new Guid("9b133739-eac7-4f24-ade4-1f623b29f442"), "Index", "Admin/Catalog", "fa-th", 999, 4, null, "Каталоги", null },
                    { new Guid("3bb4efa1-1e25-4eda-a6f8-ae114e46bc00"), "Index", "Admin/Category", "fa-tags", 999, 5, null, "Категории", null },
                    { new Guid("68e20f29-b595-4c0b-9282-65f816808f9a"), "Index", "Admin/CModel", "fa-eye", 999, 6, null, "Модели", null },
                    { new Guid("c88d494d-46e7-4834-91d7-453222d27c41"), "Index", "Admin/Discount", "fa-percent", 999, 7, null, "Скидки", null },
                    { new Guid("c9736b11-9195-4416-aa69-1933d1f71ff0"), "Index", "Admin/Blog", "fa-blog", 999, 8, null, "Блог", null },
                    { new Guid("6760ed7a-6f76-4408-b430-4e26544c9747"), "Index", "Admin/Gallery", "fa-images", 999, 9, null, "Галерея", null },
                    { new Guid("767e9420-954d-48f8-8988-af226f9d7740"), "Index", "Admin/Feedback", "fa-mail-bulk", 999, 10, null, "Сообщения", null },
                    { new Guid("653953b4-da30-4d0f-a338-3f54f46df6d5"), "Index", "Admin/Banner", "fa-bullhorn", 999, 12, null, "Баннер", null },
                    { new Guid("fb2f4e69-6741-4ba6-a65a-c04319845ad1"), "Index", "Admin/UrlAcl", "fa-link", 999, 100, null, "Доступ по URL", null }
                });
        }
    }
}
