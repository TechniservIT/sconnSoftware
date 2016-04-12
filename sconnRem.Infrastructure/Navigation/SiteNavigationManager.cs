using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;
using NLog;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnRem.Infrastructure.Content;
using sconnRem.Shells.Config;

namespace sconnRem.Infrastructure.Navigation
{
    public static class SiteNavigationManager
    {
        private static Logger _nlogger = LogManager.GetCurrentClassLogger();


        private static sconnSite currentContextSconnSite;

        public static void ShowFullScreen()
        {
            if (currentContextSconnSite != null)
            {
                MainContentViewManager.DisplaySiteInCurrentContext(currentContextSconnSite);
            }
        }

        public static void ShowEditScreen()
        {

        }

        public static void ShowConfigureScreen()
        {
            try
            {
                if (currentContextSconnSite != null)
                {
                   // WndConfigureSiteShell wnd = new WndConfigureSiteShell();

                    EndpointInfo info = new EndpointInfo();
                    info.Hostname = currentContextSconnSite.serverIP;
                    info.Port = currentContextSconnSite.serverPort;
                    DeviceCredentials cred = new DeviceCredentials();
                    cred.Password = currentContextSconnSite.authPasswd;
                    cred.Username = "";
                    AlarmSystemConfigManager manager = new AlarmSystemConfigManager(info, cred);
                    Device alrmSysDev = new Device();
                    alrmSysDev.Credentials = cred;
                    alrmSysDev.EndpInfo = info;
                    manager.RemoteDevice = alrmSysDev;


                    ConfigNavBootstrapper bootstrapper = new ConfigNavBootstrapper(manager);
                    bootstrapper.Run();

                    //ConfigureSiteViewModel context = new ConfigureSiteViewModel(manager);
                    //wnd.DataContext = context;
                    //wnd.Show();
                }

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
 
          
        }

        public static void ActivateSiteContext(sconnSite site)
        {
            if (site != null)
            {
                currentContextSconnSite = site;
            }
        }


    }


}
