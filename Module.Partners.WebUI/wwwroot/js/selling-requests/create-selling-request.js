var storehouseItems;
$(function () {
    storehouseItems = storehouseDataModel;
    if (_.find(dataModel, x => x.state === Constants.DocumentStates.Created) === undefined) {
        function addColumnsForFilterToDataSource() {
            for(var item of storehouseItems) {
                const sizes = _.chain(item.sizesInfo.totalAmountBySize).toPairs().filter(x => x[1] > 0).map(x => x[0]).value();
                sizes.push(item.modelInfo.size.line);

                item.sizesString = sizes.join(" ");
                item.colorsString = _.map(item.colors, x => x.name).join(" ");
            }
        }
        addColumnsForFilterToDataSource();

        const table = $(".storehouseItemsTable").DataTable({
            "data": storehouseItems,
            "responsive": true,
            "paging": false,
            "columns": [
                {
                    "name": "modelKey",
                    "data": "modelKey",
                    "sortable": false,
                    "render": function (data, type, full, meta) {
                        if (type !== "display") {
                            return full.modelInfo.name;
                        }

                        return getModelGridRenderer().renderModelGrid(full);
                    }
                },
                {
                    "name": "allColors",
                    "data": "colorsString",
                    "visible": false
                },
                {
                    "name": "allSizes",
                    "data": "sizesString",
                    "visible": false
                },
                {
                    "name": "totalAmount",
                    "data": "totalAmount",
                    "visible": false,
                },
                {
                    "name": "totalSum",
                    "data": "totalSum",
                    "visible": false,
                    "render": function (data, type, full, meta) {
                        return `${currencyFormatConvert(data)} $`;
                    }
                },
                {
                    "name": "actionButtons",
                    "data": "modelKey",
                    "visible": true,
                    "sortable": false,
                    "width": "15%",
                    "render": function (data, type, full, meta) {
                        return `<div><a class="btn btn-basic btn-success btn-shop dynamic-height" data-request-model-sell="${data}">Выбрать</a></div>`;
                    },
                    "className": "align-middle"
                }
            ],
            "language": {
                "url": "/common/datatables.Russian.json"
            },
            "dom": "r"
        });
        new RequestSellModalView(storehouseItems);

        initDataTablesCustomSorting({
            table: table,
            sortByColumn: {name: "modelKey", direction: "asc"}
        });

        const filter = new DataTableCustomFilter([table], {
            applyByButton: false,
            blockWrapperSelector: ".customSearchWrapper"
        });
    }
});

function RequestSellModalView(storehouseItems) {
    this.storehouseItems = storehouseItems;
    this.requestModal = $("#createSellingRequestStorehouseItem");
    this.storehouseItemsWrapper = $("#sellingRequestAmounts");
    this.requestedTotalAmount = $("#requestedTotalAmount");
    this.acceptButton = $(".accept-model-request-btn");
    this.createButton = $(".createSellingRequestBtn");
    this.currentStorehouseItem = undefined;
    this.sellingRequest = new SellingRequest();
    this.currentButton = undefined;
    const self = this;
    
    this.onInitialize = function () { 
        addEventHandlers();
    }
    
    function addEventHandlers() { 
        $(document).on("click", "[data-request-model-sell]", openModal);
        self.acceptButton.on("click", acceptModelRequest);
        self.createButton.on("click", createSellingRequest);
        $(document).on("click", "[data-model-amount-change]", function() {
            updateSellingRequestItemAmount(JSON.parse(this.dataset.modelAmountChange), $(this).parent().find(".itemAmount"));
        });
    }
    
    function openModal(e) {
        e.preventDefault();

        self.currentStorehouseItem = self.getStorehouseItemByModelKey(this.dataset.requestModelSell);
        self.sellingRequest.setCurrentModel(Object.deepCopy(self.currentStorehouseItem));
        self.currentButton = $(this);
        
        self.storehouseItemsWrapper.empty();        
        self.storehouseItemsWrapper.append(getModelGridRenderer().renderModelAmountsGridWithEditButtons(self.sellingRequest.currentModel));
        setTotalAmountInModalWindow();

        self.requestModal.modal("show");
    }

    function updateSellingRequestItemAmount (updateData, valueWrapper) {
        const requestedAmount = self.sellingRequest.getCurrentAmountBySizeAndColor(updateData.sizeValue, updateData.colorId) + updateData.amount;
        if(!isRequestedAmountValid(requestedAmount, updateData.sizeValue, updateData.colorId)) {
            return;
        }
        
        if (+valueWrapper.html() + updateData.amount >= 0) {
            self.sellingRequest.updateCurrentRealModelAmount(updateData.sizeValue, updateData.colorId, updateData.amount);
            valueWrapper.html(self.sellingRequest.getCurrentAmountBySizeAndColor(updateData.sizeValue, updateData.colorId));
        }

        $(`#totalPerSize${updateData.sizeValue}`).html(self.sellingRequest.getCurrentTotalAmountBySize(updateData.sizeValue));
        $(`#totalPerColor${updateData.colorId}`).html(self.sellingRequest.getCurrentTotalAmountByColor(updateData.colorId));
        $("#modelRealTotal").html(self.sellingRequest.currentModel.totalAmount);
        setTotalAmountInModalWindow();
    }

    function isRequestedAmountValid (requestedAmount, sizeValue, colorId) {
        const availableAmount = self.currentStorehouseItem.sizesInfo.sizeColorAmount[`${sizeValue}_${colorId}`];
        return availableAmount !== undefined && availableAmount >= requestedAmount && requestedAmount >= 0;
    }

    function acceptModelRequest() {
        self.sellingRequest.addCurrentModel();
        if (self.sellingRequest.currentModel.totalAmount > 0) {
            self.currentButton.text(`Выбрано ${self.sellingRequest.currentModel.totalAmount} из ${self.currentStorehouseItem.totalAmount}`);
            self.currentButton.removeClass("btn-success");
            self.currentButton.addClass("btn-primary");
        } else {
            self.currentButton.text(`Выбрать`);
            self.currentButton.removeClass("btn-primary");
            self.currentButton.addClass("btn-success");
        }
        self.sellingRequest.clearCurrentModel();
        self.requestModal.modal("hide");
    }

    function setTotalAmountInModalWindow () {
        self.requestedTotalAmount.html(`Итого ${self.sellingRequest.currentModel.totalAmount} ед`);
    }

    function createSellingRequest() {
        $.ajax({
            type: "POST",
            data: JSON.stringify(self.sellingRequest.transformRequestItems()),
            url: "/Partners/SellingRequests/Create",
            contentType: "application/json"
        }).done(function (result) {
            Notification.successModal({
                title: 'Запрос принят',
                text: 'Ваш запрос принят в обработку. В ближайшее время наши менеджеры свяжутся с вами. Пожалуйста ожидайте'
            }).then((result) => {
                location.reload();
            });
        }).catch(function (error) {
            showOperationResultNotification(error);
        });
    }
    
    this.getStorehouseItemByModelKey = function(modelKey) {
        return _.chain(self.storehouseItems).find(x => x.modelKey === modelKey).value();
    };
    
    this.onInitialize();
}

function SellingRequest(requestItems, isEdit = false) {
    this.isEdit = isEdit;
    this.requestItems = isEdit ? requestItems : [];
    this.currentModel = undefined;
    const self = this;

    this.onInitialize = function () {
        
    }

    this.setCurrentModel = function (storehouseModelCopy) {
        if (!isEdit) {
            setCurrentModelForCreate(storehouseModelCopy);
        } else {
            setCurrentModelForEdit(storehouseModelCopy)
        }
    }

    function setCurrentModelForCreate(storehouseModelCopy) {
        const existingModel = _.chain(self.requestItems).find(x => x.modelKey === storehouseModelCopy.modelKey).value();

        if (existingModel === undefined) {
            self.currentModel = storehouseModelCopy;
            for (var item in self.currentModel.sizesInfo.sizeColorAmount) {
                if (self.currentModel.sizesInfo.sizeColorAmount.hasOwnProperty(item)) {
                    self.currentModel.sizesInfo.sizeColorAmount[item] = 0;
                }
            }

            for (var sizeAmount in self.currentModel.sizesInfo.totalAmountBySize) {
                if (self.currentModel.sizesInfo.totalAmountBySize.hasOwnProperty(sizeAmount)) {
                    self.currentModel.sizesInfo.totalAmountBySize[sizeAmount] = 0;
                }
            }

            for (var colorAmount in self.currentModel.totalAmountByColor) {
                if (self.currentModel.totalAmountByColor.hasOwnProperty(colorAmount)) {
                    self.currentModel.totalAmountByColor[colorAmount] = 0;
                }
            }
            self.currentModel.totalAmount = 0;
        } else {
            self.currentModel = existingModel;
        }
    }
    
    function setCurrentModelForEdit(storehouseModelCopy) {

        const existingModel = _.chain(self.requestItems).find(x => x.modelKey === storehouseModelCopy.modelKey).value();

        for (var item in storehouseModelCopy.sizesInfo.sizeColorAmount) {
            if (storehouseModelCopy.sizesInfo.sizeColorAmount.hasOwnProperty(item)) {
                storehouseModelCopy.sizesInfo.sizeColorAmount[item] = 0;
            }
        }

        for (var sizeAmount in storehouseModelCopy.sizesInfo.totalAmountBySize) {
            if (storehouseModelCopy.sizesInfo.totalAmountBySize.hasOwnProperty(sizeAmount)) {
                storehouseModelCopy.sizesInfo.totalAmountBySize[sizeAmount] = 0;
            }
        }

        for (var colorAmount in storehouseModelCopy.totalAmountByColor) {
            if (storehouseModelCopy.totalAmountByColor.hasOwnProperty(colorAmount)) {
                storehouseModelCopy.totalAmountByColor[colorAmount] = 0;
            }
        }
        storehouseModelCopy.totalAmount = 0;

        Object.assign(storehouseModelCopy.sizesInfo.sizeColorAmount, existingModel.sizesInfo.sizeColorAmount);
        Object.assign(storehouseModelCopy.sizesInfo.totalAmountBySize, existingModel.sizesInfo.totalAmountBySize);

        existingModel.barcodes = storehouseModelCopy.barcodes;
        existingModel.sizesInfo = storehouseModelCopy.sizesInfo;

        self.currentModel = existingModel;
    }
    
    this.addCurrentModel = function () {
        let existingItem = _.chain(self.requestItems).find(x => x.modelKey === self.currentModel.modelKey).value();
        if(!isEdit && existingItem === undefined && self.currentModel.totalAmount > 0) {
            self.requestItems.push(self.currentModel);
        }

        if(isEdit) {
            const index = self.requestItems.indexOf(existingItem);
            if (index !== -1) {
                self.requestItems[index] = self.currentModel;
            }
        }
    }
    
    this.clearCurrentModel = function() {
        self.currentModel = undefined;
    }

    this.updateCurrentRealModelAmount = function(sizeValue, colorId, amount) {
        self.currentModel.sizesInfo.sizeColorAmount[`${sizeValue}_${colorId}`] += amount;
        self.currentModel.sizesInfo.totalAmountBySize[sizeValue] += amount;
        self.currentModel.totalAmountByColor[colorId] += amount;
        self.currentModel.totalAmount += amount;
    };

    this.getCurrentAmountBySizeAndColor = function (sizeValue, colorId) {
        return self.currentModel.sizesInfo.sizeColorAmount[`${sizeValue}_${colorId}`]
    };

    this.getCurrentTotalAmountBySize = function(sizeValue) {
        return self.currentModel.sizesInfo.totalAmountBySize[sizeValue];
    };

    this.getCurrentTotalAmountByColor = function(colorId) {
        return self.currentModel.totalAmountByColor[colorId];
    };

    this.transformRequestItems = function () {
        return _.chain(self.requestItems).map((model) => {
            var result = [];
            for (var color of model.colors) {
                for (var sizeValue of model.sizesInfo.values) {
                    const amount = model.sizesInfo.sizeColorAmount[`${sizeValue}_${color.id}`];
                    if (amount === undefined) {
                        continue;
                    }

                    const colorId = color.id;
                    const size = sizeValue;
                    result.push({
                        barcode: _.find(model.barcodes, x => x.size.value === size && x.colorId === colorId).barcode,
                        amount: amount
                    });
                }
            }
            return result;
        }).flatten().value()
    };

    this.transformRequestItemsToModelAmount = function (modelBarcodes) {
        return _.chain(self.requestItems).map((model) => {
            var result = [];
            for (var color of model.colors) {
                for (var sizeValue of model.sizesInfo.values) {
                    const amount = model.sizesInfo.sizeColorAmount[`${sizeValue}_${color.id}`];
                    if (amount === undefined) {
                        continue;
                    }

                    const colorId = color.id;
                    const size = sizeValue;
                    result.push({
                        barcode: _.find(modelBarcodes, x => x.modelId === model.modelInfo.modelId && x.size.value === size && x.colorId === colorId).barcode,
                        amount: amount
                    });
                }
            }
            return result;
        }).flatten().value()
    };
    
    this.onInitialize();
}