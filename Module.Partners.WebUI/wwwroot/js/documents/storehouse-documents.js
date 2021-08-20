$(function () {
    var documents = dataModel;

    flatpickr("[data-datetime-picker]", {
        allowInput: true,
        enableTime: false,
        locale: "ru",
        dateFormat: "d.m.Y"
    });
    
    const table = $(".documentsTable").DataTable({
        "data": documents,
        "responsive": true,
        "paging": false,
        "order": [[0, "desc"], [1, "desc"]],
        "columns": [
            {
                "name": "createDate",
                "data": "createDateAsString",
                "width": "10%",
                "render": function (data, type, full, meta) {
                    if(type === 'display') { 
                        return data;
                    }
                    
                    return full.createDate;
                }
            },
            {
                "data": "number",
                "width": "8%",
                "render": function (data, type, full, meta) {
                    if(type !== 'display'){
                        return data;
                    }
                    
                    return full.formattedNumber;
                }
            },
            {
                "name": "documentName",
                "data": "name",
                "width": "15%",
                "render": function (data, type, full, meta) {
                    if(type !== "display") {
                        return data;
                    }

                    if (full.canHaveGroupedItems) {
                        return `<a href='/Partners/Documents/Document/${full.number}?grouped=true'>${data}</a>`;
                    } else {
                        return `<a href='/Partners/Documents/Document/${full.number}'>${data}</a>`;
                    }
                }
            },
            {
                "data": "incomeAmount",
                "width": "10%"
            },
            {
                "data": "writeOffAmount",
                "width": "10%"
            },
            {
                "data": "name",
                "width": "47%",
                "sortable": false,
                "render": function (data, type, full, meta) { return "" }
            },
        ],
        "language": {
            "url": "/common/datatables.Russian.json"
        },
        "dom": "r"
    });
    table.on('search.dt', countDocumentTotalAmounts);

    new DataTableCustomFilter([table], {
        applyByButton: false,
        blockWrapperSelector: ".customSearchWrapper"
    });

    function countDocumentTotalAmounts() {
        const filteredData = table.rows({filter: 'applied'}).data();
        $('#totalIncomeAmount').html(_.sumBy(filteredData, x => x.incomeAmount));
        $('#totalWriteOffAmount').html(_.sumBy(filteredData, x => x.writeOffAmount));
    }
});