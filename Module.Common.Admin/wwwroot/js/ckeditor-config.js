$(document).ready(function () {
    var areas = $('.html-editor');

    for(var i=0; i<areas.length; i++) {
        var id = $(areas[i]).prop("id");

        CKEDITOR.timestamp = $("#ckeditor-scripts-version").val();
        CKEDITOR.dtd.$removeEmpty.span = false;
        CKEDITOR.replace(id, {
            allowedContent: true,
            extraAllowedContent: '*(*)',
            height: 400,
            filebrowserBrowseUrl: '/ckeditor/browse_file',
            filebrowserUploadUrl: '/ckeditor/upload_file',
            fillEmptyBlocks: function(element) {
                var parent = element.getAscendant(function(parentElement) {
                    if (parentElement.attributes !== undefined && parentElement.attributes !== null && parentElement.attributes['class'] !== undefined) {
                        if (parentElement.attributes['class'].indexOf('allow-empty') !== -1)
                            return true;
                    }

                    return false;
                });

                if(parent !== null)
                    return false;
            }
        });
    }
});