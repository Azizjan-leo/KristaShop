$(function () {
    const documentItems = dataModel;

    function addColumnsForFilterToDataSource() {
        for(var item of documentItems) {
            const sizes = _.chain(item.sizesInfo.totalAmountBySize).toPairs().filter(x => x[1] > 0).map(x => x[0]).value();
            sizes.push(item.modelInfo.size.line);

            item.sizesString = sizes.join(" ");
            item.colorsString = _.map(item.colors, x => x.name).join(" ");
        }
    }
    addColumnsForFilterToDataSource();
    
    const table = $(".documentItemsTable").DataTable({
        "data": documentItems,
        "responsive": true,
        "paging": false,
        "columns": [
            {
                "name": "modelKey",
                "data": "modelKey",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    if (type !== "display") {
                        return full.modelInfo.name;
                    }

                    return getModelGridRenderer().renderModelGrid(full);
                }
            },
            {
                "name": "allColors",
                "data": "colorsString",
                "visible": false
            },
            {
                "name": "allSizes",
                "data": "sizesString",
                "visible": false
            },
            {
                "name": "totalAmount",
                "data": "totalAmount",
                "visible": false,
            },
            {
                "name": "totalSum",
                "data": "totalSum",
                "visible": false,
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

    initDataTablesCustomSorting({
        table: table,
        sortByColumn: {name: "modelKey", direction: "asc"}
    });

    const filter = new DataTableCustomFilter([table], {
        applyByButton: false,
        blockWrapperSelector: ".customSearchWrapper"
    });
});