using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class UpdateSettingsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "dict_settings",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "only_root_access",
                table: "dict_settings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "dict_settings",
                columns: new[] { "id", "description", "key", "value" },
                values: new object[,]
                {
                    { new Guid("afd36311-a384-4105-946d-e2d388ab072c"), "Ссылка на оптовый аккаунт инстаграм", "KristaInstagram", "https://www.instagram.com/krista.fashion/" },
                    { new Guid("7f1c2461-2d69-4af4-9f77-15991cc420bd"), "Ссылка на оптовый аккаунт фэйсбук", "KristaFacebook", "https://www.facebook.com/kristafashion-101281188115170/" },
                    { new Guid("e509c422-9e1c-4372-bfc6-a0641ab65a55"), "Ссылка на оптовый аккаунт ютуб", "KristaYoutube", "https://www.youtube.com/channel/UCXftbG5dwIDgWGR_WKOj5CQ" },
                    { new Guid("6a2467aa-13de-45bf-9772-8d1a53f76541"), "Ссылка на подписку на оптовый аккаунт ютуб", "KristaYoutubeSubscribe", "https://www.youtube.com/channel/UCXftbG5dwIDgWGR_WKOj5CQ?sub_confirmation=1" },
                    { new Guid("f7c7016c-2a60-4e51-b6c5-9db7e61e1aa0"), "Ссылка на оптовый аккаунт в контакте", "KristaVk", "https://www.vk.com/" },
                    { new Guid("d5f1181d-89e6-4e6a-900e-2d0b5017f4f4"), "Путь к политике конфиденциальности", "TermsOfUse", "/Privacy/Index" },
                    { new Guid("5e3da824-8a53-4028-af75-f270bec049d0"), "Путь данным о доставке", "DeliveryDetails", "/Cooperation/Delivery" },
                    { new Guid("adee4fd9-878d-45a6-aed7-2cff2df6b123"), "Путь к данным об оплате", "PaymentDetails", "/Cooperation/Payment" },
                    { new Guid("b1a20574-b8db-41d0-b007-d77efa9219ee"), "Путь к контактам в футере", "FooterContacts", "/Footer/Contacts" },
                    { new Guid("d7863668-5d04-490d-b357-4c4aba7eb6d5"), "Путь к описанию на странице категориий", "CategoriesDescription", "/Category/Index" },
                    { new Guid("f4538f14-00df-4616-a3f7-d3edcf622fb2"), "Путь к дополнительному описанию открытого каталога при поиске", "OpenCatalogSearchDescription", "/Search/OpenCatalog" }
                });

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "controller_name", "icon", "menu_type", "order", "parameters", "title", "url" },
                values: new object[] { new Guid("e094bf47-e8b5-4732-84ec-5970437f48c5"), "Index", "Admin/Settings", "fa-cog", 999, 120, null, "Настройки", null });

            migrationBuilder.CreateIndex(
                name: "IX_dict_settings_key",
                table: "dict_settings",
                column: "key",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_dict_settings_key",
                table: "dict_settings");

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("5e3da824-8a53-4028-af75-f270bec049d0"));

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("6a2467aa-13de-45bf-9772-8d1a53f76541"));

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("7f1c2461-2d69-4af4-9f77-15991cc420bd"));

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("adee4fd9-878d-45a6-aed7-2cff2df6b123"));

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("afd36311-a384-4105-946d-e2d388ab072c"));

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("b1a20574-b8db-41d0-b007-d77efa9219ee"));

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("d5f1181d-89e6-4e6a-900e-2d0b5017f4f4"));

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("d7863668-5d04-490d-b357-4c4aba7eb6d5"));

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("e509c422-9e1c-4372-bfc6-a0641ab65a55"));

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("f4538f14-00df-4616-a3f7-d3edcf622fb2"));

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("f7c7016c-2a60-4e51-b6c5-9db7e61e1aa0"));

            migrationBuilder.DeleteData(
                table: "menu_items",
                keyColumn: "id",
                keyValue: new Guid("e094bf47-e8b5-4732-84ec-5970437f48c5"));

            migrationBuilder.DropColumn(
                name: "description",
                table: "dict_settings");

            migrationBuilder.DropColumn(
                name: "only_root_access",
                table: "dict_settings");
        }
    }
}
