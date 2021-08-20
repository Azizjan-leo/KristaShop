/*
 *  This file uses sweetalert2 library for showing notifications
 */

// deprecated: show alert is deprecated and will be removed soon
function showAlert(alert) {
    var htmlText = "";
    alert.messages.forEach(element => htmlText += element + "<br>");
    const toast = swal.mixin({
        toast: true,
        position: 'top-end',
        showConfirmButton: false,
        timer: 10000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer);
            toast.addEventListener('mouseleave', Swal.resumeTimer);
        }
    });

    toast.fire({
        icon: alert.alertType,
        title: htmlText
    });
}

const Toast = swal.mixin({
    toast: true,
    position: 'top-end',
    padding: '1.5rem',
    showConfirmButton: false,
    timer: 5000,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer);
        toast.addEventListener('mouseleave', Swal.resumeTimer);
    }
});

function showNotification(alertType, alertHtml) {
    Toast.fire({
        icon: alertType,
        title: alertHtml
    });
}

function showOperationResultNotification(operationResult) {
    if (operationResult == "" || operationResult == undefined) {
        return false;
    }

    var alertHtml = "";
    operationResult.messages.forEach(element => alertHtml += element + "<br>");
    showNotification(operationResult.alertType, alertHtml);
    return true;
}

function showOperationResultNotificationFromString(jsonString) {
    if (jsonString == "" || jsonString == undefined) {
        return false;
    }

    const operationResult = JSON.parse(jsonString);
    showOperationResultNotification(operationResult);
    return true;
}

function showGenericNotificationError() {
    Toast.fire({
        icon: "error",
        title: "Произошла ошибка во время выполнения операции"
    });
}

function showNotificationError(message) {
    Toast.fire({
        icon: "error",
        title: message
    });
}

function showNotificationSuccess(message) {
    Toast.fire({
        icon: "success",
        title: message
    });
}

const Notification = {};
const kristaNotificationIconHtml = "<img src='/common/img/svg/krista-logo-gradient.svg' width='95' height='48' alt='krista-logo' class='mr-2 pr-1'><img src='/common/img/svg/krista-pic-transparent-sm.gif' width='50' height='42' alt='krista-logo'>";

Notification.successModal = function(options) {
    return Swal.fire({
        icon: "question",
        iconHtml: kristaNotificationIconHtml,
        title: options.title,
        text: options.text,
        showCancelButton: false,
        cancelButtonText: "",
        showConfirmButton: true,
        confirmButtonText: "Закрыть",
        buttonsStyling: false,
        customClass: {
            container: "mt-0",
            popup: "py-4 px-3 px-lg-5",
            icon: "border-0 mt-2 mb-4",
            title: "header-font h4-smaller text-uppercase text-dark text-bold mb-3",
            content: "main-font h2 px-0 px-lg-1 pb-3 mx-0 mb-0",
            confirmButton: "btn-basic btn-basic-big btn-main-gradient m-1",
            actions: "my-3 pb-1"
        },
    });
}