using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config.Abstract.Device;
using sconnConnector.POCO.Config.Abstract.IO;

namespace sconnConnector.POCO.Config.sconn
{
    public interface IAlarmSystemConfigurationEntity : ISerializableConfiguration
    {
        
    }

    public interface IAlarmSystemNamedConfigurationEntity : IAlarmSystemConfigurationEntity
    {
        byte[] SerializeNames();
        void DeserializeNames(byte[] buffer);
    }

    public enum sconnDeviceType
    {
        Graphical_Keypad = 1,
        Motherboard,
        Gsm_Module,
        Pir_Sensor,
        Relay_Module
    }



    public class sconnDevice : IAlarmSystemNamedConfigurationEntity, ISerializableConfiguration, IFakeAbleConfiguration
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
            this.Type = (sconnDeviceType)memCFG[ipcDefines.mAdrDevType];

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

        }

        public byte[][] ScheduleCFG
        {
            get { return _ScheduleCFG; }
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

        public sconnDevice(ipcDeviceConfig cfg)
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
                    string devName = Encoding.UTF8.GetString(this.NamesCFG[0]);
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
            byte[] DeviceNameBf = new byte[ipcDefines.RAM_DEVICE_NAMES_SIZE];
            for (int i = 0; i < ipcDefines.RAM_DEV_NAMES_NO; i++)
            {
                byte[] sName = _NamesCFG[i];
                sName.CopyTo(DeviceNameBf,i*ipcDefines.RAM_NAME_SIZE);
            }
            return DeviceNameBf;
        }

        public void DeserializeNames(byte[] buffer)
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
    }
}
