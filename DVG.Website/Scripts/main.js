/*
DES: Scripts main
*/
var main = {
    init: function () {
        //wow = new WOW({
        //    animateClass: 'animated',
        //    offset: 100
        //});
        //wow.init();

        main.initForm();

        //$('#closemenu,.opacity').click(function () {
        //    $('.head-menu').removeClass('open');
        //    $('.opacity').addClass('hidden');
        //});

        //$('#showmenu').click(function () {
        //    $('.head-menu').toggleClass('open');
        //});

        //$('#header-mobile .dropdown .icon-down-open').click(function () {
        //    $(this).parent().find(".dropdown-menu-mobi").slideToggle("");
        //});

        //$("#brand-slider").owlCarousel({
        //    autoPlay: false,
        //    autoplayTimeout: 5000,
        //    slideSpeed: 2000,
        //    items: 6,
        //    pagination: false,
        //    navigation: true,
        //    itemsDesktop: [1199, 5],
        //    itemsDesktopSmall: [979, 4],
        //    itemsMobile: [767, 2],
        //});

        main.fixedMenuScroll();

        main.backToTop();

        $("#popup").on("click", "button[type=submit]", function (e) {
            main.initFormPopup();
            e.stopPropagation();
        });

        $('body').on("click", ".contact-popup", function () {
            $.ajax({
                type: "post",
                cache: false,
                url: "/Customer/ContactPopup",
                beforeSend: function () {
                    common.loading();
                    common.noScroll();
                },
                success: function (data) {
                    $('#popup').html(data);
                },
                complete: function () {
                    common.loaded();
                    common.noScroll();
                }
            })
        });

        //setTimeout(function () {
        //    var cookieBanner = cookie_manager.get_cookie("BannerPopupIds");
        //    if (cookieBanner == undefined || cookieBanner.length == 0) {
        //        $.ajax({
        //            type: "post",
        //            cache: false,
        //            url: "/Banner/BannerByPosition",
        //            data: {
        //                "pageId": 1,
        //                "positionId": 5
        //            },
        //            success: function (data) {
        //                $('#agencyRegister').html(data);
        //            }
        //        })
        //    }
        //}, 300);



        var agSwiper = $('#boxProject .swiper-container');

        if (agSwiper.length > 0) {

            var sliderView = 1;
            var ww = $(window).width();
            if (ww >= 1700) sliderView = 7;
            if (ww <= 1700) sliderView = 7;
            if (ww <= 1560) sliderView = 6;
            if (ww <= 1400) sliderView = 5;
            if (ww <= 1060) sliderView = 4;
            if (ww <= 800) sliderView = 3;
            if (ww <= 560) sliderView = 2;
            if (ww <= 400) sliderView = 2;

            var swiper = new Swiper('#boxProject .swiper-container', {
                slidesPerView: sliderView,
                spaceBetween: 0,
                loop: true,
                loopedSlides: 16,
                speed: 700,
                autoplay: true,
                autoplayDisableOnInteraction: true,
                centeredSlides: true
            });

            //$(window).resize(function () {
            //    var ww = $(window).width();
            //    if (ww >= 1700) swiper.params.slidesPerView = 7;
            //    if (ww <= 1700) swiper.params.slidesPerView = 7;
            //    if (ww <= 1560) swiper.params.slidesPerView = 6;
            //    if (ww <= 1400) swiper.params.slidesPerView = 5;
            //    if (ww <= 1060) swiper.params.slidesPerView = 4;
            //    if (ww <= 800) swiper.params.slidesPerView = 3;
            //    if (ww <= 560) swiper.params.slidesPerView = 2;
            //    if (ww <= 400) swiper.params.slidesPerView = 1;
            //});


        }


        //var homeProductBox = $('.home-product-box');
        //if (homeProductBox.length > 0) {
        //    $(homeProductBox).on("click", ".product-title-item", function () {
        //        var id = $(this).data("id");
        //        $(homeProductBox).find(".product-title-item").removeClass("active");
        //        $(this).addClass("active");
        //        $(homeProductBox).find(".product-content-item").hide();
        //        $(homeProductBox).find(".product-content-item[data-id='" + id + "']").show();
        //        if (id == "newest") {
        //            let width = $('body').width();
        //            if (width < 768) {
        //                $('#homepage [data-id="newest"] .product-content-scroll').scrollLeft(width * (225 / 780) + 50);
        //            }
        //        }
        //    });
        //}
        //var $ogSlide = $('#osgslide');
        //if ($ogSlide.length > 0) {

        //    if ($.fn.osgslide) {
        //        $('#osgslide').osgslide('/crop/90x90/', '/crop/550x490/', 90, 5, countImage);
        //    }
        //}


        main.openPopup();


        //メインスライド
        //var slider = new Swiper('#productdetail .gallery-slider', {
        //    slidesPerView: 1,
        //    centeredSlides: true,
        //    loop: false,
        //    //autoplay: true,
        //    loopedSlides: 6, //スライドの枚数と同じ値を指定
        //    navigation: {
        //        nextEl: '.swiper-button-next',
        //        prevEl: '.swiper-button-prev',
        //    },
        //});

        //サムネイルスライド
        //var thumbs = new Swiper('#productdetail .gallery-thumbs', {
        //    slidesPerView: 'auto',
        //    spaceBetween: 10,
        //    centeredSlides: true,
        //    loop: false,
        //    autoplay: true,
        //    slideToClickedSlide: true,

        //});

        //3系
        //slider.params.control = thumbs;
        //thumbs.params.control = slider;

        //4系～
        //slider.controller.control = thumbs;
        //thumbs.controller.control = slider;



        $('.js-click-product').slick({
            slidesToShow: 3,
            slidesToScroll: 1,
            asNavFor: '.js-product-slider',
            dots: false,
            focusOnSelect: true,
            infinite: true,
            arrows: true,
            vertical: true,
            responsive: [

                {
                    breakpoint: 767,
                    settings: {
                        vertical: false
                    }
                }
            ]
        });
        $('.js-product-slider').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
            asNavFor: '.js-click-product'
        });



        //var productHotSlide = $('#productHotSlide');
        //if (productHotSlide.length > 0) {
        //    var productHotSwiper = new Swiper("#productHotSlide .swiper-container", {
        //        navigation: {
        //            nextEl: "#productHotSlide .next-slide",
        //            prevEl: "#productHotSlide .previous-slide",
        //        },
        //        slidesPerView: "auto",
        //        loop: true
        //    });
        //}

        //var promotionSlide = $('#promotionSlide');
        //if (promotionSlide.length > 0) {
        //    var promotionSlideSwiper = new Swiper("#promotionSlide .swiper-container", {
        //        navigation: {
        //            nextEl: "#promotionSlide .next-slide",
        //            prevEl: "#promotionSlide .previous-slide",
        //        },
        //        slidesPerView: "auto",
        //        loop: true
        //    });
        //}
        //var promotionSlideWap = $('#promotionSlideWap');
        //if (promotionSlideWap.length > 0) {
        //    setTimeout(function () {
        //        var width = $('body').width();
        //        $('#promotionSlideWap .promotion-box').scrollLeft(width * (165 / 780) - 10);
        //    }, 300);
        //}

        //var homepageWrapp = $('#homepage');
        //if (homepageWrapp.length > 0) {
        //    let width = $('body').width();
        //    if (width < 768) {
        //        $('#homepage .product-section .product-section-box .product-scroll').scrollLeft(width * (225 / 780) + 50);
        //        $('#homepage .news-box .news-list').scrollLeft(width * (225 / 780) + 50);
        //        $('#homepage .product-content-scroll').scrollLeft(width * (225 / 780) + 50);
        //    }
        //}


        //var promotionSlideWap = $('#promotionSlideWap');
        //if (promotionSlideWap.length > 0) {
        //    setTimeout(function () {
        //        var width = $('body').width();
        //        $('#promotionSlideWap .promotion-box').scrollLeft(width * (165 / 780) - 10);
        //    }, 300);
        //}

        //var bannerSlide = $('#bannerSlide');
        //if (bannerSlide.length > 0) {
        //    var bannerSlideSwiper = new Swiper("#bannerSlide .swiper-container", {
        //        slidesPerView: "auto",
        //        loop: true,
        //        autoplay: {
        //            delay: 5000,
        //            disableOnInteraction: false
        //        },
        //        pagination: {
        //            el: '#bannerSlide .swiper-pagination',
        //            type: 'bullets',
        //        }
        //    });
        //}


        //var categorySlide = $('.category-slide');
        //if (categorySlide.length > 0) {
        //    var categorySlideSwiper = new Swiper(".category-slide .swiper-container", {
        //        navigation: {
        //            nextEl: ".category-slide .next-slide",
        //            prevEl: ".category-slide .previous-slide",
        //        },
        //        slidesPerView: "auto",
        //        loop: true
        //    });
        //}

        //var feedback = $('#feedback');
        //if (feedback.length > 0) {
        //    var feedbackSwiper = new Swiper("#feedback .swiper-container", {
        //        navigation: {
        //            nextEl: "#feedback .next-slide",
        //            prevEl: "#feedback .previous-slide",
        //        },
        //        slidesPerView: "auto",
        //        centeredSlides: true,
        //        loop: true,
        //        spaceBetween: 15
        //    });
        //}

        //$('.product-detail').on("click", ".cart-qty-minus", function () {
        //    var input = $('.product-detail .cart-qty-input');
        //    var inputValue = parseInt(input.val());
        //    if (inputValue > 1)
        //        inputValue--;
        //    input.val(inputValue);
        //});

        //$('.product-detail').on("click", ".cart-qty-plus", function () {
        //    var input = $('.product-detail .cart-qty-input');
        //    var inputValue = parseInt(input.val());
        //    if (inputValue > 0)
        //        inputValue++;
        //    input.val(inputValue);
        //});
        //$('#cartPage').on("click", ".cart-qty-minus", function () {
        //    var productId = $(this).data("productid");
        //    var price = $(this).parent().data("price");
        //    var input = $('#cartPage .cart-qty-input[data-productid="' + productId + '"]');
        //    var inputValue = parseInt(input.val());
        //    if (inputValue > 1) {
        //        inputValue--;
        //        input.val(inputValue);
        //        var totalPrice = price * inputValue;
        //        $('#cartPage .money[data-productid="' + productId + '"]').text(formatVND(totalPrice));
        //        main.updateToCart(productId, 1);
        //    }
        //});

        //$('#cartPage').on("click", ".cart-qty-plus", function () {
        //    var productId = $(this).data("productid");
        //    var price = $(this).parent().data("price");
        //    var input = $('#cartPage .cart-qty-input[data-productid="' + productId + '"]');
        //    var inputValue = parseInt(input.val());
        //    inputValue++;
        //    input.val(inputValue);
        //    var totalPrice = price * inputValue;
        //    $('#cartPage .money[data-productid="' + productId + '"]').text(formatVND(totalPrice)).attr("data-total", totalPrice);
        //    main.updateToCart(productId, 2);
        //});

        //$('.add-to-cart').on("click", function () {
        //    main.addToCart(1);
        //})

        //$('.buynow').on("click", function () {
        //    main.addToCart(2);
        //})

        //$('#btn-search').on("click", function () {
        //    main.search();
        //});


        //$('.menu-wap').on("click", ".icon-down-open", function () {
        //    $(this).parent().parent().toggleClass("active");
        //});


    },
    //removeCart: function (productId) {
    //    if (confirm("Bạn chắc chắn muốn xóa sản phẩm này chứ?")) {
    //        $.ajax({
    //            type: "post",
    //            cache: false,
    //            url: "/Cart/RemoveCart",
    //            data: {
    //                "ProductId": productId,
    //            },
    //            beforeSend: function () {
    //                common.loading();
    //                common.noScroll();
    //            },
    //            success: function (data) {
    //                if (data.Error)
    //                    alert(data.Title);
    //                else {
    //                    location.reload(true);
    //                }
    //            },
    //            complete: function () {
    //                common.loaded();
    //                common.noScroll();
    //            }
    //        })
    //    }
    //},
    //addToCart: function (type) {
    //    var quantity = $('.product-detail .cart-qty-input').val();
    //    var productID = $('#hddProductId').val();
    //    $.ajax({
    //        type: "post",
    //        cache: false,
    //        url: "/Cart/AddCart",
    //        data: {
    //            "ProductId": productID,
    //            "Quantity": quantity
    //        },
    //        beforeSend: function () {
    //            common.loading();
    //            common.noScroll();
    //        },
    //        success: function (data) {
    //            if (data.Error)
    //                alert(data.Title);
    //            else {
    //                alert(data.Title);
    //                if (type == 1)
    //                    location.reload(true);
    //                else
    //                    location.href = "/gio-hang";
    //            }
    //        },
    //        complete: function () {
    //            common.loaded();
    //            common.noScroll();
    //        }
    //    })
    //},
    //updateToCart: function (productId, type) {
    //    var quantity = $('.product-detail .cart-qty-input').val();
    //    var productID = $('#hddProductId').val();
    //    $.ajax({
    //        type: "post",
    //        cache: false,
    //        url: "/Cart/UpdateCart",
    //        data: {
    //            "productId": productId,
    //            "type": type
    //        },
    //        success: function (data) {
    //            if (data.Error)
    //                alert(data.Title);
    //            else {
    //                location.reload(true);
    //            }
    //        }
    //    })
    //},
    initForm: function () {
        $('button[type=reset]').click(function () {
            $('.field-validation-error').empty();
        });
        $('button[type=submit]').click(function () {
            //var flag = false;
            //var form = $(this).parents('form:first');
            //if (form.attr("id").match("searchForm")) {
            //    flag = true;
            //}
            //console.log(form.attr('action'),22);
            $(form).off("submit");
            if (form.valid()) {
                form.submit(function (e) {
                    var data = form.serialize();
                    if (this.beenSubmitted == data)
                        return false;
                    else {
                        common.loading();
                        this.beenSubmitted = data;
                        $.post(form.attr('action'), data, function (response) {
                            //console.log(response);
                            if (response.Error) {
                                notifies.messageError(response.Message);
                            }
                            else {
                                switch (response.NextAction) {
                                    case 1:
                                        notifies.messageSuccessPosting(response.Message);
                                        break;
                                    case 2:
                                        location.href = response.Message;
                                        break;
                                    default:
                                    
                                        break;
                                }
                            }
                            common.loaded();
                        });
                        return false;
                    }
                });
            } else {
                $('#lblMessage').empty();
                return false;
            }
        });
    },
    initFormPopup: function () {
        var form = $('#popup form:first');
        if (form.valid()) {
            form.submit(function (e) {
                var data = form.serialize();
                if (this.beenSubmitted == data)
                    return false;
                else {
                    common.loading();
                    this.beenSubmitted = data;
                    $.post(form.attr('action'), data, function (response) {
                        if (!response.Success) {
                            notifies.messageError(response.Message, form);
                        }
                        else {
                            switch (response.NextAction) {
                                case 1:
                                    //alert(1);
                                    notifies.messageSuccessPosting(response.Title);
                                    break;
                                default:
                                    notifies.messageError(response.Title, form);
                                    break;
                            }
                        }
                        common.loaded();
                    });
                    return false;
                }
            });
        }
    },
    fixedMenuScroll: function () {
        if ($(document).height() - $(window).height() > 300) {
            $(window).scroll(function () {
                if ($(window).scrollTop() >= 80) {
                    $(".back-to-top").addClass("active");
                    $('.back-to-top').fadeIn(200);
                } else if ($(".back-to-top").hasClass('active')) {
                    $(".back-to-top").removeClass("active");
                    $('.back-to-top').fadeOut(200);
                }
            });
        }
    },
    backToTop: function () {
        //$(window).scroll(function () {
        //    if ($(this).scrollTop() > 200) {
        //        $('.go-top').fadeIn(200);
        //    } else {
        //        $('.go-top').fadeOut(200);
        //    }
        //});
        $('.back-to-top').click(function (event) {
            $('html, body').animate({ scrollTop: 0 }, 300);
        })
    },
    search: function () {
        var keyword = $.trim($('#keywordSearch').val());
        var reg = new RegExp("(~|!|#|\\$|%|\\^|&|\\*|\\(|\\)|_|\\+|\\{|\\}|\\||\"|:|\\?|>|<|,|\\.|\\/|;|'|\\\|[|]|=|-)", "gi");
        var regexSpec = /[^\w\s]+/g;
        keyword = keyword.replace(reg, '');
        if (keyword.length > 0) {
            var link = "/tim-kiem-san-pham?keyword=" + keyword;
            location.href = link;
        } else {
            $('#keywordSearch').focus();
        }
    },
    openPopup: function () {
        $("body").on("click", ".btn-promo", function (e) {
            e.preventDefault();
            var me = $(this);
            $.ajax({
                type: "POST",
                cache: false,
                url: "/Contact/FormContact/",
                //data: {
                //    "articleId": 0,
                //    "articleType": 4,
                //    "brandId": brandId,
                //    "modelId": modelId,
                //    "year": year,
                //    "dataPage": dataPage
                //},
                success: function (data) {
                    $('#popup').html(data);
                    $('body').attr("data-button-name", me.text());
                },
                beforeSend: function () {
                    common.loading();
                },
                complete: function () {
                    //productCommon.initInformmationLead();
                    common.loaded();
                    common.noScroll();
                    //newCarCommon.clickonCall(me);
                }
            });
        });
    }

};


var common = {
    loading: function () {
        $('#loading').html('<div class="bg-popup-loading"> <img src="/Content/images/loading300.gif" class="loading"/></div>');
        common.noScroll();
    },
    loaded: function () {
        $('#loading').empty();
        common.autoScroll();
    },
    noScroll: function () {
        var width = $('body').width();
        $('body').css('overflow', 'hidden');
        var scrollWidth = $('body').width() - width;
        $('body').css('margin-right', scrollWidth + 'px');
    },
    autoScroll: function () {
        $('body').removeAttr('style');
    },
    closePopup: function () {
        $('#popup').empty();
        common.autoScroll();
    },
    closeRegister: function () {
        $('#agencyRegister').empty();
        cookie_manager.set_cookie("BannerPopupIds", 1, 1);
    }
};

var notifys = {
    createMessageError: function (strMessage) {
        var message = $("#lblMessage");
        message.html("<div class=\"field-validation-error red-clr\">" + strMessage + "</div>");
    },
    createMessageErrorPopup: function (strMessage, form) {
        var message = $(form).find("#lblMessage");
        message.html("<div class=\"field-validation-error red-clr\">" + strMessage + "</div>");
    },
    reloadPage: function (strMessage) {
        alert(strMessage);
        location.reload(true);
    }
};


$.fn.osgslide = function (smallThumb, largeThumb, movepx, counttab, countimage) {
    var objId = $(this).attr('id');
    var tabindex = 1;
    var imageindex = 0;
    //Sự kiện next ảnh
    $('#' + objId + ' .slide-next').click(function () {
        if ($(this).hasClass('noneclick')) {
            tabindex = 0;
            imageindex = -1;
            $(this).removeClass('noneclick');
            //return;
        }
        $('#' + objId + ' .slide-prev').removeClass('noneclick');
        imageindex++;
        if ((countimage > counttab) && (tabindex <= (countimage - counttab))) {
            tabindex++;
            var toppx = (tabindex - 1) * (-movepx);
            $('#' + objId + ' .list-thumb').animate({
                top: toppx
            }, 500);
        }
        ReplaceImage(imageindex);
        CountImage(imageindex + 1);
        if (imageindex == countimage - 1) {
            $(this).addClass('noneclick');
        } else if (imageindex == 0) {
            $('#' + objId + ' .slide-prev').addClass('noneclick');
        }
        getTitleImage();
    });

    //Sự kiện back ảnh
    $('#' + objId + ' .slide-prev').click(function () {
        var countImg = $('#' + objId + ' .multiple-img-list .osgslideimg').length;
        if ($(this).hasClass('noneclick')) {
            tabindex = countImg - (counttab - 2);
            imageindex = countImg;
            $(this).removeClass('noneclick');
            //return;
        }
        $('#' + objId + ' .slide-next').removeClass('noneclick');
        imageindex--;
        if ((countimage > counttab) && tabindex > 1) {
            tabindex--;
            var toppx = (tabindex - 1) * (-movepx);

            $('#' + objId + ' .list-thumb').animate({
                top: toppx
            }, 500);
        }
        ReplaceImage(imageindex);
        CountImage(imageindex + 1);
        if (imageindex == 0) {
            $(this).addClass('noneclick');
        } else if (imageindex == countImg - 1) {
            $('#' + objId + ' .slide-next').addClass('noneclick');
        }
        getTitleImage();
    });

    //Sự kiện khi click vào ảnh nhỏ
    $('#' + objId + ' .osgslideimg img').click(function () {
        var currentimage = $(this).parent().parent().index();
        ReplaceImage(currentimage);
        currentimage++;
        CountImage(currentimage);
        $('#' + objId + ' .slide-prev').removeClass('noneclick');
        $('#' + objId + ' .slide-next').removeClass('noneclick');
        if (currentimage == 1) {
            $('#' + objId + ' .slide-prev').addClass('noneclick');
        } else if (currentimage == $('#' + objId + ' .multiple-img-list .osgslideimg').length) {
            $('#' + objId + ' .slide-next').addClass('noneclick');
        }
        getTitleImage();
    });

    //Sự kiện khi click vào ảnh lớn
    $('#' + objId + ' .show-img img').click(function () {
        var rel = $(this).attr('rel');
        $('#' + objId + ' #' + rel).click();
    });

    $('#' + objId + ' .btn-scale').click(function () {
        var rel = $('#' + objId + ' .osgslideimg.active img').attr("rel");
        $('#' + objId + ' #' + rel).click();
    });

    //Func thay thế ảnh nhỏ => ảnh lớn
    function ReplaceImage(currentimage) {
        imageindex = currentimage;
        var src = $('#' + objId + ' .osgslideimg:eq(' + currentimage + ') img').attr('src');
        var rel = $('#' + objId + ' .osgslideimg:eq(' + currentimage + ') img').attr('rel');
        var attr = $('#' + objId + ' .osgslideimg:eq(' + currentimage + ') img').attr('class');
        var cls = "";
        if (typeof attr !== typeof undefined && attr !== false) {
            var cls = attr;
        }
        $('#' + objId + ' .show-img img')
            .attr('src', src.replace(smallThumb, largeThumb))
            .attr('rel', rel)
            .attr('class', cls);
        $('#' + objId + ' .sale-img').hide();

        $('#' + objId + ' .osgslideimg').removeClass('active');
        $('#' + objId + ' .osgslideimg:eq(' + currentimage + ')').addClass('active');
    }

    function CountImage(currentimage) {
        $('#countimage').html(currentimage);
    }

    //function getTitleImage() {
    //    var item = '#osgslide .show-img a img';
    //    var idCurrent = $(item).attr('rel');
    //    var title = $('#' + idCurrent).attr('data-desc');
    //    $(item).attr("title", title);
    //    $(item).attr("alt", title);
    //}
};


function formatVND(n) {
    return n.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, '$1,').replace(".00", "") + " VND";
}



/* Cookie manager */
var cookie_manager = cookie_manager || {};
cookie_manager = {
    set_cookie: function (cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    },
    get_cookie: function (cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    },
    set_cookie_banner: function (bannerId) {
        var listIdBannerPopup = [];
        var listbannerID = cookie_manager.get_cookie("BannerPopupIds");
        if (listbannerID != undefined && listbannerID.length > 0) {
            listIdBannerPopup = JSON.parse(listbannerID);
        }
        if (!listIdBannerPopup.includes(bannerId)) {
            listIdBannerPopup.push(bannerId);
            cookie_manager.set_cookie("BannerPopupIds", JSON.stringify(listIdBannerPopup), 365);
        }
    }
};


var productList = {
    page: 2,
    pageSize: 16,
    eleCheckTop: null,
    timerTimeline: null,
    last_scroll: 0,
    flag: false,
    url: '',
    delay: 1200,
    init: function () {
        var me = this;
        me.eleCheckTop = me.eleCheckTop || $("#elechecktop");
        if (me.eleCheckTop != null && me.eleCheckTop.length > 0) {
            $(window).on("scroll", me.scrollInit);
        }
        me.url = $("#hdRawUrl").val() || (window.location.pathname + window.location.search);
        var pMatch = me.url.match(/\/p[0-9]+/);
        if (pMatch) {
            var newPage = parseInt(pMatch[0].replace("/p", ""));
            if (newPage > 0) {
                me.page = newPage + 1;
            }
        }
    },

    filter: function (page) {
        var me = this;

        if (me.flag) return;
        me.flag = true;

        var newurl = me.url.replace(/\/p[0-9]+/, '');
        newurl = newurl.indexOf('?') != -1
            ? newurl.substr(0, newurl.indexOf('?')) + "/p" + page + newurl.substr(newurl.indexOf('?')) + "&loadmore=1"
            : newurl + "/p" + page + "?loadmore=1"
            ;
        //alert(newurl)
        $.ajax({
            type: "POST",
            url: newurl,
            beforeSend: function () {
                me.showLoading(true);
            },
            success: function (data) {
                //alert(13)
                if (data.length > 20 && data.indexOf("html") == -1) {
                    setTimeout(function () {
                        var $wrap = $(".list-item-page");
                        if (page > 1) {
                            $wrap.append(data);
                        }
                        else {
                            $wrap.html(data);
                        }
                        me.flag = false;
                        me.page++;
                        me.stop++;
                        me.showLoading(false);
                    }, 1200);
                }
                else {
                    $(window).off("scroll", me.scrollInit);
                    setTimeout(function () {
                        $(".carloading").remove();
                    }, me.delay);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                me.flag = false;
            }
        });


    },
    scrollInit: function (e) {
        var me = productList;

        if (me.timerTimeline) {
            clearTimeout(me.timerTimeline);
            me.timerTimeline = null;
        }
        me.timerTimeline = setTimeout(function () {
            var maxH = me.eleCheckTop.offset().top + me.eleCheckTop.height() - window.innerHeight / 2;
            var heightToLoad = $(this).scrollTop() + $(this).height();
            console.log(maxH, heightToLoad);
            if (heightToLoad >= maxH) {
                //alert(1);
                me.filter(me.page);

            }
        }, 150);
    },
    onNavScroll: function () {
        var me = comment, scrollPos, refElement;

        if (me.timerNav) {
            clearTimeout(me.timerNav);
            me.timerNav = null;
        }
        me.timerNav = setTimeout(function () {
            scrollPos = $(this).scrollTop() + 130;
            me.nav.each(function () {
                refElement = $('' + $(this).attr('href') + '');
                if (refElement.length > 0) {
                    if (refElement.offset().top <= scrollPos && refElement.offset().top + refElement.height() >= scrollPos) {
                        me.nav.removeClass('active');
                        $(this).addClass("active");
                    }
                    else {
                        $(this).removeClass("active");
                    }
                }
            });
        }, 50);
    },

    reset: function () {
        this.page = 1;
        this.stop = 0;
    },

    loadmore: function () {
        this.filter(this.page);
    },
    showLoading: function (isShow) {
        var $fbLoading = $(".carloading .fb-loading"), $btnLoadMore = $(".carloading > div:last");
        if (isShow) {
            $fbLoading.removeClass("hidden");
            $btnLoadMore.addClass("hidden");
        }
        else {
            $fbLoading.addClass("hidden");
            $btnLoadMore.removeClass("hidden");
        }
    },

    //checkSroll: function () {
    //    var me = this;
    //    me.eleCheckTop = me.eleCheckTop || $("#newcarchecktop");
    //    if (me.eleCheckTop != null && me.eleCheckTop.length > 0) {
    //        if (me.wrap == null)
    //            me.wrap = $("#wraploadComment");
    //        var lstItem = me.wrap.find(".active .box-comment .box-cmt");
    //        console.log(lstItem.length, 998)
    //        if (lstItem.length >= me.pageSize)
    //            $(window).on("scroll", me.scrollInit);
    //    }
    //}

};