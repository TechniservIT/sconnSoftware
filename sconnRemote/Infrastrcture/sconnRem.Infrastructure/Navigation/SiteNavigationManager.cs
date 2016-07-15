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
using AlarmSystemManagmentService.Device;
using NLog;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Device;
using sconnNetworkingServices.Abstract;
using sconnPrismSharedContext;
using sconnRem.Infrastructure.Content;
using sconnRem.Shells.Config;

namespace sconnRem.Infrastructure.Navigation
{




    public static class SiteNavigationManager
    {
        private static Logger _nlogger = LogManager.GetCurrentClassLogger();

        public static sconnSite CurrentContextSconnSite;

        //alarm syste,
        public static AlarmSystemConfigManager alarmSystemConfigManager = new AlarmSystemConfigManager();
        public static event EventHandler ConfigManagerChangedEvent;
        private static ComposablePart exportedComposablePart;

        private static CompositionContainer contextContainer;

        public static sconnDevice CurrentContextDevice;

        public static AlarmDevicesConfigService _devicesConfigService;
        public static List<sconnDevice> DeviceConfigs;
         
        public static event EventHandler AlarmDeviceChangedEvent;
        private static ComposablePart exportedDeviceComposablePart;

        public static sconnInput activeInput  = new sconnInput();
        public static sconnOutput activeOutput = new sconnOutput();
        public static sconnRelay activeRelay = new sconnRelay();



        static SiteNavigationManager()
        {
            NetworkClientStatusUpdateService.ConnectionStateChanged += NetworkClientStatusUpdateService_ConnectionStateChanged;
            DeviceConfigs = new List<sconnDevice>();
        }

        public static List<sconnDevice> GetDevices()
        {
            _devicesConfigService = new AlarmDevicesConfigService(alarmSystemConfigManager);
            DeviceConfigs = _devicesConfigService.GetAll();
            return DeviceConfigs;
        }

        public static void SaveOutputGeneric(sconnOutput output)
        {
            foreach (var dev in DeviceConfigs)
            {
                foreach (var dinput in dev.Outputs)
                {
                    if (dinput.UUID.Equals(output.UUID))
                    {
                        dinput.CopyFrom(output);
                        AlarmDevicesConfigService serv = new AlarmDevicesConfigService(alarmSystemConfigManager);
                        serv.Update(dev);   //, dinput.Id
                    }
                }
            }

        }

        private static void NetworkClientStatusUpdateService_ConnectionStateChanged(object sender, EventArgs e)
        {
            //show connection status in view
        }

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
                    AlarmDevicesConfigService serv = new AlarmDevicesConfigService(alarmSystemConfigManager);
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


        public static void SaveOutput(sconnOutput output)
        {
            foreach (var dinput in CurrentContextDevice.Outputs)
            {
                if (dinput.UUID.Equals(output.UUID))
                {
                    dinput.CopyFrom(output);
                    AlarmDevicesConfigService serv = new AlarmDevicesConfigService(alarmSystemConfigManager);
                    serv.Update(CurrentContextDevice);
                }
            }
        }

        private static ComposablePart exportedInputComposablePart;

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

                    EndpointInfo info = new EndpointInfo
                    {
                        Hostname = CurrentContextSconnSite.serverIP,
                        Port = CurrentContextSconnSite.serverPort
                    };
                    DeviceCredentials cred = new DeviceCredentials
                    {
                        Password = CurrentContextSconnSite.authPasswd,
                        Username = ""
                    };

                    Device alrmSysDev = new Device
                    {
                        Credentials = cred,
                        EndpInfo = info
                    };
                    AlarmSystemConfigManager nman = new AlarmSystemConfigManager(info, cred) {RemoteDevice = alrmSysDev};
                    alarmSystemConfigManager.CopyFrom(nman);
                    AlarmSystemContext.SetManager(alarmSystemConfigManager);

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




        public static void EditSite(sconnSite site)
        {
            SiteConnectionWizardBootstrapper boot = new SiteConnectionWizardBootstrapper(site);
            boot.Run();
        }

        public static void OpenSiteWizard()
        {
            SiteConnectionWizardBootstrapper boot = new SiteConnectionWizardBootstrapper();
            boot.Run();
        }






    }


}
