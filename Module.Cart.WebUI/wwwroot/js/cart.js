$(function () {
    new CartOrderHandler();
    new CartAmountHandler();
    
    function CartOrderHandler() {
        this.orderForm = undefined;
        this.termsAcceptedCheckBox = undefined;
        this.submitOrderButton = undefined;
        this.submitOrderButtonHover = undefined;
        const self = this;
        
        function initialize() {
            self.orderForm = $("#orderForm");
            self.termsAcceptedCheckBox = self.orderForm.find("#Accepted");
            self.submitOrderButton = self.orderForm.find("button[type='submit']");
            self.submitOrderButtonHover = self.orderForm.find(".button-hover");

            self.termsAcceptedCheckBox.on("click", onTermsAcceptedClick);
            self.submitOrderButtonHover.on("click", onSubmitOrderHoverClick);
        }
        
        function onTermsAcceptedClick(event) {
            if (isFormTermsValid()) {
                self.submitOrderButton.attr("disabled", false);
                self.submitOrderButtonHover.hide();
            } else {
                self.submitOrderButton.attr("disabled", true);
                self.submitOrderButtonHover.show();
            }
        }

        function isFormTermsValid() {
            $.validator.unobtrusive.parse(self.orderForm);
            return self.termsAcceptedCheckBox.valid();
        }

        function onSubmitOrderHoverClick(event) {
            isFormTermsValid();
        }
        
        initialize();
    }
    
    function CartAmountHandler() {
        this.totalAmount = undefined;
        this.totalSum = undefined;
        this.totalSumRub = undefined;
        const self = this;
        
        function initialize() {
            self.totalAmount = $(".jsCartTotalAmount");
            self.totalSum = $(".jsCartTotalPrice");
            self.totalSumRub = $(".jsCartTotalPriceRu");
            
            $("[data-amount-change]").on("click", cartItemAmountUpdate);
        }

        function cartItemAmountUpdate(event) {
            const button = $(this);
            const itemId = button.attr("data-cart-item-id");
            const direction = button.attr("data-amount-change");

            _sendAmountChangeData({id: itemId, dir: direction}, button);
        }
        
        function _sendAmountChangeData(amountUpdateObject, button) {
            button.prop('disabled', true);
            
            $.ajax({
                url: '/Cart/ItemAmountChange',
                type: 'POST',
                data: amountUpdateObject,
                dataType: 'json',
                success: function (result) {
                    if (result.success) {
                        setRowAmounts(button, result.addedAmount);
                    } else {
                        showNotificationError(result.message);
                    }
                },
                error: function (jqXHR) {
                    showNotificationError("Ошибка на сервере при попытке добавления товара в корзину.");
                    console.log(jqXHR);
                },
                complete: function() {
                    button.prop('disabled', false);
                }
            });
        }
        
        function setRowAmounts(button, amountToAdd) {
            const spinner = button.parent(".jsCartItemSpinner");
            const row = spinner.parents(".jsParentRow");
            
            const linesCountToAdd = amountToAdd / getRowPartsCount(row);

            setSpinnerAmount(spinner, linesCountToAdd);
            setRowTotalAmount(row, amountToAdd);
            const sumAdded = setRowTotalSum(row, amountToAdd);
            const sumAddedInRub = setRowTotalSumRu(row, amountToAdd);

            const catalog = row.parents(".jsCatalog");
            const prepayPercent = +catalog.find(".jsPrepayPercent").text();

            setCatalogTotalSum(catalog, sumAdded);
            setCatalogTotalSumRub(catalog, sumAddedInRub);
            setCatalogPrepaySum(catalog, sumAdded * prepayPercent);
            setCatalogPrepaySumRub(catalog, sumAddedInRub * prepayPercent);
            
            setTotalAmount(amountToAdd);
            setTotalSum(sumAdded);
            setTotalSumRub(sumAddedInRub);

            setNavbarAmount(amountToAdd);
        }
        
        function setSpinnerAmount(spinner, linesToAdd) {
            const amountWrapper = spinner.find(".jsAmount");
            amountWrapper.text((+amountWrapper.text()) + linesToAdd);
        }
        
        function setRowTotalAmount(row, amountToAdd) {
            const totalAmount = +row.find(".jsTotalAmount").text();
            row.find(".jsTotalAmount").text(totalAmount + amountToAdd);
        }
        
        function getRowPartsCount(row) {
            return +row.find(".jsPartsCount").text()
        }
        
        function setRowTotalSum(row, amountToAdd) {
            const price = +row.find(".jsPrice").text().replace(" ", "");
            const totalSum = +row.find(".jsTotalPrice").text().replace(" ", "");
            row.find(".jsTotalPrice").text(currencyFormatConvert(amountToAdd * price + totalSum));
            return amountToAdd * price;
        }

        function setRowTotalSumRu(row, amountToAdd) {
            const price = +row.find(".jsPriceRu").text().replace(" ", "");
            const totalSum = +row.find(".jsTotalPriceRu").text().replace(" ", "");
            row.find(".jsTotalPriceRu").text(currencyFormatConvert(amountToAdd * price + totalSum));
            return amountToAdd * price;
        }
        
        function setCatalogTotalSum(catalog, sumToAdd) {
            const sumWrapper = catalog.find(".jsCatalogTotalSum");
            const sum = +sumWrapper.text().replace(" ", "");
            sumWrapper.text(Number.toTwoDecimalPlaces(sum + sumToAdd));
        }

        function setCatalogTotalSumRub(catalog, sumToAdd) {
            const sumWrapper = catalog.find(".jsCatalogTotalSumRub");
            const sum = +sumWrapper.text().replace(" ", "");
            sumWrapper.text(Number.toTwoDecimalPlaces(sum + sumToAdd));
        }

        function setCatalogPrepaySum(catalog, sumToAdd) {
            const sumWrapper = catalog.find(".jsCatalogPrepaySum");
            const sum = +sumWrapper.text().replace(" ", "");
            sumWrapper.text(Number.toTwoDecimalPlaces(sum + sumToAdd));
        }

        function setCatalogPrepaySumRub(catalog, sumToAdd) {
            const sumWrapper = catalog.find(".jsCatalogPrepaySumRub");
            const sum = +sumWrapper.text().replace(" ", "");
            sumWrapper.text(Number.toTwoDecimalPlaces(sum + sumToAdd));
        }
        
        function setTotalAmount(amountToAdd) {
            const totalAmount = +self.totalAmount.text();
            self.totalAmount.text(totalAmount + amountToAdd);
        }
        
        function setTotalSum(sumToAdd) {
            const totalSum = +self.totalSum.text().replace(" ", "");
            self.totalSum.text(Number.toTwoDecimalPlaces(totalSum + sumToAdd));
        }

        function setTotalSumRub(sumToAdd) {
            const totalSum = +self.totalSumRub.text().replace(" ", "");
            self.totalSumRub.text(Number.toTwoDecimalPlaces(totalSum + sumToAdd));
        }
        
        function setNavbarAmount(amountToAdd) {
            if (typeof UpdateNavbarCartInfo == 'function') {
                UpdateNavbarCartInfo(amountToAdd);
            }
        }
        
        initialize();
    }
});