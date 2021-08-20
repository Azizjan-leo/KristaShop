function StorehouseModal(storehouseItems, tableSelector, onSelectCallback) {
    this.storehouseItems = storehouseItems;
    this.tableSelector = $(tableSelector);
    this.storehouseModal = $("#storehouseModal");
    this.approveSelectButton = $(".approveSelectFromStorehouseBtn");
    this.table = undefined;
    const self = this;
    
    function onInitialize() { 
        createTable();
        addEventHandlers();
    }

    this.openModal = function (selectedModelKeys) {
        const selector = _.chain(selectedModelKeys).map( function(item) { return `[data-model-key=${item}]` }).join(', ').value();
        
        self.table.rows().every(function() {
            const node = $(this.node());
            node.removeClass('selected');
            if(selectedModelKeys.includes(this.data().modelKey)) {
                node.addClass('selected');
                node.find("input[type=checkbox]").each(function () { this.checked = true; })
            }
        });
        
        self.storehouseModal.modal('show');
    }
    
    function createTable()  {
        self.table = $(".storehouseItemsModalTable").DataTable({
            "data": self.storehouseItems,
            "responsive": true,
            "paging": false,
            "autoWidth": false,
            "columns": [
                {
                    "name": "modelKey",
                    "data": "modelKey",
                    "sortable": false,
                    "width": "85%",
                    "render": function (data, type, full, meta) {
                        if (type !== "display") {
                            return full.modelInfo.name;
                        }

                        return getModelGridRenderer().renderSmallModelGrid(full);
                    }
                },
                {
                    "name": "actionButtons",
                    "data": "modelKey",
                    "visible": true,
                    "sortable": false,
                    "width": "15%",
                    "render": function (data, type, full, meta) {
                        return `<label class="container-checkbox checkbox-secondary">
                                    <input type="checkbox" name="storehouse-model-select" value="false">
                                    <span class="checkmark"></span>
                                </label>`;
                    },
                    "className": "align-middle"
                }
            ],
            createdRow: function (row, data, dataIndex) {
                $(row).attr('data-model-key', data.modelKey);
            },
            "language": {
                "url": "/common/datatables.Russian.json"
            },
            "dom": "r"
        });
    }

    function addEventHandlers() {
        self.approveSelectButton.on('click', approveSelect);
        self.tableSelector.on('click', 'tbody tr', rowSelected);
        self.tableSelector.on('click', 'input[name=storehouse-model-select]', checkboxClick)
    }
    
    function rowSelected() {
        const row = $(this);
        row.toggleClass('selected');
        row.find("input[type=checkbox]").each(function () { this.checked = !this.checked; });
    }
    
    function checkboxClick() { 
        $(this).parents('tr').toggleClass('selected');
    }
    
    function approveSelect() {
        self.selectedData = self.table.rows('.selected').data().toArray();
        self.storehouseModal.modal('hide');
        onSelectCallback(self.selectedData);
    }
    
    onInitialize();
}