using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class UpdateMenuItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM `menu_items`;", true);

            migrationBuilder.DropColumn(
                name: "url",
                table: "menu_items");

            migrationBuilder.AddColumn<string>(
                name: "area_name",
                table: "menu_items",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "parent_id",
                table: "menu_items",
                type: "binary(16)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[,]
                {
                    { new Guid("56fba3aa-25d8-4de6-b166-5da83192be99"), "Index", "Admin", "Home", "krista-home", 999, 1, null, null, "Главная страница" },
                    { new Guid("46ad49f1-21f3-48d9-bfa2-68137af8900b"), "Index", "Admin", "Identity", "krista-user", 999, 10, null, null, "Клиенты" },
                    { new Guid("cedfe1cf-4c35-4ed0-a576-f05ab5e01414"), "Index", "Admin", "Feedback", "krista-chat", 999, 20, null, null, "Связь с клиентами" },
                    { new Guid("c66fe088-7df4-4739-bedb-147b843cb834"), "Index", "Admin", "MBody", "krista-media", 999, 30, null, null, "Мультимедия" },
                    { new Guid("54297317-28ed-4abf-bd7f-bf3be9edac79"), "Index", "Admin", "Catalog", "krista-hanger", 999, 40, null, null, "Работа с моделями" },
                    { new Guid("8f1ff3c9-8f48-499d-ac33-240a3216f721"), "Index", "Admin", "Faq", "krista-info", 999, 70, null, null, "Воронка" },
                    { new Guid("c29da4a1-ea6d-4649-88a6-134fecb7bc24"), "Index", "Admin", "Settings", "krista-settings", 999, 80, null, null, "Настройки системы" }
                });

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "area_name", "controller_name", "icon", "menu_type", "order", "parameters", "parent_id", "title" },
                values: new object[,]
                {
                    { new Guid("0f0bd978-3a1a-4fcd-b710-70b0b90e0ff7"), "Index", "Admin", "MBody", "", 1, 1, null, new Guid("c66fe088-7df4-4739-bedb-147b843cb834"), "Контент страниц" },
                    { new Guid("ffc7d8ff-b78f-4c99-9bdd-8084d1a7c098"), "Index", "Admin", "Blog", "", 1, 10, null, new Guid("c66fe088-7df4-4739-bedb-147b843cb834"), "Блог" },
                    { new Guid("b525d487-7d6a-4b1c-8776-380a6b695462"), "Index", "Admin", "Gallery", "", 1, 20, null, new Guid("c66fe088-7df4-4739-bedb-147b843cb834"), "Галерея" },
                    { new Guid("9521365b-2f09-4138-b5b5-fe15d1dc2b70"), "Index", "Admin", "Banner", "", 1, 30, null, new Guid("c66fe088-7df4-4739-bedb-147b843cb834"), "Баннер" },
                    { new Guid("76f02dd3-31bf-4910-ba35-8ffdeb4398a7"), "Index", "Admin", "VideoGallery", "", 1, 40, null, new Guid("c66fe088-7df4-4739-bedb-147b843cb834"), "Видеогалерея" },
                    { new Guid("9898ef9a-8e22-4fb4-ac98-a20911ffa5f3"), "Index", "Admin", "Catalog", "", 1, 1, null, new Guid("54297317-28ed-4abf-bd7f-bf3be9edac79"), "Каталоги" },
                    { new Guid("c9ef8cb8-ff17-4b9d-aa51-4da8c7e3d055"), "Index", "Admin", "Category", "", 1, 10, null, new Guid("54297317-28ed-4abf-bd7f-bf3be9edac79"), "Категории" },
                    { new Guid("cc811614-fdf0-4d4a-89c8-e23f9be12dc7"), "Index", "Admin", "CModel", "", 1, 20, null, new Guid("54297317-28ed-4abf-bd7f-bf3be9edac79"), "Модели" },
                    { new Guid("f79ed663-09f4-42f8-9add-ac8551df6124"), "Index", "Admin", "Settings", "", 1, 1, null, new Guid("c29da4a1-ea6d-4649-88a6-134fecb7bc24"), "Настройки" },
                    { new Guid("f8949e4a-a06c-4955-94a5-c833968ea2a2"), "Index", "Admin", "Menu", "", 1, 10, null, new Guid("c29da4a1-ea6d-4649-88a6-134fecb7bc24"), "Пункты меню" },
                    { new Guid("ada79b75-9681-41c5-80d5-13d47cb8c522"), "Index", "Admin", "UrlAcl", "", 1, 20, null, new Guid("c29da4a1-ea6d-4649-88a6-134fecb7bc24"), "Доступ по URL" },
                    { new Guid("4b8dec48-c770-4c56-8bb0-5c0409d4abef"), "Execute", "Admin", "Import", "", 1, 30, null, new Guid("c29da4a1-ea6d-4649-88a6-134fecb7bc24"), "Импорт" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_menu_items_parent_id",
                table: "menu_items",
                column: "parent_id");

            migrationBuilder.AddForeignKey(
                name: "FK_menu_items_menu_items_parent_id",
                table: "menu_items",
                column: "parent_id",
                principalTable: "menu_items",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM `menu_items`;", true);

            migrationBuilder.DropColumn(
                name: "area_name",
                table: "menu_items");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "menu_items");

            migrationBuilder.AddColumn<string>(
                name: "url",
                table: "menu_items",
                type: "varchar(256) CHARACTER SET utf8mb4",
                maxLength: 256,
                nullable: true);

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "controller_name", "icon", "menu_type", "order", "parameters", "title", "url" },
                values: new object[,]
                {
                    { new Guid("9b6bedbe-6fce-4a6a-919e-511a6c299e6c"), "Index", "Admin/Home", "fa-home", 999, 1, null, "Главная", null },
                    { new Guid("dcdf01a1-78ac-4065-af38-61e2d0d226f8"), "Index", "Admin/Menu", "fa-bars", 999, 2, null, "Пункты меню", null },
                    { new Guid("cc40cee9-2a35-4c9c-b629-7ef180166119"), "Index", "Admin/MBody", "fa-file-code", 999, 3, null, "Контент страниц", null },
                    { new Guid("0b029a4c-a06d-4109-8da8-bc1375c0357f"), "Index", "Admin/Catalog", "fa-th", 999, 4, null, "Каталоги", null },
                    { new Guid("5c3c902b-6461-4f82-a4dd-85e062180a97"), "Index", "Admin/Category", "fa-tags", 999, 5, null, "Категории", null },
                    { new Guid("b48f0234-cfed-4c22-b820-f7790382b19c"), "Index", "Admin/CModel", "fa-eye", 999, 6, null, "Модели", null },
                    { new Guid("eaf89685-c81c-45a2-aaa3-0edd9d3b9769"), "Index", "Admin/Discount", "fa-percent", 999, 7, null, "Скидки", null },
                    { new Guid("67ff1ca0-fbdd-44c8-8c88-4ba1e09b739c"), "Index", "Admin/Blog", "fa-blog", 999, 8, null, "Блог", null },
                    { new Guid("c5c26b3a-9bf8-42d4-9f3b-95f848cf428e"), "Index", "Admin/Gallery", "fa-images", 999, 9, null, "Галерея", null },
                    { new Guid("5e1ca256-4fda-4347-809d-03ee27f1cca9"), "Index", "Admin/Feedback", "fa-mail-bulk", 999, 10, null, "Сообщения", null },
                    { new Guid("2fa767b8-cdf3-47e1-a536-ea5b59ff6cd8"), "Index", "Admin/Banner", "fa-bullhorn", 999, 12, null, "Баннер", null },
                    { new Guid("1784ba0c-c201-4a9c-8afe-526b78101242"), "Index", "Admin/VideoGallery", "fa-film", 999, 13, null, "Видеогалерея", null },
                    { new Guid("fada87c8-3065-4420-b318-2fb5cc942ef6"), "Index", "Admin/UrlAcl", "fa-link", 999, 100, null, "Доступ по URL", null },
                    { new Guid("e094bf47-e8b5-4732-84ec-5970437f48c5"), "Index", "Admin/Settings", "fa-cog", 999, 120, null, "Настройки", null },
                    { new Guid("1d56e74b-9c90-4000-b673-66a6fb90fb9a"), "Index", "Admin/Faq", "fa-info", 999, 140, null, "Воронка", null },
                    { new Guid("40e2e2ec-3032-4f85-87d5-0537e4a6189d"), "Execute", "Admin/Import", "fas fa-file-import", 999, 999, null, "Импорт", null }
                });
        }
    }
}
