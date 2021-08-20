var dateFormat = "D.MM.YYYY HH:mm";
var newRegistrationsTable;
$(document).ready(function () {
    const preorderTable = $(".preorderTable").DataTable({
        "data": preorderModels,
        "responsive": true,
        "columns": [
            {
                "data": "mainPhoto",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    const photo = data ? data + "?width=100" : "/common/img/noimage.png?width=100";
                    return `
                            <picture class="img-fluid">
                                <source srcset="${photo}&format=webp" type="image/webp">
                                <img srcset="${photo}" width="100" height="150" class="img-fluid" alt="${full.articul}"/>
                            </picture>`;
                }
            },
            {
                "data": "articul"
            },
            {
                "data": "colorName",
                "render": function (data, type, full, meta) {
                    const background = full.colorPhoto != "" ? full.colorValue : `url(${full.colorPhoto})`;
                    return `<div class="tal" style="text-align: left;">
                        <span class="color-round ml-0" title="${full.colorName}" style="background: ${background};"></span>
                        <div>${data}</div>
                    </div>`;
                },
                "className": "tal min-desktop"
            },
            {
                "name": "sizeValue",
                "data": "sizeValue",
                "className": "min-desktop"
            },
            {
                "data": "amount",
                "render": function (data, type, full, meta) {
                    const totalAmount = full.amount / full.partsCount;
                    if (model.isProccessedPreorder) {
                        return totalAmount;
                    } else {
                        return `<div class="form-control cart-item-spinner jsOrderItemSpinner text-center">
                                        <div class="jsBtnMinus cart-spinner-btn dn" data-orderitemid="${full.id}" onclick="OrderItemAmountDn(this);">-</div>
                                        <span class="jsAmount">${totalAmount}</span>
                                        <div class="jsBtnPlus cart-spinner-btn up" data-orderitemid="${full.id}" onclick="OrderItemAmountUp(this);">+</div>
                                    </div>`;
                    }
                },
                "className": "min-desktop"
            },
            {
                "data": "amount",
                "render": function (data, type, full, meta) {
                    return `<span class="jsTotalAmount">${data}</span>`;
                },
                "className": "min-desktop"
            },
            {
                "name": "price",
                "data": "price",
                "defaultContent": 0.0,
                "render": function (data, type, full, meta) {
                    return currencyFormatConvert(data);
                },
                "className": "min-desktop"
            },
            {
                "name": "totalSum",
                "data": "price",
                "defaultContent": 0.0,
                "render": function (data, type, full, meta) {
                    return `<span class="text-nowrap jsTotalPrice">${currencyFormatConvert(full.price * full.amount)} $</span>`;
                },
                "className": "min-desktop"
            },
            {
                "data": "id",
                "searchable": false,
                "width": "10%",
                "render": function (data, type, full, meta) {
                    if (!model.isProcessedPreorder) {
                        return `<button class="btn btn-square btn-main-danger" title="Удалить позицию из заказа" onclick="DeleteItem(${model.id}, ${full.id});"><svg class="krista-icon krista-delete"><use xlink:href="#krista-delete"></use></svg></button>`;
                    }
                    return "";
                },
                "className": "min-desktop"
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>',
        "createdRow": function (row, data, index) {
            $(row).addClass("jsParentRow");
            if (model.isProcessedPreorder) {
                $(row).addClass("table-success");
            }
        }
    });

    const inStockTable = $(".inStockTable").DataTable({
        "data": inStockModels,
        "responsive": true,
        "columns": [
            {
                "data": "mainPhoto",
                "render": function (data, type, full, meta) {
                    const path = data ? data + "?width=100" : "/common/img/noimage.png";
                    return `
                            <picture class="img-fluid">
                                    <source srcset="${path}&format=webp" type="image/webp">
                                    <img srcset="${path}" width="100" height="150" class="img-fluid" alt="${full.articul}"/>
                            </picture>`;
                }
            },
            {
                "data": "articul"
            },
            {
                "data": "colorName",
                "render": function (data, type, full, meta) {
                    const background = full.colorPhoto != "" ? full.colorValue : `url(${full.colorPhoto})`;
                    return `<div class="tal" style="text-align: left;">
                        <span class="color-round ml-0" title="${full.colorName}" style="background: ${background};"></span>
                        <div>${data}</div>
                    </div>`;
                },
                "className": "tal min-desktop"
            },
            {
                "name": "sizeValue",
                "data": "sizeValue",
                "className": "min-desktop"
            },
            {
                "data": "amount",
                "render": function (data, type, full, meta) {
                    const totalAmount = full.amount / full.partsCount;
                    if (model.isProccessedRetail) {
                        return totalAmount;
                    } else {
                        return `<div class="form-control cart-item-spinner jsOrderItemSpinner text-center">
                                        <div class="jsBtnMinus cart-spinner-btn dn" data-orderitemid="${full.id}" onclick="OrderItemAmountDn(this, ${JSON.stringify(full).split('"').join("&quot;")});">-</div>
                                        <span class="jsAmount">${totalAmount}</span>
                                        <div class="jsBtnPlus cart-spinner-btn up" data-orderitemid="${full.id}" onclick="OrderItemAmountUp(this, ${JSON.stringify(full).split('"').join("&quot;")});">+</div>
                                    </div>`;
                    }
                },
                "className": "min-desktop"
            },
            {
                "data": "amount",
                "render": function (data, type, full, meta) {
                    return `<span class="jsTotalAmount">${data}</span>`;
                },
                "className": "min-desktop"
            },
            {
                "name": "price",
                "data": "price",
                "defaultContent": 0.0,
                "render": function (data, type, full, meta) {
                    return currencyFormatConvert(data);
                },
                "className": "min-desktop"
            },
            {
                "name": "totalSum",
                "data": "price",
                "defaultContent": 0.0,
                "render": function (data, type, full, meta) {
                    return `<span class="text-nowrap jsTotalPrice">${currencyFormatConvert(full.price * full.amount)} $</span>`;
                },
                "className": "min-desktop"
            },
            {
                "data": "id",
                "searchable": false,
                "width": "10%",
                "render": function (data, type, full, meta) {
                    if (!model.isProcessedRetail) {
                        return `<button class="btn btn-square btn-main-danger" title="Удалить позицию из заказа" onclick="DeleteItem(${model.id}, ${full.id});"><svg class="krista-icon krista-delete"><use xlink:href="#krista-delete"></use></svg></button>`;
                    }
                    return "";
                },
                "className": "min-desktop"
            }
        ],
        "language": {
            "url": '/common/datatables.Russian.json'
        },
        "dom": 'rft<"row"<"bottom col-sm-12 col-md px-0 text-left"l><"bottom col-sm-12 col-md"i><"bottom col-sm-12 col-md px-0"p>>',
        "createdRow": function (row, data, index) {
            $(row).addClass("jsParentRow");
            if (model.isProcessedRetail) {
                $(row).addClass("table-success");
            }
        }
    });
});

function _sendAmountChangeData(dataObj, btnObj) {
    $.ajax({
        url: "/Admin/Orders/ItemAmountChange",
        type: "POST",
        data: dataObj,
        dataType: "json",
        success: function (result) {
            if (result.success) {
                if (result.needUpdate) {
                    $(".jsOrderTotalAmount").text(result.orderItemsCount);

                    if ($(".jsOrderTotalPriceInStock").length > 0) {
                        $(".jsOrderTotalPriceInStock").text(result.orderTotalPriceInStock);
                    }
                    if ($(".jsOrderTotalPricePreorder").length > 0) {
                        $(".jsOrderTotalPricePreorder").text(result.orderTotalPricePreorder);
                    }
                    if ($(".jsOrderTotalPrepayPreorder").length > 0) {
                        $(".jsOrderTotalPrepayPreorder").text(result.orderTotalPrepayPreorder);
                    }
                    if ($(".jsOrderTotalPrice").length > 0) {
                        $(".jsOrderTotalPrice").text(result.orderTotalPrice);
                    }
                    if ($(".jsOrderTotalAmountInStock").length > 0) {
                        $(".jsOrderTotalAmountInStock").text(result.orderTotalAmountInStock);
                    }
                    if ($(".jsOrderTotalAmountPreorder").length > 0) {
                        $(".jsOrderTotalAmountPreorder").text(result.orderTotalAmountPreorder);
                    }

                    var spinnerObj = btnObj.parent(".jsOrderItemSpinner").find(".jsAmount");

                    spinnerObj.text(result.amount);

                    var parentRowObj = spinnerObj.parents("tr");

                    var totalAmountObj = parentRowObj.find(".jsTotalAmount");
                    totalAmountObj.text(result.totalAmount);

                    var totalPriceObj = parentRowObj.find(".jsTotalPrice");
                    totalPriceObj.text(result.totalPrice);

                    updateModelsList(dataObj.id, result.totalAmount);
                }
            } else {
                showNotificationError(result.message);
            }
        },
        error: function (jqXHR) {
            if (jqXHR.status == 401)
                window.location.reload();
            showNotificationError("Ошибка на сервере при попытке добавления товара в корзину.");

            console.log(jqXHR);
        }
    });
}

function OrderItemAmountDn(btn, item) {
    const btnObj = $(btn);
    const itemId = btnObj.attr("data-orderitemid");

    const dataObj = {
        orderId: model.id,
        id: itemId,
        dir: "dn"
    };

    _sendAmountChangeData(dataObj, btnObj);
}

function OrderItemAmountUp(btn, item) {
    const btnObj = $(btn);
    const itemId = btnObj.attr("data-orderitemid");

    const dataObj = {
        orderId: model.id,
        id: itemId,
        dir: "up"
    };

    _sendAmountChangeData(dataObj, btnObj);
}

function updateModelsList(id, amount) {
    debugger;
    var item = preorderModels.find(x => x.id === +id);
    if (item == undefined) {
        item = inStockModels.find(x => x.id === +id);
    }
    item.amount = amount;
    item.totalPrice = item.price * item.amount;
    item.totalPriceInRub = item.priceInRub * item.amount;
}

function DeleteItem(orderId, id) {
    Swal.fire({
        title: "Вы уверены?",
        text: "Выбранная позиция будет удалена из заказа!",
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "Отмена",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Да, удалить позицию!",
    }).then((result) => {
        if (result.value) {
            var url = "/Admin/Orders/DeleteItem";
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    "orderId": orderId,
                    "id": id
                }
            }).done(function (responseText) {
                AjaxRedirectHandler(responseText);
            }).fail(function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            });
        }
    });
}

function DeleteOrder(id, delMode) {
    if (typeof (delMode) == 'undefined') {
        delMode = 'normal';
    }
    var message = "Заказ будет полностью удален!";
    var uriPostfix = "";
    var yesButton = "Да, удалить заказ";
    if (delMode == "preorder") {
        message = "Все позиции типа \"Предзаказ\" будут удалены!";
        uriPostfix = "?mode=preorder";
        yesButton = "Да, удалить";
    } else if (delMode == "retail") {
        message = "Все позиции типа \"Наличие\" будут удалены!";
        uriPostfix = "?mode=retail";
        yesButton = "Да, удалить";
    }
    Swal.fire({
        title: "Вы уверены?",
        text: message,
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "Отмена",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: yesButton,
    }).then((result) => {
        if (result.value) {
            var url = "/Admin/Orders/DeleteOrder" + uriPostfix;
            $.ajax({
                type: "POST",
                url: url,
                data: {
                    "id": id
                }
            }).done(function (responseText) {
                AjaxRedirectHandler(responseText);
            }).fail(function (jqXHR) {
                AjaxErrorHandler(jqXHR);
            });
        }
    });
}

function sendCheckOrderAsReviewedRequest() {
    $.ajax({
        url: `/Admin/Orders/CheckAsReviewed?orderId=${model.id}`,
        type: "POST"
    }).done(function (responseText) {
    }).fail(function (jqXHR) {
        AjaxErrorHandler(jqXHR);
    });
}