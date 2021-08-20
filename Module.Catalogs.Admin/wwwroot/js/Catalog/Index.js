var table;
$(document).ready(function () {
    table = $('.table').DataTable({
        "ajax": {
            "url": "/Admin/Catalog/LoadData",
            "type": "GET",
            "datatype": "json",
            "dataSrc": ""
        },
        "responsive": true,
        "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",
        "rowReorder": {
            "dataSrc": 'order'
        },
        "columns": [
            {
                "data": "order",
                "className": "reorder min-desktop"
            },
            { "data": "name" },
            {
                "data": "uri",
                "className": "min-desktop"
            },
            {
                "data": "id",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    if (data) {
                        return data + ' (' + full.catalog1CName + ')';
                    }
                },
                "defaultContent": "0 (Открытый каталог)",
                "className": "min-desktop"
            },
            {
                "data": "orderFormName",
                "className": "min-desktop"
            },
            {
                "data": "isVisible",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    if (data)
                        return '<i class="fa fa-eye text-success"></i>';
                    else
                        return '<i class="fa fa-eye text-danger"></i>';
                },
                "className": "min-desktop"
            },
            {
                "data": "id",
                "searchable": false,
                "width": "27%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<a href="/Admin/Catalog/Edit/${data}" class="btn btn-square btn-main-primary mr-2" data-toggle="tooltip" data-placement="bottom" title="Изменить"><svg class="krista-icon krista-edit"><use xlink:href="#krista-edit"></use></svg></a>
                        <a href="/Admin/Catalog/Delete/${data}" class="btn btn-square btn-main-danger mr-2" data-toggle="tooltip" data-placement="bottom" title="Удалить"><svg class="krista-icon krista-delete"><use xlink:href="#krista-delete"></use></svg></a>
                        <span class="btn btn-main-info mr-2" data-toggle="tooltip" data-placement="bottom" title="Количество моделей">${full.nomCount}</span>
                        <a href="/Admin/ModelsCatalog/IndexByCatalog/${data}" class="btn btn-main-success" data-toggle="tooltip" data-placement="bottom" title="Открыть список моделей">Открыть</a>`;
                },
                "className": "min-desktop"
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>'
    });

    table.on('row-reorder.dt', function (e, diff, edit) {
        for (var i = 0; i < diff.length; i++) {
            var rowData = table.row(diff[i].node).data();
            $.ajax({
                type: "POST",
                url: '/Admin/Catalog/UpdateRow',
                data: {
                    id: rowData.id,
                    fromPosition: diff[i].oldData,
                    toPosition: diff[i].newData
                },
                dataType: "json"
            });
        }
    });
});