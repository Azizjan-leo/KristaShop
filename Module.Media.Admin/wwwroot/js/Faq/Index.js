var table;
$(document).ready(function () {
    table = $('.table').DataTable({
        "ajax": {
            "url": "/Admin/Faq/LoadData",
            "type": "GET",
            "datatype": "json",
            "dataSrc": ""
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
            { "data": "title" },
            {
                "data": "id",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return '<a href="/Admin/Faq/Edit/' + full.id + '" class="btn btn-sm btn-success mr-2" data-toggle="tooltip" data-placement="bottom" title="Изменить"><i class="fas fa-edit"></i></a>' +
                        '<a href="/Admin/Faq/DeleteFaq/' + full.id + '" class="btn btn-sm btn-danger" data-toggle="tooltip" data-placement="bottom" title="Удалить"><i class="fas fa-trash"></i></a>' +
                        '<a href="/Admin/Faq/SectionList?faqId=' + full.id + '" class="btn btn-sm btn-info" style="margin-left:10px;" data-toggle="tooltip" data-placement="bottom" title="Редактировать секции"><i class="fas fa-pen"></i></a>';
                }
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        }
    });
});