using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.Abstract.Auth;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Config.sconn.Name;
using sconnConnector.POCO.Config.sconn.Schedule;

namespace sconnConnector.Config.Abstract
{
    public enum CommandOperation
    {
        Get = ipcCMD.GET,
        Set = ipcCMD.SET,
        Push = ipcCMD.PSH,
        PushFin = ipcCMD.PSHFIN,
        Fin = ipcCMD.EOT,
        Ack = ipcCMD.ACK,
        Einfo = ipcCMD.EINFO

    }

    public enum CommandConfigType
    {
         NET_PACKET_TYPE_GCFG = 1,
         NET_PACKET_TYPE_DEVCFG,
         NET_PACKET_TYPE_DEVNAMECFG,
         NET_PACKET_TYPE_SCHEDCFG,
         NET_PACKET_TYPE_PASSWDCFG,
         NET_PACKET_TYPE_AUTH,
         NET_PACKET_TYPE_DEVNETCFG,
         NET_PACKET_TYPE_GSMRCPTCFG,
         NET_PACKET_TYPE_DEVAUTHCFG,
         NET_PACKET_TYPE_GLOBNAMECFG,
         NET_PACKET_TYPE_ZONENAMECFG,
         NET_PACKET_TYPE_ZONECFG
    }

    public static class CommandManager
    {

        private static byte SetCommandForConfigType(Type type)
        {
            if (type == typeof(sconnAlarmZoneConfig))
            {
                return ipcCMD.setZoneName;
            }
            else if (type == typeof(sconnGsmConfig))
            {
                return ipcCMD.setGsmRcptCfg;
            }
            else if (type == typeof(sconnDeviceConfig))
            {
                return ipcCMD.setDeviceCfg;
            }
            else if (type == typeof(sconnGlobalConfig))
            {
                return ipcCMD.setGlobalCfg;
            }
            else if (type == typeof(sconnAuthorizedDevicesConfig))
            {
                return ipcCMD.setAuthDevCfg;
            }
            else if (type == typeof(sconnUserConfig))
            {
                return ipcCMD.setPasswdCfg;
            }
            return ipcCMD.ERRCMD;
        }

        private static byte GetConfigUploadTypeCode(Type type)
        {
            if (type == typeof(sconnAlarmZoneConfig))
            {
                return (byte)CommandConfigType.NET_PACKET_TYPE_ZONECFG;
            }
            else if (type == typeof(sconnGsmConfig))
            {
                return (byte)CommandConfigType.NET_PACKET_TYPE_GSMRCPTCFG;
            }
            else if (type == typeof(sconnDeviceConfig))
            {
                return (byte)CommandConfigType.NET_PACKET_TYPE_DEVCFG;
            }
            else if (type == typeof(sconnGlobalConfig))
            {
                return (byte)CommandConfigType.NET_PACKET_TYPE_GCFG;
            }
            else if (type == typeof(sconnAuthorizedDevicesConfig))
            {
                return (byte)CommandConfigType.NET_PACKET_TYPE_DEVAUTHCFG;
            }
            else if (type == typeof(sconnUserConfig))
            {
                return (byte)CommandConfigType.NET_PACKET_TYPE_PASSWDCFG;
            }
            return ipcCMD.ERRCMD;
        }

        private static byte GetCommandForConfigType(Type type)
        {
            if (type == typeof(sconnAlarmZoneConfig))
            {
                return ipcCMD.getZoneCfg;
            }
            else if (type == typeof(sconnGsmConfig))
            {
                return ipcCMD.getGsmRecpCfg;
            }
            else if (type == typeof(sconnDeviceConfig))
            {
                return ipcCMD.getRunDevCfg;
            }
            else if (type == typeof(sconnGlobalConfig))
            {
                return ipcCMD.getGlobCfg;
            }
            else if (type == typeof(sconnAuthorizedDevicesConfig))
            {
                return ipcCMD.getAuthDevices;
            }
            else if (type == typeof(sconnUserConfig))
            {
                return ipcCMD.getPasswdCfg;
            }
            else if (type == typeof(sconnEventConfig))
            {
                return ipcCMD.getEvents;
            }
            return ipcCMD.ERRCMD;
        }


        private static byte GetCommandOperationType(Type type, CommandOperation oper)
        {
            if (type == typeof(sconnAlarmZoneConfig))
            {
                return SetCommandForConfigType(type);
            }
            else if (oper == CommandOperation.Get)
            {
                return GetCommandForConfigType(type);
            }
            else if (oper == CommandOperation.Push)
            {
                return ipcCMD.PSHNXT;
            }
            else if (oper == CommandOperation.PushFin)
            {
                return GetConfigUploadTypeCode(type);
            }
            return ipcCMD.ERRCMD;
        }


        public static AlarmSystemConfigType CommandManager_GetConfigTypeForEntity(Type type)
        {
            if (type == typeof(sconnAlarmZoneConfig))
            {
                return AlarmSystemConfigType.AlarmSystemConfig_Zone;
            }
            else if (type == typeof(sconnGsmConfig))
            {
                return AlarmSystemConfigType.AlarmSystemConfig_Gsm;
            }
            else if (type == typeof(sconnDeviceConfig))
            {
                return AlarmSystemConfigType.AlarmSystemConfig_Device;
            }
            else if (type == typeof(sconnGlobalConfig))
            {
                return AlarmSystemConfigType.AlarmSystemConfig_Global;
            }
            else if (type == typeof(sconnAuthorizedDevicesConfig))
            {
                return AlarmSystemConfigType.AlarmSystemConfig_DeviceAuthorization;
            }
            else if (type == typeof(sconnUserConfig))
            {
                return AlarmSystemConfigType.AlarmSystemConfig_Users;
            }
            else if (type == typeof(sconnEvent))
            {
                return AlarmSystemConfigType.AlarmSystemConfig_Events;
            }
            else if (type == typeof(sconnName))
            {
                return AlarmSystemConfigType.AlarmSystemConfig_Name;
            }
            else if (type == typeof(sconnSchedule))
            {
                return AlarmSystemConfigType.AlarmSystemConfig_Schedule;
            }
            return 0;
        }


        private static byte GetCommandValueParam(Type type, CommandOperation oper, byte value)
        {
            if (oper == CommandOperation.Set)
            {
                return value;
            }
            else if (oper == CommandOperation.Push)
            {
                return ipcCMD.SVAL;
            }
            else if (oper == CommandOperation.Get)
            {
                return value;
            }
            else if (oper == CommandOperation.Push)
            {
                return ipcCMD.SVAL;
            }
            else if (oper == CommandOperation.PushFin)
            {
                return ipcCMD.SVAL;
            }
            return ipcCMD.SVAL;
        }

        private static byte GetEndCommandForOperation(Type type, CommandOperation oper)
        {
            if (oper == CommandOperation.Set)
            {
                return ipcCMD.EVAL;
            }
            else if (oper == CommandOperation.Push)
            {
                return ipcCMD.EVAL;
            }
            else if (oper == CommandOperation.Get)
            {
                return 0;
            }
            else if (oper == CommandOperation.PushFin)
            {
                return ipcCMD.EVAL;
            }
            else if (oper == CommandOperation.Einfo)
            {
                return 0;
            }
            return ipcCMD.EVAL;
        }

        public static byte[] GetHeaderForOperation(Type type, CommandOperation oper)
        {
            byte[] Header = new byte[ipcDefines.NET_UPLOAD_HEADER_BYTES];
            Header[ipcDefines.MessageHeader_Command_Pos] = (byte)oper;
            Header[ipcDefines.MessageHeader_CommandType_Pos] = (byte)GetCommandOperationType(type, oper);
            Header[ipcDefines.MessageHeader_CommandParam_Pos] = GetCommandValueParam(type, oper, 0);
            return Header;
        }

        public static byte[] GetHeaderForOperationParametrized(Type type, CommandOperation oper, int EntityId)
        {
            byte[] Header = new byte[ipcDefines.NET_UPLOAD_HEADER_BYTES];
            Header[ipcDefines.MessageHeader_Command_Pos] = (byte)oper;
            Header[ipcDefines.MessageHeader_CommandType_Pos] = (byte)CommandManager_GetConfigTypeForEntity(type);
            Header[ipcDefines.MessageHeader_CommandParam_Pos] = GetCommandValueParam(type,oper,(byte)EntityId);
            return Header; 
        }

        public static byte[] GetHeaderForOperationSingleQuery(CommandOperation oper)
        {
            byte[] Header = new byte[ipcDefines.NET_UPLOAD_HEADER_SINGLE_QUERY_BYTES];
            Header[ipcDefines.MessageHeader_Command_Pos] = (byte)oper;
            return Header;
        }

        public static byte[] GetHeaderForOperationRegisterParametrized(CommandOperation oper, byte CfgType, int RegisterId)
        {
            byte[] Header = new byte[ipcDefines.NET_UPLOAD_HEADER_BYTES];
            Header[ipcDefines.MessageHeader_Command_Pos] = (byte)oper;
            Header[ipcDefines.MessageHeader_CommandType_Pos] = (byte)CfgType;
            Header[ipcDefines.MessageHeader_Command_Reg_Low_Pos] = (byte)(RegisterId>>8);
            Header[ipcDefines.MessageHeader_Command_Reg_High_Pos] = (byte)(RegisterId);
            return Header;
        }

        public static byte[] GetTailForOperation(Type type, CommandOperation oper)
        {
            byte[] Header = new byte[ipcDefines.NET_UPLOAD_TAIL_BYTES];
            Header[ipcDefines.MessageHeader_Command_Pos] = GetEndCommandForOperation(type,oper);
            return Header;
        }

        public static byte[] GetMessageBodyForOperation(Type type, CommandOperation oper)
        {
            byte[] Header = new byte[ipcDefines.NET_UPLOAD_HEADER_BYTES];
            Header[ipcDefines.MessageHeader_Command_Pos] = (byte)oper;
            Header[ipcDefines.MessageHeader_CommandType_Pos] = (byte)GetCommandOperationType(type,oper);
            return Header;
        }


        /********** Config names ***********/

        public static bool IsConfigEntityNamed(Type type)
        {
            if (type == typeof(sconnDeviceConfig))
            {
                return true;
            }
            return false;
        }

        public static int GetNamesNumberForEntity(Type type)
        {
            if (type == typeof(sconnAlarmZoneConfig))
            {
                return 0;
            }
            else if (type == typeof(sconnDeviceConfig))
            {
                return ipcDefines.RAM_DEV_NAMES_NO;
            }
            else if (type == typeof(sconnGlobalConfig))
            {
                return ipcDefines.RAM_NAMES_Global_Total_Records;
            }
            return 0;
        }

        public static int GetNameStartPossitionForEntity(Type type)
        {
            if (type == typeof(sconnAlarmZoneConfig))
            {
                return ipcDefines.mAddr_NAMES_Zone_StartIndex;
            }
            else if (type == typeof(sconnDeviceConfig))
            {
                return ipcDefines.mAddr_NAMES_Device_StartIndex;
            }
            return 0;
        }

    }

}
