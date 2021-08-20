using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class RemoveOldTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cart_items");

            migrationBuilder.DropTable(
                name: "nom_catalog");

            migrationBuilder.DropTable(
                name: "nom_category");

            migrationBuilder.DropTable(
                name: "nom_discounts_catalogs");

            migrationBuilder.DropTable(
                name: "nom_photos");

            migrationBuilder.DropTable(
                name: "nom_preorder");

            migrationBuilder.DropTable(
                name: "nom_prod_price");

            migrationBuilder.DropTable(
                name: "nom_user_favorites");

            migrationBuilder.DropTable(
                name: "not_visible_prod_ctgrs");

            migrationBuilder.DropTable(
                name: "not_visible_prod_ctlgs");

            migrationBuilder.DropTable(
                name: "user_discounts");

            migrationBuilder.DropTable(
                name: "visible_nom_users");

            migrationBuilder.DropTable(
                name: "visible_user_catalogs");

            migrationBuilder.DropTable(
                name: "nom_discounts");

            migrationBuilder.DropTable(
                name: "nomenclatures");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "nomenclatures",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    articul = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    default_price = table.Column<double>(type: "double", nullable: false),
                    description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    image_alternative_text = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    image_path = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    is_set = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    is_visible = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    link_name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    meta_description = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    meta_keywords = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    meta_title = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    parts_count = table.Column<double>(type: "double", nullable: false),
                    youtube_link = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nomenclatures", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_discounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    discount_price = table.Column<double>(type: "double", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    user_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_discounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "visible_user_catalogs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    catalog_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    user_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visible_user_catalogs", x => x.id);
                    table.ForeignKey(
                        name: "FK_visible_user_catalogs_dict_catalogs_catalog_id",
                        column: x => x.catalog_id,
                        principalTable: "dict_catalogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cart_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    catalog_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    discount = table.Column<double>(type: "double", nullable: false),
                    nom_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    order_form_type = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "double", nullable: false),
                    product_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    total_amount = table.Column<double>(type: "double", nullable: false),
                    user_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_cart_items_nomenclatures_nom_id",
                        column: x => x.nom_id,
                        principalTable: "nomenclatures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nom_catalog",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    catalog_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    nom_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nom_catalog", x => x.id);
                    table.ForeignKey(
                        name: "FK_nom_catalog_dict_catalogs_catalog_id",
                        column: x => x.catalog_id,
                        principalTable: "dict_catalogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_nom_catalog_nomenclatures_nom_id",
                        column: x => x.nom_id,
                        principalTable: "nomenclatures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nom_category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    category_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    nom_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nom_category", x => x.id);
                    table.ForeignKey(
                        name: "FK_nom_category_dict_category_category_id",
                        column: x => x.category_id,
                        principalTable: "dict_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_nom_category_nomenclatures_nom_id",
                        column: x => x.nom_id,
                        principalTable: "nomenclatures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nom_discounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    discount_price = table.Column<double>(type: "double", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    is_active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    nom_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nom_discounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_nom_discounts_nomenclatures_nom_id",
                        column: x => x.nom_id,
                        principalTable: "nomenclatures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nom_photos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    color_id = table.Column<Guid>(type: "binary(16)", nullable: true),
                    nom_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    old_photo_path = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    order = table.Column<int>(type: "int", nullable: false),
                    photo_path = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nom_photos", x => x.id);
                    table.ForeignKey(
                        name: "FK_nom_photos_nomenclatures_nom_id",
                        column: x => x.nom_id,
                        principalTable: "nomenclatures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nom_preorder",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    counter = table.Column<int>(type: "int", nullable: false),
                    max_amout = table.Column<int>(type: "int", nullable: false),
                    nom_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nom_preorder", x => x.id);
                    table.ForeignKey(
                        name: "FK_nom_preorder_nomenclatures_nom_id",
                        column: x => x.nom_id,
                        principalTable: "nomenclatures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nom_prod_price",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    nom_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    price = table.Column<double>(type: "double", nullable: false),
                    product_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nom_prod_price", x => x.id);
                    table.ForeignKey(
                        name: "FK_nom_prod_price_nomenclatures_nom_id",
                        column: x => x.nom_id,
                        principalTable: "nomenclatures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nom_user_favorites",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    catalog_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    nom_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    user_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nom_user_favorites", x => x.id);
                    table.ForeignKey(
                        name: "FK_nom_user_favorites_dict_catalogs_catalog_id",
                        column: x => x.catalog_id,
                        principalTable: "dict_catalogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_nom_user_favorites_nomenclatures_nom_id",
                        column: x => x.nom_id,
                        principalTable: "nomenclatures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "not_visible_prod_ctgrs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    category_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    nom_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    product_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_not_visible_prod_ctgrs", x => x.id);
                    table.ForeignKey(
                        name: "FK_not_visible_prod_ctgrs_dict_category_category_id",
                        column: x => x.category_id,
                        principalTable: "dict_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_not_visible_prod_ctgrs_nomenclatures_nom_id",
                        column: x => x.nom_id,
                        principalTable: "nomenclatures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "not_visible_prod_ctlgs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    catalog_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    nom_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    product_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_not_visible_prod_ctlgs", x => x.id);
                    table.ForeignKey(
                        name: "FK_not_visible_prod_ctlgs_dict_catalogs_catalog_id",
                        column: x => x.catalog_id,
                        principalTable: "dict_catalogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_not_visible_prod_ctlgs_nomenclatures_nom_id",
                        column: x => x.nom_id,
                        principalTable: "nomenclatures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "visible_nom_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    nom_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    user_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visible_nom_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_visible_nom_users_nomenclatures_nom_id",
                        column: x => x.nom_id,
                        principalTable: "nomenclatures",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "nom_discounts_catalogs",
                columns: table => new
                {
                    nom_discount_id = table.Column<Guid>(type: "binary(16)", nullable: false),
                    catalog_id = table.Column<Guid>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nom_discounts_catalogs", x => new { x.nom_discount_id, x.catalog_id });
                    table.ForeignKey(
                        name: "FK_nom_discounts_catalogs_dict_catalogs_catalog_id",
                        column: x => x.catalog_id,
                        principalTable: "dict_catalogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_nom_discounts_catalogs_nom_discounts_nom_discount_id",
                        column: x => x.nom_discount_id,
                        principalTable: "nom_discounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_nom_id",
                table: "cart_items",
                column: "nom_id");

            migrationBuilder.CreateIndex(
                name: "IX_nom_catalog_catalog_id",
                table: "nom_catalog",
                column: "catalog_id");

            migrationBuilder.CreateIndex(
                name: "IX_nom_catalog_nom_id",
                table: "nom_catalog",
                column: "nom_id");

            migrationBuilder.CreateIndex(
                name: "IX_nom_category_category_id",
                table: "nom_category",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_nom_category_nom_id",
                table: "nom_category",
                column: "nom_id");

            migrationBuilder.CreateIndex(
                name: "IX_nom_discounts_nom_id",
                table: "nom_discounts",
                column: "nom_id");

            migrationBuilder.CreateIndex(
                name: "IX_nom_discounts_catalogs_catalog_id",
                table: "nom_discounts_catalogs",
                column: "catalog_id");

            migrationBuilder.CreateIndex(
                name: "IX_nom_photos_nom_id",
                table: "nom_photos",
                column: "nom_id");

            migrationBuilder.CreateIndex(
                name: "IX_nom_preorder_nom_id",
                table: "nom_preorder",
                column: "nom_id");

            migrationBuilder.CreateIndex(
                name: "IX_nom_prod_price_nom_id",
                table: "nom_prod_price",
                column: "nom_id");

            migrationBuilder.CreateIndex(
                name: "IX_nom_user_favorites_catalog_id",
                table: "nom_user_favorites",
                column: "catalog_id");

            migrationBuilder.CreateIndex(
                name: "IX_nom_user_favorites_nom_id",
                table: "nom_user_favorites",
                column: "nom_id");

            migrationBuilder.CreateIndex(
                name: "IX_not_visible_prod_ctgrs_category_id",
                table: "not_visible_prod_ctgrs",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_not_visible_prod_ctgrs_nom_id",
                table: "not_visible_prod_ctgrs",
                column: "nom_id");

            migrationBuilder.CreateIndex(
                name: "IX_not_visible_prod_ctlgs_catalog_id",
                table: "not_visible_prod_ctlgs",
                column: "catalog_id");

            migrationBuilder.CreateIndex(
                name: "IX_not_visible_prod_ctlgs_nom_id",
                table: "not_visible_prod_ctlgs",
                column: "nom_id");

            migrationBuilder.CreateIndex(
                name: "IX_visible_nom_users_nom_id",
                table: "visible_nom_users",
                column: "nom_id");

            migrationBuilder.CreateIndex(
                name: "IX_visible_user_catalogs_catalog_id",
                table: "visible_user_catalogs",
                column: "catalog_id");
        }
    }
}
