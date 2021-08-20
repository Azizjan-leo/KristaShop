$(function () {
    var format = "D.MM.YYYY HH:mm";

    if (typeof flatpickr !== 'undefined') {
        flatpickr("[data-date-period-picker]", {
            locale: "ru",
            mode: "range",
            maxDate: "today",
            allowInput: true,
            dateFormat: "d.m.Y",
        });
    }

    if (typeof datetimepicker !== 'undefined') {
        $("[data-datetime-picker]").datetimepicker({
            locale: moment.locale("ru"),
            format: format,
            sideBySide: false,
            buttons: {
                showToday: true,
                showClear: true,
                showClose: true
            },
            icons: {
                time: "fas fa-clock",
                date: "fas fa-calendar",
                up: "fas fa-arrow-up",
                down: "fas fa-arrow-down",
                previous: "fas fa-chevron-left",
                next: "fas fa-chevron-right",
                today: "far fa-calendar-check",
                clear: "fas fa-trash"
            },
            tooltips: {
                today: "К текущей дате",
                clear: "Очистить",
                close: "Закрыть",
                selectTime: "Выбрать время",
                selectDate: "Выбрать дату",
                selectMonth: "Выбрать месяц",
                prevMonth: "Предыдущий месяц",
                nextMonth: "Следующий месяц",
                selectYear: "Год",
                prevYear: "Предыдущий год",
                nextYear: "Следующий год",
                selectDecade: "Декада",
                prevDecade: "Предыдущая декада",
                nextDecade: "Следующая декада",
                prevCentury: "Предыдущий век",
                nextCentury: "Следующий век",
                incrementHour: "Добавить час",
                pickHour: 'Выбрать час',
                decrementHour: 'Убрать час',
                incrementMinute: 'Добавить минуту',
                pickMinute: 'Выбрать минуту',
                decrementMinute: 'Убрать минуты',
                incrementSecond: 'Добавить секунду',
                pickSecond: 'Выбрать секунды',
                decrementSecond: 'Убрать секунду'
            }
        });
    }
});