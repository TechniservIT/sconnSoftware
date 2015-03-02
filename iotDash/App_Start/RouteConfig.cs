using System.Web.Mvc;
using System.Web.Routing;

namespace iotDash
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{app}/{controller}/{action}/{id}",
                defaults: new { app="public", controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            /*
                routes.MapRoute(
                name: "App",
                url: "{app}/{controller}/{action}/{id}",
                defaults: new { app="public", controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            */

            /*
            RouteTable.Routes.MapPageRoute("AppRoute",
            "App/{Name}",
            "~/{controller}/{action}/{id}");
            */
 
            /*
            routes.MapRoute(
                name: "AppRoute",
                url: "App/{id}",    //{AppDomain}
                defaults: new { id = "sampleapp" }
            );
             
             */
             
        }
    }
}