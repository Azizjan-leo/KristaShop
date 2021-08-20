var table;
$(document).ready(function () {
    table = $('.table').DataTable({
        "ajax": {
            "url": "/Admin/MBody/LoadData",
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
                "name": "order",
                "data": "order",
                "className": "reorder min-desktop"
            },
            {
                "name": "title",
                "data": "title"
            },
            {
                "name": "url",
                "data": "url",
                "className": "min-desktop"
            },
            {
                "name": "layoutName",
                "data": "layoutName",
                "className": "min-desktop"
            },
            {
                "name": "id",
                "data": "id",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<a href="/Admin/MBody/Edit/${full.id}" class="btn btn-square btn-main-primary mr-2" data-toggle="tooltip" data-placement="bottom" title="Изменить"><svg class="krista-icon krista-edit"><use xlink:href="#krista-edit"></use></svg></a>
                        <a href="/Admin/MBody/Delete/${full.id}" class="btn btn-square btn-main-danger" data-toggle="tooltip" data-placement="bottom" title="Удалить"><svg class="krista-icon krista-delete"><use xlink:href="#krista-delete"></use></svg></a>`;
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
                url: '/Admin/MBody/UpdateRow',
                data: {
                    id: rowData.id,
                    toPosition: diff[i].newData
                },
                dataType: "json"
            });
        }
    });
});
