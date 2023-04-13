(function () {
    app.controller('CustomerController', ['$scope', '$rootScope', '$http', 'Service', '$sce', 'notify', '$ngConfirm', function ($scope, $rootScope, $http, Service, $sce, notify, $ngConfirm) {

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

        $scope.SearchModel = {}
        $scope.ParamModel = {}


        $scope.CustomerModel = {};

        $scope.CurrentFormStatus = 0;
        $scope.FormStatusEnums = { List: 0, CreateOrUpdate: 1 };

        $scope.Init = function (obj) {
            if (obj) {
                $scope.ParamModel = obj;
                if ($scope.ParamModel.CustomerId > 0) {
                    $scope.GetCustomer($scope.ParamModel.CustomerId);
                }
            }

            $scope.getListData();
        };

        //Get list credit card data
        $scope.getListData = function () {
            $scope.SearchModel.ListData = {};
            $scope.SearchModel.PageIndex = $scope.page.currentPage;
            $scope.SearchModel.PageSize = $scope.page.itemsPerPage;

            $scope.SearchModel.StartDateStr = $('#txtStartDateStr').val();
            $scope.SearchModel.EndDateStr = $('#txtEndDateStr').val();
            Service.post("/Customer/Search", { searchModel: $scope.SearchModel })
                .then(function (response) {
                    if (response.Success) {
                        $scope.SearchModel.ListData = response.Data.ListData;
                        $scope.SearchModel.ListType = response.Data.ListType;
                        $scope.SearchModel.ListSource = response.Data.ListSource;
                        $scope.page.totalItems = response.TotalRow;
                    }
                });
        };

        //Get Customer
        $scope.GetCustomer = function (id) {
            if (id > 0) {
                Service.post("/Customer/GetCustomer", { CustomerId: id })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.CustomerModel = response.Data;
                            $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
                        }
                    });
            } else {
                $scope.CustomerModel = {};
                $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
            }
        }

        $scope.doUpdate = function (Customer) {
            if (Customer != null && Customer != undefined) {
                $scope.CustomerModel = angular.copy(Customer);
            }
            Service.post("/Customer/Update", { model: $scope.CustomerModel })
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

        //Delete Customer
        $scope.delete = function (id) {
            if (confirm("Bạn muốn xóa bản ghi này?")) {
                Service.post("/Customer/Delete", { id: id })
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

        //Get Customer to update
        $scope.editextend = function (Customer) {
            if (Customer) {
                $scope.CustomerModel = Customer;
                $scope.PushStateUrl($scope.CustomerModel.Id);
            } else {
                $scope.CustomerModel = {};
            }
            $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
            document.documentElement.scrollTop = 80;
            //$scope.closeToolTips();
        }

        $scope.exportExcel = function () {
            var query = "";
            var startDate = $("#txtStartDateStr").val();
            var endDate = $("#txtEndDateStr").val();
            if (startDate.length > 0) query += "&startDate=" + startDate;
            if (endDate.length > 0) query += "&endDate=" + endDate;
            window.open("/Customer/ExportExcel?" + query);
        }

        //Edit Customer on list
        $scope.edit = function (Customer) {
            $scope.cancelUpdate();
            $scope.CustomerModel = angular.copy(Customer);
            //$scope.closeToolTips();
        };

        //Get template
        $scope.getTemplate = function (Customer) {
            if (!Customer || $scope.CurrentFormStatus == $scope.FormStatusEnums.CreateOrUpdate) {
                return 'display';
            }
            if (Customer.Id === $scope.CustomerModel.Id) return 'edit';
            else return 'display';
        }

        //Close tooltips
        $scope.closeToolTips = function () {
            $('[data-toggle="tooltip"]').tooltip('hide');
        }

        //Rewrite URL
        $scope.PushStateUrl = function (id) {
            window.setTimeout(function () {
                window.history.pushState(null, null, "/Customer?CustomerId=" + id);
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
            $scope.CustomerModel = {};
            $scope.CurrentFormStatus = $scope.FormStatusEnums.List;
            window.setTimeout(function () {
                window.history.pushState(null, null, "/Customer");
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