if (typeof (CKEDITOR) != "undefined") {
    CKEDITOR.editorConfig = function (config) {
        config.language = 'vi';
        config.height = "450px";
        config.toolbarCanCollapse = true;
        //config.toolbarStartupExpanded = false;
        config.extraPlugins = 'youtube,dvs.imagev2,dvs.video1,dvs.boxtext,dvs.boxphoto'; //widget,filetools,notificationaggregator,notification,widgetselection,lineutils,uploadwidget,uploadimage
        config.toolbar = [
            ['Sourcedialog','Bold', 'Italic', 'Underline', 'TextColor'],
            ['FontSize'],
            ['NumberedList', 'BulletedList', 'JustifyLeft', 'JustifyCenter', 'JustifyRight'],
            ['Youtube', 'dvs.imagev2', 'dvs.video1', 'Table', 'Link', 'ShowBlocks', 'Preview']
        ];
        config.allowedContent = true;
        config.uploadUrl = '/FileManager/Handler/CKEditorUpload.ashx';

        config.pasteFromWordRemoveFontStyles = false;
        config.pasteFromWordRemoveStyles = false;
    };
}
