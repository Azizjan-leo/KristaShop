(function($) {
    var RowsGroupSort = function(dt, columns, beforeApplyColumnSortCallback) {
        this.table = dt.table();
        this.columns = columns;
        this._beforeApplyColumnSort = beforeApplyColumnSortCallback;

        this._setCustomSorting(columns);
    };

    RowsGroupSort.prototype = {
        _setCustomSorting: function (columns) {
            for (var column of columns) {
                table.column(`${column.name}:name`).init();
                const header = $(table.column(`${column.name}:name`).header());
                if (column.rowsGroupSortableTarget) {
                    header.addClass("sorting");
                    header.on("click", { column, sender: this }, this._sortColumnClicked);
                }
            }
        },

        _sortColumnClicked: function (event) {
            event.data.sender._applyColumnSort({
                name: event.data.column.name,
                target: event.data.column.rowsGroupSortableTarget,
                isOrigin: event.data.column.rowsGroupSortableTarget == undefined || event.data.column.rowsGroupSortableTarget === ""
            });
        },

        _applyColumnSort: function (sortColumn, originColumn) {
            const prevOrders = table.order();
            table.order([]); // clear previous sorting

            this._beforeApplyColumnSort(sortColumn);
            if (!sortColumn.isOrigin) {
                sortColumn.name = sortColumn.target;
            }

            if (sortColumn.direction === "" || sortColumn.direction == undefined) {
                const colIndex = table.column(`${sortColumn.name}:name`).index();
                const colPrevOrder = _.find(prevOrders, function (prevOrder) { return prevOrder[0] === colIndex });
                if (colPrevOrder === undefined || colPrevOrder[1] === "asc") {
                    sortColumn.direction = "desc";
                } else {
                    sortColumn.direction = "asc";
                }
            }

            this.setColumnSort(sortColumn.name, sortColumn.direction);
            this.applySort();
        },

        setColumnSort: function (colName, direction) {
            const columnIndex = table.column(`${colName}:name`).index();
            table.order().push([columnIndex, direction]);
        },

        applySort: function () {
            table.rows().invalidate();
            table.draw();
        },
    };

    $.fn.dataTable.RowGroupsSort = RowsGroupSort;
    $.fn.DataTable.RowGroupsSort = RowsGroupSort;

    $(document).on("init.dt",
        function (e, settings) {
            // If it is not a datatables init event or
            // If the table is not grouped by rows there is no need to apply this script
            if (e.namespace !== "dt" || !settings.oInit.rowsGroup) {
                return;
            }
            
            const api = new $.fn.dataTable.Api(settings);
            const rowGroupsSort = new RowsGroupSort(api, settings.oInit.columns, settings.oInit.rowsGroupSortable.beforeApplyColumnSort);
        });
}(jQuery));