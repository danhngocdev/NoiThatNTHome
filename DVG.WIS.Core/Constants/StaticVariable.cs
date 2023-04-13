using DVG.WIS.Utilities;
using DVG.WIS.Utilities.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVG.WIS.Core.Constants
{
    public class StaticVariable
    {
        public static string DomainImage = AppSettings.Instance.GetString("DomainImage");
        public static string ImageRootPath = AppSettings.Instance.GetString("ImageRootPath");
        public static string Domain = AppSettings.Instance.GetString("Domain");
        public static string DomainMobile = AppSettings.Instance.GetString("DomainMobile");

        public static string BaseUrl = AppSettings.Instance.GetString("BaseUrl");
        public static string BaseUrlNoSlash = BaseUrl.TrimEnd('/');
        public static string CmsUrl = AppSettings.Instance.GetString("CmsUrl");
        public static string CmsUrlNoSlash = CmsUrl.TrimEnd('/');
        public static string BaseMobileUrl = AppSettings.Instance.GetString("BaseMobileUrl");
        public static string BaseMobileUrlNoSlash = BaseMobileUrl.TrimEnd('/');

        public static string NoImage = AppSettings.Instance.GetString("NoImage");
        public static string NoImage43 = AppSettings.Instance.GetString("NoImage43");
        public static string NoImage169 = AppSettings.Instance.GetString("NoImage169");

        public static string ResizeSizeContentMobile = AppSettings.Instance.GetString("ResizeSizeContentMobile");

        public static int WeekCacheTime = AppSettings.Instance.GetInt32(Const.KeyWeekCacheTime);
        public static int LongCacheTime = AppSettings.Instance.GetInt32(Const.KeyLongCacheTime);
        public static int MediumCacheTime = AppSettings.Instance.GetInt32(Const.KeyMediumCacheTime);
        public static int ShortCacheTime = AppSettings.Instance.GetInt32(Const.KeyShortCacheTime);

        public static int PageSizeCarAssessment = AppSettings.Instance.GetInt32(Const.PageSizeCarAssessment, 15);

        public static string DefaultLanguage = AppSettings.Instance.GetString(Const.DefaultLanguage, "vi");

        public static bool AllowShowDateTime = AppSettings.Instance.GetBool("AllowShowDateTime", false);
        public static string FormatDateSitemap = "yyyy-MM-dd";

        #region Crop Image
        public static string FacebookAvatar = AvatarConfigs.Value("FacebookAvatar", "/crop/620x324");
        public static string StandardAvatar = AvatarConfigs.Value("StandardAvatar");
        public static string BreakingNewsAvatar = AvatarConfigs.Value("BreakingNewsAvatar");

        public static string HomTimelineBig = AvatarConfigs.Value("HomTimelineBig");
        public static string HomeHighlightMediumAvatar = AvatarConfigs.Value("HomeHighlightMediumAvatar");
        public static string HomeHighlightSmallAvatar = AvatarConfigs.Value("HomeHighlightSmallAvatar");
        public static string HomeMostViewAvatar = AvatarConfigs.Value("HomeMostViewAvatar");
        public static string HomeGalleryAvatar = AvatarConfigs.Value("HomeGalleryAvatar");
        public static string HomReviewAvatar = AvatarConfigs.Value("HomReviewAvatar");
        public static string HomVideoBigAvatar = AvatarConfigs.Value("HomVideoBigAvatar");

        public static string FirstNewsAvatarInList = AvatarConfigs.Value("FirstNewsAvatarInList");
        public static string SecondNewsAvatarInList = AvatarConfigs.Value("SecondNewsAvatarInList");

        public static string NewsHighlight = AvatarConfigs.Value("NewsHighlight");
        public static string ListNewsAvatar = AvatarConfigs.Value("ListNewsAvatar");
        public static string HighLightCategoryAvatar = AvatarConfigs.Value("HighLightCategoryAvatar");
        public static string DetailRelationAvatar = AvatarConfigs.Value("DetailRelationAvatar");
        public static string DetailRecommendAvatar = AvatarConfigs.Value("DetailRecommendAvatar");

        public static string GalleryAvatar = AvatarConfigs.Value("GalleryAvatar");
        public static string GallerySmallAvatar = AvatarConfigs.Value("GallerySmallAvatar");
        public static string SlideBig = AvatarConfigs.Value("SlideBig");
        public static string SlideBigResize = AvatarConfigs.Value("SlideBigResize");
        public static string SlideAvatarRelation = AvatarConfigs.Value("SlideAvatarRelation");

        public static string CropSizeCMS = AvatarConfigs.Value("CropSizeCMS");
        public static string SlideNewsSubImage = AvatarConfigs.Value("SlideNewsSubImage");

        public static string CarInfoAvatar = AvatarConfigs.Value("CarInfoAvatar");
        public static string CarInfoDetailAvatar = AvatarConfigs.Value("CarInfoDetailAvatar");

        public static string MobileHighlightAvatar = AvatarConfigs.Value("MobileHighlightAvatar");
        public static string MobileBigAvatar = AvatarConfigs.Value("MobileBigAvatar");
        public static string MobileMediumAvatar = AvatarConfigs.Value("MobileMediumAvatar");
        public static string MobileSmallAvatar = AvatarConfigs.Value("MobileSmallAvatar");
        public static string MobileSmallNewsAvatar = AvatarConfigs.Value("MobileSmallNewsAvatar");
        public static string MobileResizeSlideBig = AvatarConfigs.Value("MobileResizeSlideBig");
        public static string MobileListAvatar = AvatarConfigs.Value("MobileListAvatar");
        public static string MobileOneCateAvatar = AvatarConfigs.Value("MobileOneCateAvatar");
        public static string MobilePlayListAvatar = AvatarConfigs.Value("MobilePlayListAvatar");


        #endregion

        #region Meta tag

        public static string PrefixTitle = "";
        public static string KeyNewList = MetaConfigs.Value("MetaKeywordNews");

        public static string DomainTitle = MetaConfigs.Value("DomainTitle");

        public static string MetaMainTitle = MetaConfigs.Value("MainTitle");
        public static string MetaMainDescription = MetaConfigs.Value("MainDescription");
        public static string MetaMainKeyword = MetaConfigs.Value("MainKeyword");

        public static string AboutTitle = MetaConfigs.Value("AboutTitle");
        public static string AboutDescription = MetaConfigs.Value("AboutDescription");

        public static string IeltsTitle = MetaConfigs.Value("IeltsTitle");
        public static string IeltsDescription = MetaConfigs.Value("IeltsDescription");
        public static string IeltsMetaKeyword = MetaConfigs.Value("IeltsMetaKeyword");

        public static string CitizensTitle = MetaConfigs.Value("CitizensTitle");
        public static string CitizensDescription = MetaConfigs.Value("CitizensDescription");
        public static string CitizensMetaKeyword = MetaConfigs.Value("CitizensMetaKeyword");

        public static string AcademicEnglishTitle = MetaConfigs.Value("AcademicEnglishTitle");
        public static string AcademicEnglishDescription = MetaConfigs.Value("AcademicEnglishDescription");
        public static string AcademicEnglishMetaKeyword = MetaConfigs.Value("AcademicEnglishMetaKeyword");

        public static string EurekaTitle = MetaConfigs.Value("EurekaTitle");
        public static string EurekaDescription = MetaConfigs.Value("EurekaDescription");
        public static string EurekaMetaKeyword = MetaConfigs.Value("EurekaMetaKeyword");

        public static string BlogTitle = MetaConfigs.Value("BlogTitle");
        public static string BlogDescription = MetaConfigs.Value("BlogDescription");
        public static string BlogMetaKeyword = MetaConfigs.Value("BlogMetaKeyword");

        public static string EventTitle = MetaConfigs.Value("EventTitle");
        public static string EventDescription = MetaConfigs.Value("EventDescription");
        public static string EventMetaKeyword = MetaConfigs.Value("EventMetaKeyword");

        public static string RecruitmentTitle = MetaConfigs.Value("RecruitmentTitle");
        public static string RecruitmentDescription = MetaConfigs.Value("RecruitmentDescription");
        public static string RecruitmentMetaKeyword = MetaConfigs.Value("RecruitmentMetaKeyword");

        #endregion

        #region Email

        public static string EmailSupport = AppSettings.Instance.GetString("EmailSupport");
        public static string EmailMaster = AppSettings.Instance.GetString("EmailMaster", "noreply.topbank.vn@gmail.com");
        public static string EmailNoReply = AppSettings.Instance.GetString("EmailNoReply", "noreply.topbank.vn@gmail.com");
        public static string EmailContact = AppSettings.Instance.GetString("Mail-Contact");
        public static string PassEmailNoReply = AppSettings.Instance.GetString("PassEmailNoReply", "fintech2016");

        #endregion

        public static Dictionary<int, int> DicMaxRelations = new Dictionary<int, int>()
        {
            {(int)DVG.WIS.Core.Enums.NewsTypeEnum.News, Const.MaxNewsRelation},
        };

        public static string PrefixCarInfoEmpty = AppSettings.Instance.GetString("PrefixCarInfoSpecEmpty", "x");

        #region Banner
        public static Dictionary<int, string> DicBannerPageUrl = new Dictionary<int, string>()
        {
            //{(int)DVG.WIS.Core.Enums.BannerPageEnum.AllPage, ""},
            //{(int)DVG.WIS.Core.Enums.BannerPageEnum.HomePage, "/"},
            //{(int)DVG.WIS.Core.Enums.BannerPageEnum.NewsPage, "/tin-tuc"},
        };
        #endregion
    }
}
