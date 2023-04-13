//if (uploadHandler == undefined || uploadHandler == null || uploadHandler == "") {
//    uploadHandler = "http://upload.mothe.vn/UploadHandler.php";
//}
$(document).ready(function () {
    var count = 0;
    fm.init({
        fileManagerCommitButton: ".file-manager-commit",
        toolbarLeftButton: "#toolbarLeft .dvg-button",
        switchListViewButton: ".dvg-view-button",
        dropzoneSelector: "#dropzoneForm",
        listFileArea: "#listFileArea",
        listView: ".file-explore",
        listViewItem: ".file-item",
        treeSelector: "#tree-folder",
        folderExplore: "#explore .folder-explore",
        fileExplore: "#explore .file-explore",
        folderItem: ".jstree-node",
        fileItem: "#explore .file-explore .file-item",
        fileItemClassSelected: "file-item-selected",
        startDateSearchFile: "#startDateSearchFile",
        endDateSearchFile: "#endDateSearchFile",
        searchFileButton: "#searchFileButton",
        uploadChoose: "input[type='file']",
        uploadButton: "#btnSubmit",
        statusSelector: ".status",
        loading: "#loading",
        sidepanels: "#sidepanels",
        sidepanel_title: "#sidepanel_title",
        infopanel: "#infopanel",
        selectedListPanel: "#selectedListPanel",
        sortableSelectedList: "#sortableSelectedList",
        infoPanelImage: "#infoPanelImage",
        infoDetailDimension: "#infoDetailDimension",
        infoDetailFileSize: "#infoDetailFileSize",
        infoDetailTime: "#infoDetailTime",
        infoDetailUser: "#infoDetailUser",
        infoDetailDescription: "#infoDetailDescription",
        openCropPanelButton: "#openCropPanelButton",
        cropPanel: "#cropPanel",
        cropPannelImage: "#cropPannelImage",
        fileInfoName: "#fileInfoName",
        fileInfoTitle: "#fileInfoTitle",
        editInfoSaveButton: "#editInfoSaveButton",
        multiFileBrowser: "#multiFileBrowser",
        listFileBrowser: "#listFileBrowser",
        crawlUrlPanel: "#crawlUrlPanel",
        listCrawlFromOtherPage: "#listCrawlFromOtherPage",
        uploadCrawlImagesButton: "#uploadCrawlImagesButton",
        crawlUrlFromPageText: "#crawlUrlFromPageText",
        crawlUrlFromPageButton: "#crawlUrlFromPageButton",
        crawlUrlFromPagefilter: "#crawlUrlFromPagefilter",
        crawlImagesListting: "#crawlImagesListting",
        uploadCrawlPageButton: "#uploadCrawlPageButton",
        crawlPagefileItem: "#crawlImagesListting .file-item",
        fileBrowserButton: ".file-browser-button",
        crawlPageButton: ".crawl-page-button",
        addLogoToImageButton: "#addLogoToImageButton",
        addLogoPannelImage: "#addLogoPannelImage",
        addLogoPanel: "#addLogoPanel",
        btnAddLogoGroup: ".btn-add-logo-group",
        loadFileAndFolderOption: {
            urlLoadFileAndFolder: "/FileManager/Handler/LoadFile.ashx",
            pathParameter: "path"
        },
        uploadFileOption: {
            urlUploadFile: uploadHandler,
            folderParameter: "fo"
        },
        fileActionOption: {
            urlFileAction: "/FileManager/Handler/FileAction.ashx",
            actionParameter: "action",
            fileParameter: "filename",
            folderParameter: "fo"
        },
        uploadFileCondition: {
            acceptFileTypes: /(\.|\/)(gif|jpe?g|png|zip|rar|doc|docx|xls|srt)$/i,
            maxFileSize: 6, //MB
            limitUpload: 100
        },
        loadingImage: "/FileManager/Content/Images/loading.gif",
        folderDefault: "",
        folderSeparator: "__-_-",
        createOpenFileItem: function (file) {
            var html = "";
            var src = "/FileManager/content/images/no-image.png";

            if (/(\.|\/)(gif|jpe?g|png)$/i.exec(file.FullOriginalPath) != null) {
                src = file.FullOriginalPath;
            } else if (/(\.|\/)(srt)$/i.exec(file.FullOriginalPath) != null) {
                src = "/FileManager/content/images/subtitle.png";
            }
            count++;
            html += "<div class = 'file-item' title = '" + file.Name + "' data-name = '" + file.Name + "' data-path = '" + file.Path + "' data-full-path = '" + file.FullPath + "' data-full-original-path = '" + file.FullOriginalPath + "' data-size = '" + file.Size + "' data-extension = '" + file.Extension + "'>";
            html += "<div class='dvg-img'><img src = '" + src + "' /></div>";
            html += "<div class='dvg-img-info'><p class='date'></p><p class='title'>" + file.Name + "</p></div>";
            html += "</div>";

            return html;
        },
        createFileItem: function (file) {
            var html = "";
            var src = "/FileManager/content/images/no-image.png";
            var fullOriginalPath = domainImages + "/" + file.FileUrl;
            if (/(\.|\/)(gif|jpe?g|png)$/i.exec(file.Crop105x105) != null) {
                src = file.Crop105x105;
            } else if (/(\.|\/)(srt)$/i.exec(file.Crop105x105) != null) {
                src = "/FileManager/content/images/subtitle.png";
            }
            count++;
            html += "<div class = 'file-item' title = '" + file.FileName
                + "' data-id = '" + file.Id
                + "' data-name = '" + file.FileName
                + "' data-title = '" + file.Title
                + "' data-path = '" + file.FileUrl
                + "' data-full-path = '" + fullOriginalPath
                + "' data-full-original-path = '" + fullOriginalPath
                + "' data-width = '" + file.Width
                + "' data-height = '" + file.Height
                + "' data-mimetypename = '" + file.MimeTypeName
                + "' data-time = '" + file.CreatedDateStr
                + "' data-user = '" + file.CreatedBy
                + "' data-desc = '" + file.Description
                + "' data-size = '" + file.FileSize
                + "'>";
            html += "<div class='dvg-img'><img src = '" + src + "' /></div>";
            html += "<div class='dvg-img-info'><p class='date'></p><p class='title'>" + file.FileName + "</p></div>";
            html += "</div>";

            return html;
        },
        SelectedListItem: function (obj) {
            var html = "";
            var src = "/FileManager/content/images/no-image.png";

            if (/(\.|\/)(gif|jpe?g|png)$/i.exec(obj.path) != null) {
                src = obj.path;
            } else if (/(\.|\/)(srt)$/i.exec(obj.path) != null) {
                src = "/FileManager/content/images/subtitle.png";
            }
            html += "<li data-name='" + obj.name + "'>"
            html += "<div class='dvs-selected-list-img'><img src = '" + src + "' /></div>";
            html += "<div class='dvs-selected-list-title'><p class='title'>" + obj.name + "</p></div>";
            html += "</li>"
            return html;
        },
        CrawlListItem: function (link) {
            var html = "";
            html += "<div class = 'file-item' data-full-path = '" + link + "' >";
            html += "<div class='dvg-img'><img src = '" + link + "' /></div>";
            html += "</div>";

            return html;
        },
        panelTypeEnum: { mainPanel: 0, selectedListPanel: 1, detailInfoPanel: 2, openFilesPanel: 3, crawlUrlPanel: 4, crawlFromOtherPagePanel: 5 },
        SIDEPANEL_OPENWIDTH: 250,
        LISTING_PAGEINDEX: 1,
        LISTING_PAGESIZE: 1000,
        ADD_LOGO_PADDING_POSITION: 0.5
    });
});