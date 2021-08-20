var table;
$(document).ready(function () {
    table = $('.table').DataTable({
        "ajax": {
            "url": "/Admin/Import/LoadAvailableBackups",
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
                "data": "value",
                "render": function (data, type, full, meta) {
                    let count = meta.row;
                    count = count + 1;
                    return count;
                }
            },
            {
                "data": "value",
                "searchable": false,
                "sortable": true
            },
            {
                "data": "key",
                "searchable": false,
                "width": "20%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<a href="/Admin/Import/ApplyBackup?backupName=${data}" class="btn btn-main-success mr-2" data-toggle="tooltip" data-placement="bottom" title="Выполнить"><span class="btn-icon"><svg class="krista-icon krista-check"><use xlink:href="#krista-check"></use></svg></span> Выполнить</a>`;
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
