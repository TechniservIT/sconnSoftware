using iotDash.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Linq;
using System.Web.Security;
using iotDash.Controllers.domain.managment.security;

namespace iotDash
{

    public partial class Startup
    {

        private void SetupTestAccounts()
        {
            //ApplicationDbContext cont = new ApplicationDbContext();
            //var iotDevRoles = cont.Roles.Where(r => r.Name.Equals("DomainAdmin"));
            //if (iotDevRoles.Count() <= 0)
            //{
            //    IdentityRole adminRoleCr = new IdentityRole("DomainAdmin");
            //    cont.Roles.Add(adminRoleCr);
            //    cont.SaveChanges();
            //}


        }


        private void SetupRoles()
        {


        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {


            // Configure the db context, user manager and role 
            // manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.);
           // app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManage.);
            //app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });


            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication();

            SetupRoles();
            SetupTestAccounts();
        }
    }
}