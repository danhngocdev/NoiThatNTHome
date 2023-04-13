CKEDITOR.plugins.add('dvs.boxtext', {
    init: function (editor) {
        editor.addCommand('dvs.boxtext', new CKEDITOR.dialogCommand('BoxTextDialog'));
        editor.ui.addButton('dvs.boxtext', {
            label: 'Chèn box nội dung',
            command: 'dvs.boxtext',
            toolbar: 'insert',
            icon: this.path + 'icons/dvs.boxtext.png'
        });

        CKEDITOR.dialog.add('BoxTextDialog', this.path + 'dialogs/dvs.boxtext.js');
    }
});