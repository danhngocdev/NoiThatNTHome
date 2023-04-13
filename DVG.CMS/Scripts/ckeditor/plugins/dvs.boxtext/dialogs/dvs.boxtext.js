CKEDITOR.dialog.add('BoxTextDialog', function (editor) {
    return {
        title: 'Chèn box nội dung',
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
                            type: 'text',
                            id: 'txtTitle',
                            label: 'Tiêu đề',
                            setup: function (e) {
                                var value = e.getAttribute("title");

                                if (!value) {
                                    return;
                                }

                                CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtTitle", value.replace(/"/gi, "&quot;"))
                            }
                        },
                        {
                            type: 'textarea',
                            id: 'txtContent',
                            label: 'Nội dung',
                            setup: function (e) {
                                var value = e.getAttribute("data-content");

                                if (!value) {
                                    return;
                                }

                                CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtContent", value)
                            }
                        },
                        {
                            type: 'text',
                            id: 'txtMaxWidth',
                            label: 'Max width',
                            'default': '300',
                            setup: function (e) {
                                var value = e.getAttribute("data-maxWidth");

                                if (!value) {
                                    return;
                                }

                                CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtMaxWidth", value)
                            }
                        },
                        {
                            type: 'select',
                            id: 'txtFloat',
                            label: 'Canh lề',
                            items: [['none'], ['left'], ['right'], ['center']],
                            'default': 'none',
                            setup: function (e) {
                                var value = e.getAttribute("float");

                                if (!value) {
                                    return;
                                }

                                CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtFloat", value);
                            }
                        },
                        {
                            type: 'hbox',
                            widths: ['20%', '30%'],
                            children:
                                [
                                    {
                                        type: 'text',
                                        id: 'txtTextColor',
                                        label: 'Mã màu chữ',
                                        setup: function (e) {
                                            var value = e.getAttribute("title");

                                            if (!value) {
                                                return;
                                            }

                                            CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtTextColor", value)
                                        }
                                    },
                                    {
                                        type: 'button',
                                        id: 'textButton',
                                        label: 'Chọn màu chữ',
                                        title: 'Chọn màu chữ',
                                        style: 'display: inline-block; margin-top: 20px; margin-left: auto; margin-right: auto; user-select: none;',
                                        onLoad: function () {
                                            this.getElement().getParent().setStyle("vertical-align", "bottom")
                                        },
                                        onClick: function () {
                                            editor.getColorFromDialog(function (a) {
                                                a && this.getDialog().getContentElement("tab-config", "txtTextColor").setValue(a);
                                                this.focus()
                                            }, this)
                                        }
                                    }
                                ]
                        },
                        {
                            type: 'hbox',
                            widths: ['20%', '30%'],
                            children:
                                [
                                    {
                                        type: 'text',
                                        id: 'txtBgColor',
                                        label: 'Mã màu nền',
                                        setup: function (e) {
                                            var value = e.getAttribute("title");

                                            if (!value) {
                                                return;
                                            }

                                            CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtBgColor", value)
                                        }
                                    },
                                    {
                                        type: 'button',
                                        id: 'backgroundButton',
                                        label: 'Chọn màu nền',
                                        title: 'Chọn màu nền',
                                        style: 'display: inline-block; margin-top: 20px; margin-left: auto; margin-right: auto; user-select: none;',
                                        onLoad: function () {
                                            this.getElement().getParent().setStyle("vertical-align", "bottom")
                                        },
                                        onClick: function () {
                                            editor.getColorFromDialog(function (a) {
                                                a && this.getDialog().getContentElement("tab-config", "txtBgColor").setValue(a);
                                                this.focus()
                                            }, this)
                                        }
                                    }
                                ]
                        },
                        {
                            type: 'html',
                            html: '<span id="editor2"></div>'
                        }
                    ]
            }
        ],

        onLoad: function () {
            /*fmClient.IsEditor = true;
            fmClient.SelectedControl = editor.name;*/
        },
        onShow: function () {
            function hex(x) {
                return ("0" + parseInt(x).toString(16)).slice(-2);
            }
            function rgb2hex(rgb) {
                rgb = rgb.match(/^rgba?\((\d+),\s*(\d+),\s*(\d+)(,\s*\d+\.*\d+)?\)$/);
                return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
            }
            var selection = editor.getSelection();
            var element = selection.getStartElement();

            if (element) {
                element = element.getAscendant('div', true);
            }

            if (!element
                || element.getAttribute("class") != "dvs-textbox") {
                element = editor.document.createElement('div');
                this.insertMode = true;
            } else {

                var obj = element.$;
                if (obj) {
                    var title = $(obj).find('.dvs-textbox-title').html();
                    var content = $(obj).find('.dvs-textbox-content').html();
                    var float = $(obj).attr('data-float');
                    var bgClr = rgb2hex($(obj).css("background-color"));
                    var txtClr = rgb2hex($(obj).css("color"));
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtTitle", title);
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtContent", content);
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtFloat", float);
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtMaxWidth", float);
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtBgColor", bgClr);
                    CKEDITOR.dialog.getCurrent().setValueOf("tab-config", "txtTextColor", txtClr);
                }
                this.insertMode = false;
                this.setupContent(element);
            }

            this.element = element;
        },
        buttons: [           
            CKEDITOR.dialog.okButton,
            CKEDITOR.dialog.cancelButton
        ],
        onOk: function (e) {
            var dialog = this;
            var element = dialog.element;
            var title = dialog.getValueOf('tab-config', 'txtTitle');
            var content = dialog.getValueOf('tab-config', 'txtContent');
            var float = dialog.getValueOf('tab-config', 'txtFloat');
            var maxWidth = dialog.getValueOf('tab-config', 'txtMaxWidth');
            var bgColor = dialog.getValueOf('tab-config', 'txtBgColor');
            var txtColor = dialog.getValueOf('tab-config', 'txtTextColor');
            var style = "";
            style += "max-width:" + maxWidth + "px;";

            if (float != "center") {
                style += "float:" + float + ";";
            } else {
                style += "left: 0;right: 0;margin: auto;margin-top: 10px;";
            }

            style += "background-color:" + bgColor + ";";
            style += "color:" + txtColor + ";";

            if (maxWidth == '' || maxWidth <= 0)
                maxWidth = 300;
            var template = '<p class=\'dvs-textbox-title\' >' + title + '</p><p class=\'dvs-textbox-content\'>' + content + '</p>';
            element.setAttribute("class", "dvs-textbox");
            element.setAttribute("data-type", "div");
            element.setAttribute("data-float", float);
            element.setAttribute("data-maxWidth", maxWidth);
            element.setAttribute("style", style);

            element.setHtml(template);
            dialog.commitContent(element);
            if (dialog.insertMode) {
                editor.insertElement(element);
            }
        }
    };
});