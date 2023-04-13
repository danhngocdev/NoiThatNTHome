(function () {
    app.controller('UsersController', ['$scope', '$rootScope', '$http', 'Service', '$sce', 'notify', '$ngConfirm', function ($scope, $rootScope, $http, Service, $sce, notify, $ngConfirm) {
        //function ($scope, $rootScope, $http, Service, $sce, notify, Data) {



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

        $scope.BankModel = {
            'ListBank': [],
            'CurrentBank': 0
        };

        $scope.ListAccountType = {
            "ListStatus": [],
            "CurrentType": 0
        };

        $scope.ListAuthGroupType = {
            "ListAuthGroup": [],
            "CurrentType": 0
        };

        $scope.ProvinceModel = {
            'ListProvince': [],
            'CurrentProvince': 0
        };

        $scope.DistrictModel = {
            'ListDistrict': [],
            'CurrentDistrict': 0
        };

        $scope.BankBranchModel = {
            'ListBankBranch': [],
            'CurrentBankBranch': 0
        };

        $scope.UsersStatus = {
            "Deleted": 2,
            "Active": 1,
            "Lock": 0
        }

        $scope.ResetPassWordModel = {
            'UserId': 0,
            'FullName': "",
            'Email': "",
            "NewPassword": ""
        };

        $scope.RoleManager = {
            UserTopbank: 10,
            UserLevelBank: 11,
            UserLevelBranch: 12
        };

        // The time display of message
        $scope.duration = 2000;
        $scope.PatternEmail = /^[_a-z0-9]+(\.[_a-z0-9]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,5})$/;
        $scope.PatternPhone = /^[0-9]{9,12}$/;
        $scope.PatternUserName = /^[a-zA-Z0-9-_]{3,}$/;

        $scope.Search = {};

        $scope.UsersActionModel = {};
        $scope.CurrentFormStatus = 0;
        $scope.FormStatusEnums = { List: 0, CreateOrUpdate: 1, ChangePass: 2 };

        $scope.Init = function (obj) {

            validator.defaults.classes.alert = true;
            // validate a field on "blur" event, a 'select' on 'change' event & a '.reuired' classed multifield on 'keyup':
            $('form')
                .on('blur', 'input[required], input.optional, select.required', validator.checkField)
                .on('change', 'select.required', validator.checkField)
                .on('keypress', 'input[required][pattern]', validator.keypress);

        };

        $scope.InitUpdateForm = function () {

            $(".datepicker-short").datetimepicker({
                format: 'DD/MM/YYYY',
                locale: "vi",
                keepOpen: false,
                minDate: new Date(1800, 2 - 1, 1)
            });

            //validator.defaults.classes.alert = true;
            //// validate a field on "blur" event, a 'select' on 'change' event & a '.reuired' classed multifield on 'keyup':
            //$('form')
            //  .on('blur', 'input[required], input.optional, select.required', validator.checkField)
            //  .on('change', 'select.required', validator.checkField)
            //  .on('keypress', 'input[required][pattern]', validator.keypress);

            // giới tính
            //if ($scope.UsersActionModel.Gender === "True" || $scope.UsersActionModel.Gender === true) {
            //    document.getElementById('rdoMale').checked = true;
            //} else {
            //    document.getElementById('rdoFeMale').checked = true;
            //}

            //$("#avatarUsers").attr("src", "/Content/images/no-image.png").show();

            // Load giá trị mặc định cho các combobox
            if ($scope.UsersActionModel != null && JSON.stringify($scope.UsersActionModel) != "{}") {
                $scope.BankModel.CurrentBank = $scope.UsersActionModel.BankId;
                $scope.ListAccountType.CurrentType = "" + $scope.UsersActionModel.UserType + "";
                $scope.ListAuthGroupType.CurrentType = $scope.UsersActionModel.AuthGroupId;
                $scope.ProvinceModel.CurrentProvince = $scope.UsersActionModel.ProvinceId;
                $scope.DistrictModel.CurrentDistrict = $scope.UsersActionModel.DistrictId;
                $scope.BankBranchModel.CurrentBankBranch = $scope.UsersActionModel.BankBranchId;

                if ($scope.ProvinceModel.CurrentProvince > 0) {
                    $scope.onChangeProvince();
                }
            }

            $scope.getListUsersType();

        };

        //$scope.resetFromView = function () {
        //    $(window).scrollTop(0);
        //    $RIGHT_COL.css('min-height', $(".left_col").outerHeight());
        //}

        $scope.ListData = [];

        //Get list credit card data
        $scope.getListData = function () {
            $scope.Search.PageIndex = $scope.page.currentPage;
            $scope.Search.PageSize = $scope.page.itemsPerPage;
            $scope.Search.BankId = $scope.BankModel.CurrentBank;
            $scope.Search.BankBranch = $scope.BankBranchModel.CurrentBankBranch;
            $scope.Search.UserType = $scope.ListAccountType.CurrentType;
            Service.post("/Users/Search", { searchModel: $scope.Search })
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListData = response.Data;
                        if ($scope.ListData != null && $scope.ListData.length > 0) {
                            $scope.page.totalItems = $scope.ListData[0].TotalRows;
                        } else {
                            $scope.page.totalItems = 0;
                        }
                    }
                    else {
                        notify({
                            message: "Lỗi!" + response.Message,
                            classes: $scope.classes.Error,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });
                    }
                });
        };

        //Get debit card status
        $scope.getListUsersType = function () {
            Service.post("/Users/GetListUserType", {})
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListAccountType.ListStatus = response.Data.UsersTypeList;
                    }
                });
        }

        $scope.getListAuthGroup = function () {
            Service.post("/Users/GetListAuthGroup", {})
                .then(function (response) {
                    if (response.Success) {
                        $scope.ListAuthGroupType.ListAuthGroup = response.Data;
                    }
                });
        }

        $scope.getListData();
        $scope.getListUsersType();
        $scope.getListAuthGroup();

        $scope.getUserById = function (userId) {
            Service.post("/Users/GetUserById", { userId: userId })
                .then(function (response) {
                    if (response.Success) {
                        if (response.Data) {
                            $scope.UsersActionModel = response.Data;
                        }
                        else {
                            $scope.UsersActionModel = {};
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
                    $scope.InitUpdateForm();
                });
        }

        //Update status credit card
        $scope.updateStatus = function (userId, status) {
            var msgSuccess = "";
            if (status == $scope.UsersStatus.Active) {
                msgSuccess = "Đã khóa tài khoản.";
            } else {
                msgSuccess = "Đã mở khóa tài khoản.";
            }

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
                            Service.post("/Users/UpdateStatus", { userId: userId })
                                .then(function (response) {
                                    if (response.Success) {
                                        $scope.message = msgSuccess;
                                        notify({
                                            message: $scope.message,
                                            classes: $scope.classes.Success,
                                            position: $scope.positions.Center,
                                            duration: $scope.duration
                                        });
                                        setTimeout(function () { $scope.cancleUpdate(); }, $scope.duration);

                                    } else {
                                        notify({
                                            message: "Lỗi!" + (response.Message.length > 0 ? response.Message : "Cập nhật không thành công"),
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

        //Update status credit card
        $scope.openResetPassword = function (userId, fullname, email) {
            $scope.ResetPassWordModel.UserId = userId;
            $scope.ResetPassWordModel.FullName = fullname;
            $scope.ResetPassWordModel.Email = email;
            $scope.ResetPassWordModel.NewPassword = "";
            $scope.CurrentFormStatus = $scope.FormStatusEnums.ChangePass;
        }

        //Update status credit card
        $scope.autoGeneratePassword = function () {

            Service.post("/Users/GeneratePassWord", {})
                .then(function (response) {
                    if (response.Success) {
                        $scope.ResetPassWordModel.NewPassword = response.Data;
                    }
                });
        }

        //Update status credit card
        $scope.resetPassword = function () {
            var password = $scope.ResetPassWordModel.NewPassword;
            var userId = $scope.ResetPassWordModel.UserId;

            Service.post("/Users/ResetPassWord", { userId: userId, passWord: password })
                .then(function (response) {
                    if (response.Success) {
                        $scope.message = "Đã đặt lại mật khẩu thành công.";
                        notify({
                            message: $scope.message,
                            classes: $scope.classes.Success,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });

                        setTimeout(function () { $scope.cancleUpdate(); }, $scope.duration);

                    } else {
                        notify({
                            message: "Lỗi! " + (response.Message.length > 0 ? response.Message : "Cập nhật không thành công"),
                            classes: $scope.classes.Error,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });
                    }

                    $("#modelChangePass").modal("hide");
                });
        }

        $scope.clearCacheAuthen = function (userId) {

            Service.post("/Users/ClearCacheAuthen", { userId: userId})
                .then(function (response) {
                    if (response.Success) {
                        $scope.message = "Xóa cache phân quyền người dùng thành công.";
                        notify({
                            message: $scope.message,
                            classes: $scope.classes.Success,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });

                    } else {
                        notify({
                            message: "Lỗi! " + (response.Message.length > 0 ? response.Message : "Cập nhật không thành công"),
                            classes: $scope.classes.Error,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });
                    }
                });
        }

        $rootScope.$on("CallCancleUpdateMethod", function () {
            $scope.cancleUpdate();
        });

        $scope.cancleUpdate = function () {
            $scope.getListData();
            $scope.CurrentFormStatus = $scope.FormStatusEnums.List;
            //$(window).scrollTop(null);
        }

        $scope.doUpdate = function () {
            $scope.UsersActionModel.BirthdayStr = $("#txtDob").val();
            //$scope.UsersActionModel.Gender = document.getElementById("rdoMale").checked;

            $scope.UsersActionModel.BankId = $scope.BankModel.CurrentBank;
            $scope.UsersActionModel.UserType = $scope.ListAccountType.CurrentType;
            $scope.UsersActionModel.BankBranchId = $scope.BankBranchModel.CurrentBankBranch;
            $scope.UsersActionModel.AuthGroupId = $scope.ListAuthGroupType.CurrentType;

            Service.post("/Users/Update", { users: $scope.UsersActionModel })
                .then(function (response) {
                    if (response.Success) {
                        notify({
                            message: "Đã cập nhật thông tin.",
                            classes: $scope.classes.Success,
                            position: $scope.positions.Center,
                            duration: $scope.duration
                        });
                        setTimeout(function () { $scope.cancleUpdate(); }, $scope.duration);
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

        $scope.deleteAvatar = function () {
            $scope.UsersActionModel.Avatar = "";
        }

        // Upload avatar
        $scope.selectAvatar = function () {
            var w = window.open("/FileManager/Default.aspx", "File manager", "width = 950, height = 600");

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
                    $scope.UsersActionModel.Avatar = result.path;
                });
                w.close();
            }
        }

        // Click button search
        $scope.doSearch = function () {
            $scope.page.currentPage = 1;
            $scope.getListData();
        }

        //Key press
        $scope.keypressAction = function (keyEvent) {
            if (keyEvent.which === 13) {
                $scope.page.currentPage = 1;
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

    }]);
})();