var table;
var dateFormat = "YYYY.MM.DD";
var dateFormatFilter = "DD.MM.YYYY";

$(document).ready(function () {
    table = $('.table').DataTable({
        stateSave: true,
        "ajax": {
            "url": "/Admin/Orders/LoadDataInvoices",
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
                "name": "createDate",
                "data": "createDate",
                "width": "70px",
                "render": function (data, type, full, meta) {
                    var date = "";
                    if (data != null) {
                        date = moment(data).format(dateFormat);
                        if (moment(moment(date, dateFormat)).isSameOrBefore(moment("1.01.0001 05:07", "DD.MM.YYYY HH:mm"))) {
                            date = "";
                        }
                    }
                    return date;
                }
            },
            {
                "name": "invoiceNum",
                "data": "invoiceNum",
                "width": "10%",
            },
            {
                "name": "userFullName",
                "data": "userFullName",
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
                "name": "prePayFormated",
                "data": "prePayFormated",
                "width": "10%",
                "className": "min-desktop"
            },
            {
                "name": "totalPayFormated",
                "data": "totalPayFormated",
                "width": "10%",
                "className": "min-desktop"
            },
            {
                "name": "exchangeRate",
                "data": "exchangeRate",
                "defaultContent": 1.0,
                "width": "12%",
                "render": function (data, type, full, meta) {
                    return currencyFormatConvert(data);
                },
                "className": "min-desktop"
            },
            {
                "name": "wasPayed",
                "data": "wasPayed",
                "render": function (data, type, full, meta) {
                    if (data) {
                        return `<center title="Оплачен"><span style="display: none;">1</span><i class="fa fa-check-circle text-success"></i></center>`;
                    } else {
                        return `<span style="display: none;">0</span>`;
                    }
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

                    detailBtn = `<a href="/Admin/Orders/InvoiceDetails/${full.id}?userId=${data}" class="btn btn-square btn-main-success mr-2" data-toggle="tooltip" data-placement="bottom" title="Подробно"><svg class="krista-icon krista-eye"><use xlink:href="#krista-eye"></use></svg></a>`;
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
    .order([[0, "desc"]]);

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
        var colValue = moment(data[colIndex] || "0001.01.01 05:07", dateFormat);

        if ((min == "" && max == "")) {
            return true;
        } 
        
        if (min == "" && max != "") {
            return moment(colValue).isSameOrBefore(moment(max, dateFormatFilter));
        }

        if (min != "" && max == "") {
            return moment(colValue).isSameOrAfter(moment(min, dateFormatFilter));
        }

        if (moment(colValue).isSameOrBefore(moment(max, dateFormatFilter)) &&
            moment(colValue).isSameOrAfter(moment(min, dateFormatFilter))) {
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

function postFilterAply(extData) {
    return;
}
