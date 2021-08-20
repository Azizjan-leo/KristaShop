$(function () {
    const documents = dataModel;
    var tables = [];
    for (let document of documents) {
        let table = $(`.itemsTable_${document.id}`).DataTable({
            "data": document.items,
            "responsive": true,
            "paging": false,
            "columns": getColumnsDefinitions(),
            "order": [[0, "desc"]],
            "language": {
                "url": "/common/datatables.Russian.json"
            },
            "dom": "r"
        });
        
        tables.push(table);
    }

    function getColumnsDefinitions() {
        return [
            {
                "name": "date",
                "width": "8%",
                "data": "date",
                "render": function (data, type, full, meta) {
                    if (type !== 'display') {
                        return data;
                    }

                    return full.dateFormatted;
                },
                "className": "align-middle"
            },
            {
                "name": "modelKey",
                "data": "modelName",
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
                        <div class="d-inline-block px-3">
                            <div class="mb-3">Арт: ${data}</div>
                        </div>
                    </div>`;
                },
                "className": "align-middle"
            },
            {
                "name": "allColors",
                "data": "colorName",
                "width": "14%", 
                "render": function (data, type, full, meta) {
                    const background = !full.colorImage ? full.colorCode : `url(${full.colorImage})`;
                    return `<span class="color-swatch swatch-md align-middle" data-toggle="tooltip" data-placement="top" title="${data}" style="background: ${background};"></span>
                            <span>${data}</span>`;
                },
                "className": "align-middle"
            },
            {
                "name": "allSizes",
                "data": "size.value",
                "width": "9%",
                "className": "align-middle"
            },
            {
                "data": "amount",
                "width": "12%",
                "className": "align-middle"
            },
            {
                "data": "totalPrice",
                "width": "11%",
                "render": function (data, type, full, meta) {
                    return `${currencyFormatConvert(data)} $`;
                },
                "className": "align-middle"
            },
            {
                "name": "documentName",
                "data": "fromDocumentName",
                "width": "15%",
                "className": "align-middle"
            }
        ];
    }

    new DataTableCustomFilter(tables, {
        applyByButton: false,
        blockWrapperSelector: ".customSearchWrapper"
    });
});