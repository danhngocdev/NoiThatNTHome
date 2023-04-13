(function () {
    app.controller('AccountController', ['$scope', '$http', 'Service', 'notify', function ($scope, $http, Service, notify) {
        //function ($scope, $http, Service) {
        // START
        //string email, string password, string returnUrl = null, bool isSavedPassword = false

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

        $scope.Model = {
            email: '',
            password: '',
            isSavedPassword: false
        };
        $scope.returnUrl = '';

        validator.message.date = 'not a real date';

        $scope.init = function (returnUrl) {
            $scope.returnUrl = returnUrl;

            // validate a field on "blur" event, a 'select' on 'change' event & a '.reuired' classed multifield on 'keyup':
            $('form')
                .on('blur', 'input[required], input.optional, select.required', validator.checkField)
                .on('change', 'select.required', validator.checkField)
                .on('keypress', 'input[required][pattern]', validator.keypress);
        };

        $scope.doLogin = function () {
            Service.post("/Account/DoLogin", { email: $scope.Model.email, password: $scope.Model.password, isSavedPassword: $scope.Model.isSavedPassword })
                .then(function (response) {
                    if (response.status) {
                        return location.href = $scope.returnUrl;
                    } else {
                        notify({
                            message: response.message,
                            classes: $scope.classes.Error,
                            templateUrl: $scope.template,
                            position: $scope.position,
                            duration: $scope.duration
                        });
                    }
                });
        };

        $scope.ShortSubmit = function (event) {
            if (event.which === 13)
                $scope.doLogin();
        };

        $scope.ChangePasswordModel = {
            CurrentPassword: '',
            Password: '',
            ConfirmPassword: ''
        };

        $scope.WhenSubmitChange = function (event) {
            if (event.which === 13)
                $scope.doChangePassword();
        };

        $scope.doChangePassword = function () {
            Service.post("/Account/ChangePassword", { saveModel: $scope.ChangePasswordModel })
                .then(function (response) {
                    if (response.Success) {
                        return location.reload();
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
        };

        // END
    }]);
})();