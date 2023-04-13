CKEDITOR.plugins.add('dvs.menuheading', {
    init: function (editor) {
        editor.addCommand('dvs.menuheading', new CKEDITOR.dialogCommand('MenuHeadingDialog'));
        var pluginDirectory = this.path;
        editor.ui.addButton('dvs.menuheading', {
            label: 'Thêm menu heading',
            command: 'dvs.menuheading',
            toolbar: 'insert',
            icon: pluginDirectory + 'icons/add-list-16.png'
        });

        CKEDITOR.dialog.add('MenuHeadingDialog', pluginDirectory + 'dialogs/dvs.menuheading.js');
    }
});

