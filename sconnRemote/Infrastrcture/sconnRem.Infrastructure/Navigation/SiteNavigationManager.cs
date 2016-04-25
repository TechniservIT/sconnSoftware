using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AlarmSystemManagmentService;
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

        public static sconnSite CurrentContextSconnSite;

        //alarm syste,
        public static AlarmSystemConfigManager alarmSystemConfigManager;
        public static event EventHandler ConfigManagerChangedEvent;
        private static ComposablePart exportedComposablePart;

        private static CompositionContainer contextContainer;

        public static sconnDevice CurrentContextDevice;
        public static event EventHandler AlarmDeviceChangedEvent;
        private static ComposablePart exportedDeviceComposablePart;

        public static sconnInput activeInput  = new sconnInput();
        public static sconnOutput activeOutput = new sconnOutput();
        public static sconnRelay activeRelay = new sconnRelay();

        public static sconnInput InputForId(string uuid)
        {
            if (CurrentContextDevice != null)
            {
                foreach (var input in CurrentContextDevice.Inputs)
                {
                    if (input.UUID.Equals(uuid))
                    {
                        return input;
                    }
                }
            }
            return new sconnInput();
        }

        public static void SaveInput(sconnInput input)
        {
            foreach (var dinput in CurrentContextDevice.Inputs)
            {
                if (dinput.UUID.Equals(input.UUID))
                {
                    dinput.CopyFrom(input);
                    DeviceConfigService serv = new DeviceConfigService(alarmSystemConfigManager,dinput.Id);
                    serv.Update(CurrentContextDevice);
                }
            }
        }


        public static sconnOutput OutputForId(string uuid)
        {
            if (CurrentContextDevice != null)
            {
                foreach (var input in CurrentContextDevice.Outputs)
                {
                    if (input.UUID.Equals(uuid))
                    {
                        return input;
                    }
                }
            }
            return new sconnOutput();
        }


        public static void SaveOutput(sconnOutput input)
        {
            foreach (var dinput in CurrentContextDevice.Outputs)
            {
                if (dinput.UUID.Equals(input.UUID))
                {
                    dinput.CopyFrom(input);
                    DeviceConfigService serv = new DeviceConfigService(alarmSystemConfigManager, dinput.Id);
                    serv.Update(CurrentContextDevice);
                }
            }
        }



        private static ComposablePart exportedInputComposablePart;


        //private static ConfigNavBootstrapper alarmBootstrapper;

        public static void ShowFullScreen()
        {
            if (CurrentContextSconnSite != null)
            {
                MainContentViewManager.DisplaySiteInCurrentContext(CurrentContextSconnSite);
            }
        }

        public static void ShowEditScreen()
        {

        }

        public static void ShowConfigureScreen()
        {
            try
            {
                if (CurrentContextSconnSite != null)
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
            try
            {
                if (site != null)
                {
                    CurrentContextSconnSite = site;

                    EndpointInfo info = new EndpointInfo();
                    info.Hostname = CurrentContextSconnSite.serverIP;
                    info.Port = CurrentContextSconnSite.serverPort;
                    DeviceCredentials cred = new DeviceCredentials();
                    cred.Password = CurrentContextSconnSite.authPasswd;
                    cred.Username = "";

                    ////ensure container does not maintain old manager
                    //if (alarmSystemConfigManager != null && exportedComposablePart != null && contextContainer != null)
                    //{
                    //    var batchrem = new CompositionBatch();
                    //    batchrem.RemovePart(exportedComposablePart);
                    //    contextContainer.Compose(batchrem);
                    //}

                    alarmSystemConfigManager = new AlarmSystemConfigManager(info, cred);
                    Device alrmSysDev = new Device();
                    alrmSysDev.Credentials = cred;
                    alrmSysDev.EndpInfo = info;
                    alarmSystemConfigManager.RemoteDevice = alrmSysDev;

                    AlarmSystemContext.SetManager(alarmSystemConfigManager);


                    //// register new manager in container
                    // if (contextContainer != null)
                    // {
                    //     var batch = new CompositionBatch();
                    //     exportedComposablePart = batch.AddExportedValue<IAlarmConfigManager>(alarmSystemConfigManager);
                    //     contextContainer.Compose(batch);
                    // }

                }
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }


        }

        public static void ActivateDeviceContext(sconnDevice device)
        {
            try
            {

                //try
                //{
                //    sconnDevice existingDevice = contextContainer.GetExportedValue<sconnDevice>();
                //    if (existingDevice != null)
                //    {
                //        existingDevice.CopyFrom(device);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    _nlogger.Error(ex, ex.Message);
                //}

                //ensure container does not maintain old manager
                //if (alarmDevice != null && exportedDeviceComposablePart != null && contextContainer != null)
                //{
                //    var batchrem = new CompositionBatch();
                //    batchrem.RemovePart(exportedDeviceComposablePart);
                //    exportedDeviceComposablePart = batchrem.AddExportedValue<sconnDevice>(device);
                //    contextContainer.Compose(batchrem);
                //}

                ////register new manager in container
                //else if (contextContainer != null)
                //{
                //    var batch = new CompositionBatch();
                //    exportedDeviceComposablePart = batch.AddExportedValue<sconnDevice>(device);
                //    contextContainer.Compose(batch);
                //}

                if (CurrentContextDevice != null)
                {

                    CurrentContextDevice.CopyFrom(device);
                }
                else
                {
                    CurrentContextDevice = device;
                }
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }

        }


        public static void ActivateInputContext(sconnInput input)
        {
            try
            {
                CurrentContextDevice.ActiveInput = input;
                activeInput = input;

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public static void ActivateOutputContext(sconnOutput io)
        {
            try
            {
                activeOutput = io;
            }

            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public static void ActivateRelayContext(sconnRelay io)
        {
            try
            {
                activeRelay = io;
            }

            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

    }


}
