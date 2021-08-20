var catalogItems = dataModel;
var table;
$(document).ready(function () {
    table = $('.table').DataTable({
        "data": catalogItems,
        "responsive": true,
        "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",
        "processing": true,
        "columns": [
            {
                "data": "isVisible",
                "searchable": false,
                "sortable": false,
                "width": "2px",
                "render": function (data, type, full, meta) {
                    const bg = data ? "bg-main-success" : "bg-main-danger";
                    return `<div class="w-100 h-100 position-absolute" style="top: 0; left: 0;"><div class="${bg}" style="min-height: calc(100% - 10px); margin: 5px 0;"></div></div>`;
                },
                "class": "p-0 m-0 position-relative"
            },
            {
                "data": "order",
                "name": "order",
                "render": function (data, type, full, meta) {
                    let count = meta.row;
                    count = count + 1;
                    return count;
                },
                "class": "order-column min-desktop"
            },
            {
                "name": "articul",
                "data": "articul",
                "sortable": false,
                "visible": false
            },
            {
                "data": "name",
                "render": function (data, type, full, meta) {
                    const path = full.mainPhoto ? full.mainPhoto : "/common/img/nophoto.png";
                    return `<div class='row'><div class='col-auto p-0'>
                                <picture>
                                    <source srcset="${path}?width=80&format=webp" type="image/webp">
                                    <img srcset="${path}?width=80" width="80" alt="${full.articul}" />
                                </picture>
                            </div><div class='col pl-3 pr-0'><div>Арт: ${data}</div><div>${full.colors.join(", ")}</div></div></div>`;
                }
            },
            {
                "name": "colors",
                "data": "colors",
                "sortable": false,
                "visible": false
            },
            {
                "name": "sizes",
                "data": "sizes",
                "width": "10%",
                "render": function (data, type, full, meta) {
                    if (data) {
                        return data.join(", ");
                    } else {
                        return "Отсутствует";
                    }
                },
                "className": "min-desktop"
            },
            {
                "name": "catalogs",
                "data": "catalogs",
                "defaultContent": "Отсутствует",
                "width": "15%",
                "render": function (data, type, full, meta) {
                    if (data) {
                        return data.join(", ");
                    } else {
                        return "Отсутствует";
                    }
                },
                "className": "min-desktop"
            },
            {
                "name": "categories",
                "data": "categories",
                "defaultContent": "Отсутствует",
                "width": "15%",
                "render": function (data, type, full, meta) {
                    if (data) {
                        return data.join(", ");
                    } else {
                        return "Отсутствует";
                    }
                },
                "className": "min-desktop"
            },
            {
                "name": "modelIsVisible",
                "data": "isVisible",
                "sortable": false,
                "visible": false,
                "width": "1%",
                "render": function (data, type, full, meta) {
                    if (data) {
                        return "1";
                    } else {
                        return "0";
                    }
                }
            },
            {
                "data": "articul",
                "width": "7%",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<button data-href="/Admin/ModelsCatalog/Edit/?id=${encodeURIComponent(data)}" class="btn btn-square btn-main-primary mr-2" data-toggle="tooltip" data-placement="bottom" title="Изменить" onclick="EditModel(this);"><svg class="krista-icon krista-edit"><use xlink:href="#krista-edit"></use></svg></button>`;
                },
                "className": "min-desktop"
            }
        ],
        "pageLength": -1,
        "lengthMenu": [[10, 50, 100, -1], [10, 50, 100, "Все"]],
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>'
    });

// #region filter
    //Table filter applied to any input attribute with data-target-col="table_column_name", column should have attribute "name"
    $("input[data-target-col]").on("keyup", onFilterInputChanged);
    $("select[data-target-col]").on("change", onFilterInputChanged);

    function onFilterInputChanged(event) {
        setColumnFilter(this.dataset.targetCol, this.value);
        applyFilter();
    }

    function setColumnFilter(colName, value) {
        table
            .columns(`${colName}:name`)
            .search(value);
    }

    $("input[data-target-col]").keyup();

    function applyFilter() {
        table.draw();
    }
// #endregion
});

function ResetAllValues() {
    $(".dataTables_customSearchWrapper").find("input:text").val("").trigger("keyup").focus();
    $(".dataTables_customSearchWrapper").find(".selectpicker").val("").trigger("change");
}

function EditModel(btnObj) {
    var url = $(btnObj).attr("data-href");

    var articul = $("input#Articul").val();

    window.location.href = url + (articul != "" ? "&articul=" + encodeURIComponent(articul) : "");
}
