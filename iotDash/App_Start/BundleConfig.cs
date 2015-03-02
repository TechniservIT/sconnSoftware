using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;
using System.Web.Optimization;

namespace iotDash
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {

            /*  Global */

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                       "~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Scripts/jquery-ui-*"
            ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"
                   ));

            bundles.Add(new ScriptBundle("~/bundles/sb-admin").Include(
              "~/Scripts/sb-admin.js"
           ));


            /* Tools */

            bundles.Add(new ScriptBundle("~/bundles/querybuilder").Include(
            "~/Scripts/query/query-builder.standalone.js"));

            bundles.Add(new ScriptBundle("~/bundles/toggleswitch").Include(
            "~/Scripts/switch/bootstrap-switch.js"));



            /* Style */

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/Site.less", 
                "~/Content/bootstrap/bootstrap.less",
                "~/Content/css/sb-admin-2.css",
                "~/Content/fonts/font-awesome-4.1.0/css/font-awesome.min.css",
                "~/Content/css/plugins/metisMenu/metisMenu.min.css",
                 "~/Content/css/jquery-ui.css"
                       ));

            bundles.Add(new StyleBundle("~/bundles/css/querybuilder").Include(
                     "~/Content/css/query-builder.css"
                 ));

            bundles.Add(new StyleBundle("~/bundles/css/sidebar").Include(
                     "~/Content/css/sidebar.css"
                 ));

            bundles.Add(new StyleBundle("~/bundles/css/toggleswitch").Include(
             "~/Content/switch/bootstrap-switch.css"
      ));

        }
    }
}