using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class add_new_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("173f6f93-0177-4660-a0a2-c6ce2884d388"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("378cd91c-24e2-42ff-a044-afe097705c44"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("5bd7f101-611e-47d1-9ad0-c827cb8aaa18"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("7569c646-5756-4b38-bdb6-ecf4bbf75bc4"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("7da8b97f-f713-4d83-a414-3e13879c22d7"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("80637bfc-18b9-4078-87cf-39786df2a9c7"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("99956ad8-04b3-44ce-890a-9efb0538898f"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("9e571433-8647-4baa-a32a-1a0dd3aa3594"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("b22e9166-d1bf-4355-8e18-b0565578a8bf"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("c69c45ba-0735-43d0-a786-b8ff0843dd88"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("cfb7f905-d13c-456a-bb88-029247edef6c"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("efba27c0-a25e-4839-b892-78be0a0124f3"));

            migrationBuilder.AddColumn<double>(
                name: "parts_count",
                table: "nomenclatures",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "is_authorize",
                table: "menu_contents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "additional_description",
                table: "dict_catalogs",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "parts_count",
                table: "nomenclatures");

            migrationBuilder.DropColumn(
                name: "is_authorize",
                table: "menu_contents");

            migrationBuilder.DropColumn(
                name: "additional_description",
                table: "dict_catalogs");

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "controller_name", "icon", "menu_type", "order", "parameters", "title", "url" },
                values: new object[,]
                {
                    { new Guid("7da8b97f-f713-4d83-a414-3e13879c22d7"), "Index", "Admin/Home", "fa-home", 999, 1, null, "Главная", null },
                    { new Guid("9e571433-8647-4baa-a32a-1a0dd3aa3594"), "Index", "Admin/Menu", "fa-bars", 999, 2, null, "Пункты меню", null },
                    { new Guid("7569c646-5756-4b38-bdb6-ecf4bbf75bc4"), "Index", "Admin/MBody", "fa-file-code", 999, 3, null, "Контент страниц", null },
                    { new Guid("173f6f93-0177-4660-a0a2-c6ce2884d388"), "Index", "Admin/Catalog", "fa-th", 999, 4, null, "Каталоги", null },
                    { new Guid("b22e9166-d1bf-4355-8e18-b0565578a8bf"), "Index", "Admin/Category", "fa-tags", 999, 5, null, "Категории", null },
                    { new Guid("cfb7f905-d13c-456a-bb88-029247edef6c"), "Index", "Admin/CModel", "fa-eye", 999, 6, null, "Модели", null },
                    { new Guid("5bd7f101-611e-47d1-9ad0-c827cb8aaa18"), "Index", "Admin/Discount", "fa-percent", 999, 7, null, "Скидки", null },
                    { new Guid("378cd91c-24e2-42ff-a044-afe097705c44"), "Index", "Admin/Blog", "fa-blog", 999, 8, null, "Блог", null },
                    { new Guid("c69c45ba-0735-43d0-a786-b8ff0843dd88"), "Index", "Admin/Gallery", "fa-images", 999, 9, null, "Галерея", null },
                    { new Guid("99956ad8-04b3-44ce-890a-9efb0538898f"), "Index", "Admin/Feedback", "fa-mail-bulk", 999, 10, null, "Сообщения", null },
                    { new Guid("80637bfc-18b9-4078-87cf-39786df2a9c7"), "Index", "Admin/Banner", "fa-bullhorn", 999, 12, null, "Баннер", null },
                    { new Guid("efba27c0-a25e-4839-b892-78be0a0124f3"), "Index", "Admin/UrlAcl", "fa-link", 999, 100, null, "Доступ по URL", null }
                });
        }
    }
}
