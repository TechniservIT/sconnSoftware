using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.Annotations;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config
{

    public enum DeviceCategory
    {
        GenericInputOutput = 1,
        AlarmSystem,
        IpCamera,
        Vehicle,
        Thermostat,
        Sensor,
        CommunicationModule,
        SpectrumAnalyser,
        Oscilloscope,
        Speaker,
        Servo,
        ThreePhaseMotor,
        Actuator,
        GpsTracked
    }


    public enum DeviceIoCategory
    {
        CmosInputs = 1,
        CmosOutput,
        Temperature,
        Humidity,
        PowerOutput,
        AnalogInput,
        Relay
    }
    


    public class sconnRelay : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration, INotifyPropertyChanged
    {
        public byte Id { get; set; }
        public sconnOutputType Type { get; set; }
        public bool Value { get; set; }
        public byte NameId { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public DeviceIoCategory IoCategory { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();


        public string imageIconUri { get; set; }
        public string imageRealUri { get; set; }


        public string GetRelayTypeImageUriForRelay(sconnRelay input)
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
            imageIconUri = GetRelayTypeImageUriForRelay(this);
        }


        public void CopyFrom(sconnRelay other)
        {
            this.Type = other.Type;
            this.Value = other.Value;
            this.Enabled = other.Enabled;
            this.Name = other.Name;
            this.IoCategory = other.IoCategory;
            this.NameId = other.NameId;
            this.UUID = other.UUID;
            this.imageIconUri = other.imageIconUri;

            OnPropertyChanged();
        }


        public string UUID { get; set; }

        public sconnRelay()
        {
            Name = "Relay";
            UUID = Guid.NewGuid().ToString();
            IoCategory = DeviceIoCategory.Relay; 
        }

        public sconnRelay(byte[] rawCfg) : this()
        {
                this.Deserialize(rawCfg);
        }

        public byte[] Serialize()
        {
            try
            {
                byte[] buffer = new byte[ipcDefines.RelayMemSize];
                buffer[ipcDefines.mAdrRelayType] = (byte)Type;
                buffer[ipcDefines.mAdrRelayEnabled] = (byte)(Enabled ? 1 : 0);
                buffer[ipcDefines.mAdrRelayVal] = (byte)(Value ? 1 : 0);
                buffer[ipcDefines.mAdrRelayNameAddr] = (byte)NameId;
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
                Type = (sconnOutputType)buffer[ipcDefines.mAdrRelayType];
                Value = buffer[ipcDefines.mAdrRelayVal] > 0 ? true : false;
                NameId = buffer[ipcDefines.mAdrRelayNameAddr];
                Enabled = buffer[ipcDefines.mAdrRelayEnabled] > 0 ? true : false;
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

        public bool Equals(sconnRelay other)
        {
            return null != other && UUID == other.UUID;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as sconnRelay);
        }
        public override int GetHashCode()
        {
            return Id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
