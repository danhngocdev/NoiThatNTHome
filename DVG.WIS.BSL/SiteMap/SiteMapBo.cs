using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DVG.WIS.Business.Category;
using DVG.WIS.Business.News;
using DVG.WIS.Business.Products;
using DVG.WIS.Core;
using DVG.WIS.Utilities;
using DVG.WIS.Utilities.XmlSiteMap;

namespace DVG.WIS.Business.SiteMap
{
    public class SiteMapBo : ISiteMapBo
    {
        private readonly ICategoryBo _categoryBo;
        private readonly INewsBo _newsBo;
        private readonly IProductBo _productBo;
        public SiteMapBo(ICategoryBo categoryBo, INewsBo newsBo = null, IProductBo productBo = null)
        {
            _categoryBo = categoryBo;
            _newsBo = newsBo;
            _productBo = productBo;
        }
        public string GenSiteMapArticle()
        {
            try
            {
                UrlSet sitemapModel = new UrlSet();
                AddItemSitemapArticle(sitemapModel);

                string xmlContent = SiteMapHelper.XmlSerializeToString(sitemapModel);
                if (!string.IsNullOrEmpty(xmlContent))
                {
                    xmlContent = Regex.Replace(xmlContent, "_x003A_", ":", RegexOptions.IgnoreCase);
                    xmlContent = Regex.Replace(xmlContent, "NSPImage", "xmlns:image", RegexOptions.IgnoreCase);
                }
                return xmlContent;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.ToString());
            }
            return null;
        }

        public string GenSiteMapCategory()
        {

            try
            {

                UrlSet sitemapModel = new UrlSet();
                AddItemSitemapCategory(sitemapModel);
                //AddItemBrandCategory(sitemapModel);
                //AddItemBrandModelCategory(sitemapModel);
                //sitemapModel.Locations = sitemapModel.Locations.ToList().DistinctBy(x => new { x.Url }).ToArray();
                string xmlContent = SiteMapHelper.XmlSerializeToString(sitemapModel);

                return xmlContent;


            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.ToString());
            }
            return null;
        }

        public string GenSiteMapIndex()
        {
            try
            {
                var sitemapModel = new DVG.WIS.Utilities.XmlSiteMap.Sitemap();

                foreach (var item in ListUrlIndexSiteMap)
                {
                    sitemapModel.Insert(new SiteMapLocation()
                    {
                        Url = string.Format("{0}/{1}", ConstUrl.BaseUrl, item),
                        LastModified = DateTime.Now.ToString(Const.FormatSiteMapDate)
                    });
                }
                string xmlContent = SiteMapHelper.XmlSerializeToString(sitemapModel);
                return xmlContent;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.ToString());
            }
            return string.Empty;
        }

        public string GenSiteMapProduct()
        {
            try
            {
                UrlSet sitemapModel = new UrlSet();
                AddItemSiteMapProduct(sitemapModel);

                string xmlContent = SiteMapHelper.XmlSerializeToString(sitemapModel);
                if (!string.IsNullOrEmpty(xmlContent))
                {
                    xmlContent = Regex.Replace(xmlContent, "_x003A_", ":", RegexOptions.IgnoreCase);
                    xmlContent = Regex.Replace(xmlContent, "NSPImage", "xmlns:image", RegexOptions.IgnoreCase);
                }
                return xmlContent;
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.ToString());
            }
            return null;
        }




        #region private method

        private void AddItemSitemapCategory(UrlSet siteMapCategoryAndMenu)
        {
            try
            {
                string baseUrl = string.Concat(ConstUrl.BaseUrl, "/");
                // Home
                siteMapCategoryAndMenu.Add(new Location()
                {
                    Url = baseUrl,
                    LastModified = DateTime.Now.ToString(Const.FormatSiteMapDate),
                    Priority = 1,
                    ChangeFrequency = Location.eChangeFrequency.always
                });
                var listCate = _categoryBo.GetListAll().Where(x=>x.Status == 1).ToList();
                if (listCate != null && listCate.Any())
                {
                    foreach (var item in listCate)
                    {
                        siteMapCategoryAndMenu.Add(new Location()
                        {
                            Url = string.Concat(baseUrl, item.ShortURL),
                            LastModified = DateTime.Now.ToString(Const.FormatSiteMapDate),
                            Priority = 0.8,
                            ChangeFrequency = Location.eChangeFrequency.always
                        });
                    }
                }
        
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.ToString());
            }
        }



        private void AddItemSitemapArticle(UrlSet siteMapCategoryArticle)
        {
            try
            {
                var listArticle = _newsBo.GetListArticleSiteMap();
                if (listArticle != null && listArticle.Any())
                {
                    foreach (var item in listArticle)
                    {
                        var url = CoreUtils.BuildURL("/{0}/{1}-newsId{2}", ConstUrl.News, StringUtils.UnicodeToUnsignCharAndDash(item.Title), item.Id);
                        siteMapCategoryArticle.Add(new Location()
                        {
                            Url = string.Concat(ConstUrl.BaseUrl, url),
                            LastModified = item.PublishedDate.ToString(Const.FormatSiteMapDate),
                            Priority = 0.6,
                            ChangeFrequency = Location.eChangeFrequency.daily,
                            ImageNode = new ImageNode()
                            {
                                ImageLoc = CoreUtils.BuildCropAvatar(item.Avatar, string.Empty, string.Empty),
                                ImageTitle = SiteMapHelper.CleanInvalidXmlChars(item.Title)
                            }
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.ToString());
            }
        }


        private void AddItemSiteMapProduct(UrlSet sitemapProduct)
        {
            try
            {
      
                // Home
                var listProducts = _productBo.GetListProductSiteMap();
                if (listProducts != null && listProducts.Any())
                {
                    foreach (var item in listProducts)
                    {
                        var url = CoreUtils.BuildURL("{0}/{1}-pid{2}", ConstUrl.Product, StringUtils.UnicodeToUnsignCharAndDash(item.Name), item.Id);

                        sitemapProduct.Add(new Location()
                        {
                            Url = string.Concat(ConstUrl.BaseUrl, url),
                            LastModified = item.CreatedDate.ToString(Const.FormatSiteMapDate),
                            Priority = 0.6,
                            ChangeFrequency = Location.eChangeFrequency.daily,
                            ImageNode = new ImageNode()
                            {
                                ImageLoc = CoreUtils.BuildCropAvatar(item.Avatar, string.Empty, string.Empty),
                                ImageTitle = SiteMapHelper.CleanInvalidXmlChars(item.Name)
                            }
                        });
                    }
                }
                
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex.ToString());
            }
        }

        private List<string> ListUrlIndexSiteMap = new List<string>()
        {

            "sitemap/category.xml",
            "sitemap/article.xml",
            "sitemap/product.xml",
            "sitemap/video.xml"
        };



        #endregion
    }


}
