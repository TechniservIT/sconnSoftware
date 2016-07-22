using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.Annotations;
using sconnConnector.POCO.Config.Abstract.Device;
using sconnConnector.POCO.Config.Abstract.IO;

namespace sconnConnector.POCO.Config.sconn
{


    public interface IAlarmSystemConfigurationEntity : ISerializableConfiguration
    {
        string UUID { get; set; }
    }

    public interface IAlarmSystemNamedEntityConfig : IAlarmSystemEntityConfig
    {
        byte[] SerializeEntityNames(int id);
        void DeserializeEntityNames(int id, byte[] buffer); 
    }

    public interface IAlarmSystemEntityConfig 
    {
        int GetEntityCount();
        void Clear();
        byte[] SerializeEntityWithId(int id);
        void DeserializeEntityWithId(byte[] buffer);
    }

    public interface IAlarmSystemNamedEntity : IAlarmSystemConfigurationEntity
    {
        byte[] SerializeEntityNames();
        void DeserializeEntityNames(byte[] buffer);
    }

    public enum sconnDeviceType
    {
        Graphical_Keypad = 1,
        Motherboard = 2,
        Gsm_Module = 3,
        Pir_Sensor =4,
        Relay_Module = 5,
        InputsModule = 6,
        Siren = 7,
        PirSonic_Sensor = 8,
        KeypadMotherboard = 9
    }



    public class sconnDevice :  IAlarmSystemNamedEntity, ISerializableConfiguration, IFakeAbleConfiguration, INotifyPropertyChanged
    {
        public byte Id { get; set; }
        public byte Value { get; set; }

        public bool Armed { get; set; }
        public byte ZoneId { get; set; }
        public bool Violation { get; set; }
        public bool Failure { get; set; }
        public byte DeviceId { get; set; }
        public byte InputNo { get; set; }
        public byte OutputNo { get; set; }
        public byte RelayNo { get; set; }
        public List<sconnOutput> Outputs { get; set; }
        public List<sconnInput> Inputs { get; set; }
        public List<sconnRelay> Relays { get; set; }

        public sconnInput ActiveInput { get; set; }
        public int ActiveInputId { get; set; }

        public sconnOutput ActiveOutput { get; set; }
        public sconnRelay ActiveRelay { get; set; }

        public List<sconnInput> Sensors
        {
            get
            {
                //device inputs are sensors if miwi com enabled
                if (this.ComMiWi && this.Type == sconnDeviceType.Pir_Sensor)
                {
                    return Inputs;
                }
                else
                {
                    return new List<sconnInput>();
                }
            }

            set
            {
                //device inputs are sensors if miwi com enabled
                if (this.ComMiWi && this.Type == sconnDeviceType.Pir_Sensor)
                {
                    Inputs = value;
                }
                else
                {

                }
            }
            
        }

        public void CopyFrom(sconnDevice other)
        {
            this.ActiveInput = other.ActiveInput;
            this.ActiveOutput = other.ActiveOutput;
            this.ActiveRelay = other.ActiveRelay;
            this.Armed = other.Armed;
            this.AuthDevicesCFG = other.AuthDevicesCFG;
            this.BatteryVoltage = other.BatteryVoltage;
            this.ComMiWi = other.ComMiWi;
            this.ComTcpIp = other.ComTcpIp;
            this.TemperatureModule = other.TemperatureModule;
            this.DeviceId = other.DeviceId;
            this.DomainNumber = other.DomainNumber;
            this.Failure = other.Failure;
            this.Name = other.Name;
            this.NamesCFG = other.NamesCFG;
            
            this.Inputs = other.Inputs;
            this.Outputs = other.Outputs;
            this.Relays = other.Relays;

            this.Type = other.Type;
            this.Revision = other.Revision;
            this.ActiveInputId = other.ActiveInputId;

            OnPropertyChanged();

        }

        public float MainVoltage { get; set; }
        public float BatteryVoltage { get; set; }
        public bool KeypadModule { get; set; }
        public bool TemperatureModule { get; set; }
        public bool HumidityModule { get; set; }
        public bool ComMiWi { get; set; }
        public bool ComTcpIp { get; set; }
        public byte DomainNumber { get; set; }
        public byte Revision { get; set; }
        public sconnDeviceType Type { get; set; }


        private byte[] _memCFG;
        private byte[][] _NamesCFG;
        private byte[][] _ScheduleCFG;
        private byte[] _NetworkConfig;
        public byte[] Hash;
        public byte[] AuthDevicesCFG { get; set; }

        public string imageIconUri { get; set; }
        public string imageRealUri { get; set; }

        public string Name { get; set; }

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private void LoadSupplyVoltageLevels()
        {
            MainVoltage = System.BitConverter.ToSingle(_memCFG, ipcDefines.mAdrSuppVolt_Start_Pos);
            BatteryVoltage = System.BitConverter.ToSingle(_memCFG, ipcDefines.mAdrBackupVolt_Start_Pos);
        }

        private void LoadDeviceStaticInfo()
        {
            this.KeypadModule = memCFG[ipcDefines.mAdrKeypadMod] > 0 ? true : false;
            this.TemperatureModule = memCFG[ipcDefines.mAdrTempMod] > 0 ? true : false;
            this.HumidityModule = memCFG[ipcDefines.mAdrHumMod] > 0 ? true : false;
            this.ComMiWi = memCFG[ipcDefines.comMiWi] > 0 ? true : false;
            this.ComTcpIp = memCFG[ipcDefines.comETH] > 0 ? true : false;
            this.DomainNumber = memCFG[ipcDefines.mAdrDomain];
            this.Revision = memCFG[ipcDefines.mAdrDevRev];

            if (Enum.IsDefined(typeof(sconnDeviceType), (int)memCFG[ipcDefines.mAdrDevType]))
            {
                Type = (sconnDeviceType)memCFG[ipcDefines.mAdrDevType];
            }
        }

        public string GetDeviceTypeImageUriForDevice(sconnDevice device)
        {
            if (device.Type == sconnDeviceType.Graphical_Keypad)
            {
                return "pack://application:,,,/images/klawiatura1000x1000.jpg";
            }
            else if (device.Type == sconnDeviceType.Gsm_Module)
            {
                return "pack://application:,,,/images/tel1000.jpg";
            }
            else if (device.Type == sconnDeviceType.Motherboard)
            {
                return "pack://application:,,,/images/strefa1000.jpg";
            }
            else if (device.Type == sconnDeviceType.Pir_Sensor)
            {
                return "pack://application:,,,/images/czujka1000x1000.jpg";
            }
            else if (device.Type == sconnDeviceType.Relay_Module)
            {
                return "pack://application:,,,/images/przek1000x1000.jpg";
            }
            else if (device.Type == sconnDeviceType.InputsModule)
            {
                return "pack://application:,,,/images/exp1000x1000.jpg";
            }
            else if (device.Type == sconnDeviceType.PirSonic_Sensor)
            {
                return "pack://application:,,,/images/czujka1000x1000.jpg";
            }
            else if (device.Type == sconnDeviceType.Siren)
            {
                return "pack://application:,,,/images/syrena1000x1000.jpg";
            }
            
            return null;
        }

        public void LoadImageTypeUrl()
        {
            imageIconUri = GetDeviceTypeImageUriForDevice(this);
        }


        private void LoadInputsFromConfig()
        {
            try
            {
                InputNo = _memCFG[ipcDefines.mAdrInputsNO];
                Inputs = new List<sconnInput>();
                for (int i = 0; i < InputNo; i++)
                {
                    byte[] inputBytes = new byte[ipcDefines.mAdrInputMemSize];
                    for (int j = 0; j < ipcDefines.mAdrInputMemSize; j++)
                    {
                        inputBytes[j] = _memCFG[ipcDefines.mAdrInput+ i*ipcDefines.mAdrInputMemSize + j];
                    }
                    sconnInput input = new sconnInput(inputBytes);
                    input.Id = (byte) i;
                    Inputs.Add(input);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        private void LoadOutputsFromConfig()
        {
            try
            {
                OutputNo = _memCFG[ipcDefines.mAdrOutputsNO];
                Outputs = new List<sconnOutput>();
                for (int i = 0; i < OutputNo; i++)
                {
                    byte[] outputBytes = new byte[ipcDefines.mAdrOutputMemSize];
                    for (int j = 0; j < ipcDefines.mAdrOutputMemSize; j++)
                    {
                        outputBytes[j] = _memCFG[ipcDefines.mAdrOutput + i * ipcDefines.mAdrOutputMemSize + j];
                    }
                    sconnOutput output = new sconnOutput(outputBytes);
                    output.Id = (byte)i;
                    Outputs.Add(output);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        private void LoadRelayFromConfig()
        {
            try
            {
                RelayNo = _memCFG[ipcDefines.mAdrRelayNO];
                Relays = new List<sconnRelay>();
                for (int i = 0; i < RelayNo; i++)
                {
                    byte[] relayBytes = new byte[ipcDefines.mAdrRelayMemSize];
                    for (int j = 0; j < ipcDefines.mAdrRelayMemSize; j++)
                    {
                        relayBytes[j] = _memCFG[ipcDefines.mAdrRelay + i * ipcDefines.mAdrRelayMemSize + j];
                    }
                    sconnRelay relay = new sconnRelay(relayBytes);
                    relay.Id = (byte)i;
                    Relays.Add(relay);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

       
        public void LoadPropertiesFromConfig()
        {
            LoadInputsFromConfig();
            LoadOutputsFromConfig();
            LoadRelayFromConfig();
            LoadSupplyVoltageLevels();
            LoadDeviceStaticInfo();
            LoadDeviceNames();

            LoadImageTypeUrl();
        }

        public byte[] NetworkConfig
        {
            get { return _NetworkConfig; }
            set { if (value != null) { _NetworkConfig = value; } }
        }

        public byte[] memCFG
        {
            get { return _memCFG; }
            set { if (value != null) { _memCFG = value; LoadPropertiesFromConfig(); } }
        }

        public byte[][] NamesCFG
        {
            get { return _NamesCFG; }
            set { if (value != null) { _NamesCFG = value; } }

        }

        public byte[][] ScheduleCFG
        {
            get { return _ScheduleCFG; }
            set { if (value != null) { _ScheduleCFG = value; } }
        }


        public string GetDeviceNameAt(int NameNo)
        {
            if (NameNo < ipcDefines.RAM_DEV_NAMES_NO)
            {
                return System.Text.Encoding.BigEndianUnicode.GetString(_NamesCFG[NameNo], 0, _NamesCFG[NameNo].GetLength(0));
            }
            else { return new string('E', 1); }
        }

        public void SetDeviceNameAt(int NameNo, string Name)
        {
            byte[] namebuff = System.Text.Encoding.BigEndianUnicode.GetBytes(Name);
            SetDeviceNameAt(NameNo, namebuff);
        }

        public void SetDeviceNameAt(int NameNo, byte[] Name)
        {
            try
            {
                if (Name.GetLength(0) < 32)
                {
                    byte[] resized = new byte[32];
                    Name.CopyTo(resized, 0);
                    _NamesCFG[NameNo] = resized;
                }
                else
                {
                    _NamesCFG[NameNo] = Name;
                }

                //replaces nulls by spaces
                for (int i = 0; i < _NamesCFG[NameNo].GetLength(0); i += 2)
                {
                    if ((_NamesCFG[NameNo][i] == (byte)0x00) && (_NamesCFG[NameNo][i + 1] == (byte)0x00))
                    {
                        _NamesCFG[NameNo][i] = (byte)0x20; //space
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }

    

        public sconnDevice(ipcDeviceConfig cfg) :this()
        {
            this.Inputs = cfg.Inputs;
            this.Outputs = cfg.Outputs;
            this.Relays = cfg.Relays;
            this.NetworkConfig = cfg.NetworkConfig;
            this._ScheduleCFG = cfg.ScheduleCFG;
            this._NamesCFG = cfg.NamesCFG;
            this.memCFG = cfg.memCFG;
            LoadDeviceNames();
        }

        public sconnDevice(byte[] buffer) :this()
        {
                this.Deserialize(buffer);
        }

        public void LoadDeviceNames()
        {
            try
            {
                if (this.NamesCFG.GetLength(0) == ipcDefines.RAM_DEV_NAMES_NO)
                {
                    string devName = System.Text.Encoding.BigEndianUnicode.GetString(this.NamesCFG[0]);
                    this.Name = devName;
                    int NameInc = 1;
                    for (int i = 0; i < this.Inputs.Count; i++)
                    {
                        this.Inputs[i].Name = GetDeviceNameAt(i + NameInc);
                    }
                    NameInc += ipcDefines.DeviceMaxInputs;
                    for (int i = 0; i < this.Outputs.Count; i++)
                    {
                        this.Outputs[i].Name = GetDeviceNameAt(i + NameInc);
                    }
                    NameInc += ipcDefines.DeviceMaxOutputs;
                    for (int i = 0; i < this.Relays.Count; i++)
                    {
                        this.Relays[i].Name = GetDeviceNameAt(i + NameInc);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }

        }

        public sconnDevice()
        {

            UUID = Guid.NewGuid().ToString();
            _memCFG = new byte[ipcDefines.deviceConfigSize];
            _NamesCFG = new byte[ipcDefines.RAM_DEV_NAMES_NO][];
            _NetworkConfig = new byte[ipcDefines.NET_CFG_SIZE];
            for (int i = 0; i < ipcDefines.RAM_DEV_NAMES_NO; i++)
            {
                _NamesCFG[i] = new byte[ipcDefines.RAM_NAME_SIZE];
            }
            _ScheduleCFG = new byte[ipcDefines.RAM_DEV_SCHED_NO][];
            for (int i = 0; i < ipcDefines.RAM_DEV_SCHED_NO; i++)
            {
                _ScheduleCFG[i] = new byte[ipcDefines.RAM_DEV_SCHED_MEM_SIZE];
            }

            Hash = new byte[ipcDefines.SHA256_DIGEST_SIZE];

            Inputs = new List<sconnInput>();
            Outputs = new List<sconnOutput>();
            Relays = new List<sconnRelay>();
        }

        public void SavePropertiesToRawConfig()
        {
            try
            {


                _memCFG[ipcDefines.mAdrInputsNO]  = (byte) InputNo;
                foreach (var item in Inputs)
                {
                    int baseaddr = ipcDefines.mAdrInput + ipcDefines.mAdrInputMemSize * item.Id;
                    _memCFG[baseaddr + ipcDefines.mAdrInputType] = (byte)item.Type;
                    _memCFG[baseaddr + ipcDefines.mAdrInputVal] = (byte)item.Value;
                    _memCFG[baseaddr + ipcDefines.mAdrInputNameAddr] = (byte)item.NameId;
                    _memCFG[baseaddr + ipcDefines.mAdrInputAG] = (byte)item.ActivationGroup;
                    _memCFG[baseaddr + ipcDefines.mAdrInputSensitivity] = (byte)(item.Sensitivity / ipcDefines.InputSensitivityStep);
                    _memCFG[baseaddr + ipcDefines.mAdrInputEnabled] = (byte)(item.Enabled == false ? 0 : 1);

                    SetDeviceNameAt(ipcDefines.mAddr_NAMES_Inputs_Pos + item.Id, item.Name);
                }

                _memCFG[ipcDefines.mAdrOutputsNO] =  (byte)OutputNo;
                foreach (var item in Outputs)
                {
                    int baseaddr = ipcDefines.mAdrOutput + ipcDefines.mAdrOutputMemSize * item.Id;
                    _memCFG[baseaddr + ipcDefines.mAdrOutputType] = (byte)item.Type;
                    _memCFG[baseaddr + ipcDefines.mAdrOutputVal] = (byte)(item.Value ? 1 : 0);
                    _memCFG[baseaddr + ipcDefines.mAdrOutputNameAddr] = (byte)item.NameId;
                    _memCFG[baseaddr + ipcDefines.mAdrOutputEnabled] = (byte)(item.Enabled == false ? 0 : 1);

                    SetDeviceNameAt(ipcDefines.mAddr_NAMES_Outputs_Pos + item.Id, item.Name);
                }

                _memCFG[ipcDefines.mAdrRelayNO] = (byte)RelayNo;
                foreach (var item in Relays)
                {
                    int baseaddr = ipcDefines.mAdrRelay + ipcDefines.RelayMemSize * item.Id;
                    _memCFG[baseaddr + ipcDefines.mAdrRelayType] = (byte)item.Type;
                    _memCFG[baseaddr + ipcDefines.mAdrRelayVal] = (byte)(item.Value ? 1 : 0);
                    _memCFG[baseaddr + ipcDefines.mAdrRelayNameAddr] = (byte)item.NameId;
                    _memCFG[baseaddr + ipcDefines.mAdrRelayEnabled] = (byte)(item.Enabled == false ? 0 : 1);

                    SetDeviceNameAt(ipcDefines.mAddr_NAMES_Relays_Pos + item.Id, item.Name);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
          
        }


        public byte[] GetConfigBytes()
        {
            return new byte[0];
        }

        public byte[] Serialize()
        {
            SavePropertiesToRawConfig();
            memCFG[ipcDefines.mAdrDevArmState] = (byte) (Armed ? 1 : 0);
            memCFG[ipcDefines.mAdrDevViolationState] = (byte)(Violation ? 1 : 0);
            memCFG[ipcDefines.mAdrDevFailureState] = (byte)(Failure ? 1 : 0);

            memCFG[ipcDefines.mAdrDeviceZone] = (byte)(ZoneId);
            memCFG[ipcDefines.mAdrDevRev] = (byte)(Revision);
            memCFG[ipcDefines.mAdrDevType] = (byte)(Type);
            memCFG[ipcDefines.mAdrDomain] = (byte)(DomainNumber);

            memCFG[ipcDefines.mAdrInputsNO] = (byte)(Inputs.Count);
            memCFG[ipcDefines.mAdrOutputsNO] = (byte)(Outputs.Count);
            memCFG[ipcDefines.mAdrRelayNO] = (byte)(Relays.Count);

            byte [] voltBackupBytes = System.BitConverter.GetBytes(BatteryVoltage);
            voltBackupBytes.CopyTo(memCFG, ipcDefines.mAdrBackupVolt_Start_Pos);

            byte[] voltMainBytes = System.BitConverter.GetBytes(MainVoltage);
            voltMainBytes.CopyTo(memCFG, ipcDefines.mAdrSuppVolt_Start_Pos);

            memCFG[ipcDefines.mAdrKeypadMod] = (byte)(KeypadModule ? 1 : 0);
            memCFG[ipcDefines.mAdrTempMod] = (byte)(TemperatureModule ? 1 : 0);
            memCFG[ipcDefines.mAdrHumMod] = (byte)(HumidityModule ? 1 : 0);
            memCFG[ipcDefines.mAdrCOMeth] = (byte)(ComTcpIp ? 1 : 0);
            memCFG[ipcDefines.mAdrCOMmiwi] = (byte)(ComMiWi ? 1 : 0);


            return this.memCFG;
        }

        public void Deserialize(byte[] buffer)
        {
            if (buffer.Length >= ipcDefines.deviceConfigSize)
            {
                for (int i = 0; i < ipcDefines.deviceConfigSize; i++)
                {
                    this.memCFG[i] = buffer[i];
                }
                LoadPropertiesFromConfig();
            }
        }

        public void Fake()
        {
            this.memCFG = new byte[ipcDefines.deviceConfigSize];
        }

        public byte[] SerializeNames()
        {
            try
            {
                byte[] DeviceNameBf = new byte[ipcDefines.RAM_DEVICE_NAMES_SIZE];

                //get names from all io
                SetDeviceNameAt(0, Name);
                int NameInc = 1;
                for (int i = 0; i < this.Inputs.Count; i++)
                {
                    SetDeviceNameAt(NameInc+i, this.Inputs[i].Name); // = GetDeviceNameAt(i + NameInc);
                }
                NameInc += ipcDefines.DeviceMaxInputs;
                for (int i = 0; i < this.Outputs.Count; i++)
                {
                    SetDeviceNameAt(NameInc + i, this.Outputs[i].Name); // this.Outputs[i].Name = GetDeviceNameAt(i + NameInc);
                }
                NameInc += ipcDefines.DeviceMaxOutputs;
                for (int i = 0; i < this.Relays.Count; i++)
                {
                    SetDeviceNameAt(NameInc + i, this.Relays[i].Name);   //this.Relays[i].Name = GetDeviceNameAt(i + NameInc);
                }

                for (int d = 0; d < ipcDefines.RAM_DEV_NAMES_NO; d++)
                {
                    byte[] sName = _NamesCFG[d];
                    sName.CopyTo(DeviceNameBf, d * ipcDefines.RAM_NAME_SIZE);
                }

                return DeviceNameBf;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return new byte[0];
            }
        }



        public void DeserializeEntityNames(byte[] buffer)
        {
            //convert to legacy 2dim
            byte[][] convNamesCFG = new byte[ipcDefines.RAM_DEV_NAMES_NO][];
            for (int i = 0; i < ipcDefines.RAM_DEV_NAMES_NO; i++)
            {
                byte[] sName = new byte[ipcDefines.RAM_NAME_SIZE];
                for (int j = 0; j < ipcDefines.RAM_NAME_SIZE; j++)
                {
                    sName[j] = buffer[i*ipcDefines.RAM_NAME_SIZE + j];
                }
                convNamesCFG[i] = sName;
            }
            this._NamesCFG = convNamesCFG;
            LoadDeviceNames();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public byte[] SerializeEntityNames()
        {
            return this.SerializeNames();
        }


        public string UUID { get; set; }
    }
}
