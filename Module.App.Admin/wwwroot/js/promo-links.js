var table;
$(document).ready(function () {
    const shopPromoUrl = $("#shopPromoUrl").val();

    table = $('.table').DataTable({
        "ajax": {
            "url": "/Admin/PromoLinks/LoadPromoLinks",
            "type": "GET",
            "datatype": "json",
            "dataSrc": "",
            "error": function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            }
        },
        "rowReorder": {
            "dataSrc": "order"
        },
        "columns": [
            {
                "render": function (data, type, full, meta) {
                    let count = meta.row;
                    count = count + 1;
                    totalCount = count;
                    return count;
                }
            },
            {
                "data": "title",
                "searchable": false,
                "sortable": true
            },
            {
                "data": "link",
                "searchable": false,
                "sortable": true,
                "render": function (data, type, full, meta) {
                    return `<span data-link='${shopPromoUrl}${data}'>${data}</span>`;
                }
            },
            {
                "data": "orderFormName",
                "searchable": false,
                "sortable": true
            },
            {
                "data": "deactivateTimeFormatted",
                "searchable": false,
                "sortable": true
            },
            {
                "data": "id",
                "searchable": false,
                "width": "20%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `
                        <a class="btn btn-sm btn-secondary mr-2 copy-btn text-light" data-toggle="tooltip" data-placement="bottom" title="Скопировать"><i class="fas fa-copy"></i></a>
                        <a href="/Admin/PromoLinks/Edit/${data}" class="btn btn-sm btn-success mr-2" data-toggle="tooltip" data-placement="bottom" title="Изменить"><i class="fas fa-edit"></i></a>
                        <a href="/Admin/PromoLinks/Delete/${data}" class="btn btn-sm btn-danger mr-2" data-toggle="tooltip" data-placement="bottom" title="Удалить"><i class="fas fa-trash"></i></a>`;
                }
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        }
    });

    $(".table").on("click", ".copy-btn", function(e) {
        const value = $(this).closest("tr").find("[data-link]").data("link");
        copyToClipboard(value);
    });
});