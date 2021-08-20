CKEDITOR.dialog.add('contentWithFullWidthBackgroundImageDialog', function(editor) {
        return {
            title: editor.lang.kristashop.bgImageTitle,
            resizable: CKEDITOR.DIALOG_RESIZE_BOTH,
            minWidth: 500,
            minHeight: 200,
            onShow: function() {
                var blockWithBg = editor.getSelection().getStartElement();
                if (blockWithBg.hasClass("background")) {
                    var bgValue = blockWithBg.getStyle("background-image").replace(/^url\(['"](.+)['"]\)/, '$1');

                    var dialog = this;
                    dialog.setValueOf('tab1', 'imageURL', bgValue);
                }
            },
            onOk: function() {
                var dialog = this;
                var imageURL = dialog.getValueOf('tab1', 'imageURL');

                var blockWithBg = editor.getSelection().getStartElement();
                if (blockWithBg.hasClass("background")) {
                    blockWithBg.setStyle("background-image", `url(${imageURL})`);
                } else {
                    var container = $(".content-with-fullscreen-image-wrapper");
                    container.find(".background").css("background-image", `url(${imageURL})`);
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