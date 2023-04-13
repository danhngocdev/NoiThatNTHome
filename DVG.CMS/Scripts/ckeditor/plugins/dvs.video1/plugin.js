CKEDITOR.plugins.add('dvs.video1', {
    init: function (editor) {
        editor.addCommand('dvs.video1', new CKEDITOR.dialogCommand('VideoDialog'));
        var pluginDirectory = this.path;
        editor.ui.addButton('dvs.video1', {
            label: 'Chèn video bằng link',
            command: 'dvs.video1',
            toolbar: 'insert',
            icon: pluginDirectory + 'icons/dvs.video.png'
        });
        
        CKEDITOR.dialog.add('VideoDialog', pluginDirectory + 'dialogs/dvs.video.js');
    }
});
