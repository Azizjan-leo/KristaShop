$(function () {
    $(".jsCopyLink").click(function () {
        var parentObj = $(this).parents(".jsGuestLinkWrapper");
        if (parentObj.length > 0) {
            var linkInputObj = parentObj.find(".jsGuestLink");
            if (linkInputObj.length > 0) {
                var link = linkInputObj[0];
                link.select();
                link.setSelectionRange(0, 99999);
                document.execCommand("copy");
            }
        }
    });
});