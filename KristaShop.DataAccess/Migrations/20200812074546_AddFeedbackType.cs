using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddFeedbackType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "viewed",
                table: "feedback_items",
                type: "TINYINT(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "feedback_items",
                type: "binary(16)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "binary(16)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "record_time_stamp",
                table: "feedback_items",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "current_timestamp()",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "feedback_items",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "feedback_items",
                type: "INT(11)",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "feedback_items");

            migrationBuilder.AlterColumn<bool>(
                name: "viewed",
                table: "feedback_items",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "TINYINT(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                table: "feedback_items",
                type: "binary(16)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "binary(16)",
                oldDefaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "record_time_stamp",
                table: "feedback_items",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME",
                oldDefaultValueSql: "current_timestamp()");

            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "feedback_items",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000);
        }
    }
}
