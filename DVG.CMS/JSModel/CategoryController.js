(function () {
    app.controller('CategoryController', ['$scope', '$http', 'Service', '$sce', 'notify', '$window', '$ngConfirm', function ($scope, $http, Service, $sce, notify, $window, $ngConfirm) {
        //function ($scope, $http, Service, $sce, notify) {

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

        $scope.CategoryStatus = {
            "Normal": 1,
            "Delete": 2,
            "Lock": 4
        };

        $scope.ParrentCategory = {
            'ListCategory': [],
            'CurrentCategory': 0
        };
        // Template for message
        $scope.template = '';
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

        $scope.Category = {};

        $scope.Search = {};

        $scope.InitModel = {};

        $scope.CurrentFormStatus = 0;
        $scope.FormStatusEnums = { List: 0, CreateOrUpdate: 1 };

        $scope.currentOrder = 0;

        $scope.Init = function (obj) {
            if (obj) {
                $scope.InitModel = obj;
                $scope.getListData();
                //Get list category for dropdown list
                var encryptCateId = getUrlParameter('encryptCateId');
                if (encryptCateId && encryptCateId.length > 0) {
                    $scope.getCategoryById(encryptCateId);
                }
            }
        };

        $scope.InitCreateOrUpdate = function () {
            CKEDITOR.on("instanceReady", function () {
            });
            $scope.getCategoryById('');
        };

        $scope.ListData = [];

        $scope.getCategoryById = function (encryptCateId) {
            if (encryptCateId.length > 0) pushStateUrl(encryptCateId);
            Service.post("/Category/GetByEncryptCateId", { encryptCateId: encryptCateId })
                .then(function (response) {
                    if (response.Success) {
                        if (response.Data) {
                            $scope.Category = response.Data;
                            $scope.ParrentCategory.CurrentCategory = response.Data.ParentId;
                            CKEDITOR.instances["editComment"].setData(response.Data.Description);
                            $scope.getListCategory();
                        }
                    } else {
                        notify({
                            message: "Lỗi! " + response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.positions.Center,
                            duration: 200000
                        });
                    }
                    $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
                    //$scope.InitUpdateForm();
                });
        }

        //Get list category data
        $scope.getListData = function () {
            $scope.Search.PageIndex = $scope.page.currentPage;
            $scope.Search.PageSize = $scope.page.itemsPerPage;
            Service.post("/Category/Search", { searchModel: $scope.Search })
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListData = response.Data;
                        $scope.page.totalItems = response.TotalRow;
                    }
                });
        };

        $scope.doSearch = function () {
            $scope.page.currentPage = 1;
            $scope.getListData();
        }

        $scope.onChangeData = function () {
            $scope.doSearch();
        }

        //Get current order of topic
        $scope.getCurrentOrder = function (order) {
            $scope.currentOrder = order;
        }

        //Change order
        $scope.onchangeOrder = function (category) {
            if ($scope.currentOrder !== category.item.SortOrder) {
                if (category.item.SortOrder === "") category.item.SortOrder = 0;
                Service.post("/Category/Update", { categoryModel: category.item })
                    .then(function (response) {
                        if (response.Success) {
                            notify({
                                message: "Đã cập nhật thứ tự",
                                classes: $scope.classes.Success,
                                position: $scope.positions.Center,
                                duration: $scope.duration
                            });
                            $scope.getListData();
                        }
                    });
            }
        }

        //Update status category
        $scope.updatestatus = function (category, status) {
            Service.post("/Category/Update", { categoryModel: category })
                .then(function (response) {
                    if (response.Success) {
                        $scope.message = "";
                        if (status === $scope.CategoryStatus.Delete) {
                            $scope.message = "Đã xóa chuyên mục";
                        } else if (status === $scope.CategoryStatus.Lock) {
                            $scope.message = "Đã khóa chuyên mục";
                        } else if (status === $scope.CategoryStatus.Normal) {
                            $scope.message = "Đã mở khóa chuyên mục";
                        }
                        notify({
                            message: $scope.message,
                            classes: $scope.classes.Success,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });
                        $scope.getListData();
                    } else {
                        notify({
                            message: "Lỗi! Cập nhật không thành công",
                            classes: $scope.classes.Success,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });
                    }
                });
        }

        //Lock category
        $scope.lockCategory = function (category) {
            $ngConfirm({
                title: cms.message.confirmWarningTitle,
                content: 'Bạn muốn khóa chuyên mục này?',
                type: cms.configs.ngConfirm.warningColor,
                scope: $scope,
                buttons: {
                    OK: {
                       type: cms.configs.ngConfirm.warningOKText,
                       btnClass: cms.configs.ngConfirm.warningOKClass,
                        keys: ['enter'],
                        action: function (scope, button) {
                            category.item.Status = $scope.CategoryStatus.Lock;
                            $scope.updatestatus(category.item, $scope.CategoryStatus.Lock);
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

        //Unlock category
        $scope.unlockCategory = function (category) {
            $ngConfirm({
                title: cms.message.confirmWarningTitle,
                content: 'Bạn muốn mở khóa chuyên mục này?',
                type: cms.configs.ngConfirm.warningColor,
                scope: $scope,
                buttons: {
                    OK: {
                       type: cms.configs.ngConfirm.warningOKText,
                       btnClass: cms.configs.ngConfirm.warningOKClass,
                        keys: ['enter'],
                        action: function (scope, button) {
                            category.item.Status = $scope.CategoryStatus.Normal;
                            $scope.updatestatus(category.item, $scope.CategoryStatus.Normal);
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

        //Delete category
        $scope.deleteCategory = function (category) {

            $ngConfirm({
                title: cms.message.confirmWarningTitle,
                content: 'Bạn muốn xóa chuyên mục này?',
                type: cms.configs.ngConfirm.warningColor,
                scope: $scope,
                buttons: {
                    OK: {
                       type: cms.configs.ngConfirm.warningOKText,
                       btnClass: cms.configs.ngConfirm.warningOKClass,
                        keys: ['enter'],
                        action: function (scope, button) {
                            category.item.Status = $scope.CategoryStatus.Delete;
                            $scope.updatestatus(category.item, $scope.CategoryStatus.Delete);
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

        //Update category
        $scope.updateCategory = function () {
            $scope.Category.Description = CKEDITOR.instances["editComment"].getData();
            $scope.Category.ParentId = $scope.ParrentCategory.CurrentCategory;
            if ($scope.Category.SortOrder == "" || $scope.Category.SortOrder == null) $scope.Category.SortOrder = 0;
            Service.post("/Category/Update", { categoryModel: $scope.Category })
                .then(function (response) {
                    if (response.Success) {
                        if ($scope.Category.Id > 0) {
                            notify({
                                message: "Cập nhật thành công",
                                classes: $scope.classes.Success,
                                templateUrl: $scope.template,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                        } else {
                            notify({
                                message: "Thêm thành công",
                                classes: $scope.classes.Success,
                                templateUrl: $scope.template,
                                position: $scope.position,
                                duration: $scope.duration
                            });
                        }
                        window.setTimeout(function () {
                            $scope.getListData();
                        }, $scope.duration);
                    } else {
                        notify({
                            message: "Lỗi! Cập nhật không thành công",
                            classes: $scope.classes.Error,
                            templateUrl: $scope.template,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                    }
                });
        };

        //Get list topic for combobox
        $scope.getListCategory = function () {
            var newsType = $scope.Category.Type;
            if (newsType == undefined || newsType == null || newsType == "") {
                newsType = 0;
            }
            Service.post("/Category/GetListCategoryByType", { newsType: newsType })
                .then(function (response) {
                    if (response.Success) {
                        $scope.ParrentCategory.ListCategory = response.Data;
                    }
                });
        }

        $scope.cancleUpdate = function () {
            $scope.getListData();
            $scope.CurrentFormStatus = $scope.FormStatusEnums.List;
            $(window).scrollTop(0);
        }

        //Key press
        $scope.keypressAction = function (keyEvent) {
            if (keyEvent.which === 13) {
                $scope.getListData();
            }
        }

        // html filter (render text as html)
        $scope.trustedHtml = function (plainText) {
            return $sce.trustAsHtml(plainText);
        }

        $scope.roundNumberPaging = function (itemPerPage, totalItem) {
            return Math.ceil(parseFloat(totalItem) / itemPerPage);
        }

        var pushStateUrl = function (encryptCateId) {
            //Rawparam("/encryptCateId=", encryptCateId);
            //$window.history.pushState(null, null, "/Category?encryptCateId=" + encryptCateId);
            window.setTimeout(function () {
                $window.history.pushState(null, null, "/Category?encryptCateId=" + encryptCateId);
            }, $scope.duration);
        }


        var getUrlParameter = function (sParam) {
            var sPageURL = decodeURIComponent(window.location.search.substring(1)),
                sURLVariables = sPageURL.split('&'),
                sParameterName,
                i;

            for (i = 0; i < sURLVariables.length; i++) {
                sParameterName = sURLVariables[i].split('=');

                if (sParameterName[0] === sParam) {
                    return sParameterName[1] === undefined ? true : sParameterName[1];
                }
            }
        };
    }]);
})();

$(document).ready(function () {
    InitSEO();
});