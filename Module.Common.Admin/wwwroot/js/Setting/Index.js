var table;
$(document).ready(function () {
    table = $('.table').DataTable({
        "ajax": {
            "url": "/Admin/Settings/LoadData",
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
            { "data": "key" },
            {
                "data": "value",
                "className": "min-desktop"
            },
            {
                "data": "description",
                "className": "min-desktop"
            },
            {
                "data": "id",
                "width": "10%",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    var result = `<button class="btn btn-square btn-main-primary mr-2" onclick="Edit('${data}')" data-toggle="tooltip" data-placement="bottom" title="Изменить"><svg class="krista-icon krista-edit"><use xlink:href="#krista-edit"></use></svg></button>`;
                    if ($("#delete-modal")[0]) {
                        result += `<button class="btn btn-square btn-main-danger" onclick="Delete('${data}')" data-toggle="tooltip" data-placement="bottom" title="Удалить"><svg class="krista-icon krista-delete"><use xlink:href="#krista-delete"></use></svg></button>`;
                    }
                    return result;
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


$('#create-modal').on('hidden.bs.modal', function (e) {
    $(this).find('form')[0].reset();
});
createSuccess = function (alert) {
    showAlert(alert);
    table.ajax.reload(null, false);
    $("#create-modal").modal("hide");
};
createError = function (alert) {
    showAlert(alert.responseJSON);
    $("#create-modal").modal("hide");
};


function Edit(id) {
    var url = "/Admin/Settings/Details?id=" + id;
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            $('#EditId').val(data.id);
            $('#EditKey').val(data.key);
            $('#EditValue').val(data.value);
            $('#EditDescription').val(data.description);
            $("#edit-modal").modal("show");
        }
    });
}
editSuccess = function (alert) {
    showAlert(alert);
    table.ajax.reload(null, false);
    $("#edit-modal").modal("hide");
};
editError = function (alert) {
    showAlert(alert.responseJSON);
    $("#edit-modal").modal("hide");
};


function Delete(id) {
    $('#Id').val(id);
    $("#delete-modal").modal("show");
}
deleteSuccess = function (alert) {
    showAlert(alert);
    table.ajax.reload(null, false);
    $("#delete-modal").modal("hide");
};
