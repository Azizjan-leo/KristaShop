var table;
var dateFormat = "DD.MM.YYYY HH:mm";

$(document).ready(function () {
    table = $('.table').DataTable({
        stateSave: true,
        "ajax": {
            "url": "/Admin/Orders/LoadData" + (typeof (filtrModelId) != 'undefined' && typeof (filtrColorId) != 'undefined' && filtrColorId > 0 && filtrModelId > 0 ? "?modelId=" + encodeURIComponent(filtrModelId) + "&colorId=" + encodeURIComponent(filtrColorId) + (typeof (filtrCatalogsMode) != 'undefined' && filtrCatalogsMode != "" ? "&mode=" + filtrCatalogsMode : "") : ""),
            "type": "GET",
            "datatype": "json",
            "dataSrc": "", 
            "error": function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            }
        },
        "order": [[16, "desc"], [1, "desc"]],
        "responsive": true,
        "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",
        "pageLength": 50,
        "columns": [
            {
                "name": "idCheck",
                "data": "id",
                "width": "1%",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<input type="checkbox" name="orderid[]" value="${full.id}" />`;
                },
                "className": "min-desktop d-none d-xl-table-cell"
            },
            {
                "name": "id",
                "data": "id",
                "width": "5%"
            },
            {
                "name": "createDate",
                "data": "createDate",
                "width": "10%",
                "render": function (data, type, full, meta) {
                    var date = moment(data).format(dateFormat);
                    return date;
                },
                "className": "min-desktop"
            },
            {
                "name": "userFullName",
                "data": "userFullName",
                "render": function (data, type, full, meta) {
                    return `<div>${data}</div><div class="text-main-secondary"><a style="cursor: pointer;" onClick="addCityToFilter(this, '${full.cityName}')">${full.cityName}</a></div>`;
                }
            },
            {
                "name": "cityName",
                "data": "cityName",
                "visible": false,
                "sortable": false
            },
            {
                "name": "managerFullName",
                "data": "managerFullName",
                "width": "10%",
                "defaultContent": "---",
                "className": "min-desktop"
            },
            {
                "name": "totalSum",
                "data": "totalSum",
                "defaultContent": 0.0,
                "width": "12%",
                "render": function (data, type, full, meta) {
                    return currencyFormatConvert(data);
                },
                "className": "min-desktop"
            },
            {
                "name": "preorderTotalSum",
                "data": "preorderTotalSum",
                "defaultContent": 0.0,
                "visible": false,
                "sortable": false,
                "className": "min-desktop"
            },
            {
                "name": "retailTotalSum",
                "data": "retailTotalSum",
                "defaultContent": 0.0,
                "visible": false,
                "sortable": false,
                "className": "min-desktop"
            },
            {
                "name": "totalAmount",
                "data": "id",
                "width": "10%",
                "render": function (data, type, full, meta) {
                    return full.preorderAmount + full.retailAmount;
                },
                "className": "min-desktop"
            },
            {
                "name": "preorderAmount",
                "data": "preorderAmount",
                "width": "10%",
                "defaultContent": 0,
                "className": "min-desktop"
            },
            {
                "name": "isProcessedPreorder",
                "data": "isProcessedPreorder",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    if (data || full.preorderAmount == 0) {
                        if (full.preorderAmount == 0) {
                            return '<i class="fa fa-check-circle text-gray"></i>';
                        } else {
                            return '<i class="fa fa-check-circle text-success"></i>';
                        }
                    } else {
                        return "";
                    }
                },
                "className": "min-desktop"
            },
            {
                "name": "retailAmount",
                "data": "retailAmount",
                "width": "10%",
                "defaultContent": 0,
                "className": "min-desktop"
            },
            {
                "name": "isProcessedRetail",
                "data": "isProcessedRetail",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    if (data || full.retailAmount == 0) {
                        if (full.retailAmount == 0) {
                            return '<i class="fa fa-check-circle text-gray"></i>';
                        } else {
                            return '<i class="fa fa-check-circle text-success"></i>';
                        }
                    } else {
                        return "";
                    }
                },
                "className": "min-desktop"
            },
            {
                "name": "preorderFlag",
                "data": "id",
                "sortable": false,
                "visible": false,
                "render": function (data, type, full, meta) {
                    if (full.preorderAmount > 0) {
                        return "yes";
                    } else {
                        return "no";
                    }
                },
                "className": "min-desktop"
            },
            {
                "name": "inStockFlag",
                "data": "id",
                "sortable": false,
                "visible": false,
                "render": function (data, type, full, meta) {
                    if (full.retailAmount > 0) {
                        return "yes";
                    } else {
                        return "no";
                    }
                },
                "className": "min-desktop"
            },
            {
                "name": "processedFlag",
                "data": "isUnprocessed",
                "sortable": false,
                "visible": false,
                "render": function (data, type, full, meta) {
                    if (type === "sort") {
                        return +data;
                    }

                    if (data) {
                        return "yes";
                    } else {
                        return "no";
                    }
                },
                "className": "min-desktop"
            },
            {
                "name": "userActions",
                "data": "id",
                "width": "10%",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    var detailBtn = "";
                    var userLinkBtn = "";

                    detailBtn = '<a href="/Admin/Orders/Details/' + data + (typeof (filtrModelId) != 'undefined' && typeof (filtrColorId) != 'undefined' && filtrColorId > 0 && filtrModelId > 0 ? "?modelId=" + encodeURIComponent(filtrModelId) + "&colorId=" + encodeURIComponent(filtrColorId) + (typeof (filtrCatalogsMode) != 'undefined' && filtrCatalogsMode != "" ? "&mode=" + filtrCatalogsMode : "") : "") + '"  class="btn btn-square btn-main-success mr-2" data-toggle="tooltip" data-placement="bottom" title="Подробно"><svg class="krista-icon krista-eye"><use xlink:href="#krista-eye"></use></svg></a>';
                    userLinkBtn = `<button class="btn btn-square btn-main-info" data-toggle="tooltip" data-placement="bottom" title="Ссылка" onclick="LinkGenerate('${full.userId}')"><svg class="krista-icon krista-link"><use xlink:href="#krista-link"></use></svg></button>`;

                    return detailBtn + userLinkBtn;
                },
                "className": "min-desktop"
            }
        ],
        "createdRow": function (row, data, dataIndex) {
            if (data.isUnprocessed || !data.isReviewed) {
                $(row).addClass("text-danger");
            }
        },
        "initComplete": function () {
            SetupMainSearchLocalStorage();
            postFilterAply();
        },
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>'
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
        ClearAllCheckboxes();

        table.draw();

        postFilterAply();
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

    $("#PreorderOnly").change(function () {
        filterByPreorder(this.checked);
    });
    $("#InStockOnly").change(function () {
        filterByInstock(this.checked);
    });
    $("#ProcessedOnly").change(function () {
        filterByProcessed(this.checked);
    });

    $(".jsSwitchAll").click(function () {
        $("input[name='orderid[]']").prop("checked", $(this).prop("checked"));
    });

    $(".jsCombineOrders").click(function () {
        CombineCheckedOrders();
    });
});

function ClearAllCheckboxes() {
    $("input[name='orderid[]']").prop("checked", false);
    $("input[name='allorders']").prop("checked", false);
}

function LinkGenerate(userId) {
    var url = "/Admin/Identity/CreateLink";
    $.ajax({
        type: "POST",
        url: url,
        data: { "userId": userId },
        success: function (data) {
            var linkModal = $("#LinkModal");

            var signInLinks = linkModal.find(".modal-body").find(".sign-in-links");
            signInLinks.html("");
            var linkContainer = linkModal.find(".doc-link").clone();

            var mainLink = linkContainer.clone();
            mainLink.find("label").html("Ссылка");
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
        }
    });
};

function CombineCheckedOrders() {
    if ($("input[name='orderid[]']:checked").length == 0) {
        showNotificationError("Необходимо выделить группу заказов для объединения.");
    } else {
        Swal.fire({
            title: "Вы уверены?",
            text: "Выбранная группа заказов будет объединена!",
            icon: "warning",
            showCancelButton: true,
            cancelButtonText: "Отмена",
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Да, объединить заказы!",
        }).then((result) => {
            if (result.value) {
                var ordersIDs = new Array();
                $("input[name='orderid[]']:checked").each(function () {
                    ordersIDs.push($(this).val());
                });
                var url = "/Admin/Orders/Combine";
                $.ajax({
                    type: "POST",
                    url: url,
                    data: {
                        "ordersIDs": ordersIDs
                    }
                }).done(function (responseText) {
                    AjaxRedirectHandler(responseText);
                }).fail(function (jqXHR) {
                    AjaxErrorHandler(jqXHR);
                });
            }
        });
    }
}

$("#LinkModal").on("click", ".copy-link-btn", function () {
    var link = $(this).closest(".form-group").find("input[type='text']")[0];
    link.select();
    link.setSelectionRange(0, 99999);
    document.execCommand("copy");
});

function filterByPreorder(applyFilter) {
    ClearAllCheckboxes();

    if (applyFilter) {
        table.columns("preorderFlag:name").search("yes");
        table.draw();
    } else {
        table.columns("preorderFlag:name").search("");
        table.draw();
    }

    postFilterAply();
}

function filterByInstock(applyFilter) {
    ClearAllCheckboxes();

    if (applyFilter) {
        table.columns("inStockFlag:name").search("yes");
        table.draw();
    } else {
        table.columns("inStockFlag:name").search("");
        table.draw();
    }

    postFilterAply();
}

function filterByProcessed(applyFilter) {
    ClearAllCheckboxes();

    if (applyFilter) {
        table.columns("processedFlag:name").search("yes");
        table.draw();
    } else {
        table.columns("processedFlag:name").search("");
        table.draw();
    }

    postFilterAply();
}

var totalCount = 0;
var totalSum = 0;
var inStockCount = 0;
var inStockSum = 0;

function _clearTotalData() {
    totalCount = 0;
    totalSum = 0;
    inStockCount = 0;
    inStockSum = 0;
}

function _processDataRow(data) {
    totalCount += data.preorderAmount;
    totalSum += data.preorderTotalSum;
    inStockCount += data.retailAmount;
    inStockSum += data.retailTotalSum;
}

function postFilterAply(extData) {
    _clearTotalData();
    if (typeof (extData) == 'undefined') {
        table.rows({ filter: 'applied' }).every(function (index, element) {
            var data = this.data();

            _processDataRow(data);
        });
    } else {
        for (var i = 0; i < extData.length; i++) {
            _processDataRow(extData[i]);
        }
    }

    $(".jsPreorderCount").text(totalCount);
    $(".jsPreorderSum").text(currencyFormatConvert(totalSum));
    $(".jsInStockCount").text(inStockCount);
    $(".jsInStockSum").text(currencyFormatConvert(inStockSum));
}

function ResetOrderAllValues() {
    ClearAllCheckboxes();
    EmptyLocalstorageInputs();
    filterByPreorder(false);
    filterByInstock(false);
    filterByProcessed(false);
    table.draw();
}

function addCityToFilter(target, cityName) {
    $("[data-target-col='cityName']").val(cityName);
    table
        .columns(`cityName:name`)
        .search(cityName);
    table.draw();
    postFilterAply();
}