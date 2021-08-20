$(document).ready(function () {
    table = $(".salesTable").DataTable({
        "data": tableData,
        "responsive": true,
        "columns": [
            {
                "data": "userId",
                visible: false
            },
            {
                "data": "userFullName",
                "className": "min-desktop"
            },
            {
                "data": "cityId",
                visible: false
            },
            {
                "data": "cityName",
                "render": function (data, type, full, meta) {
                    return `<div>${data ? data : "Отсутствует"}</div><div class="text-main-secondary">${full.mallAddress ? full.mallAddress : "Отсутствует"}</div>`;
                },
                "className": "min-desktop"
            },
            {
                "data": "managerId",
                visible: false
            },
            {
                "data": "managerName",
                "render": function (data, type, full, meta) {
                    return `<div>${data}</div>`;
                },
                "className": "min-desktop"
            },
            {
                "data": "amount",
                "render": function (data, type, full, meta) {
                    return `<div>${data}</div>`;
                },
                "className": "min-desktop"
            },
            {
                "data": "sum",
                "searchable": false,
                "render": function (data, type, full, meta) {
                    return `<div>${currencyFormatConvert(data)}$</div>`;
                },
                "className": "min-desktop"
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>'
    })
});

function ResetReportFilter() {
    $(".selectpicker").val('').trigger('change');
    $("#filterSubmitBtn").click();
}