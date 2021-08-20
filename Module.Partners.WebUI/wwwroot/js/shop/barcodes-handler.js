function BarcodeInputHandler(configurationOptions) {
    this.barcodeValue = '';
    const ENTER_KEYCODE = 13;
    const BARCODE_VALID_LENGTH = 16;
    const BARCODE_FIRST_VALUE = '0';
    this.options = {
        onBarcodeInput: function () {  }, //return Promise
        onEnterPressed: function () {  },
        barcodeInputSelector: "#barcodeInput",
        barcodeInput: {},
        barcodeInfoInputSelector: "#barcodeInfo",
        barcodeInfoInput: {},
        barcodeNotFoundText: 'По данному баркоду ничего не найдено',
        barcodeMaxLengthExceededText: `Баркод должен быть ${BARCODE_VALID_LENGTH} символов`
    };
    const self = this;

    function initialize() {
        Object.assign(self.options, configurationOptions);
        self.options.barcodeInput = $(self.options.barcodeInputSelector);
        self.options.barcodeInfoInput = $(self.options.barcodeInfoInputSelector);

        $(window).on('keypress', globalKeyPressHandle);
        self.options.barcodeInput.on('input', onBarcodeInput);
        self.options.barcodeInput.on('keyup', onPressEnterHandle);
    }

    function globalKeyPressHandle(event) {
        if(event.target.isSameNode(self.options.barcodeInput[0])) {
            return;
        }

        if(event.keyCode === ENTER_KEYCODE) {
            if(self.barcodeValue.startsWith(BARCODE_FIRST_VALUE) && self.barcodeValue.length === BARCODE_VALID_LENGTH) {
                self.options.barcodeInput.val(self.barcodeValue);
                self.barcodeValue = '';
                barcodeInputChanged();
                return;
            }
        }

        if(self.barcodeValue.length > BARCODE_VALID_LENGTH) {
            self.barcodeValue = '';
        }

        self.barcodeValue += event.key;
    }
    
    function onBarcodeInput() {
        barcodeInputChanged();
    }

    function onPressEnterHandle(event) {
        if (event.keyCode === ENTER_KEYCODE) {
            event.preventDefault();
            self.options.onEnterPressed();
        }
    }

    function barcodeInputChanged () {
        self.options.barcodeInput.val(RegExp.removeNonNumbers(self.options.barcodeInput.val()));
        self.options.barcodeInfoInput.val('');
        const barcode = self.options.barcodeInput.val();
        if (barcode.length === BARCODE_VALID_LENGTH){
            self.options.onBarcodeInput(barcode)
                .then(barcodeInfo => {
                    self.options.barcodeInfoInput.val(barcodeInfo);
                    self.options.barcodeInput.removeClass('border-danger');
                })
                .catch(error => {
                    self.options.barcodeInput.addClass('border-danger');
                    self.options.barcodeInfoInput.val(self.options.barcodeNotFoundText);
                })
            return;
        }

        if (barcode.length > BARCODE_VALID_LENGTH) {
            self.options.barcodeInput.addClass('border-danger');
            self.options.barcodeInfoInput.val(self.options.barcodeMaxLengthExceededText);
        } else {
            self.options.barcodeInput.removeClass('border-danger');
        }
    }
    
    this.cleanInputs = function () {
        self.options.barcodeInput.val('');
        self.options.barcodeInfoInput.val('');
        self.barcodeValue = '';
    }
    
    this.destroy = function() {
        $(window).off('keypress', globalKeyPressHandle);
        self.options.barcodeInput.off('input', onBarcodeInput);
        self.options.barcodeInput.off('keyup', onPressEnterHandle);
    }

    initialize();
}