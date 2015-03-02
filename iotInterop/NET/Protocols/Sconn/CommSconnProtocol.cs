using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using sconnConnector;
using System.Threading.Tasks;
using iotInterop.iotDbService;

namespace iotInterop
{
    public class CommSconnProtocol :ICommProtocol
    {
        private Device protocolDevice;
        
        private sconnSite site { get; set; }

        private sconnCfgMngr cfgMngr { get; set; }

        private iotConnector connector;

        int devices = 0;

        private int HostDeviceId = 0;

        public CommSconnProtocol()
        {
            connector = new iotConnector();
            site = new sconnSite();
            cfgMngr = new sconnCfgMngr();
        }

        public CommSconnProtocol(Device dev) :this()
        {
            LoadDevice(dev);
        }

        private void LoadDevice(Device dev)
        {
            protocolDevice = dev;
            SetSiteEndpoint(dev);
        }

        private void UpdateConfig(Device device)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                LoadConfigToDevice(device);
            });        
        }

        public bool LoadDeviceActions(Device device)
        {
            try
            {
                LoadDevice(device);
                if (UpdateSite())
                {
                    UpdateConfig(device);
                    return true;
                }
            }
            catch (Exception e)
            {    
            }
            return false;
        }

        public bool LoadDeviceProperties(Device device)
        {
            LoadDevice(device);

            if (UpdateSite())
            {
                return LoadConfigToDevice(device);
            }

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
            site.authPasswd = dev.Credentials.Password;
        }

        private bool UpdateSite()
        {
            int currDevs = cfgMngr.getSiteDeviceNo(site);
            if (site.siteCfg != null)
            {
                if (site.siteCfg.deviceNo != currDevs)
                {
                    devices = currDevs;
                    site.siteCfg = new ipcSiteConfig(devices); //init device configs 
                }
                else
                {
                    devices = site.siteCfg.deviceNo;
                }
            }
            else
            {
                devices = cfgMngr.getSiteDeviceNo(site);
                site.siteCfg = new ipcSiteConfig(devices); //init device configs    
            }

            return cfgMngr.ReadSiteRunningConfig(site);
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

        private int ConfigLengthForType(int type)
        {
            if (type == ipcDefines.mAdrInput)
            {
                return ipcDefines.mAdrInputMemSize;
            }
            else if (type == ipcDefines.mAdrOutput)
            {
                return ipcDefines.mAdrOutputMemSize;
            }
            else if (type == ipcDefines.mAdrRelay)
            {
                return ipcDefines.RelayMemSize;
            }
            else
            {
                return 0;
            }  
        }


        private int ConfigAddrForMapperInfo(sconnConfigMapper map)
        {
            if (map.ConfigType == ipcDefines.mAdrInput)
            {
                return ipcDefines.mAdrInput + (ipcDefines.mAdrInputMemSize * map.SeqNumber) + ipcDefines.mAdrInputVal;
            }
            else if (map.ConfigType == ipcDefines.mAdrOutput)
            {
                return ipcDefines.mAdrOutput + ipcDefines.mAdrOutputMemSize * map.SeqNumber + ipcDefines.mAdrOutputVal;
            }
            else if (map.ConfigType == ipcDefines.mAdrRelay)
            {
                return ipcDefines.mAdrRelay + ipcDefines.RelayMemSize * map.SeqNumber + ipcDefines.mAdrRelayVal;
            }
            else
            {
                return 0;    //direct value
            } 
        }



        private void LoadStringValueToByteMemoryAtAddr(string val, byte[] memory, int addr)
        {
            //parse types
            try
            {
                int ival = int.Parse(val);
                if (ival > byte.MaxValue) //word
                {
                    ival = byte.MaxValue;
                }
                memory[addr] = (byte)ival;
            }
            catch (Exception e)
            {
             
            }
        }

        private void SetConfigForActionParameterAtDevice(ActionParameter param, ipcDataType.ipcDeviceConfig config)
        {
            LoadStringValueToByteMemoryAtAddr(param.Value, config.memCFG, ConfigAddrForMapperInfo(param.sconnMappers.First()));
        }

        private void DeviceActionToConfig(DeviceAction action)
        {
            try
            {
                int devAddr = IndexForHostDevice();
                if (devAddr != -1)
                {
                    foreach (var param in action.RequiredParameters)
                    {
                        SetConfigForActionParameterAtDevice(param, site.siteCfg.deviceConfigs[devAddr]);
                    }
                }

            }
            catch (Exception e)
            {
                
             
            }
        }

        private string sconnConfigToStringVal(sconnConfigMapper map, ipcDataType.ipcDeviceConfig config)
        {
            if (map.ConfigType == ipcDefines.mAdrInput)
            {
                return config.memCFG[ConfigAddrForMapperInfo(map)].ToString();
            }
            else if (map.ConfigType == ipcDefines.mAdrOutput)
            {
                return config.memCFG[ConfigAddrForMapperInfo(map)].ToString();
            }
            else if (map.ConfigType == ipcDefines.mAdrRelay)
            {
                return config.memCFG[ConfigAddrForMapperInfo(map)].ToString();
            }
            else
            {
                return config.memCFG[map.ConfigType].ToString();    //direct value
            }         
        }

        private void LoadConfigToProperty(DeviceProperty property)
        {
            int devAddr = IndexForHostDevice();
            foreach (var param in property.ResultParameters)
            {
                param.Value = sconnConfigToStringVal(param.sconnMappers.First(), site.siteCfg.deviceConfigs[devAddr]);
            }
        }


        private void LoadConfigToAction(DeviceAction action)
        {
            int devAddr = IndexForHostDevice();
            foreach (var param in action.ResultParameters)
            {
                param.Value = sconnConfigToStringVal(param.sconnMappers.First(), site.siteCfg.deviceConfigs[devAddr]);
            }
        }

        private string ParameterNameForMapper(sconnConfigMapper maper)
        {
            if (maper.ConfigType == ipcDefines.mAdrInput)
            {
                return "Input";
            }
            else if (maper.ConfigType == ipcDefines.mAdrOutput)
            {
                return "Output";
            }
            else if (maper.ConfigType == ipcDefines.mAdrRelay)
            {
                return "Relay";
            }
            else
            {
                return "Unknown";
            }
        }

        private ParameterType ParamTypeForSconnMapper(sconnConfigMapper mapper)
        {
            var task = connector.TypeForName(ParameterNameForMapper(mapper)); //ParameterType.TypeForName(ParameterNameForMapper(mapper), cont);
            return task;
        }

        private void AddPropertyForMapperAndDevice(sconnConfigMapper maper,  Device edited, int DevNo)
        {
            try
            {
                iotRepository<DeviceProperty> proprepo = new iotRepository<DeviceProperty>();
                DeviceProperty prop = new DeviceProperty();
                prop.PropertyName = "Input" + maper.SeqNumber;    //TODO read from name cfg  
                prop.Device = edited;
                prop.LastUpdateTime = DateTime.Now;
                proprepo.Add(prop);
                List<DeviceProperty> storeprops = proprepo.GetAll().ToList();
                DeviceProperty storedProp = (from s in storeprops
                                             where s.PropertyName == prop.PropertyName
                                             select s).First();
                
                //create parameter and bind mapper to it
                iotRepository<DeviceParameter> paramrepo = new iotRepository<DeviceParameter>();
                DeviceParameter param = new DeviceParameter();
                param.Value = sconnConfigToStringVal(maper, site.siteCfg.deviceConfigs[DevNo]);
                param.Type = ParamTypeForSconnMapper(maper);
                param.Property = storedProp;
                paramrepo.Add(param);
                List<DeviceParameter> storeparams = paramrepo.GetAll().ToList();
                DeviceParameter storedParam = (from p in storeparams
                                         where p.Property == param.Property
                                         select p).First();

                maper.Parameter = storedParam;
                iotRepository<sconnConfigMapper> mapperRepo = new iotRepository<sconnConfigMapper>();
                mapperRepo.Add(maper);
            }
            catch (Exception e)
            {
                
            }


        }

        private void AddActionForMapperAndDevice(sconnConfigMapper maper,  Device edited, int DevNo)
        {
            try
            {
                DeviceAction action = new DeviceAction();
                action.ActionName = "Output" + maper.SeqNumber;    //TODO read from name cfg  
                action.Device = edited;
                action.LastActivationTime = DateTime.Now;
                var qry = connector.ActionAdd(action);


                //copy maper for action
                sconnConfigMapper actionMaper = new sconnConfigMapper();
                actionMaper.ConfigType = maper.ConfigType;
                actionMaper.SeqNumber = maper.SeqNumber;

                ParameterType paramtype = ParamTypeForSconnMapper(actionMaper);

                ActionParameter inparam = new ActionParameter();
                inparam.Value = sconnConfigToStringVal(actionMaper, site.siteCfg.deviceConfigs[DevNo]);
                inparam.Type = paramtype;
                inparam.Action = action;
                actionMaper.ActionParam = inparam;
                qry = connector.ActionParamAdd(inparam);


                //create parameter and bind mapper to it
                DeviceParameter param = new DeviceParameter();
                param.Value = sconnConfigToStringVal(maper, site.siteCfg.deviceConfigs[DevNo]);
                param.Type = paramtype;
                param.Action = action;
                qry = connector.ParameterAdd(param);


                maper.Parameter = param;
                qry = connector.MapperAdd(maper);

            }
            catch (Exception e)
            {
                
   
            }

        }


        private bool LoadConfigToDevice(Device dev)
        {
            try
            {

                //iotConnector connt = new iotConnector();
                //Device edited = connt.DeviceList().Where(n => n.DeviceId == dev.DeviceId).First();
                iotRepository<Device> devrep = new iotRepository<Device>();
                Device edited = devrep.GetById(dev.DeviceId);

                int devAddr = IndexForHostDevice();
                //Reload properties and actions if they changed
                //Update 

                int inputs = site.siteCfg.deviceConfigs[devAddr].memCFG[ipcDefines.mAdrInputsNO];
                if (edited.Properties.Count != inputs)
                {
                    //clear current properies


                    //load
                    for (int i = 0; i < inputs; i++)
                    {
                        sconnConfigMapper maper = new sconnConfigMapper();
                        maper.ConfigType = ipcDefines.mAdrInput;
                        maper.SeqNumber = i;
                        AddPropertyForMapperAndDevice(maper,   edited, devAddr);
                    }
                }
                else //update
                {
                    foreach (var item in edited.Properties)
                    {
                        //get parameter
                        // List<DeviceParameter> propparams = (from par in item.ResultParameters
                        //                         select par).ToList();
                        DeviceParameter param = item.ResultParameters.ElementAt(0);
                        param.Value = sconnConfigToStringVal(param.sconnMappers.ElementAt(0), site.siteCfg.deviceConfigs[devAddr]);
                        
                        //iotRepository<DeviceParameter> repo = new iotRepository<DeviceParameter>();
                        //repo.Update(param);
                        //cont.SaveChanges();

                        if (param != null)
                        {
                            //get input mapper
                            sconnConfigMapper maper = (from cm in param.sconnMappers
                                                       select cm).FirstOrDefault();
                            if (maper != null)
                            {
                                param.Value = sconnConfigToStringVal(maper, site.siteCfg.deviceConfigs[devAddr]);
                               // cont.SaveChanges();
                            }
                        }
                    }
                }

                int outputs = site.siteCfg.deviceConfigs[devAddr].memCFG[ipcDefines.mAdrOutputsNO];
                int relays = site.siteCfg.deviceConfigs[devAddr].memCFG[ipcDefines.mAdrRelayNO];
                if (edited.Actions.Count != outputs + relays)
                {
                    //remove existing
                    if (edited.Actions.Count > 0)
                    {
                        iotRepository<DeviceAction> actrep = new iotRepository<DeviceAction>();
                        List<DeviceAction> acts = actrep.GetAll().ToList();
                        for (int i = 0; i < acts.Count; i++)
                        {
                            actrep.Delete(acts.ElementAt(i));
                        }
                    }

                    for (int i = 0; i < outputs; i++)
                    {
                        sconnConfigMapper maper = new sconnConfigMapper();
                        maper.ConfigType = ipcDefines.mAdrOutput;
                        maper.SeqNumber = i;
                        AddActionForMapperAndDevice(maper,   edited, devAddr);
                    }

                    for (int i = 0; i < relays; i++)
                    {
                        sconnConfigMapper maper = new sconnConfigMapper();
                        maper.ConfigType = ipcDefines.mAdrRelay;
                        maper.SeqNumber = i;
                        AddActionForMapperAndDevice(maper,  edited, devAddr);
                    } 
                }
                else{
                    foreach (var item in edited.Actions)
                    {
                        //get action
                        DeviceParameter param = (from par in item.ResultParameters
                                                 select par).FirstOrDefault();
                        if (param != null)
                        {
                            //get input mapper
                            sconnConfigMapper maper = (from cm in param.sconnMappers
                                                       select cm).FirstOrDefault();
                            if (maper != null)
                            {
                                param.Value = sconnConfigToStringVal(maper, site.siteCfg.deviceConfigs[devAddr]);
                                param.Type = param.Type;
                               // cont.SaveChanges();
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                return false; 
            }

            return true;
        }

        private void AddPropertyWithConfigAddr(int addr)
        {

        }


        /******************* Public ************************/

        /*
        public bool PerformAction(DeviceAction action)
        {
            DeviceActionToConfig(action);
            return WriteSiteConfig();
        }
        */

        public bool PerformAction(DeviceAction action)
        {
            Task t = Task.Factory.StartNew(() =>
            {
                LoadDevice(action.Device);
                UpdateSite();
                DeviceActionToConfig(action);
                WriteSiteConfig();
                LoadDeviceActions(protocolDevice);     //load site config after change
            });  
            return true;
        }

        public bool PerformActions(List<DeviceAction> actions)
        {
            foreach (var action in actions)
            {
                DeviceActionToConfig(action);
            }
            return WriteSiteConfig();   
        }

        public bool ReadProperty(DeviceProperty property)
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

        public bool ReadProperties(List<DeviceProperty> properties)
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

      

 

    }


}