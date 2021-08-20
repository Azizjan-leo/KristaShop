using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class OrderProcessedRetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_processed",
                table: "for1c_orders");

            migrationBuilder.AddColumn<bool>(
                name: "is_processed_preorder",
                table: "for1c_orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_processed_retail",
                table: "for1c_orders",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_processed_preorder",
                table: "for1c_orders");

            migrationBuilder.DropColumn(
                name: "is_processed_retail",
                table: "for1c_orders");

            migrationBuilder.AddColumn<bool>(
                name: "is_processed",
                table: "for1c_orders",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
