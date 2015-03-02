using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iotDash.Startup))]
namespace iotDash
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
