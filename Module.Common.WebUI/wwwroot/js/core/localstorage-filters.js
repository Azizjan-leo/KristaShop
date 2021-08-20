setupFilters();

function setupFilters() {
    $('.localStorageFilter').each(function () {
        setValueFromLocalStorage(this);
        var action = getEventType(this);
        $(this).on(action, setToLocalStorage);
    });
}

function setToLocalStorage(event) {
    var el = event.target;
    var key = el.baseURI + "_" + el.id;
    var value = el.getAttribute('type') == 'checkbox' ? $(el).is(':checked') :
        $(el).hasClass('date') ? $(el).find("input").get(0).value : el.value;

    window.localStorage.setItem(key, value);
}

function setValueFromLocalStorage(el) {
    var value = window.localStorage.getItem(window.location + "_" + el.id);

    if (el.getAttribute('type') == 'checkbox')
        $(el).prop('checked', Boolean.parse(value));
    else if ($(el).hasClass('date'))
        $(el).find("input").get(0).value = value;
    else
        el.value = value
}

function SetupMainSearchLocalStorage() {
    mainSearchbox = $("div.dataTables_filter").find("input[type='search']").get(0);
    mainSearchbox.setAttribute('id', 'mainSearchBox');
    $(mainSearchbox).addClass('localStorageFilter');
    $('#mainSearchBox').val(window.localStorage.getItem(window.location + "_" + 'mainSearchBox')).keyup();
    $(mainSearchbox).on('keyup', setToLocalStorage);
    $(mainSearchbox).on('search', setToLocalStorage);
}

function EmptyLocalstorageInputs() {
    $('.localStorageFilter').each(function () {
        var el = this;
        var eventType = getEventType(this);

        if (el.getAttribute('type') == 'checkbox')
            $(el).prop('checked', Boolean.parse('false')).trigger(eventType);
        else if ($(el).hasClass('date')) {
            $(el).find("input").get(0).value = '';
            $(el).trigger(eventType)
        }
        else
            $(el).val('').trigger(eventType);
    });
}

function getEventType(el) {
    return el.getAttribute('type') == 'checkbox' || $(el).is('select') ? 'change' :
            $(el).hasClass('date') ? 'change.datetimepicker' : 'keyup';
}