using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class PartnershipRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "part_partnership_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    requested_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_accepted_to_process = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_confirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    answered_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part_partnership_requests", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "dict_settings",
                columns: new[] { "id", "description", "key", "value" },
                values: new object[] { new Guid("2909fb6c-fb4a-4954-9057-774aa8f4e922"), "Сообщение для клиента, при отправке запроса на партнерство", "PartnershipRequestAcceptedToProcess", "Ваша заявка принята в обработку. В ближайшее время наши менеджеры свяжутс с вами. Ожидайте пожалуйста" });

            migrationBuilder.InsertData(
                table: "dict_settings",
                columns: new[] { "id", "description", "key", "value" },
                values: new object[] { new Guid("a2695933-31c4-4a63-8088-4e2383e80c5c"), "Сообщение для клиента, если партнерство в его городе закрыто", "PartnershipRequestRejected", "В данный момент мы не нуждаемся в партнерах в вашем регионе (городе). Но мы приняли вашу заявку и в случае необходимости свяжемся с вами" });

            migrationBuilder.InsertData(
                table: "dict_settings",
                columns: new[] { "id", "description", "key", "value" },
                values: new object[] { new Guid("bc492ac9-f0e0-4b7a-94c1-f628df3da268"), "Сообщение для клиента, если он уже подавал заявку на партнерство.", "PartnershipRequstActiveRequest", "Вы уже подавали заявку. Пожалуйста, ожидайте связи с менеджером" });

            migrationBuilder.CreateIndex(
                name: "IX_part_partnership_requests_user_id",
                table: "part_partnership_requests",
                column: "user_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "part_partnership_requests");

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("2909fb6c-fb4a-4954-9057-774aa8f4e922"));

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("a2695933-31c4-4a63-8088-4e2383e80c5c"));

            migrationBuilder.DeleteData(
                table: "dict_settings",
                keyColumn: "id",
                keyValue: new Guid("bc492ac9-f0e0-4b7a-94c1-f628df3da268"));
        }
    }
}
