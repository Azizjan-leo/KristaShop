jQuery(function ($) {
    /*
     * Rewrite jQuery fadeOut function to add 2 custom events:
     * - fadeOutStarted - invoke when fadeOut function called
     * - fadeOutCompleted - invoke when fadeOut function worked and complete callback called
     */

    var fadeOutCompleted = "fadeOutCompleted";
    var fadeOutStarted = "fadeOutStarted";
    var oldFadeOutFunction = $.fn.fadeOut;

    $.fn.fadeOut = function (duration, easing, complete) {
        return $(this).each(function () {
            var obj = $(this),
                newCompleteCallback = function () {
                    if ($.isFunction(complete)) {
                        complete.apply(obj);
                    }

                    // trigger an event after fadeOut completed and base callback called
                    obj.trigger(fadeOutCompleted);
                };

            // trigger an event before fadeOut started
            obj.trigger(fadeOutStarted);

            // use the old function to show the element passing the new callback
            oldFadeOutFunction.apply(obj, [duration, easing, newCompleteCallback]);
        });
    }
});