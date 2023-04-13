(function () {
    app.controller('BannerController', ['$scope', '$rootScope', '$http', 'Service', '$sce', 'notify', '$ngConfirm', function ($scope, $rootScope, $http, Service, $sce, notify, $ngConfirm) {

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

        $scope.BannerStatus = {
            "Show": 1,
            "Hide": 2,
        };

        $scope.SearchModel = {}
        $scope.ParamModel = {}

        $scope.ListCarInfo = {};

        $scope.BannerModel = {};

        $scope.CurrentFormStatus = 0;
        $scope.FormStatusEnums = { List: 0, CreateOrUpdate: 1 };

        $scope.Init = function (obj) {
            if (obj) {
                $scope.ParamModel = obj;
                if ($scope.ParamModel.BannerId > 0) {
                    $scope.GetBanner($scope.ParamModel.BannerId);
                }
            }

            $scope.getListData();
        };

        //Get list credit card data
        $scope.getListData = function () {
            $scope.SearchModel.ListData = {};
            $scope.SearchModel.PageIndex = $scope.page.currentPage;
            $scope.SearchModel.PageSize = $scope.page.itemsPerPage;
            Service.post("/Banner/Search", { searchModel: $scope.SearchModel })
                .then(function (response) {
                    if (response.Success) {
                        $scope.SearchModel.ListData = response.Data.ListData;
                        $scope.SearchModel.ListPosition = response.Data.ListPosition;
                        $scope.SearchModel.ListPage = response.Data.ListPage;
                        $scope.SearchModel.ListStatus = response.Data.ListStatus;
                        $scope.SearchModel.ListPlatform = response.Data.ListPlatform;
                        $scope.page.totalItems = response.TotalRow;
                    }
                });
        };

        //Get banner
        $scope.GetBanner = function (id) {
            if (id > 0) {
                Service.post("/Banner/GetBanner", { bannerId: id })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.BannerModel = response.Data;
                            $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
                        }
                    });
            } else {
                $scope.BannerModel = {};
                $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
            }
        }

        $scope.doUpdate = function (banner) {
            if (banner != null && banner != undefined) {
                $scope.BannerModel = angular.copy(banner);
            }
            $scope.BannerModel.FromDateStr = $('#txtFromDate').val();
            $scope.BannerModel.UntilDateStr = $('#txtUntilDate').val();
            Service.post("/Banner/Update", { model: $scope.BannerModel })
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
        //updateStatus
        $scope.updatestatus = function (id) {
            Service.post("/Banner/UpdateStatusBanner", { id: id })
                .then(function (response) {
                    if (response.Success) {
                        notify({
                            message: Const.message.update_succsess,
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


        //lock
        $scope.lockBanner = function (bannerId) {
            $ngConfirm({
                title: cms.message.confirmWarningTitle,
                content: 'Bạn muốn khóa banner này?',
                type: cms.configs.ngConfirm.warningColor,
                scope: $scope,
                buttons: {
                    OK: {
                        type: cms.configs.ngConfirm.warningOKText,
                        btnClass: cms.configs.ngConfirm.warningOKClass,
                        keys: ['enter'],
                        action: function (scope, button) {
                            $scope.updatestatus(bannerId);
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

        //unlock
        $scope.unlockBanner = function (bannerId) {
            $ngConfirm({
                title: cms.message.confirmWarningTitle,
                content: 'Bạn muốn mở khóa banner này?',
                type: cms.configs.ngConfirm.warningColor,
                scope: $scope,
                buttons: {
                    OK: {
                        type: cms.configs.ngConfirm.warningOKText,
                        btnClass: cms.configs.ngConfirm.warningOKClass,
                        keys: ['enter'],
                        action: function (scope, button) {
                            $scope.updatestatus(bannerId);
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

        //Delete Banner
        $scope.delete = function (id) {
            if (confirm("Bạn muốn xóa bản ghi này?")) {
                Service.post("/Banner/Delete", { id: id })
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
        $scope.deleteAvatar = function () {
            $scope.BannerModel.Embed = cms.configs.NoImage;
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
                    $scope.BannerModel.Embed = result.path;
                });
                w.close();
            }
        }

        //Get banner to update
        $scope.editextend = function (banner) {
            if (banner) {
                $scope.BannerModel = banner;
                $scope.PushStateUrl($scope.BannerModel.Id);
            } else {
                $scope.BannerModel = {};
            }
            $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
            document.documentElement.scrollTop = 80;
            //$scope.closeToolTips();
        }

        //Edit banner on list
        $scope.edit = function (banner) {
            $scope.cancelUpdate();
            $scope.BannerModel = angular.copy(banner);
            //$scope.closeToolTips();
        };

        //Get template
        $scope.getTemplate = function (banner) {
            if (!banner || $scope.CurrentFormStatus == $scope.FormStatusEnums.CreateOrUpdate) {
                return 'display';
            }
            if (banner.Id === $scope.BannerModel.Id) return 'edit';
            else return 'display';
        }

        //Close tooltips
        $scope.closeToolTips = function () {
            $('[data-toggle="tooltip"]').tooltip('hide');
        }

        //Rewrite URL
        $scope.PushStateUrl = function (id) {
            window.setTimeout(function () {
                window.history.pushState(null, null, "/Banner?bannerId=" + id);
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
            $scope.BannerModel = {};
            $scope.CurrentFormStatus = $scope.FormStatusEnums.List;
            window.setTimeout(function () {
                window.history.pushState(null, null, "/Banner");
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