(function () {
    app.controller('PersonController', ['$scope', '$rootScope', '$http', 'Service', '$sce', 'notify', '$ngConfirm', function ($scope, $rootScope, $http, Service, $sce, notify, $ngConfirm) {

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

        $scope.PersonStatus = [
            { "Id": 1, "Name": "Hoạt động" },
            { "Id": 2, "Name": "Không hoạt động" }
        ];

        $scope.PersonStatusEnum = {
            "Show": 1,
            "Hide": 2,
        };


        $scope.SearchModel = {}
        $scope.ParamModel = {}

        $scope.PersonModel = {};

        $scope.CurrentFormStatus = 0;
        $scope.FormStatusEnums = { List: 0, CreateOrUpdate: 1 };

        $scope.Init = function (obj) {
            if (obj) {
                $scope.ParamModel = obj;
                if ($scope.ParamModel.PersonId > 0) {
                    $scope.GetPerson($scope.ParamModel.PersonId);
                }
            }
            $scope.getListData();
        };

        //Get list credit card data
        $scope.getListData = function () {
            $scope.SearchModel.ListData = {};
            $scope.SearchModel.PageIndex = $scope.page.currentPage;
            $scope.SearchModel.PageSize = $scope.page.itemsPerPage;
            Service.post("/Person/Search", { searchModel: $scope.SearchModel })
                .then(function (response) {
                    if (response.Success) {
                        $scope.SearchModel.ListData = response.Data.ListData;
                        $scope.page.totalItems = response.TotalRow;
                    }
                });
        };

        //Get Person
        $scope.GetPerson = function (id) {
            if (id > 0) {
                Service.post("/Person/GetPerson", { PersonId: id })
                    .then(function (response) {
                        if (response.Success) {
                            $scope.PersonModel = response.Data;
                            $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
                            //CKEDITOR.instances["txtDescription"].setData($scope.PersonModel.Embed);
                        }
                    });
            } else {
                $scope.PersonModel = {};
                $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
                //CKEDITOR.instances["txtDescription"].setData('');
            }
        }

        $scope.doUpdate = function (Person) {
            if (Person != null && Person != undefined) {
                $scope.PersonModel = angular.copy(Person);
            }
            //$scope.PersonModel.Embed = CKEDITOR.instances["txtDescription"].getData();
            //$scope.PersonModel.AvatarStr = $('#avatarTopic').attr("src");
            Service.post("/Person/Update", { model: $scope.PersonModel })
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

        //Delete Person
        $scope.delete = function (id) {
            if (confirm("Bạn muốn xóa bản ghi này?")) {
                Service.post("/Person/Delete", { id: id })
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

        //Xem ảnh gốc avatar hoặc ảnh slide
        $scope.previewImages = function (imageurl) {
            if (imageurl.indexOf(cms.configs.CropSizeCMS) > -1) {
                imageurl = imageurl.replace(cms.configs.CropSizeCMS, "/");
            }
            popupCenter("/FileManager/Default.aspx?ac=article&imgurl=" + imageurl, "File manager", 950, 600);
            //window.open("/FileManager/Default.aspx?ac=article&imgurl=" + imageurl, "File manager", "width = 950, height = 600");
        }

        $scope.deleteAvatar = function () {
            $scope.PersonModel.AvatarStr = cms.configs.NoImage;
        }

        // Upload avatar
        $scope.selectAvatar = function () {
            var left = ($(window).width() / 2) - 475;
            var top = ($(window).height() / 2) - 300;
            //var w = window.open("/FileManager/Default.aspx", "File manager", "width = 950, height = 600, top = " + top + ", left = 200");
            var w = window.open("/FileManager/Default.aspx", "File manager", "width = 950, height = 600, top = " + top );

            w.callback = function (lst) {
                if (!lst || lst.length <= 0) {
                    alert("Không có file nào được chọn!");
                    return;
                }
                var result = lst[0];

                if (/(\.|\/)(gif|jpe?g|png)$/i.exec(result.path) == null) {
                    alert("Bạn cần chọn hình ảnh");
                    return;
                }
                $scope.$apply(function () {
                    $scope.PersonModel.AvatarStr = result.path;
                });
                w.close();
            }
        }

        //Get Person to update
        $scope.editextend = function (Person) {
            if (Person) {
                $scope.PersonModel = Person;
                $scope.PushStateUrl($scope.PersonModel.Id);
                if ($scope.PersonModel.AvatarStr == "")
                    $scope.PersonModel.AvatarStr = cms.configs.NoImage;
                //window.setTimeout(function () {
                //    CKEDITOR.instances["txtDescription"].setData($scope.PersonModel.Embed);
                //}, 200);
            } else {
                $scope.PersonModel = {};
                //CKEDITOR.instances["txtDescription"].setData('');
            }
            $scope.CurrentFormStatus = $scope.FormStatusEnums.CreateOrUpdate;
            document.documentElement.scrollTop = 80;
            //$scope.closeToolTips();
        }

        //Edit Person on list
        $scope.edit = function (Person) {
            $scope.cancelUpdate();
            $scope.PersonModel = angular.copy(Person);
            //$scope.closeToolTips();
        };

        //Get template
        $scope.getTemplate = function (Person) {
            if (!Person || $scope.CurrentFormStatus == $scope.FormStatusEnums.CreateOrUpdate) {
                return 'display';
            }
            if (Person.Id === $scope.PersonModel.Id) return 'edit';
            else return 'display';
        }

        //Close tooltips
        $scope.closeToolTips = function () {
            $('[data-toggle="tooltip"]').tooltip('hide');
        }

        //Rewrite URL
        $scope.PushStateUrl = function (id) {
            window.setTimeout(function () {
                window.history.pushState(null, null, "/Person?PersonId=" + id);
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
            $scope.PersonModel = {};
            $scope.CurrentFormStatus = $scope.FormStatusEnums.List;
            window.setTimeout(function () {
                window.history.pushState(null, null, "/Person");
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