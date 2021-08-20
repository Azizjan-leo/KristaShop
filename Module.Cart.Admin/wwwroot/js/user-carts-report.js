var table;
$(document).ready(function () {
    table = $(".table").DataTable({
        "ajax": {
            "url": `/Admin/Cart/LoadUserCartsReport`,
            "type": "POST",
            "datatype": "json",
            "data": getFilterData(),
            "dataSrc": ""
        },
        "processing": true,
        "columns": [
            {
                "name": "userId",
                "data": "userId",
                "visible": false
            },
            { "data": "userFullName" },
            {
                "name": "cityId",
                "data": "cityId",
                "visible": false
            },
            { "data": "cityName" },
            {
                "name": "managerId",
                "data": "managerId",
                "visible": false
            },
            { "data": "managerName" },
            { "data": "totalItemsCount" },
            {
                "name": "totalPrice",
                "data": "totalPrice",
                "render": function (data, type, full, meta) {
                    return `<div>${currencyFormatConvert(data)} $</div><div class="text-main-secondary">${currencyFormatConvert(full.totalPriceRub)} р</div>`;
                }
            },
            {
                "name": "cartStatusIcon",
                "data": "cartStatus",
                "width": "6%",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<a href="/Admin/Identity/OpenUserCart?userId=${full.userId}" class="btn btn-square btn-main-success"><i class="fas fa-shopping-cart"></i></a>`;
                }
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rt<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>',
        "pageLength": 50
    });

    function getFilterData() {
        return {
            selectedArticuls: $("#SelectedArticuls").find(":selected").toArray().map(x => x.value),
            selectedColorIds: $("#SelectedColorIds").find(":selected").toArray().map(x => x.value),
            selectedCityIds: $("#SelectedCityIds").find(":selected").toArray().map(x => x.value),
            selectedUserIds: $("#SelectedUserIds").find(":selected").toArray().map(x => x.value),
            selectedManagerIds: $("#SelectedManagerIds").find(":selected").toArray().map(x => x.value),
            selectedCatalogIds: $("#SelectedCatalogIds").find(":selected").toArray().map(x => x.value)
        };
    }
});