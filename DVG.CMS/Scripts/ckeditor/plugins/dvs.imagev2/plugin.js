CKEDITOR.plugins.add('dvs.imagev2', {
    init: function (editor) {
        editor.addCommand('dvs.imagev2', {
            exec: function (editor) {
                //var w = window.open("/FileManager/Default.aspx", "File manager", "width = 950, height = 600");
                var w = popupCenter("/FileManager/Default.aspx", "File manager", 950, 600);
                w.callback = function (result) {
                    w.close();
                    for (var i = 0; i < result.length; i++) {
                        var imageLink = result[i].path;
                        var shortLink = result[i].shortpath;
                        var desc = result[i].desc;
                        var title = result[i].title;
                        var width = result[i].width;
                        var height = result[i].height;

                        //<div class="dvg_photo_box">
                        //    <p><img alt="vừa cắt thử cái ảnh" src="https://img.tinxe.vn/2018/03/13/ikl8LTEh/alone-1868530960720--3ab3.jpg" title="vừa cắt thử cái ảnh" data-width="500" data-height="300" /></p>
                        //    <p class="dvg_photo_caption">vừa cắt thử c&aacute;i ảnh</p>
                        //</div>

                        var container = editor.document.createElement('div');
                        var imgContainer = editor.document.createElement('p');
                        var captionContainer = editor.document.createElement('p');
                        var element = editor.document.createElement('img');

                        container.setAttribute("class", "dvg_photo_box");

                        var isGif = /\.(gif)$/.test(imageLink);
                        if (isGif) {
                            var link  = cms.configs.ViewDomain + "resize/600x-/" + shortLink + ".png";
                            element.setAttribute("data-src", imageLink);
                            element.setAttribute("src", link);
                        }
                        else
                            element.setAttribute("src", imageLink);
                        element.setAttribute("title", title);
                        element.setAttribute("alt", title);
                        element.setAttribute("data-width", width);
                        element.setAttribute("data-height", height);
                        imgContainer.setStyle("text-align", "center");
                        imgContainer.append(element);
                        container.append(imgContainer);

                        if (desc.length > 0) {
                            captionContainer.setAttribute("class", "dvg_photo_caption");
                            captionContainer.setStyle("text-align", "center");
                            captionContainer.setStyle("font-style", "italic");
                            captionContainer.appendHtml(desc);
                            container.append(captionContainer);
                        }

                        container.append(editor.document.createElement('br'));

                        editor.insertElement(container);
                    }
                };
            }
        });
        editor.ui.addButton('dvs.imagev2', {
            label: 'Chèn hình ảnh v2',
            command: 'dvs.imagev2',
            toolbar: 'insert',
            icon: this.path + 'icons/dvs.image.png'
        });
    }
});