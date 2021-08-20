CKEDITOR.plugins.add( 'kristashop', {
    lang:["ru", "en"],
    init: function( editor ) {

        editor.addContentsCss("/common/css/min/ckeditor-custom.min.css");
        editor.addContentsCss("/common/lib/twitter-bootstrap/css/bootstrap.min.css");

        editor.addCommand("contentWithFullWidthBgImgDialog",new CKEDITOR.dialogCommand("contentWithFullWidthBackgroundImageDialog"));
        CKEDITOR.dialog.add("contentWithFullWidthBackgroundImageDialog", `${this.path}dialog/content-fullwidth-bg-image-dialog.js`);

        editor.addCommand("contentWithImgDialog",new CKEDITOR.dialogCommand("contentWithImageDialog"));
        CKEDITOR.dialog.add("contentWithImageDialog", `${this.path}dialog/content-with-image-dialog.js`);

        const containerGroups = [
            {
                name: editor.lang.kristashop.containers,
                containers: [
                    initSocialsContainer(editor),
                    initPersonContainer(editor),
                    initGradientBackground(editor),
                    // initFullWidthBgImage(editor),
                    initContainerWidthImage(editor),
                    initPartnersMapContainer(editor),
                    initContactsContainer(editor)
                ]
            },
            {
                name: editor.lang.kristashop.textStyles,
                containers: [
                    initGradientText(editor),
                ]
            }
        ]

        iterateContainerGroups(function (container){
            editor.addCommand(container.name, { exec: function( editor ) { container.execute(editor); } });
        })        
        
        editor.ui.addRichCombo("KristaStyles",
            {
                label: editor.lang.kristashop.kristaStyles,
                title: editor.lang.kristashop.kristaStylesTitle,
                toolbar: "insert",
                panel: {
                    css: [CKEDITOR.skin.getPath("editor")].concat(editor.config.contentsCss),
                    multiSelect: false,
                    attributes: { 'aria-label': editor.lang.kristashop.kristaStyles }
                },
                init: function(e) {
                    const uiEditor = this;
                    iterateContainerGroups(function (container) {
                        uiEditor.add(container.name, container.labelHtml, container.title);
                    });
                },
                onClick: function(value) {
                    editor.focus();
                    editor.fire("saveSnapshot"); // for undo

                    editor.execCommand(value);

                    editor.fire("saveSnapshot");
                }
            });
        
        function iterateContainerGroups(callback){
            for(var containerGroup of containerGroups) {
                for(var container of containerGroup.containers){
                     callback(container);
                }
            }
        }
    }
});

function initSocialsContainer(editor) {
    return  {
        name : "socialsContainer",
        labelHtml: editor.lang.kristashop.socials,
        title: editor.lang.kristashop.socialsCardTitle,
        execute: function(editor) {
            var html = $(".social-networks-wrapper").html();
            editor.insertHtml( html );
        }
    }
}

function initPersonContainer(editor) {
    return {
        name: "personContainer",
        labelHtml: editor.lang.kristashop.managerCard,
        title: editor.lang.kristashop.managerCard,
        execute: function(editor) {
            var html = $(".person-card-wrapper").html();
            editor.insertHtml(html);
        }
    }
}

function initGradientBackground(editor) {
    return {
        name: "gradientBackground",
        labelHtml: editor.lang.kristashop.gradientBg,
        title: editor.lang.kristashop.gradientBg.Title,
        execute: function(editor) {
            editor.insertHtml("<div class='bg-gradient-revert'></div>");
        }
    }
}

function initFullWidthBgImage(editor) {
    return {
        name: "fullWidthImage",
        labelHtml: editor.lang.kristashop.containerWithBgImage,
        title: editor.lang.kristashop.containerWithBgImageTitle,
        execute: function(editor) {
            editor.execCommand('contentWithFullWidthBgImgDialog');
        }
    }
}

function initContainerWidthImage(editor) {
    return {
        name: "fullWidthImage",
        labelHtml: editor.lang.kristashop.containerWithImage,
        title: editor.lang.kristashop.containerWithImageTitle,
        execute: function(editor) {
            editor.execCommand('contentWithImgDialog');
        }
    }
}

function initGradientText(editor) {
    return {
        name: "gradientText",
        labelHtml: editor.lang.kristashop.gradientText,
        title: editor.lang.kristashop.gradientTextTitle,
        execute: function(editor) {
            var element = editor.getSelection().getCommonAncestor();
            var parent = element.getAscendant(function(parentElement) {
                if (parentElement.type === 1) {
                    return parentElement.hasClass("text-gradient-main");
                }
                return false;
            });

            if (parent != null) {
                parent.removeClass("text-gradient-main");
            } else {
                var selectedText = editor.getSelection().getSelectedText();
                var newElement = new CKEDITOR.dom.element("span");
                newElement.addClass("text-gradient-main");
                newElement.setText(selectedText);
                editor.insertElement(newElement);
            }
        }
    }
}

function initPartnersMapContainer(editor) {
    return {
        name: "partnersMapContainer",
        labelHtml: editor.lang.kristashop.partnersMapTitle,
        title: editor.lang.kristashop.partnersMapTitle,
        execute: function(editor) {
            const html = $(".partners-map-wrapper").html();
            editor.insertHtml(html);
        }
    }
}

function initContactsContainer(editor) {
    return {
        name: "contactsContainer",
        labelHtml: editor.lang.kristashop.contactsTitle,
        title: editor.lang.kristashop.contactsTitle,
        execute: function(editor) {
            const html = $(".contacts-container-wrapper").html();
            editor.insertHtml(html);
        }
    }
}