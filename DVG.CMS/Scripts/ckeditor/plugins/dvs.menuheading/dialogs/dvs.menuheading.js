CKEDITOR.dialog.add('MenuHeadingDialog', function (editor) {
    var plugin = CKEDITOR.plugins.get('dvs.menuheading');
    CKEDITOR.document.appendStyleSheet(CKEDITOR.getUrl(plugin.path + 'dialogs/menuheading.css'));
    return {
        title: 'Menu heading',
        minWidth: 500,
        minHeight: 300,
        contents: [
            {
                id: 'tab-menuheading',
                padding: 0,
                height: 450,
                width: 500,
                elements:
                [
                    {
                        type: 'hbox',
                        widths: ['70%', '30%'],
                        children: [
                            {
                                type: 'text',
                                id: 'txtName',
                                label: 'Tiêu đề',
                                'default': ''
                            },
                            {
                                type: 'button',
                                id: 'buttonId',
                                label: 'Thêm',
                                title: 'Thêm',
                                style: 'display: inline-block; margin-top: 20px; margin-left: auto; margin-right: auto; user-select: none;',
                                onClick: function () {
                                    // this = CKEDITOR.ui.dialog.button
                                    var dialog = CKEDITOR.dialog.getCurrent();
                                    var model = new Object();
                                    var inputName = dialog.getContentElement('tab-menuheading', 'txtName').getInputElement().$;

                                    model.Name = dialog.getValueOf('tab-menuheading', 'txtName');

                                    if (!model.Name) {
                                        $("#ck_messageBox").html(mh.msg.nameEmpty);
                                        return false;
                                    }
                                    var data = JSON.stringify({ model: model });
                                    mh.updateData(data,
                                        function (response) {
                                            if (response) {
                                                model.Id = response;
                                                var html = mh.buildMenuHeading(model);
                                                console.log(html)
                                                $("#ck_menuHeading_content").prepend(html);
                                                mh.initEvent();
                                                $(inputName).val("");
                                            }
                                        });
                                }
                            }
                        ]
                    },
                    {
                        type: 'html',
                        html: '<span id="ck_messageBox"></div>'
                    },
                    {
                        type: 'html',
                        html: '<div id="ck_listMenuHeading"><p class="ck_menuheading_title">Danh sách menu</p><div id="ck_menuHeading_content"></div></div>'
                    }
                ]
            }
        ],
        onLoad: function () {            
            mh.init();
        },
        onShow: function () {

            var selection = editor.getSelection();
            var element = selection.getStartElement();

            if (element) {
                element = element.getAscendant('div', true);
            }

            if (!element
                || element.getName() != 'div'
                || element.getAttribute("data-type") != "jwplayer") {
                element = editor.document.createElement('div');
                this.insertMode = true;
            } else {
                this.insertMode = false;

                this.setupContent(element);
            }

            this.element = element;
        },
        buttons: [CKEDITOR.dialog.okButton, CKEDITOR.dialog.cancelButton],
        onOk: function (e) {
            var dialog = this;
            var range = editor.createRange();
            var len = mh.option.listItemChecked.length, title;
            mh.option.listItemChecked.sort(mh.sortByProperty("Ordinal"));
            if (len > 0) {
                for (var i = 0; i < len; i++) {
                    var titleExtend = $("#dvs_mh_title_external_" + mh.option.listItemChecked[i].Id).val();
                    if (titleExtend) {
                        mh.option.listItemChecked[i].NameExtend = titleExtend;
                    }
                    if (mh.option.listItemChecked[i].NameExtend) {
                        title = mh.option.listItemChecked[i].NameExtend;
                    } else {
                        title = mh.option.listItemChecked[i].Name;
                    }
                    var div = new CKEDITOR.dom.element('div');
                    var elementH = CKEDITOR.dom.element.createFromHtml('<h2>' + title + '</h2>');
                    var elementP = CKEDITOR.dom.element.createFromHtml('<p></p>');
                    range.setStartAt(editor.getSelection().getStartElement().getFirst(), CKEDITOR.POSITION_BEFORE_START);
                    
                    elementH.setAttribute("id", mh.option.id + "_" + mh.option.listItemChecked[i].Id);
                    elementH.setAttribute("class", mh.option.className);
                    elementH.setAttribute("dataId", mh.option.listItemChecked[i].Id);
                    elementH.setAttribute("dataOrdinal", mh.option.listItemChecked[i].Ordinal);
                    div.setAttribute("class", mh.option.classBoxName);
                    elementP.setHtml("<br>");
                    elementH.appendTo(div);
                    elementP.appendTo(div);

                    // Insert element at the range position.
                    dialog.commitContent(div);
                    editor.editable().insertElement(div, range);

                    //dialog.commitContent(elementP);
                    //editor.editable().insertElement(elementP, range);

                }
            }

        },
        onHide: function (e) {

        }
    };
});

var mh = {
    option: {
        id: 'dvs_menuheading',
        className: 'dvs_menuheading_item',
        classBoxName: 'dvs_menuheading_box',
        itemCheck: '#ck_menuHeading_content .dvs_checkbox_menuheading',
        inputTitleExtend: '#ck_menuHeading_content .dvs_input_menuheading',
        inputOrdinal: '#ck_menuHeading_content .dvs_input_menuheading_ordinal',
        messageBox: '#ck_messageBox',
        listMenuHeading: "#ck_menuHeading_content",
        listItemChecked: [],
        count: 0
    },
    msg: {
        nameEmpty: "Vui lòng nhập tiêu đề.",
        menuAdded: "Menu đã được thêm."
    },
    init: function () {
        this.getListData();
        var dialog = CKEDITOR.dialog.getCurrent(), len = mh.option.listItemChecked.length;
        var inputName = dialog.getContentElement('tab-menuheading', 'txtName').getInputElement().$;
        $(inputName).keypress(function () {
            if ($(this).val()) {
                $(mh.option.messageBox).empty();
            } else {
                $(mh.option.messageBox).html(mh.msg.nameEmpty);
            }
        });
    },
    initEvent: function () {
        $(mh.option.itemCheck).click(function () {
            var elem = $(this);
            var model = new Object(),
                len = mh.option.listItemChecked.length,
                id = elem.attr("dataId"),
                isExisted = false;

            model.Id = id;
            model.Name = elem.attr("dataName");
            model.Ordinal = mh.option.count++;
            var itemHeadingBox = elem.closest("div.ck_mh_row");
            var tileExBox = itemHeadingBox.find("div.dvs_mh_external_box");
            var menuBox = itemHeadingBox.find("div.ck_mh_col_left");
            var ordinal = itemHeadingBox.find("input#dvs_mh_ordinal_" + id);

            if (elem.is(':checked')) {
                tileExBox.show();
                menuBox.css('margin-top', 28);
                if (len > 0) {
                    for (var i = 0; i < len; i++) {
                        if (mh.option.listItemChecked[i].Id == id) {
                            isExisted = true;
                        }
                    }
                    if (!isExisted) {
                        mh.option.listItemChecked.push(model);
                        ordinal.val(model.Ordinal);
                    }
                } else {
                    mh.option.listItemChecked.push(model);
                    ordinal.val(model.Ordinal);
                }
            } else {
                tileExBox.hide();
                menuBox.css('margin-top', 4);
                if (len > 0) {
                    for (var i = 0; i < len; i++) {
                        if (mh.option.listItemChecked[i].Id == id) {
                            mh.option.listItemChecked.splice(i, 1);
                            break;
                        }
                    }
                }
            }
        });

        $(mh.option.inputOrdinal).on("change",
            function() {
                var $elem = $(this);
                var id = $elem.attr("dataId"), 
                    len = mh.option.listItemChecked.length,
                    ordinal = $elem.val();
                if (!ordinal) {
                    ordinal = 0;
                }
                for (var i = 0; i < len; i++) {
                    if (mh.option.listItemChecked[i].Id == id) {
                        mh.option.listItemChecked[i].Ordinal = parseInt(ordinal);
                        break;
                    }
                }
            });
    },
    getListData: function () {
        CKEDITOR.dialog.getCurrent().setState(CKEDITOR.DIALOG_STATE_BUSY);
        window.setTimeout(function () {
            var jqXHR = $.ajax({
                url: "/MenuHeading/GetListMenu",
                type: "POST",
                //dataType: "json",
                contentType: "application/json; charset=utf-8",
                processData: false,
                cache: false,
                data: null,
                success: function (response) {
                    if (response.Success) {
                        var html = "";
                        if (response.Data.length > 0) {
                            for (var i = 0; i < response.Data.length; i++) {
                                html += mh.buildMenuHeading(response.Data[i]);
                            }
                            $(mh.option.listMenuHeading).empty().html(html);

                            mh.initEvent();
                        } else {
                            $(mh.option.listMenuHeading).empty().html("Chưa có menu");
                        }
                    } else {
                        $(mh.option.listMenuHeading).empty().html("Chưa có menu");
                    }
                    CKEDITOR.dialog.getCurrent().setState(CKEDITOR.DIALOG_STATE_IDLE);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log('ERRORS: ' + textStatus);
                }
            });
        }, 1500);
        
    },
    updateData: function (data, callBack) {
        var jqXHR = $.ajax({
            url: "/MenuHeading/Update",
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            processData: false,
            cache: false,
            data: data,
            success: function (response) {
                if (response.Success) {
                    $(mh.option.messageBox).html(mh.msg.menuAdded);
                    window.setTimeout(function () {
                        $(mh.option.messageBox).empty();
                    }, 2000);
                    callBack(response.Data);
                } else {
                    $(mh.option.messageBox).html(response.Message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log('ERRORS: ' + textStatus);
            }
        });
    },
    buildMenuHeading: function (data) {
        var html = "";
        html += '<div class="ck_mh_row clear-fix">';
        html += '   <div class="ck_mh_col ck_mh_col_left">';
        html += '       <input type="checkbox" id="dvs_checkbox_' + data.Id + '" class="dvs_checkbox_menuheading" dataId="' + data.Id + '" dataName="' + data.Name + '" />';
        html += '       <label id="dvs_lable_checkbox_' + data.Id + '" for="dvs_checkbox_' + data.Id + '">' + data.Name + '</label>';
        html += '   </div>';//End col left
        html += '<div class="ck_mh_col ck_mh_col_right dvs_mh_external_box">';
        html +=
            '<div role="presentation" id="dvs_mh_ordinal_box_' + data.Id + '" class="cke_dialog_ui_text dvs_mh_ordinal_box">' +
            '<label class="cke_dialog_ui_labeled_label" for="dvs_mh_ordinal_' + data.Id + '">Thứ tự</label>' +
            '<div class="cke_dialog_ui_labeled_content" role="presentation">' +
            '<div class="cke_dialog_ui_input_text" role="presentation">' +
            '<input class="cke_dialog_ui_input_text dvs_input_menuheading_ordinal" id="dvs_mh_ordinal_' + data.Id + '" dataId="' + data.Id + '" type="text">' +
            '</div>' +
            '</div>' +
            '</div>';
        html +=
            '<div role="presentation" id="dvs_mh_title_external_box_' + data.Id + '" class="cke_dialog_ui_text dvs_mh_title_external_box">' +
            '<label class="cke_dialog_ui_labeled_label" for="dvs_mh_title_external_' + data.Id + '">Đặt lại tiêu đề</label>' +
            '<div class="cke_dialog_ui_labeled_content" role="presentation">' +
            '<div class="cke_dialog_ui_input_text" role="presentation">' +
            '<input class="cke_dialog_ui_input_text dvs_input_menuheading" id="dvs_mh_title_external_' + data.Id + '" type="text">' +
            '</div>' +
            '</div>' +
            '</div>';
        html += '</div>';//End col right
        html += '</div>';//End row
        return html;
    },
    sortByProperty: function (prop) {
        return function (a, b) {
            if (typeof a[prop] == "number") {
                return (a[prop] - b[prop]);
            } else {
                return ((a[prop] < b[prop]) ? -1 : ((a[prop] > b[prop]) ? 1 : 0));
            }
        };
    }
}