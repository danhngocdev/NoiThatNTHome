(function () {
    app.controller('ContactController', ['$scope', '$http', 'Service', '$sce', 'notify', '$window', '$ngConfirm', function ($scope, $http, Service, $sce, notify, $window, $ngConfirm) {
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

        $scope.PriceListStatus = {
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


        $scope.InfoContact = {};
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
        };

        $scope.ListData = [];


        //Get list priceList data
        $scope.getListData = function () {
            $scope.Search.PageIndex = $scope.page.currentPage;
            $scope.Search.PageSize = $scope.page.itemsPerPage;
            Service.post("/Contact/Search", { searchModel: $scope.Search })
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
        $scope.updateStatus = function (contact) {
            $ngConfirm({
                title: cms.message.confirmWarningTitle,
                content: 'Bạn muốn tiếp nhận KH này?',
                type: cms.configs.ngConfirm.warningColor,
                scope: $scope,
                buttons: {
                    OK: {
                        type: cms.configs.ngConfirm.warningOKText,
                        btnClass: cms.configs.ngConfirm.warningOKClass,
                        keys: ['enter'],
                        action: function (scope, button) {
                            contact.item.Status = 1;
                            $scope.updateContact(contact.item, 1);
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


        //Update Status
        $scope.updateContact = function (contact, status) {
            Service.post("/Contact/Update", { contactModel: contact })
                .then(function (response) {
                    if (response.Success) {
                        $scope.message = "";
                        if (status === 1) {
                            $scope.message = "Thành Công";
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