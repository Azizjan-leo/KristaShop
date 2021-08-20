using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddManagerDetailsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ext1c_managers_details",
                columns: table => new
                {
                    manager_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    registration_queue_number = table.Column<int>(nullable: false),
                    notifications_email = table.Column<string>(maxLength: 256, nullable: true),
                    orders_data_access = table.Column<int>(nullable: false),
                    registrations_data_access = table.Column<int>(nullable: false),
                    send_new_registrations_notification = table.Column<bool>(nullable: false),
                    send_new_order_notification = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ext1c_managers_details", x => x.manager_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ext1c_managers_details");
        }
    }
}
