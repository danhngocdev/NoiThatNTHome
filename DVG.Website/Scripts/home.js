/*
DES: Scripts for homepage
*/

var home = {
    init: function () {
        // Swiper Configuration
        var bannerHome = $("#bannerHomePage");
        if (bannerHome.length >0) {
            var swiper = new Swiper("#bannerHomePage .swiper-container", {
                slidesPerView: 1,
                spaceBetween: 10,
                centeredSlides: true,
                freeMode: true,
                grabCursor: true,
                loop: true,
                pagination: {
                    el: "#bannerHomePage .swiper-pagination",
                    clickable: true
                },
                autoplay: {
                    delay: 4000,
                    disableOnInteraction: false
                },
                //navigation: {
                //    nextEl: ".swiper-button-next",
                //    prevEl: ".swiper-button-prev"
                //},
                breakpoints: {
                    500: {
                        slidesPerView: 1
                    },
                    700: {
                        slidesPerView: 1
                    }
                }
            });
        }
    }
}