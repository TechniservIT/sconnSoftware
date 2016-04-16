﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using iotDbConnector.DAL;
using sconnConnector;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;
using System.Linq;

namespace iotDash.Areas.AlarmSystem.Models
{

    public class AlarmSystemListModel
    {
        public int DeviceNo { get; set; }

        public List<Device> AlarmDevices { get; set; }

        public AlarmSystemListModel(List<Device> devices)
        {
            AlarmDevices = devices;
        }
    }



    public class AlarmSystemSummaryModel
    {
        public int DeviceNo { get; set; }

        public Device AlarmDevice { get; set; }

        public AlarmSystemSummaryModel(int devno, Device  dev)
        {
            DeviceNo = devno;
            AlarmDevice = dev;
        }
    }


    public class AlarmSystemInputEditModel
    {

        [Required]
        public int DeviceId { get; set; }

        public  ipcDeviceConfig DevCfg { get; set; }

        [Required]
        public List<sconnInput> Inputs { get; set; }

    }


    public class AlarmSystemDeviceModel
    {

        [Required]
        public int ServerId { get; set; }
        public sconnDevice Device { get; set; }
        public string Result { get; set; }

        public AlarmSystemDeviceModel(sconnDevice device)
        {
            this.Device = device;
        }

        public AlarmSystemDeviceModel()
        {
                
        }
      
    }


    public class AlarmSystemDeviceListModel
    {
        [Required]
        public int ServerId { get; set; }
        public List<sconnDevice> Devices { get; set; }
        public string Result { get; set; }

        public AlarmSystemDeviceListModel(List<sconnDevice> devices)
        {
            this.Devices = devices;
        }


    }


    public class AlarmSystemDetailModel
    {

        [Required]
        public int ServerId { get; set; }
       
        public List<sconnDevice> Devices { get; set; }

        public sconnDevice Device { get; set; }

        public sconnSite Config { get; set; }

        public AlarmSystemDetailModel(List<sconnDevice> devices, sconnSite site ) : this(devices)
        {
            Config = site;
        }

        public AlarmSystemDetailModel(sconnDevice device)
        {
            this.Device = device;
        }

        public AlarmSystemDetailModel(List<sconnDevice> devices)
        {
            if(devices != null)
            {
                Devices = devices;
                Device = Devices.FirstOrDefault();
            }
          
        }

    }


    public class AlarmSystemMapEditModel
    {
        [Required]
        public int ServerId { get; set; }

        public sconnGlobalConfig Config { get; set; }
        public List<sconnDevice> Devices { get; set; }
        public DeviceMapDefinition MapDefinition { get; set; }

        public AlarmSystemMapEditModel()
        {
            Config = new sconnGlobalConfig();
            Devices = new List<sconnDevice>();
            MapDefinition = new DeviceMapDefinition();
        }

        public AlarmSystemMapEditModel(List<sconnDevice> devices) : this()
        {
            this.Devices = devices;
        }

        public AlarmSystemMapEditModel(sconnGlobalConfig config, List<sconnDevice> devices) :this()
        {
            this.Config = config;
            this.Devices = devices;
        }


    }

    public class AlarmSystemGlobalModel
    {

        [Required]
        public int ServerId { get; set; }

        public sconnGlobalConfig Config { get; set; }

        public AlarmSystemGlobalModel()
        {
                
        }

        public AlarmSystemGlobalModel(sconnGlobalConfig Config)
        {
            this.Config = Config;
        }

    }


    public class AlarmSystemGlobalEditModel
    {

        [Required]
        public int ServerId { get; set; }

        public sconnGlobalConfig Config { get; set; }

        public AlarmSystemGlobalEditModel()
        {
            Config = new sconnGlobalConfig();
        }

        public AlarmSystemGlobalEditModel(sconnGlobalConfig gcfg)
        {
            Config = gcfg;
        }
        

    }


    public class AlarmSystemCommunicationEditModel
    {

        [Required]
        public int ServerId { get; set; }

        public sconnGlobalConfig Config { get; set; }

        public AlarmSystemCommunicationEditModel()
        {
            Config = new sconnGlobalConfig();
        }

        public AlarmSystemCommunicationEditModel(sconnGlobalConfig config) : this()
        {
            this.Config = config;
        }

    }


}
