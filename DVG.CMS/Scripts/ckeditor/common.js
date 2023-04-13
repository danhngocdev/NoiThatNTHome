
CKEDITOR.editorConfig = function (config) {
    config.language = 'vi';
    config.height = "600px";
    config.toolbarCanCollapse = true;
    //config.toolbarStartupExpanded = false;
    config.extraPlugins = 'youtube,dvs.imagev2,dvs.video1,dvs.boxtext,dvs.boxphoto,dvs.checkspell';  //dvs.video2,dvs.menuheading,widget,filetools,notificationaggregator,notification,widgetselection,lineutils,uploadwidget,uploadimage
    config.toolbar = [
        ['Bold', 'Italic', 'Underline', 'TextColor'],
        ['Styles', 'Format', 'FontSize'],
        ['NumberedList', 'BulletedList', 'Outdent', 'Indent', 'JustifyLeft', 'JustifyCenter', 'JustifyRight'],
        ['dvs.menuheading'],
        ['Youtube', 'dvs.video2', 'dvs.imagev2', 'dvs.boxtext', 'dvs.boxphoto', 'Table', 'Link', 'Source', 'ShowBlocks', 'dvs.checkspell', 'Preview']
    ];
    config.allowedContent = true;
    config.uploadUrl = '/FileManager/Handler/CKEditorUpload.ashx';
    //Set default show outline block
    CKEDITOR.config.startupOutlineBlocks = true;

    config.pasteFromWordRemoveFontStyles = false;
    config.pasteFromWordRemoveStyles = false;
};

