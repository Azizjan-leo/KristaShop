using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddManagerAccessEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "orders_data_access",
                table: "ext1c_managers_details");

            migrationBuilder.DropColumn(
                name: "registrations_data_access",
                table: "ext1c_managers_details");

            migrationBuilder.CreateTable(
                name: "ext1c_managers_accesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    manager_id = table.Column<int>(nullable: false),
                    access_to_manager_id = table.Column<int>(nullable: false),
                    access_to = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ext1c_managers_accesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ext1c_managers_accesses_ext1c_managers_details_manager_id",
                        column: x => x.manager_id,
                        principalTable: "ext1c_managers_details",
                        principalColumn: "manager_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ext1c_managers_accesses_manager_id",
                table: "ext1c_managers_accesses",
                column: "manager_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ext1c_managers_accesses");

            migrationBuilder.AddColumn<int>(
                name: "orders_data_access",
                table: "ext1c_managers_details",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "registrations_data_access",
                table: "ext1c_managers_details",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
