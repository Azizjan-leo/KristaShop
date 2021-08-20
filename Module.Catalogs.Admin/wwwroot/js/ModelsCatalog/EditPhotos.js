var tablePhoto;
$(document).ready(function () {
    var articul = $("#Articul").val();
    tablePhoto = $('#photos-table').DataTable({
        "ajax": {
            "url": "/Admin/ModelsCatalog/LoadPhotos",
            "type": "GET",
            "datatype": "json",
            "data": {
                "id": articul
            },
            "dataSrc": "",
            "error": function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            }
        },
        "processing": true,
        "rowReorder": {
            "dataSrc": 'order'
        },
        "columns": [
            { "data": "order", "className": 'reorder' },
            {
                "data": "photoPath",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    const imagePath = data ? data + "?width=80" : "/common/img/nophoto.png?width=80";
                    return `
                            <picture>
                                <source srcset="${imagePath}&format=webp" type="image/webp">
                                <img src="${imagePath}" width="80" alt="${full.articul}${full.colorName ? full.colorName : ""}" />
                            </picture>`;
                }
            },
            { "data": "colorName" },
            {
                "data": "id",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<button type="button" class="btn btn-square btn-main-success mr-2" data-toggle="tooltip" data-placement="bottom" title="Установить на главную" onclick="PhotoSetMain('${full.articul}', '${full.photoPath}')"><svg class="krista-icon krista-check"><use xlink:href="#krista-check"></use></svg></button>
                        <button type="button" class="btn btn-square btn-main-primary mr-2" data-toggle="tooltip" data-placement="bottom" title="Изменить" onclick="PhotoEdit('${data}')"><svg class="krista-icon krista-edit"><use xlink:href="#krista-edit"></use></svg></button>
                        <button type="button" class="btn btn-square btn-main-info mr-2" onclick="ReorderModelModal('${full.articul}', '${data}')" data-toggle="tooltip" data-placement="bottom" title="Упорядочить"><i class="fas fa-sort"></i></button>
                        <button type="button" class="btn btn-square btn-main-warning mr-2" data-toggle="tooltip" data-placement="bottom" title="Обрезать" onclick="PhotoResize('${data}','${full.photoPath}')"><svg class="krista-icon krista-cut"><use xlink:href="#krista-cut"></use></svg></button>
                        <button type="button" class="btn btn-square btn-main-danger" data-toggle="tooltip" data-placement="bottom" title="Удалить" onclick="PhotoDelete('${data}')"><svg class="krista-icon krista-delete"><use xlink:href="#krista-delete"></use></svg></button>`;
                }
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        }
    });

    tablePhoto.on('row-reorder.dt', function (e, diff, edit) {
        for (var i = 0; i < diff.length; i++) {
            var rowData = tablePhoto.row(diff[i].node).data();

            console.log(rowData);

            $.ajax({
                type: "POST",
                url: '/Admin/ModelsCatalog/UpdatePhotoRow',
                data: {
                    Id: rowData.id,
                    fromPosition: diff[i].oldData,
                    toPosition: diff[i].newData
                },
                dataType: "json"
            }).fail(function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            });
        }
    });
});

function PhotoResize(id, path) {
    $('#photo-resize-id').val(id);
    $("#resizingPhoto").attr("src", path);
    document.getElementById("result").innerHTML = "";
    $("#resize-photo-modal").modal("show");
}

var image = document.getElementById('resizingPhoto');
var cropBoxData;
var canvasData;
var cropper;
var result = document.getElementById('result');

$('#resize-photo-modal').on('shown.bs.modal', function () {
    cropper = new Cropper(image, {
        movable: false,
        zoomable: false,
        rotatable: false,
        scalable: false,
        viewMode: 2,
        aspectRatio: 2 / 3,
        autoCropArea: 1,
        ready: function () {
            //Should set crop box data first here
            cropper.setCropBoxData(cropBoxData).setCanvasData(canvasData);
        }
    });
}).on('hidden.bs.modal', function () {
    $('#btnSaveCrop').attr("disabled", true);
    document.getElementById("result").innerHTML = "";
    cropBoxData = cropper.getCropBoxData();
    canvasData = cropper.getCanvasData();
    cropper.destroy();
});

$('#btnCrop').click(function () {
    result.innerHTML = '';
    result.appendChild(cropper.getCroppedCanvas({ maxWidth: 1200, maxHeight: 800, imageSmoothingEnabled: true, imageSmoothingQuality: 'medium' }));
    $('#btnSaveCrop').attr("disabled", false);
});
$(document).ready(function () {
    $('#btnSaveCrop').attr("disabled", true);
    $('#btnSaveCrop').click(function () {
        cropper.getCroppedCanvas({ maxWidth: 4096, maxHeight: 4096 }).toBlob(function (blob) {
            var id = $('#photo-resize-id').val();
            var formData = new FormData();
            formData.append('file', blob, 'cropped.png');
            formData.append('id', id);
            $.ajax('/Admin/ModelsCatalog/ResizePhotoModel', {
                method: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (alert) {
                    showAlert(alert);
                    tablePhoto.ajax.reload(null, false);
                    $("#resize-photo-modal").modal("hide");
                }
            }).fail(function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            });
        }, 'image/jpeg', 1);
    });
});

function PhotoSetMain(articul, path) {
    var url = "/Admin/ModelsCatalog/AddMainPhoto";
    $.ajax({
        type: "POST",
        url: url,
        data: {
            id: articul,
            path: path
        },
        success: function (alert) {
            showAlert(alert);
            $("#Image-img").attr("src", path);
        }
    }).fail(function (jqXHR) {
        AjaxErrorHandler(jqXHR);
    });
}

function PhotoEdit(id) {
    $('#PhotoId').val(id);
    $("#edit-photo-modal").modal("show");
}

function EditPhotoEvent() {
    var articul = $("#Articul").val();
    var photoId = $('#PhotoId').val();
    var colorId = $('#ColorId').val();
    var url = "/Admin/ModelsCatalog/EditPhoto";
    $.ajax({
        type: "POST",
        url: url,
        data: {
            articul: articul,
            photoId: photoId,
            colorId: colorId
        },
        success: function (alert) {
            showAlert(alert);
            tablePhoto.ajax.reload(null, false);
            $("#edit-photo-modal").modal("hide");
        }
    }).fail(function (jqXHR) {
        AjaxErrorHandler(jqXHR);
    });
}

function PhotoDelete(id) {
    Swal.fire({
        title: "Вы уверены?",
        text: "Выбранная запись будет удалена из системы!",
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "Отмена",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Да, удалить запись!",
    }).then((result) => {
        if (result.value) {
            var url = "/Admin/ModelsCatalog/DeletePhoto";
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    photoId: id
                },
                success: function (alert) {
                    showAlert(alert);
                    tablePhoto.ajax.reload(null, false);
                }
            }).fail(function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            });
        }
    });
}

function ReorderModelModal(Articul, PhotoId) {
    $("#OrderNum").val("");
    $("#Articul").val(Articul);
    $("#PhotoId").val(PhotoId);
    $("#reorder-model-modal").modal("show");
};

function ReorderModel() {
    var id = $("#PhotoId").val();
    var articul = $("#Articul").val();
    var orderNum = $("#OrderNum").val();
    $.ajax({
        type: 'POST',
        url: '/Admin/ModelsCatalog/ReorderPhotoModel',
        data: {
            id: id,
            articul: articul,
            toPosition: orderNum
        },
        dataType: 'json',
        success: function (alert) {
            showAlert(alert);
            tablePhoto.ajax.reload(null, false);
            $("#reorder-model-modal").modal("hide");
        }
    }).fail(function (jqXHR) {
        AjaxErrorHandler(jqXHR);
    });
};