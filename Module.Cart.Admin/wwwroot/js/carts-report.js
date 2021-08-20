var table;
$(document).ready(function () {
    const cartItems = dataModel;
    const itemAmountGroupingKey = "itemAmountGroupingKey";
    const ordersTotalLinesCountColName = "ordersTotalLinesCount";
    const ordersTotalAmountColName = "ordersTotalAmount";
    const ordersTotalSumColName = "ordersTotalSum";
    const ordersCatalogNameColName = "ordersCatalogName";
    var currentOrderColumn = "";
    table = $(".table").DataTable({
        "data": cartItems,
        "processing": true,
        "rowsGroup": [`${itemAmountGroupingKey}:name`, "itemInfo:name", `${ordersCatalogNameColName}:name`],
        "rowsGroupSortable": {
            "beforeApplyColumnSort": function (column) {
                if (column.isOrigin) {
                    currentOrderColumn = "";
                } else {
                    currentOrderColumn = column.name;
                }
                table.order().push([table.column("colorName:name").index(), "desc"]);
            }
        },
        "columns": [
            {
                "name": itemAmountGroupingKey,
                "data": "articul",
                "visible": false,
                "render": function (data, type, full, meta) {
                    return getGroupSortingValue({ articul: full.articul, sizeValue: full.sizeValue });
                }
            },
            {
                "name": "itemInfo",
                "data": "itemGroupingKey",
                "width": "30%",
                "rowsGroupSortableTarget": "itemInfo",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    if (type === "sort") {
                        return `${full.articul}_${full.sizeValue}`;
                    }

                    const modelTotals = getModelTotals({ articul: full.articul, sizeValue: full.sizeValue });
                    const mainPhoto = String.isNullOrEmpty(full.mainPhoto) ? "/common/img/noimage.png?width=80" : `${full.mainPhoto}?width=80`;
                    return `
                    <div class="row m-0">
                        <div class="col-auto">
                            <a href="${full.mainPhoto}" data-lightbox="Изображение" data-title="${full.articul}">
                                 <picture class="img-fluid rounded-1">
                                    <source srcset="${mainPhoto}&format=webp" type="image/webp">
                                    <img srcset="${mainPhoto}" class="img-fluid rounded-1" alt="${full.articul}" />
                                </picture>
                            </a>
                        </div>
                        <div class="col">
                            <div class="mb-3">Арт: ${full.articul}</div>
                            <div><span class="size-swatch swatch-light">${full.sizeValue}</span></div>
                            <div class="mt-2"><span>${Number.toTwoDecimalPlaces(full.price)} $ за ед</span></div>
                        </div>
                    </div>
                    <div class="row m-0 my-2 font-weight-bold">
                        <div class="col-auto px-0 mr-0 mr-lg-4">Итого:</div>
                        <div class="col px-4">
                            <div class="mx-2">Линеек: ${modelTotals.totalLines}</div>
                            <div class="mx-2">Единиц: ${modelTotals.totalAmount}</div>
                            <div class="mx-2">Сумма: ${Number.toTwoDecimalPlaces(modelTotals.totalPrice)} $</div>
                        </div>
                    </div>`;
                }
            },
            {
                "name": "articul",
                "data": "articul",
                "visible": false
            },
            {
                "name": "userId",
                "data": "userId",
                "visible": false
            },
            {
                "name": "managerId",
                "data": "managerId",
                "visible": false
            },
            {
                "name": "cityId",
                "data": "cityId",
                "visible": false
            },
            {
                "name": "colorId",
                "data": "colorId",
                "visible": false
            },
            {
                "name": "colorName",
                "data": "colorName",
                "width": "14%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    const background = !full.colorPhoto ? full.colorValue : `url(${full.colorPhoto})`;
                    return `<span class="color-swatch swatch-md align-middle" data-toggle="tooltip" data-placement="top" title="${data}" style="background: ${background};"></span>
                            <span>${data}</span>`;
                }

            },
            {
                "name": ordersTotalLinesCountColName,
                "data": "amount",
                "width": "14%",
                "rowsGroupSortableTarget": itemAmountGroupingKey,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return full.amount / (full.partsCount <= 0 ? 1 : full.partsCount);
                }
            },
            {
                "name": ordersTotalAmountColName,
                "data": "amount",
                "width": "14%",
                "rowsGroupSortableTarget": itemAmountGroupingKey,
                "sortable": false
            },
            {
                "name": ordersTotalSumColName,
                "data": "totalPrice",
                "rowsGroupSortableTarget": itemAmountGroupingKey,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `${currencyFormatConvert(data)} $`;
                }
            },
            {
                "name": "catalogId",
                "data": "catalogId",
                "visible": false
            },
            {
                "name": ordersCatalogNameColName,
                "data": "catalogName",
                "rowsGroupSortableTarget": itemAmountGroupingKey,
                "sortable": false
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rt<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>',
        "pageLength": 100,
        "order": [[1, "asc"]]
    });

    function getGroupSortingValue(data) {
        if (String.isNullOrEmpty(currentOrderColumn)) {
            return 0;
        }

        const items = _.filter(cartItems, function (object) { return object.articul === data.articul && object.sizeValue === data.sizeValue });
        let result = 0;
        if (currentOrderColumn === ordersTotalAmountColName) {
            return _.sumBy(items, function (item) { return item.amount; });
        }

        if (currentOrderColumn === ordersTotalLinesCountColName) {
            return _.sumBy(items, function (item) { return item.amount / (item.partsCount <= 0 ? 1 : item.partsCount); });
        }

        if (currentOrderColumn === ordersTotalSumColName) {
            return _.sumBy(items, function (item) { return Math.ceil(item.price * item.amount); });
        }

        if (currentOrderColumn === ordersCatalogNameColName && items.length > 0) {
            return items[0].catalogName;
        }

        return result;
    }

    function getModelTotals(data) {
        const items = _.filter(cartItems, function (object) { return object.articul === data.articul && object.sizeValue === data.sizeValue });
        let result = {
            totalAmount: 0,
            totalLines: 0,
            totalPrice: 0
        }

        for (let item of items) {
            result.totalAmount += item.amount;
            result.totalLines += item.amount / (item.partsCount <= 0 ? 1 : item.partsCount);
            result.totalPrice += item.price * item.amount;
        }
        return result;
    }
});