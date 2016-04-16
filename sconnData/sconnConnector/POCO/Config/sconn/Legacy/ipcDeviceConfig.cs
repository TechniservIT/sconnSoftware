﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config
{
    public class ipcDeviceConfig
    {
        private byte[] _memCFG;
        private byte[][] _NamesCFG;
        private byte[][] _ScheduleCFG;
        private byte[] _NetworkConfig;
        public byte[] Hash;

        public byte[] AuthDevicesCFG { get; set; }


        public int DeviceId { get; set; }

        public int InputNo { get; set; }

        public int OutputNo { get; set; }

        public int RelayNo { get; set; }

        public List<sconnOutput> Outputs { get; set; }

        public List<sconnInput> Inputs { get; set; }

        public List<sconnRelay> Relays { get; set; }


        private void LoadInputsFromConfig()
        {
            Inputs = new List<sconnInput>();
            InputNo = _memCFG[ipcDefines.mAdrInputsNO];
            for (int i = 0; i < InputNo; i++)
            {
                sconnInput input = new sconnInput(_memCFG); //, _NamesCFG[ipcDefines.mAddr_NAMES_Inputs_Pos], i
                Inputs.Add(input);
            }
        }

        private void LoadOutputsFromConfig()
        {
            Outputs = new List<sconnOutput>();
            OutputNo = _memCFG[ipcDefines.mAdrOutputsNO];
            for (int i = 0; i < OutputNo; i++)
            {
                sconnOutput output = new sconnOutput(_memCFG);  //, _NamesCFG[ipcDefines.mAddr_NAMES_Outputs_Pos], i
                Outputs.Add(output);
            }
        }

        private void LoadRelayFromConfig()
        {
            Relays = new List<sconnRelay>();
            RelayNo = _memCFG[ipcDefines.mAdrRelayNO];
            for (int i = 0; i < RelayNo; i++)
            {
                sconnRelay relay = new sconnRelay(_memCFG); //, _NamesCFG[ipcDefines.mAddr_NAMES_Relays_Pos], i
                Relays.Add(relay);
            }
        }

        public void LoadPropertiesFromConfig()
        {
            LoadInputsFromConfig();
            LoadOutputsFromConfig();
            LoadRelayFromConfig();
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
            // set { if (value != null) { _NamesCFG = value; } }

        }

        public byte[][] ScheduleCFG
        {
            get { return _ScheduleCFG; }
        }



        public string GetDeviceNameAt(int NameNo)
        {
            if (NameNo < ipcDefines.RAM_DEV_NAMES_NO)
            {
                return System.Text.Encoding.Unicode.GetString(_NamesCFG[NameNo], 0, _NamesCFG[NameNo].GetLength(0));
            }
            else { return new string('E', 1); }
        }

        public void SetDeviceNameAt(int NameNo, string Name)
        {
            byte[] namebuff = System.Text.Encoding.Unicode.GetBytes(Name);
            SetDeviceNameAt(NameNo, namebuff);
        }

        public void SetDeviceNameAt(int NameNo, byte[] Name)
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

        public ipcDeviceConfig()
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
            foreach (var item in Inputs)
            {
                int baseaddr = ipcDefines.mAdrInput + ipcDefines.mAdrInputMemSize * item.Id;
                _memCFG[baseaddr + ipcDefines.mAdrInputType] = (byte)item.Type;
                _memCFG[baseaddr + ipcDefines.mAdrInputVal] = (byte)item.Value;
                _memCFG[baseaddr + ipcDefines.mAdrInputNameAddr] = (byte)item.NameId;
                _memCFG[baseaddr + ipcDefines.mAdrInputAG] = (byte)item.ActivationGroup;
                _memCFG[baseaddr + ipcDefines.mAdrInputSensitivity] = (byte)(item.Sensitivity / ipcDefines.InputSensitivityStep);
                _memCFG[baseaddr + ipcDefines.mAdrInputEnabled] = (byte)(item.Enabled == false ? 0 : 1);

                //TODO - load from memory separate service
                //SetDeviceNameAt(ipcDefines.mAddr_NAMES_Inputs_Pos + item.Id, item.Name);

                //_NamesCFG[] = System.Text.Encoding.Unicode.GetBytes(item.Name);
            }

            foreach (var item in Outputs)
            {
                int baseaddr = ipcDefines.mAdrOutput + ipcDefines.mAdrOutputMemSize * item.Id;
                _memCFG[baseaddr + ipcDefines.mAdrOutputType] = (byte)item.Type;
                _memCFG[baseaddr + ipcDefines.mAdrOutputVal] = (byte)(item.Value ? 1 : 0);
                _memCFG[baseaddr + ipcDefines.mAdrOutputNameAddr] = (byte)item.NameId;
                _memCFG[baseaddr + ipcDefines.mAdrOutputEnabled] = (byte)(item.Enabled == false ? 0 : 1);

                //TODO - load from memory separate service
               // SetDeviceNameAt(ipcDefines.mAddr_NAMES_Outputs_Pos + item.Id, item.Name);
            }

            foreach (var item in Relays)
            {
                int baseaddr = ipcDefines.mAdrRelay + ipcDefines.RelayMemSize * item.Id;
                _memCFG[baseaddr + ipcDefines.mAdrRelayType] = (byte)item.Type;
                _memCFG[baseaddr + ipcDefines.mAdrRelayVal] = (byte)(item.Value ? 1 : 0);
                _memCFG[baseaddr + ipcDefines.mAdrRelayNameAddr] = (byte)item.NameId;
                _memCFG[baseaddr + ipcDefines.mAdrRelayEnabled] = (byte)(item.Enabled == false ? 0 : 1);

                //TODO - load from memory separate service
                //SetDeviceNameAt(ipcDefines.mAddr_NAMES_Relays_Pos + item.Id, item.Name);
                //_NamesCFG[ipcDefines.mAddr_NAMES_Relays_Pos + item.Id] = System.Text.Encoding.Unicode.GetBytes(item.Name);
            }
        }


        public byte[] GetConfigBytes()
        {
            return new byte[0];
        }


    }

}
