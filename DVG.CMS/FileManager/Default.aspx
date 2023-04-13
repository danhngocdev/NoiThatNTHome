<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FileManager.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="Content/treeview/themes/default/style.min.css" rel="stylesheet" />
    <link href="Content/jqueryui/jquery-ui.css" rel="stylesheet" />
    <!--fonttello icon-->
    <link href="Content/fontello/css/fontello.css" rel="stylesheet" />
    <!--drop zone-->
    <link href="Content/dropzone/dropzone.css" rel="stylesheet" />
    <!--jplist fillter-->
    <%--<link href="Content/jplist/css/jplist.core.min.css" rel="stylesheet" />--%>
    <link href="Content/jplist/css/jplist.filter-toggle-bundle.min.css" rel="stylesheet" />
    <link href="Content/jplist/css/jplist.jquery-ui-bundle.min.css" rel="stylesheet" />
    <%-- Crop --%>
    <link rel="stylesheet" href="Content/cropperjs/dist/jquery.modal.min.css" />
    <link rel="stylesheet" href="Content/cropperjs/dist/font-awesome.min.css">
    <link rel="stylesheet" href="Content/cropperjs/dist/bootstrap.min.css">
    <link rel="stylesheet" href="Content/cropperjs/dist/cropper.css" />
    <link rel="stylesheet" href="Content/cropperjs/dist/main.css" />
    <%-- Crop --%>
    <!--Theme-->
    <link href="Content/css/theme.css" rel="stylesheet" />
</head>
<body>
    <form id="frmMain" runat="server">
        <div id="explore" runat="server">
            <div class="title-bar">
                CÔNG CỤ QUẢN LÝ FILE ẢNH
            </div>
            <div id="toolbarLeft">
                <div class="dvg-button active" data-type="1" data-toggle="tooltip" title="Danh sách ảnh"><i class="dvg-button-icon icon icon-picture"></i></div>
                <div class="dvg-button" data-type="2" data-toggle="tooltip" title="Tải lên từ máy"><i class="dvg-button-icon icon icon-folder-open"></i></div>
                <div class="dvg-button" data-type="3" data-toggle="tooltip" title="Tải ảnh từ url"><i class="dvg-button-icon icon icon-upload-cloud"></i></div>
                <div class="dvg-button" data-type="4" data-toggle="tooltip" title="Lấy ảnh từ trang khác"><i class="dvg-button-icon icon icon-flow-tree"></i></div>
            </div>
            <div id="listFileArea" class="file-container">
                <div class="dvg-control-hr dvg-toolbar-top jplist-panel">
                    <!-- filter by title -->
                    <div class="dvg-group-textbox text-filter-box">

                        <!--[if lt IE 10]>
                            <div class="jplist-label">Filter by Title:</div>
                        <![endif]-->

                        <input
                            id="dvg-keyword-search-file"
                            data-path=".title"
                            type="text"
                            value=""
                            placeholder="Tìm kiếm theo tên"
                            data-control-type="textbox"
                            data-control-name="title-filter"
                            data-control-action="filter" />
                        <i class="dvg-icon dvg-icon-search icon icon-search"></i>
                    </div>
                    <!-- date picker range filter -->
                    <div class="dvg-group-date-range"
                        data-control-type="date-picker-range-filter"
                        data-control-name="date-picker-range-filter"
                        <%--data-control-action="filter"--%>
                        data-path=".date"
                        data-datetime-format="{day}/{month}/{year}"
                        data-datepicker-func="datepicker">

                        <div class="dvg-group-textbox">
                            <input
                                id="startDateSearchFile"
                                type="text"
                                class="date-picker"
                                placeholder="Từ ngày"
                                data-type="prev" />
                            <i class="dvg-icon icon icon-calendar" aria-hidden="true"></i>
                        </div>
                        <div class="dvg-group-textbox">
                            <input
                                id="endDateSearchFile"
                                type="text"
                                class="date-picker"
                                placeholder="Đến ngày"
                                data-type="next" />
                            <i class="dvg-icon icon icon-calendar" aria-hidden="true"></i>
                        </div>
                    </div>

                    <!-- button search -->
                    <div class="dvg-group-searth-button" id="searchFileButton">
                        <span>Tìm kiếm</span>
                        <i class="dvg-icon dvg-icon-search icon icon-search"></i>
                    </div>

                    <!-- views -->
                    <div
                        class="dvg-control-group"
                        data-control-type="views"
                        data-control-name="views"
                        data-control-action="views"
                        data-default="dvg-grid-view">

                        <div class="dvg-btn dvg-view-button dvg-btn-list-view" data-type="dvg-list-view">
                            <i class="icon icon-list-bullet"></i>
                        </div>
                        <div class="dvg-btn dvg-view-button dvg-btn-grid-view active" data-type="dvg-grid-view">
                            <i class="icon icon-th-large"></i>
                        </div>
                    </div>
                </div>
                <div id="dropzoneForm" class="file-explore dropzone">
                    <div class="file-item">
                        <div class="dvg-img">
                            <img src="/FileManager/content/images/no-image.png" />
                        </div>
                        <div class="dvg-img-info">
                            <p>Không tồn tại tệp tin nào trong thư mục này</p>
                        </div>
                    </div>
                </div>
            </div>
            <div id="listFileBrowser" style="display: none;">
                <div class="file-browser-listing"></div>
                <div class="file-browser-button">

                    <div class="file-manager-button file-manager-commit file-manager-button-submit float-right">
                        <span>Chèn ảnh vừa upload</span>
                        <i class="dvg-button-icon icon icon-down-circled2"></i>
                    </div>
                </div>
            </div>
            <div id="listCrawlFromOtherPage" style="display: none;">
                <div class="dvg-control-hr dvg-toolbar-top jplist-panel">
                    <div class="dvg-group-textbox text-filter-box">

                        <!--[if lt IE 10]>
                            <div class="jplist-label">Filter by Title:</div>
                        <![endif]-->

                        <input
                            class="w-"
                            id="crawlUrlFromPageText"
                            data-path=".title"
                            type="text"
                            value=""
                            placeholder="Nhập Url"
                            data-control-type="textbox"
                            data-control-name="title-filter"
                            data-control-action="filter" />
                    </div>
                    <div class="dvg-group-searth-button" id="crawlUrlFromPageButton">
                        <span>Crawl ảnh</span>
                        <i class="dvg-icon dvg-icon-search icon icon-search"></i>
                    </div>
                    <div
                        class="dvg-group-checkbox"
                        data-control-type="checkbox-text-filter"
                        data-control-action="filter"
                        data-control-name="keywords"
                        data-path=".keywords"
                        data-logic="or">
                        <input
                            value="filterImages"
                            id="crawlUrlFromPagefilter"
                            type="checkbox" />
                        <label for="architecture">Lọc ảnh (kích thước lớn hơn 400x400)</label>
                    </div>
                </div>
                <div id="crawlImagesListting"></div>
                <div class="crawl-page-button">
                    <div class="file-manager-button file-manager-button-submit float-right" id="uploadCrawlPageButton">
                        <span>Tải ảnh</span>
                        <i class="dvg-button-icon icon icon-down-circled2"></i>
                    </div>
                </div>
            </div>
            <div id="sidepanels">
                <div id="sidepanel_title">
                    Thông tin chi tiết
                </div>
                <div id="infopanel" class="infor-detail">
                    <div class="infor-detail-image-infor">
                        <img id="infoPanelImage" src="/FileManager/content/images/no-image.png" alt="" />
                        <div class="infor-detail-crop-button fa fa-crop" data-toggle="tooltip" title="Xử lý ảnh" id="openCropPanelButton"></div>
                        <div class="infor-detail-crop-button fa fa-pencil-square" data-toggle="tooltip" title="Chèn logo" id="addLogoToImageButton"></div>
                        <div class="infor-detail-stats">
                            <span class="infor-detail-dimension">
                                <i class="dvg-button-icon icon icon-clone"></i>
                                <p id="infoDetailDimension"></p>
                            </span>
                            <span class="infor-detail-dimension">
                                <i class="dvg-button-icon icon icon-info"></i>
                                <p id="infoDetailFileSize"></p>
                            </span>
                            <span class="infor-detail-dimension">
                                <i class="dvg-button-icon icon icon-clock"></i>
                                <p id="infoDetailTime"></p>
                            </span>
                            <span class="infor-detail-dimension">
                                <i class="dvg-button-icon icon icon-user"></i>
                                <p id="infoDetailUser"></p>
                            </span>
                        </div>
                    </div>
                    <div id="fileInfo" class="infor-detail-textgroup">
                        <div class="infor-detail-unit d-none">
                            <span class="infor-detail-title">Tên ảnh</span>
                            <input class="fileInfo-name" id="fileInfoName" type="text" value="" readonly />
                        </div>
                        <div class="infor-detail-unit">
                            <span class="infor-detail-title">Tiêu đề</span>
                            <input class="fileInfo-name" id="fileInfoTitle" type="text" value="" />
                        </div>
                        <div class="infor-detail-unit">
                            <span class="infor-detail-title">Chú thích ảnh</span>
                            <textarea id="infoDetailDescription" class="fileInfo-desc" rows="3"></textarea>
                        </div>
                        <div class="infor-detail-unit">
                            <span class="infor-detail-title">Tác giả</span>
                            <input class="fileInfo-author" type="text" value="" readonly />
                        </div>
                        <div class="infor-detail-unit">
                            <span class="infor-detail-title">Từ khóa (phục vụ cho tìm kiếm ảnh)</span>
                            <textarea class="fileInfo-tags" rows="3" readonly></textarea>
                        </div>
                    </div>
                </div>
                <div id="selectedListPanel" class="dvs-selected-list-panel">
                    <ul id="sortableSelectedList">
                    </ul>
                </div>
                <div class="infor-detail-submit-group">
                    <div class="file-manager-button-group">
                        <div class="file-manager-button file-manager-button-submit file-manager-commit" data-type="sort">
                            <span>Chèn ảnh</span>
                            <i class="dvg-button-icon icon icon-down-circled2"></i>
                        </div>
                        <div class="file-manager-button file-manager-button-save" id="editInfoSaveButton">
                            <span>Lưu</span>
                            <i class="dvg-button-icon icon icon-floppy"></i>
                        </div>
                    </div>
                </div>
            </div>
            <input type="file" name="file" multiple="multiple" hidden="hidden" id="multiFileBrowser" />
            <asp:HiddenField runat="server" ID="hfToken" ClientIDMode="Static" />
            <div class="clear"></div>
        </div>
        <div id="previewimage" runat="server" visible="false">
            <table width="100%">
                <tr>
                    <td align="center">
                        <br />
                        <h2>Ảnh gốc</h2>
                        <br />
                        <img src="/FileManager/content/images/no-image.png" id="imgpreview" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <!-- Modal -->
        <div id="cropPanel" role="dialog" style="display: none;">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Cắt ảnh</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="container" id="actions">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="img-container">
                                        <img id="cropPannelImage" src="#" alt="Picture" />
                                        <div id="result"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6 docs-toggles">
                                    <!-- <h3>Toggles:</h3> -->
                                    <div class="btn-group d-flex flex-nowrap" data-toggle="buttons">
                                        <label class="btn btn-primary active">
                                            <input type="radio" class="sr-only" id="aspectRatio1" name="aspectRatio" value="1.7777777777777777" />
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Tỉ lệ: 16 / 9">16:9
                                            </span>
                                        </label>
                                        <label class="btn btn-primary">
                                            <input type="radio" class="sr-only" id="aspectRatio2" name="aspectRatio" value="1.3333333333333333" />
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Tỉ lệ: 4 / 3">4:3
                                            </span>
                                        </label>
                                        <label class="btn btn-primary">
                                            <input type="radio" class="sr-only" id="aspectRatio3" name="aspectRatio" value="1">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Tỉ lệ: 1 / 1">1:1
                                            </span>
                                        </label>
                                        <label class="btn btn-primary">
                                            <input type="radio" class="sr-only" id="aspectRatio4" name="aspectRatio" value="0.6666666666666666" />
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Tỉ lệ: 2 / 3">2:3
                                            </span>
                                        </label>
                                        <label class="btn btn-primary">
                                            <input type="radio" class="sr-only" id="aspectRatio5" name="aspectRatio" value="NaN" />
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Tỉ lệ: Tự do">Free
                                            </span>
                                        </label>
                                    </div>
                                </div>
                                <!-- /.docs-toggles -->
                            </div>
                            <div class="row  docs-buttons">
                                <div class="col-md-9">
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-primary" data-method="zoom" data-option="0.1" title="Zoom In">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Zoom +">
                                                <span class="fa fa-search-plus"></span>
                                            </span>
                                        </button>
                                        <button type="button" class="btn btn-primary" data-method="zoom" data-option="-0.1" title="Zoom Out">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Zoom -">
                                                <span class="fa fa-search-minus"></span>
                                            </span>
                                        </button>
                                    </div>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-primary" data-method="move" data-option="-10" data-second-option="0" title="Move Left">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Dịch trái">
                                                <span class="fa fa-arrow-left"></span>
                                            </span>
                                        </button>
                                        <button type="button" class="btn btn-primary" data-method="move" data-option="10" data-second-option="0" title="Move Right">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Dịch phải">
                                                <span class="fa fa-arrow-right"></span>
                                            </span>
                                        </button>
                                        <button type="button" class="btn btn-primary" data-method="move" data-option="0" data-second-option="-10" title="Move Up">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Dịch lên)">
                                                <span class="fa fa-arrow-up"></span>
                                            </span>
                                        </button>
                                        <button type="button" class="btn btn-primary" data-method="move" data-option="0" data-second-option="10" title="Move Down">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Dịch xuống">
                                                <span class="fa fa-arrow-down"></span>
                                            </span>
                                        </button>
                                    </div>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-primary" data-method="rotate" data-option="-45" title="Rotate Left">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Xoay trái">
                                                <span class="fa fa-rotate-left"></span>
                                            </span>
                                        </button>
                                        <button type="button" class="btn btn-primary" data-method="rotate" data-option="45" title="Rotate Right">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Xoay phải">
                                                <span class="fa fa-rotate-right"></span>
                                            </span>
                                        </button>
                                    </div>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-primary" data-method="scaleX" data-option="-1" title="Flip Horizontal">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Đảo chiều trái phải">
                                                <span class="fa fa-arrows-h"></span>
                                            </span>
                                        </button>
                                        <button type="button" class="btn btn-primary" data-method="scaleY" data-option="-1" title="Flip Vertical">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Đảo chiều trên dưới">
                                                <span class="fa fa-arrows-v"></span>
                                            </span>
                                        </button>
                                    </div>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-primary" data-method="reset" title="Reset">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Reset">
                                                <span class="fa fa-refresh"></span>
                                            </span>
                                        </button>
                                    </div>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-primary" data-method="getData" title="Tỉ lệ gốc">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Tỉ lệ gốc">
                                                <span class="fa fa-camera"></span>
                                            </span>
                                        </button>
                                        <button type="button" class="btn btn-secondary d-none" data-method="setData" data-target="#putData" id="cropPannelSetDataButon">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="cropper.setData(data)"></span>
                                        </button>
                                    </div>
                                </div>
                                <div class="col-md-3 text-right">
                                    <div class="btn-group btn-group-crop">
                                        <button type="button" class="btn btn-success" data-method="getCroppedCanvas" data-option="{ &quot;maxWidth&quot;: 4096, &quot;maxHeight&quot;: 4096 }">
                                            <span class="docs-tooltip fa fa-check" data-toggle="tooltip" title="Cắt ảnh"></span>
                                        </button>
                                    </div>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-light" data-dismiss="modal">
                                            <span class="docs-tooltip fa fa-remove" data-toggle="tooltip" title="Hủy"></span>
                                        </button>
                                    </div>
                                </div>
                                <!-- /.docs-buttons -->
                            </div>
                             <div class="docs-data d-none">
                                <input type="text" class="form-control" id="dataX" placeholder="x" />
                                <input type="text" class="form-control" id="dataY" placeholder="y" />
                                <input type="text" class="form-control" id="dataWidth" placeholder="width" />
                                <input type="text" class="form-control" id="dataHeight" placeholder="height" />
                                <input type="text" class="form-control" id="dataRotate" placeholder="rotate" />
                                <input type="text" class="form-control" id="dataScaleX" placeholder="scaleX" />
                                <input type="text" class="form-control" id="dataScaleY" placeholder="scaleY" />
                                <textarea class="form-control" id="putData" rows="1" placeholder=""></textarea>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
        <div id="crawlUrlPanel" role="dialog" style="display: none;">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Nhập đường dẫn ảnh</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="container">
                            <textarea class="container-fluid" rows="4"></textarea>
                            <i class="d-block text-center">Có thể nhập nhiều đường dẫn ảnh, mỗi đường dẫn trên 1 dòng.</i>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="crawl-url-button-group">
                            <div class="file-manager-button" id="uploadCrawlImagesButton">
                                <span>Upload</span>
                                <i class="dvg-button-icon icon icon-down-circled2"></i>
                            </div>
                            <%--                            <div class="file-manager-button">
                                <span>Hủy</span>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="addLogoPanel" role="dialog" style="display: none;">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Chèn logo</h4>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="container">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="img-container">
                                        <img id="addLogoPannelImage" src="/FileManager/content/images/no-image.png" alt="Picture" />
                                    </div>
                                </div>
                            </div>
                            <div class="row  docs-buttons">
                                <div class="col-md-9">
                                    <div class="btn-group">
                                        <button type="button" data-method="topleft" class="btn btn-primary btn-add-logo-group">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Góc trên-trái">
                                                <span class="fa fa-arrow-left fa-rotate-45"></span>
                                            </span>
                                        </button>
                                        <button type="button" data-method="topright" class="btn btn-primary btn-add-logo-group">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Góc trên-phải">
                                                <span class="fa fa-arrow-up fa-rotate-45"></span>
                                            </span>
                                        </button>
                                        <button type="button" data-method="botleft" class="btn btn-primary btn-add-logo-group">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Góc dưới-trái">
                                                <span class="fa fa-arrow-down fa-rotate-45"></span>
                                            </span>
                                        </button>
                                        <button type="button" data-method="botright" class="btn btn-primary btn-add-logo-group">
                                            <span class="docs-tooltip" data-toggle="tooltip" title="Góc dưới-phải">
                                                <span class="fa fa-arrow-right fa-rotate-45"></span>
                                            </span>
                                        </button>
                                    </div>
                                </div>
                                <div class="col-md-3 text-right">
                                    <div class="btn-group btn-group-crop">
                                        <button type="button" data-method="submit" class="btn btn-success btn-add-logo-group">
                                            <span class="docs-tooltip fa fa-check" data-toggle="tooltip" title="Upload"></span>
                                        </button>
                                    </div>
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-light" data-dismiss="modal">
                                            <span class="docs-tooltip fa fa-remove" data-toggle="tooltip" title="Hủy"></span>
                                        </button>
                                    </div>
                                </div>
                                <!-- /.docs-buttons -->
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="loading">
            <img src="Content/Images/loading.gif" alt="loading" />
        </div>
        <!-- Show the cropped image in modal -->
        <div class="modal fade docs-cropped" id="getCroppedCanvasModal" role="dialog" aria-hidden="true" aria-labelledby="getCroppedCanvasTitle" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="getCroppedCanvasTitle">Cropped</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body"></div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <a class="btn btn-primary" id="download" href="javascript:void(0);" download="cropped.jpg">Download</a>
                    </div>
                </div>
            </div>
        </div>
        <!-- /.modal -->
    </form>

    <script src="Content/treeview/libs/jquery.js"></script>
    <script src="Content/jqueryui/jquery-ui.js"></script>
    <script src="Content/treeview/jstree.min.js"></script>
    <script src="Content/dropzone/dropzone.js"></script>
    <%--<script src="Content/cropperjs/dist/cropper.js"></script>--%>
    <script src="Content/jplist/js/jplist.core.min.js"></script>
    <script src="Content/jplist/js/jplist.filter-toggle-bundle.min.js"></script>
    <script src="Content/jplist/js/jplist.sort-bundle.min.js"></script>
    <script src="Content/jplist/js/jplist.textbox-filter.min.js"></script>
    <script src="Content/jplist/js/jplist.jquery-ui-bundle.min.js"></script>
    <%-- Crop --%>
    <script src="Content/cropperjs/dist/jquery.modal.min.js"></script>
    <script src="Content/cropperjs/dist/bootstrap.bundle.min.js"></script>
    <script src="Content/cropperjs/dist/common.js"></script>
    <script src="Content/cropperjs/dist/cropper.js"></script>
    <script src="Content/cropperjs/dist/main.js"></script>
    <%-- Crop --%>
    <%-- Watermark --%>
    <script src="Content/watermarkjs/watermark.js"></script>
    <%-- Watermark --%>
    <%--<script src="Content/treeview/jstree.js"></script>--%>
    <script src="Content/nicescroll/jquery.nicescroll.min.js"></script>
    <script type="text/javascript">
        var domainImages = "<%= DVG.WIS.Utilities.AppSettings.Instance.GetString("View-Domain").TrimEnd('/') %>";
        var logoAddImageUrl = "<%= DVG.WIS.Utilities.AppSettings.Instance.GetString("LogoAddImage") %>";
        var uploadHandler = "<%=string.Concat(DVG.WIS.Utilities.AppSettings.Instance.GetString("Upload-Domain"), DVG.WIS.Utilities.AppSettings.Instance.GetString("Upload-Handler")) %>";
        var uploadProject = "<%=DVG.WIS.Utilities.AppSettings.Instance.GetString("Upload-Project")%>";
        const _dt_today_stamp = <%=DVG.WIS.Utilities.Utils.DateTimeToUnixTime(DateTime.Now)%>;
        const currentEndDateStr = "<%=DateTime.Now.ToString("dd/MM/yyyy")%>";
        const currentStartDateStr = "<%=DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy")%>";
    </script>
    <script src="Content/js/file-manager.js?v=<%=DVG.WIS.Utilities.AppSettings.Instance.GetString("CKEditorVersion")%>"></script>
    <script src="Content/js/script.js?v=<%=DVG.WIS.Utilities.AppSettings.Instance.GetString("CKEditorVersion")%>"></script>
</body>
</html>
