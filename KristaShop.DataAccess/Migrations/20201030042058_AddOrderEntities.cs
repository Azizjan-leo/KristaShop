using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddOrderEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "for1c_orders",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    create_date = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    user_login = table.Column<string>(nullable: false),
                    has_extra_pack = table.Column<bool>(nullable: false),
                    is_processed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_for1c_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "for1c_order_details",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    order_id = table.Column<int>(nullable: false),
                    catalog_id = table.Column<int>(nullable: false),
                    nomenclature_id = table.Column<int>(nullable: false),
                    model_id = table.Column<int>(nullable: false),
                    size_value = table.Column<string>(nullable: false),
                    color_id = table.Column<int>(nullable: false),
                    storehouse_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_for1c_order_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_for1c_order_details_for1c_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "for1c_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_for1c_order_details_order_id",
                table: "for1c_order_details",
                column: "order_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "for1c_order_details");

            migrationBuilder.DropTable(
                name: "for1c_orders");
        }
    }
}
