/*
 * Code that integrates lightbox2.js library with simple-image-inner-zoom.js library
 */

$(function () {
    var lightbox = $("#lightbox");
    var lightboxOverlay = $("#lightboxOverlay");
    var lightboxContainer = lightbox.find(".lb-container");
    var lightboxImage = lightboxContainer.find(".lb-image");
    var navContainer = lightboxContainer.find(".lb-nav");
    var navPrev = $(navContainer).find(".lb-prev");
    var navNext = $(navContainer).find(".lb-next");

    var zoomContainerHtml = "<div class='zoomed-container'><div class='zoomed-wrapper'><div class='zoom'></div></div></div>";
    var zoomWrapperSelector = ".zoomed-wrapper";
    var zoomContainerSelector = ".zoomed-container";
    var zoomSelector = ".zoom";
    var overflowWidth = 768;
    var zoomMagnifyWithOverflow = 1.5;
    var zoomMagnifyDefault = 1.2;

    lightboxOverlay.on("fadeOutCompleted", onLightboxFadeOutCompleted); // uses custom event defined in the core/jquery-extension.js
    lightboxImage.on("load", imageLoaded);


    function onLightboxFadeOutCompleted(event) {
        removeZoom(navContainer);
    }

    function imageLoaded(event) {
        if (lightbox.is(":visible")) {
            addZoom(navContainer);
        }
    }

    function addZoom(element) {
        removeZoom(element);
        element.append(zoomContainerHtml);
        var zoomWrapper = element.find(zoomWrapperSelector);
        zoomWrapper.zoom({
            on: "click",
            url: lightboxImage.attr("src"),
            target: zoomSelector,
            magnify: getZoomMagnify(),
            onInit: zoomInit,
            onZoomIn: zoomedIn,
            onZoomOut: zoomedOut
        });
    }

    function removeZoom(element) {
        element.find(zoomWrapperSelector).trigger("zoom.destroy");
        element.find(zoomContainerSelector).remove();
    }

    function zoomInit() {
        setZoomContainerFixed(navContainer);
    }

    function zoomedIn() {
        navContainer.find(zoomSelector).addClass("active");
        navPrev.hide();
        navNext.hide();
    }

    function zoomedOut() {
        navContainer.find(zoomSelector).removeClass("active");
        resetZoomContainer(navContainer);
        navPrev.show();
        navNext.show();
    }

    function setZoomContainerFixed(parent) {
        if (isOverflowEnabled()) {
            var zoomContainer = parent.find(zoomContainerSelector);
            zoomContainer.css("position", "fixed");
        }
    }

    function resetZoomContainer(parent) {
        if (isOverflowEnabled()) {
            var zoomContainer = parent.find(zoomContainerSelector);
            zoomContainer.find("img").hide();
            zoomContainer.css("position", "absolute");
        }
    }

    function isOverflowEnabled() {
        return window.innerWidth > overflowWidth;
    }

    function getZoomMagnify() {
        var magnify = isOverflowEnabled() ? zoomMagnifyWithOverflow : zoomMagnifyDefault;
        return magnify;
    }
});