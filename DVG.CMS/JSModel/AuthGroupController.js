(function () {
    app.controller('AuthGroupController', ['$scope', '$http', 'Service', 'notify', '$ngConfirm', function ($scope, $http, Service, notify, $ngConfirm) {
        // Position of message
        $scope.positions = ['center', 'left', 'right'];
        // Class of message
        $scope.classes = {
            "Success": 'alert-success',
            "Error": 'alert-danger'
        };
        $scope.position = $scope.positions[0];
        // The time display of message
        $scope.duration = 2000;
        $scope.CurrentFormStatus = 0;
        $scope.FormStatusEnums = { List: 0, Create: 1, Update: 2 };
        $scope.InitModel = {};
        $scope.ListData = [];
        $scope.ListCategoryModel = [];
        $scope.ListAuthAction = [];
        $scope.ListAuthActionSelected = [];
        $scope.ListNewsStatus = [];
        $scope.ListNewsStatusSelected = [];
        $scope.AuthGroup = {};
        $scope.ListAuthGroupStatus = [];
        $scope.AuthGroupView = {};

        $scope.Init = function (obj) {
            if (obj) {
                angular.copy(obj, $scope.InitModel);
                $scope.ListCategoryModel = obj.LstCategoryModel;
                $scope.ListAuthAction = obj.LstAuthAction;
                $scope.ListNewsStatus = obj.ListNewsStatus;
                $scope.ListAuthGroupStatus = obj.LstAuthGroupStatus;
                $scope.AuthGroupView.Status = obj.LstAuthGroupStatus[1];
            }
            $scope.getListData();
        };

        $scope.getListData = function () {

            Service.post("/AuthGroup/GetAll")
                .then(function (response) {
                    if (response && response.Success) {
                        $scope.ListData = response.Data;
                    }
                });
        };

        $scope.bindCreate = function () {
            $scope.CurrentFormStatus = $scope.FormStatusEnums.Create;
            resetInitData();
        }

        $scope.bindUpdate = function (id) {
            $scope.CurrentFormStatus = $scope.FormStatusEnums.Update;
            resetInitData();
            Service.post("/AuthGroup/GetById", { id: id })
                .then(function (response) {
                    if (response && response.Success && response.Data) {
                        bindDataViewForm(response.Data);
                    }
                });
        }

        $scope.myClick = function (node) {

        };

        $scope.CreateOrUpdate = function () {
            if ($scope.AuthGroup && $scope.AuthGroup.Id > 0)
                $scope.doUpdate();
            else $scope.doCreate();
        }

        $scope.doCreate = function () {
            if ($scope.AuthGroupForm.AuthGroup_Name.$valid) {
                var data = {};
                data.AuthGroup = $scope.AuthGroup;
                data.LstCategoryModel = $scope.ListCategoryModel;
                data.LstAuthAction = $scope.ListAuthActionSelected;
                data.LstNewsStatus = $scope.ListNewsStatusSelected;
                data.AuthGroup.Status = $scope.AuthGroupView.Status.Id;
                Service.post("/AuthGroup/Create", { model: data })
                    .then(function (response) {
                        if (response && response.Success) {
                            notify({
                                message: "Tạo mới thành công",
                                classes: $scope.classes.Success,
                                templateUrl: $scope.template,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                            $scope.getListData();
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
        }

        $scope.doUpdate = function () {
            var data = {};
            data.AuthGroup = $scope.AuthGroup;
            data.LstAuthAction = $scope.ListAuthActionSelected;
            data.LstNewsStatus = $scope.ListNewsStatusSelected;
            data.LstCategoryModel = $scope.ListCategoryModel;
            data.AuthGroup.Status = $scope.AuthGroupView.Status.Id;
            Service.post("/AuthGroup/Update", { model: data })
                .then(function (response) {
                    if (response && response.Success) {
                        notify({
                            message: "Cập nhật thành công",
                            classes: $scope.classes.Success,
                            templateUrl: $scope.template,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                        $scope.getListData();
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

        $scope.doDelete = function (id) {
            if (id > 0) {
                $ngConfirm({
                    title: cms.message.confirmWarningTitle,
                    content: 'Bạn có chắc muốn xóa?',
                    type: cms.configs.ngConfirm.warningColor,
                    scope: $scope,
                    buttons: {
                        OK: {
                           type: cms.configs.ngConfirm.warningOKText,
                           btnClass: cms.configs.ngConfirm.warningOKClass,
                            keys: ['enter'],
                            action: function (scope, button) {
                                Service.post("/AuthGroup/Delete", { id: id })
                                    .then(function (response) {
                                        if (response && response.Success) {
                                            notify({
                                                message: "Xóa thành công",
                                                classes: $scope.classes.Success,
                                                templateUrl: $scope.template,
                                                position: $scope.position,
                                                duration: $scope.duration
                                            });
                                            $scope.getListData();
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
        }

        $scope.doChangeStatus = function (id) {
            if (id > 0) {
                $ngConfirm({
                    title: cms.message.confirmWarningTitle,
                    content: 'Bạn có chắc muốn đổi trạng thái?',
                    type: cms.configs.ngConfirm.warningColor,
                    scope: $scope,
                    buttons: {
                        OK: {
                           type: cms.configs.ngConfirm.warningOKText,
                           btnClass: cms.configs.ngConfirm.warningOKClass,
                            keys: ['enter'],
                            action: function (scope, button) {
                                Service.post("/AuthGroup/ChangeStatus", { id: id })
                                    .then(function (response) {
                                        if (response && response.Success) {
                                            notify({
                                                message: "Đổi trạng thái thành công",
                                                classes: $scope.classes.Success,
                                                templateUrl: $scope.template,
                                                position: $scope.position,
                                                duration: $scope.duration
                                            });
                                            $scope.getListData();
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
        }

        $scope.cancelCreateOrUpdate = function () {
            $scope.CurrentFormStatus = $scope.FormStatusEnums.List;
        }

        $scope.chooseAction = function (obj) {
            if (obj.Checked === true) {
                $scope.ListAuthActionSelected.pushUnique(obj);
            }
            else {
                var mappedSelectedList = $scope.ListAuthActionSelected.map(function (e) {
                    return e.Id;
                });
                var index = mappedSelectedList.indexOf(obj.Id);
                if (index >= 0)
                    $scope.ListAuthActionSelected.splice(index, 1);
            }
        }

        $scope.chooseNewsStatus = function (obj) {
            if (obj.Checked === true) {
                $scope.ListNewsStatusSelected.pushUnique(obj);
            }
            else {
                var mappedSelectedList = $scope.ListNewsStatusSelected.map(function (e) {
                    return e.Id;
                });
                var index = mappedSelectedList.indexOf(obj.Id);
                if (index >= 0)
                    $scope.ListNewsStatusSelected.splice(index, 1);
            }
        }

        var bindDataViewForm = function (obj) {
            $scope.AuthGroup = obj.AuthGroup;
            for (i = 0; i < $scope.ListAuthGroupStatus.length; i++) {
                if ($scope.ListAuthGroupStatus[i].Id == obj.AuthGroup.Status)
                    $scope.AuthGroupView.Status = $scope.ListAuthGroupStatus[i];
            }
            $scope.ListCategoryModel = obj.LstSeletedCategoryModel;
            $scope.ListAuthAction = obj.LstSeletedActionModel;
            for (i = 0; i < obj.LstSeletedActionModel.length; i++) {
                if (obj.LstSeletedActionModel[i].Checked)
                    $scope.ListAuthActionSelected.pushUnique(obj.LstSeletedActionModel[i]);
            }

            $scope.ListNewsStatus = obj.LstSeletedNewsStatusModel;
            for (i = 0; i < obj.LstSeletedNewsStatusModel.length; i++) {
                if (obj.LstSeletedNewsStatusModel[i].Checked)
                    $scope.ListNewsStatusSelected.pushUnique(obj.LstSeletedNewsStatusModel[i]);
            }
        }

        var resetInitData = function () {
            $scope.AuthGroup = {};
            $scope.ListAuthActionSelected = [];
            $scope.ListNewsStatusSelected = [];
            var obj = {}
            angular.copy($scope.InitModel, obj);
            $scope.ListCategoryModel = obj.LstCategoryModel;
            $scope.ListAuthAction = obj.LstAuthAction;
            $scope.ListAuthGroupStatus = obj.LstAuthGroupStatus;
            $scope.AuthGroupView.Status = obj.LstAuthGroupStatus[1];
            $scope.ListNewsStatus = obj.LstNewsStatus;
        }
    }]);
})();