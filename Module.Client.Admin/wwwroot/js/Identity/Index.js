var table;
var dateFormat = "D.MM.YYYY HH:mm";
var mainSearchbox;

$(document).ready(function () {
    table = $('.table').DataTable({
        stateSave: true,
        "data": dataModel,
        "order": [[0, "asc"], [1, "asc"]],
        "responsive": true,
        "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",

        "columns": [
            {
                "name": "isActive",
                "data": "isActive",
                "width": "1px",
                "render": function (data, type, full, meta) {
                    if (type === "sort") {
                        return +data;
                    }

                    const bg = data ? "bg-main-success" : "bg-main-danger";
                    return `<div class="w-100 h-100 position-absolute" style="top: 0; left: 0;"><div class="${bg}" style="min-height: calc(100% - 10px); margin: 5px 0;"></div></div>`;
                },
                "class": "p-0 m-0 position-relative"
            },
            {
                "name": "person",
                "data": "fullName",
                "render": function (data, type, full, meta) {
                    return `<div>${data}</div><div class="text-main-secondary">${full.login}</div>`;
                },
                "className": "pr-3"
            },
            {
                "name": "login",
                "data": "login",
                "visible": false
            },
            {
                "name": "phone",
                "data": "phone",
                "width": "12%",
                "render": function (data, type, full, meta) {
                    return `<div>${data ? data : "Отсутствует"}</div><div class="text-main-secondary">${full.email ? full.email : "Отсутствует"}</div>`;
                },
                "className": "min-desktop pr-3"
            },
            {
                "name": "email",
                "data": "email",
                "visible": false,
                "defaultContent": "Отсутствует"
            },
            {
                "name": "cityName",
                "data": "cityName",
                "width": "11%",
                "render": function (data, type, full, meta) {
                    return `<div>${data ? data : "Отсутствует"}</div><div class="text-main-secondary">${full.mallAddress ? full.mallAddress : "Отсутствует"}</div>`;
                },
                "className": "min-desktop pr-3"
            },
            {
                "name": "managerName",
                "data": "managerName",
                "visible": true,
                "width": "9%",
                "defaultContent": "---",
                "className": "min-desktop pr-3"
            },
            {
                "name": "createDate",
                "data": "createDate",
                "width": "10%",
                "orderable": false,
                "render": function (data, type, full, meta) {
                    var createDate = "";
                    var lastSignIn = "";

                    if (data != null) {
                        createDate = moment(data).format(dateFormat);
                        if (moment(moment(createDate, dateFormat)).isSameOrBefore(moment("1.01.0001 05:07", dateFormat))) {
                            createDate = "";
                        }
                    }

                    if (full.lastSignIn) {
                        lastSignIn = moment(data).format(dateFormat);
                        if (moment(moment(lastSignIn, dateFormat)).isSameOrBefore(moment("1.01.0001 05:07", dateFormat))) {
                            lastSignIn = "";
                        }
                    }

                    return `<span class="text-main-secondary">${createDate}</span><br/><span class="text-main-secondary">${lastSignIn}</span>`;
                },
                "className": "min-desktop pr-3"
            },
            {
                "name": "inStockLinesCatalog",
                "data": "catalogs",
                "searchable": false,
                "sortable": true,
                "width": "5%",
                "render": function (data, type, full, meta) {
                    const isEnabled = data !== null && data["inStockLines"] !== undefined && data["inStockLines"];
                    if (type === 'display') {
                        return `<div class="d-inline-block d-lg-block px-3 text-left" data-catalog="InStockLines">
                                <span class="cursor-pointer" onclick="changeCatalogVisabilityForUser('${full.userId}', 'inStockLines', '${meta.row}', '${meta.col}')">${isEnabled ? '<i class="fas fa-check-circle text-success"></i>' : '<i class="fas fa-minus-circle text-danger"></i>'}</span>
                            </div>`;
                    } else {
                        return isEnabled;
                    }
                },
                "className": "min-desktop pr-3"
            },
            {
                "name": "inStockPartsCatalog",
                "data": "catalogs",
                "searchable": false,
                "sortable": true,
                "width": "5%",
                "render": function (data, type, full, meta) {
                    const isEnabled = data !== null && data["inStockParts"] !== undefined && data["inStockParts"];
                    if (type === 'display') {
                        return `<div class="d-inline-block d-lg-block px-3 text-left" data-catalog="InStockParts">
                                <span class="cursor-pointer" onclick="changeCatalogVisabilityForUser('${full.userId}', 'inStockParts', '${meta.row}', '${meta.col}')">${isEnabled ? '<i class="fas fa-check-circle text-success"></i>' : '<i class="fas fa-minus-circle text-danger"></i>'}</span>
                            </div>`;
                    } else {
                        return isEnabled;
                    }
                },
                "className": "min-desktop pr-3"
            },
            {
                "name": "preorderCatalog",
                "data": "catalogs",
                "searchable": false,
                "sortable": true,
                "width": "5%",
                "render": function (data, type, full, meta) {
                    const isEnabled = data !== null && data["preorder"] !== undefined && data["preorder"];
                    if (type === 'display') {
                        return `<div class="d-inline-block d-lg-block px-3 text-left" data-catalog="Preorder">
                                <span class="cursor-pointer" onclick="changeCatalogVisabilityForUser('${full.userId}', 'preorder', '${meta.row}', '${meta.col}')">${isEnabled ? '<i class="fas fa-check-circle text-success"></i>' : '<i class="fas fa-minus-circle text-danger"></i>'}</span>
                            </div>`;
                    } else {
                        return isEnabled;
                    }
                },
                "className": "min-desktop pr-3"
            },
            {
                "name": "saleLines",
                "data": "catalogs",
                "searchable": false,
                "sortable": true,
                "width": "5%",
                "render": function (data, type, full, meta) {
                    const isEnabled = data !== null && data["saleLines"] !== undefined && data["saleLines"];
                    if (type === 'display') {
                        return `<div class="d-inline-block d-lg-block px-3 text-left" data-catalog="RfInStockParts">
                                <span class="cursor-pointer" onclick="changeCatalogVisabilityForUser('${full.userId}', 'saleLines', '${meta.row}', '${meta.col}')">${isEnabled ? '<i class="fas fa-check-circle text-success"></i>' : '<i class="fas fa-minus-circle text-danger"></i>'}</span>
                            </div>`;
                    } else {
                        return isEnabled;
                    }
                },
                "className": `min-desktop pr-3`
            },
            {
                "name": "saleParts",
                "data": "catalogs",
                "searchable": false,
                "sortable": true,
                "width": "5%",
                "render": function (data, type, full, meta) {
                    const isEnabled = data !== null && data["saleParts"] !== undefined && data["saleParts"];
                    if (type === 'display') {
                        return `<div class="d-inline-block d-lg-block px-3 text-left" data-catalog="RfInStockParts">
                                <span class="cursor-pointer" onclick="changeCatalogVisabilityForUser('${full.userId}', 'saleParts', '${meta.row}', '${meta.col}')">${isEnabled ? '<i class="fas fa-check-circle text-success"></i>' : '<i class="fas fa-minus-circle text-danger"></i>'}</span>
                            </div>`;
                    } else {
                        return isEnabled;
                    }
                },
                "className": `min-desktop pr-3`
            },
            {
                "name": "rfInStockLinesCatalog",
                "data": "catalogs",
                "searchable": false,
                "sortable": true,
                "width": "5%",
                "render": function (data, type, full, meta) {
                    const isEnabled = data !== null && data["rfInStockLines"] !== undefined && data["rfInStockLines"];
                    if (type === 'display') {
                        return `<div class="d-inline-block d-lg-block px-3 text-left" data-catalog="RfInStockLines">
                                <span class="cursor-pointer" onclick="changeCatalogVisabilityForUser('${full.userId}', 'rfInStockLines', '${meta.row}', '${meta.col}')">${isEnabled ? '<i class="fas fa-check-circle text-success"></i>' : '<i class="fas fa-minus-circle text-danger"></i>'}</span>
                            </div>`;
                    } else {
                        return isEnabled;
                    }
                },
                "className": `min-desktop pr-3 ${featureEnabled ? "" : "never"}`
            },
            {
                "name": "rfInStockPartsCatalog",
                "data": "catalogs",
                "searchable": false,
                "sortable": true,
                "width": "5%",
                "render": function (data, type, full, meta) {
                    const isEnabled = data !== null && data["rfInStockParts"] !== undefined && data["rfInStockParts"];
                    if (type === 'display') {
                        return `<div class="d-inline-block d-lg-block px-3 text-left" data-catalog="RfInStockParts">
                                <span class="cursor-pointer" onclick="changeCatalogVisabilityForUser('${full.userId}', 'rfInStockParts', '${meta.row}', '${meta.col}')">${isEnabled ? '<i class="fas fa-check-circle text-success"></i>' : '<i class="fas fa-minus-circle text-danger"></i>'}</span>
                            </div>`;
                    } else {
                        return isEnabled;
                    }
                },
                "className": `min-desktop pr-3 ${featureEnabled ? "" : "never"}`
            },
            {
                "name": "cartStatusIcon",
                "data": "cartStatus",
                "width": "1%",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    if (data) {
                        return `<a href="/Admin/Identity/OpenUserCart?userId=${full.userId}"><i class="fas fa-shopping-cart text-success"></i></a>`;
                    } else {
                        return '<i class="fas fa-shopping-cart"></i>';
                    }
                },
                "className": "min-desktop pr-3"
            },
            {
                "name": "cartStatus",
                "data": "cartStatus",
                "visible": false
            },
            {
                "name": "userId",
                "data": "userId",
                "visible": false,
                "render": function (data, type, full, meta) {
                    if (data <= 0) {
                        return -1;
                    } else {
                        return data;
                    }
                }
            },
            {
                "name": "userActions",
                "data": "userId",
                "width": "40px",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    var update = "";
                    var linkGenerate = "";
                    var makePartner = "";
                    if (data <= 0) {
                        if (full.cartStatus) {
                            update = `<a class="dropdown-item" href="/Admin/Identity/ShowCart/${full.userId}" target="_blank">Показать корзину</a>`;
                        }
                        update += `<a class="dropdown-item" href="/Admin/Identity/CreateUser/${full.newUserId}?from=users">Активировать клиента</a>`;
                        update += `<button class="dropdown-item" onclick="DeleteNewUserRequest('${full.newUserId}');">Удалить заявку</button>`;
                    } else {
                        linkGenerate = `<button type="button" class="dropdown-item" onclick="LinkGenerate('${full.userId}')">Ссылка для входа</button>`;
                        update = `<button type="button" class="dropdown-item" onclick="RemoveLink('${full.userId}')">Отвязать ссылки</button>`;

                        if (Boolean.parse(hasAccessToMakePartner)) {
                            makePartner = `<button type="button" class="dropdown-item" onclick="makePartner('${full.userId}', '${full.fullName}')">Сделать партнером</button>`;
                        }
                    }

                    return `<div class="input-group show">
                        <button type="button" class="btn btn-circle btn-main-white" data-toggle="dropdown" aria-expanded="true" title="Другие действия с пользователем"><svg class="krista-icon krista-icon-size20 krista-more"><use xlink:href="#krista-more"></use></svg></button>
                        <div class="dropdown-menu" x-placement="bottom-start" style="position: absolute; will-change: transform; top: 0px; left: 0px; transform: translate3d(0px, 38px, 0px);">
                        ${linkGenerate} ${update} ${makePartner}</div></div>`;
                },
                "className": "min-desktop pr-1"
            }
        ],
        "createdRow": function(row, data, dataIndex) {
            if (data["userId"] <= 0) {
                $(row).addClass("text-danger");
            }
        },
        "initComplete": function () {
            $(document).find("div.dataTables_filter").appendTo("#basic-searchbox");
            mainSearchbox = $("#basic-searchbox").find("input").get(0);
            mainSearchbox.setAttribute('id', 'mainSearchBox');
            $(mainSearchbox).addClass('localStorageFilter')
            $('#mainSearchBox').val(window.localStorage.getItem(window.location + "_" + 'mainSearchBox')).keyup();
            $(mainSearchbox).on('search', setToLocalStorage)
            $(mainSearchbox).on('keyup', setToLocalStorage)            
        },
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rtf<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>',
        "pageLength": 50
    });

    // Table filter applied to any input attribute with data-target-col="table_column_name"
    $("input[data-target-col]").on("keyup", onFilterInputChanged);
    $("select[data-target-col]").on("change", onFilterInputChanged);
    $("[data-datetime-picker]").on("change.datetimepicker", function(e) { applyFilter(); });

    function onFilterInputChanged(event) {
        setColumnFilter(this.dataset.targetCol, this.value);
        applyFilter();
    }



    function setColumnFilter(colName, value) {
        table
            .columns(`${colName}:name`)
            .search(value);
    }

    function applyFilter() {
        table.draw();
    }

    var targetRanges = getFilterRanges();
    function getFilterRanges() {
        var result = {};
        $("[data-target-range-col]").each(function(index) {
            var key = this.dataset.targetRangeCol;
            if (result[key] == undefined) {
                result[key] = {};
            }
            result[key][this.dataset.targetRangeType] = { item: this, colIndex: table.column(`${key}:name`).index()}
        });

        return result;
    }

    var keyFrom = "from";
    var keyTo = "to";
    $.fn.dataTable.ext.search.push(filterNew);
    function filterNew(settings, data, dataIndex, jsonData, index) {
        for (var key in targetRanges) {
            if (targetRanges.hasOwnProperty(key)) {
                var min = targetRanges[key][keyFrom].item.value;
                var max = targetRanges[key][keyTo].item.value;
                var colIndex = targetRanges[key][keyTo].colIndex;
                if (!filterDateColumnByRange(min, max, colIndex, data)) {
                    return false;
                }
            }
        }
        return true;
    }

    function filterDateColumnByRange(min, max, colIndex, data) {
        var colValue = moment(data[colIndex] || "1.01.0001 05:07", dateFormat);

        if ((min == "" && max == "")) {
            return true;
        } 
        
        if (min == "" && max != "") {
            return moment(colValue).isSameOrBefore(moment(max, dateFormat));
        }

        if (min != "" && max == "") {
            return moment(colValue).isSameOrAfter(moment(min, dateFormat));
        }

        if (moment(colValue).isSameOrBefore(moment(max, dateFormat)) &&
            moment(colValue).isSameOrAfter(moment(min, dateFormat))) {
            return true;
        }

        return false;
    }

    $("#NewUsersOnly").change(function () {
        filterByNewUsers(this.checked);
    });
    //#endregion filter
});

function changeCatalogVisabilityForUser(userId, catalogName, rowIndex, colIndex) {
    var userCatalogs = table.cell(rowIndex, colIndex).data();
    $.ajax({
        type: "POST",
        url: "/Admin/Catalog/ChangeVisibility",
        data: {
            userId: userId,
            catalog: Constants.catalogs[catalogName],
            visibility: !userCatalogs[catalogName]
        },
        success: function () {
            userCatalogs[catalogName] = !userCatalogs[catalogName];
            table.cell(rowIndex, colIndex).data(userCatalogs).draw()     
        },
        error: function (jqXHR, exception) {
            if (jqXHR.status == 401)
                window.location.reload();
            showNotificationError("Ошибка при попытке изменения видимости каталога");
        }
    });
}

function filterByNewUsers(applyFilter) {
    if (applyFilter) {
        table.columns("userId:name").search(-1);
        table.draw();
    } else {
        table.columns("userId:name").search("");
        table.draw();
    }
}

function convertFormToQueryString(form) {
    var formData = new FormData(form);
    var data = [...formData.entries()];
    var queryString = data
        .map(x => `${encodeURIComponent(x[0])}=${encodeURIComponent(x[1])}`)
        .join('&');
    return queryString;
}

function EditUser(userId) {
    var filterForm = $("#users-filter-form")[0];
    var query = convertFormToQueryString(filterForm);
    var encoded = encodeURIComponent(query);
    var newLocation = `/admin/identity/updateuser?userid=${userId}&filter=${encoded}`;
    location.href = newLocation;
}

function DeleteNewUserRequest(id) {
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

function CatalogsVisibility(userId) {
    var targetRow = $(event.target).closest("tr");
    var url = "/Admin/Catalog/CatalogsVisibility";
    $.ajax({
        type: "POST",
        url: url,
        data: { "userId": userId },
        success: function (data) {
            var catalogVisibilityModal = $("#CatalogVisibilityModal");

            var userIdInput = catalogVisibilityModal.find("input[name='userId']");
            userIdInput.val(data.userId);

            var userName = catalogVisibilityModal.find(".modal-body").find(".jsUserName");
            userName.text(data.userName);

            var catalogsWrapper = catalogVisibilityModal.find(".modal-body").find(".catalogs-wrapper");
            catalogsWrapper.html("");
            var catalogContainer = catalogVisibilityModal.find(".catalog-line").clone();

            for (i = 0; i < data.catalogsList.length; i++) {
                var catalogItem = catalogContainer.clone();
                catalogItem.find("label").text(data.catalogsList[i].name);
                catalogItem.find("label").attr("for", `Catalog_${i}`);
                catalogItem.find("input[type='checkbox']").val(data.catalogsList[i].id);
                catalogItem.find("input[type='checkbox']").prop("checked", data.catalogsList[i].visible);
                catalogItem.find("input[type='checkbox']").attr("id", `Catalog_${i}`);
                catalogItem.find("input[type='checkbox']").on("change", function () {
                    var userInput = $(this).parents(".modal-body").find("input[name='userId']");
                    var dataRec = {
                        userId: userInput.val(),
                        catalogId: $(this).val(),
                        visible: $(this).prop("checked")
                    };

                    $.ajax({
                        type: "POST",
                        url: "/Admin/Catalog/ChangeVisibility",
                        data: dataRec,
                        success: function () {
                            const catalogCheckElement = targetRow.find(`[data-catalog=${data.catalogsList.find(x => x.id === +dataRec.catalogId).key}]`);
                            if(dataRec.visible) {
                                catalogCheckElement.html('<i class="fas fa-check-circle text-success"></i>');
                            } else {
                                catalogCheckElement.html('<i class="fas fa-minus-circle text-danger"></i>');
                            }
                        },
                        error: function (jqXHR, exception) {
                            if (jqXHR.status == 401)
                                window.location.reload();
                            showNotificationError("Ошибка при попытке изменения видимости каталога");
                        }
                    });
                });
                catalogItem.show();
                catalogsWrapper.append(catalogItem);
            }

            catalogVisibilityModal.modal("show");
        },
        error: function (error) {
            if (error.status == 401)
                window.location.reload();
        }
    });
}

function LinkGenerate(userId) {
    var url = "/Admin/Identity/CreateLink";
    $.ajax({
        type: "POST",
        url: url,
        data: { "userId": userId },
        success: function (data) {
            var linkModal = $("#LinkModal");

            var generatePasswordButton = linkModal.find(".generate-password-link-btn");
            generatePasswordButton.closest(".form-group").find("input[type='text']").val("");
            generatePasswordButton.attr("data-user-id", userId);
            generatePasswordButton.off();
            generatePasswordButton.on("click", generatePassword);
            
            var signInLinks = linkModal.find(".modal-body").find(".sign-in-links");
            signInLinks.html("");
            var linkContainer = linkModal.find(".doc-link").clone();

            var mainLink = linkContainer.clone();
            mainLink.find("label").html("Главная");
            mainLink.find("input[type='text']").val(data.link);
            mainLink.find("a[name='gotolink']").attr("href", data.link);
            mainLink.show();
            signInLinks.append(mainLink);

            if (data.docLinks != null) {
                for (var i = 0; i < data.docLinks.length; i++) {
                    var linkData = data.docLinks[i];

                    var container = linkContainer.clone();
                    container.find("label").html(linkData.docName);
                    container.find("input[type='text']").val(linkData.link);
                    container.find("a[name='gotolink']").attr("href", linkData.link);
                    container.show();

                    signInLinks.append(container);
                }
            }
           

            linkModal.modal("show");
        },
        error: function (error) {
            if (error.status == 401)
                window.location.reload();
        }
    });
};

$("#LinkModal").on("click", ".copy-link-btn", function() {
    var link = $(this).closest(".form-group").find("input[type='text']")[0];
    link.select();
    link.setSelectionRange(0, 99999);
    document.execCommand("copy");
});

function generatePassword(event) {
    var userId = this.dataset.userId;
    var url = "/Admin/Identity/CreateChangePasswordLink";
    var button = $(this);
    $.ajax({
        type: "POST",
        url: url,
        data: { "userId": userId },
        success: function (data, response, type) {
            button.closest(".form-group").find("input[type='text']").val(data.link);
        },
        error: function (jqXHR, exception) {
            if (jqXHR.status == 401)
                window.location.reload();
            showNotificationError("Не удалось сгенерировать ссылку для смены пароля");
        }
    });
}

function RemoveLink(userId) {
    Swal.fire({
        title: "Вы уверены?",
        text: "Отвязать все ссылки для входа от пользователя!",
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "Отмена",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Да, отвязать!",
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/Admin/Identity/RemoveLink",
                data: {
                    userId: userId
                },
                success: function (alert) {
                    showAlert(alert);
                },
                error: function (jqXHR) {
                    if (jqXHR.status == 401)
                        window.location.reload();
                    showAlert(jqXHR.responseJSON);
                }
            });
        }
    });
};

function makePartner(userId, userName) {
    Swal.fire({
        title: `Статус "Партнер"`,
        text: `Вы уверены, что хотите сделать ${userName} партнером?`,
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "Отмена",
        confirmButtonText: "Сделать партнером",
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: `/Admin/Partnership/MakePartner?userId=${userId}`
            }).done(function (response) {
                ShowAjaxSuccessMessage(response);
            }).fail(function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            });
        }
    });
}

function ResetUserAllValues() {
    EmptyLocalstorageInputs();
    filterByNewUsers(false);
    table.draw();
}