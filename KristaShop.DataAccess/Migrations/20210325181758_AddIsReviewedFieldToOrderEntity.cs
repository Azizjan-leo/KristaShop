using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddIsReviewedFieldToOrderEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_reviewed",
                table: "for1c_orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("UPDATE `for1c_orders` SET `is_reviewed` = 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_reviewed",
                table: "for1c_orders");
        }
    }
}
