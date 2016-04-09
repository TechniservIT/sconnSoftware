using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnRem.Infrastructure.Content;

namespace sconnRem.Infrastructure.Navigation
{
    static public class SiteNavigationManager
    {

        static public void ShowFullScreen(sconnSite site)
        {
            MainContentViewManager.DisplaySiteInCurrentContext(site);
        }

        static public void ShowEditScreen(sconnSite site)
        {

        }

        static public void ShowConfigureScreen(sconnSite site)
        {
            wndConfigureSiteShell wnd = new wndConfigureSiteShell();

            EndpointInfo info = new EndpointInfo();
            info.Hostname = site.serverIP;
            info.Port = site.serverPort;
            DeviceCredentials cred = new DeviceCredentials();
            cred.Password = site.authPasswd;
            cred.Username = "";
            AlarmSystemConfigManager manager = new AlarmSystemConfigManager(info, cred);
            Device alrmSysDev = new Device();
            alrmSysDev.Credentials = cred;
            alrmSysDev.EndpInfo = info;
            manager.RemoteDevice = alrmSysDev;


            ConfigNavBootstrapper bootstrapper = new ConfigNavBootstrapper(manager);
            bootstrapper.Run();

            ConfigureSiteViewModel context = new ConfigureSiteViewModel(_Manager);
            wnd.DataContext = context;
            wnd.Show();
        }


    }


}
