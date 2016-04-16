using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iotDash.Helpers
{

    static public class DomainRoutingHelpers
    {
        static public string GetDomainNameFromContext(HttpContextBase httpContext)
        {
            string url = httpContext.Request.RawUrl;
            var urlcomponents = url.Split('/');
            if(urlcomponents.Count() >= 2)
            {
                string appdomain = urlcomponents[1];    //first component after slash
                return appdomain;
            }
            else
            {
                return "Public";
            }
        }
    }
}