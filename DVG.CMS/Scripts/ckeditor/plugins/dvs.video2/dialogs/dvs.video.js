var inputId = null;
var _file = null;
var _count = 0;
var _linkVideoReal = null;
var _duration = null;
var _linkThumbReal = null;
var _linkThumbTemp = null;
var _firstThumbTemp = null;
CKEDITOR.dialog.add('VideoDialog2', function (editor) {
    var plugin = CKEDITOR.plugins.get('dvs.video2');
    CKEDITOR.document.appendStyleSheet(CKEDITOR.getUrl(plugin.path + 'dialogs/video.css'));
    return {
        title: 'Video',
        minWidth: 500,
        minHeight: 300,
        contents: [
            {
                id: 'tab-video',
                padding: 0,
                height: 450,
                width: 500,
                elements:
                    [
                        {
                            type: 'html',
                            html: '<div id="previewVideo" class="dvsvideo" style="width:500px; height:294px; border: 1px solid #3d3d3d;">&nbsp<div>'
                        },
                        {
                            type: 'html',
                            html: '<div id="thumbVideo" class="dvsthumb" style="width:500px;float:left"><div>'
                        },
                        {
                            type: 'file',
                            id: 'videofile',
                            style: 'opacity: 0; filter:alpha(opacity=0);border: none;padding: 0;margin: 0;width: 0;height: 0;'
                        },
                        {
                            type: 'hbox',
                            widths: ['30%', "70%"],
                            //style: 'margin-top: 10px',
                            children: [
                                {
                                    type: 'button',
                                    id: 'btnUpload',
                                    label: 'Tải lên',
                                    onClick: function () {
                                        var dialog = CKEDITOR.dialog.getCurrent();
                                        var txtFile = dialog.getContentElement('tab-video', 'videofile');
                                        var input = document.getElementById($("#" + txtFile.domId + " iframe").attr("id")).contentDocument.getElementById(txtFile.getInputElement().getId());
                                        input.click();
                                        $("#statusbarBox").empty();
                                        $(input).on("change", function () {
                                            if (_count === 0) {
                                                _count++;
                                                uploadData.Upload(this.files, this, function (result, elem) {
                                                    RefreshUpload(elem);
                                                    if (result) {
                                                        //uploadData.CopyOutFromTempFolder(result.Data.Url, function (response) {
                                                        //    if (response) {
                                                        //        _count = 0;
                                                        //        var data = JSON.parse(response);
                                                        var linkVideo = cms.configs.VideoViewDomain + result.Data.Url + "/manifest.m3u8";
                                                        //Init preview
                                                        var previewElem = $("#previewVideo");
                                                        previewElem.attr({
                                                            'data-file': linkVideo
                                                        }).css({
                                                            width: '500px',
                                                            height: '294px',
                                                            'text-align': 'center'
                                                        }).addClass('dvsvideo');
                                                        jwVideo.Init(previewElem.attr('id'));

                                                        //        //Set link to text box
                                                        //        CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtLinkVideo", linkVideo);
                                                        //        _linkVideoReal = data.OK;
                                                        //    }
                                                        //});

                                                        _duration = result.Data.Duration;
                                                        if (result.Data) {
                                                            _firstThumbTemp = result.Data.Images[0];
                                                            for (i = 0; i < result.Data.Images.length; i++) {
                                                                var linkThumb = result.Data.Images[i];
                                                                var imgThumb = document.createElement("img");
                                                                imgThumb.setAttribute('src', 'https://video-thumb.tinxe.vn/resize/620x-/' + result.Data.Images[i]);
                                                                imgThumb.setAttribute('style', 'width:111px;height:auto; margin:6px;float:left;cursor: pointer;border: 1px solid #FFFFFF;');
                                                                imgThumb.setAttribute('onclick', 'getThumb(this,"' + linkThumb + '", "' + result.Data.Url + '")');
                                                                document.getElementById("thumbVideo").appendChild(imgThumb);
                                                            }
                                                        }
                                                    }
                                                });
                                            }
                                        });

                                    }
                                },
                                {
                                    type: 'html',
                                    html: '<div id="statusbarBox"></div>'
                                }
                            ]
                        },
                        {
                            type: 'text',
                            id: 'txtLinkVideo',
                            label: 'Link video',
                            style: 'margin: 10px 0;',
                            validate: function () {
                                if (!this.getValue()) {
                                    alert('Vui lòng chọn ảnh thumb để lấy thông tin link video!');
                                    return false;
                                }

                                var urlRegEx = /^(https?:\/\/(?:www\.|(?!www))([filev1]+\.[tinxe]{2,})\.vn\/)/igm;
                                if (this.getValue().search(urlRegEx) == -1) {
                                    alert('Link không hợp lệ!');
                                    return false;
                                }
                                return true;
                            },
                            setup: function (e) {
                                var value = e.getAttribute("data-video");

                                if (!value) {
                                    return;
                                }

                                CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtLinkVideo", value);
                            }
                        }
                    ]
            }
        ],
        onLoad: function () {
            uploadData.Init();
        },
        onShow: function () {
            var inputLinkVideo = CKEDITOR.dialog.getCurrent().getContentElement('tab-video', 'txtLinkVideo');
            $("#" + inputLinkVideo.domId).find("input").focus();
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
            $("#previewVideo").replaceWith('<div id="previewVideo" class="dvsvideo" style="width:500px; height:294px; border: 1px solid #3d3d3d;">&nbsp<div>');
            //var videoId = $('.dvsvideo').attr("id");
            //jwVideo.Init(videoId);
        },
        buttons: [CKEDITOR.dialog.okButton, CKEDITOR.dialog.cancelButton],
        onOk: function (e) {
            var dialog = this;
            var defaultimagevideo = 'https://img.tinxe.vn/2017/08/30/ikl8LTEh/avtvideo-1546.png';
            var videoLink = dialog.getValueOf('tab-video', 'txtLinkVideo');
            //var subtitleLink = dialog.getValueOf('tab-video', 'txtLinkSubtitle');
            //var width = dialog.getValueOf('tab-video', 'txtWidth');
            //var height = dialog.getValueOf('tab-video', 'txtHeight');
            var width = jwVideo.Width;
            var height = jwVideo.Height;
            //var imageback = dialog.getValueOf('tab-video', 'txtImageUrl');
            //if (!imageback) {
            var imageback = defaultimagevideo;
            //}

            var jwId = "player" + new Date().getTime();
            var element = dialog.element;

            element.setAttribute("id", jwId);
            element.setAttribute("data-file", videoLink);
            //element.setAttribute("data-subtitle", subtitleLink);
            element.setAttribute("data-width", width);
            element.setAttribute("data-height", height);
            element.setAttribute("data-image", cms.configs.VideoThumbDomain + _linkThumbReal);
            //element.setAttribute("style", "width: " + width + "px; height: " + height + "px; text-align: center; border: 1px solid #CCC; margin: 5px auto;");
            element.setHtml('<img src="' + cms.configs.VideoThumbDomain + _linkThumbReal + '" style="width: ' + width + 'px; height: ' + height + 'px;max-width: 680px" />');
            element.setAttribute("class", "dvsvideo dvg_video_box");
            element.setAttribute("data-width", width);
            element.setAttribute("data-raw", _linkVideoReal);
            element.setAttribute("data-duration", _duration);
            element.setAttribute("data-raw-image", _linkThumbReal);
            dialog.commitContent(element);
            if (dialog.insertMode) {
                editor.insertElement(element);
            }
            // save duration sang form
            bindNewsVideoDuration(_duration);
            var thumbVideo = document.getElementById("thumbVideo");
            var fc = thumbVideo.firstChild;

            while (fc) {
                thumbVideo.removeChild(fc);
                fc = thumbVideo.firstChild;
            }

            try {
                var inputLinkVideo = CKEDITOR.dialog.getCurrent().getContentElement('tab-video', 'txtLinkVideo');
                var input = document.getElementById($("#" + inputLinkVideo.domId + " iframe").attr("id")).contentDocument.getElementById(inputLinkVideo.getInputElement().getId());
                $(input).focus();
            }
            catch (Ex) {
                //
            }
        },
        onHide: function (e) {
            $("#previewVideo").replaceWith('<div id="previewVideo" class="dvsvideo" style="width:500px; height:294px; border: 1px solid #3d3d3d;">&nbsp<div>');

            var thumbVideo = document.getElementById("thumbVideo");
            var fc = thumbVideo.firstChild;

            while (fc) {
                thumbVideo.removeChild(fc);
                fc = thumbVideo.firstChild;
            }
        }
    };
});

var jwVideo = {
    Width: 500,
    Height: 294,
    Init: function (elm, w, h) {
        if (!elm || $('#' + elm).length < 0) return;

        if (w == undefined) w = 500;
        if (h == undefined) h = 294;
        fileSrc = $('#' + elm).attr("data-file");
        if (fileSrc) {
            currentProtocol = location.protocol;
            if (currentProtocol == "http:")
                fileSrc = fileSrc.replace(/^https?:/i, "");

            imageSrc = $('#' + elm).attr("data-image");
            jwplayer(elm).setup({
                // playlist: [{
                // file: fileSrc,
                // type: "dash"
                // }],
                "autostart": false,
                "width": w,
                "height": h,
                "file": fileSrc,
                type: "dash",
                "image": imageSrc
            });
        }
    }
}

var uploadData = {
    Tokenkey: "",
    Init: function () {
        this.GetTokenKey();

    },
    GetTokenKey: function () {
        var jqXHR = $.ajax({
            url: "/News/GetTokenKeyForUpload",
            type: "POST",
            contentType: false,
            processData: false,
            cache: false,
            success: function (response) {
                uploadData.Tokenkey = response.Data;
            }
        });
    },
    Upload: function (files, elem, callBack) {
        var obj = $("#statusbarBox");
        //var files = document.getElementById('video-file').files;
        if (files.length > 0) {
            handleUpload(files, obj, this.Tokenkey, function (result) {
                callBack(result, elem);
                RefreshUpload(elem);
                if (!result) {
                    setTimeout(function () {
                        $("#statusbarBox").empty().html("<span style='color:red'>Đã xảy ra lỗi khi tải lên</span>");
                    }, 1000);
                }
            });

        }
    },
    CopyOutFromTempFolder: function (tempPath, callBack) {
        var jqXHR = $.ajax({
            url: "/News/CopyOutFromTempFolder",
            type: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            processData: false,
            cache: false,
            data: JSON.stringify({ tempPath: tempPath }),
            success: function (response) {
                callBack(response.Data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                callBack(tempPath);
                console.log('ERRORS: ' + textStatus);
            }
        });
    },
    requestRealImg: function (linkTemp, linkVideoTemp, callBack) {
        var obj = $("#statusbarBox");
        requestImage(this.Tokenkey, linkTemp, linkVideoTemp, obj, function (result) {
            callBack(result);
            if (!result) {
                setTimeout(function () {
                    $("#statusbarBox").empty().html("<span style='color:red'>Đã xảy ra lỗi khi tải lên</span>");
                }, 1000);
            }
        });
    }
}

/**
 * Author: [2016.03.23][DucMV]
 * Upload file to server
 * @formData : form data to upload
 * @status : status for progress
 * @domainUpload : name of domain to upload
*/
function sendFileToServer(formData, status, callBack) {
    var uploadURL = cms.configs.VideoUploadDomainClient + cms.configs.UploadHandler;//domainUpload + '/UploadHandler.php'; //Upload URL
    var progressbar = $("#statusbarBox .progressBar");
    var pendingProgress =
        '<div id="squaresWaveG">' +
        '<div id="squaresWaveG_1" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_2" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_3" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_4" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_5" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_6" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_7" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_8" class="squaresWaveG"></div></div>';

    var ajax = new XMLHttpRequest();
    ajax.upload.addEventListener("progress",
        function (event) {
            var percent = 0;
            var position = event.loaded || event.position;
            var total = event.total;
            if (event.lengthComputable) {
                percent = Math.ceil(position / total * 100);
            }
            //Set progress
            status.setProgress(percent);
            if (percent === 100) {
                progressbar.empty().removeClass('progressBar').addClass('progressPending').html(pendingProgress);
            }
        }, false);
    ajax.addEventListener("load", function (data) {
        status.setProgress(100);
        status.hideProgressbar();
        var result = JSON.parse(data.target.response);
        callBack(result);
    }, false);
    ajax.addEventListener("error", errorHandler, false);
    ajax.addEventListener("abort", abortHandler, false);
    ajax.open("POST", uploadURL, true);
    ajax.send(formData);
}

function completeHandler(event) {
    _("status").innerHTML = event.target.responseText;
    _("progressBar").value = 0;
}
function errorHandler(event) {
}
function abortHandler(event) {
}
/**
 * Author: [2016.03.23][DucMV]
 * Create progress bar
 * @obj : The element container progress upload
*/
function createStatusbar(obj) {
    this.statusbar = $("<div class='statusbar'></div>").appendTo(obj);
    this.filename = $("<div class='filename'></div>").appendTo(this.statusbar);
    this.size = $("<div class='filesize'></div>").appendTo(this.statusbar);
    this.progressBar = $("<div class='progressBar'><div></div></div>").appendTo(this.statusbar);
    //obj.after(this.statusbar);

    this.setFileNameSize = function (name, size) {
        var sizeStr = "";
        var sizeKB = size / 1024;
        if (parseInt(sizeKB) > 1024) {
            var sizeMB = sizeKB / 1024;
            sizeStr = sizeMB.toFixed(2) + " MB";
        }
        else {
            sizeStr = sizeKB.toFixed(2) + " KB";
        }

        this.filename.html(name);
        this.size.html(sizeStr);
    }
    this.setProgress = function (progress) {
        var progressBarWidth = progress * this.progressBar.width() / 100;
        this.progressBar.find('div').animate({ width: progressBarWidth }, 10).html(progress + "% ");
    }
    this.hideProgressbar = function () {
        this.statusbar.remove();
    }
}
/**
 * Author: [2016.03.23][DucMV]
 * Handle upload file
 * @files : The files object to upload
 * @obj : The element container progress upload
 * @tokenString : token to upload
 * @domainUpload : domain name to upload
*/
function handleUpload(files, obj, tokenString, callBack) {
    for (var i = 0; i < files.length; i++) {
        var fd = new FormData();
        //fd.append('id', 'TTR');
        fd.append('fileUpload', files[i]);
        fd.append('StringDecypt', tokenString);
        fd.append('UploadType', 'upload');
        fd.append('project', cms.configs.UploadProject);
        //fd.append('submit', 'Upload Image');
        var status = new createStatusbar(obj); //Using this we can set progress.
        status.setFileNameSize(files[i].name, files[i].size);
        sendFileToServer(fd, status, function (result) {
            callBack(result);
        });
    }
}

/**
 * Author: [2016.03.23][DucMV]
 * Refresh input data control
 * @file : The file object to upload
*/
function RefreshUpload(elem) {
    var control = $(elem);
    control.replaceWith(control.val('').clone(true));
    _file = null;
}

/**
 * Author: [2016.03.23][DucMV]
 * Handle upload file
 * @files : The files object to upload
 * @obj : The element container progress upload
 * @tokenString : token to upload
 * @domainUpload : domain name to upload
*/
function requestImage(tokenString, linkTemp, linkVideoTemp, obj, callBack) {
    var fd = new FormData();
    fd.append('FileTemp', linkVideoTemp);
    fd.append('ImageFileTemp', linkTemp);
    fd.append('project', cms.configs.UploadProject);
    fd.append('UploadType', 'copy');
    fd.append('StringDecypt', tokenString);
    fd.append('submit', 'Upload Image');
    var status = new createStatusbar(obj);
    sendRequestGetImg(fd, status, function (result) {
        callBack(result);
    });
}

function sendRequestGetImg(formData, status, callBack) {
    var uploadURL = cms.configs.VideoUploadDomainClient + cms.configs.UploadHandler;

    var progressbar = $("#statusbarBox .progressBar");
    var pendingProgress =
        '<div id="squaresWaveG">' +
        '<div id="squaresWaveG_1" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_2" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_3" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_4" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_5" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_6" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_7" class="squaresWaveG"></div>' +
        '<div id="squaresWaveG_8" class="squaresWaveG"></div></div>';

    var ajax = new XMLHttpRequest();

    ajax.upload.addEventListener("progress",
        function (event) {
            var percent = 0;
            var position = event.loaded || event.position;
            var total = event.total;
            if (event.lengthComputable) {
                percent = Math.ceil(position / total * 100);
            }
            //Set progress
            status.setProgress(percent);
            if (percent === 100) {
                progressbar.empty().removeClass('progressBar').addClass('progressPending').html(pendingProgress);
            }
        }, false);

    ajax.addEventListener("load", function (data) {
        status.setProgress(100);
        status.hideProgressbar();
        var result = JSON.parse(data.target.response);
        callBack(result);
    }, false);
    ajax.addEventListener("error", errorHandler, false);
    ajax.addEventListener("abort", abortHandler, false);
    ajax.open("POST", uploadURL, true);
    ajax.send(formData);
}

function getThumb(e, linkThumbTemp, linkVideoTemp) {

    var thumbVideo = document.getElementById("thumbVideo");
    var list = thumbVideo.getElementsByTagName("IMG");
    for (i = 0; i < list.length; i++) {
        list[i].style = "width:111px;height:auto; margin:6px;float:left;cursor: pointer;border: 1px solid #FFFFFF;";
    }
    e.style = "width:111px;height:auto; margin:6px;float:left;cursor: pointer;border: 1px solid #FF0000;";

    uploadData.requestRealImg(linkThumbTemp, linkVideoTemp, function (result) {
        _count = 0;
        _linkThumbReal = result.Data.ImageUrl;
        var linkVideo = cms.configs.VideoViewDomain + result.Data.Url + "/manifest.m3u8";
        //Set link to text box
        CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtLinkVideo", linkVideo);
        _linkVideoReal = result.Data.Url;

        var previewElem = $("#previewVideo");
        previewElem.attr({
            'data-file': linkVideo
        }).css({
            width: '500px',
            height: '294px',
            'text-align': 'center'
        }).addClass('dvsvideo');
        jwVideo.Init(previewElem.attr('id'));

    });
}