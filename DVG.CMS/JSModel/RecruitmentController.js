(function () {
    app.controller('RecruitmentController', ['$scope', '$rootScope', '$http', 'Service', '$sce', 'notify', '$ngConfirm', function ($scope, $rootScope, $http, Service, $sce, notify, $ngConfirm) {

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

        $scope.RecruitmentStatus = [
            { "Id": 1, "Name": "Hoạt động" },
            { "Id": 0, "Name": "Không hoạt động" }
        ];

        $scope.BannerStatusEnum = {
            "Show": 1,
            "Hide": 0,
        };


        $scope.SearchModel = {}
        $scope.ParamModel = {}


        $scope.RecruitmentModel = {};

        $scope.CurrentFormStatus = 0;
        $scope.FormStatusEnums = { List: 0, CreateOrUpdate: 1 };

        $scope.Init = function (obj) {
            if (obj) {
                $scope.ParamModel = obj;
                if ($scope.ParamModel.RecruitmentId > 0) {
                    $scope.GetRecruitment($scope.ParamModel.RecruitmentId);
                }
            }

            $scope.getListData();
        };

        //Get list credit card data
        $scope.getListData = function () {
            $scope.SearchModel.ListData = {};
            $scope.SearchModel.PageIndex = $scope.page.currentPage;
            $scope.SearchModel.PageSize = $scope.page.itemsPerPage;
            Service.post("/Recruitment/Search", { searchModel: $scope.SearchModel })
                .then(function (response) {
                    if (response.Success) {
                        $scope.SearchModel.ListData = response.Data.ListData;
                        $scope.page.totalItems = response.TotalRow;
                    }
                });
        };

        //Get Recruitment
        $scope.GetRecruitment = function (id) {
            if (id > 0) {
                Service.post("/Recruitment/GetRecruitment", { RecruitmentId: id })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.RecruitmentModel = response.Data;
                            $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
                            CKEDITOR.instances["txtDescription"].setData($scope.RecruitmentModel.Description);
                        }
                    });
            } else {
                $scope.RecruitmentModel = {};
                $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
                CKEDITOR.instances["txtDescription"].setData('');
            }
        }

        $scope.doUpdate = function (Recruitment) {
            if (Recruitment != null && Recruitment != undefined) {
                $scope.RecruitmentModel = angular.copy(Recruitment);
            }
            $scope.RecruitmentModel.Description = CKEDITOR.instances["txtDescription"].getData();
            $scope.RecruitmentModel.EndDateStr = $('#txtEndDateStr').val();
            Service.post("/Recruitment/Update", { model: $scope.RecruitmentModel })
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

        //Delete Recruitment
        $scope.delete = function (id) {
            if (confirm("Bạn muốn xóa bản ghi này?")) {
                Service.post("/Recruitment/Delete", { id: id })
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

        //Get Recruitment to update
        $scope.editextend = function (Recruitment) {
            if (Recruitment) {
                $scope.RecruitmentModel = Recruitment;
                $scope.PushStateUrl($scope.RecruitmentModel.Id);
                window.setTimeout(function () {
                    CKEDITOR.instances["txtDescription"].setData($scope.RecruitmentModel.Description);
                }, 200);
            } else {
                $scope.RecruitmentModel = {};
                CKEDITOR.instances["txtDescription"].setData('');
            }
            $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
            document.documentElement.scrollTop = 80;
            //$scope.closeToolTips();
        }

        //Edit Recruitment on list
        $scope.edit = function (Recruitment) {
            $scope.cancelUpdate();
            $scope.RecruitmentModel = angular.copy(Recruitment);
            //$scope.closeToolTips();
        };

        //Get template
        $scope.getTemplate = function (Recruitment) {
            if (!Recruitment || $scope.CurrentFormStatus == $scope.FormStatusEnums.CreateOrUpdate) {
                return 'display';
            }
            if (Recruitment.Id === $scope.RecruitmentModel.Id) return 'edit';
            else return 'display';
        }

        //Close tooltips
        $scope.closeToolTips = function () {
            $('[data-toggle="tooltip"]').tooltip('hide');
        }

        //Rewrite URL
        $scope.PushStateUrl = function (id) {
            window.setTimeout(function () {
                window.history.pushState(null, null, "/Recruitment?RecruitmentId=" + id);
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
            $scope.RecruitmentModel = {};
            $scope.CurrentFormStatus = $scope.FormStatusEnums.List;
            window.setTimeout(function () {
                window.history.pushState(null, null, "/Recruitment");
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