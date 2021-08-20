$(document).ready(function () {
    const managersList = dataModel;
    $(".table").DataTable({
        "data": managersList,
        "responsive": true,
        "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",
        "columns": [
            {
                "data": "name"
            },
            {
                "data": "id",
                "searchable": false,
                "width": "20%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<a href="/Admin/Access/EditRole/${data}" class="btn btn-square btn-main-primary mr-2" data-toggle="tooltip" data-placement="bottom" title="Изменить"><svg class="krista-icon krista-edit"><use xlink:href="#krista-edit"></use></svg></a>
                            <a href="/Admin/Access/EditRoleAccess?roleId=${data}" class="btn btn-square btn-main-info mr-2" data-toggle="tooltip" data-placement="bottom" title="Изменить настройки доступов"><svg class="krista-icon krista-settings"><use xlink:href="#krista-settings"></use></svg></a>`;
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