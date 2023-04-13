if (typeof (CKEDITOR) != "undefined") {
	CKEDITOR.editorConfig = function (config) {
		config.language = 'vi';
		config.height = "200px";
		config.toolbarCanCollapse = true;
		//config.toolbarStartupExpanded = false;
		config.extraPlugins = 'youtube,dvs.image';
		config.toolbar = [
            ['Bold', 'Italic', 'Underline'],
            ['JustifyLeft', 'JustifyCenter', 'JustifyRight'],
            ['Link', 'Maximize', 'ShowBlocks', 'Preview']
		];
        config.allowedContent = true;
        config.pasteFromWordRemoveFontStyles = false;
        config.pasteFromWordRemoveStyles = false;
	};
}
