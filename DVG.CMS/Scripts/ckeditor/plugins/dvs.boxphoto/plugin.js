CKEDITOR.plugins.add('dvs.boxphoto', {
    init: function (editor) {
        editor.addCommand('dvs.boxphoto', new CKEDITOR.dialogCommand('BoxPhotoDialog'));
        editor.ui.addButton('dvs.boxphoto', {
            label: 'Chèn box ảnh',
            command: 'dvs.boxphoto',
            toolbar: 'insert',
            icon: this.path + 'icons/dvs.boxphoto.png'
        });

        CKEDITOR.dialog.add('BoxPhotoDialog', this.path + 'dialogs/dvs.boxphoto.js');
    }
});