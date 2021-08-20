$(function () {
    const groupedItemsList = dataModel;

    for (let group of groupedItemsList) {
        $(`.itemsTable_${moment(group.createDate).format("DDMMYYYY")}`).DataTable({
            "data": group.items,
            "responsive": true,
            "paging": false,
            "columns": getColumnsDefinitions(),
            "rowsGroup": ["itemInfo:name"],
            "language": {
                "url": "/common/datatables.Russian.json"
            },
            "dom": "r"
        });
    }

    function getColumnsDefinitions() {
        return [
            {
                "name": "itemInfo",
                "data": "itemGroupingKey",
                "width": "30%",
                "render": function (data, type, full, meta) {
                    const mainPhoto = String.isNullOrEmpty(full.mainPhoto) ? "/common/img/noimage.png?width=80" : `${full.mainPhoto}?width=80`;
                    return `
                    <div class="p-0">
                        <a href="${full.mainPhoto}" data-lightbox="Изображение" data-title="${full.articul}">
                            <picture class="img-fluid rounded-1">
                                <source srcset="${mainPhoto}&format=webp" type="image/webp">
                                <img srcset="${mainPhoto}" class="img-fluid rounded-1" alt="${full.articul}" />
                            </picture>
                        </a>
                        <div class="d-inline-block align-top px-3">
                            <div class="mb-3">Арт: ${full.articul}</div>
                            <div><span class="size-swatch swatch-light">${full.size.value}</span></div>
                            <div class="mt-2"><span>${Number.toTwoDecimalPlaces(full.price)} $ за ед</span></div>
                        </div>
                    </div>`;
                }
            },
            {
                "data": "colorName",
                "width": "14%",
                "render": function (data, type, full, meta) {
                    const background = !full.colorPhoto ? full.colorValue : `url(${full.colorPhoto})`;
                    return `<span class="color-swatch swatch-md align-middle" data-toggle="tooltip" data-placement="top" title="${data}" style="background: ${background};"></span>
                            <span>${data}</span>`;
                }
            },
            {
                "data": "amount",
                "width": "14%",
                "render": function (data, type, full, meta) {
                    return full.amount / full.size.parts;
                }
            },
            {
                "data": "amount",
                "width": "21%"
            },
            {
                "data": "totalPrice",
                "width": "21%",
                "render": function (data, type, full, meta) {
                    return `${currencyFormatConvert(data)} $`;
                }
            }
        ];
    }
});