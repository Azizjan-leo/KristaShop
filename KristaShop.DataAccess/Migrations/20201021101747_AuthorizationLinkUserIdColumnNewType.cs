using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AuthorizationLinkUserIdColumnNewType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM `authorization_link`", true);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "authorization_link",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "binary(16)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "authorization_link",
                type: "binary(16)",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
