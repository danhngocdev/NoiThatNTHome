CKEDITOR.dialog.add('BoxPhotoDialog', function (editor) {
    return {
        title: 'Chèn box ảnh',
        minWidth: 500,
        minHeight: 250,
        contents: [
            {
                id: 'tab-config',
                label: "Config",
                padding: 0,
                height: 250,
                width: 500,
                elements:
                [
                    {
                        type: 'button',
                        id: 'btnUpload',
                        label: 'Tải và lấy đường dẫn ảnh',
                        onClick: function () {
                            //var w = window.open("/FileManager/Default.aspx", "File manager", "width = 950, height = 600");
                            var w = popupCenter("/FileManager/Default.aspx", "File manager", 950, 600);

                            w.callback = function (lst) {
                                w.close();

                                if (!lst || lst.length <= 0) {
                                    alert("Không có file nào được chọn!");

                                    return;
                                }
                                var result = lst[0];

                                if (/(\.|\/)(gif|jpe?g|png)$/i.exec(result.path) == null) {
                                    alert("Bạn cần chọn hình ảnh");

                                    return;
                                }

                                CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtImageLink", result.path);
                                CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtAlt", result.title.replace(/"/gi, "&quot;"))
                                CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtCaption", result.desc)
                                if (result.width == 0 || result.height == 0) {
                                    $("<img/>").attr("src", result.path).load(function () {
                                        console.log("Img info:" + this.width + "x" + this.height);
                                        CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtWidth", this.width) // callback FileManager se tra them w, h
                                        CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtHeight", this.height) // callback FileManager se tra them w, h
                                    });
                                }
                                else {
                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtWidth", result.width) 
                                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtHeight", result.height) 
                                }
                            };
                        }
                    },
                    {
                        type: 'text',
                        id: 'txtImageLink',
                        label: 'Đường dẫn ảnh',
                        validate: CKEDITOR.dialog.validate.notEmpty("Đường dẫn ảnh không được để trống"),
                        setup: function (e) {
                            var value = e.getAttribute("src");

                            if (!value) {
                                return;
                            }

                            CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtImageLink", value.replace(/"/gi, "&quot;"))
                        }
                    },
                    {
                        type: 'textarea',
                        id: 'txtCaption',
                        label: 'Mô tả ảnh',
                        //validate: CKEDITOR.dialog.validate.notEmpty("Mô tả ảnh không được để trống"),
                        setup: function (e) {
                            var value = e.getAttribute("caption");

                            if (!value) {
                                return;
                            }
                            CKEDITOR.dialog.getCurrent()
                                .setValueOf("tab-config", "txtCaption", value.replace(/"/gi, "&quot;"));
                        }
                    },
                    {
                        type: 'select',
                        id: 'txtAlign',
                        label: 'Vị trí mô tả ảnh',
                        items: [['trái'], ['phải'], ['trên'], ['dưới']],
                        'default': 'dưới',
                        setup: function (e) {
                            var value = e.getAttribute("align");

                            if (!value) {
                                return;
                            }

                            CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtAlign", value);
                        }
                    },
                    {
                        type: 'text',
                        id: 'txtAlt',
                        label: 'Nội dung hiển thị khi ảnh trống (Alt)',
                        //validate: CKEDITOR.dialog.validate.notEmpty("Trường Alt không được để trống"),
                        setup: function (e) {
                            var value = e.getAttribute("alt");

                            if (!value) {
                                return;
                            }

                            CKEDITOR.dialog.getCurrent()
                                .setValueOf("tab-config", "txtAlt", value.replace(/"/gi, "&quot;"));
                        }
                    },
                    {
                        type: 'text',
                        id: 'txtWidth',
                        label: 'Chiều rộng',
                        setup: function (e) {
                            var value = e.getAttribute("data-width");

                            if (!value) {
                                return;
                            }

                            CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtWidth", value);
                        }
                    },
                    {
                        type: 'text',
                        id: 'txtHeight',
                        label: 'Chiều cao',
                        setup: function (e) {
                            var value = e.getAttribute("data-height");

                            if (!value) {
                                return;
                            }

                            CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtHeight", value);
                        }
                    },
                    {
                        type: 'text',
                        id: 'txtClass',
                        label: 'Class',
                        setup: function (e) {
                            var value = e.getAttribute("data-class");

                            if (!value) {
                                return;
                            }

                            CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtClass", value);
                        }
                    },
                    {
                        type: 'textarea',
                        id: 'txtStyle',
                        label: 'Style',
                        setup: function (e) {
                            var value = e.getAttribute("data-style");

                            if (!value) {
                                return;
                            }

                            CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtStyle", value);
                        }
                    },
                    {
                        type: 'select',
                        id: 'txtFloat',
                        label: 'Canh lề',
                        items: [['none'], ['left'], ['right']],
                        'default': 'none',
                        setup: function (e) {
                            var value = e.getAttribute("float");

                            if (!value) {
                                return;
                            }

                            CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtFloat", value);
                        }
                    }
                ]
            }
        ],
        onLoad: function () {
            /*fmClient.IsEditor = true;
            fmClient.SelectedControl = editor.name;*/
        },
        onShow: function () {
            var selection = editor.getSelection();
            var element = selection.getStartElement();

            if (element) {
                element = element.getAscendant('div', true);
            }
            if (!element
                || !element.hasClass("dvs-photobox")) {
                element = editor.document.createElement('div');
                this.insertMode = true;
            } else {
                imageElement = element.find('img');
                var obj = imageElement.$;
                if (obj) {
                    var imageLink = $(obj).attr('src');
                    var caption = $(obj).attr('caption');
                    var alignText = $(obj).attr('data-align');
                    var alt = $(obj).attr('alt');
                    var width = $(obj).attr('data-width');
                    var height = $(obj).attr('data-height');
                    var _class = $(obj).attr('data-class');
                    var style = $(obj).attr('data-style');
                    var float = $(obj).attr('data-float');
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtImageLink", imageLink);
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtCaption", caption);
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtAlign", alignText);
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtAlt", alt);
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtWidth", width);
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtHeight", height);
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtClass", _class);
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtStyle", style);
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtFloat", float);
                }
                this.insertMode = false;
                this.setupContent(element);
            }

            this.element = element;
        },
        buttons: [CKEDITOR.dialog.okButton, CKEDITOR.dialog.cancelButton],
        onOk: function (e) {
            var dialog = this;
            var imageLink = dialog.getValueOf('tab-config', 'txtImageLink');
            var width = dialog.getValueOf('tab-config', 'txtWidth');
            var height = dialog.getValueOf('tab-config', 'txtHeight');
            var _class = dialog.getValueOf('tab-config', 'txtClass');
            var dataStyle = dialog.getValueOf('tab-config', 'txtStyle');
            var caption = dialog.getValueOf('tab-config', 'txtCaption');
            var alt = dialog.getValueOf('tab-config', 'txtAlt');
            var alignText = dialog.getValueOf('tab-config', 'txtAlign');
            var float = dialog.getValueOf('tab-config', 'txtFloat');
            var element = dialog.element;
            var imgBox = new CKEDITOR.dom.element('figure');//dialog.element;
            var imageElement = new CKEDITOR.dom.element('img');
            var captionElement = new CKEDITOR.dom.element('figcaption');
            var style = "";
            var styleDiv = "";
            var classDiv = "dvs-photobox";

            if (/(\.|\/)(gif|jpe?g|png)$/i.exec(imageLink) == null) {
                alert("Bạn chưa chọn hình ảnh");

                return;
            }

            if (_class) {
                imageElement.setAttribute("class", "dvs-photobox-img " + _class);
            }
            else {
                imageElement.setAttribute("class", "dvs-photobox-img");
            }

            if (width) {
                style += "width: " + width + "px;";
            }

            if (height) {
                style += "height: " + height + "px;";
            }

            style += dataStyle;

            imageElement.setAttribute("data-width", width);
            imageElement.setAttribute("data-height", height);
            imageElement.setAttribute("data-class", _class);
            imageElement.setAttribute("caption", caption);
            imageElement.setAttribute("data-style", dataStyle);
            imageElement.setAttribute("alt", alt);
            imageElement.setAttribute("src", imageLink);
            imageElement.setAttribute("style", style);
            imageElement.setAttribute("data-align", alignText);
            imageElement.setAttribute("data-float", float);

            captionElement.appendHtml('<em>' + caption + '</em>');
            styleDiv += "float:" + float+ ";";

            // align
            element.setHtml('');
            switch (alignText) {
                case "trái": {
                    styleDiv += "height: " + height + "px;";
                    captionElement.setAttribute("style", "height: " + height + "px;");
                    imgBox.append(captionElement);
                    imgBox.append(imageElement);
                    imgBox.setStyle("text-align", "center");
                    //element.append(captionElement);
                    element.append(imgBox);
                    classDiv += " dvs-photobox-caption-left";
                    break;
                };
                case "phải": {
                    styleDiv += "height: " + height + "px;";
                    captionElement.setAttribute("style", "height: " + height + "px;");
                    imgBox.append(imageElement);
                    imgBox.append(captionElement);
                    imgBox.setStyle("text-align", "center");
                    element.append(imgBox);
                    //element.append(imageElement);
                    //element.append(captionElement);
                    classDiv += " dvs-photobox-caption-right";
                    break;
                };
                case "trên": {
                    styleDiv += "width: " + width + "px;";
                    captionElement.setAttribute("style", "width: " + width + "px;");
                    imgBox.append(captionElement);
                    imgBox.append(imageElement);
                    imgBox.setStyle("text-align", "center");
                    element.append(imgBox);
                    //element.append(captionElement);
                    //element.append(imageElement);
                    classDiv += " dvs-photobox-caption-up";
                    break;
                };
                case "dưới": {
                    styleDiv += "width: " + width + "px;";
                    captionElement.setAttribute("style", "width: " + width + "px;");
                    imgBox.append(imageElement);
                    imgBox.append(captionElement);
                    imgBox.setStyle("text-align", "center");
                    element.append(imgBox);
                    //element.append(imageElement);
                    //element.append(captionElement);
                    classDiv += " dvs-photobox-caption-down";
                    break;
                };
                default: {
                    imgBox.append(imageElement);
                    imgBox.append(captionElement);
                    imgBox.setStyle("text-align", "center");
                    element.append(imgBox);
                    break;
                }
            }
            element.setAttribute("class", classDiv);
            //element.setAttribute("style", styleDiv);

            dialog.commitContent(element);

            if (dialog.insertMode) {
                editor.insertElement(element);
            }
        }
    };
});