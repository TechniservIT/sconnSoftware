using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AlarmSystemManagmentService;
using AlarmSystemManagmentService.Device;
using NLog;
using Prism.Regions;
using sconnConnector;
using sconnConnector.Annotations;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Device;
using sconnNetworkingServices.Abstract;
using sconnRem.Infrastructure.Content;
using sconnRem.Shells.Config;

namespace sconnRem.Infrastructure.Navigation
{

    public interface IAlarmSystemNavigationService : INotifyPropertyChanged
    {
        AlarmSystemConfigManager alarmSystemConfigManager { get; set; }
        event EventHandler ConfigManagerChangedEvent;
        sconnDevice CurrentContextDevice { get; set; }
        AlarmDevicesConfigService DevicesConfigService { get; set; }
        List<sconnDevice> DeviceConfigs { get; set; }
        event EventHandler AlarmDeviceChangedEvent;
        sconnSite CurrentContextSconnSite { get; set; }
        sconnInput activeInput { get; set; }
        sconnOutput activeOutput { get; set; }
        sconnRelay activeRelay { get; set; }
        bool Online { get; set; }
        
        List<sconnDevice> GetDevices();
        void SaveOutputGeneric(sconnOutput output);
        sconnInput InputForId(string uuid);
        void SaveInput(sconnInput input);
        sconnOutput OutputForId(string uuid);
        void SaveOutput(sconnOutput output);
        void ShowFullScreen();
        void ShowEditScreen();
        void ShowConfigureScreen();
        void ActivateSiteContext(sconnSite site);
        void ActivateDeviceContext(sconnDevice device);
        void ActivateInputContext(sconnInput input);
        void ActivateOutputContext(sconnOutput io);
        void ActivateRelayContext(sconnRelay io);
        void EditSite(sconnSite site);
        void RemoveSite(sconnSite site);
        void OpenSiteWizard();

        event PropertyChangedEventHandler PropertyChanged;
    }


    [Export(typeof(IAlarmSystemNavigationService))]
    public class AlarmSystemNavigationService : INotifyPropertyChanged, IAlarmSystemNavigationService
    {
        private static Logger _nlogger = LogManager.GetCurrentClassLogger();
        
        private AlarmSystemConfigManager _alarmSystemConfigManager;
        public AlarmSystemConfigManager alarmSystemConfigManager
        {
            get { return _alarmSystemConfigManager; }
            set
            {
                _alarmSystemConfigManager = value;
                OnPropertyChanged();
            }
        }

        public  event EventHandler ConfigManagerChangedEvent;


        private AlarmDevicesConfigService _devicesConfigService;
        public AlarmDevicesConfigService DevicesConfigService
        {
            get { return _devicesConfigService; }
            set
            {
                _devicesConfigService = value;
                OnPropertyChanged();
            }
        }


        private List<sconnDevice> _DeviceConfigs;
        public List<sconnDevice> DeviceConfigs
        {
            get { return _DeviceConfigs; }
            set
            {
                _DeviceConfigs = value;
                OnPropertyChanged();
            }
        }



        private sconnDevice _CurrentContextDevice;
        public sconnDevice CurrentContextDevice
        {
            get { return _CurrentContextDevice; }
            set
            {
                _CurrentContextDevice = value;
                OnPropertyChanged();
            }
        }


        public  event EventHandler AlarmDeviceChangedEvent;

        private sconnSite _CurrentContextSconnSite;
        public sconnSite CurrentContextSconnSite
        {
            get { return _CurrentContextSconnSite; }
            set
            {
                _CurrentContextSconnSite = value;
                OnPropertyChanged();
            }
        }

        private sconnInput _activeInput;
        public sconnInput activeInput
        {
            get { return _activeInput; }
            set
            {
                _activeInput = value;
                OnPropertyChanged();
            }
        }
        
        private sconnOutput _activeOutput;
        public sconnOutput activeOutput
        {
            get { return _activeOutput; }
            set
            {
                _activeOutput = value;
                OnPropertyChanged();
            }
        }

        private sconnRelay _activeRelay;
        public sconnRelay activeRelay
        {
            get { return _activeRelay; }
            set
            {
                _activeRelay = value;
                OnPropertyChanged();
            }
        }

        private  bool _online;
        public  bool Online
        {
            get { return _online; }
            set
            {
                _online = value;
                OnPropertyChanged();
            }
        }

    
        public AlarmSystemNavigationService()
        {
            NetworkClientStatusUpdateService.ConnectionStateChanged += NetworkClientStatusUpdateService_ConnectionStateChanged;
            DeviceConfigs = new List<sconnDevice>();
            Online = true;
            alarmSystemConfigManager = new AlarmSystemConfigManager();
        }

        public  List<sconnDevice> GetDevices()
        {
            if (Online)
            {
                DevicesConfigService = new AlarmDevicesConfigService(alarmSystemConfigManager);
                DeviceConfigs = DevicesConfigService.GetAll();
            }
            return DeviceConfigs;

        }

        public  void SaveOutputGeneric(sconnOutput output)
        {
            foreach (var dev in DeviceConfigs)
            {
                foreach (var dinput in dev.Outputs)
                {
                    if (dinput.UUID.Equals(output.UUID))
                    {
                        dinput.CopyFrom(output);
                        if (Online)
                        {
                            AlarmDevicesConfigService serv = new AlarmDevicesConfigService(alarmSystemConfigManager);
                            serv.Update(dev);   //, dinput.Id   
                        }

                    }
                }
            }

        }

        private  void NetworkClientStatusUpdateService_ConnectionStateChanged(object sender, EventArgs e)
        {
            //show connection status in view
        }

        public  sconnInput InputForId(string uuid)
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

        public  void SaveInput(sconnInput input)
        {
            foreach (var dinput in CurrentContextDevice.Inputs)
            {
                if (dinput.UUID.Equals(input.UUID))
                {
                    dinput.CopyFrom(input);
                    if (Online)
                    {
                        AlarmDevicesConfigService serv = new AlarmDevicesConfigService(alarmSystemConfigManager);
                        serv.Update(CurrentContextDevice);
                    }
                }
            }
        }


        public  sconnOutput OutputForId(string uuid)
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


        public  void SaveOutput(sconnOutput output)
        {
            foreach (var dinput in CurrentContextDevice.Outputs)
            {
                if (dinput.UUID.Equals(output.UUID))
                {
                    dinput.CopyFrom(output);
                    if (Online)
                    {
                        AlarmDevicesConfigService serv = new AlarmDevicesConfigService(alarmSystemConfigManager);
                        serv.Update(CurrentContextDevice);
                    }
                }
            }
        }

        private  ComposablePart exportedInputComposablePart;

        public  void ShowFullScreen()
        {
            if (CurrentContextSconnSite != null)
            {
                MainContentViewManager.DisplaySiteInCurrentContext(CurrentContextSconnSite);
            }
        }

        public  void ShowEditScreen()
        {

        }

        public  void ShowConfigureScreen()
        {
            try
            {
                if (CurrentContextSconnSite != null)
                {
                }

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }
        

        public  void ActivateSiteContext(sconnSite site)
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
                    AlarmSystemConfigManager nman = new AlarmSystemConfigManager(info, cred) { RemoteDevice = alrmSysDev };
                    alarmSystemConfigManager.CopyFrom(nman);
                    //AlarmSystemContext.SetManager(alarmSystemConfigManager);

                }
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }


        }

        public  void ActivateDeviceContext(sconnDevice device)
        {
            try
            {
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


        public  void ActivateInputContext(sconnInput input)
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

        public  void ActivateOutputContext(sconnOutput io)
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

        public  void ActivateRelayContext(sconnRelay io)
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




        public void EditSite(sconnSite site)
        {
            SiteConnectionWizardBootstrapper boot = new SiteConnectionWizardBootstrapper(site);
            boot.Run();
        }

        public  void RemoveSite(sconnSite site)
        {
            //prompt
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Please confirm deleting follow site : " + Environment.NewLine + site.siteName, "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                sconnDataShare.removeSite(site);
                //force list reload
            }
        }


        public void OpenSiteWizard()
        {
            SiteConnectionWizardBootstrapper boot = new SiteConnectionWizardBootstrapper();
            boot.Run();
          
            //navigate contract
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    //public static class SiteNavigationManager
    //{
    //    private static Logger _nlogger = LogManager.GetCurrentClassLogger();

    //    public static sconnSite CurrentContextSconnSite;
    //    public static AlarmSystemConfigManager alarmSystemConfigManager = new AlarmSystemConfigManager();
    //    public static event EventHandler ConfigManagerChangedEvent;
    //    private static ComposablePart exportedComposablePart;

    //    private static CompositionContainer contextContainer;

    //    public static sconnDevice CurrentContextDevice;

    //    public static AlarmDevicesConfigService _devicesConfigService;
    //    public static List<sconnDevice> DeviceConfigs;
         
    //    public static event EventHandler AlarmDeviceChangedEvent;
    //    private static ComposablePart exportedDeviceComposablePart;

    //    public static sconnInput activeInput  = new sconnInput();
    //    public static sconnOutput activeOutput = new sconnOutput();
    //    public static sconnRelay activeRelay = new sconnRelay();


    //    private static bool _online;

    //    public static bool Online
    //    {
    //        get { return _online; }
    //        set
    //        {
    //            _online = value;
    //        }
    //    }

    //    static SiteNavigationManager()
    //    {
    //        NetworkClientStatusUpdateService.ConnectionStateChanged += NetworkClientStatusUpdateService_ConnectionStateChanged;
    //        DeviceConfigs = new List<sconnDevice>();
    //    }

    //    public static List<sconnDevice> GetDevices()
    //    {
    //        if (Online)
    //        {
    //            _devicesConfigService = new AlarmDevicesConfigService(alarmSystemConfigManager);
    //            DeviceConfigs = _devicesConfigService.GetAll();
    //        }
    //        return DeviceConfigs;

    //    }

    //    public static void SaveOutputGeneric(sconnOutput output)
    //    {
    //        foreach (var dev in DeviceConfigs)
    //        {
    //            foreach (var dinput in dev.Outputs)
    //            {
    //                if (dinput.UUID.Equals(output.UUID))
    //                {
    //                    dinput.CopyFrom(output);
    //                    if (Online)
    //                    {
    //                        AlarmDevicesConfigService serv = new AlarmDevicesConfigService(alarmSystemConfigManager);
    //                        serv.Update(dev);   //, dinput.Id   
    //                    }
                           
    //                }
    //            }
    //        }

    //    }

    //    private static void NetworkClientStatusUpdateService_ConnectionStateChanged(object sender, EventArgs e)
    //    {
    //        //show connection status in view
    //    }

    //    public static sconnInput InputForId(string uuid)
    //    {
    //        if (CurrentContextDevice != null)
    //        {
    //            foreach (var input in CurrentContextDevice.Inputs)
    //            {
    //                if (input.UUID.Equals(uuid))
    //                {
    //                    return input;
    //                }
    //            }
    //        }
    //        return new sconnInput();
    //    }

    //    public static void SaveInput(sconnInput input)
    //    {
    //        foreach (var dinput in CurrentContextDevice.Inputs)
    //        {
    //            if (dinput.UUID.Equals(input.UUID))
    //            {
    //                dinput.CopyFrom(input);
    //                if (Online)
    //                {
    //                    AlarmDevicesConfigService serv = new AlarmDevicesConfigService(alarmSystemConfigManager);
    //                    serv.Update(CurrentContextDevice);
    //                }
    //            }
    //        }
    //    }


    //    public static sconnOutput OutputForId(string uuid)
    //    {
    //        if (CurrentContextDevice != null)
    //        {
    //            foreach (var input in CurrentContextDevice.Outputs)
    //            {
    //                if (input.UUID.Equals(uuid))
    //                {
    //                    return input;
    //                }
    //            }
    //        }
    //        return new sconnOutput();
    //    }


    //    public static void SaveOutput(sconnOutput output)
    //    {
    //        foreach (var dinput in CurrentContextDevice.Outputs)
    //        {
    //            if (dinput.UUID.Equals(output.UUID))
    //            {
    //                dinput.CopyFrom(output);
    //                if (Online)
    //                {
    //                    AlarmDevicesConfigService serv = new AlarmDevicesConfigService(alarmSystemConfigManager);
    //                    serv.Update(CurrentContextDevice);
    //                }
    //            }
    //        }
    //    }

    //    private static ComposablePart exportedInputComposablePart;

    //    public static void ShowFullScreen()
    //    {
    //        if (CurrentContextSconnSite != null)
    //        {
    //            MainContentViewManager.DisplaySiteInCurrentContext(CurrentContextSconnSite);
    //        }
    //    }

    //    public static void ShowEditScreen()
    //    {

    //    }

    //    public static void ShowConfigureScreen()
    //    {
    //        try
    //        {
    //            if (CurrentContextSconnSite != null)
    //            {
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            _nlogger.Error(ex, ex.Message);
    //        }
    //    }

    //    public static void SetNavigationContextContainer(CompositionContainer container)
    //    {
    //        contextContainer = container;
    //    }

    //    public static void ActivateSiteContext(sconnSite site)
    //    {
    //        try
    //        {
    //            if (site != null)
    //            {
    //                CurrentContextSconnSite = site;

    //                EndpointInfo info = new EndpointInfo
    //                {
    //                    Hostname = CurrentContextSconnSite.serverIP,
    //                    Port = CurrentContextSconnSite.serverPort
    //                };
    //                DeviceCredentials cred = new DeviceCredentials
    //                {
    //                    Password = CurrentContextSconnSite.authPasswd,
    //                    Username = ""
    //                };

    //                Device alrmSysDev = new Device
    //                {
    //                    Credentials = cred,
    //                    EndpInfo = info
    //                };
    //                AlarmSystemConfigManager nman = new AlarmSystemConfigManager(info, cred) {RemoteDevice = alrmSysDev};
    //                alarmSystemConfigManager.CopyFrom(nman);
    //                AlarmSystemContext.SetManager(alarmSystemConfigManager);

    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _nlogger.Error(ex, ex.Message);
    //        }


    //    }

    //    public static void ActivateDeviceContext(sconnDevice device)
    //    {
    //        try
    //        {

    //            if (CurrentContextDevice != null)
    //            {

    //                CurrentContextDevice.CopyFrom(device);
    //            }
    //            else
    //            {
    //                CurrentContextDevice = device;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _nlogger.Error(ex, ex.Message);
    //        }

    //    }


    //    public static void ActivateInputContext(sconnInput input)
    //    {
    //        try
    //        {
    //            CurrentContextDevice.ActiveInput = input;
    //            activeInput = input;

    //        }
    //        catch (Exception ex)
    //        {
    //            _nlogger.Error(ex, ex.Message);
    //        }
    //    }

    //    public static void ActivateOutputContext(sconnOutput io)
    //    {
    //        try
    //        {
    //            activeOutput = io;
    //        }

    //        catch (Exception ex)
    //        {
    //            _nlogger.Error(ex, ex.Message);
    //        }
    //    }

    //    public static void ActivateRelayContext(sconnRelay io)
    //    {
    //        try
    //        {
    //            activeRelay = io;
    //        }

    //        catch (Exception ex)
    //        {
    //            _nlogger.Error(ex, ex.Message);
    //        }
    //    }




    //    public static void EditSite(sconnSite site)
    //    {
    //        SiteConnectionWizardBootstrapper boot = new SiteConnectionWizardBootstrapper(site);
    //        boot.Run();
    //    }

    //    public static void RemoveSite(sconnSite site)
    //    {
    //        //prompt
    //        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Please confirm deleting follow site : " +Environment.NewLine + site.siteName, "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
    //        if (messageBoxResult == MessageBoxResult.Yes)
    //        {
    //            sconnDataShare.removeSite(site);
    //            //force list reload
    //        }
    //    }
        

    //    public static void OpenSiteWizard()
    //    {
    //        SiteConnectionWizardBootstrapper boot = new SiteConnectionWizardBootstrapper();
    //        boot.Run();
    //    }
        

    //}


}
