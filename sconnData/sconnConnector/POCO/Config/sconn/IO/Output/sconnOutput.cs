﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.Abstract.IO;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config
{
    public enum sconnOutputType
    {
        AlarmNormallyActive = 0,
        AlarmNormallyInActive,
        Power,
        SignalNormallyActive,
        SignalNormallyInactive,
        OnFailure,
        OnViolation,
        OnGsmTransmission,
        OnConfigUpload
    }
    
    public class sconnOutput : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
    {
        private byte _Id;
        public byte Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                Name = "Wyjście " + _Id.ToString();
            }
        }
        public sconnOutputType Type { get; set; }
        public bool Value { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public byte NameId { get; set; }
        public DeviceIoCategory IoCategory { get; set; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();


        public string imageIconUri { get; set; }
        public string imageRealUri { get; set; }


        public string GetOutputTypeImageUriForOutput(sconnOutput input)
        {
            if (input.Type == sconnOutputType.AlarmNormallyActive)
            {
                return "pack://application:,,,/images/elektro1000.jpg";
            }
            else if (input.Type == sconnOutputType.AlarmNormallyInActive)
            {
                return "pack://application:,,,/images/elektro1000.jpg";
            }
            else if (input.Type == sconnOutputType.OnConfigUpload)
            {
                return "pack://application:,,,/images/elektro1000.jpg";
            }
            else if (input.Type == sconnOutputType.OnFailure)
            {
                return "pack://application:,,,/images/elektro1000.jpg";
            }
            else if (input.Type == sconnOutputType.Power)
            {
                return "pack://application:,,,/images/elektro1000.jpg";
            }
            else if (input.Type == sconnOutputType.OnGsmTransmission)
            {
                return "pack://application:,,,/images/elektro1000.jpg";
            }
            else if (input.Type == sconnOutputType.OnViolation)
            {
                return "pack://application:,,,/images/elektro1000.jpg";
            }
            else if (input.Type == sconnOutputType.SignalNormallyActive)
            {
                return "pack://application:,,,/images/elektro1000.jpg";
            }
            else if (input.Type == sconnOutputType.SignalNormallyInactive)
            {
                return "pack://application:,,,/images/elektro1000.jpg";
            }
            return null;
        }

        private void LoadImageTypeUrl()
        {
            imageIconUri = GetOutputTypeImageUriForOutput(this);
        }

        public sconnOutput() 
        {
            Name = "Output";
            LoadImageTypeUrl();
            IoCategory = DeviceIoCategory.PowerOutput;  //TODO - detect
        }
         
        public sconnOutput(byte[] rawCfg) : this()
        {
            this.Deserialize(rawCfg);
        }


        public byte[] Serialize()
        {
            try
            {
                byte[] buffer = new byte[ipcDefines.mAdrOutputMemSize];
                buffer[ipcDefines.mAdrOutputType] = (byte)Type;
                buffer[ipcDefines.mAdrOutputEnabled] = (byte)(Enabled ? 1 : 0);
                buffer[ipcDefines.mAdrOutputVal] = (byte)(Value ? 1 : 0);
                buffer[ipcDefines.mAdrOutputNameAddr] = (byte)NameId;
                return buffer;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }

        }

        public void Deserialize(byte[] buffer)
        {
            try
            {
                Type = (sconnOutputType)buffer[ipcDefines.mAdrOutputType];
                Value = (buffer[ipcDefines.mAdrOutputVal] > 0 ? true : false);
                NameId = buffer[ipcDefines.mAdrOutputNameAddr];
                Enabled = buffer[ipcDefines.mAdrOutputEnabled] > 0 ? true : false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        public void Fake()
        {
            try
            {
                this.Id = 0;
                this.Enabled = true;
                this.Name = Guid.NewGuid().ToString();
                this.NameId = 0;
                this.Type = sconnOutputType.AlarmNormallyActive;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }

    }

}
