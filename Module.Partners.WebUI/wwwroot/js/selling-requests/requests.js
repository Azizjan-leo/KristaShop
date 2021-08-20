$(function () {
    const documents = dataModel;
    const tables = [];

    for (let document of documents) {
        const documentItems = document.items;

        function addColumnsForFilterToDataSource() {
            for(var item of documentItems) {
                const sizes = _.chain(item.sizesInfo.totalAmountBySize).toPairs().filter(x => x[1] > 0).map(x => x[0]).value();
                sizes.push(item.modelInfo.size.line);

                item.sizesString = sizes.join(" ");
                item.colorsString = _.map(item.colors, x => x.name).join(" ");
            }
        }
        addColumnsForFilterToDataSource();

        const table = $(`.documentItemsTable-${document.number}`).DataTable({
            "data": documentItems,
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
                    "data": "modelKey",
                    "render": function (data, type, full, meta) {
                        if(document.state === Constants.DocumentStates.Created) {
                            return `<div><a class="btn btn-basic btn-success btn-shop dynamic-height" data-edit-request-model="${data}">Изменить</a></div>`;
                        }
                        return "";
                    },
                    "className": "align-middle"
                },
            ],
            "language": {
                "url": "/common/datatables.Russian.json"
            },
            "dom": "r"
        });
        
        tables.push(table);

        initDataTablesCustomSorting({
            table: table,
            sortByColumn: {name: "modelKey", direction: "asc"}
        });
        
        if(document.state === 0) {
            new SellingRequestUpdate(documentItems, storehouseDataModel, table);
        }
    }
    
    const filter = new DataTableCustomFilter(tables, {
        applyByButton: false,
        blockWrapperSelector: ".customSearchWrapper"
    });
});

function SellingRequestUpdate(requestItems, storehouseItems, table) {
    this.storehouseItems = storehouseItems;
    this.requestItems = requestItems;
    this.sellingRequest = new SellingRequest(requestItems, true);
    this.dataTable = table;
    this.requestModal = $("#createSellingRequestStorehouseItem");
    this.storehouseItemsWrapper = $("#sellingRequestAmounts");
    this.requestedTotalAmount = $("#requestedTotalAmount");
    this.acceptButton = $(".accept-model-request-btn");
    this.updateButton = $(".updateSellingRequestBtn");
    this.openStorehouseModalButton = $(".addItemFromStorehouseBtn");
    this.currentStorehouseItem = undefined;
    this.currentRow = undefined;
    this.storehouseModal = undefined;
    const self = this;

    this.onInitialize = function () {
        addEventHandlers();
    }

    function addEventHandlers() {
        $(document).on("click", "[data-edit-request-model]", openModal);
        self.updateButton.on("click", updateSellingRequest);
        $(document).on("click", "[data-model-amount-change]", function() {
            updateSellingRequestItemAmount(JSON.parse(this.dataset.modelAmountChange), $(this).parent().find(".itemAmount"));
        });
        self.acceptButton.on("click", acceptModelRequest);
        self.openStorehouseModalButton.on("click", openStorehouseModal);
        self.storehouseModal = new StorehouseModal(self.storehouseItems, '.storehouseItemsModalTable', updateModels);
    }

    function openModal(e) {
        e.preventDefault();

        self.currentStorehouseItem = self.getStorehouseItemByModelKey(this.dataset.editRequestModel);
        self.sellingRequest.setCurrentModel(Object.deepCopy(self.currentStorehouseItem));
        self.currentButton = $(this);
        self.currentRow = self.dataTable.row($(this).parents('tr')[0]);

        self.storehouseItemsWrapper.empty();
        self.storehouseItemsWrapper.append(getModelGridRenderer().renderModelAmountsGridWithEditButtons(self.sellingRequest.currentModel));

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

    function acceptModelRequest() {
        self.sellingRequest.addCurrentModel();
        self.currentRow.data(self.sellingRequest.currentModel);
        self.sellingRequest.clearCurrentModel();
        self.requestModal.modal("hide");
    }

    function updateSellingRequest() {
        $.ajax({
            type: "POST",
            data: JSON.stringify(self.sellingRequest.transformRequestItemsToModelAmount(_.flatten(_.map(storehouseItems, x => _.map(x.barcodes, c=>Object.assign(c, {modelId: x.modelInfo.modelId})))))),
            url: "/Partners/SellingRequests/Update",
            contentType: "application/json"
        }).done(function (result) {
            console.log(result);
            Notification.successModal({
                title: 'Запрос изменен',
                text: 'Ваш запрос изменен и принят в обработку. В ближайшее время наши менеджеры свяжутся с вами. Пожалуйста ожидайте'
            });
        }).catch(function (error) {
            showOperationResultNotification(error);
        });
    }

    function openStorehouseModal() {
        self.storehouseModal.openModal(_.map(self.requestItems, function(item) { return item.modelKey }));
    }

    function updateModels(selectedStorehouseItems) { 
        const removedItems = _.differenceBy(self.requestItems, selectedStorehouseItems, 'modelKey');
        if(removedItems.length > 0) {
            for(const item of removedItems) {
                self.requestItems.splice(self.requestItems.indexOf(item), 1);
            }
        }

        const addedItems = _.differenceBy(selectedStorehouseItems, self.requestItems, 'modelKey');
        if(addedItems.length > 0) {
            for (const item of addedItems) {
                const newItem = Object.deepCopy(item);

                for (const sizeColorAmountKey in newItem.sizesInfo.sizeColorAmount) {
                    if (newItem.sizesInfo.sizeColorAmount.hasOwnProperty(sizeColorAmountKey)) {
                        newItem.sizesInfo.sizeColorAmount[sizeColorAmountKey] = 0;
                    }
                }

                for (const sizeAmount in newItem.sizesInfo.totalAmountBySize) {
                    if (newItem.sizesInfo.totalAmountBySize.hasOwnProperty(sizeAmount)) {
                        newItem.sizesInfo.totalAmountBySize[sizeAmount] = 0;
                    }
                }

                for (const colorAmount in newItem.totalAmountByColor) {
                    if (newItem.totalAmountByColor.hasOwnProperty(colorAmount)) {
                        newItem.totalAmountByColor[colorAmount] = 0;
                    }
                }
                newItem.totalAmount = 0;

                const sizes = _.chain(newItem.sizesInfo.totalAmountBySize).toPairs().filter(x => x[1] > 0).map(x => x[0]).value();
                sizes.push(newItem.modelInfo.size.line);

                newItem.sizesString = sizes.join(" ");
                newItem.colorsString = _.map(newItem.colors, x => x.name).join(" ");
                
                self.requestItems.push(newItem);
            }
        }

        redrawTableWithNewData(self.dataTable, self.requestItems);
    }
    
    function setTotalAmountInModalWindow () {
        self.requestedTotalAmount.html(`Итого ${self.sellingRequest.currentModel.totalAmount} ед`);
    }

    function isRequestedAmountValid (requestedAmount, sizeValue, colorId) {
        const availableAmount = self.currentStorehouseItem.sizesInfo.sizeColorAmount[`${sizeValue}_${colorId}`];
        return availableAmount !== undefined && availableAmount >= requestedAmount && requestedAmount >= 0;
    }
    
    this.getStorehouseItemByModelKey = function(modelKey) {
        return _.chain(self.storehouseItems).find(x => x.modelKey === modelKey).value();
    };

    this.onInitialize();
}