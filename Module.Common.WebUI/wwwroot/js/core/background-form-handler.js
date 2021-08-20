/*
 * Для работы формы в фоновом режиме нужно обернуть форму в div с классом ajax-background-form
 * у div с классом ajax-background-form могут быть следующие атрибуты:
 * data-message-success="[value]";
 * data-message-error="[value]";
 * data-use-response="[True/False]";
 * data-success-callback="[functionName]";
 * data-include-confirmation="[True/False];
 * data-confirmation-title="[value]"
 * data-confirmation-message="[value]"
 *
 * Описание атрибутов:
 * data-message-success - определяет сообщение которое будет показано если запрос выполнился успешно
 * data-message-error - определяет сообщение которое будет показано если запрос выполнился с ошибкой
 * data-use-response-message - может принимать значения True или False, определяет, что сообщение нужно брать из response
 * data-success-callback - определяет функцию, которая будет вызвана при успешном выполнении запроса, в качестве значения принимает имя функции
 * data-include-confirmation - определяет показывать ли модальное окно подтверждения запроса или нет, может принимать значения True или False
 * data-confirmation-title - определяет заголовок модального окна подтверждения запроса
 * data-confirmation-message - определяет текст модального окна подтверждения запроса
 *
 */

var ajaxBackgroundFormSelector = ".ajax-background-form";
$(ajaxBackgroundFormSelector).on("submit", "form", submitBackgroundFormEventHandler);

function submitBackgroundFormEventHandler(event) {
    event.preventDefault();
    event.stopPropagation();

    var parent = $(this).parents(ajaxBackgroundFormSelector)[0];

    if (Boolean.parse(parent.dataset.includeConfirmation)) {
        const ajaxBackgroundForm = this;
        Swal.fire({
            title: parent.dataset.confirmationTitle,
            text: parent.dataset.confirmationMessage,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Да',
            cancelButtonText: 'Нет'
        }).then((result) => {
            if (result.isConfirmed) {
                submitBackgroundForm(ajaxBackgroundForm);
            }
        })
    } else {
        submitBackgroundForm(this);
    }
}

function invokeBackgroundFormCallback(callback, form) {
    var invoke = eval(callback);
    if (typeof invoke == "function") {
        invoke(form);
    }
}

function submitBackgroundForm(form) {
    var parent = $(form).parents(ajaxBackgroundFormSelector)[0];
    var actionUrl = $(form).attr("action");
    var dataToSend = new FormData(form);

    $.validator.unobtrusive.parse(parent);
    if ($(form).valid()) {
        $.ajax({
            method: "POST",
            url: actionUrl,
            data: dataToSend,
            cache: false,
            contentType: false,
            processData: false,
            statusCode: {
                '302': function (jqXHR, responseType, type) {
                    window.location = jqXHR.responseJSON.location;
                }
            },
            success: function (data, textStatus, jqXHR) {
                if (Boolean.parse(parent.dataset.useResponseMessage)) {
                    showOperationResultNotification(data);
                } else {
                    var result = parent.dataset.messageSuccess;
                    showNotificationSuccess(result);
                }
                invokeBackgroundFormCallback(parent.dataset.successCallback, parent);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (jqXHR.status == 401)
                    window.location.reload();
                if (Boolean.parse(parent.dataset.useResponseMessage)) {
                    showOperationResultNotification(jqXHR.responseJSON);
                } else {
                    var result = parent.dataset.messageError;
                    showNotificationError(result);
                }
            }
        });
    } else {
        showNotificationError("Невозможно выполнить операцию. Форма заполнена неверно");
    }
}