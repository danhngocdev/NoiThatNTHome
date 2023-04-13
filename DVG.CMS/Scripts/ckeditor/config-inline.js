if (typeof (CKEDITOR) != "undefined") {
    CKEDITOR.editorConfig = function (config) {
        config.language = 'vi';
        config.height = "600px";
        config.toolbarCanCollapse = true;
        //config.toolbarStartupExpanded = false;
        config.extraPlugins = 'youtube,dvs.imagev2,dvs.video1,dvs.boxtext,dvs.boxphoto,dvs.checkspell,dvs.menuheading,iframe'; //dvs.video2,widget,filetools,notificationaggregator,notification,widgetselection,lineutils,uploadwidget,uploadimage,embed,autoembed
        config.toolbar = [
            ['Bold', 'Italic', 'Underline', 'TextColor'],
            ['Styles', 'Format', 'FontSize'],
            ['NumberedList', 'BulletedList', 'Outdent', 'Indent', 'JustifyLeft', 'JustifyCenter', 'JustifyRight'], ['dvs.menuheading'],
            '/',
            ['Youtube', 'dvs.video2', 'dvs.imagev2', 'dvs.boxtext', 'dvs.boxphoto', 'Table', 'Link', 'Sourcedialog', 'ShowBlocks', 'dvs.checkspell', 'Preview', 'Iframe'] //'Embed',
        
        ];

        //// Setup content provider. See https://docs.ckeditor.com/ckeditor4/docs/#!/guide/dev_media_embed
        //config.embed_provider = '//ckeditor.iframe.ly/api/oembed?url={url}&callback={callback}';

        //// Configure the Enhanced Image plugin to use classes instead of styles and to disable the
        //// resizer (because image size is controlled by widget styles or the image takes maximum
        //// 100% of the editor width).
        //config.image2_alignClasses = ['image-align-left', 'image-align-center', 'image-align-right'];
        //config.image2_disableResizer = true;

        config.allowedContent = true;
        config.uploadUrl = '/FileManager/Handler/CKEditorUpload.ashx';
        //Set default show outline block
        CKEDITOR.config.startupOutlineBlocks = true;
        CKEDITOR.timestamp = '20180907';

        config.pasteFromWordRemoveFontStyles = false;
        config.pasteFromWordRemoveStyles = false;
        //CKEDITOR.config.startupFocus = true;
    };
}
