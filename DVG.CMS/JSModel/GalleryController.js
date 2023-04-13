(function () {
    app.controller('GalleryController', ['$scope', '$rootScope', '$http', 'Service', '$sce', 'notify', '$ngConfirm', function ($scope, $rootScope, $http, Service, $sce, notify, $ngConfirm) {

        // Position of message
        $scope.positions = {
            "Center": 'center',
            "Left": 'left',
            "Right": 'right'
        };
        // Class of message
        $scope.classes = {
            "Success": 'alert-success',
            "Error": 'alert-danger'
        };
        // The time display of message
        $scope.duration = 2000;

        $scope.page = {
            'totalItems': 0,
            'currentPage': 1,
            'itemsPerPage': 15
        };

        $scope.GalleryStatus = [
            { "Id": 1, "Name": "Hoạt động" },
            { "Id": 2, "Name": "Không hoạt động" }
        ];

        $scope.GalleryStatusEnum = {
            "Show": 1,
            "Hide": 2,
        };


        $scope.SearchModel = {}
        $scope.ParamModel = {}

        $scope.GalleryModel = {};

        $scope.CurrentFormStatus = 0;
        $scope.FormStatusEnums = { List: 0, CreateOrUpdate: 1 };

        $scope.Init = function (obj) {
            if (obj) {
                $scope.ParamModel = obj;
                if ($scope.ParamModel.GalleryId > 0) {
                    $scope.GetGallery($scope.ParamModel.GalleryId);
                }
            }
            $scope.getListData();
        };

        //Get list credit card data
        $scope.getListData = function () {
            $scope.SearchModel.ListData = {};
            $scope.SearchModel.PageIndex = $scope.page.currentPage;
            $scope.SearchModel.PageSize = $scope.page.itemsPerPage;
            Service.post("/Gallery/Search", { searchModel: $scope.SearchModel })
                .then(function (response) {
                    if (response.Success) {
                        $scope.SearchModel.ListData = response.Data.ListData;
                        $scope.page.totalItems = response.TotalRow;
                    }
                });
        };

        //Get Gallery
        $scope.GetGallery = function (id) {
            if (id > 0) {
                Service.post("/Gallery/GetGallery", { GalleryId: id })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.GalleryModel = response.Data;
                            $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
                        }
                    });
            } else {
                $scope.GalleryModel = {};
                $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
            }
        }

        $scope.doUpdate = function (Gallery) {
            if (Gallery != null && Gallery != undefined) {
                $scope.GalleryModel = angular.copy(Gallery);
            }
            Service.post("/Gallery/Update", { model: $scope.GalleryModel })
                .then(function (response) {
                    if (response.Success) {
                        notify({
                            message: Const.message.update_succsess,
                            classes: $scope.classes.Success,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });
                    } else {
                        notify({
                            message: "Lỗi! " + response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });
                    }
                    $scope.cancelUpdate();
                    $scope.getListData();
                });
        }

        // Click button search
        $scope.doSearch = function () {
            $scope.page.currentPage = 1;
            $scope.getListData();
        }

        //Delete Gallery
        $scope.delete = function (id) {
            if (confirm("Bạn muốn xóa bản ghi này?")) {
                Service.post("/Gallery/Delete", { id: id })
                    .then(function (response) {
                        if (response.Success) {
                            notify({
                                message: Const.message.delete_succsess,
                                classes: $scope.classes.Success,
                                position: $scope.positions.Center,
                                duration: $scope.duration
                            });
                            $scope.doSearch();
                        } else {
                            notify({
                                message: "Lỗi! " + response.Message,
                                classes: $scope.classes.Error,
                                position: $scope.positions.Center,
                                duration: $scope.duration
                            });
                        }
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

        $scope.deleteAvatar = function () {
            $scope.GalleryModel.AvatarStr = cms.configs.NoImage;
            $scope.GalleryModel.Url = "";
        }

        // Upload avatar
        $scope.selectAvatar = function () {
            var w = window.open("/FileManager/Default.aspx", "File manager", "width = 950, height = 600");

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
                $scope.$apply(function () {
                    $scope.GalleryModel.AvatarStr = result.path;
                    $scope.GalleryModel.Url = result.path;
                });
                w.close();
            }
        }

        //Get Gallery to update
        $scope.editextend = function (Gallery) {
            if (Gallery) {
                console.log(Gallery);
                $scope.GalleryModel = Gallery;
                $scope.PushStateUrl($scope.GalleryModel.Id);
                if ($scope.GalleryModel.AvatarStr == "")
                    $scope.GalleryModel.AvatarStr = cms.configs.NoImage;
                //window.setTimeout(function () {
                //    CKEDITOR.instances["txtDescription"].setData($scope.GalleryModel.Embed);
                //}, 200);
            } else {
                $scope.GalleryModel = {};
                $scope.GalleryModel.AvatarStr = cms.configs.NoImage;
                //CKEDITOR.instances["txtDescription"].setData('');
            }
            $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
            document.documentElement.scrollTop = 80;
            //$scope.closeToolTips();
        }

        //Edit Gallery on list
        $scope.edit = function (Gallery) {
            $scope.cancelUpdate();
            $scope.GalleryModel = angular.copy(Gallery);
            //$scope.closeToolTips();
        };

        //Get template
        $scope.getTemplate = function (Gallery) {
            if (!Gallery || $scope.CurrentFormStatus == $scope.FormStatusEnums.CreateOrUpdate) {
                return 'display';
            }
            if (Gallery.Id === $scope.GalleryModel.Id) return 'edit';
            else return 'display';
        }

        //Close tooltips
        $scope.closeToolTips = function () {
            $('[data-toggle="tooltip"]').tooltip('hide');
        }

        //Rewrite URL
        $scope.PushStateUrl = function (id) {
            window.setTimeout(function () {
                window.history.pushState(null, null, "/Gallery?GalleryId=" + id);
            }, $scope.duration);
        }

        //global function getlist
        $rootScope.$on("CallGetListMethod", function () {
            $scope.getListData();
        });

        //global function cancel
        $rootScope.$on("CallCancelUpdateMethod", function () {
            $scope.cancelUpdate();
        });

        $scope.cancelUpdate = function () {
            $scope.GalleryModel = {};
            $scope.CurrentFormStatus = $scope.FormStatusEnums.List;
            window.setTimeout(function () {
                window.history.pushState(null, null, "/Gallery");
            }, $scope.duration);
            //$scope.closeToolTips();
        }

        //Key press search
        $scope.keypressAction = function (keyEvent) {
            if (keyEvent.which === 13) {
                $scope.page.currentPage = 1;
                $scope.getListData();
            }
        }

        $scope.roundNumberPaging = function (itemPerPage, totalItem) {
            return Math.ceil(parseFloat(totalItem) / itemPerPage);
        }

    }]);
})();