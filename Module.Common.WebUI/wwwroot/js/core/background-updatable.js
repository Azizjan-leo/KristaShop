$(function() {
    /*
     *  Settings:
     * data-updatable = "true/false" - parent container
     * data-update = "always/once"
     * data-route = "/Controller/Action?awd=awd"
     * data-update-callback = func(updatedItem, value)
     */

    var updateAttribute = "data-updatable";
    var nameAttribute = "data-update-name";
    var callbackAttribute = "data-update-callback";
    var routeAttribute = "data-route";
    var updateTypeAttribute = "data-update";
    var intervalAttribute = "data-interval";

    var dataUpdatable = $(`[${updateAttribute}='true']`);
    dataUpdatable.each(function(index) {
        var container = $(this);
        var data = {
            route: container.attr(routeAttribute),
            updateType: container.attr(updateTypeAttribute),
            interval: container.attr(intervalAttribute),
            fields: container.find(`[${nameAttribute}]`),
            callback: container.attr(callbackAttribute)
        }

        loadData(data);

        if (data.updateType === "once") {
            container.attr(updateAttribute, "false");
        } else {
            setInterval(function() { loadData(data); }, data.interval * 1000);
        }
    });


    function loadData(item) {
        $.ajax({
            method: "GET",
            url: item.route,
            success: function (data) {
                item.fields.each(function() {
                    var name = $(this).attr(nameAttribute);
                    $(this).html(data[name]);
                    invokeCallback(item.callback, this, data[name]);
                });
            },
            error: function (jqXHR, exception) {
                if (jqXHR.status >= 400) {
                    if (jqXHR.status == 401)
                        window.location.reload();
                    showGenericNotificationError();
                }
            }
        });
    }

    function invokeCallback(callback, field, value) {
        if (callback != "") {
            var invoke = eval(callback);
            invoke(field, value);
        }
    }
});