$.validator.addMethod("required-this", function(value, element, params) {
    var otherValue = $(element).parents("form").find("input[name=" + params.other + "]").val();
    if ((value === "" || value == undefined) && (otherValue === "" || otherValue == undefined)) {
        return false;
    }
    
    return true;
});

$.validator.unobtrusive.adapters.add("required-this", ["orother"],
    function (options) {
        options.rules["required-this"] = {
            other: options.params.orother
        };
        options.messages["required-this"] = options.message;
    });