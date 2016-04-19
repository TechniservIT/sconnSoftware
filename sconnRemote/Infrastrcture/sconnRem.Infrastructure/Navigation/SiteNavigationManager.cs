using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Device;
using sconnPrismSharedContext;
using sconnRem.Infrastructure.Content;

namespace sconnRem.Infrastructure.Navigation
{

    


    public static class SiteNavigationManager
    {
        private static Logger _nlogger = LogManager.GetCurrentClassLogger();
        
        private static sconnSite currentContextSconnSite;

        //alarm syste,
        private static AlarmSystemConfigManager alarmSystemConfigManager;
        private static ComposablePart exportedComposablePart;

        private static CompositionContainer contextContainer;

        private static sconnDevice alarmDevice;
        private static ComposablePart exportedDeviceComposablePart;

        //private static ConfigNavBootstrapper alarmBootstrapper;

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
                    //ConfigNavBootstrapper bootstrapper = new ConfigNavBootstrapper(alarmSystemConfigManager);
                    //bootstrapper.Run();
                }

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
 
          
        }
        
        public static void SetNavigationContextContainer(CompositionContainer container)
        {
            contextContainer = container;
        }

        public static void ActivateSiteContext(sconnSite site)
        {
            if (site != null)
            {
                currentContextSconnSite = site;

                EndpointInfo info = new EndpointInfo();
                info.Hostname = currentContextSconnSite.serverIP;
                info.Port = currentContextSconnSite.serverPort;
                DeviceCredentials cred = new DeviceCredentials();
                cred.Password = currentContextSconnSite.authPasswd;
                cred.Username = "";

                //ensure container does not maintain old manager
                if (alarmSystemConfigManager != null && exportedComposablePart != null && contextContainer != null)
                {
                    var batchrem = new CompositionBatch();
                    batchrem.RemovePart(exportedComposablePart);
                    contextContainer.Compose(batchrem);
                }

                alarmSystemConfigManager = new AlarmSystemConfigManager(info, cred);
                Device alrmSysDev = new Device();
                alrmSysDev.Credentials = cred;
                alrmSysDev.EndpInfo = info;
                alarmSystemConfigManager.RemoteDevice = alrmSysDev;

                AlarmSystemContext.SetManager(alarmSystemConfigManager);

                //register new manager in container
                if (contextContainer != null)
                {
                    var batch = new CompositionBatch();
                    exportedComposablePart = batch.AddExportedValue<IAlarmConfigManager>(alarmSystemConfigManager);
                    contextContainer.Compose(batch);
                }

            }
        }

        public static void ActivateDeviceContext(sconnDevice device)
        {
            alarmDevice = device;

            //ensure container does not maintain old manager
            if (alarmDevice != null && exportedDeviceComposablePart != null && contextContainer != null)
            {
                var batchrem = new CompositionBatch();
                batchrem.RemovePart(exportedDeviceComposablePart);
                contextContainer.Compose(batchrem);
            }

            //register new manager in container
            if (contextContainer != null)
            {
                var batch = new CompositionBatch();
                exportedDeviceComposablePart = batch.AddExportedValue<sconnDevice>(alarmDevice);
                contextContainer.Compose(batch);
            }
        }


    }


}
