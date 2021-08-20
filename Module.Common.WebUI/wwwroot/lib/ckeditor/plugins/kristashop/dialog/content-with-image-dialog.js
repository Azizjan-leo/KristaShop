CKEDITOR.dialog.add('contentWithImageDialog', function(editor) {
        return {
            title: editor.lang.kristashop.bgImageTitle,
            resizable: CKEDITOR.DIALOG_RESIZE_BOTH,
            minWidth: 500,
            minHeight: 200,
            onShow: function() {
                var blockWithBg = editor.getSelection().getStartElement();
                if (blockWithBg.hasClass("image")) {
                    var src = blockWithBg.getAttribute("src");
                    var index = src.indexOf("?");
                    var imgSrc = "";
                    var maxWidth = 0;
                    if (index < 0) {
                        imgSrc = src;
                    } else {
                        imgSrc = src.substring(0, index);
                        maxWidth = src.substring(src.substring(index).indexOf("="));
                    }

                    var dialog = this;
                    dialog.setValueOf('tab1', 'imageURL', imgSrc);

                    if (maxWidth > 0) {
                        dialog.setValueOf('tab1', 'maxWidth', maxWidth);
                    }
                }
            },
            onOk: function() {
                var dialog = this;
                var imageURL = dialog.getValueOf('tab1', 'imageURL');

                var blockWithBg = editor.getSelection().getStartElement();
                if (blockWithBg.hasClass("image")) {
                    $(blockWithBg).attr("src", imageURL);
                    $(blockWithBg).attr("data-cke-saved-src", `${imageURL}?width=1200`);
                } else {
                    var container = $(".content-with-image-wrapper");
                    container.find(".image").attr("src", imageURL);
                    editor.insertHtml(container.html());
                }
            },
            contents: [
                {
                    id: 'tab1',
                    label: editor.lang.kristashop.bgImageTitle,
                    title: "",
                    accessKey: 'Q',
                    elements: [
                        {
                            type: 'vbox',
                            padding: 0,
                            children: [
                                {
                                    type: 'hbox',
                                    widths: ['280px', '100px;vertical-align: middle;'],
                                    align: 'right',
                                    styles: '',
                                    children: [
                                        {
                                            type: 'text',
                                            label: editor.lang.kristashop.imageUrl,
                                            id: 'imageURL'
                                        }, {
                                            type: 'button',
                                            id: 'browse',
                                            label: editor.lang.common.browseServer,
                                            hidden: true,
                                            filebrowser: 'tab1:imageURL'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            type: 'vbox',
                            padding: 0,
                            children: [
                                {
                                    type: 'hbox',
                                    align: 'left',
                                    styles: '',
                                    children: [
                                        {
                                            type: 'text',
                                            id: 'maxWidth',
                                            label: 'Сжать по ширине до:',
                                            default: '1200'
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    id: 'Upload',
                    hidden: true,
                    filebrowser: 'uploadButton',
                    label: "Загрузить",
                    elements: [ {
                            type: 'file',
                            id: 'upload',
                            label: "Загрузить на сервер",
                            style: 'height:40px',
                            size: 38
                        },
                        {
                            type: 'fileButton',
                            id: 'uploadButton',
                            filebrowser: 'info:txtUrl',
                            label: editor.lang.image.btnUpload,
                            'for': [ 'Upload', 'upload' ]
                        } ]
                }
            ]
        }
    });