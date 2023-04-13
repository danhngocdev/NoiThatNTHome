'use strict';
var baseService = angular.module('Common.Service', []);
var appService = function ($q, $http) {
    var services = {
        post: function (url, obj) {
            var d = $q.defer();
            $http({
                method: 'POST',
                url: url,
                data: obj
            }).then(function successCallback(data) {
                var pathname = window.location.pathname;
                var url = data.url != undefined ? data.url : '/dang-nhap';
                if (data.login !== undefined && !data.login) {
                    window.location.href = url + "?returnUrl=" + pathname;
                } else if (data.access !== undefined && !data.access) {
                    window.location.href = url;
                }
                d.resolve(data.data);

                //setTimeout(function () {
                //    $('[data-toggle=tooltip]').hover(function () {
                //        // on mouseenter
                //        $(this).tooltip('show');
                //    }, function () {
                //        // on mouseleave
                //        $(this).tooltip('hide');
                //    });
                //}, 1000);
            }, function errorCallback(response) {
                d.reject(response);

            });

            return d.promise;
        }
    }
    return services;
};
appService.$inject = ["$q", "$http"];
baseService.factory("Service", appService);


var resetValueTextbox = function () {

    $('input[type="text"]').val("");

}

app.factory('location', [
    '$location',
    '$route',
    '$rootScope',
    function ($location, $route, $rootScope) {
        $location.skipReload = function () {
            var lastRoute = $route.current;
            var un = $rootScope.$on('$locationChangeSuccess', function () {
                $route.current = lastRoute;
                un();
            });
            return $location;
        };
        return $location;
    }
]);

////////////////////////////////////////////////////////////////////////////////
/// TREEVIEW  //////////////////////////////////////////////////////////////////
app.directive('treeView', function ($compile) {
    return {
        restrict: 'E',
        scope: {
            localNodes: '=model',
            localClick: '&click'
        },
        link: function (scope, tElement, tAttrs, transclude) {

            var maxLevels = (angular.isUndefined(tAttrs.maxlevels)) ? 10 : tAttrs.maxlevels;
            var hasCheckBox = (angular.isUndefined(tAttrs.checkbox)) ? false : true;
            scope.showItems = [];

            scope.showHide = function (ulId) {
                var hideThis = document.getElementById(ulId);
                var showHide = angular.element(hideThis).attr('class');
                angular.element(hideThis).attr('class', (showHide === 'show' ? 'hide' : 'show'));

                // arrow
                var changeArrThis = document.getElementById("fa-angle-" + ulId);
                var changeArr = angular.element(changeArrThis).attr('class');
                angular.element(changeArrThis).attr('class', (changeArr === 'fa fa-angle-right' ? 'fa fa-angle-down' : 'fa fa-angle-right'));
            }

            scope.showIcon = function (node) {
                if (!angular.isUndefined(node.Children) && node.Children.length > 0)
                    return true;
            }

            scope.checkIfChildren = function (node) {
                if (!angular.isUndefined(node.Children) && node.Children.length > 0) return true;
            }

            /////////////////////////////////////////////////
            /// SELECT ALL CHILDRENS
            // as seen at: http://jsfiddle.net/incutonez/D8vhb/5/
            function parentCheckChange(item) {
                for (var i in item.Children) {
                    item.Children[i].Checked = item.Checked;
                    if (item.Children[i].Children) {
                        parentCheckChange(item.Children[i]);
                    }
                }
            }

            scope.checkChange = function (node) {
                if (node.Children) {
                    parentCheckChange(node);
                }
            }
            /////////////////////////////////////////////////

            function renderTreeView(collection, level, max) {
                var text = '';
                text += '<li ng-repeat="n in ' + collection + '" >';

                text += '<span ng-show=showIcon(n) class="show-hide" ng-click=showHide(n.Id)><i id="fa-angle-{{n.Id}}" class="fa fa-angle-right"></i></span>';
                text += '<span ng-show=!showIcon(n) ></span>';

                text += '<label class="custom-control custom-checkbox">';
                if (hasCheckBox) {
                    text += '<input class="tree-checkbox custom-control-input" type=checkbox ng-model=n.Checked ng-change=checkChange(n)>';
                    text += '<span class="custom-control-indicator"></span>';
                }

                text += '<span class="edit" ng-click=localClick({node:n})><i></i></span>'

                text += '<label>{{n.Name}}</label>';
                text += '</label>';
                if (level < max) {
                    text += '<ul id="{{n.Id}}" class="hide" ng-if=checkIfChildren(n)>' + renderTreeView('n.Children', level + 1, max) + '</ul></li>';
                } else {
                    text += '</li>';
                }

                return text;
            }// end renderTreeView();

            try {
                var text = '<ul class="tree-view-wrapper">';
                text += renderTreeView('localNodes', 1, maxLevels);
                text += '</ul>';
                tElement.html(text);

                $compile(tElement.contents())(scope);
            }
            catch (err) {
                tElement.html('<b>ERROR!!!</b> - ' + err);
                $compile(tElement.contents())(scope);
            }
        }
    };
});

//String.prototype.trim = function (chars) {
//    if (chars === undefined)
//        chars = "\s";

//    return this.replace(new RegExp("^[" + chars + "]+"), "").replace(new RegExp("[" + chars + "]+$"), "");
//};

//String.prototype.replaceAll = function (replaceFrom, replaceTo) {
//    var text = this;

//    if (!replaceFrom || !replaceTo || replaceFrom == replaceTo) {
//        return text;
//    }

//    while (text.indexOf(replaceFrom) > -1) {
//        text = text.replace(replaceFrom, replaceTo);
//    }

//    return text;
//};



Array.prototype.pushUnique = function (item) {

    var mappedIds = this.map(function (e) {
        return e.Id;
    });

    if (mappedIds.indexOf(item.Id) == -1) {
        this.push(item);
        return true;
    }
    return false;
}