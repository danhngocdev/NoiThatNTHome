(function () {
    app.controller('MenuController', ['$scope', '$http', 'Service', function ($scope, $http, Service) {
        $scope.PermissionModel = {};
        $scope.Init = function (obj) {
            $scope.PermissionModel = obj;
            templateActionMenu();
        };

        var templateActionMenu = function () {
            var path = window.location.pathname;
            var splits = path.split('/');
            var controller = splits[1];
            var action = splits[2];
            var newsType = getUrlParameter('newsType');

            // active menu left
            var id = 'SidebarNav_' + controller;
            var obj = $('.sidebar').find('.' + id);
            obj.removeClass('d-none');
            var isSubMenu = obj.hasClass('nav-dropdown');
            $('.sidebar .nav-dropdown').each(function () { $(this).removeClass('open'); });

            if (action) {
                var actionWithParam = action;
                if (newsType != undefined && (newsType == "4" || newsType == "8" || newsType == "6" || newsType == "7")) actionWithParam += "?newsType=" + newsType;

                var activeAction = obj.find("a[href$='/" + actionWithParam + "']");
                activeAction.addClass('active').closest("li").addClass("open");
                if (isSubMenu) {
                    activeAction.closest('ul').parent('li').addClass('open');
                }
            }
            else {
                var activeController = obj.find("a[href='/" + controller + "']");
                activeController.addClass('active').closest("li").addClass("open");
                if (isSubMenu) {
                    activeController.closest('ul').parent('li').addClass('open');
                }
            }
            // active menu top
            var parentUrl = "";
            switch (controller) {
                case "Settings":
                case "AuthGroup":
                case "LogException":
                case "Users": { parentUrl = "Settings"; break; }
                case "Home": { if (action == "Caching") parentUrl = "Settings"; break; }
                case "PricingHighlight":
                case "BoxNewsEmbed":
                case "Highlight": { parentUrl = "Highlight"; break; }
                case "Category": { parentUrl = "Category"; break; }
                //case "Tags": { parentUrl = "Tags"; break; }
                case "BikeBrand":
                case "BikeModel":
                case "CarInfo":
                case "CarBrand":
                case "CarModel":
                case "CarModelDetail":
                case "CarSegment":
                    { parentUrl = "CarInfo"; break; }
                case "Topic":
                    { parentUrl = "Topic"; break; }
                case "Utilities":
                case "Tags":
                case "TagCloud":
                case "Banner":
                case "Subscribe":
                case "BoxLinkSEO":
                    { parentUrl = "Tags"; break; }
                case "News":
                case "NewsExternal":
                    { parentUrl = "News"; break; }
            }

            if (newsType != undefined) {
                if (newsType == 4 || newsType == 8)
                    parentUrl += "?newsType=4";
                if (newsType == 6 || newsType == 7)
                    parentUrl += "?newsType=6";
            }

            window.setTimeout(function () {
                $('.navbar-nav').find("a[href='/" + parentUrl + "']").addClass('active');
            }, 500);

            // hidden left menu if hasn't items
            var hasSubMenu = false;
            $('.sidebar-nav ul > li').not('.sidebar-nav ul > li ul li').each(function () {
                if (!$(this).hasClass('d-none')) {
                    hasSubMenu = true;
                }
            });
            if (!hasSubMenu) {
                $('body').addClass('sidebar-hidden');
            }

            // đánh giá xe, giá xe
            if (newsType != undefined && (newsType == "4" || newsType == "8" || newsType == "6" || newsType == "7")) {
                // nếu có newsType thì là đánh giá xe or giá xe
                $('.sidebar').find('.SidebarNav_News').addClass('d-none');
                switch (newsType) {
                    case "8":
                    case "4":
                        {
                            //SidebarNav_Assessment
                            $('.sidebar').find('.SidebarNav_Assessment').removeClass('d-none');
                            break;
                        }
                    case "6":
                    case "7":
                        {
                            //SidebarNav_Pricing
                            $('.sidebar').find('.SidebarNav_Pricing').removeClass('d-none');
                            break;
                        }
                }
            }
            else {
                $('.sidebar').find('.SidebarNav_Assessment').addClass('d-none');
                $('.sidebar').find('.SidebarNav_Pricing').addClass('d-none');
            }
        }
        var getUrlParameter = function getUrlParameter(sParam) {
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