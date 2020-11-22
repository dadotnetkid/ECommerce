using System.Web.Optimization;

namespace Helpers
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
               "~/content/scripts/modernizr-*",
               "~/content/scripts/jquery-{version}.js",
               "~/content/scripts/jquery.validate*",
               "~/content/scripts/jquery.validate.unobtrusive.js",
               "~/content/scripts/bootstrap.js",
               "~/content/admin-lte/js/adminlte.js",
               "~/content/scripts/custom.js"
           ));



            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/admin-lte/css/AdminLTE.css",
                "~/Content/admin-lte/css/skins/_all-skins.css",
                "~/Content/font-awesome.css",
                "~/Content/site.css"));
        }
    }
}
