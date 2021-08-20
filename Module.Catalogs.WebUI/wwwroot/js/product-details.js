$(function() {
    var amountElement = $("#BasicAmount");
    var priceElement = $("#defaultPrice");

    var priceLabel = $("#SinglePrice");
    var priceLabelRu = $("#SinglePriceRu");
    var priceLineLabel = $("#ProductPrice");
    var priceLineLabelRu = $("#ProductPriceRu");

    var priceRuElement = $("#defaultPriceRu");
    var totalPriceElement = $("#totalPrice");
    var totalPriceRuElement = $("#totalPriceRu");
    var totalPartsCountElement = $("#totalPartsCount");
    var totalLinesCountElement = $("#totalLinesCount");
    var minusElement = $(".minus");
    var plusElement = $(".plus");
    var colorNameElement = $("#colorName");
    var colorsWrapper = $(".div-colors");
    var sizesWrapper = $(".div-sizes");
    var cartBadgeElement = $(".cart-badge");

    var colorIdInput = $("#ColorId");
    var modelIdInput = $("#ModelId");
    var nomenclatureIdInput = $("#NomenclatureId");
    var sizeValueInput = $("#SizeValue")
    var amountPartsCountInput = $("#Amount");
    var priceInput = $("#Price");
    var priceInRubInput = $("#PriceInRub");
    var addToCartForm = $("#addToCartForm");
    function getAmount() {
        return parseInt(amountElement.val());
    }

    function setAmount(value, maxAmount) {
        if (maxAmount != undefined && value > maxAmount) {
            value = maxAmount;
        }
        amountElement.val(value);
    }

    function addToAmount(count, maxAmount) {
        var newValue = getAmount();

        if (maxAmount == undefined || newValue < maxAmount) {
            newValue = newValue + count;
        } else {
            newValue = maxAmount;
        }
        amountElement.val(newValue);
    }

    function subtractFromAmount(count) {
        var newValue = getAmount();
        if (newValue > 1) {
            newValue = newValue - count;
        } else {
            newValue = 0;
        }
        amountElement.val(newValue);
    }

    function getPrice() {
        if (priceElement.length > 0) {
            return parseFloat(priceElement.val().replace(",", "."));
        }
        return 0;
    }

    function setPrice(value) {
        if (priceElement.length > 0) {
            priceElement.val(value);
        }
    }
    
    function getPriceRu() {
        if (priceRuElement.length > 0) {
            return parseFloat(priceRuElement.val().replace(",", "."));
        }
        return 0;
    }

    function setPriceRu(value) {
        if (priceRuElement.length > 0) {
           priceRuElement.val(value);
        }
    }
    
    function setTotalPrice(price, partsCount) {
        totalPriceElement.val(currencyFormatDE(price));
        totalPriceElement.text(currencyFormatDE(price));

        if (priceLabel.length > 0) {
            priceLabel.text(currencyFormatDE(price / getAmount() / partsCount));
        }

        if (priceLineLabel.length > 0) {
            priceLineLabel.text(currencyFormatDE(price / getAmount()));
        }
    }

    function setTotalPriceRu(price, partsCount) {
        totalPriceRuElement.val(currencyFormatDE(price));
        totalPriceRuElement.text(currencyFormatDE(price));

        if (priceLabelRu.length > 0) {
            priceLabelRu.text(currencyFormatDE(price / getAmount() / partsCount));
        }

        if (priceLineLabelRu.length > 0) {
            priceLineLabelRu.text(currencyFormatDE(price / getAmount()));
        }
    }

    function setTotalPartsCount(partsCount) {
        totalPartsCountElement.val(partsCount);
        totalPartsCountElement.text(partsCount);
    }

    function setTotalLinesCount(linesCount) {
        totalLinesCountElement.val(linesCount);
        totalLinesCountElement.text(linesCount);
    }

    function setColorName(name) {
        colorNameElement.text(name);
    }

    function getSelectedSizeKey() {
        return $('input[type=radio][name=Size]:checked').val();
    }

    function getSelectedColorId() {
        const colorId = $('input[type=radio][name=Color]:checked').val();
        if (colorId !== undefined) {
           return parseInt(colorId);
        }

        return -1;
    }

    // Event handlers

    var slider = $("#imageGallery").lightSlider({
        gallery: true,
        adaptiveHeight: true,
        item: 1,
        thumbItem: 5,
        vThumbWidth: 150,
        isDragingThumbsNow: false,
        galleryMargin: 24,
        thumbMargin: 16,
        enableDrag: false,
        onBeforeStart: function(el) {
            el.children('li').each(function(index, element) {
                element.setAttribute("data-id", index);
            });
        }
    });

    function goToSlideWithColor(colorId, colorName) {
        $("#colorName").text(colorName);
        var $slide = $("#imageGallery [data-colorid=" + colorId + "]");
        if ($slide.length > 0) {
            var indexSlide = +$slide[0].getAttribute("data-id");
            slider.goToSlide(indexSlide);
        }
    }

    sizesWrapper.on("change", "input[type=radio][name=Size]", function(event) {
        event.preventDefault();
        sizesWrapper.find("label").removeClass("active");
        $(this).parent().addClass("active");

        const sizeKey = this.value;
        const selectedColor = setColors(catalogItem.getColorsBySizeKey(sizeKey));
        setColorName(selectedColor.name);
        goToSlideWithColor(selectedColor.id, selectedColor.name);

        const selectedItem = catalogItem.getSelectedCatalogItem(sizeKey, selectedColor.id);
        setPrices(selectedItem);
        updateAddToCartFormInputs(selectedItem);
    });

    colorsWrapper.on("change", 'input[type=radio][name="Color"]',
        function() {
            const sizeKey = getSelectedSizeKey();
            const colorId = +this.value;
            
            const selectedItem = catalogItem.getSelectedCatalogItem(sizeKey, colorId);
            goToSlideWithColor(colorId, this.dataset.colorname);
            setColorName(this.dataset.colorname);
           
            setPrices(selectedItem);
            updateAddToCartFormInputs(selectedItem);
        });

    plusElement.click(function(e) {
        e.preventDefault();
        const selectedItem = catalogItem.getSelectedCatalogItem(getSelectedSizeKey(), getSelectedColorId());
        addToAmount(1, selectedItem.realAmount);
        updateAddToCartFormInputAmount(selectedItem);

        setTotalLinesCount(getAmount());
        setTotalPartsCount(getAmount() * selectedItem.size.parts);
        setTotalPrice(getAmount() * selectedItem.size.parts * selectedItem.price, selectedItem.size.parts);
        setTotalPriceRu(getAmount() * selectedItem.size.parts * selectedItem.priceInRub, selectedItem.size.parts);
        disableMinusButtonForZeroAmount();
    });

    minusElement.click(function(e) {
        e.preventDefault();
        const selectedItem = catalogItem.getSelectedCatalogItem(getSelectedSizeKey(), getSelectedColorId());
        subtractFromAmount(1);
        updateAddToCartFormInputAmount(selectedItem);

        setTotalLinesCount(getAmount());
        setTotalPartsCount(getAmount() * selectedItem.size.parts);
        setTotalPrice(getAmount() * selectedItem.size.parts * selectedItem.price, selectedItem.size.parts);
        setTotalPriceRu(getAmount() * selectedItem.size.parts * selectedItem.priceInRub, selectedItem.size.parts);
        disableMinusButtonForZeroAmount();
    });

    addToCartForm.on("submit", function(e) {
        try {
            e.preventDefault();
            const action = this.action;
            const dataToSend = new FormData(this);
            $.ajax({
                method: "POST",
                url: action,
                data: dataToSend,
                cache: false,
                contentType: false,
                processData: false,
                success: function(data, textStatus, jqXHR) {
                    if (data.success) {
                        showNotificationSuccess(data.message);
                    } else {
                        showNotificationError(data.message);
                        reloadModelData();
                    }
                    UpdateNavbarCartInfo(data.partsCount);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    if (jqXHR.status === 401)
                        window.location.reload();
                    showNotificationError("Не удалось добавить товар в корзину. Произошла ошибка при добавлении");
                }
            });
        } catch (error) {
            location.reload();
        }
    });

    amountElement.on("change",
        function() {
            const selectedItem = catalogItem.getSelectedCatalogItem(getSelectedSizeKey(), getSelectedColorId());
            let newValue = +$(this).val();
            if (newValue <= 0) {
                newValue = 0;
            } else if (newValue > selectedItem.realAmount) {
                newValue = selectedItem.realAmount;
                showNotificationError(`Максимально доступное количество: ${selectedItem.realAmount}`);
            } else if (newValue > 1000) {
                newValue = 1000;
            }
            $(this).val(newValue);

            setTotalLinesCount(getAmount());
            setTotalPartsCount(getAmount() * selectedItem.size.parts);
            setTotalPrice(getAmount() * selectedItem.size.parts * selectedItem.price, selectedItem.size.parts);
            setTotalPriceRu(getAmount() * selectedItem.size.parts * selectedItem.priceInRub, selectedItem.size.parts);
            disableMinusButtonForZeroAmount();
            updateAddToCartFormInputAmount(selectedItem);
        });

    function disableMinusButtonForZeroAmount() {
        minusElement.removeAttr("disabled");
        if (getAmount() <= 1) {
            minusElement.attr("disabled", true);
        }
    }

    function reloadModelData() {
        catalogItem.reloadModelData();
        const selectedSizeKey = setSizes(catalogItem.getSizes());
        const selectedColor = setColors(catalogItem.getColorsBySizeKey(selectedSizeKey));
        setColorName(selectedColor.name);
        goToSlideWithColor(selectedColor.id, selectedColor.name);

        const selectedModel = catalogItem.getSelectedCatalogItem(selectedSizeKey, selectedColor.id);
        setPrices(selectedModel);
        updateAddToCartFormInputs(selectedModel);
    }

    function setSizes(sizes) {
        if (sizes == undefined) {
            throw new TypeError("Failed to set sizes");
        }

        let selectedSizeKey = getSelectedSizeKey();
        const selectedSize = sizes.find(x => x.key === selectedSizeKey);
        if (selectedSize == undefined && sizes.length > 0) {
            selectedSizeKey = sizes[0].key;
        }

        sizesWrapper.html("");
        for (let i = 0; i < sizes.length; i++) {
            const isSelected = sizes[i].key === selectedSizeKey;
            const sizeHtml =
                `<label class="btn btn-sm btn-filter-light rounded-0 mr-2 ${isSelected ? "active" : ""}">
                    <input type="radio" name="Size" autocomplete="off" value="${sizes[i].key}" ${isSelected ? "checked" : ""}>
                    ${sizes[i].value}
                </label>`;

            sizesWrapper.append(sizeHtml);
        }

        return selectedSizeKey;
    }

    function setColors(colors) {
        if (colors == undefined) {
            throw new TypeError("Failed to set colors");
        }

        const selectedColorId = getSelectedColorId();
        let selectedColor = colors.find(x => x.id === selectedColorId);
        if (selectedColor == undefined && colors.length > 0) {
            selectedColor = colors[0];
        }

        colorsWrapper.html("");
        for (let i = 0; i < colors.length; i++) {
            const isSelected = selectedColor != undefined && colors[i].id === selectedColor.id;
            const background = colors[i].image !== "" && colors[i].image != null ? `url(${colors[i].image})` : colors[i].code;
            const colorHtml =
                `<label class="m-0">
                    <input type="radio" name="Color" autocomplete="off" value="${colors[i].id}" data-colorname="${colors[i].name}" ${isSelected ? "checked" : ""} />
                    <span class="color-swatch mx-1" style="background: ${background};" data-toggle="tooltip" data-placement="top" title="${colors[i].name}"></span>
                </label>`;

            colorsWrapper.append(colorHtml);
        }

        return selectedColor;
    }

    function setPrices(selectedItem) {
        setPrice(selectedItem.price);
        setPriceRu(selectedItem.priceInRub);

        setAmount(1, selectedItem.amount);
        updateAddToCartFormInputAmount(selectedItem);

        setTotalLinesCount(getAmount());
        setTotalPartsCount(getAmount() * selectedItem.size.parts);
        setTotalPrice(getAmount() * selectedItem.size.parts * selectedItem.price, selectedItem.size.parts);
        setTotalPriceRu(getAmount() * selectedItem.size.parts * selectedItem.priceInRub, selectedItem.size.parts);
        return true;
    }

    function updateAddToCartFormInputs(selectedItem) {
        colorIdInput.val(selectedItem.color.id);
        modelIdInput.val(selectedItem.modelId);
        nomenclatureIdInput.val(selectedItem.nomenclatureId);
        sizeValueInput.val(selectedItem.size.value);
        priceInput.val(floatToCommaString(getPrice()));
        priceInRubInput.val(floatToCommaString(getPriceRu()));
        updateAddToCartFormInputAmount(selectedItem);
    }

    function updateAddToCartFormInputAmount(selectedItem) {
        amountPartsCountInput.val(getAmount() * selectedItem.size.parts);
    }

    function currencyFormatDE(num) {
        return (
            num
                .toFixed(2) // always two decimal digits
                .replace('.', ',') // replace decimal point character with ,
                .replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.')
                .replace('.', ' ') // replace decimal point character with ,
                .replace(',', '.') // replace decimal point character with ,
        );
    }

    function floatToCommaString(value) {
        return value.toString().replace('.', ',');
    }
});

function updateFavoriteButton(form) {
    var icon = $(form).find("[data-favorite]");
    var favoriteClass = icon[0].dataset.favorite;
    if (icon.hasClass(favoriteClass)) {
        icon.removeClass(favoriteClass);
    } else {
        icon.addClass(favoriteClass);
    }
}

let catalogItem = {
    modelData: {
        itemFull: { }
    },

    setModelData: function (dataString) {
        this.modelData = JSON.parse(dataString);
    },

    reloadModelData: function () {
        $.ajax({
            type: "GET",
            url: "/Catalog/ProductJson",
            data: {
                articul: this.modelData.itemFull.descriptor.articul,
                catalogId: this.modelData.itemFull.catalogId
            },
            dataType: "json",
            success: function (data, textStatus, jqXHR) {
                this.modelData = data;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                location.reload();
            }
        });
    },

    getSizes: function () {
        return this.modelData.itemFull.sizes;
    },

    getColorsBySizeKey: function (size) {
        return _.find(this.modelData.itemFull.catalogItems, x => x.sizeValue === size).colors;
    },  

    getSelectedCatalogItem: function(sizeKey, colorId) {
        return _.find(_.find(this.modelData.itemFull.catalogItems, x => x.sizeValue === sizeKey).items, x => x.color.id === colorId);
    },

    checkMaxAmount: function() {
        return this.modelData.itemFull.checkMaxAmount;
    }
}