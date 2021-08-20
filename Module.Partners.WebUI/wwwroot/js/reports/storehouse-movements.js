$(function () {
    const movementGroup = dataModel;

    flatpickr("[data-datetime-picker]", {
        allowInput: true,
        enableTime: false,
        locale: "ru",
        dateFormat: "d.m.Y",
        theme: "light",
        plugins: [
            ShortcutButtonsPlugin({
                theme: "light",
                button: [
                    { label: 'Сегодня' }
                ],
                onClick(index, fp) {
                    let date = index ? new Date(Date.now() + 24 * index * 60 * 60 * 1000) : new Date;

                    fp.setDate(date);
                    fp.close();
                }
            })
        ]
    });
    
    var dateFromInput = $("[name=DateFrom]");
    var dateToInput = $("[name=DateTo]");
    const table = $(".storehouseMovementsTable").DataTable({
        "data": movementGroup,
        "responsive": true,
        "paging": false,
        "columns": [
            {
                "name": "modelKey",
                "data": "modelKey",
                "width": "85%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    if (type !== "display") {
                        return full.modelInfo.name;
                    }

                    return getMovementGridRenderer().renderGrid(full);
                }
            },
            {
                "name": "actions",
                "data": "modelKey",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    if(isDetailView) { 
                        return "";
                    }
                    
                    return `<div><a href="/Partners/Reports/ModelMovement/${full.modelInfo.modelId}?fromDate=${dateFromInput.val()}&toDate=${dateToInput.val()}" class="btn btn-basic btn-square btn-success"><i class="bx bx-file icon-font-22px align-middle"></i></a></div>`;
                },
                "className": "align-middle"
            }
        ],
        "language": {
            "url": "/common/datatables.Russian.json"
        },
        "dom": "r"
    });
});