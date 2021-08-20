/*
 *
 *  For this script to work input and image placeholder should be in container with class "image-upload"
 *  File input should have data attributes:
 *  data-img-uploader - makes script to work, it's value should be image container selector (for example data-img-uploader="#img")
 *  data-img-uploader-btn - button to trigger open file dialog
 */

$(function() {
    const input = $(document).find("input[data-img-uploader]");
    const button = $(document).find("[data-img-uploader-btn]");
    input.on("change", function(event) { getImageByUrl(this); });
    button.on("click", function(event) { input.trigger("click"); });

    function getImageByUrl(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var img = $(input).closest(".image-upload").find(input.dataset.imgUploader);
                img.attr("src", e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    };

    // old version

    $("#Image").change(function () { readURL(this); });

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#Image-img').attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    };
});
