var table;
$(document).ready(function () {
    table = $(".table").DataTable({
        "ajax": {
            "url": `/Admin/OrderReports/LoadCitiesOrdersReport`,
            "type": "POST",
            "datatype": "json",
            "data": getFilterData(),
            "dataSrc": ""
        },
        "rowsGroup": ["cityName:name"],
        "processing": true,
        "columns": [
            {
                "name": "cityId",
                "data": "cityId",
                "visible": false
            },
            {
                "name": "cityName",
                "data": "cityName"
            },
            {
                "name": "userId",
                "data": "userId",
                "visible": false
            },
            {
                "name": "userFullName",
                "data": "userFullName"
            },
            {
                "name": "managerId",
                "data": "managerId",
                "visible": false
            },
            {
                "name": "managerName",
                "data": "managerName"
            },
            {
                "name": "totalAmount",
                "data": "totalAmount"
            },
            {
                "name": "totalSum",
                "data": "totalSum",
                "render": function (data, type, full, meta) {
                    return `<div>${Number.toTwoDecimalPlaces(data)} $</div>
                            <div class="text-main-secondary">${Number.toTwoDecimalPlaces(full.totalSumInRub)} р</div>`;
                }
            },
            {
                "name": "userActions",
                "data": "id",
                "width": "10%",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<a href="/Admin/Identity/OpenUserCart?userId=${full.userId}" class="btn btn-square btn-main-success mt-1 ${!full.userHasCartItems ? "disabled" : ""} mr-2"><i class="fas fa-shopping-cart"></i></a>
                            <a href="/Admin/Orders/Index/?ClientFullName=${full.userFullName}"  class="btn btn-square btn-main-success  ${full.totalAmount <= 0 ? "disabled" : ""}" data-toggle="tooltip" data-placement="bottom" title="Подробно"><svg class="krista-icon krista-eye"><use xlink:href="#krista-eye"></use></svg></a>`;
                },
                "className": "min-desktop"
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rt<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>',
        "pageLength": 50,
        "order": [[0, "asc"]]
    });

    function getFilterData() {
        return {
            selectedArticuls: $("#SelectedArticuls").find(":selected").toArray().map(x => x.value),
            selectedColorIds: $("#SelectedColorIds").find(":selected").toArray().map(x => x.value),
            selectedCityIds: $("#SelectedCityIds").find(":selected").toArray().map(x => x.value),
            selectedUserIds: $("#SelectedUserIds").find(":selected").toArray().map(x => x.value),
            selectedManagerIds: $("#SelectedManagerIds").find(":selected").toArray().map(x => x.value),
            selectedCatalogIds: $("#SelectedCatalogIds").find(":selected").toArray().map(x => x.value),
            onlyUnprocessedOrders: $("#OnlyUnprocessedOrders").is(":checked"),
            onlyNotEmptyUserCarts: $("#OnlyNotEmptyUserCarts").is(":checked")
        };
    }
});