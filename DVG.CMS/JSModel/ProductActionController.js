// Create the factory that share the Data
app.factory('Data', function () {
    return { CarInfo: {} };
});

(function () {
    app.controller('ProductActionController', ['$scope', '$rootScope', '$http', 'Service', 'notify', '$ngConfirm', 'Data', function ($scope, $rootScope, $http, Service, notify, $ngConfirm, Data) {
        //function ($scope, $rootScope, $http, Service, notify, Data) {
        $scope.VideoDuration = {
            'Hour': 0,
            'Minute': 0,
            'Second': 0
        };
        $scope.Product = {};

        $scope.CurrentFormEnum = {
            'Edit': 0,
            'ProductRelation': 1
        }
        $scope.CurrentForm = $scope.CurrentFormEnum.Edit;
        $scope.CurrentProductRelationTabEnum = {
            'Suggest': 0,
            'Search': 1
        }
        $scope.CurrentProductRelationTab = $scope.CurrentProductRelationTabEnum.Suggest;

        $scope.SearchProductRelationModel = {
            'ProductRelateId': '',
            'Keyword': '',
            'ProductType': 1,
            'CateId': '',
            'ListCategory': []
        };

        $scope.pageSearchProductRelation = {
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

        $scope.ProductStatus = {
            "PendingApproved": 0,
            "Published": 1,
            "UnPublished": 2,
            "Deleted": 3
        }
        $scope.getProductRelationStatusName = function (status) {
            if (status == $scope.ProductRelationStatusEnum.Active) {
                return "Đang hiển thị";
            }
            else if (status == $scope.ProductRelationStatusEnum.Sticky) {
                return "Đang neo top";
            }
            else if (status == $scope.ProductRelationStatusEnum.Blocked) {
                return "Đang bị khóa";
            }
        }

        $scope.ListProductOutOfStock = [
            { "Id": 1, "Name": "Còn hàng" },
            { "Id": 2, "Name": "Hết hàng" }
        ];

        $scope.HighLightEnum = [
            { "Id": 0, "Name": "Không kích hoạt" },
            { "Id": 1, "Name": "Kích hoạt" }
        ];
        $scope.ListCategoryEnum = [
            { "Id": 1, "Name": "Chăm sóc da mặt" },
            { "Id": 4, "Name": "Chăm sóc toàn thân" }
        ];

        $scope.ListCategoryDB = [];

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
                $scope.Product = obj;
                $scope.Product.Id = obj.IdStr;
                $scope.ListCategoryDB = obj.ListCategory;

                $scope.Product.CategoryId = obj.CategoryId;
                CKEDITOR.on("instanceReady", function () {
                    window.setTimeout(function () {
                        CKEDITOR.instances["content"].setData(obj.Description);
                    }, 200);
                });

                // scroll
                $('#updateProductBlock').niceScroll({ touchbehavior: false, cursoropacitymax: 0.6, cursorwidth: 5 });
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
            $scope.Search.BrandId = $scope.Product.ProductExtend.BrandId;
            $scope.Search.ModelId = $scope.Product.ProductExtend.ModelId;
            $scope.getListDataForControl();
            $scope.getListData();
        });

        //Get list credit card data
        $scope.getListData = function () {
            $scope.Search.PageIndex = $scope.page.currentPage;
            $scope.Search.PageSize = $scope.page.itemsPerPage;
            $scope.Search.ProductIdEncrypt = $scope.Product.EncryptProductId;
            Service.post("/CarInfo/SearchForProduct", { searchModel: $scope.Search })
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
                            $scope.Search.ModelId = $scope.Product.ProductExtend.ModelId;
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


        //Get car info to Product
        $scope.getCarInfo = function (id) {
            if (id > 0) {
                Service.post("/CarInfo/GetCarInfoUpdate", { carInfoId: id })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.CarInfoModel = response.Data;
                            $scope.updateProductCarInfo();
                            $("#modalAddCarInfo").modal("hide");
                        }
                    });
            }
        }

        //Update car info in Product
        $scope.updateProductCarInfo = function () {
            $scope.getListCarModelsForProduct($scope.CarInfoModel.BrandId);
            $scope.getListCarModelDetails($scope.CarInfoModel.BrandId, $scope.CarInfoModel.ModelId);
            $scope.Product.ProductExtend.BrandId = $scope.CarInfoModel.BrandId;
            $scope.Product.ProductExtend.ModelId = $scope.CarInfoModel.ModelId;
            $scope.Product.ProductExtend.CarModelDetailId = $scope.CarInfoModel.ModelDetailId;
            $scope.Product.ProductExtend.CarStyleId = $scope.CarInfoModel.StyleId;
            $scope.Product.ProductExtend.CarSegmentId = $scope.CarInfoModel.SegmentId;
            $scope.Product.ProductExtend.ProductionYear = $scope.CarInfoModel.ProductionYear;
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

        $scope.ChangeStatusProduct = function (statusProduct, Idstr) {
            Service.post("/Product/ChangeStatusProduct", { statusProduct: statusProduct, Idstr: Idstr })
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

        //On change title Product
        $scope.onChangeTitleProduct = function () {
            $scope.Product.AvatarDesc = $scope.Product.Title;
            if ($scope.Product.Title) {
                if (!$scope.Product.StatusOfProduct.IsPublished && !$scope.Product.StatusOfProduct.IsUnPublished) {
                    $scope.Product.OriginalUrl = UnicodeToUnsignAndSlash($scope.Product.Title);
                }
            }
            $scope.Product.SEOTitle = $scope.Product.Title;
        }

        $scope.BlurTitleInput = function () {
            $("#txtOriginalUrl").blur();
        }

        //On change sapo Product
        $scope.onChangeSapoProduct = function () {
            $scope.Product.SEODescription = $scope.Product.Sapo;
        }

        $scope.AutoCheckLinkOriginalUrl = function (url) {
            Service.post("/Product/AutoCheckLinkOriginalUrl", { "url": url })
                .then(function (result) {
                    if (result.Success && result.Data) {

                    }
                });
        }

        $scope.doUpdate = function (currentStatus, status) {
            $scope.Product.Description = CKEDITOR.instances["content"].getData();
            $scope.Product.Status = status;
            $scope.Product.DistributionDateStr = $("#txtDistributionDate").val();
            $scope.Product.PricingDateStr = $("#txtPricingDate").val();
            $scope.Product.ListTags = $("#txtAddTags").tagsinput("items");
            $scope.Product.TagItem = "";
            $scope.Product.CarInfo = $scope.CarInfoModel;
            if ($scope.Product.Description.length <= 0) {
                notify({
                    message: "Vui lòng nhập nội dung bài viết!",
                    classes: $scope.classes.Error,
                    templateUrl: $scope.template,
                    position: $scope.position,
                    duration: $scope.duration
                });
                return false;
            }
            if ($scope.Product.CategoryId <= 0) {
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
            if (!$scope.Product.Avatar || $scope.Product.Avatar.length <= 0) {
                notify({
                    message: "Bạn chưa chọn ảnh đại diện!",
                    classes: $scope.classes.Error,
                    templateUrl: $scope.template,
                    position: $scope.position,
                    duration: $scope.duration
                });
                return false;
            }
            //if ($scope.Product.DisplayStyleOnList == $scope.DisplayStyleOnListEnum.Cover && (!$scope.Product.Avatar2 || $scope.Product.Avatar2.length <= 0)) {
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
                $scope.Product.ListMenuHeading = [];
                $(listElemH).each(function () {
                    var menuHeading = new Object();
                    menuHeading.NameExtend = $(this).html();
                    menuHeading.MenuId = $(this).attr("dataId");
                    menuHeading.Ordinal = $(this).attr("dataOrdinal");
                    $scope.Product.ListMenuHeading.push(menuHeading);
                });
            }
            // processing video in content
            //var hour = parseInt($scope.VideoDuration.Hour);
            //var min = parseInt($scope.VideoDuration.Minute);
            //var sec = parseInt($scope.VideoDuration.Second);
            //if (hour > 0 || min > 0 || sec > 0) {
            //    strDuration = (hour > 9 ? hour : "0" + hour) + ":" + (min > 9 ? min : "0" + min) + ":" + (sec > 9 ? sec : "0" + sec);
            //    $scope.Product.VideoDuration = strDuration;
            //}

            var lstObjVideo = $(editor.document.$).find(".dvsvideo");
            if (lstObjVideo.length > 0) {
                // nếu có video trong bài thì không cho chọn AMP;
                $(lstObjVideo).each(function () {
                    $scope.Product.Description = $scope.Product.Description.replace(/data-raw="([^"]+)"/, "");
                    return;
                });
            }

            // xử lý xóa ký tự trống ở title box nhúng nội dung
            $scope.Product.Description = $scope.Product.Description.replace('<p class="dvs-textbox-title">&nbsp;</p>', '<p class="dvs-textbox-title"></p>');

            $scope.Product.PublishedDateStr = $('#txtPublishedDate').val();
            Service.post("/Product/UpdateProduct", { Product: $scope.Product, currentStatus: currentStatus })
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
                        $scope.Product.Status = currentStatus;
                    }
                });
        }

        var doAutoSave = function () {
            $scope.Product.Description = CKEDITOR.instances["content"].getData();
            var data = $scope.Product.Description.replace(/<[^>]+>/g, '\n').replace(/\n\s*\n/g, '\n\n').trim();
            if (data.length <= 0) {
                return;
            }
            $scope.Product.DistributionDateStr = $("#txtDistributionDate").val();
            $scope.Product.PricingDateStr = $("#txtPricingDate").val();
            $scope.Product.ListTags = $("#txtAddTags").tagsinput("items");
            $scope.Product.TagItem = "";
            $scope.Product.CarInfo = $scope.CarInfoModel;

            if (!$scope.Product.IsReference) {
                $scope.Product.ReferenceUrl = null;
            }
            var len = $scope.Product.ListTags.length;
            for (var i = 0; i < len; i++) {
                $scope.Product.TagItem = $scope.Product.TagItem + $scope.Product.ListTags[i].Name + ";";
            }
            if ($scope.Product.ListProductRelation != null && $scope.Product.ListProductRelation != undefined) {
                for (var i = 0; i < $scope.Product.ListProductRelation.length; i++) {
                    if ($scope.Product.ListProductRelation[i].IsSticky) {
                        $scope.Product.ListProductRelation[i].StickyFromDate = $("[name='txtRelaStickyFromDateRelation" + i + "']").val();
                        $scope.Product.ListProductRelation[i].StickyUntilDate = $("[name='txtRelaStickyUntilDateRelation" + i + "']").val();
                    }
                }
            }

            //Process for menu heading
            var editor = CKEDITOR.instances['content'];
            var listElemH = $(editor.document.$).find(".dvs_menuheading_item");
            if (listElemH.length > 0) {
                $scope.Product.ListMenuHeading = [];
                $(listElemH).each(function () {
                    var menuHeading = new Object();
                    menuHeading.NameExtend = $(this).html();
                    menuHeading.MenuId = $(this).attr("dataId");
                    $scope.Product.ListMenuHeading.push(menuHeading);
                });
            }

            // processing video in content
            var hour = parseInt($scope.VideoDuration.Hour);
            var min = parseInt($scope.VideoDuration.Minute);
            var sec = parseInt($scope.VideoDuration.Second);
            if (hour > 0 || min > 0 || sec > 0) {
                strDuration = (hour > 9 ? hour : "0" + hour) + ":" + (min > 9 ? min : "0" + min) + ":" + (sec > 9 ? sec : "0" + sec);
                $scope.Product.VideoDuration = strDuration;
            }

            var lstObjVideo = $(editor.document.$).find(".dvsvideo");
            if (lstObjVideo.length > 0) {
                $(lstObjVideo).each(function () {
                    var dataRaw = $(this).attr("data-raw");
                    if (dataRaw && dataRaw.length > 0)
                        $scope.Product.ProductExtend.VideoUrl = $(this).attr("data-raw");
                    $scope.Product.ProductExtend.VideoThumb = $(this).attr("data-raw-image");
                    if (!$scope.Product.VideoDuration || $scope.Product.VideoDuration == null) $scope.Product.VideoDuration = $(this).attr("data-duration");
                    $scope.Product.Description = $scope.Product.Description.replace(/data-raw="([^"]+)"/, "");
                    return;
                });
            }
            else {
                $scope.Product.ProductExtend.VideoUrl = null;
                $scope.Product.ProductExtend.VideoThumb = null;
                $scope.Product.VideoDuration = null;
            }

            Service.post("/Product/AutoSave", { Product: $scope.Product })
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

        //Save Product
        $scope.doSave = function () {
            ngConfirm($scope.doUpdate, $scope.Product.Status, $scope.Product.Status, cms.message.confirmSave);
        }

        //Publish Product
        $scope.doPublish = function () {
            ngConfirm($scope.doUpdate, $scope.Product.Status, $scope.ProductStatus.Published, cms.message.confirmDoPublish);
        }

        //Gỡ bài đã publish
        $scope.doUnPublish = function () {
            ngConfirm($scope.doUpdate, $scope.Product.Status, $scope.ProductStatus.UnPublished, cms.message.confirmDoUnPublish);
        }

        //Gửi bài lên chờ duyệt
        $scope.doPendingApprove = function () {
            ngConfirm($scope.doUpdate, $scope.Product.Status, $scope.ProductStatus.PendingApproved, cms.message.confirmDoPendingApprove);
        }

        // xóa bài
        $scope.doDelete = function () {
            ngConfirm($scope.doUpdate, $scope.Product.Status, $scope.ProductStatus.Deleted, cms.message.confirmDoDelete);
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

        //Add Product tags
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
                        $scope.Product.Avatar = result.path;
                        $scope.Product.AvatarStr = result.path;
                    });
                }
                else if (index == "avatar2") {
                    $scope.$apply(function () {
                        $scope.Product.Avatar2 = result.path;
                        $scope.Product.AvatarStr2 = result.path;
                    });
                }
                else {
                    $scope.$apply(function () {
                        $scope.Product.ListImage[index].ImageUrl = result.path;
                        $scope.Product.ListImage[index].ImageUrlCrop = result.path;
                    });
                }
                w.close();
            }
        }
        //Xóa ảnh avatar hoặc ảnh slide
        $scope.delImages = function (index) {
            if (index == "avatar1") {
                $scope.Product.Avatar = "";
                $scope.Product.AvatarStr = cms.configs.NoImage;
            }
            else if (index == "avatar2") {
                $scope.Product.Avatar2 = "";
                $scope.Product.AvatarStr2 = cms.configs.NoImage;
            }
            else {
                $scope.Product.ListImage.splice(index, 1);
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
                        if ($scope.Product.ListImage == null) {
                            $scope.Product.ListImage = [];
                        }
                        $scope.Product.ListImage.push(imageadd);
                    });
                }
                w.close();
            }
        }
        $scope.OldProductType = "";
        $scope.ChangeProductType = function () {
            if (!confirm('Nếu đổi loại tin, một vài thông tin khác có thể sẽ bị thay đổi (VD: giá xe, đánh giá xe,...). Bạn có chắc chắn đổi không?')) {
                $scope.Product.ProductType = $scope.OldProductType;
            }
            $scope.SelectInit();
            $scope.OldProductType = $scope.Product.ProductType;
            if ($scope.Product.ProductType == $scope.ProductTypeEnum.Gallery) {
                $scope.Product.DisplayStyle = 2; //Mặc định là bài ảnh
            }

            // reset ProductExtend
            $scope.Product.ProductExtend.BrandId = 0;
            $scope.Product.ProductExtend.ModelId = 0;
            $scope.Product.ProductExtend.CarModelDetailId = 0;
            $scope.Product.ProductExtend.CarStyleId = 0;
            $scope.Product.ProductExtend.CarSegmentId = 0;
            $scope.Product.ProductExtend.ProductionYear = "";

            $scope.Product.TypeOfProduct.IsAssessment = false;
            $scope.Product.TypeOfProduct.IsBikeAssessment = false;
            $scope.Product.TypeOfProduct.IsPricing = false;
            $scope.Product.TypeOfProduct.IsBikePricing = false;
            $scope.Product.TypeOfProduct.IsCarInfo = false;
            switch ($scope.Product.ProductType) {
                case $scope.ProductTypeEnum.CarInfo:
                    {
                        $scope.Product.TypeOfProduct.IsCarInfo = true;
                        break;
                    }
                case $scope.ProductTypeEnum.Assessment:
                    {
                        $scope.Product.TypeOfProduct.IsAssessment = true;
                        break;
                    }
                case $scope.ProductTypeEnum.BikeAssessment:
                    {
                        $scope.Product.TypeOfProduct.IsBikeAssessment = true;
                        break;
                    }
                case $scope.ProductTypeEnum.Pricing:
                    {
                        $scope.Product.TypeOfProduct.IsPricing = true;
                        break;
                    }
                case $scope.ProductTypeEnum.BikePricing:
                    {
                        $scope.Product.TypeOfProduct.IsBikePricing = true;
                        break;
                    }
            }
        }

        $scope.CateChange = function () {
            if ($scope.Product.CateId == null) {
                $scope.Product.CateId = 0;
            }
        }

        $scope.GetTopic = function () {
            Service.post("/Product/GetTopic")
                .then(function (response) {
                    if (response.Success) {
                        $scope.Product.ListTopic = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        //Get list car model
        $scope.getListCarModelsForProduct = function (brandId) {
            Service.post("/Product/GetCarModelByCarBrandId", { brandId: brandId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.Product.ListCarModel = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        //Get list bike model
        $scope.getListBikeModelsForProduct = function (brandId) {
            Service.post("/Product/GetBikeModelByBikeBrandId", { brandId: brandId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.Product.ListBikeModel = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        //Get list car model details
        $scope.getListCarModelDetails = function (brandId, modelId) {
            Service.post("/CarInfo/GetCarModelDetails", { brandId: brandId, modelId: modelId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.Product.ListCarModelDetail = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        $scope.BrandChange = function () {
            if ($scope.Product.ProductExtend.BrandId == null) {
                $scope.Product.ProductExtend.BrandId = 0;
            }
            if ($scope.Product.TypeOfProduct.IsAssessment || $scope.Product.TypeOfProduct.IsPricing || $scope.Product.TypeOfProduct.IsCarInfo)
                $scope.getListCarModelsForProduct($scope.Product.ProductExtend.BrandId);
            else if ($scope.Product.TypeOfProduct.IsBikeAssessment || $scope.Product.TypeOfProduct.IsBikePricing)
                $scope.getListBikeModelsForProduct($scope.Product.ProductExtend.BrandId);

        }

        $scope.ModelChange = function () {
            if ($scope.Product.ProductExtend.ModelId == null) {
                $scope.Product.ProductExtend.ModelId = 0;
            }
            if ($scope.Product.TypeOfProduct.IsAssessment || $scope.Product.TypeOfProduct.IsPricing || $scope.Product.TypeOfProduct.IsCarInfo)
                $scope.getListCarModelDetails($scope.Product.ProductExtend.BrandId, $scope.Product.ProductExtend.ModelId);
        }

        $scope.showProductRelation = function () {
            closeAsidePanel("sm");
            $scope.CurrentForm = $scope.CurrentFormEnum.ProductRelation;
            $scope.getSuggestionList();
            $scope.ListProductSearched = [];
            $scope.SearchProductRelationModel.ProductType = $scope.Product.ProductType;
            $scope.SearchProductRelationModel.DisplayStyle = $scope.Product.DisplayStyle;
            $scope.SearchProductRelationModel.CateId = $scope.Product.CateId;
            //$("#divProductrela").show();
            //$("#tab1").trigger("click");
            $scope.getSearchInfo();
            scrollTo($("html"));
        }

        $scope.backtoedit = function () {
            openAsidePanel("sm");
            $("#divProductrela").hide();
            scrollTo($("#btnshowrela"));

        }

        $scope.getSuggestionList = function () {
            $scope.ListProductSuggested = [];
            Service.post("/Product/GetListSuggestionProduct",
                {
                    ModelId: $scope.Product.ProductExtend.ModelId,
                    BrandId: $scope.Product.ProductExtend.BrandId,
                    CarSegmentId: $scope.Product.ProductExtend.CarSegmentId,
                    CarStyleId: $scope.Product.ProductExtend.CarStyleId,
                    ProductType: $scope.Product.ProductType,
                    CateId: $scope.Product.CateId,
                    CurrentProductId: $scope.Product.Id,
                    ListTags: $("#txtAddTags").tagsinput("items"),
                    DisplayStyle: $scope.Product.DisplayStyle
                }
            )
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListProductRelationModels.lists.Source = $scope.ListProductSuggested = response.Data;
                    }
                });
        }

        $scope.getSearchInfo = function () {
            $scope.SearchProductRelationModel.ListCategory = [];
            for (var i = 0; i < $scope.Product.ListCategory.length; i++) {
                if ($scope.Product.ListCategory[i].Type == $scope.SearchProductRelationModel.ProductType) {
                    $scope.SearchProductRelationModel.ListCategory.push($scope.Product.ListCategory[i]);
                }
            }
            $scope.SelectInit();
        }

        $scope.searchNewRelation = function () {
            $scope.pageSearchProductRelation.currentPage = 1;
            $scope.getListNewRelation();
        }

        $scope.getListNewRelation = function () {
            Service.post("/Product/SearchProduct",
                {
                    CurrentProductId: $scope.Product.Id,
                    ProductType: $scope.SearchProductRelationModel.ProductType,
                    DisplayStyle: $scope.SearchProductRelationModel.DisplayStyle,
                    ProductRelateId: $scope.SearchProductRelationModel.ProductRelateId,
                    Keyword: $scope.SearchProductRelationModel.Keyword,
                    CateId: $scope.SearchProductRelationModel.CateId,
                    PageIndex: $scope.pageSearchProductRelation.currentPage,
                    PageSize: $scope.pageSearchProductRelation.itemsPerPage
                }
            )
                .then(function (response) {
                    if (response.Success) {
                        $scope.pageSearchProductRelation.totalItems = response.Data.TotalRows;
                        $scope.ListProductSearched = response.Data.ListProductSearched;
                        $scope.ListProductRelationModels.lists.Source = $scope.ListProductSearched;
                    }
                });
        }

        $scope.ProductRelationChangeTab = function (tab) {
            $scope.CurrentProductRelationTab = tab;
            if (tab == $scope.CurrentProductRelationTabEnum.Search) {
                $scope.searchNewRelation();
            } else if (tab == $scope.CurrentProductRelationTabEnum.Suggest) {
                $scope.ListProductRelationModels.lists.Source = $scope.ListProductSuggested;
            }
        }

        $scope.clickStickyProductRelation = function (index, oldStatus) {
            $scope.Product.ListProductRelation[index].IsSticky = !$scope.Product.ListProductRelation[index].IsSticky;
            if ($scope.Product.ListProductRelation[index].IsSticky) {
                $scope.Product.ListProductRelation[index].Status = $scope.ProductRelationStatusEnum.Sticky;
            }
            else {
                if (oldStatus == $scope.ProductRelationStatusEnum.Sticky)
                    $scope.Product.ListProductRelation[index].Status = $scope.ProductRelationStatusEnum.Active;
                else
                    $scope.Product.ListProductRelation[index].Status = oldStatus;
            }
        }

        $scope.dropCallback = function (listName, item) {
            if (listName == 'Target') {
                // xóa bài trùng khi kéo sang
                var mappedList = $scope.ListProductRelationModels.lists.Target.map(function (e) {
                    return e.ProductRelateIdStr;
                });
                var index = mappedList.indexOf(item.ProductRelateIdStr);

                if (index >= 0) {
                    $scope.ListProductRelationModels.lists.Target.splice(index, 1);
                }

                $scope.DatePickerInit();
                return item;
            } else if (listName == 'Source') {

                // xóa bài trùng khi kéo trả
                var mappedList = $scope.ListProductRelationModels.lists.Source.map(function (e) {
                    return e.ProductRelateIdStr;
                });
                var index = mappedList.indexOf(item.ProductRelateIdStr);
                if (index >= 0) {
                    $scope.ListProductRelationModels.lists.Source.splice(index, 1);
                }
                return item;
            }
        }

        $scope.sendItem = function (item) {
            var mappedList = $scope.ListProductRelationModels.lists.Source.map(function (e) {
                return e.ProductRelateIdStr;
            });
            var index = mappedList.indexOf(item.ProductRelateIdStr);
            if (index >= 0) {

                // nếu list targer có bài rồi thì thôi
                var mappedListTarget = $scope.ListProductRelationModels.lists.Target.map(function (e) {
                    return e.ProductRelateIdStr;
                });
                var indexTarget = mappedListTarget.indexOf(item.ProductRelateIdStr);
                if (indexTarget < 0) {
                    $scope.ListProductRelationModels.lists.Target.push(item);
                }

                $scope.ListProductRelationModels.lists.Source.splice(index, 1);
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
            var mappedList = $scope.ListProductRelationModels.lists.Source.map(function (e) {
                return e.ProductRelateIdStr;
            });
            var index = mappedList.indexOf(item.ProductRelateIdStr);

            $scope.ListProductRelationModels.lists.Target.splice(i, 1);
            if (index >= 0) {
                $scope.ListProductRelationModels.lists.Source.splice(index, 1);
            }
            $scope.ListProductRelationModels.lists.Source.push(item);
        }

        $scope.moveItemTarget = function (index, param) {
            if (index + param >= 0 && index + param < $scope.ListProductRelationModels.lists.Target.length) {
                var arr = $scope.ListProductRelationModels.lists.Target;
                arr = array_move(arr, index, index + param);
                $scope.ListProductRelationModels.lists.Target = arr;
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

        $scope.ChangeRelaProductType = function () {
            $scope.SearchProductRelationModel.ListCategory = [];
            for (var i = 0; i < $scope.Product.ListCategory.length; i++) {
                if ($scope.Product.ListCategory[i].Type == $scope.SearchProductRelationModel.ProductType) {
                    $scope.SearchProductRelationModel.ListCategory.push($scope.Product.ListCategory[i]);
                }
            }
            $scope.SelectInit();
        }

        $scope.getProductTypeName = function (ProductType) {
            return getProductTypeName(ProductType);
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
    //    $('#updateProductEditor').css({ height: height });
    //    var heightBlock = height - $('#updateProductBlock .card-header').innerHeight();
    //    $('#updateProductBlock .card-block').css({ height: heightBlock });
    //    $(window).resize(function () {
    //        var height = $(window).innerHeight();
    //        $('#updateProductEditor').css({ height: height });
    //        $('#updateProductBlock').css({ height: height });
    //    });
    //}, 300);
    //$(window).resize(function () {
    //    var divLeft = $('#updateProductEditor');
    //    var divRight = $('#updateProductBlock');
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

function bindProductVideoDuration(duration) {
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
            location.href = '/Product/';
        }
    }
}
$(window).bind('beforeunload', function () {
    //save info somewhere
    if (!allowreload) {
        return confirm('Bạn có chắc muốn tải lại trang?');
    }
});
