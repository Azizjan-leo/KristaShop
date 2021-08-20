function getMovementGridRenderer() {
    return {
        renderGrid: function (full) {
            return new MovementGridRenderer({ full: full }).renderGrid()
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
    this.columnClasses = {
        modelColumn: "col-3",
        modelInfoColumn: "col-8",
        colorColumn: "col-3-bigger",
        sizeColumn: "col-1-bigger",
        initialAmount: "col-2",
        incomeAmount: "col-2",
        documentsColumn: "col-9 px-3"
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

    this.renderGrid = function () {
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
                <div class="row mx-0 no-gutters py-2">
                    <div class="${self.columnClasses.modelColumn}">
                        <div class='py-2'>
                            <b>Итого по модели</b>
                        </div>
                    </div>
                    <div class="${self.columnClasses.modelInfoColumn}">
                        <div class='row mx-0 no-gutters py-2'>
                            <div class="${self.columnClasses.colorColumn}"></div>
                            <div class="${self.columnClasses.sizeColumn}"></div>
                            <div class="${self.columnClasses.initialAmount}">
                                <b>${self.full.totalAmount}</b>
                            </div>
                            <div class="${self.columnClasses.incomeAmount}">
                                <b>${self.full.totalSum}</b>
                            </div>
                        </div>
                    </div>
                </div>`
    }

    function renderModelInfo() {
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
                    </div>
                </div>`;
    }

    function renderAmountsRows() {
        let result = "";
        for (let item of self.full.items) {
           
            result += `<div class='row mx-0 no-gutters py-2'>
                            <div class="${self.columnClasses.colorColumn}">
                                ${renderColor(item.colorCode, item.colorName)}
                            </div>
                            <div class="${self.columnClasses.sizeColumn}">
                                ${renderDefaultColumn(item.size.value)}
                            </div>
                            <div class="${self.columnClasses.initialAmount}">
                                ${renderDefaultColumn(item.amount)}
                            </div>
                            <div class="${self.columnClasses.incomeAmount}">
                                ${renderDefaultColumn(item.price)}
                            </div>
                        </div>`;
        }

        return result;
    }

    function renderColor(colorCode, colorName) {
        const background = colorCode; //!color.image ? color.code : `url(${color.image})`;
        return `<div class='row mx-0 no-gutters h-100 align-items-center'>
                   <div class="col-12">
                        <div class="px-2">
                            <span class="color-swatch swatch-md align-middle" data-toggle="tooltip" data-placement="top" title="${colorName}" style="background: ${background};"></span>
                            <span class="ml-1">${colorName}</span>
                        </div>
                   </div>
               </div>`;
    }

    function renderDefaultColumn(value) {
        return `<div class='row mx-0 no-gutters h-100 align-items-center'>
                   <div class="col-12">
                        ${value}
                   </div>
               </div>`;
    }

    function renderDocuments() {
        if (!self.full.documents || self.full.documents.length <= 0) return "";
        return `<div class="row mx-0 no-gutters py-2">
                    <div class="${self.columnClasses.documentsColumn}">
                       ${renderDocumentsHeader()}
                       ${renderDocument()}
                       ${renderDocumentsTotals()}
                    </div>
                </div>`;
    }

   

    initialize();
}