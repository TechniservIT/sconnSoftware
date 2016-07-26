using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.Annotations;
using sconnConnector.POCO.Config.Abstract.IO;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.POCO.Config
{

    public enum sconnInputType
    {
        NormallyOpen = 0,
        NormallyClosed,
        DoubleParametrizedNC,
        SingleParametrizedNC,
        DoubleParametrizedNO,
        ParametrizedSensor    
    }

    public enum sconnInputTypeMaskValues
    {
        InputDelayedMask = 128
    }

    public enum sconnActivationGroup
    {
        Arm = 0,
        Disarm,
        ArmedViolation,
        DisarmedViolation,
        ArmedAndDisarmedViolation
    }



    public class sconnInput : IAlarmSystemConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration, INotifyPropertyChanged, IAlarmSystemZonedIo
    {
        public byte NameId { get; set; }

        private byte _Id;
        public byte Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                Name = "Wejście " + _Id.ToString();
            }
        }

        public string UUID { get; set; }
        public sconnInputType Type { get; set; }
        public byte Value { get; set; }
        public uint Sensitivity { get; set; }
        public int ZoneId { get; set; }
        public bool Enabled { get; set; }
        public bool Delayed { get; set; }
        public string Name { get; set; }
        public sconnActivationGroup ActivationGroup { get; set; }
        public DeviceIoCategory IoCategory { get; set; }
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public string imageIconUri { get; set; }
        public string imageRealUri { get; set; }

        public sconnInput()
        {
            Name = "Input";
            UUID = Guid.NewGuid().ToString();
            LoadImageTypeUrl();
            IoCategory = DeviceIoCategory.CmosInputs;  //TODO - detect
        }

        public void CopyFrom(sconnInput other)
        {
            this.Type = other.Type;
            this.Value = other.Value;
            this.Sensitivity = other.Sensitivity;
            this.Enabled = other.Enabled;
            this.Name = other.Name;
            this.ActivationGroup = other.ActivationGroup;
            this.IoCategory = other.IoCategory;
            this.NameId = other.NameId;
            this.UUID = other.UUID;
            this.imageIconUri = other.imageIconUri;

            OnPropertyChanged();
        }

        public string GetInputTypeImageUriForInput(sconnInput input)
        {
            if (input.Type == sconnInputType.DoubleParametrizedNC )
            {
                return "pack://application:,,,/images/eye1000.jpg";
            }
            else if (input.Type == sconnInputType.DoubleParametrizedNO)
            {
                return "pack://application:,,,/images/eye1000.jpg";
            }
            else if (input.Type == sconnInputType.NormallyClosed)
            {
                return "pack://application:,,,/images/eye1000.jpg";
            }
            else if (input.Type == sconnInputType.NormallyOpen)
            {
                return "pack://application:,,,/images/eye1000.jpg";
            }
            else if (input.Type == sconnInputType.SingleParametrizedNC)
            {
                return "pack://application:,,,/images/eye1000.jpg";
            }

            return null;
        }

        private void LoadImageTypeUrl()
        {
            imageIconUri = GetInputTypeImageUriForInput(this);
        }



        public sconnInput(byte[] rawBytes) : this()
        {
                this.Deserialize(rawBytes);
        }

        public byte[] Serialize()
        {
            try
            {
                byte[] buffer = new byte[ipcDefines.mAdrInputMemSize];
                if (Delayed)
                {

                    buffer[ipcDefines.mAdrInputType] = (byte)((byte)Type | (byte)sconnInputTypeMaskValues.InputDelayedMask);
                }
                else
                {
                    buffer[ipcDefines.mAdrInputType] = (byte)Type;
                }
              
                buffer[ipcDefines.mAdrInputEnabled] = (byte)(Enabled ? 1 : 0);
                buffer[ipcDefines.mAdrInputVal] = (byte)Value;
                buffer[ipcDefines.mAdrInputNameAddr] = (byte)NameId;
                buffer[ipcDefines.mAdrInputSensitivity] = (byte)(Sensitivity / ipcDefines.InputSensitivityStep);
                buffer[ipcDefines.mAdrInputAG] = (byte)ActivationGroup;
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
                byte typeVal = buffer[ipcDefines.mAdrInputType];
                if ((typeVal & (byte)sconnInputTypeMaskValues.InputDelayedMask) > 0)
                {
                    Delayed = true;
                    typeVal = (byte) (typeVal - (byte)sconnInputTypeMaskValues.InputDelayedMask);
                }

                if (Enum.IsDefined(typeof(sconnInputType), (int)typeVal))
                {
                    Type = (sconnInputType)typeVal;
                }

                Value = buffer[ipcDefines.mAdrInputVal];
                NameId = buffer[ipcDefines.mAdrInputNameAddr];
                Enabled = buffer[ipcDefines.mAdrInputEnabled] > 0 ? true : false;
                Sensitivity = (uint) (buffer[ipcDefines.mAdrInputSensitivity] * ipcDefines.InputSensitivityStep);

                if (Enum.IsDefined(typeof(sconnActivationGroup), (int)buffer[ipcDefines.mAdrInputAG]))
                {
                    ActivationGroup = (sconnActivationGroup)buffer[ipcDefines.mAdrInputAG];
                }
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
                this.Value = 1;
                this.Type = sconnInputType.NormallyClosed;
                this.ActivationGroup = sconnActivationGroup.ArmedViolation;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        public bool Equals(sconnInput other)
        {
            return null != other && UUID == other.UUID;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as sconnInput);
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
