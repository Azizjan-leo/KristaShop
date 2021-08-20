$(function () {
    const itemAmountGroupingKey = "itemAmountGroupingKey";
    const orderedTotalAmountColName = "orderedTotalAmount";
    const manufacturingTotalAmountColName = "manufacturingTotalAmount";
    const cuttingTotalAmountColName = "cuttingTotalAmount";
    const sewingTotalAmountColName = "sewingTotalAmount";
    const reservationTotalAmountColName = "reservationTotalAmount";
    const shipmentAmountCountColName = "shipmentTotalAmount";

    const manufactureState = {
        Kroy: 0,
        KroyComplete: 1,
        Zapusk: 2,
        VPoshive: 3,
        SkladGP: 4
    }

    var model = dataModel;
    initializeModelItemsGroups();
    initializeCollectionTables();
    var currentOrderColumn = manufacturingTotalAmountColName;

    function initializeCollectionTables() {
        const tables = [];
        for (let collectionGroup of model.processedItems.items) {
            tables.push(initializeTable(`.itemsTable_${collectionGroup.id}`, collectionGroup.items));
            initializeShipments(_.find(model.shipments.items, x => x.id === collectionGroup.id), collectionGroup.id);
        }
        const filter = new DataTableCustomFilter(tables);

        for (let table of tables) {
            const customSorting = initDataTablesCustomSorting({
                table: table,
                sortByColumn: {name: reservationTotalAmountColName},
                beforeApplyColumnSort: function (aTable, orderArray, column) {
                    currentOrderColumn = column.name;
                    if (isCustomOrderColumn(currentOrderColumn)) {
                        column.name = itemAmountGroupingKey;
                    }

                    // This drops table cached values. Because usually datatables cache column values when it first loaded
                    // to work faster. Due to the specific of sorting in this table, we need to recalculate
                    // some column values based on column we sorting by. That is why we drop cache deliberately sacrificing sorting performance.
                    aTable.rows().invalidate();
                }
            });
        }
    }

    function initializeTable(aTableClassName, aTableData) {
        return $(aTableClassName).DataTable({
            "data": aTableData,
            "responsive": true,
            "orderCellsTop": true,
            "paging": false,
            "cache": false,
            "fixedHeader": true,
            "rowsGroup": [`${itemAmountGroupingKey}:name`, "createDate:name", "itemInfo:name", "colorName:name"],
            "columns": getColumnsDefinitions(aTableData),
            "language": {
                "url": "/common/datatables.Russian.json"
            },
            "dom": "r"
        });
    }

    function getColumnsDefinitions(aTableData) {
        return [
            {
                "name": itemAmountGroupingKey,
                "data": "articul",
                "visible": false,
                "render": function (data, type, full, meta) {
                    return countAmountValueForGroup(aTableData, { articul: full.articul, sizeValue: full.size.value });
                }
            },
            {
                "name": "createDate",
                "data": "itemGroupingKey",
                "type": "itemGroupingKeyType",
                "sortable": false,
                "visible": true,
                "width": "7%",
                "render": function (data, type, full, meta) {
                    if (type === "sort" && currentOrderColumn === "itemInfo") {
                        return 0;
                    }

                    return moment(full.createDate).format("DD.MM.YYYY");
                }
            },
            {
                "name": "itemInfo",
                "data": "itemGroupingKey",
                "type": "itemGroupingKeyType",
                "sortable": false,
                "width": "26%",
                "render": function(data, type, full, meta) {
                    if (type === "sort") {
                        return `${full.articul}_${full.size.value}`.replace(/\s+/g, "_");
                    }

                    const mainPhoto = String.isNullOrEmpty(full.mainPhoto) ? "/common/img/noimage.png?width=80" : `${full.mainPhoto}?width=80`;
                    return `
                    <div class="p-0">
                        <a href="${full.mainPhoto}" data-lightbox="Изображение" data-title="${full.articul}">
                            <picture class="img-fluid rounded-1">
                                <source srcset="${mainPhoto}&format=webp" type="image/webp">
                                <img srcset="${mainPhoto}" class="img-fluid rounded-1" alt="${full.articul}" />
                            </picture>
                        </a>
                        <div class="d-inline-block align-top px-3">
                            <div class="mb-3">Арт: ${full.articul}</div>
                            <div><span class="size-swatch swatch-light">${full.size.value}</span></div>
                            <div class="mt-2"><span>${Number.toTwoDecimalPlaces(full.price)} $ за ед</span></div>
                        </div>
                    </div>`;
                }
            },
            {
                "name": "colorName",
                "data": "colorName",
                "sortable": false,
                "width": "14%",
                "render": function(data, type, full, meta) {
                    if (type === "sort") {
                        // when we sort table by the partsCount columns, we need to sort colors by partsCount too,
                        // so the colors with most partsCount was on top of the group, and with least partsCount was at the bottom of the group.
                        // To do this, based on what parstsCount column was clicked we recalculate value by which this column is sorted.
                        // If we sorting by any of the partsCount columns it returns partsCount for this item (by articul, sizeValue, colorName),
                        // otherwise it returns color name string.
                        return getColorsOrderValue(full);
                    }
                    const background = !full.colorPhoto ? full.colorValue : `url(${full.colorPhoto})`;
                    return `<span class="color-swatch swatch-md align-middle" data-toggle="tooltip" data-placement="top" title="${
                        data}" style="background: ${background};"></span>
                            <span>${data}</span>`;
                },
                "className": "border-top"
            },
            {
                "name": orderedTotalAmountColName,
                "data": "amount",
                "width": "7.5%",
                "sortable": false,
                "render": function(data, type, full, meta) {
                    return getOrderedItemTotalAmount(full);
                },
                "className": "border-top"
            },
            {
                "name": manufacturingTotalAmountColName,
                "data": "amount",
                "width": "7.5%",
                "sortable": false,
                "render": function(data, type, full, meta) {
                    return getItemManufacturingTotalAmount(full, [manufactureState.Zapusk]);
                },
                "className": "border-top"
            },
            {
                "name": cuttingTotalAmountColName,
                "data": "amount",
                "width": "7.5%",
                "sortable": false,
                "render": function(data, type, full, meta) {
                    return getItemManufacturingTotalAmount(full, [manufactureState.Kroy, manufactureState.KroyComplete]);
                },
                "className": "border-top"
            },
            {
                "name": sewingTotalAmountColName,
                "data": "amount",
                "width": "7.5%",
                "sortable": false,
                "render": function(data, type, full, meta) {
                    return getItemManufacturingTotalAmount(full, [manufactureState.VPoshive, manufactureState.SkladGP]);
                },
                "className": "border-top"
            },
            {
                "name": reservationTotalAmountColName,
                "data": "amount",
                "width": "7.5%",
                "sortable": false,
                "render": function(data, type, full, meta) {
                    return getItemReservationTotalAmount(full);
                },
                "className": "border-top"
            },
            {
                "name": shipmentAmountCountColName,
                "data": "amount",
                "width": "7.5%",
                "sortable": false,
                "render": function(data, type, full, meta) {
                    return getItemShipmentTotalAmount(full);
                },
                "className": "border-top"
            }
        ];
    }

    function countAmountValueForGroup(aTableData, data) {
        if (!isCustomOrderColumn(currentOrderColumn)) {
            return 0;
        }

        const items = _.filter(aTableData, function (object) { return object.articul === data.articul && object.size.value === data.sizeValue });

        let result = 0;
        if (currentOrderColumn === orderedTotalAmountColName) {
            for (let item of items) {
                result += getOrderedItemTotalAmount(item);
            }
            return result;
        }

        if (currentOrderColumn === manufacturingTotalAmountColName) {
            for (let item of items) {
                result += getItemManufacturingTotalAmount(item,  [manufactureState.Zapusk]);
            }
            return result;
        }

        if (currentOrderColumn === cuttingTotalAmountColName) {
            for (let item of items) {
                result += getItemManufacturingTotalAmount(item, [manufactureState.Kroy, manufactureState.KroyComplete]);
            }
            return result;
        }

        if (currentOrderColumn === sewingTotalAmountColName) {
            for (let item of items) {
                result += getItemManufacturingTotalAmount(item, [manufactureState.VPoshive, manufactureState.SkladGP]);
            }
            return result;
        }

        if (currentOrderColumn === reservationTotalAmountColName) {
            for (let item of items) {
                result += getItemReservationTotalAmount(item);
            }
            return result;
        }

        if (currentOrderColumn === shipmentAmountCountColName) {
            for (let item of items) {
                result += getItemShipmentTotalAmount(item);
            }
            return result;
        }

        return result;
    }

    function getColorsOrderValue(full) {
        if (currentOrderColumn === orderedTotalAmountColName) {
            return getOrderedItemTotalAmount(full);
        }

        if (currentOrderColumn === manufacturingTotalAmountColName) {
            return getItemManufacturingTotalAmount(full, [manufactureState.Zapusk]);
        }
        
        if(currentOrderColumn === cuttingTotalAmountColName) {
            return getItemManufacturingTotalAmount(full, [manufactureState.Kroy, manufactureState.KroyComplete]);
        }
        
        if(currentOrderColumn === sewingTotalAmountColName){
            return getItemManufacturingTotalAmount(full, [manufactureState.VPoshive, manufactureState.SkladGP])
        }

        if (currentOrderColumn === reservationTotalAmountColName) {
            return getItemReservationTotalAmount(full);
        }

        if (currentOrderColumn === shipmentAmountCountColName) {
            return getItemShipmentTotalAmount(full);
        }

        return full.colorName;
    }

    function isCustomOrderColumn(columnName) {
        if (columnName !== shipmentAmountCountColName &&
            columnName !== reservationTotalAmountColName &&
            columnName !== manufacturingTotalAmountColName &&
            columnName !== cuttingTotalAmountColName &&
            columnName !== sewingTotalAmountColName &&
            columnName !== orderedTotalAmountColName) {
            return false;
        }

        return true;
    }

    //#region items amounts
    function initializeModelItemsGroups() {
        model.amountDictionaries = {
            orderedItems: createItemSumsGroups(model.orderedItems, getItemKey),
            manufactureItems: createItemSumsGroups(model.manufactureItems, getManufactureItemKey),
            reservations: createItemSumsGroups(model.reservations, getItemKey),
            shipments: createItemSumsGroupsComplex(model.shipments, getItemKey)
        }
    }

    function createItemSumsGroups(itemGroups, groupingFunc) {
        var result = {};
        _.chain(itemGroups.items)
            .map("items")
            .flatten()
            .groupBy(x => groupingFunc(x))
            .map((items, key) => (result[key] = { key: key, amount: _.sumBy(items, x => x.amount), lines: _.sumBy(items, x => x.amount / x.size.parts) }))
            .value();
        return result;
    }

    function createItemSumsGroupsComplex(itemGroups, groupingFunc) {
        var result = {};
        _.chain(itemGroups.items)
            .map("items")
            .flatten()
            .map("items")
            .flatten()
            .groupBy(x => groupingFunc(x))
            .map((items, key) => (result[key] = { key: key, amount: _.sumBy(items, x => x.amount), lines: _.sumBy(items, x => x.amount / x.size.parts) }))
            .value();
        return result;
    }


    function getManufactureItemKey(x) {
        return `${getItemKey(x)}_${x.state}`;
    }

    function getItemKey(aBasicOrderItem) {
        return `${aBasicOrderItem.articul}_${aBasicOrderItem.size.value}_${aBasicOrderItem.colorName}`;
    }

    function getOrderedItemTotalAmount(aBasicOrderItem) {
        const item = model.amountDictionaries.orderedItems[getItemKey(aBasicOrderItem)];
        return item ? item.amount : 0;
    }

    function getItemManufacturingTotalAmount(aBasicOrderItem, state) {
        if(model.manufactureItems.items.length <= 0){
            return 0;
        }

        var collection = _.find(model.manufactureItems.items, x => x.id === aBasicOrderItem.collectionId);
        if(collection === undefined || collection === null){
            return 0;
        }
        
        const manufactureItem = _.find(collection.items, x => x.modelKey === aBasicOrderItem.modelKey);
        if (manufactureItem === undefined || !_.includes(state, manufactureItem.state)) {
            return 0;
        }

        const item = model.amountDictionaries.manufactureItems[getManufactureItemKey(manufactureItem)];
        return item ? item.amount : 0;
    }

    function getItemReservationTotalAmount(aBasicOrderItem) {
        const item = model.amountDictionaries.reservations[getItemKey(aBasicOrderItem)];
        return item ? item.amount : 0;
    }

    function getItemShipmentTotalAmount(aBasicOrderItem) {
        const item = model.amountDictionaries.shipments[getItemKey(aBasicOrderItem)];
        return item ? item.amount : 0;
    }

    //#endregion items amounts

    //#region shipments
    function initializeShipments(shipmentsList, collectionId) {
        if (shipmentsList == undefined) return;

        for (let shipmentGroup of shipmentsList.items) {
            $(`.shipmentItemsTable_${collectionId}_${moment(shipmentGroup.createDate).format("DDMMYYYY")}_${shipmentGroup.name}`).DataTable({
                "data": shipmentGroup.items,
                "responsive": true,
                "paging": false,
                "columns": getColumnsDefinitions(),
                "rowsGroup": ["itemInfo:name"],
                "language": {
                    "url": "/common/datatables.Russian.json"
                },
                "dom": "r"
            });
        }

        function getColumnsDefinitions() {
            return [
                {
                    "name": "itemInfo",
                    "data": "itemGroupingKey",
                    "width": "30%",
                    "render": function (data, type, full, meta) {
                        const mainPhoto = String.isNullOrEmpty(full.mainPhoto) ? "/common/img/noimage.png?width=80" : `${full.mainPhoto}?width=80`;
                        return `
                        <div class="p-0">
                            <a href="${full.mainPhoto}" data-lightbox="Изображение" data-title="${full.articul}">
                                <picture class="img-fluid rounded-1">
                                    <source srcset="${mainPhoto}&format=webp" type="image/webp">
                                    <img srcset="${mainPhoto}" class="img-fluid rounded-1" alt="${full.articul}" />
                                </picture>
                            </a>
                            <div class="d-inline-block align-top px-3">
                                <div class="mb-3">Арт: ${full.articul}</div>
                                <div><span class="size-swatch swatch-light">${full.size.value}</span></div>
                                <div class="mt-2"><span>${Number.toTwoDecimalPlaces(full.price)} $ за ед</span></div>
                            </div>
                        </div>`;
                    }
                },
                {
                    "data": "colorName",
                    "width": "14%",
                    "render": function (data, type, full, meta) {
                        const background = !full.colorPhoto ? full.colorValue : `url(${full.colorPhoto})`;
                        return `<span class="color-swatch swatch-md align-middle" data-toggle="tooltip" data-placement="top" title="${data}" style="background: ${background};"></span>
                            <span>${data}</span>`;
                    }
                },
                {
                    "data": "amount",
                    "width": "14%",
                    "render": function (data, type, full, meta) {
                        return full.amount / (full.size.parts <= 0 ? 1 : full.size.parts);
                    },
                },
                {
                    "data": "amount",
                    "width": "21%"
                },
                {
                    "data": "totalPrice",
                    "width": "21%",
                    "render": function (data, type, full, meta) {
                        return `${currencyFormatConvert(data)} $`;
                    }
                }
            ];
        }
    };

    $("[data-save-filepath]").on("click", saveFile);
    function saveFile(e) {
        e.preventDefault();
        e.stopPropagation();
        const source = this.dataset;
        $.ajax({
            type: "GET",
            url: `/Personal/GetShipmentFile?filePath=${source.saveFilepath}`,
            dataType: "json"
        }).done(function (responseText) {
            File.saveFromBase64(source.saveFileName, "pdf", responseText)
        }).fail(function (jqXHR) {
            showNotificationError("Не удалось скачать файл");
        });
    }
    //#endregion
});