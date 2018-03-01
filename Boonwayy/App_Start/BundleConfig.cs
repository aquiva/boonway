using System.Web;
using System.Web.Optimization;

namespace Boonwayy
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/admin").Include(
                "~/Content/admin/css/bootstrap.min.css",
                "~/Content/admin/css/animate.min.css",
                "~/Content/admin/css/light-bootstrap-dashboard.css",
                "~/Content/admin/css/pe-icon-7-stroke.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/adminjs").Include(
                      "~/Scripts/admin/jquery.3.2.1.min.js",
                      "~/Scripts/admin/bootstrap.min.js",
                      "~/Scripts/admin/chartist.min.js",
                      "~/Scripts/admin/bootstrap-notify.js",
                      "~/Scripts/admin/light-bootstrap-dashboard.js"
                      ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryui")
   .Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/jqueryui")
   .Include("~/Content/themes/base/all.css"));
        }
    }
}
