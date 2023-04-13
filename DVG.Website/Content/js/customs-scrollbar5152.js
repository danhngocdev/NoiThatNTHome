jQuery(document).ready(function ($) {
    $('.nav.nav-vertical').each(function () {
        const ps = new PerfectScrollbar($(this)[0], {
            suppressScrollX: true
        })
    });
});