using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class AddPartnerDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "part_documents",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    parent_id = table.Column<Guid>(type: "binary(16)", nullable: true),
                    number = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    create_date = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    document_type = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    execution_date = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    sum = table.Column<double>(type: "double", nullable: true),
                    state = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part_documents", x => x.id);
                    table.ForeignKey(
                        name: "FK_part_documents_part_documents_parent_id",
                        column: x => x.parent_id,
                        principalTable: "part_documents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "part_documents_sequence",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part_documents_sequence", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "part_documents_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    document_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    articul = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    model_id = table.Column<int>(type: "int", nullable: false),
                    size_value = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    color_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "double", nullable: false),
                    price_rub = table.Column<double>(type: "double", nullable: false),
                    operation_date = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    from_document_id = table.Column<Guid>(type: "binary(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_part_documents_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_part_documents_items_part_documents_document_id",
                        column: x => x.document_id,
                        principalTable: "part_documents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_part_documents_items_part_documents_from_document_id",
                        column: x => x.from_document_id,
                        principalTable: "part_documents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "part_documents_sequence",
                column: "id",
                value: 1ul);

            migrationBuilder.CreateIndex(
                name: "IX_part_documents_number",
                table: "part_documents",
                column: "number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_part_documents_parent_id",
                table: "part_documents",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_documents_items_color_id",
                table: "part_documents_items",
                column: "color_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_documents_items_document_id",
                table: "part_documents_items",
                column: "document_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_documents_items_from_document_id",
                table: "part_documents_items",
                column: "from_document_id");

            migrationBuilder.CreateIndex(
                name: "IX_part_documents_items_model_id",
                table: "part_documents_items",
                column: "model_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "part_documents_items");

            migrationBuilder.DropTable(
                name: "part_documents_sequence");

            migrationBuilder.DropTable(
                name: "part_documents");
        }
    }
}
