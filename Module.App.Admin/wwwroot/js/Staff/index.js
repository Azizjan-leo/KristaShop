$(document).ready(function () {
    const managersList = dataModel;
    $(".table").DataTable({
        "data": managersList,
        "responsive": true,
        "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",
        "columns": [
            {
                "data": "name",
                "className": 'min-desktop'
            },
            {
                "data": "name",
                "render": function (data, type, full, meta) {
                    return "Менеджеры";
                },
                "className": "min-desktop"
            },
            {
                "data": "notificationsEmail",
                "className": "min-desktop"
            },
            {
                "data": "registrationsQueueNumber",
                "render": function (data, type, full, meta) {
                    if(type !== 'display' && data <= 0) { 
                        return 10000;
                    }
                    
                    if(data > 0) { 
                        return data;
                    }
                    return '';
                },
                "className": "min-desktop"
            },
            {
                "data": "id",
                "searchable": false,
                "width": "20%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<a href="/Admin/Staff/Edit/${data}" class="btn btn-square btn-main-primary mr-2" data-toggle="tooltip" data-placement="bottom" title="Изменить"><svg class="krista-icon krista-edit"><use xlink:href="#krista-edit"></use></svg></a>`;
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