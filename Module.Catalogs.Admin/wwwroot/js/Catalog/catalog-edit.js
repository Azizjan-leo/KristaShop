$(function() {
    $("[name='OrderForm']").on("change", function(event) { updateTimerField(); });

    updateTimerField();

    function updateTimerField() {
        var closeTime = $(".close-time");
        var orderForm = +$("[name='OrderForm']").val();
        if (orderForm === 2) {
             closeTime.show();
        } else {
            closeTime.hide();
        }
    }
});

function openExtraChargeModal(catalogId, extraChargeId, extraChargeType, price) {
    if (catalogId != null) { // edit extra charge
        $("#headerText").text("Редактировать наценку"); 
        $("#extraChargeType").val(Constants.extraCharges[extraChargeType]).change();
        $("#extraChargeSum").val(price);
        $("#cancelBtn").addClass("d-none");
        $("#deleteBtn").prop('data-extra-charge-id', extraChargeId);
        $("#deleteBtn").prop('data-catalog-id', catalogId);
        $("#deleteBtn").removeClass("d-none");
    }
    else { // add extra charge
        $("#headerText").text("Добавить наценку");
        $("#extraChargeType").val(0).change();
        $("#extraChargeSum").val("");
        $("#deleteBtn").addClass("d-none");
        $("#cancelBtn").removeClass("d-none");
    }
    $('#edit-extra-charge-modal').modal('toggle');
}

function addOrSetCatalogExtraCharge(catalogId) {
    var extraChargeType = $('#extraChargeType').find(":selected").val();
    var extraChargeSum = $('#extraChargeSum').val();
    if (!$.isNumeric(extraChargeSum)) {
        showNotificationError('Введите числовое значение');
        $('#extraChargeSum').focus();
        return;
    }
    if (extraChargeSum < 0) {
        showNotificationError('Введите положительное значение');
        $('#extraChargeSum').focus();
        return;
    }
    if (extraChargeType < 1) {
        showNotificationError('Выберите тип наценки');
        $('#extraChargeType').focus();
        return;
    }
    $.ajax({
        type: "POST",
        url: "/Admin/Catalog/AddOrSetCatalogExtraCharge",
        data: {
            catalogId: catalogId,
            extraChargeType: extraChargeType,
            sum: extraChargeSum
        },
        success: function (updatedCatalogs) {
            showNotificationSuccess("Наценка добавлена успешно!");
            $('#edit-extra-charge-modal').modal('toggle');
            $('#extraCharges').html(updatedCatalogs); 
        },
        error: function (jqXHR, exception) {
            if (jqXHR.status == 401)
                window.location.reload();
            $('#edit-extra-charge-modal').modal('toggle');
            showNotificationError("Ошибка при попытке добавления/изменения наценки каталога");
        }
    });
}

function deleteCatalogExtraCharge() {
    var extraChargeId = $("#deleteBtn").prop('data-extra-charge-id');
    var catalogId = $("#deleteBtn").prop('data-catalog-id');

    $.ajax({
        type: "DELETE",
        url: "/Admin/Catalog/DeleteCatalogExtraCharge",
        data: {
            catalogId: catalogId,
            extraChargeId: extraChargeId
        },
        success: function (updatedCatalogs) {
            showNotificationSuccess("Наценка успешно удалена!");
            $('#edit-extra-charge-modal').modal('toggle');
            $('#extraCharges').html(updatedCatalogs);
        },
        error: function (jqXHR, exception) {
            if (jqXHR.status == 401)
                window.location.reload();
            $('#edit-extra-charge-modal').modal('toggle');
            showNotificationError("Ошибка при попытке удаления наценки каталога");
        }
    });
}