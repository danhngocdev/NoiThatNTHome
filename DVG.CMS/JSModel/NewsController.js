(function () {
    app.controller('NewsController', ['$scope', '$http', 'Service', '$sce', 'notify', '$ngConfirm', function ($scope, $http, Service, $sce, notify, $ngConfirm) {
        $scope.page = {
            'totalItems': 0,
            'currentPage': 1,
            'itemsPerPage': 15
        };

        $scope.NewsStatus = {
            "Temp": 0,
            "Pending": 1,
            "PendingApproved": 2,
            "Published": 3,
            "Returned": 4,
            "UnPublished": 5,
            "Deleted": 6
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
        $scope.StatusOfNewsPermission = {};
        $scope.IsPreviewForm = false;
        $scope.currentOrder = 0;

        $scope.NewsDisplayPosition = {
            "Normal": 1,
            "Highlight": 2,
            "HighlightOnCategory": 3
        }

        //$scope.AutoSave = {
        //    HasDraft: false,
        //    Params: ""
        //};

        $scope.Search.ListCategory = [
           
        ];

        $scope.ViewDataModel = {
            'EncryptNewsId': ''
        };

        $scope.Init = function (obj) {
            if (obj) {
                //$scope.Search.Status = obj.Status;
                //$scope.Search.DisplayPosition = obj.DisplayPosition;
                //$scope.Search.DisplayStyle = obj.DisplayStyle;
                //$scope.Search.ListDisplayStyle = obj.ListDisplayStyle;
                //$scope.Search.TagsName = obj.TagsName;

                //$scope.Search.TypeOfNews = obj.TypeOfNews;
                //$scope.Search.PositionOfNews = obj.PositionOfNews;
                //$scope.Search.ListNewsType = obj.ListNewsType;
                //$scope.Search.NewsType = obj.NewsType;
                //$scope.Search.ListNewsStatus = obj.ListNewsStatus;
                //$scope.Search.Status = obj.Status;
                //$scope.Search.ListUserOnControll = obj.ListUserOnControll;

                $scope.Search.ListStatus = obj.ListStatus;
                //$scope.Search.CategoryId = obj.CateId;
                //$scope.Search.StatusOfNews = obj.StatusOfNews;
                $scope.StatusOfNewsPermission = obj.StatusOfNewsPermission;
                $scope.getListCategory();
                // check autosave
                //if (cms.configs.NewsAutoSave.Enable && obj.NewsAutoSave != null && obj.NewsAutoSave.HasDraft) {
                //    $scope.AutoSave.HasDraft = true;
                //    if (obj.NewsAutoSave.Id > 0) $scope.AutoSave.Params = "newsId=" + obj.NewsAutoSave.EncryptNewsId + "&";
                //    $scope.AutoSave.Title = obj.NewsAutoSave.Title;
                //}

                //GetListCountNewsByStatus(obj.NewsType);
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
            Service.post("/News/Search", { searchModel: $scope.Search })
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListData = response.Data.ListData;
                        if ($scope.ListData != null && $scope.ListData.length > 0) {
                            $scope.page.totalItems = response.TotalRow;
                        } else {
                            $scope.page.totalItems = 0;
                        }
                    }
                    $("#indexnews").removeClass("hidden");
                });
        };

        //Init chosen for select option
        $scope.SelectInit = function () {
            setTimeout(function () {
                $(".chosen-select").chosen();
                $('.chosen-select').trigger('chosen:updated');
            }, 200);
        }

        //on change news type
        $scope.onChangeNewsType = function () {
            $scope.doSearch();
            $scope.getListCategory();
        }

        $scope.roundNumberPaging = function (itemPerPage, totalItem) {
            return Math.ceil(parseFloat(totalItem) / itemPerPage);
        }

        $scope.getListCategory = function () {
            if ($scope.Search.NewsType) {
                Service.post("/Category/GetListCategoryByType", { newsType: $scope.Search.NewsType })
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
        $scope.doPublish = function (newsId) {
            if (newsId.length > 0) {
                ngConfirm(doPublish, newsId, cms.message.confirmDoPublish);
            }
        }
        var doPublish = function (newsId) {
            Service.post("/News/DoPublish", { newsId: newsId })
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
        $scope.doPending = function (newsId) {
            if (newsId.length > 0) {
                ngConfirm(doPending, newsId, cms.message.confirmDoPending);
            }
        }
        var doPending = function (newsId) {
            Service.post("/News/DoPending", { newsId: newsId })
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
        $scope.doRecivedEdit = function (newsId) {
            if (newsId.length > 0) {
                ngConfirm(doRecivedEdit, newsId, cms.message.confirmDoRecivedEdit);
            }
        }
        var doRecivedEdit = function (newsId) {
            Service.post("/News/DoRecivedEdit", { newsId: newsId })
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
        $scope.doPendingApprove = function (newsId) {
            if (newsId.length > 0) {
                ngConfirm(doPendingApprove, newsId, cms.message.confirmDoPendingApprove);
            }
        }
        var doPendingApprove = function (newsId) {
            Service.post("/News/DoPendingApprove", { newsId: newsId })
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
        $scope.doRecivedApprove = function (newsId) {
            if (newsId.length > 0) {
                ngConfirm(doRecivedApprove, newsId, cms.message.confirmDoRecivedApprove);
            }
        }
        var doRecivedApprove = function (newsId) {
            Service.post("/News/DoRecivedApprove", { newsId: newsId })
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
        $scope.doReturn = function (newsId) {
            if (newsId.length > 0) {
                ngConfirm(doReturn, newsId, cms.message.confirmDoReturn);
            }
        }
        var doReturn = function (newsId) {
            Service.post("/News/DoReturn", { newsId: newsId })
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
        $scope.doUnPublish = function (newsId) {
            if (newsId.length > 0) {
                ngConfirm(doUnPublish, newsId, cms.message.confirmDoUnPublish);
            }
        }
        var doUnPublish = function (newsId) {
            Service.post("/News/DoUnPublish", { newsId: newsId })
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
        $scope.doDelete = function (newsId) {
            if (newsId.length > 0) {
                ngConfirm(doDelete, newsId, cms.message.confirmDoDelete);
            }
        }
        var doDelete = function (newsId) {
            Service.post("/News/DoDelete", { newsId: newsId })
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

        $scope.getNewsById = function (newsId) {
            Service.post("/News/NewsView", { newsId: newsId })
                .then(function (response) {
                    if (response.Success) {
                        $scope.News = response.Data;
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

        //Update order position News HightLight
        $scope.onchangeOrderNewsHightLight = function (newsObject) {
            if (isNaN(newsObject.item.Ordinal) || parseInt(newsObject.item.Ordinal) < 0) {
                notify({
                    message: "Số thứ tự phải là số nguyên dương.",
                    classes: $scope.classes.Error,
                    position: $scope.positions.Center,
                    duration: $scope.duration
                });
                newsObject.item.Ordinal = $scope.currentOrder;
            } else {
                if ($scope.currentOrder !== newsObject.item.Ordinal) {
                    if (newsObject.item.Ordinal === "") newsObject.item.Ordinal = 0;
                    Service.post("/News/UpdateOrderNews ", { encryptId: newsObject.item.EncryptNewsId, ordinal: newsObject.item.Ordinal, position: $scope.NewsDisplayPosition.Highlight, currentPosition: newsObject.item.DisplayPosition })
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

        //Update order position News HightLight
        $scope.onchangeOrderNewsHightLightOnCategory = function (newsObject) {
            if (isNaN(newsObject.item.Ordinal) || parseInt(newsObject.item.Ordinal) < 0) {
                notify({
                    message: "Số thứ tự phải là số nguyên dương.",
                    classes: $scope.classes.Error,
                    position: $scope.positions.Center,
                    duration: $scope.duration
                });
                newsObject.item.Ordinal = $scope.currentOrder;
            } else {
                if ($scope.currentOrder !== newsObject.item.Ordinal) {
                    if (newsObject.item.Ordinal === "") newsObject.item.Ordinal = 0;
                    Service.post("/News/UpdateOrderNews ", { encryptId: newsObject.item.EncryptNewsId, ordinal: newsObject.item.Ordinal, position: $scope.NewsDisplayPosition.HighlightOnCategory, currentPosition: newsObject.item.DisplayPosition })
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

        $scope.getListCategory = function () {
            var newsType = 2;
            Service.post("/Category/GetListCategoryByType", { newsType: newsType })
                .then(function (response) {
                    if (response.Success) {
                        $scope.Search.ListCategory = response.Data;
                    }
                });
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

        var GetListCountNewsByStatus = function (newsType) {
            setTimeout(function () {
                Service.post("/News/GetListCountNewsByStatus", { newsType: newsType })
                    .then(function (response) {
                        if (response.Success) {
                            for (i = 0; i < response.Data.length; i++) {
                                if (response.Data[i].Count > 0)
                                    $('.countNews-' + newsType + '-' + response.Data[i].Status).append(' (<b class="text-danger">' + response.Data[i].Count + '</b>)');
                            }
                        }
                    });
            }, 3000);
        }

        $scope.getNewsTypeName = function (newsType) {
            return getNewsTypeName(newsType);
        }

        $scope.getLinkUrl = function (url) {
            prompt("Nhấn Ctrl + C để sao chép nội dung", url);
        };

        var ngConfirm = function (callback, newsId, message) {
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
                            callback(newsId);
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

        //$scope.doRemoveAutoSave = function (newsId) {
        //    Service.post("/News/RemoveAutoSave", {})
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
            $scope.ViewDataModel.EncryptNewsId = '';
        });

        $scope.closeModal = function () {
            $scope.ViewDataModel.EncryptNewsId = '';
        }

        $scope.actionHistory = function (EncryptNewsId) {
            $scope.ActionHistory = [];
            $scope.ViewDataModel.EncryptNewsId = EncryptNewsId;
            Service.post("/News/ActionHistory", { encript: EncryptNewsId })
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
            window.open("/News/ExportExcel?" + query);
        }
    }]);
})();

$(document).ready(function () {
    $("#newsmanager").addClass("active");
    $("#newsmanager ul.child_menu").show();
});

function backtolist() {
    $("#listview").show();
    $("#detailview").hide();
}

