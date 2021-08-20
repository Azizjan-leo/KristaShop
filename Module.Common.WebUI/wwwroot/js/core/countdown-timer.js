$(function() {
    runCountdown();

    function runCountdown() {
        var countdownContainer = $("[data-countdown]");
        if (countdownContainer.length < 1) return;

        var now = new Date();
        var utcNow = now.getTime() + (now.getTimezoneOffset() * 60000);
        var shift = new Date(countdownContainer.attr("data-server-date")) - utcNow;
        countdownContainer.countdown({
            date: countdownContainer.attr("data-countdown"), // date format: 07/27/2017 17:00:00
            recoupMilliseconds: Math.abs(shift) > 5000 ? shift : 0,
            hour: "Часов",
            hours: "Часов",
            day: "Дней",
            days: "Дней",
            minute: "Минут",
            minutes: "Минут",
            second: "Секунд",
            seconds: "Секунд",
            hideOnComplete: true
        }, function (container) {
            redirectTo(countdownContainer.attr("data-redirect-oncomplete"));
        });
    }

    function redirectTo(path) {
        location.href = path;
    }
});