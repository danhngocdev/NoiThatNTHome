﻿using DVG.WIS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DVG.Website.Controllers
{
    public class PagingController: Controller
    {
        public ActionResult Index(Pagings pagingInfo)
        {
            int pageNum = (int)Math.Ceiling((double)pagingInfo.Count / pagingInfo.PageSize);
            if (pageNum * pagingInfo.PageSize < pagingInfo.Count)
            {
                pageNum++;
            }
            pagingInfo.LinkSite = pagingInfo.LinkSite.TrimEnd('/') + "/";
            const string buildLink = "<li><a href='{0}{1}' {2}>{3}</a> </li>";
            const string active = " class=\"active\" ";
            const string first_page = "class='first-page'";
            const string prev_page = "class='prev-page'";
            const string next_page = "class='next-page'";
            const string last_page = "class='last-page'";
            const string strTrang = "p";
            string currentPage = pagingInfo.PageIndex.ToString();
            StringBuilder htmlPage = new StringBuilder();
            int iCurrentPage = 0;
            if (pagingInfo.PageIndex <= 0) iCurrentPage = 1;
            else iCurrentPage = pagingInfo.PageIndex;
            if (pageNum <= 3)
            {
                if (pageNum != 1)
                {
                    for (int i = 1; i <= pageNum; i++)
                    {
                        if (i == 1)
                        {
                            htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite.TrimEnd('/'), string.Empty, i == pagingInfo.PageIndex ? active : string.Empty, i);
                        }
                        else
                        {
                            htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite, strTrang + i, i == pagingInfo.PageIndex ? active : string.Empty, i);
                        }
                    }
                }
            }
            else
            {
                if (iCurrentPage > 1)
                {
                    htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite.TrimEnd('/'), string.Empty, first_page, string.Empty);
                }
                else
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        if (i == 1)
                        {
                            htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite.TrimEnd('/'), "", i == pagingInfo.PageIndex ? active : string.Empty, i);
                        }
                        else
                        {
                            htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite, strTrang + i, i == pagingInfo.PageIndex ? active : string.Empty, i);
                        }
                    }
                    htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite, strTrang + 2, next_page, string.Empty);
                }
                if (iCurrentPage > 1 && iCurrentPage < pageNum)
                {
                    if (iCurrentPage > 1)
                    {
                        if (iCurrentPage - 1 == 1)
                        {
                            htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite.TrimEnd('/'), string.Empty, prev_page, string.Empty);
                        }
                        else
                        {
                            htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite, strTrang + (iCurrentPage - 1), prev_page, string.Empty);
                        }
                    }
                    for (int i = (iCurrentPage - 1); i <= (iCurrentPage + 1 < pageNum ? iCurrentPage + 1 : pageNum); i++)
                    {
                        if (i > 0)
                        {
                            if (i == 1)
                            {
                                htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite.TrimEnd('/'), "", i == pagingInfo.PageIndex ? active : string.Empty, i);
                            }
                            else
                            {
                                htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite, strTrang + i, i == pagingInfo.PageIndex ? active : string.Empty, i);
                            }
                        }
                    }
                    if (iCurrentPage <= pageNum - 1)
                    {
                        htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite, strTrang + (iCurrentPage + 1), next_page, string.Empty);
                    }
                }
                int intCurrentPage = 0;
                int.TryParse(currentPage, out intCurrentPage);
                if (intCurrentPage == 0) intCurrentPage = 1;
                if (intCurrentPage < pageNum)
                {
                    htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite, strTrang + pageNum, last_page, string.Empty);
                }
                else
                {
                    if (pageNum - 2 == 1)
                    {
                        htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite.TrimEnd('/'), string.Empty, prev_page, string.Empty);
                    }
                    else
                    {
                    }
                    int j = 2;
                    for (int i = pageNum; i >= pageNum - 2; i--)
                    {
                        htmlPage.AppendFormat(buildLink, pagingInfo.LinkSite, strTrang + (pageNum - j), j == 0 ? active : string.Empty, pageNum - j);
                        j--;
                    }
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<ul class='pagination'>{0}</ul>", htmlPage);
            return PartialView("~/Views/Shared/_Paging.cshtml", sb.ToString());
        }

    }
}