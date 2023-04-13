using DVG.WIS.Core;
using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DVG.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Product


            routes.MapRoute(
                name: "ProductDetail",
                url: ConstUrl.ProductDetail,
                defaults: new { controller = "Product", action = "Detail" },
                constraints: new { productId = @"\d+" }
            );

            routes.MapRoute(
            name: "ProductSearchPaging",
            url: ConstUrl.ProductSearchPaging,
            defaults: new { controller = "Product", action = "Search" },
            constraints: new { pageIndex = @"\d+" }
        );
            routes.MapRoute(
               name: "ProductSearch",
               url: ConstUrl.ProductListSearch,
               defaults: new { controller = "Product", action = "Search" }
           );

            routes.MapRoute(
              name: "ProductListAjax",
              url: ConstUrl.ProductListAjax,
              defaults: new { controller = "Product", action = "List" },
              constraints: new { pageIndex = @"\d+" }
          );
            

            routes.MapRoute(
              name: "ProductListPaging",
              url: ConstUrl.ProductListPaging,
              defaults: new { controller = "Product", action = "List" },
              constraints: new { pageIndex = @"\d+" }
          );
            routes.MapRoute(
               name: "ProductList",
               url: ConstUrl.ProductList,
               defaults: new { controller = "Product", action = "List" }
           );

            routes.MapRoute(
              name: "Product",
              url: ConstUrl.Product,
              defaults: new { controller = "Product", action = "Index" }
          );

            #endregion


            #region News

            routes.MapRoute(
              name: "News",
              url: ConstUrl.News,
              defaults: new { controller = "News", action = "Index" }
          );

            routes.MapRoute(
                name: "NewsDetail",
                url: ConstUrl.NewsDetail,
                defaults: new { controller = "News", action = "Detail" },
                constraints: new { newsId = @"\d+" }
            );



            routes.MapRoute(
              name: "NewsListPaging",
              url: ConstUrl.NewsListPaging,
              defaults: new { controller = "News", action = "List" },
              constraints: new
              {
                  pageIndex = @"\d+",
              }
          );
            routes.MapRoute(
               name: "NewsList",
               url: ConstUrl.NewsList,
               defaults: new { controller = "News", action = "List" }
               
           );

            routes.MapRoute(
              name: "NewsPaing",
              url: ConstUrl.NewsPaging,
              defaults: new { controller = "News", action = "Index" },
              constraints: new { pageIndex = @"\d+" }
          );
           
            #endregion


            #region SinglePage

            routes.MapRoute(
                name: "DynamicStaticPage",
                url: "{category}",
                defaults: new { controller = "Home", action = "DynamicStaticPage" },
                constraints: new { category = $"{ConstUrl.AboutUs}|{ConstUrl.SuccessStory}" }
            );
            routes.MapRoute(
               name: "Procedure",
               url: ConstUrl.Procedure,
               defaults: new { controller = "Home", action = "Procedure" }
           );
            routes.MapRoute(
               name: "PriceList",
               url: ConstUrl.PriceList,
               defaults: new { controller = "PriceList", action = "Index" }
           );

            routes.MapRoute(
                name: "AboutUs",
                url: ConstUrl.AboutUs,
                defaults: new { controller = "Home", action = "AboutUs" }
            );

            routes.MapRoute(
                name: "Contact",
                url: ConstUrl.Contact,
                defaults: new { controller = "Home", action = "Contact" }
            );
             routes.MapRoute(
                name: "Cart",
                url: ConstUrl.Cart,
                defaults: new { controller = "Cart", action = "Checkout" }
            );

            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
