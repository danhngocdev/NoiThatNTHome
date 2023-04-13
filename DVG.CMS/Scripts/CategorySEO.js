//BOXSEO---------------
var BaseUrl = 'http://tinxe.vn/';


function CountKeywordsMatch(content, item) {
	var pat = new RegExp(item, "gi");
	var pat2 = new RegExp(item + "[a-z]", "gi");
	var matchResult = RemoveHtmlTag(content).match(pat);
	var match2 = content.match(pat2);
	if (matchResult != null) {
		if (match2 != null) {
			count = matchResult.length - match2.length;
		}
		else {
			count = matchResult.length
		}
		return count;
	}
	else {
		return 0;
	}
}
function RemoveHtmlTag(content) {
	var regex = /(<([^>]+)>)/ig;
	var result = content.replace(regex, "");
	return result;
}
function UnicodeToUnsignAndSlash(s) {
	if (s == null || s == "") return "";
	strChar = "abcdefghijklmnopqrstxyzuvxw0123456789 ";
	s = UnicodeToUnsign(s.toLowerCase());
	sReturn = ""; for (i = 0; i < s.length; i++) { if (strChar.indexOf(s.charAt(i)) > -1) { if (s.charAt(i) != ' ') { sReturn += s.charAt(i); } else if (i > 0 && s.charAt(i - 1) != ' ' && s.charAt(i - 1) != '-') { sReturn += "-"; } } } return sReturn;
}
function UnicodeToUnsign(s) {
	uniChars = "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";
	UnsignChars = "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";
	retVal = ""; for (i = 0; i < s.length; i++) { pos = uniChars.indexOf(s.charAt(i)); if (pos >= 0) { retVal += UnsignChars.charAt(pos); } else { retVal += s.charAt(i); } } return retVal;
}

function InitSEO() {

	var elem = $("#metaTitleLimit");
	var idmessenger1 = $("#metaTitleLimitMessenger");
	var elem2 = $("#metaDesLimit");
	var idmessenger2 = $("#metaDesLimitMessenger");

	$('#txtMetaTitle').limiter(58, elem, idmessenger1);
	$('#txtMetaDesc').limiter(150, elem2, idmessenger2);
}
//---------------------
(function (jQuery) {

    jQuery.fn.extend({
        limiter: function (limit, elem, idmessenger) {
            jQuery(this)
                .on("keyup focus", function () {
                    setCount(this, elem, idmessenger);

                });

            function setCount(src, elem, idmessenger) {
                var chars = src.value.length;
                //                if (chars > limit) {
                //                    src.value = src.value.substr(0, limit);
                //                    chars = limit;
                //                }

                if (limit - chars < 0) {
                    jQuery(idmessenger).css("display", "block");
                } else {
                    jQuery(idmessenger).css("display", "none");
                }
                elem.html(limit - chars);

            }
            setCount(jQuery(this)[0], elem, idmessenger);

        }
    });
})(jQuery);