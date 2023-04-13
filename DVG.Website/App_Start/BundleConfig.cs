using DVG.WIS.Utilities;
using System.Web;
using System.Web.Optimization;

namespace DVG.Website
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleHelper.RegisterBundles(bundles);
            BundleTable.EnableOptimizations = AppSettings.Instance.GetBool("StaticsZipOptimize");
        }
    }
}
