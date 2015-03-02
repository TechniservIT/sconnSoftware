using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using sconnConnector;
using iotDash.DAL.Device.Proprietary;
using iotDash.Models;
using iodash.Models.Auth.Credential;

namespace iodash.Models.Common
{
    public class CommSconnProtocol :ICommProtocol
    {
        private Device protocolDevice;
        
        private sconnSite site { get; set; }

        private sconnCfgMngr cfgMngr { get; set; }

        private int HostDeviceId = 0;

        public CommSconnProtocol(Device dev)
        {
            site = new sconnSite();
            cfgMngr = new sconnCfgMngr();
            LoadDevice(dev);
        }

        private void LoadDevice(Device dev)
        {
            protocolDevice = dev;
            SetSiteEndpoint(dev);
        }



        public bool LoadDeviceActions(Device device)
        {
            LoadDevice(device);
            UpdateSite();
            LoadConfigToDevice(device);
            return false;
        }

        public bool LoadDeviceProperties(Device device)
        {
            LoadDevice(device);
            UpdateSite();
            LoadConfigToDevice(device);
            return false;
        }


        public bool ProtocolDeviceQueryAble()
        {
            return true;
        }

        private void SetSiteEndpoint(Device dev)
        {
            site.serverIP = dev.EndpInfo.Hostname;
            site.serverPort = dev.EndpInfo.Port;
            site.authPasswd = dev.EndpInfo.Device.Credentials.Password;
        }

        private bool UpdateSite()
        {
           return  cfgMngr.ReadSiteRunningConfig(site);
        }

        private bool WriteSiteConfig(){
            return cfgMngr.WriteDeviceCfg(site); 
        }


        //get endpoint device id
        private int GetDeviceId()
        {
            return 0;
        }

        private ushort WordFromBufferAtAddr(byte[] buffer, int addr)
        {
            ushort val;
            val = buffer[addr + 1];
            val |= (ushort)((buffer[addr])<<8);
            return val;
        }

        private int IndexForHostDevice()
        {
            for (int i = 0; i < site.siteCfg.deviceNo; i++)
			{
			     if ( WordFromBufferAtAddr(site.siteCfg.deviceConfigs[i].memCFG, ipcDefines.mAdrDevID) == HostDeviceId){
                     return i;
                }
			}
            return -1;
        }

        private bool GetDeviceRunCfg(){
            return true;
        }

        private int ConfigAddrForMapperInfo(sconnConfigMapper info)
        {
            return 0;
        }

        private byte[] LoadStringValueToMemoryAtAddr(string val, byte[] memory, int addr)
        {
            return new byte[0];
        }

        private void SetConfigForActionParameterAtDevice(DeviceParameter param, ipcDataType.ipcDeviceConfig config)
        {
            config.memCFG = LoadStringValueToMemoryAtAddr(param.Value, config.memCFG, ConfigAddrForMapperInfo(param.sconnMapper));
        }

        private void DeviceActionToConfig(DeviceAction action)
        {
            int devAddr = IndexForHostDevice();
            foreach (var param in action.RequiredParameters)
            {
                SetConfigForActionParameterAtDevice(param, site.siteCfg.deviceConfigs[devAddr]);
            }
        }

        private string sconnConfigToStringVal(sconnConfigMapper map, ipcDataType.ipcDeviceConfig config)
        {
            return "";
        }

        private void LoadConfigToProperty(DeviceProperty property)
        {
            int devAddr = IndexForHostDevice();
            foreach (var param in property.ResultParameters)
            {
                param.Value = sconnConfigToStringVal(param.sconnMapper, site.siteCfg.deviceConfigs[devAddr]);
            }
        }


        private void LoadConfigToAction(DeviceAction action)
        {
            int devAddr = IndexForHostDevice();
            foreach (var param in action.ResultParameters)
            {
                param.Value = sconnConfigToStringVal(param.sconnMapper, site.siteCfg.deviceConfigs[devAddr]);
            }
        }

        private void LoadConfigToDevice(Device dev)
        {
            int devAddr = IndexForHostDevice();


            //Reload properties and actions if they changed


            //Update 
            
            int inputs = site.siteCfg.deviceConfigs[devAddr].memCFG[ipcDefines.mAdrInputsNO];
            for (int i = 0; i < inputs; i++)
            {
                DeviceProperty prop = new DeviceProperty();
                prop.PropertyName = "Input" + i;    //TODO read from name cfg
                DeviceParameter param = new DeviceParameter();
                sconnConfigMapper maper = new sconnConfigMapper();
                maper.ConfigType = ipcDefines.mAdrInput;
                maper.SeqNumber = i;
                param.sconnMapper = maper;
                param.Value = sconnConfigToStringVal(maper,site.siteCfg.deviceConfigs[devAddr]);
                prop.ResultParameters.Add(param);
                dev.Properties.Add(prop);
            }

            int outputs = site.siteCfg.deviceConfigs[devAddr].memCFG[ipcDefines.mAdrOutputsNO];
            for (int i = 0; i < outputs; i++)
            {
                DeviceAction action = new DeviceAction();
                action.ActionName = "Output" + i;    //TODO read from name cfg
                DeviceParameter inparam = new DeviceParameter();
                //DeviceParameter outparam = new DeviceParameter();
                sconnConfigMapper maper = new sconnConfigMapper();
                maper.ConfigType = ipcDefines.mAdrOutput;
                maper.SeqNumber = i;
                inparam.sconnMapper = maper;
                inparam.Value = sconnConfigToStringVal(maper, site.siteCfg.deviceConfigs[devAddr]);
                action.ResultParameters.Add(inparam);
                dev.Actions.Add(action);
            }

            int relays = site.siteCfg.deviceConfigs[devAddr].memCFG[ipcDefines.mAdrRelayNO];
            for (int i = 0; i < relays; i++)
            {
                DeviceAction action = new DeviceAction();
                action.ActionName = "Relay" + i;    //TODO read from name cfg
                DeviceParameter inparam = new DeviceParameter();
                //DeviceParameter outparam = new DeviceParameter();
                sconnConfigMapper maper = new sconnConfigMapper();
                maper.ConfigType = ipcDefines.mAdrRelay;
                maper.SeqNumber = i;
                inparam.sconnMapper = maper;
                inparam.Value = sconnConfigToStringVal(maper, site.siteCfg.deviceConfigs[devAddr]);
                action.ResultParameters.Add(inparam);
                dev.Actions.Add(action);
            }   

        }

        private void AddPropertyWithConfigAddr(int addr)
        {

        }



        /******************* Public ************************/


        public bool PerformActionAsync(DeviceAction action)
        {
            DeviceActionToConfig(action);
            return WriteSiteConfig();   
        }

        public bool PerformActionsAsync(List<DeviceAction> actions)
        {
            foreach (var action in actions)
            {
                DeviceActionToConfig(action);
            }
            return WriteSiteConfig();   
        }

        public bool ReadPropertyAsync(DeviceProperty property)
        {
            bool update = UpdateSite();
            if (update)
            {
                LoadConfigToProperty(property);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ReadPropertiesAsync(List<DeviceProperty> properties)
        {
            bool update = UpdateSite();
            if (update)
            {
                foreach (DeviceProperty property in properties)
                {
                    LoadConfigToProperty(property);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public Device GetDevice()
        {
            Device dev = new Device();
            UpdateSite();
            LoadConfigToDevice(dev);

            ApplicationDbContext cont = new ApplicationDbContext(); 
            dev.DeviceName = Guid.NewGuid().ToString();
            EndpointInfo epi = (from ep in cont.Endpoints
                                where ep.Hostname == this.site.serverIP &&
                                     ep.Port == this.site.serverPort
                                select ep).First();
            if (epi != null)
            {
                dev.EndpInfo = epi;
            }
            DeviceCredentials cred = (from c in cont.Credentials
                                          where c.Password == this.site.authPasswd 
                                          select c).First();
            if ( cred != null){
                dev.Credentials = cred;
            }

            DeviceType type = (from dt in cont.Types
                               where dt.TypeName.Equals("sconnMB")
                               select dt).First();
            if (type != null)
            {
                dev.Type = type;
            }
            else
            {
                //add type
            }
            

            return dev;
        }

 

    }


}