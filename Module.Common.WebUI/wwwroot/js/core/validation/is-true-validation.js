$.validator.addMethod("istrue", function(value, element, params) {
    var result = Boolean(value) === true;
    return result;
});

$.validator.unobtrusive.adapters.addBool("istrue", "required");