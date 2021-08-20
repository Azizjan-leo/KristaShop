var table;
$(document).ready(function () {
    table = $('.table').DataTable({
        "ajax": {
            "url": "/Admin/Category/LoadData",
            "type": "GET",
            "datatype": "json",
            "dataSrc": ""
        },
        "rowReorder": {
            "dataSrc": 'order'
        },
        "responsive": true,
        "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",
        "columns": [
            {
                "data": "order",
                "width": "10%",
                "className": "reorder min-desktop"
            },
            {
                "data": "imagePath",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    data = (data) ? data + '?width=100' : '/common/img/nophoto.png?width=100';
                    return `
                            <picture>
                                <source srcset="${data}&format=webp" type="image/webp">
                                <img srcset="${data}" width="100" alt="Alternate Text" />
                            </picture>`;
                },
                "className": "min-desktop"
            },
            {
                "data": "categoryId1C",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    if (data) {
                        return data + ' (' + full.category1CName + ')';
                    }
                },
                "defaultContent": "---",
                "className": "min-desktop"
            },

            { "data": "name" },
            {
                "data": "isVisible",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    if (data)
                        return '<i class="fa fa-eye text-success"></i>';
                    else
                        return '<i class="fa fa-eye text-danger"></i>';
                }
            },
            {
                "data": "id",
                "searchable": false,
                "width": "20%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<a href="/Admin/Category/Edit/${data}" class="btn btn-square btn-main-primary mr-2" data-toggle="tooltip" data-placement="bottom" title="Изменить"><svg class="krista-icon krista-edit"><use xlink:href="#krista-edit"></use></svg></a>
                        <a href="/Admin/Category/Delete/${data}" class="btn btn-square btn-main-danger mr-2" data-toggle="tooltip" data-placement="bottom" title="Удалить"><svg class="krista-icon krista-delete"><use xlink:href="#krista-delete"></use></svg></a>`;
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
                url: '/Admin/Category/UpdateRow',
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