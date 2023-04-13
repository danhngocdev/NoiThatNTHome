(function () {
    app.controller('VideoController', ['$scope', '$http', 'Service', '$sce', 'notify', '$window', '$ngConfirm', function ($scope, $http, Service, $sce, notify, $window, $ngConfirm) {
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

        $scope.VideoStatus = {
            "Active": 1,
            "NonActive": 0,

        };

        // $scope.ParrentCategory = {
        //     'ListCategory': [],
        //     'CurrentCategory': 0
        // };
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

        $scope.Video = {};
        $scope.ParamModel = {}

        $scope.Search = {};

        $scope.InitModel = {};

        $scope.CurrentFormStatus = 0;
        $scope.FormStatusEnums = { List: 0, CreateOrUpdate: 1 };

        $scope.currentOrder = 0;

        $scope.Init = function (obj) {
            if (obj) {
                $scope.InitModel = obj;
                $scope.getListData();
                // //Get list category for dropdown list
                // var encryptCateId = getUrlParameter('encryptCateId');
                // if (encryptCateId && encryptCateId.length > 0) {
                //     $scope.getCategoryById(encryptCateId);
                // }
            }
        };

        $scope.InitCreateOrUpdate = function () {
            CKEDITOR.on("instanceReady", function () {
            });
            $scope.getVideoById(0);
        };

        $scope.ListData = [];

        $scope.getVideoById = function (Id) {
            Service.post("/Video/GetVideoById", { Id: Id })
                .then(function (response) {
                    if (response.Success) {
                        if (response.Data) {
                            console.log(response.Data, 99);
                            $scope.Video = response.Data;
                            $scope.Video.ListStatus = response.Data.ListStatus;
                               console.log(response.Data, 99);
                            //$scope.ParrentCategory.CurrentCategory = response.Data.ParentId;
                            //CKEDITOR.instances["editComment"].setData(response.Data.Description);
                            //$scope.getListCategory();
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

        //Get list priceList data
        $scope.getListData = function () {
            $scope.Search.PageIndex = $scope.page.currentPage;
            $scope.Search.PageSize = $scope.page.itemsPerPage;
            Service.post("/Video/Search", { searchModel: $scope.Search })
                .then(function (response) {
                    if (response.Success) {
                        console.log(response, 88);
                        $scope.ListData = response.Data.ListData;
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

        // //Change order
        // $scope.onchangeOrder = function (category) {
        //     if ($scope.currentOrder !== category.item.SortOrder) {
        //         if (category.item.SortOrder === "") category.item.SortOrder = 0;
        //         Service.post("/Category/Update", { categoryModel: category.item })
        //             .then(function (response) {
        //                 if (response.Success) {
        //                     notify({
        //                         message: "Đã cập nhật thứ tự",
        //                         classes: $scope.classes.Success,
        //                         position: $scope.positions.Center,
        //                         duration: $scope.duration
        //                     });
        //                     $scope.getListData();
        //                 }
        //             });
        //     }
        // }

        ////Update status category
        //$scope.updatestatus = function (pricelist, status) {
        //    Service.post("/PriceList/Update", { categoryModel: pricelist })
        //        .then(function (response) {
        //            if (response.Success) {
        //                $scope.message = "";
        //                if (status === $scope.CategoryStatus.Delete) {
        //                    $scope.message = "Đã xóa chuyên mục";
        //                } else if (status === $scope.CategoryStatus.Lock) {
        //                    $scope.message = "Đã khóa chuyên mục";
        //                } else if (status === $scope.CategoryStatus.Normal) {
        //                    $scope.message = "Đã mở khóa chuyên mục";
        //                }
        //                notify({
        //                    message: $scope.message,
        //                    classes: $scope.classes.Success,
        //                    position: $scope.positions.Center,
        //                    duration: $scope.duration
        //                });
        //                $scope.getListData();
        //            } else {
        //                notify({
        //                    message: "Lỗi! Cập nhật không thành công",
        //                    classes: $scope.classes.Success,
        //                    position: $scope.positions.Center,
        //                    duration: $scope.duration
        //                });
        //            }
        //        });
        //}

        ////Lock category
        //$scope.lockCategory = function (category) {
        //    $ngConfirm({
        //        title: cms.message.confirmWarningTitle,
        //        content: 'Bạn muốn khóa chuyên mục này?',
        //        type: cms.configs.ngConfirm.warningColor,
        //        scope: $scope,
        //        buttons: {
        //            OK: {
        //               type: cms.configs.ngConfirm.warningOKText,
        //               btnClass: cms.configs.ngConfirm.warningOKClass,
        //                keys: ['enter'],
        //                action: function (scope, button) {
        //                    category.item.Status = $scope.CategoryStatus.Lock;
        //                    $scope.updatestatus(category.item, $scope.CategoryStatus.Lock);
        //                }
        //            },
        //            Cancel: {
        //               type: cms.configs.ngConfirm.warningCancelText,
        //               btnClass: cms.configs.ngConfirm.warningCancelClass,
        //                keys: ['esc'],
        //                action: function (scope, button) {
        //                }
        //            },
        //        }
        //    });
        //}

        //Unlock category
        //$scope.unlockCategory = function (category) {
        //    $ngConfirm({
        //        title: cms.message.confirmWarningTitle,
        //        content: 'Bạn muốn mở khóa chuyên mục này?',
        //        type: cms.configs.ngConfirm.warningColor,
        //        scope: $scope,
        //        buttons: {
        //            OK: {
        //                type: cms.configs.ngConfirm.warningOKText,
        //                btnClass: cms.configs.ngConfirm.warningOKClass,
        //                keys: ['enter'],
        //                action: function (scope, button) {
        //                    category.item.Status = $scope.CategoryStatus.Normal;
        //                    $scope.updatestatus(category.item, $scope.CategoryStatus.Normal);
        //                }
        //            },
        //            Cancel: {
        //                type: cms.configs.ngConfirm.warningCancelText,
        //                btnClass: cms.configs.ngConfirm.warningCancelClass,
        //                keys: ['esc'],
        //                action: function (scope, button) {
        //                }
        //            },
        //        }
        //    });
        //}

        //Delete PriceList
        $scope.deleteVideo = function (id) {
            console.log(id);
            $ngConfirm({
                title: cms.message.confirmWarningTitle,
                content: 'Bạn muốn xóa bản ghi này?',
                type: cms.configs.ngConfirm.warningColor,
                scope: $scope,
                buttons: {
                    OK: {
                        type: cms.configs.ngConfirm.warningOKText,
                        btnClass: cms.configs.ngConfirm.warningOKClass,
                        keys: ['enter'],
                        action: function (scope, button) {
                            Service.post("/Video/Delete", { id: id })
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


        //Update Video
        $scope.updateVideo = function () {
            Service.post("/Video/Update", { video: $scope.Video })
                .then(function (response) {
                    if (response.Success) {
                        if ($scope.Video.Id > 0) {
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

        // var pushStateUrl = function (encryptCateId) {
        //     //Rawparam("/encryptCateId=", encryptCateId);
        //     //$window.history.pushState(null, null, "/Category?encryptCateId=" + encryptCateId);
        //     window.setTimeout(function () {
        //         $window.history.pushState(null, null, "/Category?encryptCateId=" + encryptCateId);
        //     }, $scope.duration);
        // }


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

// $(document).ready(function () {
//     InitSEO();
// });