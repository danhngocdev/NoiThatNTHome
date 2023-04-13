CKEDITOR.plugins.add('dvs.checkspell', {
    init: function (editor) {
        editor.addCommand('dvs.checkspell', {
            exec: function (editor) {
                $('.loader-container').show();
                var html = editor.document.getBody().getHtml();
                var data = html.replace(/<[^>]+>/g, '\n').replace(/\n\s*\n/g, '\n\n');;
                $.ajax({
                    url: cms.configs.SpellCheckerApiDomain + "?input=" + encodeURIComponent(data),
                    type: "GET",
                    dataType: "json",
                    //async: false,
                    success: function (result) {
                        if (!result || result.status != 'SUCCESS') {
                            $('.loader-container').hide();
                            alert("Lỗi gọi API");
                            return;
                        }

                        if (result.code == '1') {
                            var newContent = data;
                            if (result.mapcorrect.length > 0) {
                                console.log(result);
                                var sortedList = result.mapcorrect.sort(function (a, b) { return (a.index > b.index) ? 1 : ((b.index > a.index) ? -1 : 0); });
                                console.log(sortedList);
                                for (var i = 0; i < sortedList.length; i++) {
                                    var index = sortedList[i].index;
                                    var begin = newContent.substring(0, index);
                                    var end = newContent.substring(index, newContent.length);
                                    end = end.replace(result.mapcorrect[i].wrong, '<i class="bg-danger text-muted" data-toggle="tooltip" data-placement="top" title="Gợi ý: ' + result.mapcorrect[i].corrected + '">' + result.mapcorrect[i].wrong + '</i>');
                                    newContent = begin + end;
                                }
                            }
                            newContent = newContent.replaceAll('&nbsp;', ' ').replaceAll('\n', '<br />');
                            $('#rightInputContent').hide();
                            $('#popupSpellingCheck').show();
                            $('#popupSpellingCheckResult').html(newContent);     
                            $('#alertSpellingCheckFounded').fadeIn(function () {
                                setTimeout(function () {
                                    $('#alertSpellingCheckFounded').fadeOut();
                                }, 3000);
                            });
                        }
                        else {
                            $('#alertSpellingCheckNotFound').fadeIn(function () {
                                setTimeout(function () {
                                    $('#alertSpellingCheckNotFound').fadeOut();
                                }, 3000);
                            });
                            $('#popupSpellingCheck').hide();
                            $('#rightInputContent').show();
                        }
                        $('.loader-container').hide();
                    },
                    error: function () {
                        $('.loader-container').hide();
                        alert("Lỗi gọi API");
                    }
                });
            }
        });
        editor.ui.addButton('dvs.checkspell', {
            label: 'Check lỗi chính tả',
            command: 'dvs.checkspell',
            toolbar: 'insert',
            icon: this.path + 'icons/spell-check-16.png'
        });
    }
});

String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};
