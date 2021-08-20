var table;
var dateFormat = "D.MM.YYYY HH:mm";
$(document).ready(function () {
    table = $('.table').DataTable({
        "data": dataModel,
        "order": [[0, "asc"]],
        "responsive": true,
        "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",
        "columns": [
            {
                "name": "person",
                "data": "fullName",
                "render": function (data, type, full, meta) {
                    return `<div>${data}</div><div class="text-main-secondary">${full.managerName}</div>`;
                }
            },
            {
                "name": "fullName",
                "data": "fullName",
                "visible": false
            },
            {
                "name": "login",
                "data": "login",
                "visible": false
            },
            {
                "name": "contacts",
                "data": "phone",
                "render": function (data, type, full, meta) {
                    return `<div>${data ? data : "Отсутствует"}</div><div class="text-main-secondary">${full.email ? full.email : "Отсутствует"}</div>`;
                },
                "className": "min-desktop"
            },
            {
                "name": "email",
                "data": "email",
                "visible": false,
                "defaultContent": "Отсутствует"
            },
            {
                "name": "phone",
                "data": "phone",
                "visible": false,
                "defaultContent": "Отсутствует"
            },
            {
                "name": "cityName",
                "data": "cityName",
                "render": function (data, type, full, meta) {
                    return `<div>${data ? data : "Отсутствует"}</div><div class="text-main-secondary">${full.mallAddress ? full.mallAddress : "Отсутствует"}</div>`;
                },
                "className": "min-desktop"
            },
            {
                "name": "managerName",
                "data": "managerName",
                "visible": false,
                "defaultContent": "---",
                "className": "min-desktop"
            },
            {
                "name": "shipments",
                "data": "shipmentsItemsCount",
                "width": "10%",
                "render": function (data, type, full, meta) {
                    return `<div>${data} / ${full.shipmentsItemsSum}$</div>`;
                },
                "className": "min-desktop"
            },

            {
                "name": "storehouse",
                "data": "storehouseItemsCount",
                "width": "10%",
                "render": function(data, type, full, meta) {
                    return `<div>${data}</div>`;
                },
                "className": "min-desktop"
            },
            {
                "name": "debt",
                "data": "debtItemsCount",
                "searchable": false,
                "sortable": true,
                "width": "75px",
                "render": function (data, type, full, meta) {
                    return `<div>${data} / ${full.debtItemsSum}$</div>`;
                },
                "className": "min-desktop"
            },
            {
                "name": "revisionDate",
                "data": "revisionDate",
                "searchable": false,
                "sortable": true,
                "width": "75px",
                "render": function (data, type, full, meta) {
                    return data ? `<div>${full.revisionDateFormatted}</div>` : "<div>-----</div>";
                },
                "className": "min-desktop"
            },
            {
                "name": "paymentDate",
                "data": "paymentDate",
                "searchable": false,
                "sortable": true,
                "width": "75px",
                "render": function (data, type, full, meta) {
                    return data ? `<div>${full.paymentDateFormatted}</div>` : "<div>-----</div>";
                },
                "className": "min-desktop"
            },
            {
                "name": "action",
                "data": "userId",
                "width": "1%",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<a href="/Admin/Partnership/UserActivePayments?userId=${data}" class="btn btn-square btn-main-success mt-1 mr-2"><i class="fas fa-dollar-sign align-middle"></i></a>`;
                },
                "className": "min-desktop"
            }
        ],
        "createdRow": function(row, data, dataIndex) {
            if (data["userId"] <= 0) {
                $(row).addClass("text-danger");
            }
        },
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>',
        "pageLength": 50
    });
});

function ResetFilter() {
    $(".selectpicker").val('').trigger('change');
    for(var picker of $("[data-date-period-picker]")) {
        picker._flatpickr.clear();
    }
}