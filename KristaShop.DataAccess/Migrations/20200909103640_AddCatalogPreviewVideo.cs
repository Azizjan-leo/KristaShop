using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddCatalogPreviewVideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "video_path",
                table: "video_gallery",
                maxLength: 256,
                nullable: true,
                defaultValueSql: "''",
                oldClrType: typeof(string),
                oldType: "varchar(256) CHARACTER SET utf8mb4",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "preview_path",
                table: "video_gallery",
                maxLength: 256,
                nullable: true,
                defaultValueSql: "''",
                oldClrType: typeof(string),
                oldType: "varchar(256) CHARACTER SET utf8mb4",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "close_time",
                table: "dict_catalogs",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "preview_path",
                table: "dict_catalogs",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "video_path",
                table: "dict_catalogs",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "close_time",
                table: "dict_catalogs");

            migrationBuilder.DropColumn(
                name: "preview_path",
                table: "dict_catalogs");

            migrationBuilder.DropColumn(
                name: "video_path",
                table: "dict_catalogs");

            migrationBuilder.AlterColumn<string>(
                name: "video_path",
                table: "video_gallery",
                type: "varchar(256) CHARACTER SET utf8mb4",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true,
                oldDefaultValueSql: "''");

            migrationBuilder.AlterColumn<string>(
                name: "preview_path",
                table: "video_gallery",
                type: "varchar(256) CHARACTER SET utf8mb4",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true,
                oldDefaultValueSql: "''");
        }
    }
}
