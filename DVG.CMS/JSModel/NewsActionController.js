// Create the factory that share the Data
app.factory('Data', function () {
    return { CarInfo: {} };
});

(function () {
    app.controller('NewsActionController', ['$scope', '$rootScope', '$http', 'Service', 'notify', '$ngConfirm', 'Data', function ($scope, $rootScope, $http, Service, notify, $ngConfirm, Data) {
        //function ($scope, $rootScope, $http, Service, notify, Data) {
        $scope.VideoDuration = {
            'Hour': 0,
            'Minute': 0,
            'Second': 0
        };
        $scope.News = {};

        $scope.CurrentFormEnum = {
            'Edit': 0,
            'NewsRelation': 1
        }
        $scope.CurrentForm = $scope.CurrentFormEnum.Edit;
        $scope.CurrentNewsRelationTabEnum = {
            'Suggest': 0,
            'Search': 1
        }
        $scope.CurrentNewsRelationTab = $scope.CurrentNewsRelationTabEnum.Suggest;

        $scope.SearchNewsRelationModel = {
            'NewsRelateId': '',
            'Keyword': '',
            'NewsType': 1,
            'CateId': '',
            'ListCategory': []
        };

        $scope.pageSearchNewsRelation = {
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

        $scope.NewsTypeEnum = {
            "News": 1, //Tin tức
            "Advices": 2, //Tư vấn
            "CarInfo": 3, //Thông tin xe
            "Assessment": 4, //Đánh giá xe
            "Gallery": 5, //Gallery
            "Pricing": 6, // Giá xe
            "BikePricing": 7, // Giá xe máy
            "BikeAssessment": 8 // Đánh giá xe máy
        }

        $scope.NewsStatus = {
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

        $scope.NewsRelationStatusEnum = {
            "Active": 1, //Đang hiển thị
            "Sticky": 2, //Đang neo top
            "Blocked": 3 //Đang bị block
        }

        $scope.getNewsRelationStatusName = function (status) {
            if (status == $scope.NewsRelationStatusEnum.Active) {
                return "Đang hiển thị";
            }
            else if (status == $scope.NewsRelationStatusEnum.Sticky) {
                return "Đang neo top";
            }
            else if (status == $scope.NewsRelationStatusEnum.Blocked) {
                return "Đang bị khóa";
            }
        }

        $scope.CarInfoSimilarStatusEnum = {
            "Active": 1, //Đang hiển thị
            "Sticky": 2, //Đang neo top
            "Blocked": 3 //Đang bị block
        }

        $scope.ListNewsRelationModels = {
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
           
        ];

        $scope.HighLightEnum = [
            { "Id": 0, "Name": "Không kích hoạt" },
            { "Id": 1, "Name": "Kích hoạt" }
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
                $scope.News = obj;
                $scope.News.Id = obj.IdStr;

                $scope.News.CategoryId = obj.CategoryId;
                $scope.getListCategory();
                CKEDITOR.on("instanceReady", function () {
                    window.setTimeout(function () {
                        CKEDITOR.instances["content"].setData(obj.Description);
                    }, 200);
                });

                // scroll
                $('#updateNewsBlock').niceScroll({ touchbehavior: false, cursoropacitymax: 0.6, cursorwidth: 5 });
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
            $scope.Search.BrandId = $scope.News.NewsExtend.BrandId;
            $scope.Search.ModelId = $scope.News.NewsExtend.ModelId;
            $scope.getListDataForControl();
            $scope.getListData();
        });



        $scope.getListCategory = function () {
            var newsType = 2;
            Service.post("/Category/GetListCategoryByType", { newsType: newsType })
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListCategoryEnum = response.Data;
                    }
                });
        }


        //Get list credit card data
        $scope.getListData = function () {
            $scope.Search.PageIndex = $scope.page.currentPage;
            $scope.Search.PageSize = $scope.page.itemsPerPage;
            $scope.Search.NewsIdEncrypt = $scope.News.EncryptNewsId;
            Service.post("/CarInfo/SearchForNews", { searchModel: $scope.Search })
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
                            $scope.Search.ModelId = $scope.News.NewsExtend.ModelId;
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


        //Get car info to news
        $scope.getCarInfo = function (id) {
            if (id > 0) {
                Service.post("/CarInfo/GetCarInfoUpdate", { carInfoId: id })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.CarInfoModel = response.Data;
                            $scope.updateNewsCarInfo();
                            $("#modalAddCarInfo").modal("hide");
                        }
                    });
            }
        }

        //Update car info in news
        $scope.updateNewsCarInfo = function () {
            $scope.getListCarModelsForNews($scope.CarInfoModel.BrandId);
            $scope.getListCarModelDetails($scope.CarInfoModel.BrandId, $scope.CarInfoModel.ModelId);
            $scope.News.NewsExtend.BrandId = $scope.CarInfoModel.BrandId;
            $scope.News.NewsExtend.ModelId = $scope.CarInfoModel.ModelId;
            $scope.News.NewsExtend.CarModelDetailId = $scope.CarInfoModel.ModelDetailId;
            $scope.News.NewsExtend.CarStyleId = $scope.CarInfoModel.StyleId;
            $scope.News.NewsExtend.CarSegmentId = $scope.CarInfoModel.SegmentId;
            $scope.News.NewsExtend.ProductionYear = $scope.CarInfoModel.ProductionYear;
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

        $scope.ChangeStatusNews = function (statusNews, Idstr) {
            Service.post("/News/ChangeStatusNews", { statusNews: statusNews, Idstr: Idstr })
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

        //On change title news
        $scope.onChangeTitleNews = function () {
            $scope.News.AvatarDesc = $scope.News.Title;
            if ($scope.News.Title) {
                if (!$scope.News.StatusOfNews.IsPublished && !$scope.News.StatusOfNews.IsUnPublished) {
                    $scope.News.OriginalUrl = UnicodeToUnsignAndSlash($scope.News.Title);
                }
            }
            $scope.News.SEOTitle = $scope.News.Title;
        }

        $scope.BlurTitleInput = function () {
            $("#txtOriginalUrl").blur();
        }

        //On change sapo news
        $scope.onChangeSapoNews = function () {
            $scope.News.SEODescription = $scope.News.Sapo;
        }

        $scope.AutoCheckLinkOriginalUrl = function (url) {
            Service.post("/News/AutoCheckLinkOriginalUrl", { "url": url })
                .then(function (result) {
                    if (result.Success && result.Data) {

                    }
                });
        }

        $scope.doUpdate = function (currentStatus, status) {
            $scope.News.Description = CKEDITOR.instances["content"].getData();
            $scope.News.Status = status;
            $scope.News.DistributionDateStr = $("#txtDistributionDate").val();
            $scope.News.PricingDateStr = $("#txtPricingDate").val();
            $scope.News.ListTags = $("#txtAddTags").tagsinput("items");
            $scope.News.TagItem = "";
            $scope.News.CarInfo = $scope.CarInfoModel;
            if ($scope.News.Description.length <= 0) {
                notify({
                    message: "Vui lòng nhập nội dung bài viết!",
                    classes: $scope.classes.Error,
                    templateUrl: $scope.template,
                    position: $scope.position,
                    duration: $scope.duration
                });
                return false;
            }
            if ($scope.News.CategoryId <= 0) {
                notify({
                    message: "Vui lòng chọn chuyên mục!",
                    classes: $scope.classes.Error,
                    templateUrl: $scope.template,
                    position: $scope.position,
                    duration: $scope.duration
                });
                return false;
            }

            // bài ảnh lớn buộc phải nhập avatar2, còn lại all phải nhập avatar
            if (!$scope.News.Avatar || $scope.News.Avatar.length <= 0) {
                notify({
                    message: "Bạn chưa chọn ảnh đại diện!",
                    classes: $scope.classes.Error,
                    templateUrl: $scope.template,
                    position: $scope.position,
                    duration: $scope.duration
                });
                return false;
            }
            //if ($scope.News.DisplayStyleOnList == $scope.DisplayStyleOnListEnum.Cover && (!$scope.News.Avatar2 || $scope.News.Avatar2.length <= 0)) {
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
                $scope.News.ListMenuHeading = [];
                $(listElemH).each(function () {
                    var menuHeading = new Object();
                    menuHeading.NameExtend = $(this).html();
                    menuHeading.MenuId = $(this).attr("dataId");
                    menuHeading.Ordinal = $(this).attr("dataOrdinal");
                    $scope.News.ListMenuHeading.push(menuHeading);
                });
            }
            // processing video in content
            //var hour = parseInt($scope.VideoDuration.Hour);
            //var min = parseInt($scope.VideoDuration.Minute);
            //var sec = parseInt($scope.VideoDuration.Second);
            //if (hour > 0 || min > 0 || sec > 0) {
            //    strDuration = (hour > 9 ? hour : "0" + hour) + ":" + (min > 9 ? min : "0" + min) + ":" + (sec > 9 ? sec : "0" + sec);
            //    $scope.News.VideoDuration = strDuration;
            //}

            var lstObjVideo = $(editor.document.$).find(".dvsvideo");
            if (lstObjVideo.length > 0) {
                // nếu có video trong bài thì không cho chọn AMP;
                $(lstObjVideo).each(function () {
                    $scope.News.Description = $scope.News.Description.replace(/data-raw="([^"]+)"/, "");
                    return;
                });
            }

            // xử lý xóa ký tự trống ở title box nhúng nội dung
            $scope.News.Description = $scope.News.Description.replace('<p class="dvs-textbox-title">&nbsp;</p>', '<p class="dvs-textbox-title"></p>');

            $scope.News.PublishedDateStr = $('#txtPublishedDate').val();
            Service.post("/News/UpdateNews", { news: $scope.News, currentStatus: currentStatus })
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
                        $scope.News.Status = currentStatus;
                    }
                });
        }

        var doAutoSave = function () {
            $scope.News.Description = CKEDITOR.instances["content"].getData();
            var data = $scope.News.Description.replace(/<[^>]+>/g, '\n').replace(/\n\s*\n/g, '\n\n').trim();
            if (data.length <= 0) {
                return;
            }
            $scope.News.DistributionDateStr = $("#txtDistributionDate").val();
            $scope.News.PricingDateStr = $("#txtPricingDate").val();
            $scope.News.ListTags = $("#txtAddTags").tagsinput("items");
            $scope.News.TagItem = "";
            $scope.News.CarInfo = $scope.CarInfoModel;

            if (!$scope.News.IsReference) {
                $scope.News.ReferenceUrl = null;
            }
            var len = $scope.News.ListTags.length;
            for (var i = 0; i < len; i++) {
                $scope.News.TagItem = $scope.News.TagItem + $scope.News.ListTags[i].Name + ";";
            }
            if ($scope.News.ListNewsRelation != null && $scope.News.ListNewsRelation != undefined) {
                for (var i = 0; i < $scope.News.ListNewsRelation.length; i++) {
                    if ($scope.News.ListNewsRelation[i].IsSticky) {
                        $scope.News.ListNewsRelation[i].StickyFromDate = $("[name='txtRelaStickyFromDateRelation" + i + "']").val();
                        $scope.News.ListNewsRelation[i].StickyUntilDate = $("[name='txtRelaStickyUntilDateRelation" + i + "']").val();
                    }
                }
            }

            //Process for menu heading
            var editor = CKEDITOR.instances['content'];
            var listElemH = $(editor.document.$).find(".dvs_menuheading_item");
            if (listElemH.length > 0) {
                $scope.News.ListMenuHeading = [];
                $(listElemH).each(function () {
                    var menuHeading = new Object();
                    menuHeading.NameExtend = $(this).html();
                    menuHeading.MenuId = $(this).attr("dataId");
                    $scope.News.ListMenuHeading.push(menuHeading);
                });
            }

            // processing video in content
            var hour = parseInt($scope.VideoDuration.Hour);
            var min = parseInt($scope.VideoDuration.Minute);
            var sec = parseInt($scope.VideoDuration.Second);
            if (hour > 0 || min > 0 || sec > 0) {
                strDuration = (hour > 9 ? hour : "0" + hour) + ":" + (min > 9 ? min : "0" + min) + ":" + (sec > 9 ? sec : "0" + sec);
                $scope.News.VideoDuration = strDuration;
            }

            var lstObjVideo = $(editor.document.$).find(".dvsvideo");
            if (lstObjVideo.length > 0) {
                $(lstObjVideo).each(function () {
                    var dataRaw = $(this).attr("data-raw");
                    if (dataRaw && dataRaw.length > 0)
                        $scope.News.NewsExtend.VideoUrl = $(this).attr("data-raw");
                    $scope.News.NewsExtend.VideoThumb = $(this).attr("data-raw-image");
                    if (!$scope.News.VideoDuration || $scope.News.VideoDuration == null) $scope.News.VideoDuration = $(this).attr("data-duration");
                    $scope.News.Description = $scope.News.Description.replace(/data-raw="([^"]+)"/, "");
                    return;
                });
            }
            else {
                $scope.News.NewsExtend.VideoUrl = null;
                $scope.News.NewsExtend.VideoThumb = null;
                $scope.News.VideoDuration = null;
            }

            Service.post("/News/AutoSave", { news: $scope.News })
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

        //Save news
        $scope.doSave = function () {
            ngConfirm($scope.doUpdate, $scope.News.Status, $scope.News.Status, cms.message.confirmSave);
        }

        //Publish news
        $scope.doPublish = function () {
            ngConfirm($scope.doUpdate, $scope.News.Status, $scope.NewsStatus.Published, cms.message.confirmDoPublish);
        }

        //Gỡ bài đã publish
        $scope.doUnPublish = function () {
            ngConfirm($scope.doUpdate, $scope.News.Status, $scope.NewsStatus.UnPublished, cms.message.confirmDoUnPublish);
        }

        //Gửi bài lên chờ duyệt
        $scope.doPendingApprove = function () {
            ngConfirm($scope.doUpdate, $scope.News.Status, $scope.NewsStatus.PendingApproved, cms.message.confirmDoPendingApprove);
        }

        // xóa bài
        $scope.doDelete = function () {
            ngConfirm($scope.doUpdate, $scope.News.Status, $scope.NewsStatus.Deleted, cms.message.confirmDoDelete);
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

        //Add news tags
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
                        $scope.News.Avatar = result.path;
                        $scope.News.AvatarStr = result.path;
                    });
                }
                else if (index == "avatar2") {
                    $scope.$apply(function () {
                        $scope.News.Avatar2 = result.path;
                        $scope.News.AvatarStr2 = result.path;
                    });
                }
                else {
                    $scope.$apply(function () {
                        $scope.News.ListImage[index].ImageUrl = result.path;
                        $scope.News.ListImage[index].ImageUrlCrop = result.path;
                    });
                }
                w.close();
            }
        }
        //Xóa ảnh avatar hoặc ảnh slide
        $scope.delImages = function (index) {
            if (index == "avatar1") {
                $scope.News.Avatar = "";
                $scope.News.AvatarStr = cms.configs.NoImage;
            }
            else if (index == "avatar2") {
                $scope.News.Avatar2 = "";
                $scope.News.AvatarStr2 = cms.configs.NoImage;
            }
            else {
                $scope.News.ListImage.splice(index, 1);
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
                        if ($scope.News.ListImage == null) {
                            $scope.News.ListImage = [];
                        }
                        $scope.News.ListImage.push(imageadd);
                    });
                }
                w.close();
            }
        }
        $scope.OldNewsType = "";
        $scope.ChangeNewsType = function () {
            if (!confirm('Nếu đổi loại tin, một vài thông tin khác có thể sẽ bị thay đổi (VD: giá xe, đánh giá xe,...). Bạn có chắc chắn đổi không?')) {
                $scope.News.NewsType = $scope.OldNewsType;
            }
            $scope.SelectInit();
            $scope.OldNewsType = $scope.News.NewsType;
            if ($scope.News.NewsType == $scope.NewsTypeEnum.Gallery) {
                $scope.News.DisplayStyle = 2; //Mặc định là bài ảnh
            }

            // reset NewsExtend
            $scope.News.NewsExtend.BrandId = 0;
            $scope.News.NewsExtend.ModelId = 0;
            $scope.News.NewsExtend.CarModelDetailId = 0;
            $scope.News.NewsExtend.CarStyleId = 0;
            $scope.News.NewsExtend.CarSegmentId = 0;
            $scope.News.NewsExtend.ProductionYear = "";

            $scope.News.TypeOfNews.IsAssessment = false;
            $scope.News.TypeOfNews.IsBikeAssessment = false;
            $scope.News.TypeOfNews.IsPricing = false;
            $scope.News.TypeOfNews.IsBikePricing = false;
            $scope.News.TypeOfNews.IsCarInfo = false;
            switch ($scope.News.NewsType) {
                case $scope.NewsTypeEnum.CarInfo:
                    {
                        $scope.News.TypeOfNews.IsCarInfo = true;
                        break;
                    }
                case $scope.NewsTypeEnum.Assessment:
                    {
                        $scope.News.TypeOfNews.IsAssessment = true;
                        break;
                    }
                case $scope.NewsTypeEnum.BikeAssessment:
                    {
                        $scope.News.TypeOfNews.IsBikeAssessment = true;
                        break;
                    }
                case $scope.NewsTypeEnum.Pricing:
                    {
                        $scope.News.TypeOfNews.IsPricing = true;
                        break;
                    }
                case $scope.NewsTypeEnum.BikePricing:
                    {
                        $scope.News.TypeOfNews.IsBikePricing = true;
                        break;
                    }
            }
        }

        $scope.CateChange = function () {
            if ($scope.News.CateId == null) {
                $scope.News.CateId = 0;
            }
        }

        $scope.GetTopic = function () {
            Service.post("/News/GetTopic")
                .then(function (response) {
                    if (response.Success) {
                        $scope.News.ListTopic = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        //Get list car model
        $scope.getListCarModelsForNews = function (brandId) {
            Service.post("/News/GetCarModelByCarBrandId", { brandId: brandId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.News.ListCarModel = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        //Get list bike model
        $scope.getListBikeModelsForNews = function (brandId) {
            Service.post("/News/GetBikeModelByBikeBrandId", { brandId: brandId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.News.ListBikeModel = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        //Get list car model details
        $scope.getListCarModelDetails = function (brandId, modelId) {
            Service.post("/CarInfo/GetCarModelDetails", { brandId: brandId, modelId: modelId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.News.ListCarModelDetail = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        $scope.BrandChange = function () {
            if ($scope.News.NewsExtend.BrandId == null) {
                $scope.News.NewsExtend.BrandId = 0;
            }
            if ($scope.News.TypeOfNews.IsAssessment || $scope.News.TypeOfNews.IsPricing || $scope.News.TypeOfNews.IsCarInfo)
                $scope.getListCarModelsForNews($scope.News.NewsExtend.BrandId);
            else if ($scope.News.TypeOfNews.IsBikeAssessment || $scope.News.TypeOfNews.IsBikePricing)
                $scope.getListBikeModelsForNews($scope.News.NewsExtend.BrandId);

        }

        $scope.ModelChange = function () {
            if ($scope.News.NewsExtend.ModelId == null) {
                $scope.News.NewsExtend.ModelId = 0;
            }
            if ($scope.News.TypeOfNews.IsAssessment || $scope.News.TypeOfNews.IsPricing || $scope.News.TypeOfNews.IsCarInfo)
                $scope.getListCarModelDetails($scope.News.NewsExtend.BrandId, $scope.News.NewsExtend.ModelId);
        }

        $scope.showNewsRelation = function () {
            closeAsidePanel("sm");
            $scope.CurrentForm = $scope.CurrentFormEnum.NewsRelation;
            $scope.getSuggestionList();
            $scope.ListNewsSearched = [];
            $scope.SearchNewsRelationModel.NewsType = $scope.News.NewsType;
            $scope.SearchNewsRelationModel.DisplayStyle = $scope.News.DisplayStyle;
            $scope.SearchNewsRelationModel.CateId = $scope.News.CateId;
            //$("#divnewsrela").show();
            //$("#tab1").trigger("click");
            $scope.getSearchInfo();
            scrollTo($("html"));
        }

        $scope.backtoedit = function () {
            openAsidePanel("sm");
            $("#divnewsrela").hide();
            scrollTo($("#btnshowrela"));

        }

        $scope.getSuggestionList = function () {
            $scope.ListNewsSuggested = [];
            Service.post("/News/GetListSuggestionNews",
                {
                    ModelId: $scope.News.NewsExtend.ModelId,
                    BrandId: $scope.News.NewsExtend.BrandId,
                    CarSegmentId: $scope.News.NewsExtend.CarSegmentId,
                    CarStyleId: $scope.News.NewsExtend.CarStyleId,
                    NewsType: $scope.News.NewsType,
                    CateId: $scope.News.CateId,
                    CurrentNewsId: $scope.News.Id,
                    ListTags: $("#txtAddTags").tagsinput("items"),
                    DisplayStyle: $scope.News.DisplayStyle
                }
            )
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListNewsRelationModels.lists.Source = $scope.ListNewsSuggested = response.Data;
                    }
                });
        }



        $scope.getSearchInfo = function () {
            $scope.SearchNewsRelationModel.ListCategory = [];
            for (var i = 0; i < $scope.News.ListCategory.length; i++) {
                if ($scope.News.ListCategory[i].Type == $scope.SearchNewsRelationModel.NewsType) {
                    $scope.SearchNewsRelationModel.ListCategory.push($scope.News.ListCategory[i]);
                }
            }
            $scope.SelectInit();
        }

        $scope.searchNewRelation = function () {
            $scope.pageSearchNewsRelation.currentPage = 1;
            $scope.getListNewRelation();
        }

        $scope.getListNewRelation = function () {
            Service.post("/News/SearchNews",
                {
                    CurrentNewsId: $scope.News.Id,
                    NewsType: $scope.SearchNewsRelationModel.NewsType,
                    DisplayStyle: $scope.SearchNewsRelationModel.DisplayStyle,
                    NewsRelateId: $scope.SearchNewsRelationModel.NewsRelateId,
                    Keyword: $scope.SearchNewsRelationModel.Keyword,
                    CateId: $scope.SearchNewsRelationModel.CateId,
                    PageIndex: $scope.pageSearchNewsRelation.currentPage,
                    PageSize: $scope.pageSearchNewsRelation.itemsPerPage
                }
            )
                .then(function (response) {
                    if (response.Success) {
                        $scope.pageSearchNewsRelation.totalItems = response.Data.TotalRows;
                        $scope.ListNewsSearched = response.Data.ListNewsSearched;
                        $scope.ListNewsRelationModels.lists.Source = $scope.ListNewsSearched;
                    }
                });
        }

        $scope.newsRelationChangeTab = function (tab) {
            $scope.CurrentNewsRelationTab = tab;
            if (tab == $scope.CurrentNewsRelationTabEnum.Search) {
                $scope.searchNewRelation();
            } else if (tab == $scope.CurrentNewsRelationTabEnum.Suggest) {
                $scope.ListNewsRelationModels.lists.Source = $scope.ListNewsSuggested;
            }
        }

        $scope.clickStickyNewsRelation = function (index, oldStatus) {
            $scope.News.ListNewsRelation[index].IsSticky = !$scope.News.ListNewsRelation[index].IsSticky;
            if ($scope.News.ListNewsRelation[index].IsSticky) {
                $scope.News.ListNewsRelation[index].Status = $scope.NewsRelationStatusEnum.Sticky;
            }
            else {
                if (oldStatus == $scope.NewsRelationStatusEnum.Sticky)
                    $scope.News.ListNewsRelation[index].Status = $scope.NewsRelationStatusEnum.Active;
                else
                    $scope.News.ListNewsRelation[index].Status = oldStatus;
            }
        }

        $scope.dropCallback = function (listName, item) {
            if (listName == 'Target') {
                // xóa bài trùng khi kéo sang
                var mappedList = $scope.ListNewsRelationModels.lists.Target.map(function (e) {
                    return e.NewsRelateIdStr;
                });
                var index = mappedList.indexOf(item.NewsRelateIdStr);

                if (index >= 0) {
                    $scope.ListNewsRelationModels.lists.Target.splice(index, 1);
                }

                $scope.DatePickerInit();
                return item;
            } else if (listName == 'Source') {

                // xóa bài trùng khi kéo trả
                var mappedList = $scope.ListNewsRelationModels.lists.Source.map(function (e) {
                    return e.NewsRelateIdStr;
                });
                var index = mappedList.indexOf(item.NewsRelateIdStr);
                if (index >= 0) {
                    $scope.ListNewsRelationModels.lists.Source.splice(index, 1);
                }
                return item;
            }
        }

        $scope.sendItem = function (item) {
            var mappedList = $scope.ListNewsRelationModels.lists.Source.map(function (e) {
                return e.NewsRelateIdStr;
            });
            var index = mappedList.indexOf(item.NewsRelateIdStr);
            if (index >= 0) {

                // nếu list targer có bài rồi thì thôi
                var mappedListTarget = $scope.ListNewsRelationModels.lists.Target.map(function (e) {
                    return e.NewsRelateIdStr;
                });
                var indexTarget = mappedListTarget.indexOf(item.NewsRelateIdStr);
                if (indexTarget < 0) {
                    $scope.ListNewsRelationModels.lists.Target.push(item);
                }

                $scope.ListNewsRelationModels.lists.Source.splice(index, 1);
            }
        }

        $scope.sortableOptions = {
            placeholder: "connectedSortableItem",
            connectWith: ".connectedSortable",
            cancel: ".not-sortable",
            'receive': receiveCallback
        };

        $scope.getListCategory = function () {
            var newsType = 2;
            Service.post("/Category/GetListCategoryByType", { newsType: newsType })
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListCategoryEnum = response.Data;
                    }
                });
        }

        function receiveCallback(e, ui) {
            var item = ui.item.sortable.model;
            var listName = ui.item.sortable.droptarget.hasClass('Source') ? 'Source' : 'Target';
            $scope.dropCallback(listName, item);
        }

        $scope.removeItemTarget = function (i, item) {
            var mappedList = $scope.ListNewsRelationModels.lists.Source.map(function (e) {
                return e.NewsRelateIdStr;
            });
            var index = mappedList.indexOf(item.NewsRelateIdStr);

            $scope.ListNewsRelationModels.lists.Target.splice(i, 1);
            if (index >= 0) {
                $scope.ListNewsRelationModels.lists.Source.splice(index, 1);
            }
            $scope.ListNewsRelationModels.lists.Source.push(item);
        }

        $scope.moveItemTarget = function (index, param) {
            if (index + param >= 0 && index + param < $scope.ListNewsRelationModels.lists.Target.length) {
                var arr = $scope.ListNewsRelationModels.lists.Target;
                arr = array_move(arr, index, index + param);
                $scope.ListNewsRelationModels.lists.Target = arr;
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

        $scope.ChangeRelaNewsType = function () {
            $scope.SearchNewsRelationModel.ListCategory = [];
            for (var i = 0; i < $scope.News.ListCategory.length; i++) {
                if ($scope.News.ListCategory[i].Type == $scope.SearchNewsRelationModel.NewsType) {
                    $scope.SearchNewsRelationModel.ListCategory.push($scope.News.ListCategory[i]);
                }
            }
            $scope.SelectInit();
        }

        $scope.getNewsTypeName = function (newsType) {
            return getNewsTypeName(newsType);
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
    //    $('#updateNewsEditor').css({ height: height });
    //    var heightBlock = height - $('#updateNewsBlock .card-header').innerHeight();
    //    $('#updateNewsBlock .card-block').css({ height: heightBlock });
    //    $(window).resize(function () {
    //        var height = $(window).innerHeight();
    //        $('#updateNewsEditor').css({ height: height });
    //        $('#updateNewsBlock').css({ height: height });
    //    });
    //}, 300);
    //$(window).resize(function () {
    //    var divLeft = $('#updateNewsEditor');
    //    var divRight = $('#updateNewsBlock');
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

function bindNewsVideoDuration(duration) {
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
            location.href = '/News/';
        }
    }
}
$(window).bind('beforeunload', function () {
    //save info somewhere
    if (!allowreload) {
        return confirm('Bạn có chắc muốn tải lại trang?');
    }
});
