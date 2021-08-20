function getModelGridRenderer() {
    return {
        renderModelGrid: function (full) {
            return new ModelGridRenderer({full: full}).renderModelGrid()
        },
        renderSmallModelGrid: function(full) {
            return new ModelGridRenderer({
                full: full,
                columnClasses:  {
                    modelWrapperColumn: "col-4",
                    dataWrapperColumn: "col-8",
                    colorColumn: "col-4 py-1",
                    sizesColumn: "col-3 py-1",
                    totalAmountByColorColumn: "col-2 py-1",
                    totalSumByColorColumn: "col-3 py-1",
                }
            }).renderModelGrid();
        },
        renderModelGridWithIds: function (full, customIdsPrefix) {
            return new ModelGridRenderer({
                full: full,
                cellsIds: {
                    appendCustomPrefix: true,
                    customPrefix: customIdsPrefix,
                    totalAmountByColorColumnIdPrefix: "totalByColor",
                    totalAmountBySizeColumnIdPrefix: "totalBySize",
                    totalAmountByModel: "totalByModel",
                    colorRowPrefix: "colorRow",
                }
            }).renderModelGrid()
        },
        renderModelAmountsGrid: function (full) {
            return new ModelGridRenderer({
                full: full,
                hideColorTotalSumsColumn: true,
                colorsTotalRowText: "Итого",
                columnClasses: {colorColumn: "col-3 py-1"}
            }).renderModelAmountsGrid()
        },
        renderModelAmountsGridWithEditButtons: function (full) {
            return new ModelGridRenderer({
                full: full,
                hideColorTotalSumsColumn: true,
                addAmountEditButtons: true,
                colorsTotalRowText: "Итого",
                columnClasses: {colorColumn: "col-3", sizesColumn: "col-8", totalAmountByColorColumn: "col-1"},
                cellsIds: {
                    appendCustomPrefix: false,
                    totalAmountByColorColumnIdPrefix: "totalPerColor",
                    totalAmountBySizeColumnIdPrefix: "totalPerSize",
                    totalAmountByModel: "modelRealTotal"
                }
            }).renderModelAmountsGrid()
        }
    };
}

function ModelGridRenderer(options) {
    this.full = {};
    this.colorsTotalRowText = '';
    this.columnClasses =  { 
        modelWrapperColumn: "col-3",
        dataWrapperColumn: "col-9",
        colorColumn: "col-2 py-1",
        sizesColumn: "col-2 py-1",
        totalAmountByColorColumn: "col-3 py-1",
        totalSumByColorColumn: "col py-1",
    }
    
    this.cellsIds = {
        appendCustomPrefix: false,
        customPrefix: "",
        totalAmountByColorColumnIdPrefix: "",
        totalAmountBySizeColumnIdPrefix: "",
        colorRowPrefix: "",
        totalAmountByModel: "",
        
        getIdForTotalAmountByColorColumnId(postfix) {
            return this.totalAmountByColorIdPrefix === '' ? '' : this.appendCustomPrefixToId(`${this.totalAmountByColorColumnIdPrefix}${postfix}`);
        },

        getIdForTotalAmountBySizeColumn(postfix) {
            return this.totalAmountBySizeColumnIdPrefix === '' ? '' : this.appendCustomPrefixToId(`${this.totalAmountBySizeColumnIdPrefix}${postfix}`);
        },
        
        getIdForTotalAmountByModel() {
            return this.appendCustomPrefixToId(this.totalAmountByModel);
        },
        
        getIdForColorRow(postfix) {
            return this.colorRowPrefix === '' ? '' : this.appendCustomPrefixToId(`${this.colorRowPrefix}${postfix}`);
        },
        
        appendCustomPrefixToId(value) {
            return this.appendCustomPrefix ?`${this.customPrefix}_${value}` : value;
        }
    }

    this._constructor = function (options) {
        let defaultOptions = {
            full: {},
            hideColorTotalSumsColumn: false,
            addAmountEditButtons: false,
            colorTotalSumClasses: { },
            colorsTotalRowText: "",
            cellsIds: {}
        }

        let resultOptions = Object.assign(defaultOptions, options);
        this.full = resultOptions.full;
        this.colorsTotalRowText = resultOptions.colorsTotalRowText;
        this.addAmountEditButtons = resultOptions.addAmountEditButtons;
        
        this.columnClasses = Object.assign(this.columnClasses, resultOptions.columnClasses);
        this.cellsIds = Object.assign(this.cellsIds, resultOptions.cellsIds);
        if(resultOptions.hideColorTotalSumsColumn) {
            this.columnClasses.totalSumByColorColumn += " d-none";
        }
    };
    this._constructor(options);
    const self = this;

    this.renderModelGrid = function() {
        return `<div class="row mx-0 no-gutters py-2">
                <div class="${self.columnClasses.modelWrapperColumn}">
                    <div class="row mx-0 no-gutters">
                        ${self._renderModelInfo()}
                    </div>
                </div>
                <div class="${self.columnClasses.dataWrapperColumn}">
                    <div class="row mx-0 no-gutters">
                        <div class="${self.columnClasses.colorColumn}"></div>
                        <div class="${self.columnClasses.sizesColumn}">${self._renderSizesHeaderRow(self.full.sizesInfo.values)}</div>
                        <div class="${self.columnClasses.totalAmountByColorColumn}"></div>
                        <div class="${self.columnClasses.totalSumByColorColumn}"></div>
                    </div>
                    ${self._renderAmountsRows()}
                </div>
           </div>
           ${self._renderModelGridTotalsRow()}`
    }

    this.renderModelAmountsGrid = function () {
        return `<div class='row mx-0 no-gutters py-2 main-font h7 text-main-grey-lighter text-uppercase'>
                    <div class="${self.columnClasses.colorColumn}">
                        <div class="px-2"><span class="text-bold">Цвета</span></div>
                    </div>
                    <div class="${self.columnClasses.sizesColumn}">
                        ${self._renderSizesHeaderRow(self.full.sizesInfo.values)}
                    </div>
                    <div class="${self.columnClasses.totalAmountByColorColumn}">
                        <div class="pl-2"><span class="text-bold">Итого ед</span></div>
                    </div>
                </div>
                ${self._renderAmountsRows()}
                ${self._renderTotalsRow()}`
    }

    this._renderModelInfo = function () {
        console.log(self.full.modelInfo);
        const mainPhoto = String.isNullOrEmpty(self.full.modelInfo.mainPhoto) ? "/common/img/noimage.png?width=80" : `${self.full.modelInfo.mainPhoto}?width=80`;
        return `<div class="row m-0 px-2">
                    <div class="col-auto px-2">
                        <a href="${self.full.modelInfo.mainPhoto}" data-lightbox="Изображение" data-title="${self.full.modelInfo.articul}">
                            <picture  class="img-fluid rounded-1">
                                <source srcset="${mainPhoto}&format=webp" type="image/webp">
                                <img srcset="${mainPhoto}" class="img-fluid rounded-1" alt="${self.full.modelInfo.articul}" />
                            </picture>
                        </a>
                    </div>
                    <div class="col px-2">
                        <div class="mb-3">Арт: ${self.full.modelInfo.name}</div>
                        <div><span class="size-swatch swatch-light">${self.full.modelInfo.size.line}</span></div>
                        <div class="mt-2"><span>${Number.toTwoDecimalPlaces(self.full.modelInfo.price)} $ за ед</span></div>
                    </div>
                </div>`;
    }

    this._renderAmountsRows = function () {
        var result = "";
        for (var color of self.full.colors) {
            result += `<div id="${self.cellsIds.getIdForColorRow(color.id)}" class='row mx-0 no-gutters py-2'>
                            <div class="${self.columnClasses.colorColumn}">
                                ${self._renderColorRow(color)}
                            </div>
                            <div class="${self.columnClasses.sizesColumn}">
                                ${self._renderSizesRow(self.full.sizesInfo, color.id)}
                            </div>
                            <div class="${self.columnClasses.totalAmountByColorColumn}">
                                ${self._renderColorTotalAmountRow(color.id, self.full.totalAmountByColor[color.id])}
                            </div>
                            <div class="${self.columnClasses.totalSumByColorColumn}">
                                ${self._renderColorTotalSumRow(self.full.totalSumByColor[color.id])}
                            </div>
                        </div>`;
        }
        return result;
    }

    this._renderModelGridTotalsRow = function() {
        return `<div class='row mx-0 no-gutters py-2 text-bold'>
                    <div class="${self.columnClasses.modelWrapperColumn} text-bold py-2 align-self-center"><span class="px-3">Итого по модели:</span></div>
                    <div class="${self.columnClasses.dataWrapperColumn}">
                        ${self._renderTotalsRow()}
                    </div>
               </div>`
    }

    this._renderTotalsRow = function () {
        return `<div class='row mx-0 no-gutters py-2 text-bold'>
                    <div class="${self.columnClasses.colorColumn}">
                        <div class="px-2">${self.colorsTotalRowText}</div>
                    </div>
                    <div class="${self.columnClasses.sizesColumn}">
                        ${self._renderSizesTotalsRow(self.full.sizesInfo)}
                    </div>
                    <div class="${self.columnClasses.totalAmountByColorColumn}">
                        ${self._renderModelTotalAmountRow(self.full.totalAmount)}
                    </div>
                    <div class="${self.columnClasses.totalSumByColorColumn}">
                        ${self._renderModelTotalSumRow(self.full.totalSum)}
                    </div>
                </div>`
    }

    this._renderSizesHeaderRow = function(sizeValues) {
        var result = `<div class="row mx-0 no-gutters main-font h7 text-main-grey-lighter ${self.addAmountEditButtons ? "text-center" : ""}">`;
        for (var sizeValue of sizeValues) {
            result += `<div class="col-3"><div class="px-2 text-bold">${sizeValue}</div></div>`;
        }
        return result += `</div>`
    }

    this._renderSizesRow = function (sizesInfo, colorId) {
        if (self.addAmountEditButtons) {
            return self._renderSizesRowWithButtons(sizesInfo, colorId);
        }
        return self._renderSizesRowDefault(sizesInfo, colorId);
    }

    this._renderSizesRowDefault = function (sizesInfo, colorId) {
        var result = `<div class='row mx-0 no-gutters flex-nowrap h-100 align-items-center'>`;
        for (var sizeValue of sizesInfo.values) {
            const amount = sizesInfo.sizeColorAmount[`${sizeValue}_${colorId}`];
                result += `<div class="col-3"><div class="px-2 text-nowrap">${amount ? amount : 0}</div></div>`;
        }
        return result += `</div>`
    }

    this._renderSizesRowWithButtons = function (sizesInfo, colorId) {
        var result = `<div class='row mx-0 no-gutters py-1'>`;
        const modelKey = self.full.modelKey;
        for (var sizeValue of sizesInfo.values) {
            const amount = sizesInfo.sizeColorAmount[`${sizeValue}_${colorId}`];
            result += `<div class="col-3 text-center">
                            <div class="row mx-1 px-2 no-gutters justify-content-between align-items-center">
                                <button class="btn btn-quantity-revision btn-quantity-minus" type="button" data-model-amount-change="${JSON.toFunctionParameter({modelKey, colorId, sizeValue, amount: -1})}">
                                    <span class="btn-text">-</span>
                                </button>
                                <span class="itemAmount">${amount ? amount : 0}</span>
                                <button class="btn btn-quantity-revision btn-quantity-plus" type="button" data-model-amount-change="${JSON.toFunctionParameter({modelKey, colorId, sizeValue, amount: +1 })}">
                                    <span class="btn-text">+</span>
                                </button>
                            </div>
                       </div>`;
        }
        return result += `</div>`
    }

    this._renderSizesTotalsRow = function (sizesInfo) {
        var result = `<div class='row mx-0 no-gutters ${self.addAmountEditButtons ? "text-center" : ""}'>`;
        for (var sizeValue of sizesInfo.values) {
            const amount = sizesInfo.totalAmountBySize[sizeValue];
            result += `<div class="col-3"><div class="px-2"><span id="${self.cellsIds.getIdForTotalAmountBySizeColumn(sizeValue)}">${amount ? amount : 0}</span></div></div>`;
        }
        return result += `</div>`
    }

    this._renderColorRow = function (color) {
        const background = !color.image ? color.code : `url(${color.image})`;
        return `<div class='row mx-0 no-gutters h-100 align-items-center'>
                   <div class="col-12">
                        <div class="px-2">
                            <span class="color-swatch swatch-md align-middle" data-toggle="tooltip" data-placement="top" title="${color.name}" style="background: ${background};"></span>
                            <span class="ml-1">${color.name}</span>
                        </div>
                   </div>
               </div>`;
    }

    this._renderColorTotalAmountRow = function (colorId, amount) {
        return `<div class='row mx-0 no-gutters h-100 align-items-center'><div class="col-12"><div class="px-2"><span id="${self.cellsIds.getIdForTotalAmountByColorColumnId(colorId)}">${amount}</span></div></div></div>`;
    }

    this._renderColorTotalSumRow = function (sum) {
        return `<div class='row mx-0 no-gutters h-100 align-items-center'><div class="col-12"><div class="px-2"><span class="text-nowrap">${sum}$</span></div></div></div>`;
    }

    this._renderModelTotalAmountRow = function (amount) {
        return `<div class='row mx-0 no-gutters'><div class="col-12"><div class="px-2"><span id="${self.cellsIds.getIdForTotalAmountByModel()}">${amount}</span></div></div></div>`;
    }

    this._renderModelTotalSumRow = function (sum) {
        return `<div class='row mx-0 no-gutters'><div class="col-12"><div class="px-2"><span class="text-nowrap">${sum}$</span></div></div></div>`;
    }
}