let revisionTable;
let revisionHandler = {};

$(function () {
    revisionHandler = new RevisionView(new Revision(dataModel.items, dataModel.totals.paymentRate));
    
    revisionTable = $(".revisionTable").DataTable({
        "data": revisionHandler.revision.dbStorehouseItems,
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
                "width": "16%",
                "sortable": false,
                "render": function (data, type, full, meta) {
                    return `<div style="display: none" class="modelRevisionBtn">
                                <a class="btn btn-basic btn-success btn-shop dynamic-height"
                                 id="modelBtn${full.modelInfo.modelId}" data-open-revision-modal="${data}">
                                 Ревизия по модели
                                </a>
                            </div>`;
                },
                "className": "align-middle"
            }
        ],
        "language": {
            "url": "/common/datatables.Russian.json"
        },
        "dom": "r"
    });
    
    initDataTablesCustomSorting({
        table: revisionTable,
        sortByColumn: {name: "modelKey", direction: "asc"}
    });

    const filter = new DataTableCustomFilter([revisionTable], {
        applyByButton: false,
        blockWrapperSelector: ".customSearchWrapper"
    });
});

function RevisionView(revisionOptions) {
    this.revision = revisionOptions;
    this.dbStorehouseItemsWrapper = $("#dbStorehouseAmounts");
    this.realStorehouseItemsWrapper = $("#realStorehouseAmounts");
    this.revisionTitleWrapper = $(".revisionModalName");
    this.startButton = $(".startBtn");
    this.bottomStartButton = $("#bottomStartBtn");
    this.acceptModelRevisionButton = $(".accept-model-revision-btn");
    this.lackButton = $("#lackBtn");
    this.revisionItemsModal = $("#revisionItemModal");
    this.tableActionColumn = $("#actionColumn");
    this.paymentWrapper = $(".payment-wrapper");
    this.paymentSumWrapper = $(".payment");
    this.totalsTextWrapper = $(".totals");
    const self = this;

    this.addEventHandlers = function () {
        self.startButton.on("click", self.startRevision);
        self.acceptModelRevisionButton.on("click", self.acceptModelRevision)
        $(document).on("click", "[data-open-revision-modal]", function () { self.openRevisionItemModal(this.dataset.openRevisionModal) });
        $(document).on("click", "[data-model-amount-change]", function() {
            self.updateRealStorehouseItemAmount(JSON.parse(this.dataset.modelAmountChange), $(this).parent().find(".itemAmount"));
        });
    };
    
    this.startRevision = function (event) {
        event.stopPropagation();
        if (!self.revision.revisionStarted) {
            self.setStartedRevisionButtons();
            self.revision.startRevision();
        } else {
            if (self.revision.canFinishRevision()) {
                self.finishRevision();
            } else {
                self.showCancelRevisionModal(self.revision.getNotReviewedStorehouseModelsCount());
            }
        }
    };

    this.setStartedRevisionButtons = function () {
        self.setStartButtonText(0, self.revision.getDbStorehouseItemsTotal());
        self.setLackButtonText(0);
        self.tableActionColumn.html("По ревизии");
        $(".modelRevisionBtn").show();
    };

    this.finishRevision = function () {
        $.ajax({
            type: "POST",
            data: JSON.stringify(this.revision.transformRealStorehouseItemsForRevisionRequest()),
            url: "/Partners/Shop/AuditStorehouseItems",
            contentType: "application/json"
        }).done(function (result) {
            self.revision.finishRevision(result.items, result.totals.paymentRate);
            redrawTableWithNewData(revisionTable, result.items);
            self.startButton.html("Начать ревизию");
            self.bottomStartButton.attr('hidden', true);
            self.totalsTextWrapper.html(`${self.revision.getDbStorehouseItemsTotal()} единиц`);
        }).catch(function (error) {
            showOperationResultNotification(error);
        });
    };

    this.openRevisionItemModal = function (modelKey) {
        self.revision.switchCurrentModel(modelKey);
        self.revisionTitleWrapper.text(self.revision.currentDbModel.modelInfo.name);

        self.dbStorehouseItemsWrapper.empty();
        self.dbStorehouseItemsWrapper.append(getModelGridRenderer().renderModelAmountsGrid(self.revision.currentDbModel));

        self.realStorehouseItemsWrapper.empty();
        self.realStorehouseItemsWrapper.append(getModelGridRenderer().renderModelAmountsGridWithEditButtons(self.revision.currentRealModel));

        self.setTotalAmountInModalWindow();
        self.showBillTextInModalWindow();
        self.showRevisionModalWindow();
    };

    this.updateRealStorehouseItemAmount = function (updateData, valueWrapper) {
        if (self.revision.modelHasAmountForSizeAndColor(self.revision.currentRealModel, updateData.sizeValue, updateData.colorId)) {
            return;
        }

        if (+valueWrapper.html() + updateData.amount >= 0) {
            self.revision.updateCurrentRealModelAmount(updateData.sizeValue, updateData.colorId, updateData.amount);
            valueWrapper.html(self.revision.getModelAmountBySizeAndColor(self.revision.currentRealModel, updateData.sizeValue, updateData.colorId));
        }

        $(`#totalPerSize${updateData.sizeValue}`).html(self.revision.getModelTotalAmountBySize(self.revision.currentRealModel, updateData.sizeValue));
        $(`#totalPerColor${updateData.colorId}`).html(self.revision.getModelTotalAmountByColor(self.revision.currentRealModel, updateData.colorId));
        $("#modelRealTotal").html(self.revision.currentRealModel.totalAmount);
        self.setTotalAmountInModalWindow();
        self.showBillTextInModalWindow();
    };

    this.acceptModelRevision = function () {
        self.hideRevisionModalWindow();
        self.revision.finishRevisionByCurrentModel();
        self.setModelRevisionResultToModelButton();

        const dbTotal = self.revision.getDbStorehouseItemsTotal();
        self.setStartButtonText(self.revision.getReviewedStorehouseItemsTotalAmount(), dbTotal);
        self.bottomStartButton.removeAttr('hidden');
        self.totalsTextWrapper.html(`${self.revision.getReviewedStorehouseItemsTotalAmount()} из ${dbTotal} единиц`);
        self.setLackButtonText();
    };

    this.setStartButtonText = function (actualAmount, expectedAmount) {
       self.startButton.html(`Завершить ${actualAmount} из ${expectedAmount}`);
    };

    this.setLackButtonText = function () {
        let amount = this.revision.getLackTotalAmount();
        if(amount < 0) {
            amount = 0;
        }
        self.lackButton.html(`Недостача по ревизии ${amount} единиц`);
        self.lackButton.show();
    };

    this.setModelRevisionResultToModelButton = function () {
        const modelRevisionButton = $(`#modelBtn${self.revision.currentDbModel.modelInfo.modelId}`);
        modelRevisionButton.text(`${self.revision.currentRealModel.totalAmount}/${self.revision.currentDbModel.totalAmount}`);

        if (self.revision.currentRealModel.lackAmount > 0)
            modelRevisionButton.removeClass("btn-success").addClass("btn-danger");
        else {
            modelRevisionButton.removeClass("btn-danger").addClass("btn-success");
        }
    };

    this.setTotalAmountInModalWindow = function () {
        $("#modelItemsCount").html(`${self.revision.currentRealModel.totalAmount} ед из ${self.revision.currentDbModel.totalAmount}`);
    };

    this.showBillTextInModalWindow = function () {
        if (self.revision.getTotalAmountToPayFor() > 0) {
            self.paymentWrapper.show();
            self.paymentSumWrapper.html(`<span>${self.revision.getTotalAmountToPayFor()}</span> <i class='bx bx-x align-self-center'></i> <span>${currencyFormatConvert(self.revision.paymentSumForSingleItem)} $</span><span class="mx-1">= ${currencyFormatConvert(self.revision.getTotalSumToPay())} $</span>`);
        } else {
            self.paymentWrapper.hide();
        }
    };

    this.showRevisionModalWindow = function () {
        self.revisionItemsModal.modal("show");
    };

    this.hideRevisionModalWindow = function () {
        self.revisionItemsModal.modal("hide");
    };

    this.showCancelRevisionModal = function (unreviewedAmount) {
        Swal.fire({
            icon: "error",
            title: 'Отмена ревизии',
            text: `Вы уверены, что хотите отменить ревизию? Осталось моделей: ${unreviewedAmount}.`,
            showCancelButton: true,
            cancelButtonText: "Нет",
            showConfirmButton: true,
            confirmButtonText: "Да",
            buttonsStyling: false,
            customClass: {
                container: "mt-0",
                popup: "py-4 px-3 px-lg-5",
                title: "header-font h4-smaller text-uppercase text-main-danger text-bold mb-3",
                content: "main-font h2 px-0 px-lg-1 pb-3 mx-0 mb-0 mb-lg-4",
                cancelButton: "btn-basic btn-basic-big btn-success m-1",
                confirmButton: "btn-basic btn-basic-big btn-danger m-1",
                actions: "my-3 pb-1"
            }
        }).then((result) => {
            if (result.isConfirmed) {
                location.reload();
            }
        });
    }
    
    this.addEventHandlers();
}

function Revision(dbStorehouseItems, paymentRate) {
    const GENERAL_PARTNERS_PAYMENT_RATE = 15;
    this.dbStorehouseItems = [];
    this.realStorehouseItems = [];
    this.currentDbModel = {};
    this.currentRealModel = {};
    this.paymentSumForSingleItem = GENERAL_PARTNERS_PAYMENT_RATE;
    this.revisionStarted = false;
    const self = this;

    this.onInitialize = function() {
        self.resetRevision(dbStorehouseItems, paymentRate);
    }
    
    this.resetRevision = function (dbStorehouseItems, paymentRate) {
        self.dbStorehouseItems = dbStorehouseItems;
        self.paymentSumForSingleItem = paymentRate;
        self.realStorehouseItems = Object.deepCopy(self.dbStorehouseItems);

        for (var item of self.realStorehouseItems) {
            item.isReviewed = false;
            item.lackAmount = 0;
        }

        self.currentDbModel = {};
        self.revisionStarted = false;
        self.addColumnsForFilterToDataSource();
    };

    this.addColumnsForFilterToDataSource = function() {
        for(var item of self.dbStorehouseItems) {
            const sizes = _.chain(item.sizesInfo.totalAmountBySize).toPairs().filter(x => x[1] > 0).map(x => x[0]).value();
            sizes.push(item.modelInfo.size.line);

            item.sizesString = sizes.join(" ");
            item.colorsString = _.map(item.colors, x => x.name).join(" ");
        }
    }

    this.getDbStorehouseItemsTotal = function() {
        return _.sumBy(self.dbStorehouseItems, "totalAmount");
    };

    this.getDbStorehouseItemByModelKey = function(modelKey) {
        return _.chain(self.dbStorehouseItems).find(x => x.modelKey === modelKey).value();
    };

    this.getRealStorehouseItemByModelKey = function(modelKey) {
        return _.chain(self.realStorehouseItems).find(x => x.modelKey === modelKey).value();
    };

    this.getReviewedStorehouseItemsTotalAmount = function() {
        return _.chain(self.realStorehouseItems).filter(x=>x.isReviewed === true).sumBy("totalAmount").value();
    };

    this.getNotReviewedStorehouseModelsCount = function() {
        return _.chain(self.realStorehouseItems).filter(x=>x.isReviewed === false).value().length;
    };

    this.startRevision = function () {
        self.revisionStarted = true;
    };

    this.switchCurrentModel = function(modelKey) {
        self.currentDbModel = self.getDbStorehouseItemByModelKey(modelKey);
        self.currentRealModel = Object.deepCopy(self.getRealStorehouseItemByModelKey(modelKey));
    };

    this.canFinishRevision = function() {
        return self.getNotReviewedStorehouseModelsCount() <= 0;
    };

    this.updateCurrentRealModelAmount = function(sizeValue, colorId, amount) {
        self.currentRealModel.sizesInfo.sizeColorAmount[`${sizeValue}_${colorId}`] += amount;
        self.currentRealModel.sizesInfo.totalAmountBySize[sizeValue] += amount;
        self.currentRealModel.totalAmountByColor[colorId] += amount;
        self.currentRealModel.totalAmount += amount;
    };

    this.finishRevisionByCurrentModel = function() {
        self.currentRealModel.isReviewed = true;
        self.currentRealModel.lackAmount = self.currentDbModel.totalAmount - self.currentRealModel.totalAmount;

        const index = _.chain(self.realStorehouseItems).findIndex(x => x.modelKey === self.currentDbModel.modelKey).value();
        self.realStorehouseItems[index] = self.currentRealModel;
    };

    this.getTotalAmountToPayFor = function() {
        return self.currentDbModel.totalAmount - self.currentRealModel.totalAmount;
    };

    this.getTotalSumToPay = function() {
        return self.getTotalAmountToPayFor() * self.paymentSumForSingleItem;
    };

    this.finishRevision = function (updatedDbStorehouseItems, paymentRate) {
        self.resetRevision(updatedDbStorehouseItems, paymentRate);
    };

    this.getLackTotalAmount = function (){
        return _.sumBy(self.realStorehouseItems, x => x.lackAmount);
    };

    this.modelHasAmountForSizeAndColor = function (model, sizeValue, colorId) {
        return model.sizesInfo.sizeColorAmount[`${sizeValue}_${colorId}`] === undefined;
    };

    this.getModelAmountBySizeAndColor = function (model, sizeValue, colorId) {
        return model.sizesInfo.sizeColorAmount[`${sizeValue}_${colorId}`]
    };

    this.getModelTotalAmountBySize = function(model, sizeValue) {
        return model.sizesInfo.totalAmountBySize[sizeValue];
    };

    this.getModelTotalAmountByColor = function(model, colorId) {
        return model.totalAmountByColor[colorId];
    };

    this.transformRealStorehouseItemsForRevisionRequest = function () {
        return _.chain(self.realStorehouseItems).map((model) => {
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
    
    this.onInitialize();
}