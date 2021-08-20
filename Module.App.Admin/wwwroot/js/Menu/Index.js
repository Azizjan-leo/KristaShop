var table;
$(document).ready(function () {
    table = $('.table').DataTable({
        "ajax": {
            "url": "/Admin/Menu/LoadData",
            "type": "GET",
            "datatype": "json",
            "dataSrc": ""
        },
        "responsive": true,
        "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",
        "columns": [
            {
                "data": "id",
                "render": function (data, type, full, meta) {
                    let count = meta.row;
                    count = count + 1;
                    return count;
                },
                "className": "min-desktop"
            },
            { "data": "title" },
            {
                "data": "controllerName"
            },
            {
                "data": "actionName",
                "className": "min-desktop"
            },
            {
                "data": "icon",
                "className": "min-desktop"
            },
            {
                "data": "order",
                "className": "min-desktop"
            },
            {
                "data": "id",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<a href="/Admin/Menu/Edit/${full.id}" class="btn btn-square btn-main-primary mr-2" data-toggle="tooltip" data-placement="bottom" title="Изменить"><svg class="krista-icon krista-edit"><use xlink:href="#krista-edit"></use></svg></a>
                        <a href="/Admin/Menu/Delete/${full.id}" class="btn btn-square btn-main-danger" data-toggle="tooltip" data-placement="bottom" title="Удалить"><svg class="krista-icon krista-delete"><use xlink:href="#krista-delete"></use></svg></a>`;
                },
                "className": "min-desktop"
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>'
    });
});