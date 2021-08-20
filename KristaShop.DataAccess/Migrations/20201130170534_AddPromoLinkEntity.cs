using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddPromoLinkEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "promo_link",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    link = table.Column<string>(maxLength: 256, nullable: false),
                    manager_id = table.Column<int>(nullable: false),
                    OrderForm = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(nullable: false),
                    link_deactivate_time = table.Column<DateTimeOffset>(nullable: false),
                    title = table.Column<string>(maxLength: 256, nullable: false),
                    description = table.Column<string>(maxLength: 2048, type: "varchar(2048)", nullable: true),
                    video_link = table.Column<string>(maxLength: 256, nullable: true),
                    video_preview_link = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promo_link", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "promo_link");
        }
    }
}
