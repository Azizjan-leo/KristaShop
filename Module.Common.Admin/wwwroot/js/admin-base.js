//#region Trigger right filter sidebar
$("[data-toggle='toggle-collapse']").click(function () {
    const selector = $(this).data("target");
    $(selector).toggleClass("in");
});

$(".sidebar-content-hover").click(function() {
    $(this).prev().removeClass("in");
});
//#endregion Trigger right filter sidebar

function topDashboardUpdated(item, value) {
    if (+value > 0) {
        $(item).css("display", "inline-block");
    } else {
        $(item).css("display", "none");
    }
}

function AjaxErrorHandler(jqXHR) {
    try {
        if (jqXHR.status === 401)
            window.location.reload();
        if(jqXHR.status === 403) {
            showNotificationError("У вас нет доступа к выполнению данного действия");
            return;
        }
        var responseObj = jQuery.parseJSON(jqXHR.responseText);
        if (responseObj.detail !== 'undefined') {
            var responseDetailObj = jQuery.parseJSON(responseObj.detail);
            if (typeof (responseDetailObj.redirectUrl) != 'undefined') {
                location.href = responseDetailObj.redirectUrl;
            } else {
                showOperationResultNotificationFromString(responseObj.detail);
            }
        }
    } catch (ex) {
        location.reload();
    }
}

function AjaxRedirectHandler(responseText) {
    try {
        var responseObj = jQuery.parseJSON(responseText);
        if (typeof (responseObj.redirectUrl) != 'undefined') {
            location.href = responseObj.redirectUrl;
        } else {
            location.reload();
        }
    } catch (ex) {
        location.reload();
    }
}

function ShowAjaxSuccessMessage(operationResult) {
    try {
        if (typeof (operationResult.alertType) != 'undefined' && operationResult.alertType == 'success' &&
            typeof (operationResult.isSuccess) != 'undefined' && operationResult.isSuccess == true &&
            typeof (operationResult.messages) != 'undefined') {
            showOperationResultNotification(operationResult);
        }       
    } catch (ex) {

    }
}

$('.custom-file-input').on('change', function() { 
    let fileName = $(this).val().split('\\').pop(); 
    $(this).next('.custom-file-label').addClass("selected").html(fileName); 
});

function copyToClipboard(text) {
    const dummy = document.createElement("textarea");
    document.body.appendChild(dummy);
    dummy.value = text;
    dummy.select();
    dummy.setSelectionRange(0, 99999);
    document.execCommand("copy");
    document.body.removeChild(dummy);
}