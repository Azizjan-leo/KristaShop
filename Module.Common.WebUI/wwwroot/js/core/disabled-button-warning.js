/*
 *  Показывать предупреждение по клику на disabled кнопку
 *  Кнопка должна содержать следующие атрибуты:
 *  data-disabled-show-onclick - selector контейнера который будет отображаться по клику на disabled кнопку
 *  data-disabled-hide-byclick - selector элемента по клику на который контейнер из data-disabled-show-onclick будет скрыт
 *  data-disabled-reload-byclick - selector элемента по клику на который все кнопки активируются заново
 *  data-disabled-linked - может содержать любое значение, если несколько кнопок связаны между собой, все кнопки должны содержать этот атрибут с одинаковым значением
 */
$(function() {
    var disabledButtons = undefined;
    var hoverHtml = '<div class="disabled-button-hover" style="position: absolute; width: 100%; z-index: 999;"></div>';
    var hoverSelector = ".disabled-button-hover";

    load();

    function load() {
        disabledButtons = $("[data-disabled-target]");
        disabledButtons.each(function(index) {
            var data = extractData(this);
            var button = $(this);

            addButtonHover(button, data.target);
            addHoverRemover(data.hoverRemover, button, data.target, data.link);
            addDisabledReloader(data.reloader);
        });
    }

    function unload() {
        disabledButtons.each(function(index) {
            var data = extractData(this);
            var button = $(this);

            removeButtonHover(button, data.target);
            removeHoverRemover(data.hoverRemover);
            removeDisabledReloader(data.reloader);
        });
    }

    function reload() {
        unload();
        load();
    }

    function extractData(button) {
        var target = $(document).find(button.dataset.disabledTarget); // target container to show on click
        var hoverRemover = { // container that calls remove hover
            remover: $(document).find(button.dataset.disabledHoverRemover),
            selector: button.dataset.disabledHoverRemover
        };
        var reloader = $(document).find(button.dataset.disabledReloader);
        var link = button.dataset.disabledLinked;

        var data = {
            target: target,
            hoverRemover: hoverRemover,
            reloader: reloader,
            link: link
        }
        return data;
    }

    function addButtonHover(button, target) {
        var parent = button.parent();
        parent.append(hoverHtml); // add hover to disabled button
        
        var hover = parent.find(hoverSelector);
        hover.css({ "min-height": parent.height() });
        hover.on("click.disabled", function(event) {
            buttonHoverClicked(event, target);
        });
    }

    function removeButtonHover(button, target) {
        target.hide();

        var hover = button.parent().find(hoverSelector);
        hover.off(".disabled");
        hover.remove();
    }

    function addHoverRemover(hoverRemover, button, target, link) {
        $(document).on("click.disabled", hoverRemover.selector, function(event) {
            hoverHideClicked(event, button, target, link);
        });
    }

    function removeHoverRemover(hoverRemover) {
        hoverRemover.remover.off(".disabled");
    }

    function addDisabledReloader(reloader) {
        if (reloader == undefined) return;

        reloader.on("click.disabled", function(event) { reload(); });
    }

    function removeDisabledReloader(reloader) {
        if (reloader == undefined) return;

        reloader.off(".disabled");
    }

    function buttonHoverClicked(event, target) {
        target.show();
    }

    function hoverHideClicked(event, button, target, link) {
        target.hide();
        button.parent().find(hoverSelector).hide();

        hideLinkedHovers(link);
    }

    function hideLinkedHovers(link) {
        if (link == undefined || link === "") return;

        var links = disabledButtons.filter(`button[data-disabled-linked='${link}']`);
        links.each(function() {
            var hover = $(link).parent().find(hoverSelector);
            hover.hide();
        });
    }
});