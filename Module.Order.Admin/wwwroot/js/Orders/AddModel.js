var table;
var dateFormat = "DD.MM.YYYY HH:mm";

function AddToOrder(orderId, usePreorder, useInStock, defCatalog) {
    if (typeof (defCatalog) == 'undefined') {
        defCatalog = "none";
    }
    $.ajax({
        url: "/Admin/ModelsCatalog/IndexPopUp?defCatalog=" + defCatalog,
        dataType: 'html',
        success: function (html) {
            var response = $(html);

            $('#addModelModal .modal-body').html("");
            $('#addModelModal .modal-body').append(response.find("section.content"));


            $('#addModelModal .modal-body .selectpicker').selectpicker();

            table = $('#addModelModal .modal-body .table').DataTable({
                "ajax": {
                    "url": "/Admin/ModelsCatalog/LoadDataPopUp",
                    "type": "GET",
                    "datatype": "json",
                    "dataSrc": "",
                    "error": function (jqXHR) {
                        AjaxErrorHandler(jqXHR);
                    }
                },
                "responsive": true,
                "pagingType": $(window).width() < 1200 ? "numbers" : "simple_numbers",
                "processing": true,
                "columns": [
                    {
                        "data": "modelPhoto",
                        "render": function (data, type, full, meta) {
                            const photo = data ? data  : "/common/img/noimage.png";
                            return `<picture>
                                        <source srcset="${photo}?width=100&format=webp" type="image/webp">
                                        <img srcset="${data}" width="100" alt="Картинка"/>
                                    </picture>`;
                        },
                        "className": "min-desktop"
                    },
                    {
                        "name": "articul",
                        "data": "articul"
                    },
                    {
                        "name": "color",
                        "data": "colorName",
                        "defaultContent": "---"
                    },
                    {
                        "name": "size",
                        "data": "sizeValue",
                        "defaultContent": "---"
                    },
                    {
                        "name": "catalog",
                        "data": "catalogName",
                        "defaultContent": "---"
                    },
                    {
                        "data": "articul",
                        "searchable": false,
                        "sortable": false,
                        "render": function (data, type, full, meta) {
                            if (full.catalogId == 3) {
                                if (!usePreorder) return "";
                            } else {
                                if (!useInStock) return "";
                            }
                            var amount = (full.amount - full.amount % full.partsCount) / full.partsCount;
                            var maxVal = (amount > 99 ? 99 : (amount <= 0 ? 1 : amount));
                            return `<nobr class="jsAmountWrapper"><input type="number" min="1" max="${maxVal}" class="form-control jsAmountValue" name="item-amount" value="1" style="width:60px; float: left; margin-right: 5px;" /><button class="btn btn-square btn-main-success" data-toggle="tooltip" data-placement="bottom" title="Добавить в заказ" onClick="AddModelToOrder(this, ${orderId}, ${full.catalogId}, ${full.modelId}, ${full.colorId}, ${full.nomenclatureId}, ${full.partsCount});"><svg class="krista-icon krista-plus"><use xlink:href="#krista-plus"></use></svg></i></button></nobr>`;
                        },
                        "className": "min-desktop"
                    }
                ],
                "pageLength": 5,
                "lengthMenu": [[5, 10, 20], [5, 10, 20]],
                "dom": 'rt<"row"<"bottom col-sm-12 col-md-4 mt-3 text-left"l><"bottom col-sm-12 col-md-4 text-center"i><"bottom col-sm-12 col-md-4 col-auto"p>>',
                "language": {
                    "url": "/common/datatables.Russian.json"
                }
            })
            .order([[4, "asc"], [1, "asc"], [2, "asc"], [3, "asc"]]);

            function setColumnFilter(colName, value) {
                if (value == "") {
                    table
                        .columns(`${colName}:name`)
                        .search(value);
                } else {
                    if (colName != "articul") {
                        table
                            .columns(`${colName}:name`)
                            .search("(^" + value + "$)", true, false);
                    } else {
                        table
                            .columns(`${colName}:name`)
                            .search(value);
                    }
                }
            }

            $("#Articul").keyup(function () {
                setColumnFilter("articul", $(this).val());
                table.draw();
            });

            $('#Color').change(function () {
                setColumnFilter("color", $(this).val());
                table.draw();
            });

            $('#SizeLine').change(function () {
                setColumnFilter("size", $(this).val());
                table.draw();
            });

            $('#Catalog').change(function () {
                setColumnFilter("catalog", $(this).val());
                table.draw();
            });

            $('#Catalog').change();

            $('#addModelModal').modal('show');

        },
        error: function (request, status, error) {
            if (request.status == 401)
                window.location.reload();
            console.log("ajax call went wrong:" + request.responseText);
        }
    });
}

function ResetAllValues() {
    $('.dataTables_customSearchWrapper').find('input:text').val('').trigger('keyup').focus();
    $('.dataTables_customSearchWrapper').find('.selectpicker').val('').trigger('change');
    /*
    $('.card-body').find('input:text').val('').trigger('keyup').focus();
    $('.card-body').find('.selectpicker').val('').trigger('change');
    */
}

function AddModelToOrder(btnObj, orderId, catalogId, modelId, colorId, nomenclatureId, partsCount) {
    var parentObj = $(btnObj).parent(".jsAmountWrapper");
    if (parentObj.length <= 0) return;
    var spinnerObj = parentObj.find(".jsAmountValue");
    if (spinnerObj.length <= 0) return;
    var amount = spinnerObj.val();
    if (amount <= 0) return;

    amount = amount * partsCount;

    $.ajax({
        type: "POST",
        url: "/Admin/Orders/AddModel",
        dataType: "json",
        data: {
            "orderId": orderId,
            "catalogId": catalogId,
            "modelId": modelId,
            "colorId": colorId,
            "nomenclatureId": nomenclatureId,
            "amount": amount
        }
    }).done(function (responseText) {
        AjaxRedirectHandler(responseText);
    }).fail(function (jqXHR) {
        AjaxErrorHandler(jqXHR);
    });
}