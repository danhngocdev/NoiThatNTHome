using DVG.WIS.Business.Banner;
using DVG.WIS.Business.Category;
using DVG.WIS.Business.Category.Cached;
using DVG.WIS.Business.Customers;
using DVG.WIS.Business.InfoContact;
using DVG.WIS.Business.Menu;
using DVG.WIS.Business.News;
using DVG.WIS.Business.Persons;
using DVG.WIS.Business.Products;
using DVG.WIS.Business.ProductShowHome;
using DVG.WIS.Business.Recruitments;
using DVG.WIS.Business.Video;
using DVG.WIS.Caching.Cached;
using DVG.WIS.Caching.Cached.Implements;
using DVG.WIS.Caching.DTO.Entities;
using DVG.WIS.DAL.Banner;
using DVG.WIS.DAL.Category;
using DVG.WIS.DAL.Customers;
using DVG.WIS.DAL.InfoContact;
using DVG.WIS.DAL.News;
using DVG.WIS.DAL.Persons;
using DVG.WIS.DAL.Products;
using DVG.WIS.DAL.ProductShowHome;
using DVG.WIS.DAL.Recruitments;
using DVG.WIS.DAL.Video;
using DVG.WIS.Utilities;
using System;

using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace DVG.Website
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
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            // Cache
            int cacheType = AppSettings.Instance.GetInt32("CacheType", (int)CachedEnum.CachedTypes.Redis);

            CachingConfigModel config = new CachingConfigModel()
            {
                IpServer = AppSettings.Instance.GetString("RedisIP"),
                Port = AppSettings.Instance.GetInt32("RedisPort"),
                DB = AppSettings.Instance.GetInt32("RedisDB"),
                ConnectTimeout = AppSettings.Instance.GetInt32("RedisTimeout", 600),
                RedisSlotNameInMemory = AppSettings.Instance.GetString("RedisSlotName", "RedisTinxe")
            };

            if (cacheType == (int)CachedEnum.CachedTypes.Redis)
            {
                container.RegisterType<ICached, RedisCached>(new SingletonLifetimeManager(), new InjectionConstructor(config));
            }
            else
            {
                container.RegisterType<ICached, IISCached>(new SingletonLifetimeManager());
            }

            container.RegisterType<IRedisCached, RedisCached>(new SingletonLifetimeManager(), new InjectionConstructor(config));


            //uContainer.RegisterType<ICategoryBoCached, CategoryBoCached>();
            container.RegisterType<ICategoryBoCached, CategoryBoCached>(new SingletonLifetimeManager());
            //Recruitment
            container.RegisterType<IRecruitmentBo, RecruitmentBo>(new SingletonLifetimeManager());
            container.RegisterType<IRecruitmentDal, RecruitmentDal>(new SingletonLifetimeManager());

            //Person
            container.RegisterType<IPersonBo, PersonBo>(new SingletonLifetimeManager());
            container.RegisterType<IPersonDal, PersonDal>(new SingletonLifetimeManager());

            //News
            container.RegisterType<INewsBo, NewsBo>(new SingletonLifetimeManager());
            container.RegisterType<INewsDal, NewsDal>(new SingletonLifetimeManager());

            //Product
            container.RegisterType<IProductBo, ProductBo>(new SingletonLifetimeManager());
            container.RegisterType<IProductDal, ProductDal>(new SingletonLifetimeManager());

            //customer
            container.RegisterType<ICustomerBo, CustomerBo>(new SingletonLifetimeManager());
            container.RegisterType<ICustomerDal, CustomerDal>(new SingletonLifetimeManager());

            //category
            container.RegisterType<ICategoryBo, CategoryBo>(new SingletonLifetimeManager());
            container.RegisterType<ICategoryDal, CategoryDal>(new SingletonLifetimeManager());
            container.RegisterType<ICategoryDalFE, CategoryDalFE>(new SingletonLifetimeManager());
            container.RegisterType<ICategoryBoFE, CategoryBoFE>(new SingletonLifetimeManager());

            //Banner
            container.RegisterType<IBannerBo, BannerBo>(new SingletonLifetimeManager());
            container.RegisterType<IBannerDal, BannerDal>(new SingletonLifetimeManager());

            //Video
            container.RegisterType<IVideoBo, VideoBo>(new SingletonLifetimeManager());
            container.RegisterType<IVideoDal, VideoDal>(new SingletonLifetimeManager());
            container.RegisterType<IMenuBo, MenuBo>(new SingletonLifetimeManager());
            //ProductShowHome

            container.RegisterType<IProductShowHomeBo, ProductShowHomeBo>(new SingletonLifetimeManager());
            container.RegisterType<IProductShowHomeDal, ProductShowHomeDal>(new SingletonLifetimeManager());


            //InfoContactDal
            container.RegisterType<IInfoContactDal, InfoContactDal>();
            container.RegisterType<IInfoContactBo, InfoContactBo>();


        }
    }
}