using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddRfInstockCatalogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "dict_catalogs",
                columns: new[] { "id", "additional_description", "catalog_id_1c", "close_time", "description", "is_disable_discount", "is_open", "is_set", "is_visible", "meta_description", "meta_keywords", "meta_title", "name", "order", "order_form", "preview_path", "uri", "video_path" },
                values: new object[] { new Guid("777723b2-cf1c-4a81-ad89-b7eaaedd3109"), null, 4, null, null, false, false, true, true, null, null, null, "На складе РФ сериями", 5, 1, "", "rf-instock-series", "" });

            migrationBuilder.InsertData(
                table: "dict_catalogs",
                columns: new[] { "id", "additional_description", "catalog_id_1c", "close_time", "description", "is_disable_discount", "is_open", "is_set", "is_visible", "meta_description", "meta_keywords", "meta_title", "name", "order", "order_form", "preview_path", "uri", "video_path" },
                values: new object[] { new Guid("08ea0ff5-2d5f-4b32-b224-1c1616dd11d8"), null, 5, null, null, false, false, false, true, null, null, null, "На складе РФ не сериями", 6, 1, "", "rf-instock-noseries", "" });

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_1c_color_id",
                table: "cart_items_1c",
                column: "color_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_1c_model_id",
                table: "cart_items_1c",
                column: "model_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_1c_user_id",
                table: "cart_items_1c",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_cart_items_1c_color_id",
                table: "cart_items_1c");

            migrationBuilder.DropIndex(
                name: "IX_cart_items_1c_model_id",
                table: "cart_items_1c");

            migrationBuilder.DropIndex(
                name: "IX_cart_items_1c_user_id",
                table: "cart_items_1c");

            migrationBuilder.DeleteData(
                table: "dict_catalogs",
                keyColumn: "id",
                keyValue: new Guid("08ea0ff5-2d5f-4b32-b224-1c1616dd11d8"));

            migrationBuilder.DeleteData(
                table: "dict_catalogs",
                keyColumn: "id",
                keyValue: new Guid("777723b2-cf1c-4a81-ad89-b7eaaedd3109"));
        }
    }
}
