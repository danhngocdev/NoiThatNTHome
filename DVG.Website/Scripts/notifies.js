/*
DES: Scripts notifys
*/

var notifies = {

    messageError: function (message) {
        var htmlPopup = '';
        htmlPopup += '<div class="popup-loan-package leadpopup campaign-leads"</div>';
        htmlPopup += `<div class="bca-popup">
                             <div class="header-title">
                                <span class="txt">Gửi Thông Tin Thất Bại</span>
                                <span class="btn_close" onclick="common.closePopup();"><i class="fa-window-close"></i></span>
                            </div>
                                <div class="page-wrapper-popup">
                                <div class="custom-modal">
                                    <div class="danger danger-animation icon-top"><i class="fa fa-times"></i></div>
                                    <div class="danger border-bottom"></div>
                                    <div class="content">
                                        <p class="type">Thất Bại</p>
                                        <p class="message-type">Bạn Vui Lòng Thử Lại</p>
                                    </div>
                                </div>
                            </div>
                      </div>`;
        $('#popup').html(htmlPopup);
        common.noScroll();
    },
    messageSuccessPosting: function () {
        var htmlPopup = '';
        htmlPopup += '<div class="popup-loan-package leadpopup campaign-leads"</div>';
        htmlPopup += `<div class="bca-popup">
                             <div class="header-title">
                                <span class="txt">Gửi Thông Tin Thành Công</span>
                                <span class="btn_close" onclick="common.closePopup();"><i class="fa-window-close"></i></span>
                            </div>
                                <div class="page-wrapper-popup">
                                <div class="custom-modal">
                                    <div class="succes succes-animation icon-top"><i class="fa fa-check"></i></div>
                                    <div class="succes border-bottom"></div>
                                    <div class="content">
                                        <p class="type">Thành Cônng</p>
                                        <p class="message-type">Chúng Tôi Sẽ Liên Hệ Lại Với Bạn</p>
                                    </div>
                                </div>
                            </div>
                      </div>`;
        $('#popup').html(htmlPopup);
        common.noScroll();
    }
}

  