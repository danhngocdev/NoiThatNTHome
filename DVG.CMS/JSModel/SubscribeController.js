(function () {
    app.controller('SubscribeController', ['$scope', '$rootScope', '$http', 'Service', '$sce', 'notify', '$ngConfirm', function ($scope, $rootScope, $http, Service, $sce, notify, $ngConfirm) {

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
        $scope.SubscribeStatusEnum = {
            'Active': 1,
            'Banned': 2
        }
        $scope.SubscribeSearchModel = {};
        $scope.CurrentFormStatus = 0;
        $scope.FormStatusEnums = { List: 0, CreateOrUpdate: 1 };
        $scope.EditItem = {
        };
        $scope.PatternEmail = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,5})$/;

        $scope.Init = function () {
        };

        //Init chosen for select option
        $scope.SelectInit = function () {
            setTimeout(function () {
                $(".chosen-select").chosen();
                $('.chosen-select').trigger('chosen:updated');
            }, 500);
        }

        $scope.NewsTypeChange = function () {
            var newsType = $scope.EditItem.NewsType;
            if (newsType == undefined || newsType == null || newsType == "") {
                newsType = 0;
            }
            Service.post("/Subscribe/GetCateByNewsType", { newsType: newsType })
                .then(function (response) {
                    if (response.Success) {
                        $scope.InitModel.ListCategories = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        $scope.BrandChange = function () {
            var brandId = $scope.EditItem.BrandId;
            if (brandId == undefined || brandId == null || brandId == "") {
                brandId = 0;
            }
            Service.post("/Subscribe/GetModelByBrandId", { brandId: brandId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.InitModel.ListCarModels = response.Data;
                        $scope.SelectInit();
                    }
                });
        }

        //Get list credit card data
        $scope.getListData = function () {
            $scope.EditItem = {};
            $scope.SubscribeSearchModel.ListData = {};
            $scope.SubscribeSearchModel.PageIndex = $scope.page.currentPage;
            $scope.SubscribeSearchModel.PageSize = $scope.page.itemsPerPage;
            Service.post("/Subscribe/Search", { searchModel: $scope.SubscribeSearchModel })
                .then(function (response) {
                    if (response.Success) {
                        $scope.SubscribeSearchModel.ListData = response.Data;
                        $scope.page.totalItems = response.TotalRow;
                    }
                });
        };
        $scope.getListData();

        //Delete Subscribe
        $scope.delete = function (id) {
            if (confirm("Bạn muốn xóa đăng ký này?")) {
                Service.post("/Subscribe/Delete", { id: id })
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

        $scope.updatestatus = function (id, action) {
            var confirmMes = "";
            if (action == "ban")
                confirmMes = "Bạn muốn Ban email này?";
            else if (action == "unban")
                confirmMes = "Bạn muốn hủy Ban email này?";
            if (confirm(confirmMes)) {
                Service.post("/Subscribe/UpdateStatus", { id: id, action: action })
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

        }

        //Get subscribe to update
        $scope.edit = function (item) {
            if (item) {
                Service.post("/Subscribe/GetDetailBySubscribeId", { id: item.Id })
                    .then(function (response) {
                        if (response.Success) {
                            if (response.Data) {
                                $scope.EditItem = item;
                                $scope.EditItem.ListDetail = response.Data;
                                if ($scope.EditItem.ListDetail.length > 0) {
                                    if ($scope.EditItem.ListDetail[0].AllOfType == true) {
                                        $scope.EditItem.AllOfType = true;
                                    }
                                    else {
                                        $scope.EditItem.AllOfType = false;
                                    }
                                }
                            } else {
                                $scope.EditItem = {};
                            }
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
            $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
        }

        // Click button search
        $scope.doSearch = function () {
            $scope.page.currentPage = 1;
            $scope.getListData();
        }

        //Key press search
        $scope.keypressAction = function (keyEvent) {
            if (keyEvent.which === 13) {
                $scope.doSearch();
            }
        }

        $scope.roundNumberPaging = function (itemPerPage, totalItem) {
            return Math.ceil(parseFloat(totalItem) / itemPerPage);
        }

        $scope.backtolist = function () {
            $scope.CurrentFormStatus = $scope.FormStatusEnums.List;
            $scope.EditItem = {};
        }

    }]);
})();