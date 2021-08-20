var table;

$(document).ready(function () {
    table = $('.table').DataTable({
        "ajax": {
            "url": "/Admin/Feedback/LoadData",
            "type": "GET",
            "datatype": "json",
            "dataSrc": ""
        },
        "columns": [
            {
                "className": 'details-control btn-open',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            {
                "width": "1%",
                "render": function (data, type, full, meta) {
                    let count = meta.row;
                    count = count + 1;
                    totalCount = count;
                    return count;
                }
            },
            { "data": "person" },
            { "data": "phone" },
            { "data": "email" },
            {
                "data": "formattedDate",
                "width": "10%"
            },
            {
                "data": "feedbackType",
                "width": "10%"
            },
            {
                "data": "viewed",
                "width": "5%",
                "render": function (data, type, full, meta) {
                    if (data)
                        return '<i class="fa fa-eye text-success"></i>';
                    else
                        return '<i class="fa fa-eye text-muted"></i>';
                }
            },
            {
                "data": "id",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    const deleteBtn = `<a href="/Admin/Feedback/Delete?id=${data}" class="btn btn-square btn-main-danger mr-2" data-toggle="tooltip" data-placement="bottom" title="Удалить"><svg class="krista-icon krista-delete"><use xlink:href="#krista-delete"></use></svg></a>`;

                    if (full.viewed)
                        return `${deleteBtn}`;
                    else
                        return `${deleteBtn}<button class="btn btn-square btn-main-success mr-2" onclick="Edit('${data}')" data-toggle="tooltip" data-placement="bottom" title="Отметить как прочитанное"><svg class="krista-icon krista-eye"><use xlink:href="#krista-eye"></use></svg></button>`;
                }
            }
        ],
        "order": [[1, 'asc']],
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>'
    });

    function formatInnerRow(data, html) {
        if (html !== "")
            return $(html).find(".row-content");

        var result = $(".row-content-container").clone();
        result.find(".row-content-message").html(data.message);
        result.removeClass("hidden");

        if (data.filesCount > 0) {
            $.ajax({
                method: "POST",
                url: "/Admin/Feedback/GetFiles/" + data.id,
                cache: false,
                contentType: false,
                processData: false,
                async: false,
                success: function (data) {
                    var a = result.find(".row-content-files");
                    a.html(data);
                }
            });
        }

        return result[0].innerHTML;
    }


    $('.table tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
            $(this).removeClass("active");
        }
        else {
            // Open this row

            var html = "";
            if (tr.hasClass("loaded")) {
                html = row.child().html();
            }

            row.child(formatInnerRow(row.data(), html)).show();
            tr.addClass('shown');
            tr.addClass('loaded');
        }
    });
});


$('#create-modal').on('hidden.bs.modal', function (e) {
    $(this).find('form')[0].reset();
});

function Edit(id) {
    var url = "/Admin/Feedback/Edit";
    $.ajax({
        type: "POST",
        url: url,
        data: {
            id: id
        },
        success: function (data) {
            showAlert(data);
            table.ajax.reload(null, false);
        }
    });
}
