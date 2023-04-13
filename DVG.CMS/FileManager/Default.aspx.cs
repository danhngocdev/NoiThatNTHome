using FileManager.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FileManager
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
            }

            string userName = HttpContext.Current.User.Identity.Name;
            FileStorage.AESIV = Config.AESIV;
            FileStorage.AESKey = Config.AESKey;

            this.hfToken.Value = FileStorage.AESEncrypt(userName + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

			var imageurl = Request["imgurl"];
			if(!string.IsNullOrEmpty(imageurl))
			{
				this.explore.Visible = false;
				this.previewimage.Visible = true;
				this.imgpreview.Src = imageurl;
			}
			else
			{
				this.explore.Visible = true;
				this.previewimage.Visible = false;
			}
		}
	}
}