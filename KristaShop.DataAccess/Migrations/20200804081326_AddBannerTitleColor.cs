using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddBannerTitleColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "title_color",
                table: "banner_items",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "controller_name", "icon", "menu_type", "order", "parameters", "title", "url" },
                values: new object[,]
                {
                    { new Guid("e21e82e4-9d80-4acc-9839-4c3f571d51a5"), "Index", "Admin/Home", "fa-home", 999, 1, null, "Главная", null },
                    { new Guid("b9e54846-ebee-4cce-82e1-2532c4b75f9e"), "Index", "Admin/Menu", "fa-bars", 999, 2, null, "Пункты меню", null },
                    { new Guid("95f179a8-d778-47c3-a694-b8a385797f6b"), "Index", "Admin/MBody", "fa-file-code", 999, 3, null, "Контент страниц", null },
                    { new Guid("ed85480c-08d7-4a88-ac9f-4b31e02a0406"), "Index", "Admin/Catalog", "fa-th", 999, 4, null, "Каталоги", null },
                    { new Guid("7a3999d5-05cc-4380-b5e7-4ed851709950"), "Index", "Admin/Category", "fa-tags", 999, 5, null, "Категории", null },
                    { new Guid("7ed2911a-b6c2-482b-b0e6-eb44085c6cc0"), "Index", "Admin/CModel", "fa-eye", 999, 6, null, "Модели", null },
                    { new Guid("5aa4ca65-41ab-4a44-bb7e-4e21533ea5d4"), "Index", "Admin/Discount", "fa-percent", 999, 7, null, "Скидки", null },
                    { new Guid("ce3eac50-17f3-420d-abf1-4169669e7435"), "Index", "Admin/Blog", "fa-blog", 999, 8, null, "Блог", null },
                    { new Guid("5b27eb3d-e5c4-4d02-b9a3-3a41b08d4f8d"), "Index", "Admin/Gallery", "fa-images", 999, 9, null, "Галерея", null },
                    { new Guid("b7c6a779-6e64-43b9-bb1b-7371b241d1ed"), "Index", "Admin/Feedback", "fa-mail-bulk", 999, 10, null, "Сообщения", null },
                    { new Guid("eee23d92-9e0f-4f0f-aa51-0516c1ee6d70"), "Index", "Admin/Banner", "fa-bullhorn", 999, 12, null, "Баннер", null },
                    { new Guid("68c26127-83c3-43a9-8c57-a39b2be01d38"), "Index", "Admin/UrlAcl", "fa-link", 999, 100, null, "Доступ по URL", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("5aa4ca65-41ab-4a44-bb7e-4e21533ea5d4"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("5b27eb3d-e5c4-4d02-b9a3-3a41b08d4f8d"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("68c26127-83c3-43a9-8c57-a39b2be01d38"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("7a3999d5-05cc-4380-b5e7-4ed851709950"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("7ed2911a-b6c2-482b-b0e6-eb44085c6cc0"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("95f179a8-d778-47c3-a694-b8a385797f6b"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("b7c6a779-6e64-43b9-bb1b-7371b241d1ed"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("b9e54846-ebee-4cce-82e1-2532c4b75f9e"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("ce3eac50-17f3-420d-abf1-4169669e7435"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("e21e82e4-9d80-4acc-9839-4c3f571d51a5"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("ed85480c-08d7-4a88-ac9f-4b31e02a0406"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("eee23d92-9e0f-4f0f-aa51-0516c1ee6d70"));

            migrationBuilder.DropColumn(
                name: "title_color",
                table: "banner_items");

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
    }
}
