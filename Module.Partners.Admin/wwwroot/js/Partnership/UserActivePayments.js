$(function () {
    const documents = dataModel;

    for (let document of documents) {
        $(`.itemsTable_${document.id}`).DataTable({
            "data": document.items,
            "responsive": true,
            "paging": false,
            "columns": getColumnsDefinitions(),
            "order": [[0, "desc"]],
            "autoWidth": false,
            "language": {
                "url": "/common/datatables.Russian.json"
            },
            "dom": "r"
        });
    }
    
    function getColumnsDefinitions() {
        return [
            {
                "name": "date",
                "data": "date",
                "width": "8%",
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
                        <picture class="img-fluid rounded-1">
                            <source srcset="${mainPhoto}&format=webp" type="image/webp">
                            <img srcset="${mainPhoto}" class="img-fluid rounded-1" alt="${full.articul}" />
                        </picture>
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
                "className": "align-middle"
            }
        ];
    }
}); 

function ResetReportFilter() {
    $(".selectpicker").val('').trigger('change');
    for(var picker of $("[data-date-period-picker]")) {
        picker._flatpickr.clear();
    } 
}