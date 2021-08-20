using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddRoleToManagerDetailsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "role_id",
                table: "ext1c_managers_details",
                type: "binary(16)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ext1c_managers_details_role_id",
                table: "ext1c_managers_details",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ext1c_managers_details_roles_role_id",
                table: "ext1c_managers_details",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ext1c_managers_details_roles_role_id",
                table: "ext1c_managers_details");

            migrationBuilder.DropIndex(
                name: "IX_ext1c_managers_details_role_id",
                table: "ext1c_managers_details");

            migrationBuilder.DropColumn(
                name: "role_id",
                table: "ext1c_managers_details");
        }
    }
}
