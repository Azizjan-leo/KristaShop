using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddPartner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_partner",
                table: "user_data");

            migrationBuilder.CreateTable(
                name: "partners",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    date_approved = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    payment_rate = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partners", x => x.user_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "dict_settings",
                columns: new[] { "id", "description", "key", "value" },
                values: new object[] { new Guid("0f24ac33-cec2-4d84-be9f-8e0209fd5244"), "Для партнера - сумма к выплате поставщику за единицу", "DefaultPartnerPaymentRate", "15.0" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "partners");

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("0f24ac33-cec2-4d84-be9f-8e0209fd5244"));

            migrationBuilder.AddColumn<bool>(
                name: "is_partner",
                table: "user_data",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
