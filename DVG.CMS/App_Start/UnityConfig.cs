using DVG.WIS.Business.Activities;
using DVG.WIS.Business.Users;
using DVG.WIS.Business.AuthAction;
using DVG.WIS.Business.AuthGroup;
using DVG.WIS.Business.AuthGroupActionMapping;
using DVG.WIS.Business.AuthGroupCategoryMapping;
using DVG.WIS.Business.AuthGroupNewsStatusMapping;
using DVG.WIS.Business.AuthGroupUserMapping;
using DVG.WIS.Business.ConfigSystem;
using DVG.WIS.Caching.Cached;
using DVG.WIS.Caching.Cached.Implements;
using DVG.WIS.Caching.DTO.Entities;
using DVG.WIS.DAL.Activities;
using DVG.WIS.DAL.AuthAction;
using DVG.WIS.DAL.AuthGroup;
using DVG.WIS.DAL.AuthGroupActionMapping;
using DVG.WIS.DAL.AuthGroupCategoryMapping;
using DVG.WIS.DAL.AuthGroupNewsStatusMapping;
using DVG.WIS.DAL.AuthGroupUserMapping;
using DVG.WIS.DAL.Users;
using DVG.WIS.Utilities;
using System;
using Unity;
using Unity.Injection;
using DVG.WIS.Business.Category;
using DVG.WIS.DAL.Category;
using DVG.WIS.DAL.Banner;
using DVG.WIS.Business.Banner;
using DVG.WIS.Business.Recruitments;
using DVG.WIS.Business.Customers;
using DVG.WIS.DAL.Recruitments;
using DVG.WIS.DAL.Customers;
using DVG.WIS.DAL.News;
using DVG.WIS.Business.News;
using DVG.WIS.Business.Category.Cached;
using DVG.WIS.Business.Persons;
using DVG.WIS.DAL.Persons;
using DVG.WIS.DAL.Galleries;
using DVG.WIS.Business.Galleries;
using DVG.WIS.DAL.Products;
using DVG.WIS.Business.Products;
using DVG.WIS.Business.Subscribe;
using DVG.WIS.DAL.Subscribe;
using DVG.WIS.Business.Orders;
using DVG.WIS.DAL.Orders;
using DVG.WIS.DAL.Slider;
using DVG.WIS.DAL.PriceList;
using DVG.WIS.Business.PriceList;
using DVG.WIS.Business.Video;
using DVG.WIS.DAL.Video;
using DVG.WIS.DAL.ProductShowHome;
using DVG.WIS.Business.ProductShowHome;
using DVG.WIS.Business.Menu;
using DVG.WIS.DAL.InfoContact;
using DVG.WIS.Business.InfoContact;

namespace DVG.CMS
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer uContainer)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();

            // Cache
            //int cacheType = AppSettings.Instance.GetInt32("CacheType", (int)CachedEnum.CachedTypes.Redis);

            //if (cacheType == (int)CachedEnum.CachedTypes.Redis)
            //{
            //    CachingConfigModel config = new CachingConfigModel()
            //    {
            //        IpServer = AppSettings.Instance.GetString("RedisIP"),
            //        Port = AppSettings.Instance.GetInt32("RedisPort"),
            //        DB = AppSettings.Instance.GetInt32("RedisDB"),
            //        ConnectTimeout = AppSettings.Instance.GetInt32("RedisTimeout", 600),
            //        RedisSlotNameInMemory = AppSettings.Instance.GetString("RedisSlotName", "RedisTinxe")
            //    };
            //    uContainer.RegisterType<ICached, RedisCached>(new InjectionConstructor(config));
            //}
            //else
            //{
            uContainer.RegisterType<ICached, IISCached>();
            //}
            //User
            uContainer.RegisterType<IUserDAL, UserDAL>();
            uContainer.RegisterType<IUserBo, UserBo>();
            uContainer.RegisterType<IConfigSystemBo, ConfigSystemBo>();

            //Banner
            uContainer.RegisterType<IBannerDal, BannerDal>();
            uContainer.RegisterType<IBannerBo, BannerBo>();

            //category
            uContainer.RegisterType<ICategoryBoCached, CategoryBoCached>();
            uContainer.RegisterType<ICategoryBoFE, CategoryBoFE>();
            uContainer.RegisterType<ICategoryDalFE, CategoryDalFE>();
            uContainer.RegisterType<ICategoryBo, CategoryBo>();
            uContainer.RegisterType<ICategoryDal, CategoryDal>();

            //Recruitment
            uContainer.RegisterType<IRecruitmentBo, RecruitmentBo>();
            uContainer.RegisterType<IRecruitmentDal, RecruitmentDal>();

            //Person
            uContainer.RegisterType<IPersonBo, PersonBo>();
            uContainer.RegisterType<IPersonDal, PersonDal>();

            //Gallery
            uContainer.RegisterType<IGalleryBo, GalleryBo>();
            uContainer.RegisterType<IGalleryDal, GalleryDal>();

            //Customer
            uContainer.RegisterType<ICustomerBo, CustomerBo>();
            uContainer.RegisterType<ICustomerDal, CustomerDal>();

            //News
            uContainer.RegisterType<INewsBo, NewsBo>();
            uContainer.RegisterType<INewsDal, NewsDal>();

            //Product
            uContainer.RegisterType<IProductBo, ProductBo>();
            uContainer.RegisterType<IProductDal, ProductDal>();

            //Activity
            uContainer.RegisterType<IActivityDal, ActivityDal>();
            uContainer.RegisterType<IActivityBo, ActivityBo>();

            //Activity
            uContainer.RegisterType<ISubscribeBo, SubscribeBo>();
            uContainer.RegisterType<ISubscribeDal, SubscribeDal>();
            //Order
            uContainer.RegisterType<IOrderBo, OrderBo>();
            uContainer.RegisterType<IOrderDal, OrderDal>();

            //Auth
            uContainer.RegisterType<IAuthGroupDal, AuthGroupDal>();
            uContainer.RegisterType<IAuthGroupBo, AuthGroupBo>();

            //Menu
            uContainer.RegisterType<IMenuBo,MenuBo>();

            //Price List
            uContainer.RegisterType<IPriceListDal, PriceListDal>();
            uContainer.RegisterType<IPriceListBo, PriceListBo>();

            //Video
            uContainer.RegisterType<IVideoDal, VideoDal>();
            uContainer.RegisterType<IVideoBo, VideoBo>();



            //ProductSHowhome
            uContainer.RegisterType<IProductShowHomeDal, ProductShowHomeDal>();
            uContainer.RegisterType<IProductShowHomeBo, ProductShowHomeBo>();

            //InfoContactDal
            uContainer.RegisterType<IInfoContactDal, InfoContactDal>();
            uContainer.RegisterType<IInfoContactBo, InfoContactBo>();


            uContainer.RegisterType<IAuthActionDal, AuthActionDal>();
            uContainer.RegisterType<IAuthActionBo, AuthActionBo>();

            uContainer.RegisterType<IAuthGroupActionMappingDal, AuthGroupActionMappingDal>();
            uContainer.RegisterType<IAuthGroupActionMappingBo, AuthGroupActionMappingBo>();

            uContainer.RegisterType<IAuthGroupCategoryMappingDal, AuthGroupCategoryMappingDal>();
            uContainer.RegisterType<IAuthGroupCategoryMappingBo, AuthGroupCategoryMappingBo>();

            uContainer.RegisterType<IAuthGroupUserMappingDal, AuthGroupUserMappingDal>();
            uContainer.RegisterType<IAuthGroupUserMappingBo, AuthGroupUserMappingBo>();

            uContainer.RegisterType<IAuthGroupNewsStatusMappingDal, AuthGroupNewsStatusMappingDal>();
            uContainer.RegisterType<IAuthGroupNewsStatusMappingBo, AuthGroupNewsStatusMappingBo>();


           

        }
    }
}