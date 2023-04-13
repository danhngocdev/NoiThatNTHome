CKEDITOR.plugins.add('dvs.video2', {
    init: function (editor) {
        editor.addCommand('dvs.video2', new CKEDITOR.dialogCommand('VideoDialog2'));
        var pluginDirectory = this.path;
        editor.ui.addButton('dvs.video2', {
            label: 'Chèn video bằng link',
            command: 'dvs.video2',
            toolbar: 'insert',
            icon: pluginDirectory + 'icons/dvs.video.png'
        });
        
        CKEDITOR.dialog.add('VideoDialog2', pluginDirectory + 'dialogs/dvs.video.js?v=' + (new Date()));
    }
});
