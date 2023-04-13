var flatlineUIFurniture;
(function ($) {
    "use strict";
    flatlineUIFurniture = (function () {
        return {
            init: function () {
                this.flatlineUIFurnitureInit();
            },
            flatlineUIFurnitureInit: function () {
                this.flatlineUIFurnitureSideNav();
            }
        }
    }());
})(jQuery);
(function ($) {
    "use strict";
    flatlineUIFurniture.flatlineUIFurnitureSideNav = function () {
        $("#requestCallbackBtn").click(function () {
            $(this).siblings(".custom-tooltips-container").toggleClass("hidden")
        });

        $(".close-custom-tooltips").click(function () {
            $(this).closest(".custom-tooltips-container").toggleClass("hidden");
        });
        var cookieCallMeBackId = 'sidenavCallMeBack';
        // cookie(cookieCallMeBackId, null, -1);
        if ((cookie(cookieCallMeBackId) !== "1")) {
            setTimeout(function () {
                $(".custom-tooltips-container").removeClass("hidden");
            }, 2*60*1000)
        }
        $(".hide24h").click(function ()  {
            $(".custom-tooltips-container").addClass("hidden");
            cookie(cookieCallMeBackId, "1", 1);
        });

    };
})(jQuery);
jQuery(document).ready(function () {
    flatlineUIFurniture.init();
});