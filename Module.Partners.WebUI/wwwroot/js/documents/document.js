$(function () {
    var documentItems = dataModel;

    $(".documentItemsTable").DataTable({
        "data": documentItems,
        "responsive": true,
        "paging": false,
        "rowsGroup": ["itemInfo:name", "colorName:name"],
        "columns": [
            {
                "name": "itemInfo",
                "data": "itemGroupingKey",
                "width": "30%",
                "render": function (data, type, full, meta) {
                    const mainPhoto = String.isNullOrEmpty(full.mainPhoto) ? "/common/img/noimage.png?width=80" : `${full.mainPhoto}?width=80`;
                    var modelTotals = getModelTotals({ articul: full.articul, line: full.size.line });
                    return `
                    <div class="row m-0">
                        <div class="col-auto">
                            <a href="${full.mainPhoto}" data-lightbox="Изображение" data-title="${full.articul}">
                                 <picture  class="img-fluid rounded-1">
                                    <source srcset="${mainPhoto}&format=webp" type="image/webp">
                                    <img srcset="${mainPhoto}" class="img-fluid rounded-1" alt="${full.articul}" />
                                </picture>
                            </a>
                        </div>
                        <div class="col">
                            <div class="mb-3">Арт: ${full.articul}</div>
                            <div><span class="size-swatch swatch-light">${full.size.line}</span></div>
                            <div class="mt-2"><span>${Number.toTwoDecimalPlaces(full.price)} $ за ед</span></div>
                        </div>
                    </div>
                    <div class="row m-0 my-2 font-weight-bold">
                        <div class="col-auto px-0 mr-0 mr-lg-4">Итого:</div>
                        <div class="col px-4">
                            <div class="mx-2">Единиц: ${modelTotals.totalAmount}</div>
                            <div class="mx-2">Сумма: ${Number.toTwoDecimalPlaces(modelTotals.totalPrice)} $</div>
                        </div>
                    </div>`;
                }
            },
            {
                "name": "colorName",
                "data": "colorName",
                "width": "14%",
                "render": function (data, type, full, meta) {
                    const background = !full.colorImage ? full.colorCode : `url(${full.colorImage})`;
                    return `<span class="color-swatch swatch-md align-middle" data-toggle="tooltip" data-placement="top" title="${data}" style="background: ${background};"></span>
                            <span>${data}</span>`;
                }
            },
            {
                "data": "size",
                "width": "14%",
                "render": function (data, type, full, meta) {
                    return data.value;
                }
            },
            {
                "data": "amount",
                "width": "10%"
            },
            {
                "data": "totalPrice",
                "render": function (data, type, full, meta) {
                    return `${currencyFormatConvert(data)} $`;
                }
            }
        ],
        "language": {
            "url": "/common/datatables.Russian.json"
        },
        "dom": "r"
    });

    function getModelTotals(data) {
        const items = _.filter(documentItems, function (object) { return object.articul === data.articul && object.size.line === data.line });
        let result = {
            totalAmount: 0,
            totalPrice: 0
        }

        for (let item of items) {
            result.totalAmount += item.amount;
            result.totalPrice += item.price * item.amount;
        }
        return result;
    }
});