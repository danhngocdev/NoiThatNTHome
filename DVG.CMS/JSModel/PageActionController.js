// Create the factory that share the Data
app.factory('Data', function () {
    return { CarInfo: {} };
});

(function () {
    app.controller('PageActionController', ['$scope', '$rootScope', '$http', 'Service', 'notify', '$ngConfirm', 'Data', function ($scope, $rootScope, $http, Service, notify, $ngConfirm, Data) {
        //function ($scope, $rootScope, $http, Service, notify, Data) {
        $scope.VideoDuration = {
            'Hour': 0,
            'Minute': 0,
            'Second': 0
        };
        $scope.Page = {};

        $scope.CurrentFormEnum = {
            'Edit': 0,
            'PageRelation': 1
        }
        $scope.CurrentForm = $scope.CurrentFormEnum.Edit;
        $scope.CurrentPageRelationTabEnum = {
            'Suggest': 0,
            'Search': 1
        }
        $scope.CurrentPageRelationTab = $scope.CurrentPageRelationTabEnum.Suggest;

        $scope.SearchPageRelationModel = {
            'PageRelateId': '',
            'Keyword': '',
            'PageType': 1,
            'CateId': '',
            'ListCategory': []
        };

        $scope.pageSearchPageRelation = {
            'totalItems': 0,
            'currentPage': 1,
            'itemsPerPage': 15
        };

        // Template for message
        $scope.template = '';
        // Position of message
        $scope.positions = ['center', 'left', 'right'];
        // Class of message
        $scope.classes = {
            "Success": 'alert-success',
            "Error": 'alert-danger',
            "Warning": 'alert-warning'
        };
        $scope.position = $scope.positions[0];
        // The time display of message
        $scope.duration = 3000;

        $scope.IsHasLinkDetail = false;

        $scope.IsSubmited = false;

        $scope.PageTypeEnum = {
            "Page": 1, //Tin tức
            "Advices": 2, //Tư vấn
            "CarInfo": 3, //Thông tin xe
            "Assessment": 4, //Đánh giá xe
            "Gallery": 5, //Gallery
            "Pricing": 6, // Giá xe
            "BikePricing": 7, // Giá xe máy
            "BikeAssessment": 8 // Đánh giá xe máy
        }

        $scope.PageStatus = {
            "Temp": 0,
            "Pending": 1,
            "PendingApproved": 2,
            "Published": 3,
            "Returned": 4,
            "UnPublished": 5,
            "Deleted": 6
        }

        $scope.DisplayStyleEnum = {
            "Image": 2,
            "Video": 3,
            "Media": 4
        }

        $scope.DisplayStyleOnListEnum = {
            "Normal": 0,
            "Slide": 1,
            "Cover": 2,
            "Alert": 3
        }

        $scope.PageRelationStatusEnum = {
            "Active": 1, //Đang hiển thị
            "Sticky": 2, //Đang neo top
            "Blocked": 3 //Đang bị block
        }

        $scope.getPageRelationStatusName = function (status) {
            if (status == $scope.PageRelationStatusEnum.Active) {
                return "Đang hiển thị";
            }
            else if (status == $scope.PageRelationStatusEnum.Sticky) {
                return "Đang neo top";
            }
            else if (status == $scope.PageRelationStatusEnum.Blocked) {
                return "Đang bị khóa";
            }
        }

        $scope.CarInfoSimilarStatusEnum = {
            "Active": 1, //Đang hiển thị
            "Sticky": 2, //Đang neo top
            "Blocked": 3 //Đang bị block
        }

        $scope.ListPageRelationModels = {
            selected: null,
            lists: { "Source": [], "Target": [] }
        };

        //Init tag
        $('#txtAddTags').tagsinput({
            confirmKeys: [],
            maxTags: 10,
            trimValue: true,
            allowDuplicates: false,
            itemValue: 'Id',
            itemText: 'Name'
        });

        $scope.TagsModel = {
        };

        $scope.ListCategoryEnum = [
            { "Id": 1, "Name": "Tin tức doanh nghiệp" },
            { "Id": 2, "Name": "Blogs" }
        ];

        $('#modalAddTags').on('shown.bs.modal', function () {
            var tagsName = $("#tagsGroup .ui-autocomplete-input").val();
            $scope.TagsModel.Name = tagsName;
            $("#tagsGroup .ui-autocomplete-input").val("");
            $("#txtName").val(tagsName);
            $("#txtName").focus();
            $("#txtKeyword").val("");
            CKEDITOR.instances["txtDescription"].setData("");
        });

        $('#modalAddTags').on('hidden.bs.modal', function () {
            $scope.TagsModel = {
            };
        });

        //Key press add tags
        $scope.keypressOpenModal = function (keyEvent) {
            if (keyEvent.which === 13) {
                var tagsName = $("#tagsGroup .ui-autocomplete-input").val();
                $scope.TagsModel.Name = tagsName;
                //$scope.addNewTags();
                $('#modalAddTags').modal('show');
            }
        }

        //Key press add tags
        $scope.keypressAddTags = function (keyEvent) {
            if (keyEvent.which === 13) {
                $scope.doAddTags();
            }
        }

        $scope.Init = function (obj) {
            if (obj) {
                $scope.Page = obj;
                $scope.Page.Id = obj.IdStr;
                CKEDITOR.on("instanceReady", function () {
                    window.setTimeout(function () {
                        CKEDITOR.instances["content"].setData(obj.Description);
                    }, 200);
                });

                // scroll
                $('#updatePageBlock').niceScroll({ touchbehavior: false, cursoropacitymax: 0.6, cursorwidth: 5 });
            }
            openAsidePanel("sm");
            $('body').addClass("aside-edit-panel-fixed");

        };

        //=====Get list car info=====
        //===========================

        $scope.page = {
            'totalItems': 0,
            'currentPage': 1,
            'itemsPerPage': 10
        };

        //Carinfo model
        $scope.CarInfoModel = {};

        $scope.Search = {};
        $scope.ParamModel = {};
        $scope.IsFromExternal = true;

        $('#modalAddCarInfo').on('shown.bs.modal', function () {
            $scope.IsFromExternal = true;
            $scope.Search.BrandId = $scope.Page.PageExtend.BrandId;
            $scope.Search.ModelId = $scope.Page.PageExtend.ModelId;
            $scope.getListDataForControl();
            $scope.getListData();
        });

        //Get list credit card data
        $scope.getListData = function () {
            $scope.Search.PageIndex = $scope.page.currentPage;
            $scope.Search.PageSize = $scope.page.itemsPerPage;
            $scope.Search.PageIdEncrypt = $scope.Page.EncryptPageId;
            Service.post("/CarInfo/SearchForPage", { searchModel: $scope.Search })
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListCarInfo = response.Data;
                        $scope.page.totalItems = response.TotalRow;
                    }
                });
        };

        //Get list data for control
        $scope.getListDataForControl = function () {
            Service.post("/CarInfo/GetDataForControl")
                .then(function (response) {
                    if (response.Success) {
                        $scope.ParamModel = response.Data;
                        if ($scope.Search.BrandId > 0) {
                            $scope.getListCarModels($scope.Search.BrandId);
                        }
                        $scope.SelectInit();
                    }
                });
        }

        // Click button search
        $scope.doSearch = function () {
            $scope.page.currentPage = 1;
            $scope.getListData();
        }

        //Get list car model
        $scope.getListCarModels = function (brandId) {
            Service.post("/CarInfo/GetCarModels", { brandId: brandId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.ParamModel.ListCarModels = response.Data;
                        if ($scope.Search.ModelId == null) {
                            $scope.Search.ModelId = $scope.Page.PageExtend.ModelId;
                        }
                        $scope.SelectInit();
                    }
                });
        }

        //Init chosen for select option
        $scope.SelectInit = function () {
            setTimeout(function () {
                $(".chosen-select").chosen();
                $('.chosen-select').trigger('chosen:updated');
            }, 200);
        }

        //Init cái lịch
        $scope.DatePickerInit = function () {
            $(".datepicker-short").datetimepicker({
                format: 'DD/MM/YYYY',
                locale: "vi",
                keepOpen: false
            });
        }


        //Get car info to Page
        $scope.getCarInfo = function (id) {
            if (id > 0) {
                Service.post("/CarInfo/GetCarInfoUpdate", { carInfoId: id })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.CarInfoModel = response.Data;
                            $scope.updatePageCarInfo();
                            $("#modalAddCarInfo").modal("hide");
                        }
                    });
            }
        }

        //Update car info in Page
        $scope.updatePageCarInfo = function () {
            $scope.getListCarModelsForPage($scope.CarInfoModel.BrandId);
            $scope.getListCarModelDetails($scope.CarInfoModel.BrandId, $scope.CarInfoModel.ModelId);
            $scope.Page.PageExtend.BrandId = $scope.CarInfoModel.BrandId;
            $scope.Page.PageExtend.ModelId = $scope.CarInfoModel.ModelId;
            $scope.Page.PageExtend.CarModelDetailId = $scope.CarInfoModel.ModelDetailId;
            $scope.Page.PageExtend.CarStyleId = $scope.CarInfoModel.StyleId;
            $scope.Page.PageExtend.CarSegmentId = $scope.CarInfoModel.SegmentId;
            $scope.Page.PageExtend.ProductionYear = $scope.CarInfoModel.ProductionYear;
        }

        //On change car segment
        $scope.onChangeCarSegment = function () {
            $scope.doSearch();
        }

        //On change brand
        $scope.onChangeCarBrand = function () {
            $scope.ParamModel.ListCarModels = [];
            $scope.Search.ModelId = 0;
            if ($scope.Search.BrandId > 0) {
                $scope.getListCarModels($scope.Search.BrandId);
            } else {
                $scope.SelectInit();
            }
            $scope.doSearch();
        }

        //On change model
        $scope.onChangeCarModel = function () {
            if ($scope.CarInfoModel.CurrentModel > 0) {
            } else {
                $scope.SelectInit();
            }
            $scope.doSearch();
        }

        //On change car style
        $scope.onChangeCarStyle = function () {
            $scope.doSearch();
        }

        //On change production year
        $scope.onChangeProductionYear = function () {
            $scope.doSearch();
        }

        //Key press search
        $scope.keypressSearch = function (keyEvent) {
            if (keyEvent.which === 13) {
                $scope.doSearch();
            }
        }

        $scope.ChangeStatusPage = function (statusPage, Idstr) {
            Service.post("/Page/ChangeStatusPage", { statusPage: statusPage, Idstr: Idstr })
                .then(function (response) {
                    if (response.ErrorCode == 0) {
                        notify({
                            message: "Cập nhật thành công",
                            classes: $scope.classes.Success,
                            templateUrl: $scope.template,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                        allowreload = true;
                        $scope.IsSubmited = true;
                        window.setTimeout(function () {
                            window.close();
                        }, $scope.duration);
                    } else {
                        notify({
                            message: response.Message,
                            classes: $scope.classes.Error,
                            templateUrl: $scope.template,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                    }
                });
        }

        //On change title Page
        $scope.onChangeTitlePage = function () {
            $scope.Page.AvatarDesc = $scope.Page.Title;
            if ($scope.Page.Title) {
                if (!$scope.Page.StatusOfPage.IsPublished && !$scope.Page.StatusOfPage.IsUnPublished) {
                    $scope.Page.OriginalUrl = UnicodeToUnsignAndSlash($scope.Page.Title);
                }
            }
            $scope.Page.SEOTitle = $scope.Page.Title;
        }

        $scope.BlurTitleInput = function () {
            $("#txtOriginalUrl").blur();
        }

        //On change sapo Page
        $scope.onChangeSapoPage = function () {
            $scope.Page.SEODescription = $scope.Page.Sapo;
        }

        $scope.AutoCheckLinkOriginalUrl = function (url) {
            Service.post("/Page/AutoCheckLinkOriginalUrl", { "url": url })
                .then(function (result) {
                    if (result.Success && result.Data) {

                    }
                });
        }

        $scope.doUpdate = function (currentStatus, status) {
            $scope.Page.Description = CKEDITOR.instances["content"].getData();
            if ($scope.Page.Description.length <= 0) {
                notify({
                    message: "Vui lòng nhập nội dung bài viết!",
                    classes: $scope.classes.Error,
                    templateUrl: $scope.template,
                    position: $scope.position,
                    duration: $scope.duration
                });
                return false;
            }

            // bài ảnh lớn buộc phải nhập avatar2, còn lại all phải nhập avatar
            //if (!$scope.Page.Avatar || $scope.Page.Avatar.length <= 0) {
            //    notify({
            //        message: "Bạn chưa chọn ảnh đại diện!",
            //        classes: $scope.classes.Error,
            //        templateUrl: $scope.template,
            //        position: $scope.position,
            //        duration: $scope.duration
            //    });
            //    return false;
            //}
            //if ($scope.Page.DisplayStyleOnList == $scope.DisplayStyleOnListEnum.Cover && (!$scope.Page.Avatar2 || $scope.Page.Avatar2.length <= 0)) {
            //    notify({
            //        message: "Bạn chưa chọn ảnh nổi bật!",
            //        classes: $scope.classes.Error,
            //        templateUrl: $scope.template,
            //        position: $scope.position,
            //        duration: $scope.duration
            //    });
            //    return false;
            //}

            //Process for menu heading
            var editor = CKEDITOR.instances['content'];
            var listElemH = $(editor.document.$).find(".dvs_menuheading_item");
            if (listElemH.length > 0) {
                $scope.Page.ListMenuHeading = [];
                $(listElemH).each(function () {
                    var menuHeading = new Object();
                    menuHeading.NameExtend = $(this).html();
                    menuHeading.MenuId = $(this).attr("dataId");
                    menuHeading.Ordinal = $(this).attr("dataOrdinal");
                    $scope.Page.ListMenuHeading.push(menuHeading);
                });
            }
            // processing video in content
            //var hour = parseInt($scope.VideoDuration.Hour);
            //var min = parseInt($scope.VideoDuration.Minute);
            //var sec = parseInt($scope.VideoDuration.Second);
            //if (hour > 0 || min > 0 || sec > 0) {
            //    strDuration = (hour > 9 ? hour : "0" + hour) + ":" + (min > 9 ? min : "0" + min) + ":" + (sec > 9 ? sec : "0" + sec);
            //    $scope.Page.VideoDuration = strDuration;
            //}

            var lstObjVideo = $(editor.document.$).find(".dvsvideo");
            if (lstObjVideo.length > 0) {
                // nếu có video trong bài thì không cho chọn AMP;
                $(lstObjVideo).each(function () {
                    $scope.Page.Description = $scope.Page.Description.replace(/data-raw="([^"]+)"/, "");
                    return;
                });
            }

            // xử lý xóa ký tự trống ở title box nhúng nội dung
            $scope.Page.Description = $scope.Page.Description.replace('<p class="dvs-textbox-title">&nbsp;</p>', '<p class="dvs-textbox-title"></p>');

            //$scope.Page.PublishedDateStr = $('#txtPublishedDate').val();
            Service.post("/Page/UpdatePage", { Page: $scope.Page, currentStatus: currentStatus })
                .then(function (response) {
                    if (response.Success) {
                        notify({
                            message: "Cập nhật thành công",
                            classes: $scope.classes.Success,
                            templateUrl: $scope.template,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                        allowreload = true;
                        $scope.IsSubmited = true;
                        window.setTimeout(function () {
                            window.close();
                        }, $scope.duration);

                    } else {
                        notify({
                            message: response.Message,
                            classes: $scope.classes.Error,
                            templateUrl: $scope.template,
                            position: $scope.position,
                            duration: $scope.duration
                        });

                        // trả lại status gốc của tin về trạng thái ban đầu
                        $scope.Page.Status = currentStatus;
                    }
                });
        }

        var doAutoSave = function () {
            $scope.Page.Description = CKEDITOR.instances["content"].getData();
            var data = $scope.Page.Description.replace(/<[^>]+>/g, '\n').replace(/\n\s*\n/g, '\n\n').trim();
            if (data.length <= 0) {
                return;
            }
            $scope.Page.DistributionDateStr = $("#txtDistributionDate").val();
            $scope.Page.PricingDateStr = $("#txtPricingDate").val();
            $scope.Page.ListTags = $("#txtAddTags").tagsinput("items");
            $scope.Page.TagItem = "";
            $scope.Page.CarInfo = $scope.CarInfoModel;

            if (!$scope.Page.IsReference) {
                $scope.Page.ReferenceUrl = null;
            }
            var len = $scope.Page.ListTags.length;
            for (var i = 0; i < len; i++) {
                $scope.Page.TagItem = $scope.Page.TagItem + $scope.Page.ListTags[i].Name + ";";
            }
            if ($scope.Page.ListPageRelation != null && $scope.Page.ListPageRelation != undefined) {
                for (var i = 0; i < $scope.Page.ListPageRelation.length; i++) {
                    if ($scope.Page.ListPageRelation[i].IsSticky) {
                        $scope.Page.ListPageRelation[i].StickyFromDate = $("[name='txtRelaStickyFromDateRelation" + i + "']").val();
                        $scope.Page.ListPageRelation[i].StickyUntilDate = $("[name='txtRelaStickyUntilDateRelation" + i + "']").val();
                    }
                }
            }

            //Process for menu heading
            var editor = CKEDITOR.instances['content'];
            var listElemH = $(editor.document.$).find(".dvs_menuheading_item");
            if (listElemH.length > 0) {
                $scope.Page.ListMenuHeading = [];
                $(listElemH).each(function () {
                    var menuHeading = new Object();
                    menuHeading.NameExtend = $(this).html();
                    menuHeading.MenuId = $(this).attr("dataId");
                    $scope.Page.ListMenuHeading.push(menuHeading);
                });
            }

            // processing video in content
            var hour = parseInt($scope.VideoDuration.Hour);
            var min = parseInt($scope.VideoDuration.Minute);
            var sec = parseInt($scope.VideoDuration.Second);
            if (hour > 0 || min > 0 || sec > 0) {
                strDuration = (hour > 9 ? hour : "0" + hour) + ":" + (min > 9 ? min : "0" + min) + ":" + (sec > 9 ? sec : "0" + sec);
                $scope.Page.VideoDuration = strDuration;
            }

            var lstObjVideo = $(editor.document.$).find(".dvsvideo");
            if (lstObjVideo.length > 0) {
                $(lstObjVideo).each(function () {
                    var dataRaw = $(this).attr("data-raw");
                    if (dataRaw && dataRaw.length > 0)
                        $scope.Page.PageExtend.VideoUrl = $(this).attr("data-raw");
                    $scope.Page.PageExtend.VideoThumb = $(this).attr("data-raw-image");
                    if (!$scope.Page.VideoDuration || $scope.Page.VideoDuration == null) $scope.Page.VideoDuration = $(this).attr("data-duration");
                    $scope.Page.Description = $scope.Page.Description.replace(/data-raw="([^"]+)"/, "");
                    return;
                });
            }
            else {
                $scope.Page.PageExtend.VideoUrl = null;
                $scope.Page.PageExtend.VideoThumb = null;
                $scope.Page.VideoDuration = null;
            }

            Service.post("/Page/AutoSave", { Page: $scope.Page })
                .then(function (response) {
                    //if (response.Success) {
                    //    notify({
                    //        message: "Hệ thống tự động lưu bản nháp!",
                    //        classes: $scope.classes.Success,
                    //        templateUrl: $scope.template,
                    //        position: $scope.position,
                    //        duration: $scope.duration
                    //    });
                    //} else {
                    //    notify({
                    //        message: response.Message,
                    //        classes: $scope.classes.Error,
                    //        templateUrl: $scope.template,
                    //        position: $scope.position,
                    //        duration: $scope.duration
                    //    });
                    //}
                });
        }

        //Save Page
        $scope.doSave = function () {
            ngConfirm($scope.doUpdate, 0, 0,cms.message.confirmSave);
        }

        //Publish Page
        $scope.doPublish = function () {
            ngConfirm($scope.doUpdate, $scope.Page.Status, $scope.PageStatus.Published, cms.message.confirmDoPublish);
        }

        //Gỡ bài đã publish
        $scope.doUnPublish = function () {
            ngConfirm($scope.doUpdate, $scope.Page.Status, $scope.PageStatus.UnPublished, cms.message.confirmDoUnPublish);
        }

        //Gửi bài lên chờ duyệt
        $scope.doPendingApprove = function () {
            ngConfirm($scope.doUpdate, $scope.Page.Status, $scope.PageStatus.PendingApproved, cms.message.confirmDoPendingApprove);
        }

        // xóa bài
        $scope.doDelete = function () {
            ngConfirm($scope.doUpdate, $scope.Page.Status, $scope.PageStatus.Deleted, cms.message.confirmDoDelete);
        }

        var ngConfirm = function (callback, currentStatus, status, message) {
            $ngConfirm({
                title: cms.message.confirmWarningTitle,
                content: message,
                type: cms.configs.ngConfirm.warningColor,
                scope: $scope,
                buttons: {
                    OK: {
                        type: cms.configs.ngConfirm.warningOKText,
                        btnClass: cms.configs.ngConfirm.warningOKClass,
                        keys: ['enter'],
                        action: function (scope, button) {
                            callback(currentStatus, status);
                        }
                    },
                    Cancel: {
                        type: cms.configs.ngConfirm.warningCancelText,
                        btnClass: cms.configs.ngConfirm.warningCancelClass,
                        keys: ['esc'],
                        action: function (scope, button) {
                        }
                    },
                }
            });
        }

        //Add Page tags
        $scope.doAddTags = function () {
            if ($scope.TagsModel.Name && $scope.TagsModel.Name.trim() != '') {

                $scope.TagsModel.Description = CKEDITOR.instances["txtDescription"].getData();
                Service.post("/Tags/Update", { tagsModel: $scope.TagsModel })
                    .then(function (response) {
                        if (response.Success) {
                            notify({
                                message: Const.message.update_succsess,
                                classes: $scope.classes.Success,
                                position: $scope.positions.Center,
                                duration: $scope.duration
                            });
                            setTimeout(function () {
                                $("#modalAddTags").modal("hide");
                                $('#txtAddTags').tagsinput('add', { Id: response.Data, Name: $scope.TagsModel.Name }, { preventPost: true });
                            }, $scope.duration);
                        } else {
                            notify({
                                message: "Lỗi! " + response.Message,
                                classes: $scope.classes.Error,
                                position: $scope.positions.Center,
                                duration: $scope.duration
                            });
                            //setTimeout(function () {
                            //    $("#modalAddTags").modal("hide");
                            //}, $scope.duration);
                        }
                    });
            } else {
                notify({
                    message: "Tên tags không được để trống",
                    classes: $scope.classes.Error,
                    position: $scope.positions.Center,
                    duration: $scope.duration
                });
            }
        }

        //Xem ảnh gốc avatar hoặc ảnh slide
        $scope.previewImages = function (imageurl) {
            if (imageurl.indexOf(cms.configs.CropSizeCMS) > -1) {
                imageurl = imageurl.replace(cms.configs.CropSizeCMS, "/");
            }
            popupCenter("/FileManager/Default.aspx?ac=article&imgurl=" + imageurl, "File manager", 950, 600);
            //window.open("/FileManager/Default.aspx?ac=article&imgurl=" + imageurl, "File manager", "width = 950, height = 600");
        }

        //Chọn lại ảnh avatar hoặc ảnh slide
        $scope.selectImages = function (index) {
            //var w = window.open("/FileManager/Default.aspx?ac=article", "File manager", "width = 950, height = 600");
            var w = popupCenter("/FileManager/Default.aspx?ac=article", "File manager", 950, 600);

            w.callback = function (lst) {
                if (!lst || lst.length <= 0) {
                    alert("Không có file nào được chọn!");
                    return;
                }
                var result = lst[0];

                if (/(\.|\/)(gif|jpe?g|png)$/i.exec(result.path) == null) {
                    alert("Bạn cần chọn hình ảnh");
                    return;
                }

                if (index == "avatar1") {
                    $scope.$apply(function () {
                        $scope.Page.Avatar = result.path;
                        $scope.Page.AvatarStr = result.path;
                    });
                }
                else if (index == "avatar2") {
                    $scope.$apply(function () {
                        $scope.Page.Avatar2 = result.path;
                        $scope.Page.AvatarStr2 = result.path;
                    });
                }
                else {
                    $scope.$apply(function () {
                        $scope.Page.ListImage[index].ImageUrl = result.path;
                        $scope.Page.ListImage[index].ImageUrlCrop = result.path;
                    });
                }
                w.close();
            }
        }
        //Xóa ảnh avatar hoặc ảnh slide
        $scope.delImages = function (index) {
            if (index == "avatar1") {
                $scope.Page.Avatar = "";
                $scope.Page.AvatarStr = cms.configs.NoImage;
            }
            else if (index == "avatar2") {
                $scope.Page.Avatar2 = "";
                $scope.Page.AvatarStr2 = cms.configs.NoImage;
            }
            else {
                $scope.Page.ListImage.splice(index, 1);
            }
        }

        //Thêm ảnh slide
        $scope.addImages = function () {
            //var w = window.open("/FileManager/Default.aspx?ac=article", "File manager", "width = 950, height = 600");
            var w = popupCenter("/FileManager/Default.aspx?ac=article", "File manager", 950, 600);

            w.callback = function (lst) {
                if (!lst || lst.length <= 0) {
                    alert("Không có file nào được chọn!");
                    return;
                }

                for (i = 0; i < lst.length; i++) {
                    var result = lst[i];

                    if (/(\.|\/)(gif|jpe?g|png)$/i.exec(result.path) == null) {
                        alert("Bạn cần chọn hình ảnh");
                        continue;
                    }


                    var imageadd = new Object();
                    imageadd.Id = 0;
                    imageadd.ImageUrl = result.path;
                    imageadd.ImageUrlCrop = result.path;
                    $scope.$apply(function () {
                        if ($scope.Page.ListImage == null) {
                            $scope.Page.ListImage = [];
                        }
                        $scope.Page.ListImage.push(imageadd);
                    });
                }
                w.close();
            }
        }
        $scope.OldPageType = "";
        $scope.ChangePageType = function () {
            if (!confirm('Nếu đổi loại tin, một vài thông tin khác có thể sẽ bị thay đổi (VD: giá xe, đánh giá xe,...). Bạn có chắc chắn đổi không?')) {
                $scope.Page.PageType = $scope.OldPageType;
            }
            $scope.SelectInit();
            $scope.OldPageType = $scope.Page.PageType;
            if ($scope.Page.PageType == $scope.PageTypeEnum.Gallery) {
                $scope.Page.DisplayStyle = 2; //Mặc định là bài ảnh
            }

            // reset PageExtend
            $scope.Page.PageExtend.BrandId = 0;
            $scope.Page.PageExtend.ModelId = 0;
            $scope.Page.PageExtend.CarModelDetailId = 0;
            $scope.Page.PageExtend.CarStyleId = 0;
            $scope.Page.PageExtend.CarSegmentId = 0;
            $scope.Page.PageExtend.ProductionYear = "";

            $scope.Page.TypeOfPage.IsAssessment = false;
            $scope.Page.TypeOfPage.IsBikeAssessment = false;
            $scope.Page.TypeOfPage.IsPricing = false;
            $scope.Page.TypeOfPage.IsBikePricing = false;
            $scope.Page.TypeOfPage.IsCarInfo = false;
            switch ($scope.Page.PageType) {
                case $scope.PageTypeEnum.CarInfo:
                    {
                        $scope.Page.TypeOfPage.IsCarInfo = true;
                        break;
                    }
                case $scope.PageTypeEnum.Assessment:
                    {
                        $scope.Page.TypeOfPage.IsAssessment = true;
                        break;
                    }
                case $scope.PageTypeEnum.BikeAssessment:
                    {
                        $scope.Page.TypeOfPage.IsBikeAssessment = true;
                        break;
                    }
                case $scope.PageTypeEnum.Pricing:
                    {
                        $scope.Page.TypeOfPage.IsPricing = true;
                        break;
                    }
                case $scope.PageTypeEnum.BikePricing:
                    {
                        $scope.Page.TypeOfPage.IsBikePricing = true;
                        break;
                    }
            }
        }

        $scope.CateChange = function () {
            if ($scope.Page.CateId == null) {
                $scope.Page.CateId = 0;
            }
        }

        $scope.GetTopic = function () {
            Service.post("/Page/GetTopic")
                .then(function (response) {
                    if (response.Success) {
                        $scope.Page.ListTopic = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        //Get list car model
        $scope.getListCarModelsForPage = function (brandId) {
            Service.post("/Page/GetCarModelByCarBrandId", { brandId: brandId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.Page.ListCarModel = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        //Get list bike model
        $scope.getListBikeModelsForPage = function (brandId) {
            Service.post("/Page/GetBikeModelByBikeBrandId", { brandId: brandId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.Page.ListBikeModel = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        //Get list car model details
        $scope.getListCarModelDetails = function (brandId, modelId) {
            Service.post("/CarInfo/GetCarModelDetails", { brandId: brandId, modelId: modelId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.Page.ListCarModelDetail = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        $scope.BrandChange = function () {
            if ($scope.Page.PageExtend.BrandId == null) {
                $scope.Page.PageExtend.BrandId = 0;
            }
            if ($scope.Page.TypeOfPage.IsAssessment || $scope.Page.TypeOfPage.IsPricing || $scope.Page.TypeOfPage.IsCarInfo)
                $scope.getListCarModelsForPage($scope.Page.PageExtend.BrandId);
            else if ($scope.Page.TypeOfPage.IsBikeAssessment || $scope.Page.TypeOfPage.IsBikePricing)
                $scope.getListBikeModelsForPage($scope.Page.PageExtend.BrandId);

        }

        $scope.ModelChange = function () {
            if ($scope.Page.PageExtend.ModelId == null) {
                $scope.Page.PageExtend.ModelId = 0;
            }
            if ($scope.Page.TypeOfPage.IsAssessment || $scope.Page.TypeOfPage.IsPricing || $scope.Page.TypeOfPage.IsCarInfo)
                $scope.getListCarModelDetails($scope.Page.PageExtend.BrandId, $scope.Page.PageExtend.ModelId);
        }

        $scope.showPageRelation = function () {
            closeAsidePanel("sm");
            $scope.CurrentForm = $scope.CurrentFormEnum.PageRelation;
            $scope.getSuggestionList();
            $scope.ListPageSearched = [];
            $scope.SearchPageRelationModel.PageType = $scope.Page.PageType;
            $scope.SearchPageRelationModel.DisplayStyle = $scope.Page.DisplayStyle;
            $scope.SearchPageRelationModel.CateId = $scope.Page.CateId;
            //$("#divPagerela").show();
            //$("#tab1").trigger("click");
            $scope.getSearchInfo();
            scrollTo($("html"));
        }

        $scope.backtoedit = function () {
            openAsidePanel("sm");
            $("#divPagerela").hide();
            scrollTo($("#btnshowrela"));

        }

        $scope.getSuggestionList = function () {
            $scope.ListPageSuggested = [];
            Service.post("/Page/GetListSuggestionPage",
                {
                    ModelId: $scope.Page.PageExtend.ModelId,
                    BrandId: $scope.Page.PageExtend.BrandId,
                    CarSegmentId: $scope.Page.PageExtend.CarSegmentId,
                    CarStyleId: $scope.Page.PageExtend.CarStyleId,
                    PageType: $scope.Page.PageType,
                    CateId: $scope.Page.CateId,
                    CurrentPageId: $scope.Page.Id,
                    ListTags: $("#txtAddTags").tagsinput("items"),
                    DisplayStyle: $scope.Page.DisplayStyle
                }
            )
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListPageRelationModels.lists.Source = $scope.ListPageSuggested = response.Data;
                    }
                });
        }

        $scope.getSearchInfo = function () {
            $scope.SearchPageRelationModel.ListCategory = [];
            for (var i = 0; i < $scope.Page.ListCategory.length; i++) {
                if ($scope.Page.ListCategory[i].Type == $scope.SearchPageRelationModel.PageType) {
                    $scope.SearchPageRelationModel.ListCategory.push($scope.Page.ListCategory[i]);
                }
            }
            $scope.SelectInit();
        }

        $scope.searchNewRelation = function () {
            $scope.pageSearchPageRelation.currentPage = 1;
            $scope.getListNewRelation();
        }

        $scope.getListNewRelation = function () {
            Service.post("/Page/SearchPage",
                {
                    CurrentPageId: $scope.Page.Id,
                    PageType: $scope.SearchPageRelationModel.PageType,
                    DisplayStyle: $scope.SearchPageRelationModel.DisplayStyle,
                    PageRelateId: $scope.SearchPageRelationModel.PageRelateId,
                    Keyword: $scope.SearchPageRelationModel.Keyword,
                    CateId: $scope.SearchPageRelationModel.CateId,
                    PageIndex: $scope.pageSearchPageRelation.currentPage,
                    PageSize: $scope.pageSearchPageRelation.itemsPerPage
                }
            )
                .then(function (response) {
                    if (response.Success) {
                        $scope.pageSearchPageRelation.totalItems = response.Data.TotalRows;
                        $scope.ListPageSearched = response.Data.ListPageSearched;
                        $scope.ListPageRelationModels.lists.Source = $scope.ListPageSearched;
                    }
                });
        }

        $scope.PageRelationChangeTab = function (tab) {
            $scope.CurrentPageRelationTab = tab;
            if (tab == $scope.CurrentPageRelationTabEnum.Search) {
                $scope.searchNewRelation();
            } else if (tab == $scope.CurrentPageRelationTabEnum.Suggest) {
                $scope.ListPageRelationModels.lists.Source = $scope.ListPageSuggested;
            }
        }

        $scope.clickStickyPageRelation = function (index, oldStatus) {
            $scope.Page.ListPageRelation[index].IsSticky = !$scope.Page.ListPageRelation[index].IsSticky;
            if ($scope.Page.ListPageRelation[index].IsSticky) {
                $scope.Page.ListPageRelation[index].Status = $scope.PageRelationStatusEnum.Sticky;
            }
            else {
                if (oldStatus == $scope.PageRelationStatusEnum.Sticky)
                    $scope.Page.ListPageRelation[index].Status = $scope.PageRelationStatusEnum.Active;
                else
                    $scope.Page.ListPageRelation[index].Status = oldStatus;
            }
        }

        $scope.dropCallback = function (listName, item) {
            if (listName == 'Target') {
                // xóa bài trùng khi kéo sang
                var mappedList = $scope.ListPageRelationModels.lists.Target.map(function (e) {
                    return e.PageRelateIdStr;
                });
                var index = mappedList.indexOf(item.PageRelateIdStr);

                if (index >= 0) {
                    $scope.ListPageRelationModels.lists.Target.splice(index, 1);
                }

                $scope.DatePickerInit();
                return item;
            } else if (listName == 'Source') {

                // xóa bài trùng khi kéo trả
                var mappedList = $scope.ListPageRelationModels.lists.Source.map(function (e) {
                    return e.PageRelateIdStr;
                });
                var index = mappedList.indexOf(item.PageRelateIdStr);
                if (index >= 0) {
                    $scope.ListPageRelationModels.lists.Source.splice(index, 1);
                }
                return item;
            }
        }

        $scope.sendItem = function (item) {
            var mappedList = $scope.ListPageRelationModels.lists.Source.map(function (e) {
                return e.PageRelateIdStr;
            });
            var index = mappedList.indexOf(item.PageRelateIdStr);
            if (index >= 0) {

                // nếu list targer có bài rồi thì thôi
                var mappedListTarget = $scope.ListPageRelationModels.lists.Target.map(function (e) {
                    return e.PageRelateIdStr;
                });
                var indexTarget = mappedListTarget.indexOf(item.PageRelateIdStr);
                if (indexTarget < 0) {
                    $scope.ListPageRelationModels.lists.Target.push(item);
                }

                $scope.ListPageRelationModels.lists.Source.splice(index, 1);
            }
        }

        $scope.sortableOptions = {
            placeholder: "connectedSortableItem",
            connectWith: ".connectedSortable",
            cancel: ".not-sortable",
            'receive': receiveCallback
        };

        function receiveCallback(e, ui) {
            var item = ui.item.sortable.model;
            var listName = ui.item.sortable.droptarget.hasClass('Source') ? 'Source' : 'Target';
            $scope.dropCallback(listName, item);
        }

        $scope.removeItemTarget = function (i, item) {
            var mappedList = $scope.ListPageRelationModels.lists.Source.map(function (e) {
                return e.PageRelateIdStr;
            });
            var index = mappedList.indexOf(item.PageRelateIdStr);

            $scope.ListPageRelationModels.lists.Target.splice(i, 1);
            if (index >= 0) {
                $scope.ListPageRelationModels.lists.Source.splice(index, 1);
            }
            $scope.ListPageRelationModels.lists.Source.push(item);
        }

        $scope.moveItemTarget = function (index, param) {
            if (index + param >= 0 && index + param < $scope.ListPageRelationModels.lists.Target.length) {
                var arr = $scope.ListPageRelationModels.lists.Target;
                arr = array_move(arr, index, index + param);
                $scope.ListPageRelationModels.lists.Target = arr;
            }
        }
        $scope.closeEditContent = function () {
            closeAsidePanel("sm");
        };
        $scope.openEditContent = function () {
            openAsidePanel("sm");
        };

        $scope.backToEdit = function () {
            openAsidePanel("sm");
            $scope.CurrentForm = $scope.CurrentFormEnum.Edit;
        }

        $scope.ChangeRelaPageType = function () {
            $scope.SearchPageRelationModel.ListCategory = [];
            for (var i = 0; i < $scope.Page.ListCategory.length; i++) {
                if ($scope.Page.ListCategory[i].Type == $scope.SearchPageRelationModel.PageType) {
                    $scope.SearchPageRelationModel.ListCategory.push($scope.Page.ListCategory[i]);
                }
            }
            $scope.SelectInit();
        }

        $scope.getPageTypeName = function (PageType) {
            return getPageTypeName(PageType);
        }

        $scope.addNewTags = function () {
            $ngConfirm({
                title: 'Thêm mới tags',
                //content: '<div class="form-group">' +
                //'<input type="text" id="txtName" name="txtName" required="required" class="form-control" ng-model="TagsModel.Name" ng-keypress="keypressAddTags($event)" maxlength="80">' +
                //'</div>',
                contentUrl: '/template/_AddTags.html',
                scope: $scope,
                buttons: {
                    OK: {
                        text: 'Thêm mới +',
                        btnClass: cms.configs.ngConfirm.infoOKClass,
                        keys: ['enter'],
                        action: function (scope, button) {
                            $scope.doAddTags();
                        }
                    },
                    Cancel: {
                        type: cms.configs.ngConfirm.infoCancelText,
                        btnClass: cms.configs.ngConfirm.infoCancelClass,
                        keys: ['esc'],
                        action: function (scope, button) {
                        }
                    }
                }
            });
        };

        $scope.substring = function (str, length) {
            if (str.length > length)
                return str.substring(0, length) + "...";
            else
                return str
        }

        function array_move(arr, old_index, new_index) {
            if (new_index >= arr.length) {
                var k = new_index - arr.length + 1;
                while (k--) {
                    arr.push(undefined);
                }
            }
            arr.splice(new_index, 0, arr.splice(old_index, 1)[0]);
            return arr; // for testing
        };

        $scope.roundNumberPaging = function (itemPerPage, totalItem) {
            return Math.ceil(parseFloat(totalItem) / itemPerPage);
        }
    }]);
})();
var lstError = new Array();
$(document).ready(function () {
    $('body').addClass('sidebar-hidden');
    var areaedit = $("#divedit");
    //#Phần này để ở đây để khi load form nó sẽ validate và bật sự kiện blur cho các element luôn
    $("input[required]", areaedit).each(function () {
        if ($(this).val() == "") {
            $(this).addClass("parsley-error");
            $("#error" + $(this).attr("id")).show();
            if (lstError.indexOf($(this).attr("id")) < 0) {
                lstError.push($(this).attr("id"));
            }
        } else {
            $(this).removeClass("parsley-error");
            $("#error" + $(this).attr("id")).hide();
            if (lstError.indexOf($(this).attr("id")) > -1) {
                lstError.splice(lstError.indexOf($(this).attr("id")), 1);
            }
        }
        $(this).blur(function () {
            if ($(this).val() == "") {
                $(this).addClass("parsley-error");
                $("#error" + $(this).attr("id")).show();
                if (lstError.indexOf($(this).attr("id")) < 0) {
                    lstError.push($(this).attr("id"));
                }
            } else {
                $(this).removeClass("parsley-error");
                $("#error" + $(this).attr("id")).hide();
                if (lstError.indexOf($(this).attr("id")) > -1) {
                    lstError.splice(lstError.indexOf($(this).attr("id")), 1);
                }
            }
        });
    });
    $("textarea.ckeditor", areaedit).each(function () {
        if ($(this).val() == "") {
            $(this).addClass("parsley-error");
            $("#error" + $(this).attr("id")).show();
            if (lstError.indexOf($(this).attr("id")) < 0) {
                lstError.push($(this).attr("id"));
            }
        } else {
            $(this).removeClass("parsley-error");
            $("#error" + $(this).attr("id")).hide();
            if (lstError.indexOf($(this).attr("id")) > -1) {
                lstError.splice(lstError.indexOf($(this).attr("id")), 1);
            }
        }
        var textarea = $(this);
        CKEDITOR.instances[textarea.attr("id")].on('blur', function () {
            var content = CKEDITOR.instances[textarea.attr("id")].getData();
            if (content == "") {
                textarea.addClass("parsley-error");
                $("#error" + textarea.attr("id")).show();
                if (lstError.indexOf(textarea.attr("id")) < 0) {
                    lstError.push(textarea.attr("id"));
                }
            } else {
                textarea.removeClass("parsley-error");
                $("#error" + textarea.attr("id")).hide();
                if (lstError.indexOf(textarea.attr("id")) > -1) {
                    lstError.splice(lstError.indexOf(textarea.attr("id")), 1);
                }
            }
        });
    });
    $("select[required]", areaedit).each(function () {
        if ($(this).val() == "") {
            $(this).addClass("parsley-error");
            $("#error" + $(this).attr("id")).show();
            if (lstError.indexOf($(this).attr("id")) < 0) {
                lstError.push($(this).attr("id"));
            }
        } else {
            $(this).removeClass("parsley-error");
            $("#error" + $(this).attr("id")).hide();
            if (lstError.indexOf($(this).attr("id")) > -1) {
                lstError.splice(lstError.indexOf($(this).attr("id")), 1);
            }
        }
        $(this).change(function () {
            if ($(this).val() == "") {
                $(this).addClass("parsley-error");
                $("#error" + $(this).attr("id")).show();
                if (lstError.indexOf($(this).attr("id")) < 0) {
                    lstError.push($(this).attr("id"));
                }
            } else {
                $(this).removeClass("parsley-error");
                $("#error" + $(this).attr("id")).hide();
                if (lstError.indexOf($(this).attr("id")) > -1) {
                    lstError.splice(lstError.indexOf($(this).attr("id")), 1);
                }
            }
        });
    });
    //#Phần này để ở đây để khi load form nó sẽ validate và bật sự kiện blur cho các element luôn
    InitSEO();


    //window.setTimeout(function () {
    //    var height = $(window).innerHeight() - ($('header').innerHeight()) - ($('footer').innerHeight()) - 30;
    //    console.log(height);
    //    $('#updatePageEditor').css({ height: height });
    //    var heightBlock = height - $('#updatePageBlock .card-header').innerHeight();
    //    $('#updatePageBlock .card-block').css({ height: heightBlock });
    //    $(window).resize(function () {
    //        var height = $(window).innerHeight();
    //        $('#updatePageEditor').css({ height: height });
    //        $('#updatePageBlock').css({ height: height });
    //    });
    //}, 300);
    //$(window).resize(function () {
    //    var divLeft = $('#updatePageEditor');
    //    var divRight = $('#updatePageBlock');
    //    var parent = divLeft.parent();
    //    if (parent.width() < 1300 && divLeft.hasClass('col-xl-6')) {
    //        divLeft.removeClass().addClass('col-xl-12');
    //        divRight.removeClass().addClass('col-xl-12');
    //    }
    //    else if (parent.width() >= 1300 && divLeft.hasClass('col-xl-12')) {
    //        divLeft.removeClass().addClass('col-xl-6');
    //        divRight.removeClass().addClass('col-xl-6');
    //    }
    //});
});

function bindPageVideoDuration(duration) {
    if (duration && duration.length > 0) {
        var splits = duration.split(':');
        $('#videoDurationHour').val(splits[0]);
        $('#videoDurationHour').trigger('change');
        $('#videoDurationMin').val(splits[1]);
        $('#videoDurationMin').trigger('change');
        $('#videoDurationSecond').val(splits[2]);
        $('#videoDurationSecond').trigger('change');
    }
}

function validate() {
    if (lstError.length > 0) {
        for (var i = 0; i < lstError.length; i++) {
            scrollTo($("#error" + lstError[0]));
            break;
        }
        return false;
    } else {
        return true;
    }
}
var allowreload = false;
//Check khi reload trang
function confirmCancel() {
    var conf = confirm('Bạn có chắc muốn hủy?');
    if (conf == false) {
        allowreload = false;
        return false;
    }
    else {
        allowreload = true;
        if (!window.close()) {
            location.href = '/Page/';
        }
    }
}
$(window).bind('beforeunload', function () {
    //save info somewhere
    if (!allowreload) {
        return confirm('Bạn có chắc muốn tải lại trang?');
    }
});
