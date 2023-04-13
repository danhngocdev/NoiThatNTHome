using DVG.WIS.Utilities;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace DVG.CMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            CssFixRewriteUrlTransform rewriteUrlTransform = new CssFixRewriteUrlTransform();
            StyleBundle stylesJqueryUi = new StyleBundle("~/WIS/styles/JqueryUi");
            StyleBundle stylesTemplate = new StyleBundle("~/WIS/styles/Template");
            StyleBundle stylesExternal = new StyleBundle("~/WIS/styles/External");

            ScriptBundle scriptsConfigs = new ScriptBundle("~/WIS/scripts/Configs");
            ScriptBundle scriptsTemplate = new ScriptBundle("~/WIS/scripts/Template");
            ScriptBundle scriptsJquery = new ScriptBundle("~/WIS/scripts/Jquery");
            ScriptBundle scriptsJqueryUi = new ScriptBundle("~/WIS/scripts/JqueryUi");
            ScriptBundle scriptsTether = new ScriptBundle("~/WIS/scripts/Tether");
            ScriptBundle scriptsAngular = new ScriptBundle("~/WIS/scripts/Angular");
            ScriptBundle scriptsAngularPlugin = new ScriptBundle("~/WIS/scripts/AngularPlugin");
            ScriptBundle scriptsAngularCustomize = new ScriptBundle("~/WIS/scripts/AngularCustomize");
            ScriptBundle scriptsExternal = new ScriptBundle("~/WIS/scripts/External");


            stylesJqueryUi.Include("~/Scripts/jQueryUI/jquery-ui-1.12.1.min.css");

            stylesTemplate.Include("~/Content/angular-notify.min.css",
                                    "~/Content/angular-confirm.min.css",
                                    "~/Content/ng-animation.css",
                                    "~/Content/font-awesome.min.css",
                                    "~/Content/simple-line-icons.css",
                                    "~/Content/style.css",
                                    "~/Content/site.css");

            stylesExternal.Include("~/Content/bootstrap-datetimepicker.min.css",
               "~/Content/JqueryChoosen/chosen.min.css",
               "~/Content/JqueryChoosen/bootstrap-tagsinput.css",
               "~/Scripts/json-formatter/json-formatter.min.css");

            scriptsJquery.Include("~/Scripts/jquery-2.2.3.min.js",
                                "~/Scripts/popper.min.js",
                                "~/Scripts/bootstrap.min.js",
                                "~/Scripts/perfect-scrollbar.min.js",
                                "~/Scripts/coreui.min.js");

            scriptsJqueryUi.Include("~/Scripts/jQueryUI/jquery-ui-1.12.1.min.js");
            scriptsTether.Include("~/Scripts/tether.min.js");

            scriptsAngular.Include("~/Scripts/angular/angular.min.js",
                                    "~/Scripts/angular/angular-route.js",
                                    "~/Scripts/angular/angular-sanitize.min.js",
                                    "~/Scripts/angular/angular-animate.js",
                                    "~/Scripts/angular/angular-messages.min",
                                    "~/Scripts/angular/angular-mocks.js",
                                    "~/Scripts/angular/angular-notify.js",
                                    "~/Scripts/angular/ui-bootstrap.min.js",
                                    "~/Scripts/angular/ui-bootstrap-custom-tpls-0.10.0.min.js",
                                    "~/Scripts/angular/ui-bootstrap-tpls.min.js",
                                    "~/Scripts/angular/angular-confirm.min.js",
                                    "~/Scripts/angular/angular-drag-and-drop-lists.js",
                                    "~/Scripts/angular/angular-sortable.min.js");
            scriptsExternal.Include(
                "~/Scripts/bootstrap-datetimepicker/moment.js",
                "~/Scripts/bootstrap-datetimepicker/bootstrap-datetimepicker.min.js",
                //"~/Scripts/bootstrap-datetimepicker/bootstrap-datetimepicker.v.4.0.js",
                "~/Scripts/bootstrap-datetimepicker/locale/vi.js",
                "~/Scripts/JqueryChoosen/chosen.jquery.min.js",
                "~/Scripts/bootstrap-tagsinput.min.js",
                "~/Scripts/nicescroll/jquery.nicescroll.js",
                "~/Scripts/json-formatter/json-formatter.min.js");
            scriptsAngularPlugin.Include("~/Scripts/angular/angular-ui-router.min.js",
                                            "~/Scripts/angular/ocLazyLoad.min.js",
                                            "~/Scripts/angular/angular-breadcrumb.min.js",
                                            "~/Scripts/angular/loading-bar.min.js");
            scriptsConfigs.Include("~/Scripts/configs.js");
            scriptsAngularCustomize.Include("~/Scripts/ModulesExtension.js",
                                    "~/Scripts/validator.js");
            scriptsTemplate.Include("~/Scripts/template/app.js",
                                        "~/Scripts/template/directives.js",
                                        "~/Scripts/main.js");

            bundles.Add(stylesJqueryUi);
            bundles.Add(stylesTemplate);
            bundles.Add(stylesExternal);
            bundles.Add(scriptsJquery);
            bundles.Add(scriptsJqueryUi);
            bundles.Add(scriptsTether);
            bundles.Add(scriptsAngular);
            bundles.Add(scriptsAngularPlugin);
            bundles.Add(scriptsConfigs);
            bundles.Add(scriptsAngularCustomize);
            bundles.Add(scriptsTemplate);
            bundles.Add(scriptsExternal);

            #region Menu

            ScriptBundle scriptsMenu = new ScriptBundle("~/WIS/scripts/menu");
            scriptsMenu.Include("~/JsModel/MenuController.js", rewriteUrlTransform);
            bundles.Add(scriptsMenu);

            #endregion

            #region Account
            ScriptBundle scriptsAccount = new ScriptBundle("~/WIS/scripts/Account");
            scriptsAccount.Include("~/JsModel/AccountController.js", rewriteUrlTransform);
            bundles.Add(scriptsAccount);
            #endregion

            #region Users
            ScriptBundle scriptsUsers = new ScriptBundle("~/WIS/scripts/Users");
            scriptsUsers.Include("~/JsModel/UsersController.js", rewriteUrlTransform);
            bundles.Add(scriptsUsers);
            #endregion

            #region AuthGroup

            ScriptBundle scriptsAuthGroup = new ScriptBundle("~/WIS/scripts/AuthGroup");
            scriptsAuthGroup.Include("~/JsModel/AuthGroupController.js", rewriteUrlTransform);
            bundles.Add(scriptsAuthGroup);

            #endregion

            #region Category
            ScriptBundle scriptsCategory = new ScriptBundle("~/WIS/scripts/Category");
            scriptsCategory.Include("~/JsModel/CategoryController.js", rewriteUrlTransform);
            scriptsCategory.Include("~/Scripts/CategorySEO.js", rewriteUrlTransform);
            bundles.Add(scriptsCategory);
            #endregion

            #region Banner

            ScriptBundle scriptsBanner = new ScriptBundle("~/WIS/scripts/Banner");
            scriptsBanner.Include("~/JsModel/BannerController.js", rewriteUrlTransform);
            bundles.Add(scriptsBanner);

            #endregion  

            #region Recruitment

            ScriptBundle scriptsRecruitment = new ScriptBundle("~/WIS/scripts/Recruitment");
            scriptsRecruitment.Include("~/JsModel/RecruitmentController.js", rewriteUrlTransform);
            bundles.Add(scriptsRecruitment);

            #endregion


            #region Person

            ScriptBundle scriptsPerson = new ScriptBundle("~/WIS/scripts/Person");
            scriptsPerson.Include("~/JsModel/PersonController.js", rewriteUrlTransform);
            bundles.Add(scriptsPerson);

            #endregion

            #region Person

            ScriptBundle scriptsOrder = new ScriptBundle("~/WIS/scripts/Order");
            scriptsOrder.Include("~/JsModel/OrderController.js", rewriteUrlTransform);
            bundles.Add(scriptsOrder);

            #endregion


            #region Gallery

            ScriptBundle scriptsGallery = new ScriptBundle("~/WIS/scripts/Gallery");
            scriptsGallery.Include("~/JsModel/GalleryController.js", rewriteUrlTransform);
            bundles.Add(scriptsGallery);

            #endregion

            #region Customer

            ScriptBundle scriptsCustomer = new ScriptBundle("~/WIS/scripts/Customer");
            scriptsCustomer.Include("~/JsModel/CustomerController.js", rewriteUrlTransform);
            bundles.Add(scriptsCustomer);

            #endregion

            #region Subscribe
            ScriptBundle scriptsSubscribe = new ScriptBundle("~/WIS/scripts/Subscribe");
            scriptsSubscribe.Include("~/JsModel/SubscribeController.js", rewriteUrlTransform);
            bundles.Add(scriptsSubscribe);
            #endregion

            #region News
            ScriptBundle scriptsNews = new ScriptBundle("~/WIS/scripts/News");
            scriptsNews.Include("~/JsModel/NewsController.js", rewriteUrlTransform);
            bundles.Add(scriptsNews);
            #endregion

            #region NewsAction
            ScriptBundle scriptsNewsAction = new ScriptBundle("~/WIS/scripts/NewsAction");
            scriptsNewsAction.Include("~/Scripts/search-tags.js", rewriteUrlTransform);
            scriptsNewsAction.Include("~/JsModel/NewsActionController.js", rewriteUrlTransform);
            bundles.Add(scriptsNewsAction);
            #endregion

            #region Page
            ScriptBundle scriptsPageAction = new ScriptBundle("~/WIS/scripts/PageAction");
            scriptsPageAction.Include("~/JsModel/PageActionController.js", rewriteUrlTransform);
            bundles.Add(scriptsPageAction);
            #endregion

            #region Video
            ScriptBundle scriptsVideoAction = new ScriptBundle("~/WIS/scripts/VideoAction");
            scriptsVideoAction.Include("~/JsModel/VideoActionController.js", rewriteUrlTransform);
            bundles.Add(scriptsVideoAction);
            #endregion

            #region Product
            ScriptBundle scriptsProduct = new ScriptBundle("~/WIS/scripts/Product");
            scriptsProduct.Include("~/JsModel/ProductController.js", rewriteUrlTransform);
            bundles.Add(scriptsProduct);
            #endregion

            #region ProductAction
            ScriptBundle scriptsProductAction = new ScriptBundle("~/WIS/scripts/ProductAction");
            scriptsProductAction.Include("~/JsModel/ProductActionController.js", rewriteUrlTransform);
            bundles.Add(scriptsProductAction);
            #endregion


            #region SEO box
            ScriptBundle scriptsSEOBox = new ScriptBundle("~/WIS/scripts/SEOBox");
            scriptsSEOBox.Include("~/Scripts/SEO.js", rewriteUrlTransform);
            bundles.Add(scriptsSEOBox);
            #endregion


            #region PriceList
            ScriptBundle scriptsPriceList = new ScriptBundle("~/WIS/scripts/PriceList");
            scriptsPriceList.Include("~/JsModel/PriceListController.js", rewriteUrlTransform);
            //scriptsPriceList.Include("~/Scripts/CategorySEO.js", rewriteUrlTransform);
            bundles.Add(scriptsPriceList);
            #endregion



            #region Video
            ScriptBundle scriptsVideo = new ScriptBundle("~/WIS/scripts/Video");
            scriptsVideo.Include("~/JsModel/VideoController.js", rewriteUrlTransform);
            //scriptsPriceList.Include("~/Scripts/CategorySEO.js", rewriteUrlTransform);
            bundles.Add(scriptsVideo);
            #endregion

            #region ProductShowHome
            ScriptBundle scriptsProductShowHome = new ScriptBundle("~/WIS/scripts/ProductShowHome");
            scriptsProductShowHome.Include("~/JsModel/ProductShowHomeController.js", rewriteUrlTransform);
            //scriptsPriceList.Include("~/Scripts/CategorySEO.js", rewriteUrlTransform);
            bundles.Add(scriptsProductShowHome);
            #endregion


            #region InfoContact
            ScriptBundle scriptsContact = new ScriptBundle("~/WIS/scripts/InfoContact");
            scriptsContact.Include("~/JsModel/InfoContactController.js", rewriteUrlTransform);
            //scriptsPriceList.Include("~/Scripts/CategorySEO.js", rewriteUrlTransform);
            bundles.Add(scriptsContact);
            #endregion
            BundleTable.EnableOptimizations = AppSettings.Instance.GetBool("StaticsZipOptimize");
        }


        public class CssFixRewriteUrlTransform : IItemTransform
        {
            private static string ConvertUrlsToAbsolute(string baseUrl, string content)
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    return content;
                }
                var regex = new Regex("url\\(['\"]?(?<url>[^)]+?)['\"]?\\)");
                return regex.Replace(content, match => string.Concat("url(", RebaseUrlToAbsolute(baseUrl, match.Groups["url"].Value), ")"));
            }

            public string Process(string includedVirtualPath, string input)
            {
                if (includedVirtualPath == null)
                {
                    throw new ArgumentNullException("includedVirtualPath");
                }
                var directory = VirtualPathUtility.GetDirectory(includedVirtualPath);
                return ConvertUrlsToAbsolute(directory, input);
            }

            private static string RebaseUrlToAbsolute(string baseUrl, string url)
            {
                if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(baseUrl) || url.StartsWith("/", StringComparison.OrdinalIgnoreCase) ||
                    url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || url.StartsWith("data:image", StringComparison.OrdinalIgnoreCase))
                {
                    return url;
                }
                if (!baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    baseUrl = string.Concat(baseUrl, "/");
                }
                return VirtualPathUtility.ToAbsolute(string.Concat(baseUrl, url));
            }
        }
    }
}
