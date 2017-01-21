using System.Web;
using System.Web.Optimization;

namespace TollesburySurgery
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                   "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/VendorCss/css").Include(
                    "~/Content/VendorCss/font-awesome.min.css",
                    "~/Content/VendorCss/animate.min.css",
                    "~/Content/VendorCss/simple-line-icons.min.css",
                    "~/Content/VendorCss/owl.carousel.min.css",
                    "~/Content/VendorCss/owl.theme.default.min.css",
                    "~/Content/VendorCss/magnific-popup.min.css",
                    "~/Content/VendorCss/settings.css",
                    "~/Content/VendorCss/layers.css",
                    "~/Content/VendorCss/navigation.css"));

            bundles.Add(new StyleBundle("~/Theme/css").Include(
                   "~/Content/theme-elements.css",
                   "~/Content/theme.css"));

            bundles.Add(new StyleBundle("~/Skin/css").Include(
                  "~/Content/skin-medical.css"));

            bundles.Add(new StyleBundle("~/Custom/css").Include(
                  "~/Content/custom.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
