CKEDITOR.dialog.add('VideoDialog', function (editor) {
	return {
		title: 'Video',
		minWidth: 300,
		minHeight: 100,
		contents: [
					{
						id: 'tab-video',
						padding: 0,
						height: 250,
						width: 300,
						elements:
						[
							{
								type: 'hbox',
								widths: ['50%', '50%'],
								children:
								[
									{
										type: 'button',
										id: 'btnUpload',
										label: 'Upload file sub',
										onClick: function () {
											var w = window.open("/FileManager/Default.aspx?ac=subtitle", "File manager", "width = 950, height = 600");

											w.callback = function (result) {
												if (!result || !result.Result) {
													alert("Không có file nào được chọn!");

													return;
												}

												if (result.FullPath.lastIndexOf(".srt") == -1) {
													alert("Bạn cần chọn tệp subtitle");

													return;
												}

												CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtLinkSubtitle", result.FullPath);
												w.close();
											};
										}
									},
									{
										type: 'button',
										id: 'btnUploadImage',
										label: 'Upload ảnh nền',
										onClick: function () {
											var w = window.open("/FileManager/Default.aspx?ac=article", "File manager", "width = 950, height = 600");

											w.callback = function (result) {
												if (!result || !result.Result) {
													alert("Không có file nào được chọn!");

													return;
												}

												if (/(\.|\/)(gif|jpe?g|png)$/i.exec(result.FullPath) == null) {
													alert("Bạn cần chọn hình ảnh");
													return;
												}

												CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtImageUrl", result.FullPath);
												w.close();
											};
										}
									}
								]
							},
                            {
                            	type: 'text',
                            	id: 'txtLinkVideo',
                            	label: 'Link video',
                            	validate: CKEDITOR.dialog.validate.notEmpty("Vui lòng nhập link video!"),
                            	setup: function (e) {
                            		var value = e.getAttribute("data-video");

                            		if (!value) {
                            			return;
                            		}

                            		CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtLinkVideo", value)
                            	}
                            },
                            {
                            	type: 'text',
                            	id: 'txtLinkSubtitle',
                            	label: 'Link subtitle',
                            	//validate: CKEDITOR.dialog.validate.notEmpty("Link subtitle field cannot be empty"),
                            	setup: function (e) {
                            		var value = e.getAttribute("data-subtitle");

                            		if (!value) {
                            			return;
                            		}

                            		CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtLinkSubtitle", value)
                            	}
                            },
                            {
                            	type: 'text',
                            	id: 'txtWidth',
                            	label: 'Chiều rộng',
                            	validate: CKEDITOR.dialog.validate.notEmpty("Vui lòng nhập chiều rộng khung video!"),
                            	setup: function (e) {
                            		var value = e.getAttribute("data-width");

                            		if (!value) {
                            			return;
                            		}

                            		CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtWidth", value)
                            	}
                            },
                            {
                            	type: 'text',
                            	id: 'txtHeight',
                            	label: 'Chiều cao',
                            	validate: CKEDITOR.dialog.validate.notEmpty("Vui lòng nhập chiều cao khung video!"),
                            	setup: function (e) {
                            		var value = e.getAttribute("data-height");

                            		if (!value) {
                            			return;
                            		}

                            		CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtHeight", value)
                            	}
                            },
                            {
                            	type: 'text',
                            	id: 'txtImageUrl',
                            	label: 'Link ảnh nền',
                            	//validate: CKEDITOR.dialog.validate.notEmpty("Vui lòng nhập chiều cao khung video!"),
                            	setup: function (e) {
                            		var value = e.getAttribute("data-image");

                            		if (!value) {
                            			return;
                            		}

                            		CKEDITOR.dialog.getCurrent().setValueOf("tab-video", "txtImageUrl", value)
                            	}
                            }
						]
					}
		],
		onLoad: function () {
			/*fmClient.IsEditor = true;
            fmClient.SelectedControl = editor.name;*/
		},
		onShow: function () {
			var selection = editor.getSelection();
			var element = selection.getStartElement();

			if (element) {
				element = element.getAscendant('div', true);
			}

			if (!element
                || element.getName() != 'div'
                || element.getAttribute("data-type") != "jwplayer") {
				element = editor.document.createElement('div');
				this.insertMode = true;
			} else {
				this.insertMode = false;

				this.setupContent(element);
			}

			this.element = element;
		},
		buttons: [CKEDITOR.dialog.okButton, CKEDITOR.dialog.cancelButton],
		onOk: function (e) {
			/*var dialog = this;
            var videoLink = dialog.getValueOf('tab-video', 'txtLinkVideo');
            var subtitleLink = dialog.getValueOf('tab-video', 'txtLinkSubtitle');
            var width = dialog.getValueOf('tab-video', 'txtWidth');
            var height = dialog.getValueOf('tab-video', 'txtHeight');
            var jwId = "player" + new Date().getTime();
            var element = dialog.element;//editor.document.createElement('div');
            var template = "<div id='" + jwId + "'><img src = '/assets/img/video.png' alt='video' /></div><script type='text/javascript'>jwplayer('" + jwId + "').setup({ width: " + width + ", height: " + height + ", controlbar: 'bottom', playlist: [{ captions: [ { file: '" + subtitleLink + "', label: 'English', 'default': true } ], file: '" + videoLink + "' }] });</script>";
            element.setAttribute("data-type", "jwplayer");
            element.setAttribute("data-video", videoLink);
            element.setAttribute("data-subtitle", subtitleLink);
            element.setAttribute("data-width", width);
            element.setAttribute("data-height", height);
            element.setAttribute("contenteditable", false);
            element.setAttribute("style", "width: " + width + "px; height: " + height + "px; text-align: center; border: 1px solid #CCC; margin: 5px auto;");
            element.setHtml(template);
            dialog.commitContent(element);

            if (dialog.insertMode) {
                editor.insertElement(element);
            }*/

			var dialog = this;
			var defaultimagevideo = '/Content/images/no-image.png';
			var videoLink = dialog.getValueOf('tab-video', 'txtLinkVideo');
			var subtitleLink = dialog.getValueOf('tab-video', 'txtLinkSubtitle');
			var width = dialog.getValueOf('tab-video', 'txtWidth');
			var height = dialog.getValueOf('tab-video', 'txtHeight');
			var imageback = dialog.getValueOf('tab-video', 'txtImageUrl');
			if (imageback == "" || imageback == null) {
				imageback = defaultimagevideo;
			}
			var jwId = "player" + new Date().getTime();
			var element = dialog.element;
			element.setAttribute("id", jwId);
			element.setAttribute("data-file", videoLink);
			element.setAttribute("data-subtitle", subtitleLink);
			element.setAttribute("data-width", width);
			element.setAttribute("data-height", height);
			element.setAttribute("data-image", imageback);
			element.setAttribute("style", "width: " + width + "px; height: " + height + "px; text-align: center; border: 1px solid #CCC; margin: 5px auto;");
			element.setHtml('<img src="' + imageback + '" style="width: ' + width + 'px; height: ' + height + 'px" />');
			element.setAttribute("class", "dvsvideo");
			dialog.commitContent(element);

			if (dialog.insertMode) {
				editor.insertElement(element);
			}
		}
	};
});