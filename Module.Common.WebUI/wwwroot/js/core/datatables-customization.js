//#region sorting

function initDataTablesCustomSorting(options) {
    const dataTablesCustomSorting = {
        options: {
            table: undefined,
            sortByColumn: { name: '', direction: 'asc' },
            beforeApplyColumnSort: function(aTable, orderArray, column) { }
        },
        dataTagName: "data-sort-target-col",
        orderArray: [],
        initSort: function () {
            this.options = options;
            if(this.options.beforeApplyColumnSort === undefined){
                this.options.beforeApplyColumnSort = function (aTable, orderArray, column){};
            }
            
            $(this.options.table.table().node())
                .find(`[${this.dataTagName}]`)
                .on("click", sortColumnClicked);
            if(this.options.sortByColumn.name !== '') {
                this.applyColumnSort(this.options.sortByColumn);
            }
        },

        setColumnSort: function (colName, direction) {
            const columnIndex = this.options.table.column(`${colName}:name`).index();
            this.orderArray.unshift([columnIndex, direction]);
        },

        applySort: function () {
            this.options.table.order([]);
            this.options.table
                .order(this.orderArray)
                .draw();
        },

        applyColumnSort: function (column) {
            const prevOrders = this.options.table.order();
            this.orderArray = []; // clear previous sorting

            this.options.beforeApplyColumnSort(this.options.table, this.orderArray, column);
            if (column.direction === "" || column.direction === undefined) {
                const colIndex = this.options.table.column(`${column.name}:name`).index();
                const colPrevOrder = _.find(prevOrders, function(prevOrder) { return prevOrder[0] === colIndex });
                if (colPrevOrder === undefined || colPrevOrder[1] === "asc") {
                    column.direction = "desc";
                } else {
                    column.direction = "asc";
                }
            }

            this.setColumnSort(column.name, column.direction);
            this.applySort();
        }
    };

    function sortColumnClicked(event) {
        dataTablesCustomSorting.applyColumnSort({ name: this.dataset.sortTargetCol });
    }

    dataTablesCustomSorting.initSort();
    return dataTablesCustomSorting;
}

//#endregion sortings

//#region filtering
function DataTableCustomFilter(tables, options = undefined) {
    this.tables = tables;
    this.filterOptions = {
        applyByButton: true,
        clearByButton: true,
        blockWrapperSelector: ".dataTables_customSearchWrapper",
        applyButtonSelector: "[data-apply-filter]",
        clearButtonSelector: "[data-clear-filter]",
        inputsDataAttributeName: "target-col",
        rangeInputsDataAttributeName: "range-target-col",
        rangeTypeAttributeName: "range-type",
        inputsSelector: "",
        rangeInputsSelector: "",
        dateFormat: "DD.MM.YYYY",
    };
    const self = this;
         
    this.onInitialize = function() {
        initFilterOptions();
        initFilterEvents();
        
        // This filters are applied when table is drawing (i.e. when draw() function called on table) 
        $.fn.dataTable.ext.search.push(applyRangeFromFilter);
        $.fn.dataTable.ext.search.push(applyRangeToFilter);
        
        // apply initial filter
        applyFilter();
    }
    
    function initFilterOptions() {
        self.filterOptions = Object.assign(self.filterOptions, options);
        self.filterOptions.inputsSelector = `[data-${self.filterOptions.inputsDataAttributeName}]`;
        self.filterOptions.rangeInputsSelector = `[data-${self.filterOptions.rangeInputsDataAttributeName}]`;
        self.filterOptions.wrapper = $(document).find(self.filterOptions.blockWrapperSelector);
        
        self.filterOptions.inputs = self.filterOptions.wrapper.find(self.filterOptions.inputsSelector);
        self.filterOptions.rangeInputs = self.filterOptions.wrapper.find(self.filterOptions.rangeInputsSelector);
        
        self.filterOptions.textInputs =  self.filterOptions.inputs.filter("input[type=text]");
        self.filterOptions.rangeDateInputsFrom = self.filterOptions.rangeInputs.filter(`input[type=text][data-datetime-picker][data-${self.filterOptions.rangeTypeAttributeName}="from"]`);
        self.filterOptions.rangeDateInputsTo = self.filterOptions.rangeInputs.filter(`input[type=text][data-datetime-picker][data-${self.filterOptions.rangeTypeAttributeName}="to"]`);
        self.filterOptions.selectInputs =  self.filterOptions.inputs.filter("select.selectpicker");
    }
    
    function initFilterEvents() {
        if (self.filterOptions.applyByButton) {
            self.filterOptions.wrapper.find(self.filterOptions.applyButtonSelector).on("click", applyFilter);
        } else {
            initInputsChangeEvents();
        }
        
        if(self.filterOptions.clearByButton) {
            self.filterOptions.wrapper.find(self.filterOptions.clearButtonSelector).on("click", clearFilter);
        }
    }
    
    function initInputsChangeEvents(){
        self.filterOptions.textInputs.on("keyup", applyFilter);
        self.filterOptions.selectInputs.on("changed.bs.select", applySelectFilter);
        self.filterOptions.rangeDateInputsFrom.on("change.datetimepicker", function () { filterTables() });
        self.filterOptions.rangeDateInputsTo.on("change.datetimepicker", function () { filterTables() });
    }
    
    function applyFilter() {
        for (var input of self.filterOptions.textInputs) {
            setColumnFilter($(input).data(self.filterOptions.inputsDataAttributeName), $(input).val());
        }
        filterTables();
    }

    function applySelectFilter() {
        for (var input of self.filterOptions.selectInputs) {
            let value = $(input).val();
            if(Array.isArray(value)){
                value = value.join(" ");
            }
            setColumnFilter($(input).data(self.filterOptions.inputsDataAttributeName), $(input).val());
        }
        filterTables();
    }
    
    function applyRangeFromFilter(settings, data, dataIndex) { 
        var table = getTable(settings);
        if(table === undefined) return true;
        
        for(var input of self.filterOptions.rangeDateInputsFrom) {
            var filterDate = dayjs(input.value, self.filterOptions.dateFormat);
            if(!filterDate.isValid()) continue;
            
            const column = table.column(`${$(input).data(self.filterOptions.rangeInputsDataAttributeName)}:name`).index();
            if(column === undefined) continue;
            
            if(dayjs(data[column]) < filterDate) {
                return false;
            }
        }
        
        return true;
    }

    function applyRangeToFilter(settings, data, dataIndex) {
        var table = getTable(settings);
        if(table === undefined) return true;

        for(var input of self.filterOptions.rangeDateInputsTo) {
            var filterDate = dayjs(input.value, self.filterOptions.dateFormat);
            if(!filterDate.isValid()) continue;

            const column = table.column(`${$(input).data(self.filterOptions.rangeInputsDataAttributeName)}:name`).index();
            if(column === undefined) continue;

            if(dayjs(data[column]) > filterDate) {
                return false;
            }
        }

        return true;
    }
    
    function getTable(settings) {
        for(var table of self.tables) {
            if(table.context[0].sTableId === settings.sTableId) {
                return table;
            }
        }
        return undefined;
    }

    function clearFilter() {
        for (var input of self.filterOptions.textInputs) {
            setColumnFilter($(input).data(self.filterOptions.inputsDataAttributeName), $(input).val(""));
        }
        self.filterOptions.selectInputs.selectpicker('deselectAll');
        self.filterOptions.selectInputs.selectpicker('val', '');
        self.filterOptions.selectInputs.selectpicker('render');
        self.filterOptions.rangeInputs.val('');
        clearTables();
    }

    function setColumnFilter(colName, value) {
        for (var table of self.tables) {
            table.columns(`${colName}:name`)
                .search(value);
        }
    }

    function filterTables() {
        for (var table of self.tables) {
            table.draw();
        }
    }

    function clearTables() {
        for (var table of self.tables) {
            table.search("")
                .columns().search("")
                .draw();
        }
    }
    
    this.onInitialize();
}

//#endregion filtering

function redrawTableWithNewData (aTable, data) {
    aTable.clear();
    aTable.rows.add(data); // Add new data
    aTable.columns.adjust().draw(); // Redraw the DataTable
}