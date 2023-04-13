(function () {
    app.controller('ProductController', ['$scope', '$http', 'Service', '$sce', 'notify', '$ngConfirm', function ($scope, $http, Service, $sce, notify, $ngConfirm) {
        $scope.page = {
            'totalItems': 0,
            'currentPage': 1,
            'itemsPerPage': 15
        };

        $scope.ProductStatus = {
            "PendingApproved": 1,
            "Published": 2,
            "UnPublished": 3,
            "Deleted": 4
        }

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
        $scope.durationMax = 1000 * 60 * 60 * 24;

        $scope.Search = {};
        $scope.SearchExportExcel = {};
        $scope.StatusOfProductPermission = {};
        $scope.IsPreviewForm = false;
        $scope.currentOrder = 0;

        $scope.ProductDisplayPosition = {
            "Normal": 1,
            "Highlight": 2,
            "HighlightOnCategory": 3
        }

        //$scope.AutoSave = {
        //    HasDraft: false,
        //    Params: ""
        //};

        $scope.ViewDataModel = {
            'EncryptProductId': ''
        };

        $scope.Init = function (obj) {
            if (obj) {
                //$scope.Search.Status = obj.Status;
                //$scope.Search.DisplayPosition = obj.DisplayPosition;
                //$scope.Search.DisplayStyle = obj.DisplayStyle;
                //$scope.Search.ListDisplayStyle = obj.ListDisplayStyle;
                //$scope.Search.TagsName = obj.TagsName;

                //$scope.Search.TypeOfProduct = obj.TypeOfProduct;
                //$scope.Search.PositionOfProduct = obj.PositionOfProduct;
                //$scope.Search.ListProductType = obj.ListProductType;
                //$scope.Search.ProductType = obj.ProductType;
                //$scope.Search.ListProductStatus = obj.ListProductStatus;
                //$scope.Search.Status = obj.Status;
                //$scope.Search.ListUserOnControll = obj.ListUserOnControll;

                $scope.Search.ListStatus = obj.ListStatus;
                //$scope.Search.CategoryId = obj.CateId;
                //$scope.Search.StatusOfProduct = obj.StatusOfProduct;
                $scope.StatusOfProductPermission = obj.StatusOfProductPermission;
                // check autosave
                //if (cms.configs.ProductAutoSave.Enable && obj.ProductAutoSave != null && obj.ProductAutoSave.HasDraft) {
                //    $scope.AutoSave.HasDraft = true;
                //    if (obj.ProductAutoSave.Id > 0) $scope.AutoSave.Params = "ProductId=" + obj.ProductAutoSave.EncryptProductId + "&";
                //    $scope.AutoSave.Title = obj.ProductAutoSave.Title;
                //}

                //GetListCountProductByStatus(obj.ProductType);
            }

            $scope.getListData();
            //$scope.getListCategory();
        };

        $scope.ListData = [];

        $scope.ActionHistory = [];

        //Get list category data
        $scope.getListData = function () {
            $scope.Search.PageIndex = $scope.page.currentPage;
            $scope.Search.PageSize = $scope.page.itemsPerPage;
            Service.post("/Product/Search", { searchModel: $scope.Search })
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListData = response.Data.ListData;
                        if ($scope.ListData != null && $scope.ListData.length > 0) {
                            $scope.page.totalItems = response.TotalRow;
                        } else {
                            $scope.page.totalItems = 0;
                        }
                    }
                    $("#indexProduct").removeClass("hidden");
                });
        };

        //Init chosen for select option
        $scope.SelectInit = function () {
            setTimeout(function () {
                $(".chosen-select").chosen();
                $('.chosen-select').trigger('chosen:updated');
            }, 200);
        }

        //on change Product type
        $scope.onChangeProductType = function () {
            $scope.doSearch();
            $scope.getListCategory();
        }

        $scope.roundNumberPaging = function (itemPerPage, totalItem) {
            return Math.ceil(parseFloat(totalItem) / itemPerPage);
        }

        $scope.getListCategory = function () {
            if ($scope.Search.ProductType) {
                Service.post("/Category/GetListCategoryByType", { ProductType: $scope.Search.ProductType })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.Search.ListCate = response.Data;
                        }
                    });
            } else {
                $scope.Search.ListCate = [];
            }
        }

        //Xuất bản
        $scope.doPublish = function (ProductId) {
            if (ProductId.length > 0) {
                ngConfirm(doPublish, ProductId, cms.message.confirmDoPublish);
            }
        }
        var doPublish = function (ProductId) {
            Service.post("/Product/DoPublish", { ProductId: ProductId })
                .then(function (response) {
                    if (response.Success) {
                        notify({
                            message: cms.message.itemPublished,
                            classes: $scope.classes.Success,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                        $scope.getListData();
                    } else {
                        notify({
                            message: response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                    }
                });
        }

        //Gửi biên tập
        $scope.doPending = function (ProductId) {
            if (ProductId.length > 0) {
                ngConfirm(doPending, ProductId, cms.message.confirmDoPending);
            }
        }
        var doPending = function (ProductId) {
            Service.post("/Product/DoPending", { ProductId: ProductId })
                .then(function (response) {
                    if (response.Success) {
                        notify({
                            message: cms.message.itemPending,
                            classes: $scope.classes.Success,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                        $scope.getListData();
                    } else {
                        notify({
                            message: response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                    }
                });
        }

        //Nhận biên tập
        $scope.doRecivedEdit = function (ProductId) {
            if (ProductId.length > 0) {
                ngConfirm(doRecivedEdit, ProductId, cms.message.confirmDoRecivedEdit);
            }
        }
        var doRecivedEdit = function (ProductId) {
            Service.post("/Product/DoRecivedEdit", { ProductId: ProductId })
                .then(function (response) {
                    if (response.Success) {
                        notify({
                            message: cms.message.itemRecivedEdit,
                            classes: $scope.classes.Success,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                        $scope.getListData();
                    } else {
                        notify({
                            message: response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                    }
                });
        }

        //Gửi duyệt
        $scope.doPendingApprove = function (ProductId) {
            if (ProductId.length > 0) {
                ngConfirm(doPendingApprove, ProductId, cms.message.confirmDoPendingApprove);
            }
        }
        var doPendingApprove = function (ProductId) {
            Service.post("/Product/DoPendingApprove", { ProductId: ProductId })
                .then(function (response) {
                    if (response.Success) {
                        notify({
                            message: cms.message.itemPendingApprove,
                            classes: $scope.classes.Success,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                        $scope.getListData();
                    } else {
                        notify({
                            message: response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                    }
                });
        }

        //Nhận duyệt
        $scope.doRecivedApprove = function (ProductId) {
            if (ProductId.length > 0) {
                ngConfirm(doRecivedApprove, ProductId, cms.message.confirmDoRecivedApprove);
            }
        }
        var doRecivedApprove = function (ProductId) {
            Service.post("/Product/DoRecivedApprove", { ProductId: ProductId })
                .then(function (response) {
                    if (response.Success) {
                        notify({
                            message: cms.message.itemRecivedApprove,
                            classes: $scope.classes.Success,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                        $scope.getListData();
                    } else {
                        notify({
                            message: response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                    }
                });
        }

        //Trả bài
        $scope.doReturn = function (ProductId) {
            if (ProductId.length > 0) {
                ngConfirm(doReturn, ProductId, cms.message.confirmDoReturn);
            }
        }
        var doReturn = function (ProductId) {
            Service.post("/Product/DoReturn", { ProductId: ProductId })
                .then(function (response) {
                    if (response.Success) {
                        notify({
                            message: cms.message.itemRetuned,
                            classes: $scope.classes.Success,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                        $scope.getListData();
                    } else {
                        notify({
                            message: response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                    }
                });
        }

        //Gỡ bài
        $scope.doUnPublish = function (ProductId) {
            if (ProductId.length > 0) {
                ngConfirm(doUnPublish, ProductId, cms.message.confirmDoUnPublish);
            }
        }
        var doUnPublish = function (ProductId) {
            Service.post("/Product/DoUnPublish", { ProductId: ProductId })
                .then(function (response) {
                    if (response.Success) {
                        notify({
                            message: cms.message.itemUnPublished,
                            classes: $scope.classes.Success,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                        $scope.getListData();
                    } else {
                        notify({
                            message: response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                    }
                });
        }

        //Xóa bài
        $scope.doDelete = function (ProductId) {
            if (ProductId.length > 0) {
                ngConfirm(doDelete, ProductId, cms.message.confirmDoDelete);
            }
        }
        var doDelete = function (ProductId) {
            Service.post("/Product/DoDelete", { ProductId: ProductId })
                .then(function (response) {
                    if (response.Success) {
                        notify({
                            message: cms.message.itemDeleted,
                            classes: $scope.classes.Success,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                        $scope.getListData();
                    } else {
                        notify({
                            message: response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                    }
                });
        }

        $scope.getProductById = function (ProductId) {
            Service.post("/Product/ProductView", { ProductId: ProductId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.Product = response.Data;
                        $scope.IsPreviewForm = true;
                        setTimeout(function () {
                            VideoLibrary.Register();
                        }, 1000);
                    }
                    else {
                        notify({
                            message: response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                    }
                });
        }

        $scope.canclePreview = function () {
            $scope.IsPreviewForm = false;
        }

        //Get current order of topic
        $scope.getCurrentOrder = function (order) {
            $scope.currentOrder = order;
        }

        //Update order position Product HightLight
        $scope.onchangeOrderProductHightLight = function (ProductObject) {
            if (isNaN(ProductObject.item.Ordinal) || parseInt(ProductObject.item.Ordinal) < 0) {
                notify({
                    message: "Số thứ tự phải là số nguyên dương.",
                    classes: $scope.classes.Error,
                    position: $scope.positions.Center,
                    duration: $scope.duration
                });
                ProductObject.item.Ordinal = $scope.currentOrder;
            } else {
                if ($scope.currentOrder !== ProductObject.item.Ordinal) {
                    if (ProductObject.item.Ordinal === "") ProductObject.item.Ordinal = 0;
                    Service.post("/Product/UpdateOrderProduct ", { encryptId: ProductObject.item.EncryptProductId, ordinal: ProductObject.item.Ordinal, position: $scope.ProductDisplayPosition.Highlight, currentPosition: ProductObject.item.DisplayPosition })
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
        }

        //Update order position Product HightLight
        $scope.onchangeOrderProductHightLightOnCategory = function (ProductObject) {
            if (isNaN(ProductObject.item.Ordinal) || parseInt(ProductObject.item.Ordinal) < 0) {
                notify({
                    message: "Số thứ tự phải là số nguyên dương.",
                    classes: $scope.classes.Error,
                    position: $scope.positions.Center,
                    duration: $scope.duration
                });
                ProductObject.item.Ordinal = $scope.currentOrder;
            } else {
                if ($scope.currentOrder !== ProductObject.item.Ordinal) {
                    if (ProductObject.item.Ordinal === "") ProductObject.item.Ordinal = 0;
                    Service.post("/Product/UpdateOrderProduct ", { encryptId: ProductObject.item.EncryptProductId, ordinal: ProductObject.item.Ordinal, position: $scope.ProductDisplayPosition.HighlightOnCategory, currentPosition: ProductObject.item.DisplayPosition })
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
        }

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

        // html filter (render text as html)
        $scope.trustedHtml = function (plainText) {
            return $sce.trustAsHtml(plainText);
        }

        $scope.CateChange = function () {
            $scope.doSearch();
        }

        var GetListCountProductByStatus = function (ProductType) {
            setTimeout(function () {
                Service.post("/Product/GetListCountProductByStatus", { ProductType: ProductType })
                    .then(function (response) {
                        if (response.Success) {
                            for (i = 0; i < response.Data.length; i++) {
                                if (response.Data[i].Count > 0)
                                    $('.countProduct-' + ProductType + '-' + response.Data[i].Status).append(' (<b class="text-danger">' + response.Data[i].Count + '</b>)');
                            }
                        }
                    });
            }, 3000);
        }

        $scope.getProductTypeName = function (ProductType) {
            return getProductTypeName(ProductType);
        }

        $scope.getLinkUrl = function (url) {
            prompt("Nhấn Ctrl + C để sao chép nội dung", url);
        };

        var ngConfirm = function (callback, ProductId, message) {
            $ngConfirm({
                title: cms.message.confirmWarningTitle,
                content: message,
                type: cms.configs.ngConfirm.warningColor,
                scope: $scope,
                buttons: {
                    OK: {
                        type: cms.configs.ngConfirm.warningOKText,
                        btnClass: cms.configs.ngConfirm.warningOKClass,
                        keys: ['enter'],
                        action: function (scope, button) {
                            callback(ProductId);
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

        //$scope.doRemoveAutoSave = function (ProductId) {
        //    Service.post("/Product/RemoveAutoSave", {})
        //        .then(function (response) {
        //            if (response.Success) {
        //                notify({
        //                    message: "Đã xóa bản nháp thành công",
        //                    classes: $scope.classes.Success,
        //                    position: $scope.position,
        //                    duration: $scope.duration
        //                });
        //                $scope.AutoSave.HasDraft = false;
        //            } else {
        //                notify({
        //                    message: "Có lỗi xảy ra",
        //                    classes: $scope.classes.Error,
        //                    position: $scope.position,
        //                    duration: $scope.duration
        //                });
        //            }
        //        });
        //}

        //$scope.CloseAutoSave = function () {
        //    $scope.AutoSave.HasDraft = false;
        //    $('#autoSaveAlert').hide();
        //}

        $("#modalActionHistory").on('hide.bs.modal', function () {
            $scope.ViewDataModel.EncryptProductId = '';
        });

        $scope.closeModal = function () {
            $scope.ViewDataModel.EncryptProductId = '';
        }

        $scope.actionHistory = function (EncryptProductId) {
            $scope.ActionHistory = [];
            $scope.ViewDataModel.EncryptProductId = EncryptProductId;
            Service.post("/Product/ActionHistory", { encript: EncryptProductId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.ActionHistory = response.Data;
                    }
                });
        }

        $scope.openExportExcel = function () {
            $scope.SearchExportExcel = { IsCTV: 'false' };
            console.log($scope.SearchExportExcel);
        }

        $scope.doExportExcel = function () {
            var query = "";
            query += $scope.SearchExportExcel.IsCTV == 'true' ? "isCTV=true" : "isCTV=false";
            if ($scope.SearchExportExcel.CreatedBy != undefined) query += "&createdBy=" + $scope.SearchExportExcel.CreatedBy;
            var fromCreatedDate = $("#txtFromCreatedDate").val();
            var toCreatedDate = $("#txtToCreatedDate").val();
            var fromPublishedDate = $("#txtFromPublishedDate").val();
            var toPublishedDate = $("#txtToPublishedDate").val();
            if (fromCreatedDate.length > 0) query += "&fromCreatedDate=" + fromCreatedDate;
            if (toCreatedDate.length > 0) query += "&toCreatedDate=" + toCreatedDate;
            if (fromPublishedDate.length > 0) query += "&fromPublishedDate=" + fromPublishedDate;
            if (toPublishedDate.length > 0) query += "&toPublishedDate=" + toPublishedDate;
            window.open("/Product/ExportExcel?" + query);
        }
    }]);
})();

$(document).ready(function () {
    $("#Productmanager").addClass("active");
    $("#Productmanager ul.child_menu").show();
});

function backtolist() {
    $("#listview").show();
    $("#detailview").hide();
}

