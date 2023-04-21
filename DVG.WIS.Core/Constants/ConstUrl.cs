using DVG.WIS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core
{
    public class ConstUrl
    {


        public static string NewsDetail = "tin-tuc/{title}-newsId{newsId}";
        public static string News = "tin-tuc";
        public static string Cart = "gio-hang";
        public static string NewsPaging = "tin-tuc/p{pageIndex}";


        public static string NewsList = "tin-tuc";
        public static string NewsListPaging = "tin-tuc/p{pageIndex}";

        public static string Product = "san-pham";
        public static string ProductBody = "giay-da-bong-adidas";
        public static string ProductFace = "giay-da-bong-nike";
        public static string ProductListPaging = "san-pham/{category}/p{pageIndex}";
        public static string ProductList = "{category}";
        public static string ProductListAjax = "{category}/p{pageIndex}";
        public static string ProductPaging = "p{pageIndex}";
        public static string ProductListSearch = "tim-kiem-san-pham";
        public static string ProductSearchPaging = "tim-kiem-san-pham/p{pageIndex}";
        public static string ProductDetail = "san-pham/{title}-pid{productId}";


        public const string NewsDetailUrl = "/{0}/{1}-id{2}";
        public const string ProductDetailUrl = "san-pham";
        public const string AboutUs = "gioi-thieu";
        public const string Procedure = "quy-trinh-thi-cong";
        public static string Contact = "lien-he";
        public static string PriceList = "bao-gia-noi-that";
        public const string SuccessStory = "cau-chuyen-thanh-cong";

        public const string BeautyKnowledge = "kien-thuc-lam-dep";
        public const string CompanyNews = "tin-doanh-nghiep";
        public const string CareerNews = "tin-tuyen-dung";
        public const string CustomerStory = "cau-chuyen-khach-hang";


    }

    public class ConstMeta
    {
        public static string HomeTitle = AppSettings.Instance.GetString("HomeTitle");
        public static string HomeDesc = AppSettings.Instance.GetString("HomeDesc");

        public static string ProductTitle = AppSettings.Instance.GetString("ProductTitle");
        public static string ProductDesc = AppSettings.Instance.GetString("ProductDesc");

        public static string AboutUsTitle = AppSettings.Instance.GetString("AboutUsTitle");
        public static string AboutUsDesc = AppSettings.Instance.GetString("AboutUsDesc");

        public static string ContactTitle = AppSettings.Instance.GetString("ContactTitle");
        public static string ContactDesc = AppSettings.Instance.GetString("ContactDesc");

        public static string CoreTitle = AppSettings.Instance.GetString("CoreTitle");
        public static string CoreDesc = AppSettings.Instance.GetString("CoreDesc");

        public static string SuccessStoryTitle = AppSettings.Instance.GetString("SuccessStoryTitle");
        public static string SuccessStoryDesc = AppSettings.Instance.GetString("SuccessStoryDesc");

        public static string BeautyKnowledgeTitle = AppSettings.Instance.GetString("BeautyKnowledgeTitle");
        public static string BeautyKnowledgeDesc = AppSettings.Instance.GetString("BeautyKnowledgeDesc");

        public static string CompanyNewsTitle = AppSettings.Instance.GetString("CompanyNewsTitle");
        public static string CompanyNewsDesc = AppSettings.Instance.GetString("CompanyNewsDesc");

        public static string CareerNewsTitle = AppSettings.Instance.GetString("CareerNewsTitle");
        public static string CareerNewsDesc = AppSettings.Instance.GetString("CareerNewsDesc");

        public static string CustomerStoryTitle = AppSettings.Instance.GetString("CustomerStoryTitle");
        public static string CustomerStoryDesc = AppSettings.Instance.GetString("CustomerStoryDesc");
    }
}
