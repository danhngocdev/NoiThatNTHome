if (typeof (CKEDITOR) != "undefined") {
    CKEDITOR.config.customConfig = "/Scripts/ckeditor/common.js?v=" + cms.configs.CKVERSION;
    CKEDITOR.on('instanceReady', function (e) {
        var $editorInline = $(".editor-inline").hasClass("ckeditor-copytable");
        var editor = e.editor;
        if (!$editorInline) {
            editor.on('paste', function (evnt) {
                var data = evnt.data;
                //var regex = new RegExp(/<a[^>]*href=(["'][^"']+["'])[^>]*>(((?!<\/a>).)*)<\/a>/ig);
                var reg = /<a[^>]*href=["']([^"']+)["'][^>]*>(((?!<\/a>).)*)<\/a>/ig;
                var content = data.dataValue.replace(reg, function myFunction(x, x1, x2) {
                    var host = getHostName(x1);
                    if (host == cms.configs.ProductionDomain)
                        return '<a href="' + x1 + '" class="blue-clr" target="_blank">' + x2 + '</a>';
                    else
                        return '<a href="' + x1 + '" class="blue-clr" rel="nofollow" target="_blank">' + x2 + '</a>';
                });

                content = removeUnnecessaryTags(content);
                //content = urlify(content);
                data.dataValue = content;
                data.type = 'html';
            });
        }
        editor.on('doubleclick', function (evt) {
            var element = evt.data.element;
            var strName = element.$.className;
            if (strName.indexOf('dvs-textbox') != -1) {
                evt.data.dialog = 'BoxTextDialog';
            }
            else if (strName.indexOf('dvs-photobox') != -1) {
                evt.data.dialog = 'BoxPhotoDialog';
            }
        });
    });
}


var removeUnnecessaryTags = function (data) {
    if (data == null || data === '') return '';
    //console.log('removeUnnecessaryTags');
    var splitReg = /(?:\s|\t|\n|\r|&nbsp;)*<\/?(?:p\s+[^>]*|p|div[^>]*)>(?:\s|\t|\n|\r|&nbsp;)*/gi,
        removeTagReg = /(?:\s|\t|\n|\r|&nbsp;)*<\/?(?:(?:p\s+|div|table|tbody|tr|th|td|ul|li|br|h1|h2|a|\??[a-z0-9\-]+\:[a-z0-9\-]+)[^>]*|p)>(?:\s|\t|\n|\r|&nbsp;)*|<\!--[\S\s]*-->/gi,
        styleTagReg = /<\/?(?:(?:p\s+|em|u|b|strong|<i\/?>|font|span|\??[a-z0-9\-]+\:[a-z0-9\-]+)[^>]*)>/gi,
        trimReg = /^(?:\s|\t|\n|\r|&nbsp;|<br\/?>)+|(?:\s|\t|\n|\r|&nbsp;|<br\/?>)+$/gi,
        photoReg = /<img[^>]+>/gi,
        beginValidTag = /^(?:\s|\t|\n|\r|&nbsp;)*<(?:div[^>]*|p\s+[^>]*|p|h\d[^>]*)>/gi,
        paragraphs,
        hasTag = /<\/?[a-z0-9]+[^>]*>/gi;
    data = data.replace(/<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi, '');
    data = data.replace(/(?:<br>|<br >|<br\/>|<br \/>)/gi, '<p>&nbsp;</p>');
    //data = data.replace(/font-family:[^;]+/gi, '');
    //data = data.replace(/font-size:[^;]+/gi, '');
    //data = data.replace(/font:[^;]+/gi, '');
    //data = data.replace(/line-height:[^;]+/gi, '');
    paragraphs = data.split(splitReg);
    if (paragraphs && paragraphs.length > 1) {
        for (var i = 0; i < paragraphs.length; ++i) {
            paragraphs[i] = paragraphs[i].replace(trimReg, '').trim();
            if (!paragraphs[i] || /^(?:(?:\s|\t|\n|\r|&nbsp;)*<\/?(?:span|div|p|br)+[^>]*>(?:\s|\t|\n|\r|&nbsp;)*)+$/gi.test(paragraphs[i])) {
                paragraphs[i] = '';
            } else {
                // paragraphs[i] = paragraphs[i].replace(/<\/?(?:strong|b)>*|<\!--[\S\s]*-->/gi, ' ');
                paragraphs[i] = paragraphs[i].replace(removeTagReg, ' ');
                paragraphs[i] = paragraphs[i].replace(styleTagReg, '');
                if (paragraphs[i] !== '') {
                    if (!beginValidTag.test(paragraphs[i]) && !photoReg.test(paragraphs[i])) {
                        paragraphs[i] = "<p>" + paragraphs[i] + "</p>";
                    }
                    paragraphs[i] = paragraphs[i].replace(photoReg, function (m) {
                        return '<div class="media_plugin" type="photo" style="text-align:center;"><div>' + m.replace(/style\s*=\s*[\"\'][^\"\']*[\"\']/gi, '') + '</div><div></div></div><p>&nbsp;</p>';
                    });
                }
            }
        }
        data = paragraphs.join('').replace(trimReg, '');
    }
    return data;
};

function urlify(text) {
    var urlRegex = /(https?:\/\/[^\s<"']+)/gi;
    return text.replace(urlRegex, function (url) {
        return '<a href="' + url + '" class="blue-clr" rel="nofollow" target="_blank">' + url + '</a>';
    });
}

var Const = (function ($) {
    var message = {
        invalid: 'Dữ liệu không hợp lệ',
        checked: 'Bạn phải check vào ô này',
        empty: 'Bạn không được để trống',
        update_succsess: 'Dữ liệu cập nhật thành công',
        delete_succsess: 'Dữ liệu đã được xóa',
        update_error: 'Cập nhật không thành công!',
        itemRetuned: 'Bài viết đã được trả lại',
        itemBanned: 'Bài viết đã được gỡ bỏ',
        itemPublished: 'Bài viết đã được xuất bản',
        itemAdded: 'Bài viết đã được thêm',
        itemExisted: 'Bài viết đã tồn tại',
        itemMax: 'Danh sách đã đủ bài viết',
        NewsTypeInvalid: 'Loại tin không hợp lệ',
        DisplayTypeInvalid: 'Kiểu hiển thị không hợp lệ',
        max: 'input is too long',
        number_min: 'too low',
        number_max: 'too high',
        url: 'invalid URL',
        number: 'not a number',
        password_repeat: 'mật khẩu không đúng',
        repeat: 'không đúng',
        uncomplete: 'Dữ liệu nhập chưa hoàn thành',
        select: 'Please select an option'
    };
    return {
        message: message
    }
})(jQuery);

$(document).ready(function () {

    //Tool tips for control
    //$('[data-toggle="tooltip"]').tooltip();

    $(".datepicker-inp").datetimepicker({
        format: 'DD/MM/YYYY HH:mm',
        locale: "vi",
        keepOpen: true,
        //sideBySide: true
    });

    $(".datepicker-short").datetimepicker({
        format: 'DD/MM/YYYY',
        locale: "vi",
        keepOpen: true
    });

    $(".datepicker-month").datetimepicker({
        format: "MM/YYYY",
        viewMode: "months",
        locale: "vi",
        keepOpen: true
    });

    $(".datepicker-year").datetimepicker({
        format: "YYYY",
        viewMode: "years",
        locale: "vi",
        minDate: '1970',
        maxDate: new Date(),
        keepOpen: false
    });
    loadEditors();
});

$.fn.isCKEDITOREmpty = function () {
    var idElem = $(this).attr("id");
    var messageLength = CKEDITOR.instances[idElem].getData().replace("<img", "||img").replace(/<[^>]*>/gi, '').replace(/&nbsp;/g, '').trim().length;
    if (!messageLength) return true;
    return false;
}

function wordCount(val) {
    var wom = val.match(/\S+/g);
    return {
        charactersNoSpaces: val.replace(/\s+/g, '').length,
        characters: val.length,
        words: wom ? wom.length : 0,
        lines: val.split(/\r*\n/).length
    };
}

function PostAjax(url, data, callback) {
    var uploadURL = url; //Upload URL
    var jqXHR = $.ajax({
        url: uploadURL,
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        processData: false,
        cache: false,
        data: data,
        async: false,
        success: function (resultData) {
            callback(resultData);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            // Handle errors here
            console.log('ERRORS: ' + textStatus);
        },
        complete: function (resultData) {

        }
    });
}

function loadEditors(height) {
    if (typeof (CKEDITOR) != "undefined") {
        var $editors = $("textarea.ckeditor");
        var $editorInline = $(".editor-inline");
        if ($editors.length) {
            $editors.each(function () {
                var editorID = $(this).attr("id");
                var isShortEdit = $(this).hasClass("ckeditor-short");
                var instance = CKEDITOR.instances[editorID];
                if (instance) { instance.destroy(true); }
                if (isShortEdit) {
                    if (height == undefined || height == null)
                        CKEDITOR.replace(editorID, { customConfig: '/Scripts/ckeditor/config-short.js?v=' + cms.configs.CKVERSION });
                    else
                        CKEDITOR.replace(editorID, { customConfig: '/Scripts/ckeditor/config-short.js?v=' + cms.configs.CKVERSION, height: height });
                } else {
                    if (height == undefined || height == null)
                        CKEDITOR.replace(editorID, { customConfig: '/Scripts/ckeditor/common.js?v=' + cms.configs.CKVERSION });
                    else
                        CKEDITOR.replace(editorID, { customConfig: '/Scripts/ckeditor/common.js?v=' + cms.configs.CKVERSION, height: height });
                }
            });
        }

        if ($editorInline.length > 0) {
            $editorInline.each(function () {
                var editorId = $(this).attr("id");
                var isSimpleEdit = $(this).hasClass("ck-inline-simple");
                var instance = CKEDITOR.instances[editorId];
                if (instance) {
                    instance.destroy(true);
                }
                CKEDITOR.disableAutoInline = true;
                if (isSimpleEdit) {
                    CKEDITOR.inline(editorId,
                        {
                            //startupFocus: true,
                            customConfig: '/Scripts/ckeditor/config-inline-simple.js?v=' + cms.configs.CKVERSION
                        });
                } else {
                    CKEDITOR.inline(editorId,
                        {
                            //startupFocus: true,
                            customConfig: '/Scripts/ckeditor/config-inline.js?v=' + cms.configs.CKVERSION
                        });
                }
                
            });
        }
    }
}

function popupCenter(pageURL, title, w, h) {
    var y = window.top.outerHeight / 2 + window.top.screenY - (h / 2);
    var x = window.top.outerWidth / 2 + window.top.screenX - (w / 2);
    return window.open(pageURL, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + y + ', left=' + x);
}

function UnFormatNumber(currency) {
    var number = 0;
    if (currency != null) {
        currency = currency.toString().replace(/\./g, "");
        if ((currency.match(/,/g) || []).length > 1) {
            currency = currency.toString().replace(/,/g, "");
        } else {
            currency = currency.toString().replace(/,/g, ".");
        }
        number = Number(currency.replace(/[^0-9\.-]+/g, ""));
    }
    return number;
}

$.fn.FormatNumberInt = function () {
    $(this).on("keyup", function (event) {
        if (event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40) {
            return;
        }
        var regexNumber = /^[0-9]+$/i;
        var val = this.value;
        val = val.replace(/[^\d.-]+/g, "");
        this.value = "";
        val += '';
        var x = val.split('.');
        var x1 = x[0];

        if (!regexNumber.test(x1)) {
            return;
        }
        if (x1.length > 1) {
            x1 = parseInt(x1.replace(/^0/, '')).toString();
        }
        this.value = x1;
    });
}

var _timeStamp = 0;
$.fn.FormatCurrency = function () {
    $(this).on("keyup", function (event) {
        if (_timeStamp === event.timeStamp) {
            return;
        }
        _timeStamp = event.timeStamp;
        var regexNumber = /^[0-9]+$/i;

        if (event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40) {
            return;
        }
        var val = this.value;
        if (val.length > 1) {
            val = val.replace(/^0/, '').toString();
        }
        var maxLen = this.maxLength, len = val.length, dotNumber = (val.split(',').length - 1) + (val.split('.').length - 1);
        var subNumber = maxLen - ((maxLen / 3).toFixed(0) - 1 - dotNumber);
        val = val.toString().substr(0, subNumber);
        val = val.replace(/[^\d,-]/g, "");
        this.value = "";
        val += '';
        var x = val.split(',');
        var x1 = x[0];
        var x2 = x.length > 1 ? ',' + x[1] : '';

        var rgx = /(\d+)(\d{3})/;
        if (!regexNumber.test(x1) || (x2 !== "," && x2.replace(/,/g, "") !== "" && !regexNumber.test(x2.replace(/,/g, "")))) {
            return;
        }
        //if (x1.length > 1) {
        //    x1 = parseInt(x1.replace(/^0/, '')).toString();
        //}
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + '.' + '$2');
        }
        this.value = x1 + x2;//.replace(/^[0,]/g, "")
    });
}

$.fn.FormatNumberNegativeInt = function () {
    $(this).on("keyup", function () {
        var regexNumber = /^[0-9\-]+$/i;

        if (event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40) {
            return;
        }
        var val = this.value;
        val = val.replace(/[^\d,-]/g, "");
        this.value = "";
        val += '';
        var x = val.split(',');
        var x1 = x[0];
        var x2 = x.length > 1 ? ',' + x[1] : '';

        var rgx = /(\d+)(\d{3})/;
        if (!regexNumber.test(x1) || (x2 !== "," && x2.replace(/,/g, "") !== "" && !regexNumber.test(x2.replace(/,/g, "")))) {
            return;
        }
        if (x1.length > 1) {
            x1 = parseInt(x1.replace(/^0/, '')).toString();
        }
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + '.' + '$2');
        }
        this.value = x1 + x2;//.replace(/^[0,]/g, "")
    });
}

function validFromdateToDate(startdatestring, enddatestring) {
    if (startdatestring == undefined || startdatestring == null || startdatestring == "" ||
        enddatestring == undefined || enddatestring == null || enddatestring == "") {
        return true;
    }
    var startdate = new Date(startdatestring.split("/")[2] + "-" + startdatestring.split("/")[1] + "-" + startdatestring.split("/")[0]);
    var enddate = new Date(enddatestring.split("/")[2] + "-" + enddatestring.split("/")[1] + "-" + enddatestring.split("/")[0]);
    if (startdate > enddate) {
        return false;
    }
    else {
        return true;
    }
}

function getNewsTypeName(newsType) {
    if (newsType == 1) {
        return "Tin tức";
    }
}

function scrollTo(element) {
    var area = $('html, body');
    if (element == undefined || element == null) {
        element = $("html");
    }
    area.animate({
        scrollTop: element.offset().top - 100
    }, 200);
}


var VideoLibrary = {
    Register: function (avatar, w, h) {
        $('.dvsvideo').each(function (idx, item) {
            id = $(item).attr("id");
            if (id == null || id == undefined || id == '') {
                id = '_dvs_video' + Math.floor((Math.random() * 1000) + 1);
                $(item).attr("id", id);
            }
            VideoLibrary.Init(id, avatar, w, h);
        });
    },
    Init: function (elm, avatar, w, h) {
        if (!elm || $('#' + elm).length < 0) return;

        if (w == undefined) w = 640;
        if (h == undefined) h = 360;

        dataW = $('#' + elm).attr("data-width");
        dataH = $('#' + elm).attr("data-height");
        if (dataW != null && dataW != '') w = dataW;
        if (dataH != null && dataH != '') h = dataH;

        fileSrc = $('#' + elm).attr("data-file");
        if (fileSrc.indexOf('manifest.mpd') != -1) {
            fileSrc = fileSrc.replace('/manifest.mpd', '/playlist.m3u8');
        }
        imageSrc = $('#' + elm).attr("data-image");
        if (imageSrc == undefined || imageSrc == '' || imageSrc == "/Content/images/no-image.png" || imageSrc == "/Content/images/avtvideo.png") imageSrc = avatar;
        jwplayer(elm).setup({
            "autostart": false,
            "width": w,
            "height": h,
            "file": fileSrc,
            "image": imageSrc
        });
    },
    PlayMultipleVideo: function (className, avatar) {
        $(className).each(function (idx, item) {
            id = $(item).attr("id");
            if (id == null || id == undefined || id == '') {
                id = '_dvs_video' + Math.floor((Math.random() * 1000) + 1);
                $(item).attr("id", id);
            }
            VideoLibrary.Init(id, avatar);
        });
    }
};

function getParam(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return results[1];
}
function getvalofparam(h, _u) {
    if (typeof (h) == "undefined") var h = 'c';
    //else h = h.toLowerCase();
    if (typeof (_u) == "undefined") var _u = getparameter(0);
    var _arrParam = _u.split('/');
    var val = '0';
    for (var _index = 0; _index < _arrParam.length; _index++) {
        if (_arrParam[_index].indexOf(h) >= 0) {
            val = _arrParam[_index].replace(h, '');
            break;
        }
    }
    return val;
}

// Example: url: http://example/#page1
// getparameter(0) = 1;
function getparameter(p) {
    if (typeof (p) == "undefined") p = 0;
    var param = location.href;
    var val = '';
    if (param.indexOf('#') >= 0) {
        val = param.substring(param.indexOf('#') + 1, param.length);
        switch (p) {
            case 0:
                if (val.indexOf('?') > 0)
                    val = val.substring(0, val.indexOf('?'));
                if (val.indexOf('&') > 0)
                    val = val.substring(0, val.indexOf('&'));
                break;
            case 1: // Is Exist character "?". Exp url: #anticipate?m=1
                if (val.indexOf('?') >= 0)
                    val = '&' + val.substring(val.indexOf('?') + 1, val.length);
                else val = '';
                break;
        }
    } else val = '';
    return val;
}

function Rawparam(h, v) {
    if (typeof (h) == "undefined") var h = 'c';
    var _u = getparameter(0);
    if (_u.indexOf(h) < 0) _u = _u.concat('/').concat(h).concat('0');
    _u = location.href.substring(0, location.href.indexOf("#")).concat('#').concat(_u.replace(h + getvalofparam(h, _u), h + v))
    document.location = _u;
}

function RawparamPos(val, p) {
    if (typeof (p) == "undefined") var p = 0;
    var params = location.href;
    params = params.indexOf('#') >= 0 ? params.substring(params.indexOf('#') + 1, params.length) : "";
    if (params == "") {
        location.href = location.href + "#" + val;
    }
    else {
        var v = params;
        if (params.indexOf('/') >= 0) {
            v = params.split('/');
            location.href = location.href.replace(v[p], val);
        }
        else {
            location.href = location.href.substring(0, location.href.indexOf('#')) + "#" + val;
        }
    }
    return false;
}

function getHostName(url) {
    var match = url.match(/:\/\/(www[0-9]?\.)?(.[^/:]+)/i);
    if (match != null && match.length > 2 && typeof match[2] === 'string' && match[2].length > 0) {
        return match[2];
    }
    else {
        return null;
    }
}