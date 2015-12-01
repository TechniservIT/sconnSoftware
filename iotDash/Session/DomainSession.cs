using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iotDbConnector.DAL;

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

        static public iotDomain GetDomainForHttpContext(HttpContextBase hcontext)
        {
            try
            {
                var cont = (iotContext)hcontext.Session["iotcontext"];
                string domainId = DomainSession.GetContextDomain(hcontext);
                iotDomain d = cont.Domains.First(dm => dm.DomainName.Equals(domainId));
                return d;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}