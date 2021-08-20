var table;
var dateFormat = "DD.MM.YYYY HH:mm";

$(document).ready(function () {
    table = $('.table').DataTable({
        stateSave: true,
        "ajax": {
            "url": "/Admin/Orders/LoadDataRequests",
            "type": "GET",
            "datatype": "json",
            "dataSrc": function (data) {
                postFilterAply(data);

                return data;
            }, 
            "error": function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            }
        },
        "responsive": true,
        "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",
        "pageLength": 50,
        "columns": [
            {
                "name": "requestDate",
                "data": "requestDate",
                "width": "7.5%",
                "render": function (data, type, full, meta) {
                    return moment(data).format(Date.BasicDateFormat);
                }
            },
            {
                "name": "userFullName",
                "data": "userFullName"
            },
            {
                "name": "cityName",
                "data": "cityName",
                "className": "min-desktop"
            },
            {
                "name": "managerFullName",
                "data": "managerFullName",
                "width": "10%",
                "defaultContent": "---",
                "className": "min-desktop"
            },
            {
                "name": "totAmount",
                "data": "totAmount",
                "defaultContent": 0,
                "width": "12%",
                "className": "min-desktop"
            },
            {
                "name": "totPrice",
                "data": "totPrice",
                "defaultContent": 0.0,
                "width": "12%",
                "render": function (data, type, full, meta) {
                    return currencyFormatConvert(data);
                },
                "className": "min-desktop"
            },
            {
                "name": "totPriceInRub",
                "data": "totPriceInRub",
                "defaultContent": 0.0,
                "width": "12%",
                "render": function (data, type, full, meta) {
                    return currencyFormatConvert(data);
                },
                "className": "min-desktop"
            },
            {
                "name": "userActions",
                "data": "userId",
                "width": "10%",
                "searchable": false,
                "sortable": false,
                "render": function (data, type, full, meta) {
                    var detailBtn = "";
                    var userLinkBtn = "";

                    detailBtn = '<a href="/Admin/Orders/RequestsDetails/' + data + '"  class="btn btn-square btn-main-success mr-2" data-toggle="tooltip" data-placement="bottom" title="Подробно"><svg class="krista-icon krista-eye"><use xlink:href="#krista-eye"></use></svg></a>';
                    userLinkBtn = `<button class="btn btn-square btn-main-info" data-toggle="tooltip" data-placement="bottom" title="Ссылка" onclick="LinkGenerate('${full.userId}')"><svg class="krista-icon krista-link"><use xlink:href="#krista-link"></use></svg></button>`;

                    return detailBtn + userLinkBtn;
                },
                "className": "min-desktop"
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "initComplete": SetupMainSearchLocalStorage
    })
    .order([[0, "asc"]]);

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

});

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

$("#LinkModal").on("click", ".copy-link-btn", function () {
    var link = $(this).closest(".form-group").find("input[type='text']")[0];
    link.select();
    link.setSelectionRange(0, 99999);
    document.execCommand("copy");
});

function ResetOrderAllValues() {
    EmptyLocalstorageInputs();
    table.draw();
    postFilterAply();
}

var totalCount = 0;
var totalSum = 0;

function _clearTotalData() {
    totalCount = 0;
    totalSum = 0;
}

function _processDataRow(data) {
    totalCount += data.totAmount;
    totalSum += data.totPrice;
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

    $(".jsTotalCount").text(totalCount);
    $(".jsTotalSum").text(currencyFormatConvert(totalSum));
}
