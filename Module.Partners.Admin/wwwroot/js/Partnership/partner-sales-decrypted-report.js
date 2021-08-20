$(function () {
    const movementGroup = dataModel;

    flatpickr("[data-date-period-picker]", {
        allowInput: true,
        enableTime: false,
        locale: "ru",
        dateFormat: "d.m.Y",
        theme: "light",
        mode: "range",
        maxDate: "today"
    });

    const table = $(".storehouseMovementsTable").DataTable({
        "data": movementGroup,
        "responsive": true,
        "paging": true,
        "columns": [
            {
                "name": "modelKey",
                "data": "modelKey",
                "width": "100%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    if (type !== "display") {
                        return full.modelInfo.name;
                    }

                    return getMovementGridRenderer().renderGrid(full);
                }
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
        "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>'
    });

    initDataTablesCustomSorting({
        table: table,
        sortByColumn: { name: "modelKey", direction: "asc" }
    });
});

function ResetReportFilter() {
    var formObj = $("form#SalesReportForm");
    $("input.order-input").val("");
    $("form#SalesReportForm .selectpicker").val("");
    $("form#SalesReportForm input[type='checkbox']").prop("checked", false);
    $("form#SalesReportForm input.datetimepicker-input").val("");

    formObj.submit();
}