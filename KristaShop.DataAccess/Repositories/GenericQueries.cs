using KristaShop.Common.Enums;
using KristaShop.Common.Models;
using SqlKata;

namespace KristaShop.DataAccess.Repositories {
    public static class GenericQueries {
        public static Query GetModelPhotoByColorInnerQuery() {
            return new Query("model_photos_1c AS model_photos")
                .Select("model_photos.articul", "model_photos.photo_path", "model_photos.color_id")
                .WhereNotNull("model_photos.color_id")
                .GroupBy("model_photos.articul", "model_photos.color_id");
        }

        public static Query JoinModelCollection(this Query query, string fromTable, string collectionFkFieldName = "collection") {
            return query.SelectRaw("IFNULL(`collection`.`id`, -1) AS `collection_id`")
                .SelectRaw("IF(`collection`.`procent` IS NULL OR `collection`.`procent` = 0, ?, `collection`.`procent`) AS `collection_percent`",
                    GlobalConstant.GeneralPrepayPercentValue)
                .SelectRaw("IFNULL(`collection`.`name`, 'Коллекция №1') AS `collection_name`")
                .LeftJoin("1c_collection AS collection", $"{fromTable}.{collectionFkFieldName}", "collection.id");
        }

        public static Query JoinCatalogItemVisibility(this Query query, string modelTableAlias, string catalogTableAlias, string tableWithCatalogIdAlias) {
            return query.SelectRaw("IFNULL(`item_visibility`.`is_visible`, 1) AS `is_visible_color`")
                .LeftJoin("catalog_item_visibility AS item_visibility",
                    x => x.WhereRaw(
                        $@"`{modelTableAlias}`.`id` = `item_visibility`.`model_id` AND `{catalogTableAlias}`.`color` = `item_visibility`.`color_id` AND `{tableWithCatalogIdAlias}`.`razdel` = `item_visibility`.`catalog_id`
 AND (((`{catalogTableAlias}`.`razmer` = '' OR `{catalogTableAlias}`.`razmer` = '0') AND `{modelTableAlias}`.`line` = `item_visibility`.`size_value`) OR `{catalogTableAlias}`.`razmer` = `item_visibility`.`size_value`)"));
        }

        public static Query OrderByDirection(this Query query, CatalogOrderDir orderDirection, params string[] columns) {
            return orderDirection == CatalogOrderDir.Asc ? query.OrderBy(columns) : query.OrderByDesc(columns);
        }
    }
}
