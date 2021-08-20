var soldItems = dataModel;
var historyItemsTable;
var modelInfo;

$(function () {
    historyItemsTable = $("#historyItemsTable").DataTable({
        "data": soldItems,
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
                            <div class="mb-3">Арт: ${full.name}</div>
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
                "data": "size.value",
                "width": "14%"
            },
            {
                "data": "amount",
                "width": "10%"
            }
        ],
        "language": {
            "url": "/common/datatables.Russian.json"
        },
        "dom": "r"
    });
});

const barcodeHandler = new BarcodeInputHandler({
    barcodeInfoInputSelector: "#modelInfo",
    onBarcodeInput: function (barcode) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: "GET",
                url: `/Partners/Shop/GetModelInfoByBarcode?barcode=${barcode}`,
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (result) {
                    if (result) {
                        modelInfo = result;
                        resolve(`Арт: ${result.articul} ${result.colorName} ${result.size.value}`)
                    } else {
                        modelInfo = null;
                        reject();
                    }
                },
                error: function (error) {
                    if (error.status == 401)
                        window.location.reload();
                    console.error(error);
                    reject();
                }
            });
        });
    }
});

function sellModelItem() {
    if (modelInfo) {
        $.ajax({
            type: "POST",
            url: "/Partners/Shop/SellModelItem",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ modelId: modelInfo.modelId, colorId: modelInfo.colorId, sizeValue: modelInfo.size.value })
        }).done(function (result) {
            barcodeHandler.cleanInputs();
            modelInfo = null;
            soldItems = result;
            redrawTableWithNewData(historyItemsTable, soldItems);
        });
    }
}