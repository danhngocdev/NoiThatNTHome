(function () {
    var a = {
        exec: function (editor, data) {
            DVG_obj_boxTypePhotoShow(editor, data);
        }
    },

b = 'dvgboxphoto';
    CKEDITOR.plugins.add(b, {
        init: function (editor) {
            editor.addCommand(b, a);
        }
    });
})();

function DVG_obj_boxTypePhotoShow(editor, oldContent) {
    
    $('.DVG_obj_box_DivPosition').unbind('click').on('click', function (e) {
        e.preventDefault();
        alert('dvgboxphoto');
    });
}