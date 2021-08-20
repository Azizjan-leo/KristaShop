lightbox.option({
    wrapAround: true,
    albumLabel: "Изображение %1 из %2"
});

//#region Trigger right filter sidebar
$("[data-toggle='toggle-collapse']").click(function () {
    const selector = $(this).data("target");
    $(selector).toggleClass("in");
});

$(".sidebar-content-hover").click(function () {
    $(this).prev().removeClass("in");
});
//#endregion Trigger right filter sidebar

// Fix bootstrap custom file input
$(document).ready(function() {
    $(".custom-file-input").on("change", function (e) {
        console.log("custom input changed");
        var files = [];
        for (var i = 0; i < $(this)[0].files.length; i++) {
            files.push($(this)[0].files[i].name);
        }
        $(this).next(".custom-file-label").html(files.join(", "));
    });
});


// Top right notification window

var RECAPTCHA_SITE_KEY = "6LckvL4ZAAAAAFrjlGA2YXw_fdGLVpiYf--2qx3-";
function addRecapcha(placeholder, selector) {
    window.grecaptcha.execute(RECAPTCHA_SITE_KEY, { action: "home" }).then(function (token) {
        $(placeholder).find(selector).val(token);
    });
}

function SwitchCatalogFilter(btnObj) {
    var textBlockObj = $(btnObj).parents("#searchForm").find("#collapseCatalogFilter");
    if (textBlockObj.length > 0) {
        if (textBlockObj.css("display") == "block") {
            textBlockObj.css("display", "none");
            $(btnObj).find("p").text("Показать фильтр");
        } else {
            textBlockObj.css("display", "block");
            $(btnObj).find("p").text("Скрыть фильтр");
        }
    }
}

$(function() {
    const collapseHide = function () {
        const button = $(this).parent().find(`[data-target='#${$(this).attr("id")}']`);
        button.text(button[0].dataset.collapseShow);
    };

    const collapseShow = function() {
        const button = $(this).parent().find(`[data-target='#${$(this).attr("id")}']`);
        button.text(button[0].dataset.collapseHide);
    };

    function showPanelCollapse() {
        const button = $(".panel-collapse").parent().find("[data-toggle='collapse']");
        if (button[0] != undefined && button[0].dataset.collapseMax != undefined && window.innerWidth <= button[0].dataset.collapseMax) {
            $(".panel-collapse").collapse("hide");
        }
    }

    $(".panel-collapse").on("hide.bs.collapse", collapseHide);
    $(".panel-collapse").on("show.bs.collapse", collapseShow);
    showPanelCollapse();
});