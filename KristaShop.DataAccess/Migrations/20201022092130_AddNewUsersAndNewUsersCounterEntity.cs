using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddNewUsersAndNewUsersCounterEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "for1c_new_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    fullname = table.Column<string>(maxLength: 255, nullable: false),
                    city_id = table.Column<int>(nullable: true),
                    new_city = table.Column<string>(nullable: true),
                    mall_address = table.Column<string>(maxLength: 255, nullable: false),
                    CompanyAddress = table.Column<string>(nullable: true),
                    phone = table.Column<string>(maxLength: 255, nullable: true),
                    email = table.Column<string>(maxLength: 255, nullable: true),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    manager_id = table.Column<int>(nullable: true),
                    user_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_for1c_new_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "new_users_counter",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    counter = table.Column<long>(nullable: false),
                    update_time_stamp = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_new_users_counter", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "for1c_new_users");

            migrationBuilder.DropTable(
                name: "new_users_counter");
        }
    }
}
