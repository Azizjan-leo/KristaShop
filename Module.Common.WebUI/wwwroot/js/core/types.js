// Parse boolean
var falsy = /^(?:f(?:alse)?|no?|0+)$/i;
Boolean.parse = function (val) {
    return !falsy.test(val) && !!val;
};

RegExp.removeNonNumbers = function (value){
    return value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1');
}

$(document).on("click", "[data-generate-password]", function() {
    var pass = Math.random().toString().substring(2, 8);
    var target = this.dataset.generatePassword;
    $(target).val(pass);
});

function currencyFormatConvert(num) {
    return (
        num
            .toFixed(2) // always two decimal digits
            .replace('.', ',') // replace decimal point character with ,
            .replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.')
            .replace('.', ' ') // replace decimal point character with ,
            .replace(',', '.') // replace decimal point character with ,
    );
}

Number.toTwoDecimalPlaces = currencyFormatConvert;

String.isNullOrEmpty = function(aString) {
    return aString === "" || aString === null || aString === undefined;
}

JSON.toFunctionParameter = function(anObject) {
    return JSON.stringify(anObject).split('"').join("&quot;");
}

Object.deepCopy = function (anObject){
    return JSON.parse(JSON.stringify(anObject));
}

Date.BasicDateFormat = "DD.MM.YYYY";
Date.BasicDateTimeFormat = "DD.MM.YYYY HH:mm";

String.ConvertDataAccessType = function(aIntVal) {
    if (aIntVal === 1) {
        return "Свои";
    } else if (aIntVal === 2) {
        return "Все";
    }

    return "Не установлено";
}

File.saveFromBase64 = function (fileName, fileType, aBase64String) {
    saveByteArray(fileName, fileType, base64ToArrayBuffer(aBase64String));
}

function base64ToArrayBuffer(base64) {
    const binaryString = window.atob(base64);
    const bytes = new Uint8Array(binaryString.length);
    for (let i = 0; i < binaryString.length; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }
    return bytes;
}

function saveByteArray(fileName, fileType, byteArray) {
    const blob = new Blob([byteArray], {type: `application/${fileType}`});
    const link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    link.download = fileName;
    link.click();
}

function stopPropagation(event) {
    event.stopPropagation();
}