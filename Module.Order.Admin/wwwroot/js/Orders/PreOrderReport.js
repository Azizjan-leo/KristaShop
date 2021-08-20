var table;
var dateFormat = "DD.MM.YYYY HH:mm";

$(document).ready(function () {
    table = $('.table').DataTable({
        "ajax": {
            "url": "/Admin/Orders/LoadPreOrderReportData",
            "type": "GET",
            "datatype": "json",
            "dataSrc": "",
            "error": function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            }
        },
        "responsive": true,
        "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",
        "columns": [
            {
                "name": "mainPhoto",
                "data": "mainPhoto",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    const photo = data ? data : "/common/img/noimage.png?width=100";
                    return `<picture>
                                <source srcset="${photo}&format=webp" type="image/webp">
                                <img srcset="${data}?width=100" width="100" height="150" />
                            </picture>`;
                },
                "className": "min-desktop"
            },
            {
                "name": "articul",
                "data": "articul"
            },
            {
                "name": "colorName",
                "data": "colorName",
                "render": function (data, type, full, meta) {
                    var background = full.colorPhoto != "" ? full.colorValue : "url(" + full.ColorPhoto + ")";
                    return `<div class="tal" style="text-align: left;">
                        <span class="color-round" title="${full.colorName}" style="background: ${background};"></span>
                        <div>${data}</div>
                    </div>`;
                }
            },
            {
                "name": "colorId",
                "data": "colorId",
                "sortable": false,
                "visible": false,
                "defaultContent": 0
            },
            {
                "name": "sizeValue",
                "data": "sizeValue"
            },
            {
                "name": "amount",
                "data": "amount",
                "searchable": false,
                "className": "min-desktop"
            },
            {
                "name": "price",
                "data": "price",
                "defaultContent": 0.0,
                "width": "15%",
                "render": function (data, type, full, meta) {
                    return currencyFormatConvert(data);
                },
                "className": "min-desktop"
            },
            {
                "name": "totalSum",
                "data": "price",
                "defaultContent": 0.0,
                "width": "15%",
                "render": function (data, type, full, meta) {
                    return currencyFormatConvert(full.price * full.amount);
                },
                "className": "min-desktop"
            },
            {
                "data": "articul",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return '<a href="/Admin/Orders/Index?modelId=' + encodeURIComponent(full.modelId) + '&colorId=' + encodeURIComponent(full.colorId) + '" target="_blank" class="btn btn-square btn-main-info jsShowRelatedOrdersList" data-toggle="tooltip" data-placement="bottom" title="Список заказов, содержащих данную позицию"><svg class="krista-icon krista-shop"><use xlink:href="#krista-shop"></use></svg></a>';
                },
                "className": "min-desktop"
            }
        ],
        "pageLength": -1,
        "lengthMenu": [[10, 50, 100, -1], [10, 50, 100, "Все"]],
        "language": {
            "url": '/common/datatables.Russian.json'
        }
    })
    .order([[1, "asc"], [4, "asc"], [2, "asc"]]);

    filterOnLoad();

    // Table filter applied to any input attribute with data-target-col="table_column_name"
    $("input[data-target-col]").on("keyup", onFilterInputChanged);
    $("select[data-target-col]").on("change", onFilterInputChanged);

    function filterOnLoad() {
        $("[data-target-col]").each(function(index) {
            setColumnFilter(this.dataset.targetCol, this.value);
        });

        applyFilter();
    }

    function onFilterInputChanged(event) {
        setColumnFilter(this.dataset.targetCol, this.value);
        applyFilter();
    }

    function setColumnFilter(colName, value) {
        if (value == "") {
            table
                .columns(`${colName}:name`)
                .search(value);
        } else {
            if (colName != "articul") {
                table
                    .columns(`${colName}:name`)
                    .search("(^" + value + "$)", true, false);
            } else {
                table
                    .columns(`${colName}:name`)
                    .search(value);
            }
        }
    }

    function applyFilter() {
        table.draw();
    }
});


function ResetOrderAllValues() {
    $('.dataTables_customSearchWrapper').find('.order-input:text').val('').trigger('keyup');
    $('.dataTables_customSearchWrapper').find('.selectpicker').val('').trigger('change');
    table.draw();
}
