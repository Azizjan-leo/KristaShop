function ChangeVideo() {
    var videoUrl = $("#VideoUrl").val();
    var correctUrl = videoUrl.replace("watch?v=", "embed/");
    $("#VideoUrl").val(correctUrl);
    document.getElementById("videoFrame").src = correctUrl;
}


$('.custom-file-input').change(function (e) {
    var files = [];
    for (var i = 0; i < $(this)[0].files.length; i++) {
        files.push($(this)[0].files[i].name);
    }
    $(this).next('.custom-file-label').html(files.join(', '));
});

function updateModelVisibility(target, articul, modelId, sizeValue, sizeLine, colorId, catalogId) {
    debugger;
    $.ajax({
        type: "POST",
        url: '/Admin/ModelsCatalog/UpdateModelVisibility',
        data: {
            articul: articul,
            modelId: modelId,
            sizeValue: sizeValue,
            sizeLine: sizeLine,
            colorId: colorId,
            catalogId: catalogId,
            isVisible: $(target).prop("checked")
        },
        dataType: "json"
    }).done(function (responseText) {
        ShowAjaxSuccessMessage(responseText);
    }).fail(function (jqXHR) {
        AjaxErrorHandler(jqXHR);
    });
}