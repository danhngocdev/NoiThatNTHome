CKEDITOR.plugins.add('dvs.video', {
    init: function (editor) {
        editor.addCommand('dvs.video', new CKEDITOR.dialogCommand('VideoDialog'));
        editor.ui.addButton('dvs.video', {
            label: 'Chèn video bằng link',
            command: 'dvs.video',
            toolbar: 'insert',
            icon: this.path + 'icons/dvs.video.png',
        });

        CKEDITOR.dialog.add('VideoDialog', this.path + 'dialogs/dvs.video.js');
    }
});