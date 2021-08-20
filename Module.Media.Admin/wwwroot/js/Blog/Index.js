var table;
$(document).ready(function () {
    table = $('.table').DataTable({
        "ajax": {
            "url": "/Admin/Blog/LoadData",
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
                "className": 'reorder min-desktop'
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
                }
            },
            { "data": "title" },
            {
                "data": "description",
                "className": "min-desktop"
            },
            {
                "data": "link",
                "className": "min-desktop"
            },
            {
                "data": "id",
                "searchable": false,
                "width": "20%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    var eyeBtnColor = full.isVisible ? '#27AE60' : '#E0E0E0';
                    return `<a href="/Admin/Blog/SwitchVisibility/${full.id}" class="btn btn-square btn-main-primary mr-2" style="background: ${eyeBtnColor}; border-style: none;" data-toggle="tooltip" data-placement="bottom" title="Изменить видимость"><svg class="krista-icon"><use xlink:href="#krista-eye"></use></svg></a>
                        <a href="/Admin/Blog/Edit/${data}" class="btn btn-square btn-main-primary mr-2" data-toggle="tooltip" data-placement="bottom" title="Изменить"><svg class="krista-icon krista-edit"><use xlink:href="#krista-edit"></use></svg></a>
                        <a href="/Admin/Blog/Delete/${data}" class="btn btn-square btn-main-danger mr-2" data-toggle="tooltip" data-placement="bottom" title="Удалить"><svg class="krista-icon krista-delete"><use xlink:href="#krista-delete"></use></svg></a>`;
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
                url: '/Admin/Blog/UpdateRow',
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