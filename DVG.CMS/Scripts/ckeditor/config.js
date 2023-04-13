/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
    config.extraPlugins = 'widget,filetools,notificationaggregator,notification,widgetselection,lineutils,uploadwidget,uploadimage'; //widget,filetools,notificationaggregator,notification,widgetselection,lineutils,uploadwidget,uploadimage
    config.allowedContent = true;
    config.uploadUrl = 'https://upload.tinxe.vn/UploadHandler.php';
};
