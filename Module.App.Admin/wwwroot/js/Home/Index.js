var dateFormat = "D.MM.YYYY HH:mm";
var newRegistrationsTable;
$(document).ready(function () {
   if(typeof partnershipRequests !== 'undefined') {
       $(".partnershipRequstsTable").DataTable({
           "data": partnershipRequests,
           "responsive": true,
           "columns": [
               {
                   "data": "fullName"
               },
               {
                   "data": "managerName",
                   "className": "min-desktop"
               },
               {
                   "data": "phone",
                   "render": function (data, type, full, meta) {
                       return `<div>${data ? data : "Отсутствует"}</div><div class="text-main-secondary">${full.email ? full.email : ""}</div>`;
                   },
                   "className": "min-desktop"
               },
               {
                   "data": "cityName",
                   "render": function (data, type, full, meta) {
                       return `<div>${data ? data : "Отсутствует"}</div><div class="text-main-secondary">${full.mallAddress ? full.mallAddress : "Отсутствует"}</div>`;
                   },
                   "className": "min-desktop"
               },
               {
                   "data": "requestedDate",
                   "render": function (data, type, full, meta) {
                       return `<div class="text-main-secondary">${moment(data).format(dateFormat)} <br/> ${moment(full.lastSignIn).format(dateFormat)}</div>`;
                   },
                   "className": "min-desktop"
               },
               {
                   "data": "id",
                   "searchable": false,
                   "width": "15%",
                   "sortable": false,
                   "render": function (data, type, full, meta) {
                       const checkBtnColor = full.isAcceptedToProcess ? '#E0E0E0' : '#602FED';
                       return `<button class="btn btn-square btn-main-primary mr-2" data-recid="${data}" data-toggle="tooltip" data-placement="bottom" title="${full.isAcceptedToProcess ? 'В обработке' : 'Принять в обработку' }" onclick="AcceptPartnershipRequestToProcess(this);" style="background: ${checkBtnColor}; border-style: none;"><svg class="krista-icon krista-check"><use xlink:href="#krista-check"></use></svg></a>
                            <button class="btn btn-square btn-main-danger mr-2" data-recid="${data}" data-toggle="tooltip" data-placement="bottom" title="Удалить заявку на партнерство" onclick="DeletePartnershipRequest(this);"><svg class="krista-icon krista-close"><use xlink:href="#krista-close"></use></svg></button>`;
                   },
                   "className": "min-desktop"
               },
               {
                   "data": "id",
                   "searchable": false,
                   "width": "15%",
                   "visible": Boolean.parse(hasAccessToApprovePartnership),
                   "sortable": false,
                   "render": function (data, type, full, meta) {
                       return `
                            <button class="btn btn-square btn-main-success mr-2" data-recid="${data}" data-toggle="tooltip" data-placement="bottom" title="Удовлетворить заявку на партнерство" onclick="ApprovePartnershipRequest(this);">OK</button>`;
                   },
                   "className": "min-desktop"
               }
           ],
           "language": {
               "url": '/common/datatables.Russian.json'
           },
           "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>'
       });
   }

    if(typeof newRegistrationsData !== 'undefined') {
        newRegistrationsTable = $(".newRegistrationsTable").DataTable({
            "data": newRegistrationsData,
            "responsive": true,
            "columns": [
                {
                    "data": "fullName"
                },
                {
                    "data":"phone",
                    "render": function (data, type, full, meta) {
                        return `<div>${data ? data : "Отсутствует"}</div><div class="text-main-secondary">${full.email ? full.email : ""}</div>`;
                    },
                    "className": "min-desktop"
                },
                {
                    "data": "cityName",
                    "render": function (data, type, full, meta) {
                        return `<div>${data ? data : "Отсутствует"}</div><div class="text-main-secondary">${full.mallAddress ? full.mallAddress : "Отсутствует"}</div>`;
                    },
                    "className": "min-desktop"
                },
                {
                    "data": "managerName",
                    "className": "min-desktop"
                },
                {
                    "data": "createDate",
                    "render": function (data, type, full, meta) {
                        return `<div class="text-main-secondary">${moment(data).format(dateFormat)}</div>`;
                    },
                    "className": "min-desktop"
                },
                {
                    "data": "id",
                    "searchable": false,
                    "width": "15%",
                    "sortable": false,
                    "render": function (data, type, full, meta) {
                        return `<a href="/Admin/Identity/CreateUser/${data}" class="btn btn-square btn-main-primary mr-2" data-toggle="tooltip" data-placement="bottom" title="Создать контрагента"><svg class="krista-icon krista-check"><use xlink:href="#krista-check"></use></svg></a>
                            <button class="btn btn-square btn-main-danger mr-2" data-recid="${data}" data-toggle="tooltip" data-placement="bottom" title="Удалить заявку на регистрацию" onclick="DeleteNewUserRequest(this);"><svg class="krista-icon krista-close"><use xlink:href="#krista-close"></use></svg></button>`;
                    },
                    "className": "min-desktop"
                }
            ],
            "language": {
                "url": '/common/datatables.Russian.json'
            },
            "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>'
        });
    }

    if(typeof newPromoRegistrationsData !== 'undefined') {
        newPromoRegistrationsTable = $(".newPromoRegistrationsTable").DataTable({
            "data": newPromoRegistrationsData,
            "responsive": true,
            "columns": [
                {
                    "data": "fullName"
                },
                {
                    "data": "phone",
                    "render": function (data, type, full, meta) {
                        return `<div>${data ? data : "Отсутствует"}</div><div class="text-main-secondary">${full.email ? full.email : ""}</div>`;
                    },
                    "className": "min-desktop"
                },
                {
                    "data": "createDate",
                    "render": function (data, type, full, meta) {
                        return `<div class="text-main-secondary">${moment(data).format(dateFormat)}</div>`;
                    },
                    "className": "min-desktop"
                },
                {
                    "data": "id",
                    "searchable": false,
                    "width": "15%",
                    "sortable": false,
                    "render": function (data, type, full, meta) {
                        return `<a href="/Admin/Identity/CreateUser/${data}" class="btn btn-square btn-main-primary mr-2" data-toggle="tooltip" data-placement="bottom" title="Создать контрагента"><svg class="krista-icon krista-check"><use xlink:href="#krista-check"></use></svg></a>
                            <button class="btn btn-square btn-main-danger mr-2" data-recid="${data}" data-toggle="tooltip" data-placement="bottom" title="Удалить заявку на регистрацию" onclick="DeleteNewUserRequest(this);"><svg class="krista-icon krista-close"><use xlink:href="#krista-close"></use></svg></button>
                            <a href="/Admin/Identity/ShowCart/${data}" class="btn btn-square btn-main-success mr-2" target="_blank" data-toggle="tooltip" data-placement="bottom" title="Посмотреть корзину"><svg class="krista-icon krista-shop"><use xlink:href="#krista-shop"></use></svg></a>`;
                    },
                    "className": "min-desktop"
                }
            ],
            "language": {
                "url": '/common/datatables.Russian.json'
            },
            "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>'
        });
    }
});

function ApprovePartnershipRequest(btnObj) {
    var id = $(btnObj).attr("data-recid");

    Swal.fire({
        title: "Вы уверены?",
        text: "Заявка на партнерство будет одобрена!",
        icon: "info",
        showCancelButton: true,
        cancelButtonText: "Отмена",
        confirmButtonColor: "#27AE60",
        confirmButtonText: "Да, одобрить!",
    }).then((result) => {
        if (result.value) {
            var url = "/Admin/Partnership/ApprovePartnershipRequest";
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    "id": id
                }
            }).done(function (responseText) {
                window.location.reload();
            }).fail(function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            });
        }
    });
}
function AcceptPartnershipRequestToProcess(btnObj) {
    var id = $(btnObj).attr("data-recid");

    $.ajax({
        type: "POST",
        url: "/Admin/Partnership/AcceptPartnershipRequestToProcess",
        data: {
            "id": id
        }
    }).done(function (responseText) {
        window.location.reload();
    }).fail(function (jqXHR) {
        AjaxErrorHandler(jqXHR);
    });
} 

function DeletePartnershipRequest(btnObj) {
    var id = $(btnObj).attr("data-recid");

    Swal.fire({
        title: "Вы уверены?",
        text: "Заявка на партнерство будет удалена!",
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "Отмена",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Да, удалить!",
    }).then((result) => {
        if (result.value) {
            var url = "/Admin/Partnership/DeletePartnershipRequest";
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    "id": id
                }
            }).done(function (responseText) {
                window.location.reload();
            }).fail(function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            });
        }
    });
}
function DeleteNewUserRequest(btnObj) {
    var id = $(btnObj).attr("data-recid");

    Swal.fire({
        title: "Вы уверены?",
        text: "Заявка на регистрацию будет удалена!",
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "Отмена",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Да, удалить!",
    }).then((result) => {
        if (result.value) {
            var url = "/Admin/Identity/DeleteNewUserRequest";
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    "id": id
                }
            }).done(function (responseText) {
                window.location.reload();
            }).fail(function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            });
        }
    });
}