// Отправка формы в модальном окне через ajax
// Вызывающий элемент (кнопка/ссылка) должна содержать 2 параметра: 
//      data-toggle="ajax-modal"
//      data-target="#{id контейнера}" (например: data-target="#feedbackModal)
//      data-target-tab="#{id ссылки на tab}" - опциональный атрибут, если присутствует, то открывается вклада на которую он ссылается, если нет, то открывается первая вкладка
//      data-callback="[functionName]", фукция должна принимать (isSuccess)
// контейнер указанный в data-target должен содержать bootstrap классы для модальных окон
// код расчитан на модальное окно с табами
// у каждого таба должен быть атрибут data-url="" со ссылкой на /controller/action, который возвращает PartialView с формой
// форма должна быть обернута в div с классом ajax-post-form
// а инпуты формы должны быть обернуты в div с классом ajax-post-form-body

$(function () {
    $('a[data-toggle="ajax-modal"]').click(handleAjaxModalWindow);
    $('button[data-toggle="ajax-modal"]').click(handleAjaxModalWindow);

    if (typeof (ShowRegistrationForm) != 'undefined' && ShowRegistrationForm === true) {
        var btn = $(".jsShowRegiatrationForm");
        if (btn.length > 0) {
            btn.click();
        }
    } else {
        if (typeof (showLoginFormFlag) != 'undefined' && showLoginFormFlag) {
            if ($(".jsTopLoginButton").length > 0) {
                $(".jsTopLoginButton").click();
            }
        }
    }

    var modalWindow = undefined;

    function handleAjaxModalWindow(event) {
        modalWindow = $(this).attr("data-target");
        var toggles = $(modalWindow).find("[data-toggle='tab']");
        setActiveTab(toggles, $(this).attr("data-target-tab"));
        for (var i = 0; i < toggles.length; i++) {
            var placeholder = {
                //url: $(toggles[i]).data("url"),
                url: $(toggles[i]).attr("data-url"),
                //callback: $(toggles[i]).data("callback"),
                callback: $(toggles[i]).attr("data-callback"),
                placeholder: $(toggles[i]).attr("href")
            };
            updatePlaceholders(placeholder);
            $(modalWindow).modal("show");
        }
    }

    function setActiveTab(tabs, tabSelector) {
        if (tabSelector == undefined || tabSelector === "") {
            tabs.first().click();
            return;
        }
        var tab = tabs.filter(tabSelector);
        if (tab) {
            tabs.removeClass("active");
            $(tab).click();
        }

    }

    function updatePlaceholders(placeholder) {
        $.get(placeholder.url).done(function (data) {
            $(placeholder.placeholder).html(data);
            $(placeholder.placeholder).off("submit");
            $(placeholder.placeholder).on("submit", "form", function (event) {submitAjaxForm(event, this, placeholder)});
            $(placeholder.placeholder).find(".ajax-post-form").trigger("ready");
            addRecapcha(placeholder.placeholder, "#captchaInput");
        });
    }

    function submitAjaxForm(event, caller, placeholder) {
        event.preventDefault();
        event.stopPropagation();

        var formWrapper = $(caller).parents(".ajax-post-form");
        var form = formWrapper.find("form");

        var submitButton = form.find('button[type="submit"]');
        $(submitButton).prop("disabled", true);

        var actionUrl = form.attr("action");
        var dataToSend = new FormData(form[0]);

        $.validator.unobtrusive.parse(formWrapper);
        if (form.valid()) {
            $.ajax({
                method: "POST",
                url: actionUrl,
                data: dataToSend,
                cache: false,
                contentType: false,
                processData: false,
                statusCode: {
                    '201': function (jqXHR, responseType, type) {
                        showAlert(jqXHR.operationResult);
                        $(modalWindow).modal("hide");
                    },
                    '206': function (jqXHR, responseType, type) {
                        showAlert(jqXHR);
                        $(modalWindow).modal("hide");
                    },
                    '302': function (jqXHR, responseType, type) {
                        window.location = jqXHR.responseJSON.location;
                    }
                },
                success: function (data, r, t) {
                    var newBody = $(".ajax-post-form-body", data);
                    formWrapper.find(".ajax-post-form-body").replaceWith(newBody);
                    $(submitButton).prop("disabled", false);

                    var isValid = Boolean.parse(newBody.find('[name="IsValid"]').val()) === true;
                    if (isValid) {
                        $(modalWindow).modal("hide");
                    }
                    invokeCallback(placeholder.callback, true);
                },
                error: function (jqXHR, exception) {
                    if (jqXHR.status >= 400) {
                        if (jqXHR.status == 401)
                            window.location.reload();
                        showGenericNotificationError();
                        $(modalWindow).modal("hide");
                    }
                    invokeCallback(placeholder.callback, false);
                }
            });
            $(formWrapper).trigger("ready");
        } else {
            $(submitButton).prop("disabled", false);
        }
    }

    
    function invokeCallback(callback, isSuccess) {
        if (callback == undefined) return;

        const invoke = eval(callback);
        if (typeof invoke == "function") {
            invoke(isSuccess);
        }
    }
});