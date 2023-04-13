(function () {
    app.controller('OrderController', ['$scope', '$rootScope', '$http', 'Service', '$sce', 'notify', '$ngConfirm', function ($scope, $rootScope, $http, Service, $sce, notify, $ngConfirm) {

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

        $scope.OrderStatus = {
            "New": 0,
            "InProgess": 1,
            "Done": 2
        };

        $scope.SearchModel = {}
        $scope.ParamModel = {}


        $scope.OrderModel = {};
        $scope.OrderDetailModel = {};

        $scope.CurrentFormStatus = 0;
        $scope.FormStatusEnums = { List: 0, CreateOrUpdate: 1 };

        $scope.Init = function (obj) {
            if (obj) {
                $scope.ParamModel = obj;
                if ($scope.ParamModel.OrderId > 0) {
                    $scope.GetOrder($scope.ParamModel.OrderId);
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
            Service.post("/Order/Search", { searchModel: $scope.SearchModel })
                .then(function (response) {
                    if (response.Success) {
                        $scope.SearchModel.ListData = response.Data.ListData;
                        $scope.SearchModel.ListStatus = response.Data.ListStatus;
                        $scope.SearchModel.ListPaymentStatus = response.Data.ListPaymentStatus;
                        $scope.SearchModel.ListPaymentType = response.Data.ListPaymentType;
                        $scope.page.totalItems = response.TotalRow;
                    }
                });
        };

        //Get Order
        $scope.GetOrder = function (id) {
            if (id > 0) {
                Service.post("/Order/GetOrder", { OrderId: id })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.OrderModel = response.Data;
                            $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
                        }
                    });
            } else {
                $scope.OrderModel = {};
                $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
            }
        }

        $scope.doUpdate = function (Order) {
            if (Order != null && Order != undefined) {
                $scope.OrderModel = angular.copy(Order);
            }
            Service.post("/Order/Update", { model: $scope.OrderModel })
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

        //Delete Order
        $scope.delete = function (id) {
            if (confirm("Bạn muốn xóa bản ghi này?")) {
                Service.post("/Order/Delete", { id: id })
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

        //Get Order to update
        $scope.editextend = function (Order) {
            if (Order) {
                $scope.OrderModel = Order;
                $scope.PushStateUrl($scope.OrderModel.Id);
            } else {
                $scope.OrderModel = {};
            }
            Service.post("/Order/GetOrderDetailByOrderID", { orderId: Order.Id })
                .then(function (response) {
                    if (response.Success) {
                        $scope.OrderDetailModel = response.Data;
                    } else {
                        notify({
                            message: "Lỗi! " + response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });
                    }
                });
            $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
            document.documentElement.scrollTop = 80;
        }

        $scope.exportExcel = function () {
            var query = "";
            var startDate = $("#txtStartDateStr").val();
            var endDate = $("#txtEndDateStr").val();
            if (startDate.length > 0) query += "&startDate=" + startDate;
            if (endDate.length > 0) query += "&endDate=" + endDate;
            window.open("/Order/ExportExcel?" + query);
        }

        //Edit Order on list
        $scope.edit = function (Order) {
            $scope.cancelUpdate();
            $scope.OrderModel = angular.copy(Order);
            //$scope.closeToolTips();
        };

        //Get template
        $scope.getTemplate = function (Order) {
            if (!Order || $scope.CurrentFormStatus == $scope.FormStatusEnums.CreateOrUpdate) {
                return 'display';
            }
            if (Order.Id === $scope.OrderModel.Id) return 'edit';
            else return 'display';
        }

        //Close tooltips
        $scope.closeToolTips = function () {
            $('[data-toggle="tooltip"]').tooltip('hide');
        }

        //Rewrite URL
        $scope.PushStateUrl = function (id) {
            window.setTimeout(function () {
                window.history.pushState(null, null, "/Order?OrderId=" + id);
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
            $scope.OrderModel = {};
            $scope.CurrentFormStatus = $scope.FormStatusEnums.List;
            window.setTimeout(function () {
                window.history.pushState(null, null, "/Order");
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