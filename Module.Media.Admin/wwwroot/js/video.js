var table;
$(document).ready(function () {
    var galleryId = $("#GalleryId").val();
    table = $('.table').DataTable({
        "ajax": {
            "url": "/Admin/Video/LoadData",
            "type": "GET",
            "datatype": "json",
            "data": {
                galleryId: galleryId
            },
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
                "data": "order",
                "className": "reorder min-desktop"
            },
            {
                "data": "previewPath",
                "width": "5%",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    const photo = data ? data : "/common/img/nophoto.png?width=100";
                    return `<picture>
                                <source srcset="${photo}?width=100&format=webp" type="image/webp">
                                <img srcset="${data}" width="100" alt="video"/>
                            </picture>`;
                }
            },
            { "data": "title" },
            {
                "data": "description",
                "className": "min-desktop"
            },
            {
                "data": "id",
                "searchable": false,
                "width": "20%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    var eyeBtnColor = full.isVisible ? '#27AE60' : '#E0E0E0';

                    return `<button value="${full.id}" onClick="switchVisability('${full.id}')" class="btn btn-square btn-main-primary mr-2" style="background: ${eyeBtnColor}; border-style: none;" data-toggle="tooltip" data-placement="bottom" title="Изменить видимость"><svg class="krista-icon"><use xlink:href="#krista-eye"></use></svg></button>
                        <a href="/Admin/Video/Edit/${data}" class="btn btn-square btn-main-primary mr-2" data-toggle="tooltip" data-placement="bottom" title="Изменить"><svg class="krista-icon krista-edit"><use xlink:href="#krista-edit"></use></svg></a>
                        <a href="/Admin/Video/Delete/${data}" class="btn btn-square btn-main-danger mr-2" data-toggle="tooltip" data-placement="bottom" title="Удалить"><svg class="krista-icon krista-delete"><use xlink:href="#krista-delete"></use></svg></a>`;
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
                url: '/Admin/Video/UpdateRow',
                data: {
                    videoId: rowData.id,
                    galleryId: galleryId,
                    toPosition: diff[i].newData
                },
                dataType: "json"
            }).done(function (jqXHR) {
                ShowAjaxSuccessMessage(jqXHR);
            }).fail(function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            });
        }
    });
});
function switchVisability(id) {
  
    $.ajax({
        url: `/Admin/Video/SwitchVisibility/${id}`,
        data: { id: id }
    }).done(function () {
        location.reload();
    });
}
