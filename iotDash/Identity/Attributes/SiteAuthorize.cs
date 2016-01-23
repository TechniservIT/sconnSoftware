using iotDash.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iotDash.Identity.Attributes
{

    public class SiteAuthorizeAttribute : DomainAuthorizeAttribute
    {
        public string AppDomain { get; set; }

        private readonly bool _authorize;

        public SiteAuthorizeAttribute()
        {
            _authorize = true;  //auth by default
        }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!_authorize)
                return true;

            try
            {
                bool basicAuthed = base.AuthorizeCore(httpContext);
                if (basicAuthed)
                {

                    if (DomainAuthHelper.IsUserContextDomainAuthorized(httpContext))
                    {
                        return DomainAuthHelper.UserHasSiteAccess(httpContext);
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }
    }




}