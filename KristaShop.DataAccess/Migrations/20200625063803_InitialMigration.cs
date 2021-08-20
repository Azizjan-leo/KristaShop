using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KristaShop.DataAccess.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authorization_link",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    random_code = table.Column<string>(maxLength: 64, nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    record_time_stamp = table.Column<DateTime>(nullable: false),
                    valid_to = table.Column<DateTime>(nullable: true),
                    login_date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authorization_link", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "banner_items",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    title = table.Column<string>(maxLength: 100, nullable: false),
                    caption = table.Column<string>(maxLength: 100, nullable: false),
                    image_path = table.Column<string>(maxLength: 100, nullable: false),
                    description = table.Column<string>(maxLength: 2000, nullable: false),
                    link = table.Column<string>(nullable: false),
                    is_visible = table.Column<bool>(nullable: false),
                    order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banner_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "blog_items",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    image_path = table.Column<string>(maxLength: 100, nullable: false),
                    title = table.Column<string>(maxLength: 64, nullable: false),
                    description = table.Column<string>(maxLength: 2000, nullable: false),
                    link = table.Column<string>(nullable: false),
                    is_visible = table.Column<bool>(nullable: false),
                    order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blog_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dict_catalogs",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(maxLength: 64, nullable: false),
                    uri = table.Column<string>(maxLength: 64, nullable: false),
                    order_form = table.Column<int>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    meta_title = table.Column<string>(nullable: true),
                    meta_keywords = table.Column<string>(nullable: true),
                    meta_description = table.Column<string>(nullable: true),
                    order = table.Column<int>(nullable: false),
                    is_disable_discount = table.Column<bool>(nullable: false),
                    is_visible = table.Column<bool>(nullable: false),
                    is_open = table.Column<bool>(nullable: false),
                    is_set = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dict_catalogs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dict_category",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    image_path = table.Column<string>(maxLength: 100, nullable: true),
                    name = table.Column<string>(maxLength: 100, nullable: false),
                    description = table.Column<string>(nullable: true),
                    order = table.Column<int>(nullable: false),
                    is_visible = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dict_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dict_settings",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    key = table.Column<string>(maxLength: 64, nullable: false),
                    value = table.Column<string>(maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dict_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "feedback_items",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    person = table.Column<string>(maxLength: 64, nullable: false),
                    phone = table.Column<string>(maxLength: 64, nullable: false),
                    message = table.Column<string>(nullable: true),
                    email = table.Column<string>(maxLength: 64, nullable: true),
                    viewed = table.Column<bool>(nullable: false),
                    record_time_stamp = table.Column<DateTime>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    view_time_stamp = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedback_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gallery_items",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    image_path = table.Column<string>(maxLength: 100, nullable: false),
                    description = table.Column<string>(maxLength: 2000, nullable: false),
                    link = table.Column<string>(nullable: false),
                    is_visible = table.Column<bool>(nullable: false),
                    order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gallery_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "menu_contents",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    url = table.Column<string>(maxLength: 256, nullable: false),
                    title = table.Column<string>(maxLength: 64, nullable: false),
                    body = table.Column<string>(nullable: false),
                    layout = table.Column<string>(maxLength: 64, nullable: false),
                    meta_title = table.Column<string>(maxLength: 500, nullable: true),
                    meta_description = table.Column<string>(maxLength: 500, nullable: true),
                    meta_keywords = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_contents", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "menu_items",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    menu_type = table.Column<int>(nullable: false),
                    title = table.Column<string>(maxLength: 64, nullable: false),
                    controller_name = table.Column<string>(maxLength: 64, nullable: false),
                    action_name = table.Column<string>(maxLength: 64, nullable: false),
                    url = table.Column<string>(maxLength: 256, nullable: true),
                    parameters = table.Column<string>(nullable: true),
                    icon = table.Column<string>(nullable: true),
                    order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "nomenclatures",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    articul = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    default_price = table.Column<double>(nullable: false),
                    description = table.Column<string>(nullable: true),
                    youtube_link = table.Column<string>(nullable: true),
                    meta_title = table.Column<string>(nullable: true),
                    link_name = table.Column<string>(nullable: true),
                    meta_keywords = table.Column<string>(nullable: true),
                    meta_description = table.Column<string>(nullable: true),
                    is_visible = table.Column<bool>(nullable: false),
                    is_set = table.Column<bool>(nullable: false),
                    image_path = table.Column<string>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nomenclatures", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "url_access",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    url = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    acl = table.Column<int>(nullable: false),
                    access_groups_json = table.Column<string>(nullable: true),
                    denied_groups_json = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_url_access", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_discounts",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    discount_price = table.Column<double>(nullable: false),
                    is_active = table.Column<bool>(nullable: false),
                    start_date = table.Column<DateTime>(nullable: true),
                    end_date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_discounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "catalog_discounts",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    catalog_id = table.Column<Guid>(nullable: false),
                    discount_price = table.Column<double>(nullable: false),
                    is_active = table.Column<bool>(nullable: false),
                    start_date = table.Column<DateTime>(nullable: true),
                    end_date = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_discounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_catalog_discounts_dict_catalogs_catalog_id",
                        column: x => x.catalog_id,
                        principalTable: "dict_catalogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "visible_user_catalogs",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    catalog_id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false)
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
                    id = table.Column<Guid>(nullable: false),
                    nom_id = table.Column<Guid>(nullable: false),
                    product_id = table.Column<Guid>(nullable: false),
                    catalog_id = table.Column<Guid>(nullable: false),
                    order_form_type = table.Column<int>(nullable: false),
                    price = table.Column<double>(nullable: false),
                    discount = table.Column<double>(nullable: false),
                    amount = table.Column<int>(nullable: false),
                    total_amount = table.Column<double>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false)
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
                    id = table.Column<Guid>(nullable: false),
                    nom_id = table.Column<Guid>(nullable: false),
                    catalog_id = table.Column<Guid>(nullable: false),
                    order = table.Column<int>(nullable: false)
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
                    id = table.Column<Guid>(nullable: false),
                    nom_id = table.Column<Guid>(nullable: false),
                    category_id = table.Column<Guid>(nullable: false)
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
                    id = table.Column<Guid>(nullable: false),
                    nom_id = table.Column<Guid>(nullable: false),
                    discount_price = table.Column<double>(nullable: false),
                    is_active = table.Column<bool>(nullable: false),
                    start_date = table.Column<DateTime>(nullable: true),
                    end_date = table.Column<DateTime>(nullable: true)
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
                    id = table.Column<Guid>(nullable: false),
                    nom_id = table.Column<Guid>(nullable: false),
                    photo_path = table.Column<string>(nullable: true),
                    old_photo_path = table.Column<string>(nullable: true),
                    order = table.Column<int>(nullable: false),
                    color_id = table.Column<Guid>(nullable: true)
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
                    id = table.Column<Guid>(nullable: false),
                    nom_id = table.Column<Guid>(nullable: false),
                    max_amout = table.Column<int>(nullable: false),
                    counter = table.Column<int>(nullable: false)
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
                    id = table.Column<Guid>(nullable: false),
                    product_id = table.Column<Guid>(nullable: false),
                    nom_id = table.Column<Guid>(nullable: false),
                    price = table.Column<double>(nullable: false)
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
                    id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false),
                    nom_id = table.Column<Guid>(nullable: false),
                    catalog_id = table.Column<Guid>(nullable: false)
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
                    id = table.Column<Guid>(nullable: false),
                    product_id = table.Column<Guid>(nullable: false),
                    nom_id = table.Column<Guid>(nullable: false),
                    category_id = table.Column<Guid>(nullable: false)
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
                    id = table.Column<Guid>(nullable: false),
                    product_id = table.Column<Guid>(nullable: false),
                    nom_id = table.Column<Guid>(nullable: false),
                    catalog_id = table.Column<Guid>(nullable: false)
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
                    id = table.Column<Guid>(nullable: false),
                    nom_id = table.Column<Guid>(nullable: false),
                    user_id = table.Column<Guid>(nullable: false)
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

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "id", "action_name", "controller_name", "icon", "menu_type", "order", "parameters", "title", "url" },
                values: new object[,]
                {
                    { new Guid("7da8b97f-f713-4d83-a414-3e13879c22d7"), "Index", "Admin/Home", "fa-home", 999, 1, null, "Главная", null },
                    { new Guid("9e571433-8647-4baa-a32a-1a0dd3aa3594"), "Index", "Admin/Menu", "fa-bars", 999, 2, null, "Пункты меню", null },
                    { new Guid("7569c646-5756-4b38-bdb6-ecf4bbf75bc4"), "Index", "Admin/MBody", "fa-file-code", 999, 3, null, "Контент страниц", null },
                    { new Guid("173f6f93-0177-4660-a0a2-c6ce2884d388"), "Index", "Admin/Catalog", "fa-th", 999, 4, null, "Каталоги", null },
                    { new Guid("b22e9166-d1bf-4355-8e18-b0565578a8bf"), "Index", "Admin/Category", "fa-tags", 999, 5, null, "Категории", null },
                    { new Guid("cfb7f905-d13c-456a-bb88-029247edef6c"), "Index", "Admin/CModel", "fa-eye", 999, 6, null, "Модели", null },
                    { new Guid("5bd7f101-611e-47d1-9ad0-c827cb8aaa18"), "Index", "Admin/Discount", "fa-percent", 999, 7, null, "Скидки", null },
                    { new Guid("378cd91c-24e2-42ff-a044-afe097705c44"), "Index", "Admin/Blog", "fa-blog", 999, 8, null, "Блог", null },
                    { new Guid("c69c45ba-0735-43d0-a786-b8ff0843dd88"), "Index", "Admin/Gallery", "fa-images", 999, 9, null, "Галерея", null },
                    { new Guid("99956ad8-04b3-44ce-890a-9efb0538898f"), "Index", "Admin/Feedback", "fa-mail-bulk", 999, 10, null, "Сообщения", null },
                    { new Guid("80637bfc-18b9-4078-87cf-39786df2a9c7"), "Index", "Admin/Banner", "fa-bullhorn", 999, 12, null, "Баннер", null },
                    { new Guid("efba27c0-a25e-4839-b892-78be0a0124f3"), "Index", "Admin/UrlAcl", "fa-link", 999, 100, null, "Доступ по URL", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_nom_id",
                table: "cart_items",
                column: "nom_id");

            migrationBuilder.CreateIndex(
                name: "IX_catalog_discounts_catalog_id",
                table: "catalog_discounts",
                column: "catalog_id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "authorization_link");

            migrationBuilder.DropTable(
                name: "banner_items");

            migrationBuilder.DropTable(
                name: "blog_items");

            migrationBuilder.DropTable(
                name: "cart_items");

            migrationBuilder.DropTable(
                name: "catalog_discounts");

            migrationBuilder.DropTable(
                name: "dict_settings");

            migrationBuilder.DropTable(
                name: "feedback_items");

            migrationBuilder.DropTable(
                name: "gallery_items");

            migrationBuilder.DropTable(
                name: "menu_contents");

            migrationBuilder.DropTable(
                name: "menu_items");

            migrationBuilder.DropTable(
                name: "nom_catalog");

            migrationBuilder.DropTable(
                name: "nom_category");

            migrationBuilder.DropTable(
                name: "nom_discounts");

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
                name: "url_access");

            migrationBuilder.DropTable(
                name: "user_discounts");

            migrationBuilder.DropTable(
                name: "visible_nom_users");

            migrationBuilder.DropTable(
                name: "visible_user_catalogs");

            migrationBuilder.DropTable(
                name: "dict_category");

            migrationBuilder.DropTable(
                name: "nomenclatures");

            migrationBuilder.DropTable(
                name: "dict_catalogs");
        }
    }
}
