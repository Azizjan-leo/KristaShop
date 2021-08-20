function getMovementGridRenderer() {
    return {
        renderGrid: function (full) {
            return new MovementGridRenderer({full: full}).renderGrid()
        },
    };
}

function MovementGridRenderer(options) {
    this.full = {
        modelInfo: {},
        items: [],
        documents: []
    };
    this.colorsTotalRowText = '';
    this.columnClasses =  {
        modelColumn: "col-4",
        modelInfoColumn: "col-8",
        colorColumn: "col-3-bigger",
        sizeColumn: "col-1-bigger",
        initialAmount: "col-2",
        incomeAmount: "col-1-bigger",
        writeOffAmount: "col-1-bigger",
        currentAmount: "col-2",
        documentsColumn: "col-9 px-3",
        documentDateColumn: "col-1-bigger",
        documentNameColumn: "col-2",
        documentInitialAmountColumn: "col-2",
        documentIncomeAmountColumn: "col-1-bigger",
        documentWriteOffAmountColumn: "col-1-bigger",
        documentCurrentAmountColumn: "col-2",
    }
    const self = this;
    
    function initialize() {
        let defaultOptions = {
            full: {},
            columnClasses: {}
        }
        let resultOptions = Object.assign(defaultOptions, options);
        self.full = resultOptions.full;
        self.columnClasses = Object.assign(self.columnClasses, resultOptions.columnClasses);
    }

    this.renderGrid = function() {
        return `<div class="row mx-0 no-gutters py-2">
                    <div class="${self.columnClasses.modelColumn}">
                        <div class="row mx-0 no-gutters">
                            ${renderModelInfo()}
                        </div>
                    </div>
                    <div class="${self.columnClasses.modelInfoColumn}">
                        ${renderAmountsRows()}
                    </div>
                </div>
                ${renderDocuments()}`
    }
    
    function renderModelInfo () {
        const mainPhoto = String.isNullOrEmpty(self.full.modelInfo.mainPhoto) ? "/common/img/noimage.png?width=80" : `${self.full.modelInfo.mainPhoto}?width=80`;
        return `<div class="row m-0 px-2">
                    <div class="col-auto px-2">
                        <a href="${self.full.modelInfo.mainPhoto}" data-lightbox="Изображение" data-title="${self.full.modelInfo.articul}">
                            <picture class="img-fluid rounded-1">
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
    
    function renderAmountsRows() {
        let result = "";
        for(let item of self.full.items) {
            result += `<div class='row mx-0 no-gutters py-2'>
                            <div class="${self.columnClasses.colorColumn}">
                                ${renderColor(item.color)}
                            </div>
                            <div class="${self.columnClasses.sizeColumn}">
                                ${renderDefaultColumn(item.size.value)}
                            </div>
                            <div class="${self.columnClasses.initialAmount}">
                                ${renderDefaultColumn(item.initialAmount)}
                            </div>
                            <div class="${self.columnClasses.incomeAmount}">
                                ${renderDefaultColumn(item.incomeAmount)}
                            </div>
                            <div class="${self.columnClasses.writeOffAmount}">
                                ${renderDefaultColumn(item.writeOffAmount)}
                            </div>
                            <div class="${self.columnClasses.currentAmount}">
                                ${renderDefaultColumn(item.currentAmount)}
                            </div>
                        </div>`;
        }
        
        return result;
    }

    function renderColor (color) {
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

    function renderDefaultColumn (value) {
        return `<div class='row mx-0 no-gutters h-100 align-items-center'>
                   <div class="col-12">
                        ${value}
                   </div>
               </div>`;
    }
    
    function renderDocuments() {
        if(!self.full.documents || self.full.documents.length <= 0) return "";
        return `<div class="row mx-0 no-gutters py-2">
                    <div class="${self.columnClasses.documentsColumn}">
                       ${renderDocumentsHeader()}
                       ${renderDocument()}
                       ${renderDocumentsTotals()}
                    </div>
                </div>`;
    }
    
    function renderDocumentsHeader() {
        return `<div class="row mx-0 no-gutters py-2 main-font h7 text-main-grey-lighter text-uppercase">
                    <div class="${self.columnClasses.documentDateColumn} text-bold">Дата</div>
                    <div class="${self.columnClasses.documentNameColumn} text-bold">Документ</div>
                    <div class="${self.columnClasses.documentInitialAmountColumn} text-bold">Нач. остаток</div>
                    <div class="${self.columnClasses.documentIncomeAmountColumn} text-bold">Приход</div>
                    <div class="${self.columnClasses.documentWriteOffAmountColumn} text-bold">Расход</div>
                    <div class="${self.columnClasses.documentCurrentAmountColumn} text-bold">Кон. остаток</div>
                </div>`;
    }
    
    function renderDocument() {
        let result = "";
        for (var document of self.full.documents) { 
            result +=  `<div class="row mx-0 no-gutters py-2">
                            <div class="${self.columnClasses.documentDateColumn}">${dayjs(document.createDate).format("DD.MM.YYYY")}</div>
                            <div class="${self.columnClasses.documentNameColumn}"><a href="/Partners/Documents/Document/${document.number}" class="link-base link-dark">${document.name}</a></div>
                            <div class="${self.columnClasses.documentInitialAmountColumn}">${document.initialAmount}</div>
                            <div class="${self.columnClasses.documentIncomeAmountColumn}">${document.incomeAmount}</div>
                            <div class="${self.columnClasses.documentWriteOffAmountColumn}">${document.writeOffAmount}</div>
                            <div class="${self.columnClasses.documentCurrentAmountColumn}">${document.currentAmount}</div>
                        </div>`;
        }
        
        return result;
    }

    function renderDocumentsTotals() {
        const totals = self.full.items[0];
        if(!totals) return "";
        
        return `<div class="row mx-0 no-gutters pb-2 pt-3 text-bold">
                    <div class="${self.columnClasses.documentDateColumn} text-bold">Итого</div>
                    <div class="${self.columnClasses.documentNameColumn} text-bold"></div>
                    <div class="${self.columnClasses.documentInitialAmountColumn} text-bold">${totals.initialAmount}</div>
                    <div class="${self.columnClasses.documentIncomeAmountColumn} text-bold">${totals.incomeAmount}</div>
                    <div class="${self.columnClasses.documentWriteOffAmountColumn} text-bold">${totals.writeOffAmount}</div>
                    <div class="${self.columnClasses.documentCurrentAmountColumn} text-bold">${totals.currentAmount}</div>
                </div>`;
    }
    
    initialize();
}