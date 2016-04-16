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




            /*************************  SCRIPTS *************************/


            /************ Global **********/

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.validate.js",
                       "~/Scripts/jquery.validate.unobtrusive.js"
                       ));

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

            bundles.Add(new ScriptBundle("~/bundles/fancybox").Include(
              "~/Scripts/jquery.fancybox.pack.js"));

            bundles.Add(new ScriptBundle("~/bundles/sb-admin").Include(
              "~/Scripts/sb-admin.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-formhelpers").Include(
              "~/Scripts/bootstrap-formhelpers.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-treeview").Include(
              "~/Scripts/bootstrap-treeview.js"
           ));
            
            bundles.Add(new ScriptBundle("~/bundles/gray").Include(
              "~/Scripts/grayscale.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/easing").Include(
              "~/Scripts/jquery.easing.min.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/linq").Include(
             "~/Scripts/linq.js"
          ));


            /* Tools */

            bundles.Add(new ScriptBundle("~/bundles/querybuilder").Include(
            "~/Scripts/query/query-builder.standalone.js"));

            bundles.Add(new ScriptBundle("~/bundles/toggleswitch").Include(
            "~/Scripts/switch/bootstrap-switch.js"));

            bundles.Add(new ScriptBundle("~/bundles/numspin").Include(
            "~/Scripts/bootstrap-spinedit.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/html5shiv").Include(
            "~/Scripts/html5shiv.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-toggle").Include(
              "~/Scripts/bootstrap-toggle.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/maps").Include(
            "~/Scripts/leaflet.js",
            "~/Scripts/leaflet.awesome-markers.js",
            "~/Scripts/leaflet.extra-markers.js",
            "~/Scripts/leaflet.draw.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/fullpage").Include(
              "~/Scripts/jquery.fullpage.js"
            ));

            /*************************  STYLES *************************/

            /* Style */

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/Site.css", 
                "~/Content/bootstrap.css",
                "~/Content/css/sb-admin-2.css",
                "~/Content/fonts/font-awesome-4.1.0/css/font-awesome.css",
                "~/Content/css/plugins/metisMenu/metisMenu.css",
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


            bundles.Add(new StyleBundle("~/bundles/css/numspin").Include(
            "~/Content/css/spinedit.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/css/bootstrap-treeview").Include(
            "~/Content/css/bootstrap-treeview.css"
                ));
            
            bundles.Add(new StyleBundle("~/bundles/css/grayscale").Include(
                 "~/Content/css/grayscale.css"
             ));

            bundles.Add(new StyleBundle("~/bundles/css/bootstrap-toggle").Include(
                 "~/Content/css/bootstrap-toggle.css"
             ));

            bundles.Add(new StyleBundle("~/bundles/css/sidebar").Include(
                 "~/Content/css/sidebar.css"
             ));
            bundles.Add(new StyleBundle("~/bundles/css/sidebarsimple").Include(
               "~/Content/css/simple-sidebar.css"
           ));
            bundles.Add(new StyleBundle("~/bundles/css/bootstrap-formhelpers").Include(
               "~/Content/css/bootstrap-formhelpers.css"
           ));

            bundles.Add(new StyleBundle("~/bundles/css/maps").Include(
                 "~/Content/css/leaflet.css",
                 "~/Content/css/leaflet.awesome-markers.css",
                  "~/Content/css/leaflet.extra-markers.css",
                    "~/Content/css/leaflet.draw.css"
             ));

            bundles.Add(new StyleBundle("~/bundles/css/fullpage").Include(
               "~/Content/css/jquery.fullpage.css"
           ));

            /* LESS Bundles */
            bundles.Add(new LessBundle("~/bundles/less").Include("~/Content/less/*.less"));



        }
    }
}