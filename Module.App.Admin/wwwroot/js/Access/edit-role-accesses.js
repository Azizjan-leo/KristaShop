const accesses = dataModel;
var table;
$(document).ready(function () {
    table = $(".table").DataTable({
        "data": accesses,
        "responsive": true,
        "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",
        "pageLength": "100",
        "columns": [
            {
                "data": "itemKey"
            },
            {
                "data": "id",
                "searchable": false,
                "width": "20%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<div class="custom-control custom-switch mt-4">
                                <input type="checkbox" class="custom-control-input" id="${full.itemKey}" ${full.isAccessGranted ? 'checked' : '' }/>
                                <label class="custom-control-label" for="${full.itemKey}" onclick="updateAccessesValue(${JSON.stringify(full).split('"').join("&quot;")})"></label>
                            </div>`;
                },
                "className": "min-desktop"
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>'
    });

    $("#UpdateRoleAccessForm").on("submit", function (event) {
        event.preventDefault();
        $.ajax({
            url: "/Admin/Access/EditRoleAccess",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            data: JSON.stringify(accesses),
        }).done(function (responseText) {
            AjaxRedirectHandler(responseText);
        }).fail(function (jqXHR) {
            AjaxErrorHandler(jqXHR);
        });
    });
});

function updateAccessesValue(roleAccess) {
    var item = _.find(accesses, function (object) { return object.itemKey === roleAccess.itemKey && object.id === roleAccess.id });
    item.isAccessGranted = !item.isAccessGranted;
}

function grantAccessForAll() {
    debugger;
    for (let item of accesses) {
        item.isAccessGranted = true;
    }
    table.rows().invalidate().draw();
}

function removeAccessForAll() {
    debugger;
    for (let item of accesses) {
        item.isAccessGranted = false;
    }
    table.rows().invalidate().draw();
}