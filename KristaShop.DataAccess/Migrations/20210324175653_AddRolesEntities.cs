using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddRolesEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    name = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_accesses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    area = table.Column<string>(maxLength: 256, nullable: false),
                    controller = table.Column<string>(maxLength: 256, nullable: false),
                    action = table.Column<string>(maxLength: 256, nullable: false),
                    is_access_granted = table.Column<bool>(nullable: false),
                    description = table.Column<string>(maxLength: 521, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_accesses", x => x.id);
                    table.ForeignKey(
                        name: "FK_role_accesses_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_role_accesses_RoleId",
                table: "role_accesses",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "role_accesses");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
