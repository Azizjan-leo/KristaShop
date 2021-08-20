$("#RegCityId").on("change", function () {
    updateCityInputs(this);
});

$("#register").on("change", "#RegCityId", function() {
    updateCityInputs(this);
});

$("#register").on("ready", ".ajax-post-form", function () {
    updateCityInputs($(this).find("#RegCityId")[0]);
});

function updateCityInputs(caller) {
    var form = $(caller).parents("form");
    var isNewCity = Boolean.parse($(caller.selectedOptions[0]).attr("data-new-city"));
    if (isNewCity) {
        form.find("#IdentityNewCity").show();
        form.find("#RegNewCity").val("");
    } else {
        form.find("#IdentityNewCity").hide();
        form.find("#RegNewCity").val("");
    }
}