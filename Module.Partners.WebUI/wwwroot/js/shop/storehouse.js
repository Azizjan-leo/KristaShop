var storehouseItems;
var table;
$(function () {
    storehouseItems = dataModel;
    function addColumnsForFilterToDataSource() {
        for(var item of storehouseItems) {
            const sizes = _.chain(item.sizesInfo.totalAmountBySize).toPairs().filter(x => x[1] > 0).map(x => x[0]).value();
            sizes.push(item.modelInfo.size.line);

            item.sizesString = sizes.join(" ");
            item.colorsString = _.map(item.colors, x => x.name).join(" ");
        }
    }
    addColumnsForFilterToDataSource();

    const modalView = new SellModalView(storehouseItems);
    table = $(".storehouseItemsTable").DataTable({
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
                "width": "12%",
                "render": function (data, type, full, meta) {
                    return `<div><a class="btn btn-basic btn-success btn-shop dynamic-height" data-sell-model="${data}">Продажа</a></div>`;
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
        table: table,
        sortByColumn: {name: "modelKey", direction: "asc"}
    });

    const filter = new DataTableCustomFilter([table], {
        applyByButton: false,
        blockWrapperSelector: ".customSearchWrapper"
    });
});

function SellModalView(storehouseItems) {
    this.storehouseItems = storehouseItems;
    this.currentSellModel = {};
    this.sellingWindow = $("#sellStorehouseItemModal");
    this.sellWindowArticulWrapper = $("#sellModalArticul");
    this.sellingFormWrapper = $("#sellingFormWrapper");
    this.selectedModelInfoWrapper = $("#selectedModelInfo");
    this.sellConfirmButton = $("[data-selling-confirm]");
    var self = this;
    
    this.onInitialize = function () { 
        addEventHandlers();
    }
    
    function addEventHandlers() { 
        $(document).on("click", "[data-sell-model]", openSellingWindow)
        self.sellConfirmButton.on("click", confirmSelling);
    }

    function openSellingWindow(e) { 
        e.preventDefault();

        self.currentSellModel = findStorehouseItemByModelKey(this.dataset.sellModel);
        if(self.currentSellModel === undefined) return;

        self.sellWindowArticulWrapper.html(self.currentSellModel.modelInfo.name);
        drawSellingForm();
        self.sellingWindow.modal("show");
    }
    
    function confirmSelling() {
        const model = getSelectedModel();
        if(model === undefined) { 
            return;
        }
        
        self.sellingWindow.modal("hide");
        $.ajax({
            url: "/Partners/Shop/SellModelItem?includeHistoryItems=false",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                modelId: model.modelId,
                colorId: model.colorId,
                sizeValue: model.sizeValue,
            })
        }).done(function (responseText) {
            showNotificationSuccess(`Модель ${model.articul} ${model.colorName} ${model.sizeValue} продана в кол-ве ${model.amount}`);
            updateModelAmounts(model);
            redrawTableWithNewData(table, self.storehouseItems);
        }).fail(function (jqXHR) {
            Swal.fire({
                icon: "error",
                title: 'Ошибка продажи',
                text: `Не удалось выполнить продажу модели ${model.articul} ${model.colorName} ${model.sizeValue} в кол-ве ${model.amount}`,
                showCancelButton: false,
                showConfirmButton: true,
                confirmButtonText: "Продолжить",
                buttonsStyling: false,
                customClass: {
                    container: "mt-0",
                    popup: "py-4 px-3 px-lg-5",
                    title: "header-font h4-smaller text-uppercase text-bold mb-3",
                    content: "main-font h2 px-0 px-lg-1 pb-3 mx-0 mb-0 mb-lg-4",
                    confirmButton: "btn-basic btn-basic-big btn-success m-1",
                    actions: "my-3 pb-1"
                }
            }).then((result) => {
                location.reload();
            });
        });
    }
    
    function updateModelAmounts(model) {
        self.currentSellModel.sizesInfo.sizeColorAmount[`${model.sizeValue}_${model.colorId}`] -= model.amount;
        self.currentSellModel.sizesInfo.totalAmountBySize[model.sizeValue] -= model.amount;
        self.currentSellModel.totalAmountByColor[model.colorId] -= model.amount;
        self.currentSellModel.totalSumByColor[model.colorId] -= model.amount * self.currentSellModel.modelInfo.price;
        self.currentSellModel.totalAmount -= model.amount;
        self.currentSellModel.totalSum -= model.amount * self.currentSellModel.modelInfo.price;
    }
    
    function drawSellingForm() {
        removeRadioButtonsClickHandlers();
        self.sellingFormWrapper.empty();
        
        self.sellingFormWrapper.html(drawSellingFormTable());
        addRadioButtonsClickHandlers();
    }
    
    function drawSellingFormTable() { 
        return `<div class="row no-gutters">
                    <div class="col-7">
                        ${drawHeaderRow()}
                        ${drawDataRows()}
                    </div>
                </div>`;
    }
    
    function drawHeaderRow() {
        return `<div class="row mx-0 my-3 py-1 no-gutters main-font h7 text-main-grey-lighter text-uppercase">
                    <div class="col-4 px-1 text-bold">Укажите цвет</div>
                    <div class="col-8 px-1 text-bold">Укажите размер</div>
                </div>`;
    }
    
    function drawDataRows() { 
        var result = "";
        for(var color of self.currentSellModel.colors) {
            result += drawDataRow(color);
        }
        
        return result;
    }
    
    function drawDataRow(color) { 
        return `<div class="row mx-0 py-1 no-gutters">
                    <div class="col-4">${drawColorCell(color)}</div>
                    <div class="col-8">${drawSizesRow(color.id)}</div>
                </div>`;
    }
    
    function drawColorCell(color) {
        const background = !color.image ? color.code : `url(${color.image})`;
        return `<div class="px-1">
                    <div class="radio-rectangle-button" data-color-item="${color.id}">
                        <input class="form-check-input" type="radio" name="color-options" id="color-option-${color.id}" value="${color.name}">
                        <label class="form-check-label d-block" for="color-option-${color.id}">
                            <span class="color-swatch m-0 swatch-md align-middle" data-toggle="tooltip" data-placement="top" title="${color.name}" style="background: ${background};"></span>
                            <span class="ml-1 align-middle">${color.name}</span>
                        </label>
                    </div>
                </div>`
    }
    
    function drawSizesRow(colorId) {
        var result = `<div class='row mx-0 no-gutters flex-nowrap h-100 align-items-center' data-sizes-group="${colorId}">`;
        for (var sizeValue of self.currentSellModel.sizesInfo.values) {
            let amount = self.currentSellModel.sizesInfo.sizeColorAmount[`${sizeValue}_${colorId}`];
            if(amount === undefined) { 
                amount = 0;
            }
            result += `<div class="col-3">
                            <div class="px-1 text-center">
                                <div class="radio-circle-button">
                                    <input class="form-check-input" type="radio" name="color-size-options-${colorId}" id="color-size-option-${colorId}-${sizeValue}" ${amount <= 0 ? "disabled" : ""} value="${sizeValue}">
                                    <label class="form-check-label" for="color-size-option-${colorId}-${sizeValue}">
                                        ${sizeValue}
                                    </label>
                                </div>
                            </div>
                       </div>`;
        }
        return result += `</div>`
    }
    
    function addRadioButtonsClickHandlers() {
        self.sellingFormWrapper.find("[data-sizes-group]").on("click", sizesGroupClicked)
        self.sellingFormWrapper.find("[data-color-item]").on("click", colorClicked);
    }
    
    function removeRadioButtonsClickHandlers() {
        self.sellingFormWrapper.find("[data-sizes-group]").off("click", sizesGroupClicked);
        self.sellingFormWrapper.find("[data-color-item]").off("click", colorClicked);
    }
    
    function sizesGroupClicked(e) {
        self.sellingFormWrapper.find(`#color-option-${this.dataset.sizesGroup}`).prop("checked", true);
        self.sellingFormWrapper.find(`[data-sizes-group]:not([data-sizes-group=${this.dataset.sizesGroup}]) input`).prop("checked", false);
        
        var target = e.target || e.srcElement || e.originalTarget;
        if(!$(target).is('label') && !$(target).is('input')) {
            self.sellingFormWrapper.find(`[data-sizes-group=${this.dataset.sizesGroup}] input`).first().prop("checked", true);
        }
        setSellingInfoText();
    }

    function colorClicked(e) {
        self.sellingFormWrapper.find(`[data-sizes-group]:not([data-sizes-group=${this.dataset.colorItem}]) input`).prop("checked", false);
        self.sellingFormWrapper.find(`[data-sizes-group=${this.dataset.colorItem}] input`).first().prop("checked", true);
        setSellingInfoText();
    }
    
    function setSellingInfoText() {
        const model = getSelectedModel();
        if(model === undefined){
            self.selectedModelInfoWrapper.hide();
            return;
        }
        
        self.selectedModelInfoWrapper.show();
        self.selectedModelInfoWrapper.html(`Итого: Арт ${model.articul} / Цвет ${model.colorName} / Размер ${model.sizeValue} / Кол-во ${model.amount}`);
    }
    
    function getSelectedModel() {
        const colorInput = $("[name=color-options]:checked");
        const colorId = colorInput.parent().data("colorItem");
        const sizeInput = $(`[name=color-size-options-${colorId}]:checked`);
        if (colorInput.length <= 0 || sizeInput.length <= 0) {
            return undefined;
        }

        return {
            modelId: self.currentSellModel.modelInfo.modelId,
            articul: self.currentSellModel.modelInfo.articul,
            colorId: colorId,
            colorName: colorInput.val(),
            sizeValue: sizeInput.val(),
            amount: 1
        }
    }
    
    function findStorehouseItemByModelKey(modelKey) {
        return _.find(storehouseItems, x => x.modelKey === modelKey);
    }
    
    this.onInitialize();
}