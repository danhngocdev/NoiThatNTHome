using DVG.WIS.Entities;
using DVG.WIS.Services.News;
using DVG.WIS.Utilities;
using DVG.WIS.Core.Enums;
using DVG.WIS.Utilities.FileStorage;
using System;
using System.Collections.Generic;
using DVG.WIS.Core;
using DVG.WIS.Core.Constants;
using System.Text.RegularExpressions;

namespace DVG.WIS.Services.News
{
    public class NewsService : INewsService
    {
        //private INewsBo _newsBo;
        //private INewsExternalBo _newsExternalBo;
        //private INewsOutsideBo _newsOutsideBo; 

        //public NewsService(INewsBo newsBo, INewsExternalBo newsExternalBo, INewsOutsideBo newsOutsideBo)
        //{
        //    this._newsBo = newsBo;
        //    this._newsExternalBo = newsExternalBo;
        //    this._newsOutsideBo = newsOutsideBo;
        //}

        //public ErrorCodes ReciverNewsFromExternal(int newsId, out long id)
        //{
        //    id = 0;

        //    if (!AuthenService.IsLogin())
        //    {
        //        return ErrorCodes.NotLogin;
        //    }

        //    WIS.Entities.PushNewsInfo newsExternal = _newsExternalBo.GetById(newsId);

        //    if (newsExternal == null || newsExternal.ID <= 0)
        //    {
        //        return ErrorCodes.NewsNotFound;
        //    }

        //    UserLogin userInfo = AuthenService.GetUserLogin();

        //    WIS.Entities.News news = new WIS.Entities.News();

        //    news.Id = Utils.CreateNewsId();
        //    id = news.Id;

        //    string avatar = string.Empty;
        //    //news.Content = Utils.UploadImageIncontent(newsExternal.LongContent, out avatar, FileStorage.UploadToServer);
        //    if (!string.IsNullOrEmpty(avatar))
        //    {
        //        avatar = avatar.Replace(StaticVariable.DomainImage, "");
        //    }
        //    news.Avatar = avatar;
        //    news.CateId = newsExternal.CategoryID;
        //    news.OriginalId = newsExternal.ID;
        //    news.Title = newsExternal.Title;
        //    news.Sapo = newsExternal.Sapo;
        //    //news.Source = newsExternal.DomainName;
        //    news.Status = (int)NewsStatusEnum.Temp;
        //    news.LastReceiver = userInfo.UserName;
        //    news.CreatedDate = DateTime.Now;
        //    news.LastModifiedDate = DateTime.Now;
        //    news.CreatedBy = string.IsNullOrEmpty(userInfo.UserName) ? userInfo.FullName : userInfo.UserName;
        //    news.DistributionDate = DateTime.Now;
        //    news.PublishedBy = userInfo.UserName;
        //    //news.WordCount = StringUtils.CountWords(newsExternal.LongContent);
        //    news.NewsType = (int)NewsDisplayPositionEnum.Normal;
        //    ErrorCodes errorCode = _newsBo.Update(news, null, null, null, null, null, null, null, null, null, null, null, null);
        //    if (errorCode == ErrorCodes.Success)
        //    {
        //        bool result = _newsExternalBo.MarkAsApproved(newsId, (int) NewsStatusExternalEnum.Recived, news.LastReceiver);
        //        if (result) return ErrorCodes.Success;
        //    }

        //    return ErrorCodes.UnknowError;
        //}

        //public ErrorCodes ReciverNewsFromOutside(int newsId, out long id)
        //{
        //    id = 0;

        //    if (!AuthenService.IsLogin())
        //    {
        //        return ErrorCodes.NotLogin;
        //    }

        //    Entities.NewsExternal newsExternal = _newsOutsideBo.GetById(newsId);

        //    if (newsExternal == null || newsExternal.NewsId <= 0)
        //    {
        //        return ErrorCodes.NewsNotFound;
        //    }

        //    UserLogin userInfo = AuthenService.GetUserLogin();

        //    Entities.News news = new Entities.News {Id = Utils.CreateNewsId()};

        //    id = news.Id;

        //    string avatar = string.Empty;
        //    //news.Content = Utils.UploadImageIncontent(newsExternal.Content, out avatar, FileStorage.UploadToServer);

        //    if (!string.IsNullOrEmpty(avatar))
        //    {
        //        avatar = avatar.Replace(StaticVariable.DomainImage, "");
        //    }
        //    news.Avatar = avatar;
        //    news.CateId = newsExternal.CateId;
        //    news.OriginalId = newsExternal.NewsId;
        //    news.Title = newsExternal.Title;
        //    news.Sapo = newsExternal.Summary;
        //    news.Source = newsExternal.DomainName;
        //    news.Status = (int)NewsStatusEnum.Temp;
        //    news.LastReceiver = userInfo.UserName;
        //    news.CreatedDate = DateTime.Now;
        //    news.CreatedDateSpan = Utils.DateTimeToUnixTime(news.CreatedDate.Value);
        //    news.LastModifiedDate = DateTime.Now;
        //    news.CreatedBy = string.IsNullOrEmpty(userInfo.UserName) ? userInfo.FullName : userInfo.UserName;
        //    news.DistributionDate = DateTime.Now;
        //    news.PublishedBy = userInfo.UserName;
        //    //news.WordCount = StringUtils.CountWords(newsExternal.Content);
        //    news.NewsType = (int)NewsTypeEnum.News;
        //    news.DisplayPosition = (int)NewsDisplayPositionEnum.Normal;
        //    ErrorCodes errorCode = _newsBo.Update(news, null, null, null, null, null, null, null, null, null, null, null, null);
        //    if (errorCode == ErrorCodes.Success)
        //    {
        //        bool result = _newsOutsideBo.MarkAsApproved(newsId, (int)NewsStatusExternalEnum.Recived, news.LastReceiver);
        //        if (result) return ErrorCodes.Success;
        //    }

        //    return ErrorCodes.UnknowError;
        //}

        //public ErrorCodes ReciverNewsFromOutsideNoAuth(int newsId, out long id)
        //{
        //    id = 0;

        //    Entities.NewsExternal newsExternal = _newsOutsideBo.GetById(newsId);

        //    if (newsExternal == null || newsExternal.NewsId <= 0)
        //    {
        //        return ErrorCodes.NewsNotFound;
        //    }
        //    string username = "admin_crawler";

        //    Entities.News news = new Entities.News { Id = Utils.CreateNewsId() };

        //    Entities.NewsDetail newsDetail = new Entities.NewsDetail() { NewsId = news.Id };

        //    id = news.Id;

        //    string avatar;
        //    //news.Content = Utils.UploadImageIncontent(newsExternal.Content, out avatar, FileStorage.UploadToServer);
        //    string sourceDomain = newsExternal.DomainLink;

        //    if (!string.IsNullOrEmpty(sourceDomain))
        //    {
        //        if (!sourceDomain.Contains("http"))
        //        {
        //            sourceDomain = string.Format("http://{0}", sourceDomain);
        //        }
        //        else
        //        {
        //            Uri uri = new Uri(sourceDomain);
        //            sourceDomain = uri.GetLeftPart(UriPartial.Authority);
        //        }
        //    }

        //    string originAvatar = newsExternal.Avatar;

        //    newsDetail.Content = StringUtils.UploadImageIncontent(newsExternal.Content, out avatar, StaticVariable.ImageRootPath, sourceDomain, StaticVariable.DomainImage, FileStorage.DownloadImageFromURL);

        //    newsDetail.Content = Regex.Replace(newsDetail.Content, @"(<p([^>]+)?>([\s\r\n]+)?<(strong|b)>([\s\r\n]+)?Xem thêm(\s+)?:([\s\r\n]+)?</(strong|b)>([\s\r\n]+)?</p>.+)", string.Empty, RegexOptions.Singleline);

        //    //if(!string.IsNullOrEmpty(originAvatar))
        //    //{
        //    //    originAvatar = FileStorage.DownloadImageFromURL(originAvatar, StaticVariable.ImageRootPath, sourceDomain, StaticVariable.DomainImage);
        //    //}

        //    //if (!string.IsNullOrEmpty(originAvatar)) avatar = originAvatar;

        //    if (string.IsNullOrEmpty(avatar))
        //    {
        //        if (!string.IsNullOrEmpty(newsExternal.Avatar))
        //        {
        //            avatar = FileStorage.DownloadImageFromURL(newsExternal.Avatar, StaticVariable.ImageRootPath, sourceDomain, StaticVariable.DomainImage);
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(avatar))
        //    {
        //        avatar = avatar.Replace(StaticVariable.DomainImage, "");
        //    }

        //    DateTime now = DateTime.Now;
        //    news.Avatar = avatar;
        //    news.CateId = newsExternal.CateId;
        //    news.OriginalId = newsExternal.NewsId;
        //    news.Title = newsExternal.Title;
        //    news.Sapo = newsExternal.Summary;
        //    news.Source = newsExternal.DomainName;
        //    news.Status = (int)NewsStatusEnum.Published;
        //    news.LastReceiver = username;
        //    news.CreatedDate = newsExternal.CreateDate;
        //    news.CreatedDateSpan = Utils.DateTimeToUnixTime(newsExternal.CreateDate);
        //    news.LastModifiedDate = now;
        //    news.LastModifiedDateSpan = Utils.DateTimeToUnixTime(now);
        //    news.CreatedBy = username;
        //    news.DistributionDate = now;
        //    news.PublishedBy = username;
        //    news.WordCount = StringUtils.CountWords(newsExternal.Content);
        //    news.NewsType = (int)NewsTypeEnum.News;
        //    news.DisplayPosition = (int)NewsDisplayPositionEnum.Normal;
        //    //news.Source = newsExternal.DomainName;
        //    List<Tags> listTags = new List<Tags>();
        //    if (!string.IsNullOrEmpty(newsExternal.Keywords))
        //    {
        //        var arrKeyword = newsExternal.Keywords.Split('|');
        //        if (arrKeyword != null)
        //        {
        //            foreach (var item in arrKeyword)
        //            {
        //                listTags.Add(new Tags()
        //                {
        //                    Name = item,
        //                    UnsignName = StringUtils.UnicodeToUnsignCharAndDash(item),
        //                    Url = StringUtils.UnicodeToUnsignCharAndDash(item),
        //                    CreatedBy = username,
        //                    CreatedDate = now,
        //                    CreatedDateSpan = Utils.DateTimeToUnixTime(now),
        //                    LastModifiedBy = username,
        //                    LastModifedDate = now,
        //                    LastModifiedDateSpan = Utils.DateTimeToUnixTime(now),
        //                    Priority = 1
        //                });
        //            }
        //        }
        //        newsDetail.TagItem = newsExternal.Keywords.Replace("|", ";");
        //    }
        //    ErrorCodes errorCode = _newsBo.UpdateNoAuthen(news, newsDetail, listTags, null, null, null, null, null, null, null, null);
        //    if (errorCode == ErrorCodes.Success)
        //    {
        //        bool result = _newsOutsideBo.MarkAsApproved(newsId, (int)NewsStatusExternalEnum.Recived, username);
        //        if (result) return ErrorCodes.Success;
        //    }

        //    return ErrorCodes.UnknowError;
        //}
    }
}
