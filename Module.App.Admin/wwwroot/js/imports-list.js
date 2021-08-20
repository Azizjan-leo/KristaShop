var table;
$(document).ready(function () {
    const isRoot = Boolean.parse($("#isRoot").val());
    table = $('.table').DataTable({
        "ajax": {
            "url": "/Admin/Import/LoadAppliedImports",
            "type": "GET",
            "datatype": "json",
            "dataSrc": "",
            "error": function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            }
        },
        "responsive": true,
        "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",
        "rowReorder": {
            "dataSrc": "order"
        },
        "columns": [
            {
                "data": "keyValue",
                "width": "15%",
                "render": function (data, type, full, meta) {
                    let count = meta.row;
                    count = count + 1;
                    return count;
                },
                "className": "min-desktop"
            },
            {
                "data": "applyDateFormatted",
                "searchable": false,
                "sortable": true
            },
            {
                "data": "keyValue",
                "searchable": false,
                "sortable": true
            },
            {
                "data": "login",
                "searchable": false,
                "sortable": true,
                "className": "min-desktop"
            },
            {
                "data": "id",
                "searchable": false,
                "width": "20%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    var deleteBtn = "";
                    if (isRoot) {
                        deleteBtn = `<a href="/Admin/Import/DeleteImport?id=${data}" class="btn btn-square btn-main-danger mr-2" data-toggle="tooltip" data-placement="bottom" title="Удалить"><svg class="krista-icon krista-delete"><use xlink:href="#krista-delete"></use></svg></span></a>`;
                    }

                    return `${deleteBtn}<a href="/Admin/Import/ApplyBackup?backupName=${full.backupFileName}" class="btn btn-main-success mr-2" data-toggle="tooltip" data-placement="bottom" title="Выполнить"><svg class="krista-icon krista-check"><use xlink:href="#krista-check"></use></svg></span> Выполнить</a>`;
                }
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>'
    });
});