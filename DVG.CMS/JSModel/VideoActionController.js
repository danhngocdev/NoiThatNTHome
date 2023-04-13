// Create the factory that share the Data
app.factory('Data', function () {
    return { CarInfo: {} };
});

(function () {
    app.controller('VideoActionController', ['$scope', '$rootScope', '$http', 'Service', 'notify', '$ngConfirm', 'Data', function ($scope, $rootScope, $http, Service, notify, $ngConfirm, Data) {
        //function ($scope, $rootScope, $http, Service, notify, Data) {
        $scope.VideoDuration = {
            'Hour': 0,
            'Minute': 0,
            'Second': 0
        };
        $scope.Video = {};

        $scope.CurrentFormEnum = {
            'Edit': 0,
            'VideoRelation': 1
        }
        $scope.CurrentForm = $scope.CurrentFormEnum.Edit;
        $scope.CurrentVideoRelationTabEnum = {
            'Suggest': 0,
            'Search': 1
        }
        $scope.CurrentVideoRelationTab = $scope.CurrentVideoRelationTabEnum.Suggest;

        $scope.SearchVideoRelationModel = {
            'VideoRelateId': '',
            'Keyword': '',
            'VideoType': 1,
            'CateId': '',
            'ListCategory': []
        };

        $scope.VideoSearchVideoRelation = {
            'totalItems': 0,
            'currentVideo': 1,
            'itemsPerVideo': 15
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

        $scope.VideoTypeEnum = {
            "Video": 1, //Tin tức
            "Advices": 2, //Tư vấn
            "CarInfo": 3, //Thông tin xe
            "Assessment": 4, //Đánh giá xe
            "Gallery": 5, //Gallery
            "Pricing": 6, // Giá xe
            "BikePricing": 7, // Giá xe máy
            "BikeAssessment": 8 // Đánh giá xe máy
        }

        $scope.VideoStatus = {
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

        $scope.VideoRelationStatusEnum = {
            "Active": 1, //Đang hiển thị
            "Sticky": 2, //Đang neo top
            "Blocked": 3 //Đang bị block
        }

        $scope.getVideoRelationStatusName = function (status) {
            if (status == $scope.VideoRelationStatusEnum.Active) {
                return "Đang hiển thị";
            }
            else if (status == $scope.VideoRelationStatusEnum.Sticky) {
                return "Đang neo top";
            }
            else if (status == $scope.VideoRelationStatusEnum.Blocked) {
                return "Đang bị khóa";
            }
        }

        $scope.CarInfoSimilarStatusEnum = {
            "Active": 1, //Đang hiển thị
            "Sticky": 2, //Đang neo top
            "Blocked": 3 //Đang bị block
        }

        $scope.ListVideoRelationModels = {
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
                $scope.Video = obj;
                $scope.Video.Id = obj.IdStr;
                CKEDITOR.on("instanceReady", function () {
                    window.setTimeout(function () {
                        CKEDITOR.instances["content"].setData(obj.Description);
                    }, 200);
                });

                // scroll
                $('#updateVideoBlock').niceScroll({ touchbehavior: false, cursoropacitymax: 0.6, cursorwidth: 5 });
            }
            openAsidePanel("sm");
            $('body').addClass("aside-edit-panel-fixed");

        };

        //=====Get list car info=====
        //===========================

        $scope.Video = {
            'totalItems': 0,
            'currentVideo': 1,
            'itemsPerVideo': 10
        };

        //Carinfo model
        $scope.CarInfoModel = {};

        $scope.Search = {};
        $scope.ParamModel = {};
        $scope.IsFromExternal = true;

        $('#modalAddCarInfo').on('shown.bs.modal', function () {
            $scope.IsFromExternal = true;
            $scope.Search.BrandId = $scope.Video.VideoExtend.BrandId;
            $scope.Search.ModelId = $scope.Video.VideoExtend.ModelId;
            $scope.getListDataForControl();
            $scope.getListData();
        });

        //Get list credit card data
        $scope.getListData = function () {
            $scope.Search.VideoIndex = $scope.Video.currentVideo;
            $scope.Search.VideoSize = $scope.Video.itemsPerVideo;
            $scope.Search.VideoIdEncrypt = $scope.Video.EncryptVideoId;
            Service.post("/CarInfo/SearchForVideo", { searchModel: $scope.Search })
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListCarInfo = response.Data;
                        $scope.Video.totalItems = response.TotalRow;
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
            $scope.Video.currentVideo = 1;
            $scope.getListData();
        }

        //Get list car model
        $scope.getListCarModels = function (brandId) {
            Service.post("/CarInfo/GetCarModels", { brandId: brandId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.ParamModel.ListCarModels = response.Data;
                        if ($scope.Search.ModelId == null) {
                            $scope.Search.ModelId = $scope.Video.VideoExtend.ModelId;
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


        //Get car info to Video
        $scope.getCarInfo = function (id) {
            if (id > 0) {
                Service.post("/CarInfo/GetCarInfoUpdate", { carInfoId: id })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.CarInfoModel = response.Data;
                            $scope.updateVideoCarInfo();
                            $("#modalAddCarInfo").modal("hide");
                        }
                    });
            }
        }

        //Update car info in Video
        $scope.updateVideoCarInfo = function () {
            $scope.getListCarModelsForVideo($scope.CarInfoModel.BrandId);
            $scope.getListCarModelDetails($scope.CarInfoModel.BrandId, $scope.CarInfoModel.ModelId);
            $scope.Video.VideoExtend.BrandId = $scope.CarInfoModel.BrandId;
            $scope.Video.VideoExtend.ModelId = $scope.CarInfoModel.ModelId;
            $scope.Video.VideoExtend.CarModelDetailId = $scope.CarInfoModel.ModelDetailId;
            $scope.Video.VideoExtend.CarStyleId = $scope.CarInfoModel.StyleId;
            $scope.Video.VideoExtend.CarSegmentId = $scope.CarInfoModel.SegmentId;
            $scope.Video.VideoExtend.ProductionYear = $scope.CarInfoModel.ProductionYear;
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

        $scope.ChangeStatusVideo = function (statusVideo, Idstr) {
            Service.post("/Video/ChangeStatusVideo", { statusVideo: statusVideo, Idstr: Idstr })
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

        //On change title Video
        $scope.onChangeTitleVideo = function () {
            $scope.Video.AvatarDesc = $scope.Video.Title;
            if ($scope.Video.Title) {
                if (!$scope.Video.StatusOfVideo.IsPublished && !$scope.Video.StatusOfVideo.IsUnPublished) {
                    $scope.Video.OriginalUrl = UnicodeToUnsignAndSlash($scope.Video.Title);
                }
            }
            $scope.Video.SEOTitle = $scope.Video.Title;
        }

        $scope.BlurTitleInput = function () {
            $("#txtOriginalUrl").blur();
        }

        //On change sapo Video
        $scope.onChangeSapoVideo = function () {
            $scope.Video.SEODescription = $scope.Video.Sapo;
        }

        $scope.AutoCheckLinkOriginalUrl = function (url) {
            Service.post("/Video/AutoCheckLinkOriginalUrl", { "url": url })
                .then(function (result) {
                    if (result.Success && result.Data) {

                    }
                });
        }

        $scope.doUpdate = function (currentStatus, status) {
            $scope.Video.Description = CKEDITOR.instances["content"].getData();
            if ($scope.Video.Description.length <= 0) {
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
            //if (!$scope.Video.Avatar || $scope.Video.Avatar.length <= 0) {
            //    notify({
            //        message: "Bạn chưa chọn ảnh đại diện!",
            //        classes: $scope.classes.Error,
            //        templateUrl: $scope.template,
            //        position: $scope.position,
            //        duration: $scope.duration
            //    });
            //    return false;
            //}
            //if ($scope.Video.DisplayStyleOnList == $scope.DisplayStyleOnListEnum.Cover && (!$scope.Video.Avatar2 || $scope.Video.Avatar2.length <= 0)) {
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
                $scope.Video.ListMenuHeading = [];
                $(listElemH).each(function () {
                    var menuHeading = new Object();
                    menuHeading.NameExtend = $(this).html();
                    menuHeading.MenuId = $(this).attr("dataId");
                    menuHeading.Ordinal = $(this).attr("dataOrdinal");
                    $scope.Video.ListMenuHeading.push(menuHeading);
                });
            }
            // processing video in content
            //var hour = parseInt($scope.VideoDuration.Hour);
            //var min = parseInt($scope.VideoDuration.Minute);
            //var sec = parseInt($scope.VideoDuration.Second);
            //if (hour > 0 || min > 0 || sec > 0) {
            //    strDuration = (hour > 9 ? hour : "0" + hour) + ":" + (min > 9 ? min : "0" + min) + ":" + (sec > 9 ? sec : "0" + sec);
            //    $scope.Video.VideoDuration = strDuration;
            //}

            var lstObjVideo = $(editor.document.$).find(".dvsvideo");
            if (lstObjVideo.length > 0) {
                // nếu có video trong bài thì không cho chọn AMP;
                $(lstObjVideo).each(function () {
                    $scope.Video.Description = $scope.Video.Description.replace(/data-raw="([^"]+)"/, "");
                    return;
                });
            }

            // xử lý xóa ký tự trống ở title box nhúng nội dung
            $scope.Video.Description = $scope.Video.Description.replace('<p class="dvs-textbox-title">&nbsp;</p>', '<p class="dvs-textbox-title"></p>');

            //$scope.Video.PublishedDateStr = $('#txtPublishedDate').val();
            Service.post("/Video/UpdateVideo", { Video: $scope.Video, currentStatus: currentStatus })
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
                        $scope.Video.Status = currentStatus;
                    }
                });
        }

        var doAutoSave = function () {
            $scope.Video.Description = CKEDITOR.instances["content"].getData();
            var data = $scope.Video.Description.replace(/<[^>]+>/g, '\n').replace(/\n\s*\n/g, '\n\n').trim();
            if (data.length <= 0) {
                return;
            }
            $scope.Video.DistributionDateStr = $("#txtDistributionDate").val();
            $scope.Video.PricingDateStr = $("#txtPricingDate").val();
            $scope.Video.ListTags = $("#txtAddTags").tagsinput("items");
            $scope.Video.TagItem = "";
            $scope.Video.CarInfo = $scope.CarInfoModel;

            if (!$scope.Video.IsReference) {
                $scope.Video.ReferenceUrl = null;
            }
            var len = $scope.Video.ListTags.length;
            for (var i = 0; i < len; i++) {
                $scope.Video.TagItem = $scope.Video.TagItem + $scope.Video.ListTags[i].Name + ";";
            }
            if ($scope.Video.ListVideoRelation != null && $scope.Video.ListVideoRelation != undefined) {
                for (var i = 0; i < $scope.Video.ListVideoRelation.length; i++) {
                    if ($scope.Video.ListVideoRelation[i].IsSticky) {
                        $scope.Video.ListVideoRelation[i].StickyFromDate = $("[name='txtRelaStickyFromDateRelation" + i + "']").val();
                        $scope.Video.ListVideoRelation[i].StickyUntilDate = $("[name='txtRelaStickyUntilDateRelation" + i + "']").val();
                    }
                }
            }

            //Process for menu heading
            var editor = CKEDITOR.instances['content'];
            var listElemH = $(editor.document.$).find(".dvs_menuheading_item");
            if (listElemH.length > 0) {
                $scope.Video.ListMenuHeading = [];
                $(listElemH).each(function () {
                    var menuHeading = new Object();
                    menuHeading.NameExtend = $(this).html();
                    menuHeading.MenuId = $(this).attr("dataId");
                    $scope.Video.ListMenuHeading.push(menuHeading);
                });
            }

            // processing video in content
            var hour = parseInt($scope.VideoDuration.Hour);
            var min = parseInt($scope.VideoDuration.Minute);
            var sec = parseInt($scope.VideoDuration.Second);
            if (hour > 0 || min > 0 || sec > 0) {
                strDuration = (hour > 9 ? hour : "0" + hour) + ":" + (min > 9 ? min : "0" + min) + ":" + (sec > 9 ? sec : "0" + sec);
                $scope.Video.VideoDuration = strDuration;
            }

            var lstObjVideo = $(editor.document.$).find(".dvsvideo");
            if (lstObjVideo.length > 0) {
                $(lstObjVideo).each(function () {
                    var dataRaw = $(this).attr("data-raw");
                    if (dataRaw && dataRaw.length > 0)
                        $scope.Video.VideoExtend.VideoUrl = $(this).attr("data-raw");
                    $scope.Video.VideoExtend.VideoThumb = $(this).attr("data-raw-image");
                    if (!$scope.Video.VideoDuration || $scope.Video.VideoDuration == null) $scope.Video.VideoDuration = $(this).attr("data-duration");
                    $scope.Video.Description = $scope.Video.Description.replace(/data-raw="([^"]+)"/, "");
                    return;
                });
            }
            else {
                $scope.Video.VideoExtend.VideoUrl = null;
                $scope.Video.VideoExtend.VideoThumb = null;
                $scope.Video.VideoDuration = null;
            }

            Service.post("/Video/AutoSave", { Video: $scope.Video })
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

        //Save Video
        $scope.doSave = function () {
            ngConfirm($scope.doUpdate, 0, 0,cms.message.confirmSave);
        }

        //Publish Video
        $scope.doPublish = function () {
            ngConfirm($scope.doUpdate, $scope.Video.Status, $scope.VideoStatus.Published, cms.message.confirmDoPublish);
        }

        //Gỡ bài đã publish
        $scope.doUnPublish = function () {
            ngConfirm($scope.doUpdate, $scope.Video.Status, $scope.VideoStatus.UnPublished, cms.message.confirmDoUnPublish);
        }

        //Gửi bài lên chờ duyệt
        $scope.doPendingApprove = function () {
            ngConfirm($scope.doUpdate, $scope.Video.Status, $scope.VideoStatus.PendingApproved, cms.message.confirmDoPendingApprove);
        }

        // xóa bài
        $scope.doDelete = function () {
            ngConfirm($scope.doUpdate, $scope.Video.Status, $scope.VideoStatus.Deleted, cms.message.confirmDoDelete);
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

        //Add Video tags
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
                        $scope.Video.Avatar = result.path;
                        $scope.Video.AvatarStr = result.path;
                    });
                }
                else if (index == "avatar2") {
                    $scope.$apply(function () {
                        $scope.Video.Avatar2 = result.path;
                        $scope.Video.AvatarStr2 = result.path;
                    });
                }
                else {
                    $scope.$apply(function () {
                        $scope.Video.ListImage[index].ImageUrl = result.path;
                        $scope.Video.ListImage[index].ImageUrlCrop = result.path;
                    });
                }
                w.close();
            }
        }
        //Xóa ảnh avatar hoặc ảnh slide
        $scope.delImages = function (index) {
            if (index == "avatar1") {
                $scope.Video.Avatar = "";
                $scope.Video.AvatarStr = cms.configs.NoImage;
            }
            else if (index == "avatar2") {
                $scope.Video.Avatar2 = "";
                $scope.Video.AvatarStr2 = cms.configs.NoImage;
            }
            else {
                $scope.Video.ListImage.splice(index, 1);
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
                        if ($scope.Video.ListImage == null) {
                            $scope.Video.ListImage = [];
                        }
                        $scope.Video.ListImage.push(imageadd);
                    });
                }
                w.close();
            }
        }
        $scope.OldVideoType = "";
        $scope.ChangeVideoType = function () {
            if (!confirm('Nếu đổi loại tin, một vài thông tin khác có thể sẽ bị thay đổi (VD: giá xe, đánh giá xe,...). Bạn có chắc chắn đổi không?')) {
                $scope.Video.VideoType = $scope.OldVideoType;
            }
            $scope.SelectInit();
            $scope.OldVideoType = $scope.Video.VideoType;
            if ($scope.Video.VideoType == $scope.VideoTypeEnum.Gallery) {
                $scope.Video.DisplayStyle = 2; //Mặc định là bài ảnh
            }

            // reset VideoExtend
            $scope.Video.VideoExtend.BrandId = 0;
            $scope.Video.VideoExtend.ModelId = 0;
            $scope.Video.VideoExtend.CarModelDetailId = 0;
            $scope.Video.VideoExtend.CarStyleId = 0;
            $scope.Video.VideoExtend.CarSegmentId = 0;
            $scope.Video.VideoExtend.ProductionYear = "";

            $scope.Video.TypeOfVideo.IsAssessment = false;
            $scope.Video.TypeOfVideo.IsBikeAssessment = false;
            $scope.Video.TypeOfVideo.IsPricing = false;
            $scope.Video.TypeOfVideo.IsBikePricing = false;
            $scope.Video.TypeOfVideo.IsCarInfo = false;
            switch ($scope.Video.VideoType) {
                case $scope.VideoTypeEnum.CarInfo:
                    {
                        $scope.Video.TypeOfVideo.IsCarInfo = true;
                        break;
                    }
                case $scope.VideoTypeEnum.Assessment:
                    {
                        $scope.Video.TypeOfVideo.IsAssessment = true;
                        break;
                    }
                case $scope.VideoTypeEnum.BikeAssessment:
                    {
                        $scope.Video.TypeOfVideo.IsBikeAssessment = true;
                        break;
                    }
                case $scope.VideoTypeEnum.Pricing:
                    {
                        $scope.Video.TypeOfVideo.IsPricing = true;
                        break;
                    }
                case $scope.VideoTypeEnum.BikePricing:
                    {
                        $scope.Video.TypeOfVideo.IsBikePricing = true;
                        break;
                    }
            }
        }

        $scope.CateChange = function () {
            if ($scope.Video.CateId == null) {
                $scope.Video.CateId = 0;
            }
        }

        $scope.GetTopic = function () {
            Service.post("/Video/GetTopic")
                .then(function (response) {
                    if (response.Success) {
                        $scope.Video.ListTopic = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        //Get list car model
        $scope.getListCarModelsForVideo = function (brandId) {
            Service.post("/Video/GetCarModelByCarBrandId", { brandId: brandId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.Video.ListCarModel = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        //Get list bike model
        $scope.getListBikeModelsForVideo = function (brandId) {
            Service.post("/Video/GetBikeModelByBikeBrandId", { brandId: brandId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.Video.ListBikeModel = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        //Get list car model details
        $scope.getListCarModelDetails = function (brandId, modelId) {
            Service.post("/CarInfo/GetCarModelDetails", { brandId: brandId, modelId: modelId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.Video.ListCarModelDetail = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        $scope.BrandChange = function () {
            if ($scope.Video.VideoExtend.BrandId == null) {
                $scope.Video.VideoExtend.BrandId = 0;
            }
            if ($scope.Video.TypeOfVideo.IsAssessment || $scope.Video.TypeOfVideo.IsPricing || $scope.Video.TypeOfVideo.IsCarInfo)
                $scope.getListCarModelsForVideo($scope.Video.VideoExtend.BrandId);
            else if ($scope.Video.TypeOfVideo.IsBikeAssessment || $scope.Video.TypeOfVideo.IsBikePricing)
                $scope.getListBikeModelsForVideo($scope.Video.VideoExtend.BrandId);

        }

        $scope.ModelChange = function () {
            if ($scope.Video.VideoExtend.ModelId == null) {
                $scope.Video.VideoExtend.ModelId = 0;
            }
            if ($scope.Video.TypeOfVideo.IsAssessment || $scope.Video.TypeOfVideo.IsPricing || $scope.Video.TypeOfVideo.IsCarInfo)
                $scope.getListCarModelDetails($scope.Video.VideoExtend.BrandId, $scope.Video.VideoExtend.ModelId);
        }

        $scope.showVideoRelation = function () {
            closeAsidePanel("sm");
            $scope.CurrentForm = $scope.CurrentFormEnum.VideoRelation;
            $scope.getSuggestionList();
            $scope.ListVideoSearched = [];
            $scope.SearchVideoRelationModel.VideoType = $scope.Video.VideoType;
            $scope.SearchVideoRelationModel.DisplayStyle = $scope.Video.DisplayStyle;
            $scope.SearchVideoRelationModel.CateId = $scope.Video.CateId;
            //$("#divVideorela").show();
            //$("#tab1").trigger("click");
            $scope.getSearchInfo();
            scrollTo($("html"));
        }

        $scope.backtoedit = function () {
            openAsidePanel("sm");
            $("#divVideorela").hide();
            scrollTo($("#btnshowrela"));

        }

        $scope.getSuggestionList = function () {
            $scope.ListVideoSuggested = [];
            Service.post("/Video/GetListSuggestionVideo",
                {
                    ModelId: $scope.Video.VideoExtend.ModelId,
                    BrandId: $scope.Video.VideoExtend.BrandId,
                    CarSegmentId: $scope.Video.VideoExtend.CarSegmentId,
                    CarStyleId: $scope.Video.VideoExtend.CarStyleId,
                    VideoType: $scope.Video.VideoType,
                    CateId: $scope.Video.CateId,
                    CurrentVideoId: $scope.Video.Id,
                    ListTags: $("#txtAddTags").tagsinput("items"),
                    DisplayStyle: $scope.Video.DisplayStyle
                }
            )
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListVideoRelationModels.lists.Source = $scope.ListVideoSuggested = response.Data;
                    }
                });
        }

        $scope.getSearchInfo = function () {
            $scope.SearchVideoRelationModel.ListCategory = [];
            for (var i = 0; i < $scope.Video.ListCategory.length; i++) {
                if ($scope.Video.ListCategory[i].Type == $scope.SearchVideoRelationModel.VideoType) {
                    $scope.SearchVideoRelationModel.ListCategory.push($scope.Video.ListCategory[i]);
                }
            }
            $scope.SelectInit();
        }

        $scope.searchNewRelation = function () {
            $scope.VideoSearchVideoRelation.currentVideo = 1;
            $scope.getListNewRelation();
        }

        $scope.getListNewRelation = function () {
            Service.post("/Video/SearchVideo",
                {
                    CurrentVideoId: $scope.Video.Id,
                    VideoType: $scope.SearchVideoRelationModel.VideoType,
                    DisplayStyle: $scope.SearchVideoRelationModel.DisplayStyle,
                    VideoRelateId: $scope.SearchVideoRelationModel.VideoRelateId,
                    Keyword: $scope.SearchVideoRelationModel.Keyword,
                    CateId: $scope.SearchVideoRelationModel.CateId,
                    VideoIndex: $scope.VideoSearchVideoRelation.currentVideo,
                    VideoSize: $scope.VideoSearchVideoRelation.itemsPerVideo
                }
            )
                .then(function (response) {
                    if (response.Success) {
                        $scope.VideoSearchVideoRelation.totalItems = response.Data.TotalRows;
                        $scope.ListVideoSearched = response.Data.ListVideoSearched;
                        $scope.ListVideoRelationModels.lists.Source = $scope.ListVideoSearched;
                    }
                });
        }

        $scope.VideoRelationChangeTab = function (tab) {
            $scope.CurrentVideoRelationTab = tab;
            if (tab == $scope.CurrentVideoRelationTabEnum.Search) {
                $scope.searchNewRelation();
            } else if (tab == $scope.CurrentVideoRelationTabEnum.Suggest) {
                $scope.ListVideoRelationModels.lists.Source = $scope.ListVideoSuggested;
            }
        }

        $scope.clickStickyVideoRelation = function (index, oldStatus) {
            $scope.Video.ListVideoRelation[index].IsSticky = !$scope.Video.ListVideoRelation[index].IsSticky;
            if ($scope.Video.ListVideoRelation[index].IsSticky) {
                $scope.Video.ListVideoRelation[index].Status = $scope.VideoRelationStatusEnum.Sticky;
            }
            else {
                if (oldStatus == $scope.VideoRelationStatusEnum.Sticky)
                    $scope.Video.ListVideoRelation[index].Status = $scope.VideoRelationStatusEnum.Active;
                else
                    $scope.Video.ListVideoRelation[index].Status = oldStatus;
            }
        }

        $scope.dropCallback = function (listName, item) {
            if (listName == 'Target') {
                // xóa bài trùng khi kéo sang
                var mappedList = $scope.ListVideoRelationModels.lists.Target.map(function (e) {
                    return e.VideoRelateIdStr;
                });
                var index = mappedList.indexOf(item.VideoRelateIdStr);

                if (index >= 0) {
                    $scope.ListVideoRelationModels.lists.Target.splice(index, 1);
                }

                $scope.DatePickerInit();
                return item;
            } else if (listName == 'Source') {

                // xóa bài trùng khi kéo trả
                var mappedList = $scope.ListVideoRelationModels.lists.Source.map(function (e) {
                    return e.VideoRelateIdStr;
                });
                var index = mappedList.indexOf(item.VideoRelateIdStr);
                if (index >= 0) {
                    $scope.ListVideoRelationModels.lists.Source.splice(index, 1);
                }
                return item;
            }
        }

        $scope.sendItem = function (item) {
            var mappedList = $scope.ListVideoRelationModels.lists.Source.map(function (e) {
                return e.VideoRelateIdStr;
            });
            var index = mappedList.indexOf(item.VideoRelateIdStr);
            if (index >= 0) {

                // nếu list targer có bài rồi thì thôi
                var mappedListTarget = $scope.ListVideoRelationModels.lists.Target.map(function (e) {
                    return e.VideoRelateIdStr;
                });
                var indexTarget = mappedListTarget.indexOf(item.VideoRelateIdStr);
                if (indexTarget < 0) {
                    $scope.ListVideoRelationModels.lists.Target.push(item);
                }

                $scope.ListVideoRelationModels.lists.Source.splice(index, 1);
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
            var mappedList = $scope.ListVideoRelationModels.lists.Source.map(function (e) {
                return e.VideoRelateIdStr;
            });
            var index = mappedList.indexOf(item.VideoRelateIdStr);

            $scope.ListVideoRelationModels.lists.Target.splice(i, 1);
            if (index >= 0) {
                $scope.ListVideoRelationModels.lists.Source.splice(index, 1);
            }
            $scope.ListVideoRelationModels.lists.Source.push(item);
        }

        $scope.moveItemTarget = function (index, param) {
            if (index + param >= 0 && index + param < $scope.ListVideoRelationModels.lists.Target.length) {
                var arr = $scope.ListVideoRelationModels.lists.Target;
                arr = array_move(arr, index, index + param);
                $scope.ListVideoRelationModels.lists.Target = arr;
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

        $scope.ChangeRelaVideoType = function () {
            $scope.SearchVideoRelationModel.ListCategory = [];
            for (var i = 0; i < $scope.Video.ListCategory.length; i++) {
                if ($scope.Video.ListCategory[i].Type == $scope.SearchVideoRelationModel.VideoType) {
                    $scope.SearchVideoRelationModel.ListCategory.push($scope.Video.ListCategory[i]);
                }
            }
            $scope.SelectInit();
        }

        $scope.getVideoTypeName = function (VideoType) {
            return getVideoTypeName(VideoType);
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

        $scope.roundNumberPaging = function (itemPerVideo, totalItem) {
            return Math.ceil(parseFloat(totalItem) / itemPerVideo);
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
    //    $('#updateVideoEditor').css({ height: height });
    //    var heightBlock = height - $('#updateVideoBlock .card-header').innerHeight();
    //    $('#updateVideoBlock .card-block').css({ height: heightBlock });
    //    $(window).resize(function () {
    //        var height = $(window).innerHeight();
    //        $('#updateVideoEditor').css({ height: height });
    //        $('#updateVideoBlock').css({ height: height });
    //    });
    //}, 300);
    //$(window).resize(function () {
    //    var divLeft = $('#updateVideoEditor');
    //    var divRight = $('#updateVideoBlock');
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

function bindVideoVideoDuration(duration) {
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
            location.href = '/Video/';
        }
    }
}
$(window).bind('beforeunload', function () {
    //save info somewhere
    if (!allowreload) {
        return confirm('Bạn có chắc muốn tải lại trang?');
    }
});
