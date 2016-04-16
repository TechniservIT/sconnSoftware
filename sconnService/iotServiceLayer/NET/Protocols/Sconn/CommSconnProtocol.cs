﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using sconnConnector;
using System.Threading.Tasks;
using iotDbConnector;
using iotDbConnector.DAL;
using NLog;
using System.Diagnostics;
using sconnConnector.POCO.Config;


namespace iotServiceProvider
{
    public class CommSconnProtocol :ICommProtocol
    {
        private Device protocolDevice;

        private Logger nlogger = LogManager.GetCurrentClassLogger();

        private sconnSite site { get; set; }

        private sconnCfgMngr cfgMngr { get; set; }

        int devices = 0;

        public CommSconnProtocol()
        {
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
           // Task t = Task.Factory.StartNew(() =>
           // {
            LoadConfigToDevice(device.Id);
           // });        
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
                nlogger.ErrorException(e.Message, e);
            }
            return false;
        }

        public bool LoadDeviceProperties(Device device)
        {
            try
            {
                LoadDevice(device);
                if (UpdateSite())
                {
                    return LoadConfigToDevice(device.Id);
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }

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
            try
            {
                int currDevs = cfgMngr.getSiteDeviceNo(site);
                //TODO - cache site configs  for hash min rx
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

                return cfgMngr.ReadSiteRunningConfigIO(site);
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
              
            }

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
            try
            {
                ushort val;
                val = buffer[addr + 1];
                val |= (ushort)((buffer[addr]) << 8);
                return val;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return 0;
            }

        }

        private int IndexForHostDevice()
        {
            try
            {
                for (int i = 0; i < site.siteCfg.deviceNo; i++)
                {
                    if (WordFromBufferAtAddr(site.siteCfg.deviceConfigs[i].memCFG, ipcDefines.mAdrDevID) == 0)
                    {
                        return i;
                    }
                }
                return -1;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return -1;
            }

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
            try
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
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return 0;
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
                nlogger.ErrorException(e.Message, e);
            }
        }

        private void SetConfigForActionParameterAtDevice(ActionParameter param,  ipcDeviceConfig config)
        {
            LoadStringValueToByteMemoryAtAddr(param.Value, config.memCFG, ConfigAddrForMapperInfo(param.sconnMappers.First()));
        }

        private void DeviceActionToConfig(DeviceAction action)
        {
            try
            {
                int devAddr = 0; //IndexForHostDevice();
                if (site.siteCfg.deviceNo == 0)
                {
                    return;
                }
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
                nlogger.ErrorException(e.Message, e);
            }
        }

        private string sconnConfigToStringVal(sconnConfigMapper map,  ipcDeviceConfig config)
        {
            try
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
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return "";
            
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
            try
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
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return "";
            }

        }

        private ParameterType ParamTypeForSconnMapper(sconnConfigMapper mapper)
        {
            try
            {
                iotConnector connector = new iotConnector();
                return connector.TypeForName(ParameterNameForMapper(mapper)); //ParameterType.TypeForName(ParameterNameForMapper(mapper), cont);
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new ParameterType();
            }

        }

        private void AddPropertyForMapperAndDevice(sconnPropertyMapper maper,  Device edited, int DevNo)
        {
            try
            {
                iotContext cont = new iotContext();
                DeviceProperty prop = new DeviceProperty();
                prop.PropertyName = "Input" + maper.SeqNumber;    //TODO read from name cfg
  
                Device storedDevice = cont.Devices.Where( d =>  d.Id == edited.Id).First();     //devRepo.GetById(edited.Id);
                prop.Device = storedDevice;
                prop.LastUpdateTime = DateTime.Now;
                cont.Properties.Add(prop);
                cont.SaveChanges();
                

                DeviceParameter param = new DeviceParameter();
                param.Value = sconnConfigToStringVal(maper, site.siteCfg.deviceConfigs[DevNo]);
                ParameterType extType = ParamTypeForSconnMapper(maper);
                ParameterType inType = cont.ParamTypes.Where(p => p.Id == extType.Id).First();
                param.Type = inType;
                param.Property = prop;
                cont.Parameters.Add(param);
                cont.SaveChanges();

                maper.Parameter = param;
                cont.PropertyResultMappers.Add(maper);
                cont.SaveChanges();

            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
            }


        }

        private void AddActionForMapperAndDevice(sconnActionResultMapper maper, Device edited, int DevNo)
        {
            try
            {
                iotContext cont = new iotContext();
                Device storedDevice = cont.Devices.Where(d => d.Id == edited.Id).First(); 
                DeviceAction action = new DeviceAction();
                action.RequiredParameters = new List<ActionParameter>();
                action.ResultParameters = new List<DeviceActionResult>();
                action.ActionName = "Output" + maper.SeqNumber;    //TODO read from name cfg  
                action.Device = storedDevice;
                action.LastActivationTime = DateTime.Now;

                cont.Actions.Add(action);
                cont.SaveChanges();

                //copy maper for action
                sconnActionResultMapper actionResultMaper = new sconnActionResultMapper();
                actionResultMaper.ConfigType = maper.ConfigType;
                actionResultMaper.SeqNumber = maper.SeqNumber;

                sconnActionMapper actionInputMaper = new sconnActionMapper();
                actionInputMaper.ConfigType = maper.ConfigType;
                actionInputMaper.SeqNumber = maper.SeqNumber;


                ParameterType extType = ParamTypeForSconnMapper(maper);
                ParameterType inType = cont.ParamTypes.Where(p => p.Id == extType.Id).First();
                
                ActionParameter inparam = new ActionParameter();
                inparam.Value = sconnConfigToStringVal(actionInputMaper, site.siteCfg.deviceConfigs[DevNo]);
                inparam.Type = inType;
                inparam.Action = action;
                cont.ActionParameters.Add(inparam);
                actionInputMaper.ActionParam = inparam;
                cont.ActionParamMappers.Add(actionInputMaper);
                cont.SaveChanges();

                //create result parameter and bind mapper to it
                DeviceActionResult param = new DeviceActionResult();
                param.Value = sconnConfigToStringVal(maper, site.siteCfg.deviceConfigs[DevNo]);
                param.Type = inType;
                param.Action = action;
                cont.ActionResultParameters.Add(param);
                cont.SaveChanges();
                actionResultMaper.ActionParam = param;

                cont.ActionResultMappers.Add(actionResultMaper);
                cont.SaveChanges();

            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
            }

        }


        private bool LoadConfigToDevice(int devId)
        {
            try
            {
                //iotConnector connt = new iotConnector();
                //Device edited = connt.DeviceList().Where(n => n.Id == dev.Id).First();
                //iotRepository<Device> devrep = new iotRepository<Device>();
                Stopwatch watch = new Stopwatch();


                watch.Start();
                iotContext cont = new iotContext();
                //cont.Configuration.LazyLoadingEnabled = false;
                //Device edited = cont.Devices.Where(d => d.Id == dev.Id)
                //    .Include(d => d.Actions.Select(a => a.ResultParameters.Select(r => r.sconnMappers)))
                //    .Include(d => d.Properties.Select(p => p.ResultParameters.Select(r => r.sconnMappers)))
                //    .First();  
                Device edited = cont.Devices.First(d => d.Id == devId);

                
                watch.Stop();
                Debug.WriteLine("Execution time : " + watch.ElapsedMilliseconds + " ms @ device query");
                watch.Reset();

                int devAddr = 0;
                if ( site.siteCfg.deviceNo == 0){
                    return false;
                }
         
                //Reload properties and actions if they changed
                //Update 

                int inputs = site.siteCfg.deviceConfigs[devAddr].memCFG[ipcDefines.mAdrInputsNO];
                if (edited.Properties.Count != inputs)
                {
                    watch.Start();

                    //clear current properies
                    for (int i = 0; i < edited.Properties.Count; i++)
                    {
                        cont.Properties.Remove(edited.Properties[i]);
                    }
                    cont.SaveChanges();

                    //load
                    for (int i = 0; i < inputs; i++)
                    {
                        sconnPropertyMapper maper = new sconnPropertyMapper();
                        maper.ConfigType = ipcDefines.mAdrInput;
                        maper.SeqNumber = i;
                        AddPropertyForMapperAndDevice(maper,   edited, devAddr);
                    }

                    watch.Stop();
                    Debug.WriteLine("Execution time : " + watch.ElapsedMilliseconds + " ms @ readd prop");
                    watch.Reset();
                }
                else //update
                {
                    watch.Reset();
                    watch.Start();

                    foreach (var item in edited.Properties)
                    {
                        //get parameter
                        Stopwatch watch4 = new Stopwatch();
                        watch4.Start();
                        DeviceParameter param = item.ResultParameters.FirstOrDefault();

                        watch4.Stop();
                        Debug.WriteLine("Execution time : " + watch4.ElapsedMilliseconds + " ms @ prop update dev param query");
                        
                        watch4.Restart();
                        param.Value = sconnConfigToStringVal(param.sconnMappers.FirstOrDefault(), site.siteCfg.deviceConfigs[devAddr]);
                        watch4.Stop();
                        Debug.WriteLine("Execution time : " + watch4.ElapsedMilliseconds + " ms @ prop update dev param parse");
                        watch4.Reset();

                        if (param != null)
                        {
                            //get input mapper
                            Stopwatch watch2 = new Stopwatch();
                            watch2.Start();
                            sconnConfigMapper maper = param.sconnMappers.FirstOrDefault();
                            watch2.Stop();
                            Debug.WriteLine("Execution time : " + watch2.ElapsedMilliseconds + " ms @ prop update mapper query");
                            watch2.Reset();

                            Stopwatch watch3 = new Stopwatch();
                            watch3.Start();
                            if (maper != null)
                            {
                                param.Value = sconnConfigToStringVal(maper, site.siteCfg.deviceConfigs[devAddr]);
                            }
                            watch3.Stop();
                            Debug.WriteLine("Execution time : " + watch3.ElapsedMilliseconds + " ms @ prop update cfg to str");
                            watch3.Reset();

                           
                        }
                    }

                    Stopwatch watch1 = new Stopwatch();
                    watch1.Start();
                    cont.SaveChanges(); //apply
                    watch1.Stop();
                    Debug.WriteLine("Execution time : " + watch1.ElapsedMilliseconds + " ms @ prop update save");
                    watch1.Reset();
   
                    watch.Stop();
                    Debug.WriteLine("Execution time : " + watch.ElapsedMilliseconds + " ms @ prop update");
                    watch.Reset();

                }

                int outputs = site.siteCfg.deviceConfigs[devAddr].memCFG[ipcDefines.mAdrOutputsNO];
                int relays = site.siteCfg.deviceConfigs[devAddr].memCFG[ipcDefines.mAdrRelayNO];
                if (edited.Actions.Count != outputs + relays)
                {
                    watch.Start();

                    //remove existing
                    if (edited.Actions.Count > 0)
                    {
                        for (int i = 0; i < edited.Actions.Count; i++)
                        {
                            cont.Actions.Remove(edited.Actions[i]);
                        }
                        cont.SaveChanges();
                    }

                    for (int i = 0; i < outputs; i++)
                    {
                        sconnActionResultMapper maper = new sconnActionResultMapper();
                        maper.ConfigType = ipcDefines.mAdrOutput;
                        maper.SeqNumber = i;
                        AddActionForMapperAndDevice(maper,   edited, devAddr);
                    }

                    for (int i = 0; i < relays; i++)
                    {
                        sconnActionResultMapper maper = new sconnActionResultMapper();
                        maper.ConfigType = ipcDefines.mAdrRelay;
                        maper.SeqNumber = i;
                        AddActionForMapperAndDevice(maper,  edited, devAddr);
                    }

                    watch.Stop();
                    Debug.WriteLine("Execution time : " + watch.ElapsedMilliseconds + " ms @ action readd");
                    watch.Reset();
                }
                else
                {
                    watch.Start();

                    foreach (var item in edited.Actions)
                    {
                        //get action
                        DeviceActionResult param = (from par in item.ResultParameters
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
                            }
                        }
                    }
                    cont.SaveChanges(); //save updates

                    watch.Stop();
                    Debug.WriteLine("Execution time : " + watch.ElapsedMilliseconds + " ms @ action update");
                    watch.Reset();
                }

            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false; 
            }

            return true;
        }

        private void AddPropertyWithConfigAddr(int addr)
        {

        }


        /******************* Public ************************/


        public bool PerformAction(DeviceAction action)
        {
            try
            {
                Stopwatch watch = new Stopwatch();

                LoadDevice(action.Device);
 
                watch.Start();
                UpdateSite();
                watch.Stop();
                Debug.WriteLine("Execution time : " + watch.ElapsedMilliseconds + " ms");
                watch.Reset();

                DeviceActionToConfig(action);


                watch.Start();
                WriteSiteConfig();
                watch.Stop();
                Debug.WriteLine("Execution time : " + watch.ElapsedMilliseconds + " ms");
                watch.Reset();


                watch.Start();
                LoadDeviceActions(action.Device);     //load site config after change
                watch.Stop();
                Debug.WriteLine("Execution time : " + watch.ElapsedMilliseconds + " ms");
                watch.Reset();

                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }

        public Task PerformActionAsync(DeviceAction action)
        {
            try   
            {
                Task t = Task.Factory.StartNew(() =>
                {
                    LoadDevice(action.Device);
                    UpdateSite();
                    DeviceActionToConfig(action);
                    WriteSiteConfig();
                    LoadDeviceActions(action.Device);     //load site config after change
                });
                return t;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return null;
            }
        }

        public bool PerformActions(List<DeviceAction> actions)
        {
            return false; 
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