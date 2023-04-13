//if (uploadProject === undefined || uploadProject === null || uploadProject === "") {
//    uploadProject = "OpenCard";
//}
(function (window, $) {
    if (window.fm) {
        return;
    }

    var fm = {
        version: "1.0.0",
        dropzone: null,
        listview: null,
        tree: null,
        data: null,
        selectedList: [],
        action: {
            name: "",
            file: "",
            folder: ""
        },
        option: {
            fileManagerCommitButton: "",
            toolbarLeftButton: "",
            switchListViewButton: "",
            dropzoneSelector: "",
            listFileArea: "",
            listView: "",
            listViewItem: "",
            treeSelector: "",
            folderExplore: "",
            fileExplore: "",
            fileItem: "",
            folderItem: "",
            fileItemClassSelected: "",
            startDateSearchFile: "",
            endDateSearchFile: "",
            searchFileButton: "",
            uploadChoose: "",
            uploadButton: "",
            statusSelector: "",
            loading: "",
            sidepanels: "",
            sidepanel_title: "",
            infopanel: "",
            selectedListPanel: "",
            sortableSelectedList: "",
            infoPanelImage: "",
            infoDetailDimension: "",
            infoDetailFileSize: "",
            infoDetailTime: "",
            infoDetailUser: "",
            infoDetailDescription: "",
            openCropPanelButton: "",
            cropPanel: "",
            cropPannelImage: "",
            fileInfoName: "",
            fileInfoTitle: "",
            editInfoSaveButton: "",
            multiFileBrowser: "",
            listFileBrowser: "",
            crawlUrlPanel: "",
            listCrawlFromOtherPage: "",
            uploadCrawlImagesButton: "",
            crawlUrlFromPageText: "",
            crawlUrlFromPageButton: "",
            crawlUrlFromPagefilter: "",
            crawlImagesListting: "",
            uploadCrawlPageButton: "",
            crawlPagefileItem: "",
            fileBrowserButton: "",
            crawlPageButton: "",
            addLogoToImageButton: "",
            addLogoPanel: "",
            btnAddLogoGroup: "",
            addLogoPannelImage: "",
            loadFileAndFolderOption: {
                urlLoadFileAndFolder: "",
                pathParameter: ""
            },
            uploadFileOption: {
                urlUploadFile: "",
                folderParameter: ""
            },
            fileActionOption: {
                urlFileAction: "",
                actionParameter: "",
                fileParameter: "",
                folderParameter: ""
            },
            uploadFileCondition: {
                acceptFileTypes: "",
                maxFileSize: 0, //MB
                limitUpload: 0
            },
            loadingImage: "",
            folderDefault: "",
            folderSeparator: "",
            createOpenFileItem: null,
            createFileItem: null,
            SelectedListItem: null,
            CrawlListItem: null,
            panelTypeEnum: null,
            SIDEPANEL_OPENWIDTH: 0,
            LISTING_PAGEINDEX: 0,
            LISTING_PAGESIZE: 0,
            ADD_LOGO_PADDING_POSITION: 0
        }
    };
    //var files,
    var selectedFolder = "", workarea, CURRENT_TAB = 1, UploadCrawlPageCount = 0, UploadCrawlImagesCount = 0, UploadCrawlImagesTotal = 0, CURRENT_FILE_OBJECT = null;
    var CURRENT_LOGO_BASE64 = "", CURRENT_IMAGE_BASE64 = "";

    fm.init = function (option) {
        for (var i in fm.option) {
            if (!option[i]) {
                continue;
            }

            fm.option[i] = option[i];
        }
        workarea = $(fm.option.listFileArea);
        if (!fm.option.folderDefault) {

            var homnay = new Date(_dt_today_stamp);

            var tzoffset = homnay.getTimezoneOffset() * 60000; //offset in milliseconds
            var strhomnay = (new Date(homnay - tzoffset)).toISOString().substring(0, 10).replace(/-/g, "/");

            fm.option.folderDefault = strhomnay;
        }

        selectedFolder = fm.option.folderDefault;

        $(fm.option.toolbarLeftButton).click(function () {
            var type = $(this).attr('data-type');
            if (CURRENT_TAB !== type) {
                fm.selectedList = [];
                $(fm.option.toolbarLeftButton).removeClass('active');
                $(this).addClass('active');
                CURRENT_TAB = type;
            }

            switch (type) {
                case '1': { fm.bindClickMainPanel(); break; }
                case '2': { fm.bindClickOpenFilesPanel(); break; }
                case '3': { fm.bindCrawlUrlPanel(); break; }
                case '4': { fm.bindCrawlFromOtherPagePanel(); break; }
            }
        });

        $("#chkIsLarge").click(function () {
            if ($(this).prop("checked") === true) {
                if (!confirm("Chỉ tích khi ảnh lớn hơn 1000px. Bạn có chắc chắn ảnh của bạn >1000px?")) {
                    $(this).prop("checked", false);
                }
            }
        });

        Dropzone.options.dropzoneForm = {
            url: fm.option.uploadFileOption.urlUploadFile,
            margin: 20,
            allowedFileTypes: 'image.*, pdf',
            params: {
                'action': 'save'
            },
            clickable: false,
            preview: true,
            uploadOnDrop: true,
            uploadOnPreview: true,
            thumbnailWidth: 100,
            thumbnailHeight: 100,
            init: function () {
                fm.load();
            },
            sending: function (file, xhr, formData) {
                formData.append('project', uploadProject);
                formData.append('UploadType', 'upload');
                if ($("#chkIsLarge").prop("checked") === true) {
                    formData.append('IsLarge', '1');
                }
                formData.append('StringDecypt', $('#hfToken').val());
                formData.append('submit', 'Upload Image');
                formData.append('fileToUpload', file);
            },
            success: function (res, index) {
                var result = JSON.parse(res.xhr.response);

                if (result.ErrorCode !== 200) {
                    alert("Không thể upload tệp tin: " + fileName);
                }
                else {
                    fm.updateFileInfoToDb(result.Url, result, function (d) {
                        fm.load();
                    });
                }
            },
            accept: function (file, done) {
                var fileName = file.name;

                if (fm.option.uploadFileCondition.acceptFileTypes.exec(fileName) === null) {
                    return done('Định dạng file không được chấp nhận!\n(' + fileName + ')');
                }

                if (file.size > Math.pow(1024, 2) * fm.option.uploadFileCondition.maxFileSize) {
                    return done('Kích thước không cho phép! Chỉ được upload file < ' + fm.option.uploadFileCondition.maxFileSize + 'MB\n(' + fileName + ')');
                }

                return done();
            }
        };

        jQuery.fn.jplist.settings = {
            datepicker: function (input, options) {

                //set options
                options.dateFormat = 'dd/mm/yy';
                options.locale = "vi";
                options.changeMonth = true;
                options.changeYear = true;

                //start datepicker
                input.datepicker(options);
            }

        };

        $(fm.option.fileExplore).multiSelect({
            unselectOn: fm.option.dropzoneSelector,
            keepSelection: false,
        });

        $(fm.option.crawlImagesListting).multiSelect({
            unselectOn: fm.option.crawlImagesListting,
            keepSelection: false,
        });

        // bind item-selected vào list chèn
        $(fm.option.dropzoneSelector).click(function () {
            fm.pushSelectedItemToList();
            if (fm.selectedList.length <= 0 && $(fm.option.sidepanels).is(':visible')) {
                //$(fm.option.sidepanels).hide();
                fm.toggleSidePanel(true);
            }
        })

        $(fm.option.uploadButton).click(function () {

            //Vì quy định upload theo ngày tháng năm nên set lại folder được chọn thành thư mục gốc
            //Nếu muốn upload theo thư mục được chọn thì bỏ phần này đi
            //selectedFolder = fm.option.folderDefault;
            var fileNames = new Array();

            $.each(files, function (key, value) {

                var fileName = value.name;

                if (fm.option.uploadFileCondition.acceptFileTypes.exec(fileName) === null) {
                    alert('Định dạng file không được chấp nhận!\n(' + fileName + ')');

                    flag = false;

                    return false;
                }

                if (value.size > Math.pow(1024, 2) * fm.option.uploadFileCondition.maxFileSize) {
                    alert('Kích thước không cho phép! Chỉ được upload file < ' + fm.option.uploadFileCondition.maxFileSize + 'MB\n(' + fileName + ')');

                    flag = false;

                    return false;
                }

                var data = new FormData();

                data.append('project', uploadProject);
                data.append('UploadType', 'upload');
                if ($("#chkIsLarge").prop("checked") === true) {
                    data.append('IsLarge', '1');
                }
                data.append('StringDecypt', $('#hfToken').val());
                data.append('submit', 'Upload Image');
                data.append('fileToUpload', value);

                $.ajax({
                    url: fm.option.uploadFileOption.urlUploadFile,
                    type: "POST",
                    data: data,
                    dataType: "json",
                    processData: false, // Don't process the files
                    contentType: false, // Set content type to false as jQuery will tell the server its a query string request
                    async: false,
                    success: function (result) {
                        if (!result.OK) {
                            alert("Không thể upload tệp tin: " + fileName);
                        }

                    },
                    error: function (ex) {
                        alert("Error");
                        console.log(ex);
                    }
                });
            });

            fm.load();
        });

        $(fm.option.uploadChoose).change(function (e) {
            var files = this.files;
            if (files.length > 0)
                fm.uploadFiles(files, fm.bindListUploadedOpenFilesPanel);
        });

        $(fm.option.folderExplore).niceScroll({ touchbehavior: false, /*cursorcolor: "#0000FF", */ cursoropacitymax: 0.6, cursorwidth: 5 });
        $(fm.option.fileExplore).niceScroll({ touchbehavior: false, /*cursorcolor: "#0000FF", */ cursoropacitymax: 0.6, cursorwidth: 5 });
        $(fm.option.sidepanels).niceScroll({ touchbehavior: false, /*cursorcolor: "#0000FF", */ cursoropacitymax: 0.6, cursorwidth: 5 });

        $(fm.option.searchFileButton).click(function () {
            fm.load();
        });

        $(fm.option.openCropPanelButton).click(function () {
            fm.loading(true);
            var imgObj = $(fm.option.infoPanelImage);
            var urlImage = imgObj.attr('src'); //local bi cross domain
            var originalW = imgObj.attr('data-width');
            var originalH = imgObj.attr('data-height');
            var contentType = 'image/jpeg';
            var apiUrl = fm.option.fileActionOption.urlFileAction + '?' + fm.option.fileActionOption.actionParameter + '=convertUrlImagesToBase64';
            $.ajax({
                type: "POST",
                url: apiUrl,
                data: { url: urlImage },
                success: function (data) {
                    if (data.Success === true) {
                        $(fm.option.cropPannelImage).attr('src', 'data:' + contentType + ';base64,' + data.Data);
                        $(fm.option.cropPannelImage).attr('data-width', originalW).attr('data-height', originalH);
                        $(fm.option.cropPanel).modal();
                        // xử lý load lại ảnh lần 2
                        var isCheck = $('#aspectRatio5').parent().hasClass('active');
                        if (isCheck) {
                            $('#aspectRatio1').click();
                            $('#aspectRatio5').click();
                        }
                        else
                            $('#aspectRatio5').click();
                    }
                    fm.loading(false);
                },
                error: function (msg) {
                    fm.loading(false);
                    return false;
                }
            });
        });

        $(fm.option.uploadCrawlImagesButton).click(function () {
            var textArea = $(fm.option.crawlUrlPanel).find('textarea');
            var urls = textArea.val();
            if (urls.length <= 0) return;
            fm.loading(true);
            UploadCrawlImagesTotal = 0;
            UploadCrawlImagesCount = 0;
            var splits = urls.split('\n');
            UploadCrawlImagesTotal = splits.length;
            for (var i = 0; i < splits.length; i++) {
                var link = splits[i];
                if (link !== null && link.length > 0) {
                    fm.uploadFromUrl(link, function (e) {
                        UploadCrawlImagesCount++;
                        if (UploadCrawlImagesCount === UploadCrawlImagesTotal) {
                            textArea.val('');
                            $(fm.option.crawlUrlPanel).modal('hide');
                            fm.loading(false);
                            $(fm.bindClickMainPanel).click();
                        }
                    });
                }
            }
        });

        $(fm.option.crawlUrlFromPageButton).click(function () {
            fm.loading(true);
            $(fm.option.crawlPageButton).hide();
            $(fm.option.crawlImagesListting).html('');
            fm.selectedList = [];
            CrawlImagesFromUrlLargeCount = 0;
            CrawlImagesFromUrlLargeTotal = 0;
            var url = $(fm.option.crawlUrlFromPageText).val();
            var isLarge = $(fm.option.crawlUrlFromPagefilter).is(":checked");
            $.ajax({
                type: "POST",
                url: fm.option.fileActionOption.urlFileAction + '?' + fm.option.fileActionOption.actionParameter + '=crawlImagesFromUrl',
                data: { url: url },
                success: function (data) {
                    if (data.Success === true) {
                        for (var i = 0; i < data.Data.length; i++) {
                            var obj = data.Data[i];
                            if (isLarge) {
                                $("<img/>").attr("src", obj).load(function () {
                                    if (this.width >= 400 || this.height >= 400) {
                                        var item = fm.option.CrawlListItem($(this).attr('src'));
                                        $(fm.option.crawlImagesListting).append(item);
                                        //fm.bindClickCrawlPagefileItem();
                                    }
                                });
                            }
                            else {
                                var item = fm.option.CrawlListItem(obj);
                                $(fm.option.crawlImagesListting).append(item);
                            }
                            $(fm.option.crawlPageButton).show();
                        }
                    }

                    //if (!isLarge) {
                    //    fm.bindClickCrawlPagefileItem();
                    //}
                    fm.loading(false);
                },
                error: function (msg) {
                    fm.loading(false);
                    return false;
                }
            });

        });

        $(fm.option.uploadCrawlPageButton).click(function () {
            fm.loading(true);
            $(fm.option.crawlPagefileItem).each(function () {
                var item = { name: $(this).attr('data-full-path'), path: $(this).attr('data-full-path'), desc: "", width: 0, height: 0 };
                if ($(this).hasClass(fm.option.fileItemClassSelected)) {
                    fm.selectedList.pushUnique(item);
                }
                else {
                    var mappedSelectedList = fm.selectedList.map(function (e) {
                        return e.name;
                    });
                    var index = mappedSelectedList.indexOf(item.name);
                    if (index >= 0)
                        fm.selectedList.splice(index, 1);
                }
            });

            if (fm.selectedList === [] || fm.selectedList.length <= 0) { fm.loading(false); return; };

            UploadCrawlPageCount = 0;
            for (var i = 0; i < fm.selectedList.length; i++) {
                var link = fm.selectedList[i].path;
                fm.uploadFromUrl(link, function (e) {
                    fm.loading(true);
                    UploadCrawlPageCount++;
                    if (UploadCrawlPageCount === fm.selectedList.length) {
                        fm.loading(false);
                        $(fm.bindClickMainPanel).click();
                    }
                });
            }

        });

        $(fm.option.fileManagerCommitButton).click(function () {
            var type = $(this).attr('data-type');

            if (type === 'sort' && fm.selectedList.length > 1) {
                var finalSelectedList = [];
                $(fm.option.sortableSelectedList).children().each(function () {
                    var name = $(this).attr('data-name');
                    var mappedSelectedList = fm.selectedList.map(function (e) {
                        return e.name;
                    });
                    var index = mappedSelectedList.indexOf(name);
                    var item = fm.selectedList[index];
                    finalSelectedList.pushUnique(item);
                });
                fm.selectedList = finalSelectedList;
            }

            try {
                window.parent.callback(fm.selectedList);
            }
            catch (ex) {
                console.log(ex);
            }
        });

        $(fm.option.editInfoSaveButton).click(function () {
            if (CURRENT_FILE_OBJECT !== null) {
                fm.loading(true);
                var fileName = CURRENT_FILE_OBJECT.FileName;
                var desc = $(fm.option.infoDetailDescription).val();
                var title = $(fm.option.fileInfoTitle).val();

                var apiUrl = fm.option.fileActionOption.urlFileAction + '?' + fm.option.fileActionOption.actionParameter + '=updateFileinfo';
                $.ajax({
                    type: "POST",
                    url: apiUrl,
                    data: { filename: CURRENT_FILE_OBJECT.FileName, fileurl: CURRENT_FILE_OBJECT.FileUrl, title: title, filesize: CURRENT_FILE_OBJECT.FileSize, width: CURRENT_FILE_OBJECT.Width, height: CURRENT_FILE_OBJECT.Height, mimetypename: CURRENT_FILE_OBJECT.MimeTypeName, description: desc },
                    success: function (data) {
                        if (data.Success === true) {
                            fm.binItemDataUpdated(data.Data);
                        }

                        fm.loading(false);
                    },
                    error: function (msg) {
                        console.log(msg);
                        fm.loading(false);
                    }
                });
            }
        });

        $(fm.option.switchListViewButton).click(function () {
            $(fm.option.switchListViewButton).removeClass('active');
            $(this).addClass('active');
            var type = $(this).attr('data-type');
            if (type === 'dvg-list-view') {
                if (!$(fm.option.dropzoneSelector).hasClass('dvg-list-view')) {
                    $(fm.option.dropzoneSelector).addClass('dvg-list-view');
                }
            }
            else {
                $(fm.option.dropzoneSelector).removeClass('dvg-list-view');
            }
        });

        $(fm.option.addLogoToImageButton).click(function () {
            fm.loading(true);
            var imageUrl = $(fm.option.infoPanelImage).attr('src');
            var logoUrl = logoAddImageUrl;
            var apiUrl = fm.option.fileActionOption.urlFileAction + '?' + fm.option.fileActionOption.actionParameter + '=convertMultiUrlImagesToBase64';
            var contentType = 'image/jpeg';
            $.ajax({
                type: "POST",
                url: apiUrl,
                data: { urls: imageUrl + ";" + logoUrl },
                success: function (data) {
                    if (data.Success === true) {
                        if (data.Data.length > 1) {
                            CURRENT_IMAGE_BASE64 = 'data:' + contentType + ';base64,' + data.Data[0];
                            CURRENT_LOGO_BASE64 = 'data:' + contentType + ';base64,' + data.Data[1];
                            $(fm.option.addLogoPannelImage).attr('src', CURRENT_IMAGE_BASE64);
                            $(fm.option.addLogoPanel).modal();
                        }
                    }
                    fm.loading(false);
                },
                error: function (msg) {
                    fm.loading(false);
                    return false;
                }
            });
        });

        $(fm.option.btnAddLogoGroup).click(function () {
            var method = $(this).attr('data-method');
            console.log(method);

            switch (method) {
                case 'topleft': {
                    watermark([CURRENT_IMAGE_BASE64, CURRENT_LOGO_BASE64])
                        .image(watermark.image.upperLeft(fm.option.ADD_LOGO_PADDING_POSITION))
                        .then(function (img) {
                            if (img && img.src)
                                $(fm.option.addLogoPannelImage).attr('src', img.src);
                        });
                    break;
                }
                case 'topright': {
                    watermark([CURRENT_IMAGE_BASE64, CURRENT_LOGO_BASE64])
                        .image(watermark.image.upperRight(fm.option.ADD_LOGO_PADDING_POSITION))
                        .then(function (img) {
                            if (img && img.src)
                                $(fm.option.addLogoPannelImage).attr('src', img.src);
                        });
                    break;
                }
                case 'botleft': {
                    watermark([CURRENT_IMAGE_BASE64, CURRENT_LOGO_BASE64])
                        .image(watermark.image.lowerLeft(fm.option.ADD_LOGO_PADDING_POSITION))
                        .then(function (img) {
                            if (img && img.src)
                                $(fm.option.addLogoPannelImage).attr('src', img.src);
                        });
                    break;
                }
                case 'botright': {
                    watermark([CURRENT_IMAGE_BASE64, CURRENT_LOGO_BASE64])
                        .image(watermark.image.lowerRight(fm.option.ADD_LOGO_PADDING_POSITION))
                        .then(function (img) {
                            if (img && img.src)
                                $(fm.option.addLogoPannelImage).attr('src', img.src);
                        });
                    break;
                }
                case 'submit': {
                    fm.loading(true);
                    var uriBase64 = $(fm.option.addLogoPannelImage).attr('src');
                    // upload crop
                    var data = new FormData();

                    //fileName
                    var fileName = $(fm.option.fileInfoName).val();
                    data.append('fileName', fileName);
                    data.append('project', 'TinXe');
                    data.append('UploadType', 'uploadFromBase64');
                    data.append('StringDecypt', $('#hfToken').val());
                    data.append('submit', 'Upload Image');
                    data.append('fileToUpload', uriBase64);

                    $.ajax({
                        url: fm.option.uploadFileOption.urlUploadFile,
                        type: "POST",
                        data: data,
                        dataType: "json",
                        processData: false, // Don't process the files
                        contentType: false, // Set content type to false as jQuery will tell the server its a query string request
                        //async: false,
                        success: function (result) {
                            if (result.ErrorCode !== 200) {
                                alert("Không thể upload tệp tin: " + fileName);
                            }
                            else {
                                fm.updateFileInfoToDb(result.Url, result, function (d) {
                                    fm.load();
                                    $(fm.option.addLogoPanel).modal('hide');
                                    fm.loading(false);
                                });
                            }
                        },
                        error: function () {
                            alert("Error");
                            fm.loading(false);
                        }
                    });
                    break;
                }
            }
        });

        $(fm.option.listFileBrowser).hide();

        $(fm.option.cropPanel).hide();

        return fm;
    };

    fm.load = function load() {
        fm.loading(true);
        var startDate = $(fm.option.startDateSearchFile).val();
        var endDate = $(fm.option.endDateSearchFile).val();

        $.ajax({
            type: "POST",
            async: false,
            url: fm.option.fileActionOption.urlFileAction + '?' + fm.option.fileActionOption.actionParameter + '=searchFile',
            data: { from: startDate, to: endDate, pagesize: fm.option.LISTING_PAGESIZE, pageindex: fm.option.LISTING_PAGEINDEX },
            //contentType: "application/json; charset=utf-8",
            //dataType: "json",
            success: function (result) {
                fm.data = result;

                if (result.Success === false) {
                    alert(result.Message);

                    return fm;
                }

                //for (var i = 1; i < 7; i++) {
                //    var homnay = new Date(_dt_today_stamp);
                //    var tzoffset = homnay.getTimezoneOffset() * 60000; //offset in milliseconds
                //    //var ngaystring = (new Date(homnay - tzoffset)).toISOString().substring(0, 10).replace(/-/g, "/");

                //    var ngay = new Date(homnay.setDate(homnay.getDate() - i));
                //    var ngaystring = (new Date(ngay - tzoffset)).toISOString().substring(0, 10).replace(/-/g, "/");
                //    //fm.tree.jstree(true).create_node(fm.tree, { id: ngaystring, text: ngaystring, state: { opened: false } });
                //}

                var html = "";

                for (var i = 0; i < result.Data.length; i++) {
                    var file = result.Data[i];
                    html += fm.option.createFileItem(file);
                }
                $(fm.option.fileExplore).html(html);

                $(fm.option.fileItem).click(function (e) {
                    fm.pushSelectedItemToList();
                    // nếu list có dữ liệu tức là đang chọn ảnh
                    if (fm.selectedList.length > 1) {
                        fm.switchPanel(fm.option.panelTypeEnum.selectedListPanel);
                        //$(fm.option.fileItem).removeClass(fm.option.fileItemClassSelected);
                        fm.bindSelectedListImages();
                    }
                    // nếu list k có gì thì tức là xem details
                    else {
                        fm.switchPanel(fm.option.panelTypeEnum.detailInfoPanel);
                        //$(fm.option.fileItem).removeClass(fm.option.fileItemClassSelected);
                        //$(this).addClass(fm.option.fileItemClassSelected);
                        fm.bindImageDetails($(this));
                    }
                })
                    .dblclick(function () {
                        try {
                            var name = $(this).attr('data-name');
                            var title = $(this).attr('data-title');
                            var path = $(this).attr('data-full-path');
                            var shortpath = $(this).attr('data-path');
                            var desc = $(this).attr('data-desc');
                            var width = $(this).attr('data-width');
                            var height = $(this).attr('data-height');
                            var item = { name: name, title: title, path: path, desc: desc, width: width, height: height, shortpath: shortpath };
                            fm.selectedList.pushUnique(item);
                            window.parent.callback(fm.selectedList);
                        }
                        catch (ex) {
                            console.log(ex);
                        }
                    })
                    .toggleSidePanel();

                $(fm.option.listFileArea).jplist({
                    command: 'empty'
                });

                $(fm.option.listFileArea).jplist({
                    itemsBox: fm.option.listView
                    , itemPath: fm.option.listViewItem
                    , panelPath: '.jplist-panel'
                });

                fm.initCurrentTime();

                fm.loading(false);
            },
            error: function () {
                alert("Error");
                fm.loading(false);
            }
        });
        return fm;
    };

    fm.fileAction = function (action) {
        var param = "?" + fm.option.fileActionOption.actionParameter + "=" + action.name;
        var file = action.file.replaceAll(fm.option.folderSeparator, "/");;

        param += "&" + fm.option.fileActionOption.fileParameter + "=" + file;

        if (action.name === "cut" || action.name === "copy") {
            var folder = action.folder.replaceAll(fm.option.folderSeparator, "/");

            param += "&" + fm.option.fileActionOption.folderParameter + "=" + folder;
        }

        $.ajax({
            type: "POST",
            async: false,
            url: fm.option.fileActionOption.urlFileAction + param,
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (!result.Result) {
                    alert(result.Message);

                    return fm;
                }

                fm.load();
            },
            error: function () {
                alert("Error");
            }
        });

        return fm;
    };

    fm.loading = function (isLoading) {
        var ls = $(fm.option.loading);
        var w = $(window);

        if (!isLoading) {
            ls.hide();

            return fm;
        }

        var top = (w.height() - ls.height()) / 2;
        var left = (w.width() - ls.width()) / 2;

        ls.css("top", top).css("left", left).show();

        return fm;
    };

    fm.findFileInfo = function (fileName) {
        if (!fm.data || !fm.data.FileInfos) {
            return null;
        }

        for (var i in fm.data.FileInfos) {
            var file = fm.data.FileInfos[i];

            if (file.Name.toLowerCase() === fileName.toLowerCase()) {
                return file;
            }
        }

        return null;
    };

    fm.addFileInfo = function (file) {
        if (!fm.data || !fm.data.FileInfos) {
            return;
        }

        fm.data.FileInfos.push(file);
    }

    fm.pushSelectedItemToList = function () {
        $(fm.option.fileItem).each(function () {
            var name = $(this).attr('data-name');
            var title = $(this).attr('data-title');
            var path = $(this).attr('data-full-path');
            var shortpath = $(this).attr('data-path');
            var desc = $(this).attr('data-desc');
            var width = $(this).attr('data-width');
            var height = $(this).attr('data-height');
            console.log($(this).attr('data-title'));
            var item = { name: name, title: title, path: path, desc: desc, width: width, height: height, shortpath: shortpath };
            if ($(this).hasClass(fm.option.fileItemClassSelected)) {
                fm.selectedList.pushUnique(item);
            }
            else {
                var mappedSelectedList = fm.selectedList.map(function (e) {
                    return e.name;
                });
                var index = mappedSelectedList.indexOf(item.name);
                if (index >= 0)
                    fm.selectedList.splice(index, 1);
            }
        });
    }

    fm.bindClickMainPanel = function () {
        fm.switchPanel(fm.option.panelTypeEnum.mainPanel);
        fm.load();
    }

    fm.bindClickOpenFilesPanel = function () {
        fm.switchPanel(fm.option.panelTypeEnum.openFilesPanel);
        $(fm.option.multiFileBrowser).click();
    }

    fm.bindListUploadedOpenFilesPanel = function (result) {
        var path = result.Url;
        var pieces = path.split('/');
        var name = pieces[pieces.length - 1];
        var fullPath = domainImages + "/" + path;
        var file = { FullOriginalPath: fullPath, Name: name, Path: "", FullPath: "", Size: "", Extension: "" };
        var html = fm.option.createOpenFileItem(file);
        $(fm.option.listFileBrowser).find('.file-browser-listing').append(html);
        var item = { name: name, title: "", path: fullPath, desc: "", width: 0, height: 0 };
        fm.selectedList.pushUnique(item);
        $(fm.option.fileBrowserButton).show();
        fm.loading(false);
    }

    fm.bindCrawlUrlPanel = function () {
        $(fm.option.crawlUrlPanel).modal();
    }

    fm.bindCrawlFromOtherPagePanel = function () {
        fm.switchPanel(fm.option.panelTypeEnum.crawlFromOtherPagePanel);
    }

    fm.uploadFiles = function (files, callback) {
        fm.loading(true);
        var fileNames = new Array();

        $.each(files, function (key, value) {

            var fileName = value.name;

            if (fm.option.uploadFileCondition.acceptFileTypes.exec(fileName) === null) {
                alert('Định dạng file không được chấp nhận!\n(' + fileName + ')');

                flag = false;
                fm.loading(false);
                return false;
            }

            if (value.size > Math.pow(1024, 2) * fm.option.uploadFileCondition.maxFileSize) {
                alert('Kích thước không cho phép! Chỉ được upload file < ' + fm.option.uploadFileCondition.maxFileSize + 'MB\n(' + fileName + ')');

                flag = false;
                fm.loading(false);
                return false;
            }

            var data = new FormData();

            data.append('project', uploadProject);
            data.append('UploadType', 'upload');
            if ($("#chkIsLarge").prop("checked") === true) {
                data.append('IsLarge', '1');
            }
            data.append('StringDecypt', $('#hfToken').val());
            data.append('submit', 'Upload Image');
            data.append('fileToUpload', value);

            var request = new XMLHttpRequest();

            request.onreadystatechange = function () {
                if (this.readyState === 4 && this.status === 200) {
                    var result = JSON.parse(this.responseText);

                    if (result.ErrorCode !== 200) {
                        alert("Không thể upload tệp tin: " + fileName);
                    }
                    else {
                        fm.updateFileInfoToDb(result.Url, result, function (d) { });
                        callback(result);
                    }
                }

            };
            request.open("POST", fm.option.uploadFileOption.urlUploadFile);
            request.send(data);

            /*
            $.ajax({
                url: fm.option.uploadFileOption.urlUploadFile,
                type: "POST",
                data: data,
                dataType: "json",
                processData: false, // Don't process the files
                contentType: false, // Set content type to false as jQuery will tell the server its a query string request
                //async: false,
                success: function (result) {
                    if (!result.OK) {
                        alert("Không thể upload tệp tin: " + fileName);
                    }
                    else {
                        fm.getImageInfoFromApi(result.OK, function (fileUrl, e) {
                            fm.updateFileInfoToDb(fileUrl, e, function (d) { });
                        });
                        callback(result);
                    }
                },
                error: function (ex) {
                    alert("Error");
                    console.log(ex);
                    fm.loading(false);
                }
            });
            */
        });
    }

    fm.uploadFromUrl = function (link, callback) {

        var data = new FormData();
        data.append('project', uploadProject);
        data.append('UploadType', 'downloadFileFromUrl');
        data.append('StringDecypt', $('#hfToken').val());
        data.append('submit', 'Check');
        data.append('downloadUrl', link);
        $.ajax({
            url: fm.option.uploadFileOption.urlUploadFile,
            type: "POST",
            data: data,
            dataType: "json",
            processData: false, // Don't process the files
            contentType: false, // Set content type to false as jQuery will tell the server its a query string request
            //async: false,
            success: function (result) {

                if (result.ErrorCode !== 200) {
                    alert("Không thể upload tệp tin: " + fileName);
                }
                else {
                    fm.updateFileInfoToDb(result.Url, result, function (d) { });
                    callback(result);
                }
            },
            error: function () {
                alert("Lỗi upload ảnh!");
                callback(null);
            }
        });
    }

    fm.getImageInfoFromApi = function (path, callback) {        var data = new FormData();
        data.append('project', uploadProject);
        data.append('UploadType', 'info');
        data.append('StringDecypt', $('#hfToken').val());
        data.append('submit', 'Check');
        data.append('FileTemp', path);
        $.ajax({
            url: fm.option.uploadFileOption.urlUploadFile,
            type: "POST",
            data: data,
            dataType: "json",
            processData: false, // Don't process the files
            contentType: false, // Set content type to false as jQuery will tell the server its a query string request
            //async: false,
            success: function (result) {
                callback(path, result);
            },
            error: function () {
                alert("Lỗi lấy thông tin ảnh!");
                callback(path, null);
            }
        });
    }

    fm.updateFileInfoToDb = function (fileUrl, obj, callback) {
        var apiUrl = fm.option.fileActionOption.urlFileAction + '?' + fm.option.fileActionOption.actionParameter + '=updateFileinfo';
        var pieces = fileUrl.split('/');
        var fileName = pieces[pieces.length - 1];
        var fileSize = obj.FileSize;
        var width = obj.Width;
        var height = obj.Height;
        var mimeTypeName = obj.MimeType;

        $.ajax({
            type: "POST",
            url: apiUrl,
            data: { filename: fileName, fileurl: fileUrl, filesize: fileSize, width: width, height: height, mimetypename: mimeTypeName },
            success: function (data) {
                callback(data);
                fm.loading(false);
            },
            error: function (msg) {
                console.log(msg);
                fm.loading(false);
            }
        });
    }

    fm.getFileInfoFromDb = function (fileName, callback) {
        var apiUrl = fm.option.fileActionOption.urlFileAction + '?' + fm.option.fileActionOption.actionParameter + '=getFileinfo';
        $.ajax({
            type: "POST",
            url: apiUrl,
            data: { filename: fileName },
            success: function (data) {
                if (data.Success) {
                    callback(data.Data);
                }
                else {
                    callback(null);
                }
            },
            error: function (msg) {
                console.log(msg);
                fm.loading(false);
            }
        });
    }

    fm.binItemDataUpdated = function (obj) {
        $(fm.option.fileItem + "[data-id='" + obj.Id + "']").attr('data-desc', obj.Description).attr('data-title', obj.Title);
        // update lại item đã được sửa trong selectedList
        fm.selectedList[0].title = obj.Title;
        fm.selectedList[0].desc = obj.Description;
    }

    fm.initCurrentTime = function () {
        if ($(fm.option.startDateSearchFile).val().length <= 0 && $(fm.option.endDateSearchFile).val().length <= 0) {
            $(fm.option.startDateSearchFile).val(currentStartDateStr);
            $(fm.option.endDateSearchFile).val(currentEndDateStr);
        }
    }

    var changeSidePanelWidth = function (delta) {
        var w = $(fm.option.sidepanels).width() + delta;
        $(fm.option.sidepanels).width(w);
        workarea.css('right', parseInt(workarea.css('right'), 0) + delta);
    };

    fm.toggleSidePanel = function (close) {
        if (close) $(fm.option.sidepanels).hide();
        else $(fm.option.sidepanels).show();
        var w = $(fm.option.sidepanels).width();
        var deltaX = (w > 0 && close ? 0 : fm.option.SIDEPANEL_OPENWIDTH) - w;
        changeSidePanelWidth(deltaX);
    };

    $.fn.toggleSidePanel = function () {
        $(this).mousedown(function (evt) { })
            .mouseup(function (evt) {
                var isOpen = $(this).hasClass(fm.option.fileItemClassSelected);
                fm.toggleSidePanel();
                //if (isOpen) {
                //    fm.toggleSidePanel(true);
                //} else {
                //    fm.toggleSidePanel();
                //}
            });
        $(window).mouseup(function () {
        });
    }

    fm.bindImageDetails = function (obj) {
        var root = $(fm.option.infopanel);
        var imgObj = $(fm.option.infoPanelImage);
        imgObj.attr('src', fm.option.loadingImage);
        var fullPath = obj.attr('data-full-path');
        var path = obj.attr('data-path');
        var title = obj.attr('data-title');
        var name = obj.attr('data-name');
        var time = obj.attr('data-time');
        var user = obj.attr('data-user');
        var size = obj.attr('data-size');
        var desc = obj.attr('data-desc');
        var width = obj.attr('data-width');
        var height = obj.attr('data-height');
        var mimetypename = obj.attr('data-mimetypename');
        $("<img/>").attr("src", fullPath).load(function () {
            imgObj.attr('src', fullPath).attr('alt', name).attr('data-width', width).attr('data-height', height);
        });
        $(fm.option.fileInfoName).val(name);
        $(fm.option.fileInfoTitle).val(title);
        $(fm.option.infoDetailDimension).html(width + "x" + height + " px");
        $(fm.option.infoDetailTime).html(time);
        $(fm.option.infoDetailUser).html(user);
        $(fm.option.infoDetailFileSize).html(Math.floor(size / 1024) + " KB");
        $(fm.option.infoDetailDescription).val(desc);

        CURRENT_FILE_OBJECT = { FileName: name, FileUrl: path, Width: width, Height: height, MimeTypeName: mimetypename, Description: desc, FileSize: size }
    }
    /** click item => view detail **/

    /** chọn nhiều ảnh để chèn **/
    fm.bindSelectedListImages = function () {
        var html = "";
        for (var i = 0; i < fm.selectedList.length; i++) {
            var obj = fm.selectedList[i];
            var item = fm.option.SelectedListItem(obj);
            html += item;
        }
        $(fm.option.sortableSelectedList).html(html);
        $(fm.option.sortableSelectedList).sortable(function (e) { console.log(e); });
    }

    fm.switchPanel = function (type) {
        if (type === fm.option.panelTypeEnum.mainPanel) {
            fm.selectedList = [];
            console.log($(fm.option.sidepanels).is(':hidden'));
            if ($(fm.option.sidepanels).is(':visible')) fm.toggleSidePanel(true);
            $(fm.option.listFileBrowser).hide();
            $(fm.option.listCrawlFromOtherPage).hide();
            $(fm.option.crawlPageButton).hide();
            $(fm.option.listFileArea).show();
        }
        else if (type === fm.option.panelTypeEnum.selectedListPanel) {
            $(fm.option.sidepanels).show();
            //fm.toggleSidePanel();
            $(fm.option.sidepanel_title).html('DANH SÁCH ẢNH ĐÃ CHỌN');
            $(fm.option.infopanel).hide();
            $(fm.option.listCrawlFromOtherPage).hide();
            $(fm.option.selectedListPanel).show();
            $(fm.option.editInfoSaveButton).hide();
        }
        else if (type === fm.option.panelTypeEnum.detailInfoPanel) {
            $(fm.option.sidepanels).show();
            //fm.toggleSidePanel();
            //$(fm.option.fileItem).removeClass(fm.option.fileItemClassSelected);
            $(fm.option.sidepanel_title).html('THÔNG TIN CHI TIẾT');
            $(fm.option.infopanel).show();
            $(fm.option.selectedListPanel).hide();
            $(fm.option.listCrawlFromOtherPage).hide();
            $(fm.option.editInfoSaveButton).show();
        }
        else if (type === fm.option.panelTypeEnum.openFilesPanel) {
            //$(fm.option.sidepanels).hide();
            fm.toggleSidePanel(true);
            $(fm.option.listFileArea).hide();
            $(fm.option.listCrawlFromOtherPage).hide();
            $(fm.option.crawlPageButton).hide();
            $(fm.option.listFileBrowser).show();
        }
        else if (type === fm.option.panelTypeEnum.crawlFromOtherPagePanel) {
            //$(fm.option.sidepanels).hide();
            fm.toggleSidePanel(true);
            $(fm.option.listFileArea).hide();
            $(fm.option.listFileBrowser).hide();
            $(fm.option.crawlImagesListting).html('');
            $(fm.option.listCrawlFromOtherPage).show();
        }

    }
    /** chọn nhiều ảnh để chèn **/

    /**
    jQuery multiSelect plugin.
    (c) Kane Cohen - Dec, 2011.

**/


    $.fn.multiSelect = function (o) {
        var defaults = {
            multiselect: true,
            selected: fm.option.fileItemClassSelected,
            filter: ' > *',
            unselectOn: false,
            keepSelection: true,
            list: $(this).selector,
            e: null,
            element: null,
            start: false,
            stop: false,
            unselecting: false
        }
        return this.each(function (k, v) {
            var options = $.extend({}, defaults, o || {});
            // selector - parent, assign listener to children only
            $(document).on('mousedown', options.list + options.filter, function (e) {
                if (e.which === 1) {
                    if (options.handle !== undefined && !$(e.target).is(options.handle)) {
                        // TODO:
                        // keep propagation?
                        // return true;
                    }
                    options.e = e;
                    options.element = $(this);
                    multiSelect(options);
                }
                return true;
            });

            if (options.unselectOn) {
                // event to unselect

                $(document).on('mousedown', options.unselectOn, function (e) {
                    if (!$(e.target).parents().is(options.list) && e.which !== 3) {
                        $(options.list + ' .' + options.selected).removeClass(options.selected);
                        if (options.unselecting !== false) {
                            options.unselecting();
                        }
                    }
                });

            }

        });


    }

    function multiSelect(o) {

        var target = o.e.target;
        var element = o.element;
        var list = o.list;

        if ($(element).hasClass('ui-sortable-helper')) {
            return false;
        }

        if (o.start !== false) {
            var start = o.start(o.e, $(element));
            if (start === false) {
                return false;
            }
        }

        if (o.e.shiftKey && o.multiselect) {
            // get one already selected row
            $(element).addClass(o.selected);
            first = $(o.list).find('.' + o.selected).first().index();
            last = $(o.list).find('.' + o.selected).last().index();

            // if we hold shift and try to select last element that is upper in the list
            if (last < first) {
                firstHolder = first;
                first = last;
                last = firstHolder;
            }

            if (first === -1 || last === -1) {
                return false;
            }

            $(o.list).find('.' + o.selected).removeClass(o.selected);

            var num = last - first;
            var x = first;

            for (i = 0; i <= num; i++) {
                $(list).find(o.filter).eq(x).addClass(o.selected);
                x++;
            }
        } else if ((o.e.ctrlKey || o.e.metaKey) && o.multiselect) {
            // reset selection
            if ($(element).hasClass(o.selected)) {
                $(element).removeClass(o.selected);
            } else {
                $(element).addClass(o.selected);
            }
        } else {
            // reset selection
            if (o.keepSelection && !$(element).hasClass(o.selected)) {
                $(list).find('.' + o.selected).removeClass(o.selected);
                $(element).addClass(o.selected);
            } else {
                $(list).find('.' + o.selected).removeClass(o.selected);
                $(element).addClass(o.selected);
            }

        }

        if (o.stop !== false) {
            o.stop($(list).find('.' + o.selected), $(element));
        }

    }

    String.prototype.trim = function (chars) {
        if (chars === undefined)
            chars = "\s";

        return this.replace(new RegExp("^[" + chars + "]+"), "").replace(new RegExp("[" + chars + "]+$"), "");
    };

    String.prototype.replaceAll = function (replaceFrom, replaceTo) {
        var text = this;

        if (!replaceFrom || !replaceTo || replaceFrom === replaceTo) {
            return text;
        }

        while (text.indexOf(replaceFrom) > -1) {
            text = text.replace(replaceFrom, replaceTo);
        }

        return text;
    };

    Array.prototype.pushUnique = function (item) {

        var mappedIds = this.map(function (e) {
            return e.name;
        });

        if (mappedIds.indexOf(item.name) === -1) {
            this.push(item);
            return true;
        }
        return false;
    }

    window.fm = fm;
})(window, $);

function sendFileToServer(formData, status, domainUpload) {
    var uploadURL = domainUpload + '/UploadHandler.php'; //Upload URL
    var extraData = {}; //Extra Data.
    var jqXHR = $.ajax({
        xhr: function () {
            var xhrobj = $.ajaxSettings.xhr();
            if (xhrobj.upload) {
                xhrobj.upload.addEventListener('progress', function (event) {
                    var percent = 0;
                    var position = event.loaded || event.position;
                    var total = event.total;
                    if (event.lengthComputable) {
                        percent = Math.ceil(position / total * 100);
                    }
                    //Set progress
                    status.setProgress(percent);
                }, false);
            }
            return xhrobj;
        },
        url: uploadURL,
        type: "POST",
        contentType: false,
        processData: false,
        cache: false,
        data: formData,
        success: function (data) {
            status.setProgress(100);
            status.hideProgressbar();
            var convertJsonData = JSON.parse(data);
            SetImageToEditor(domainStorageImg + convertJsonData.OK);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            // Handle errors here
            //console.log('ERRORS: ' + textStatus);
        },
        complete: function (data) {

        }
    });
}

function sleep(delay) {
    var start = new Date().getTime();
    while (new Date().getTime() < start + delay);
}