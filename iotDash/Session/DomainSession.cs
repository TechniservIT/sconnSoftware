using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iotDash.Session
{
    static public class DomainSession
    {
        static public string GetContextDomain(HttpContextBase httpContext)
        {
            string url = httpContext.Request.RawUrl;
            var urlcomponents = url.Split('/');
            string appdomain = urlcomponents[1];
            return appdomain;
        }

    }
}