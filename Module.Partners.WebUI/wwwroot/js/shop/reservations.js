const selectorDateFormat = "DDMMYYYY";
var reservationHandler = {};

$(function () {
    reservationHandler = new ReservationView(dataModel);
    for (var reservationGroup of reservationHandler.reservations.reservationsGroups) {
        const reservationDate = dayjs(reservationGroup.createDate).format(selectorDateFormat);
        const reservationTable = $(`.shipmentItemsTable_${reservationDate}`).DataTable({
            "data": reservationGroup.items,
            "responsive": true,
            "paging": false,
            "columns":
                [
                    {
                        "name": "modelKey",
                        "data": "modelKey",
                        "sortable": false,
                        "render": function (data, type, full, meta) {
                            if (type !== "display") {
                                return full.modelInfo.name;
                            }

                            return getModelGridRenderer().renderModelGridWithIds(full, `${reservationDate}_${full.modelKey}`);
                        }
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
                    }
                ],
            "language": {
                "url": "/common/datatables.Russian.json"
            },
            "dom": "r"
        });

        initDataTablesCustomSorting({
            table: reservationTable,
            sortByColumn: {name: "modelKey", direction: "asc"}
        });
    }
});

function ReservationView(reservationsGroups) {
    this.reservations = new Reservations(reservationsGroups);
    this.incomeReservation = new IncomeReservation();
    this.dateFormat = "DD.MM.YYYY";
    this.barcodeInput = {};
    this.modelInfoInput = {};
    this.modelFound = false;
    this.barcodeHandler = undefined;
    const ENTER_KEY_CODE = 13;
    const ALLOWED_BARCODE_LENGTH = 16;
    var self = this;

    this.addEventHandlers = function () {
        $(document).on("click", "[data-auto-income-reservation]", self.startAutomatedIncomeEventHandler);
        $(document).on("click", "[data-income-reservation]", self.startIncomeEventHandler);
        $(document).on("click", "[data-accept-model]", self.acceptModelIncomeEventHandler);
    };
    
    this.startAutomatedIncomeEventHandler = function (event) {
        event.stopPropagation();
        
        if (!self.incomeReservation.isStarted) {
            const reservationDate = this.dataset.autoIncomeReservation;
            var reservationGroup = self.reservations.getReservationGroupByDate(reservationDate);
            Swal.fire({
                title: 'Вы уверены, что хотите принять отправку полностью?',
                text: `Отправка от ${reservationDate} должна содержать ${reservationGroup.totals.totalAmount} единиц на сумму ${currencyFormatConvert(reservationGroup.totals.totalSum)}$`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Да, принять отправку',
                cancelButtonText: 'Отмена',
                buttonsStyling: false,
                customClass: {
                    container: "mt-0",
                    popup: "py-4 px-3 px-lg-5",
                    title: "header-font h4-smaller text-uppercase text-bold mb-3",
                    content: "main-font h2 px-0 px-lg-1 pb-3 mx-0 mb-0 mb-lg-4",
                    cancelButton: "btn-basic btn-basic-big btn-danger m-1",
                    confirmButton: "btn-basic btn-basic-big btn-success m-1",
                    actions: "my-3 pb-1"
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    self.sendAutoIncomeReservationRequest(reservationDate);
                }
            });
        } else {
            const actual = self.incomeReservation.getTotalAmountIncomed();
            const expected = self.incomeReservation.getTotalAmountToIncome();
            let text = "";
            if (expected > actual) {
                text = `Осталось единиц ${expected - actual} из ${expected}.`;
            }

            Swal.fire({
                icon: "warning",
                title: 'Завершение приходования',
                text: `Вы уверены, что хотие завершить оприходование? ${text}`,
                showCancelButton: true,
                cancelButtonText: "Нет, продолжить",
                showConfirmButton: true,
                confirmButtonText: "Завершить",
                buttonsStyling: false,
                customClass: {
                    container: "mt-0",
                    popup: "py-4 px-3 px-lg-5",
                    title: "header-font h4-smaller text-uppercase text-bold mb-3",
                    content: "main-font h2 px-0 px-lg-1 pb-3 mx-0 mb-0 mb-lg-4",
                    cancelButton: "btn-basic btn-basic-big btn-success m-1",
                    confirmButton: "btn-basic btn-basic-big btn-danger m-1",
                    actions: "my-3 pb-1"
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    self.sendReservationRequest();
                }
            });

        }
    }
    
    this.startIncomeEventHandler = function(event) {
        event.stopPropagation();

        const reservationDate = this.dataset.incomeReservation;
        self.incomeReservation = new IncomeReservation(self.reservations.getReservationGroupByDate(reservationDate));
        self.incomeReservation.start();
        
        self.showStartRevisionButtons();
        
        if(self.barcodeHandler !== undefined) { 
            self.barcodeHandler.destroy();
        }
        self.barcodeHandler = new BarcodeInputHandler({
            barcodeInputSelector: `#barcodeInput_${self.incomeReservation.reservationDateJointString}`,
            barcodeInfoInputSelector: `#modelInfo_${self.incomeReservation.reservationDateJointString}`,
            onBarcodeInput: self.onBarcodeChanged,
            onEnterPressed: self.acceptModelIncomeEventHandler
        })
        self.barcodeInput = $(`#barcodeInput_${self.incomeReservation.reservationDateJointString}`);
        self.changeTextAndBackground();
    }

    this.showStartRevisionButtons = function () {
        $(`#startIncomeBtn_${self.incomeReservation.reservationDateJointString}`).hide();
        $(".reservationItemInMainList").hide();
        $(`#${self.incomeReservation.reservationDateJointString}`).show();
        $(`#hiddenInputsBlock_${self.incomeReservation.reservationDateJointString}`).removeClass("d-none");
        $(`#collapseShipment_${self.incomeReservation.reservationDateJointString}`).collapse('show');
        $("html, body").animate({ scrollTop: 0 }, "slow");
    };

    this.changeTextAndBackground = function () {
        $(`#autoIncomeBtn_${self.incomeReservation.reservationDateJointString}`).text(`Завершть приход ${self.incomeReservation.getTotalAmountIncomed()}/${self.incomeReservation.getTotalAmountToIncome()}`);

        for(var model of self.incomeReservation.actualReservationItems) {
            for(var color of model.colors) {
                const row = $(`[id=${self.incomeReservation.reservationDateJointString}_${model.modelKey}_colorRow${color.id}]`);
                row.children("div:gt(0)").addClass("bg-red text-white");
                const colorAmount = row.find(`[id=${self.incomeReservation.reservationDateJointString}_${model.modelKey}_totalByColor${color.id}]`);
                colorAmount.html(`0 из ${colorAmount.html()}`);
            }
        }
    }

    this.onBarcodeChanged = function (barcode) {
        return new Promise((resolve, reject) => {
            const foundBarcode = self.incomeReservation.findBarcode(barcode);
            if (foundBarcode !== undefined) {
                self.modelFound = true;
                resolve(foundBarcode.name);
            } else {
                reject();
                self.modelFound = false;
            }
        });
    }

    this.acceptModelIncomeEventHandler = function() {
        if (self.modelFound) {
            const barcode = self.barcodeInput.val();
            if(!self.incomeReservation.increaseModelAmount(barcode)) return;
            
            var row = $(`[id=${self.incomeReservation.reservationDateJointString}_${self.incomeReservation.lastFoundBarcode.model.modelKey}_colorRow${self.incomeReservation.lastFoundBarcode.barcode.colorId}]`);
            const totalAmounts = self.incomeReservation.getColorTotalAmounts(barcode); 
            if(totalAmounts.isFullyIncomed) {
                const cells = row.children(".bg-red");
                cells.removeClass("bg-red");
                cells.addClass("bg-balance");
            }
            var colorTotalsWrapper = row.find(`[id=${self.incomeReservation.reservationDateJointString}_${self.incomeReservation.lastFoundBarcode.model.modelKey}_totalByColor${self.incomeReservation.lastFoundBarcode.barcode.colorId}]`);
            colorTotalsWrapper.html(`${totalAmounts.actualAmount} из ${totalAmounts.expectedAmount}`);
            
            $(`#autoIncomeBtn_${self.incomeReservation.reservationDateJointString}`).text(`Завершть приход ${self.incomeReservation.getTotalAmountIncomed()}/${self.incomeReservation.getTotalAmountToIncome()}`);
        }
    }

    this.sendAutoIncomeReservationRequest = function (reservationDate) {
        $.ajax({
            type: "POST",
            url: `/Partners/Shop/IncomeShipment`,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(reservationDate),
            success: self.incomeSuccessfullyFinishedAlert,
            error: self.incomeFinishedWithErrorAlert
        });
    };

    this.sendReservationRequest = function () {
        $.ajax({
            type: "POST",
            url: "/Partners/Shop/IncomeShipmentItems",
            data: self.incomeReservation.getResult(),
            success: self.incomeSuccessfullyFinishedAlert,
            error: self.incomeFinishedWithErrorAlert
        });
    };

    this.incomeFinishedWithErrorAlert = function () {
        Swal.fire(
            'Ошибка!',
            'Оприходование не завершено',
            'error'
        ).then((result) => {
            if (result.isConfirmed) {
                location.reload(true);
            }
        });
    };

    this.incomeSuccessfullyFinishedAlert = function () {
        Swal.fire(
            'Отлично!',
            'Оприходование завершено успешно',
            'success'
        ).then((result) => {
            if (result.isConfirmed) {
                location.reload();
            }
        });
    };

    this.addEventHandlers();
}

function IncomeReservation(reservationGroup) {
    this.reservationGroup = reservationGroup;
    this.expectedReservationItems = [];
    this.actualReservationItems = [];
    this.reservationDateJointString = "";
    this.isStarted = false;
    this.dateFormat = "DD.MM.YYYY";
    this.lastFoundBarcode = {
        name: "",
        model: undefined,
        barcode: {
            barcode: "",
            colorId: -1
        }
    };
    var self = this;
    
    this.start = function () { 
        self.isStarted = true;
        self.expectedReservationItems = self.reservationGroup.items;
        self.actualReservationItems = Object.deepCopy(self.reservationGroup.items);

        for (var item of self.actualReservationItems) {
            for(var sizeColorAmount in item.sizesInfo.sizeColorAmount) {
                if(item.sizesInfo.sizeColorAmount.hasOwnProperty(sizeColorAmount)) {
                    item.sizesInfo.sizeColorAmount[sizeColorAmount] = 0;
                }
            }

            for(var totalSizeAmount in item.sizesInfo.totalAmountBySize) {
                if(item.sizesInfo.totalAmountBySize.hasOwnProperty(totalSizeAmount)) {
                    item.sizesInfo.totalAmountBySize[totalSizeAmount] = 0;
                }
            }
            
            for(var totalColorAmount in item.totalAmountByColor) { 
                if(item.totalAmountByColor.hasOwnProperty(totalColorAmount)) {
                    item.totalAmountByColor[totalColorAmount] = 0;
                }
            }

            for(var totalColorSum in item.totalSumByColor) {
                if(item.totalSumByColor.hasOwnProperty(totalColorSum)) {
                    item.totalSumByColor[totalColorSum] = 0;
                }
            }

            item.totalAmount = 0;
            item.totalSum = 0;
        }
        
        self.reservationDateJointString = dayjs(self.reservationGroup.createDate).format(selectorDateFormat);
    };
    
    this.getTotalAmountToIncome = function () { 
        return _.sumBy(self.expectedReservationItems, x => x.totalAmount );
    };

    this.getTotalAmountIncomed = function () {
        return _.chain(self.actualReservationItems).sumBy(x => x.totalAmount).value();
    }
    
    this.findBarcode = function (barcode) {
        if(self.lastFoundBarcode.barcode.barcode === barcode) { 
            return self.lastFoundBarcode;
        }
        
        for(var item of self.actualReservationItems) { 
            const found = _.find(item.barcodes, x=>x.barcode === barcode);
            if(found !== undefined) { 
                const color = _.find(item.colors, x=>x.id === found.colorId);
                self.lastFoundBarcode = {
                    name: `Арт: ${item.modelInfo.articul} ${color.name} ${item.modelInfo.size.value}`,
                    model: item,
                    barcode: found,
                }
                return self.lastFoundBarcode;
            }
        }
        return undefined;
    }
    
    this.increaseModelAmount = function(barcode) {
        const foundBarcode = self.findBarcode(barcode);
        if(foundBarcode === undefined) return false;

        foundBarcode.model.totalAmountByColor[foundBarcode.barcode.colorId] += foundBarcode.model.modelInfo.size.parts;
        foundBarcode.model.totalAmount += foundBarcode.model.modelInfo.size.parts;
        return true;
    }
    
    this.getColorTotalAmounts = function(barcode) {
        const foundBarcode = self.findBarcode(barcode);
        if(foundBarcode === undefined) return false;

        const expectedModel = _.find(self.expectedReservationItems, x => x.modelKey === foundBarcode.model.modelKey);
        const expectedAmount =  expectedModel.totalAmountByColor[foundBarcode.barcode.colorId];
        const actualAmount = foundBarcode.model.totalAmountByColor[foundBarcode.barcode.colorId];
        return {
            isFullyIncomed: actualAmount >= expectedAmount,
            expectedAmount,
            actualAmount,
        };
    }

    this.getResult = function () {
        return {
            reservationDate: dayjs(self.reservationGroup.createDate).format(self.dateFormat),
            storehouseIncomes: self.transformActualItemsToBarcodeAmountList(),
        }
    };
    
    this.transformActualItemsToBarcodeAmountList = function () {
        return _.chain(self.actualReservationItems).map((model) => {
            var result = [];
            for (var color of model.colors) {
                const colorId = color.id;
                const modelBarcode = _.find(model.barcodes, x => x.size.value === model.modelInfo.size.value && x.colorId === colorId);
                if (modelBarcode !== undefined) {
                    if (model.totalAmountByColor[colorId] / model.modelInfo.size.parts <= 0) continue;
                    result.push({barcode: modelBarcode.barcode, amount: model.totalAmountByColor[colorId] / model.modelInfo.size.parts});
                } else {
                    for (var sizeValue of model.sizesInfo.values) {
                        const amount = model.sizesInfo.sizeColorAmount[`${sizeValue}_${color.id}`];
                        if (amount === undefined || amount <= 0) {
                            continue;
                        }

                        const size = sizeValue;
                        const sizeColorBarcode = _.find(model.barcodes, x => x.size.value === size && x.colorId === colorId).barcode;
                        if (sizeColorBarcode === undefined) {
                            console.error("barcode not found", model, colorId, size);
                            continue;
                        }

                        result.push({barcode: sizeColorBarcode.barcode, amount: amount});
                    }
                }
            }
            return result;
        }).flatten().value()
    };
}

function Reservations(reservationsGroups) {
    this.reservationsGroups = reservationsGroups;
    this.dateFormat = "DD.MM.YYYY";
    var self = this;

    this.getReservationGroupByDate = function (createDate) {
        return _.find(self.reservationsGroups, x => dayjs(x.createDate).format(self.dateFormat) === createDate);
    }
}