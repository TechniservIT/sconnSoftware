using System;
using Microsoft.Owin;
using NLog;
using Owin;

[assembly: OwinStartupAttribute(typeof(iotDash.Startup))]
namespace iotDash
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();

        }
    }
}

