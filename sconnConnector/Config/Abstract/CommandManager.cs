using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.Abstract.Auth;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.Config.Abstract
{
    public enum CommandOperation
    {
        Get = ipcCMD.GET,
        Set = ipcCMD.SET,
        Push = ipcCMD.PSH,
        PushFin = ipcCMD.PSHFIN,
        Fin = ipcCMD.EOT,
        Ack = ipcCMD.ACK

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
            else if (type == typeof(sconnAuthorizedDevices))
            {
                return ipcCMD.setAuthDevCfg;
            }
            else if (type == typeof(sconnUserConfig))
            {
                return ipcCMD.setPasswdCfg;
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
            else if (type == typeof(sconnAuthorizedDevices))
            {
                return ipcCMD.getAuthDevices;
            }
            else if (type == typeof(sconnUserConfig))
            {
                return ipcCMD.getPasswdCfg;
            }
            return ipcCMD.ERRCMD;
        }

        private static byte GetCommandOperationType(Type type, CommandOperation oper)
        {
            if (oper == CommandOperation.Set)
            {
                return SetCommandForConfigType(type);
            }
            else if (oper == CommandOperation.Get)
            {
                return GetCommandForConfigType(type);
            }
            else if (oper == CommandOperation.Push)
            {
                return ipcCMD.PSH;
            }
            else if (oper == CommandOperation.PushFin)
            {
                return ipcCMD.PSHFIN;
            }
            return ipcCMD.ERRCMD;
        }

        public static byte[] GetHeaderForOperation(Type type, CommandOperation oper)
        {
            byte[] Header = new byte[ipcDefines.NET_UPLOAD_HEADER_BYTES];
            Header[ipcDefines.MessageHeader_Command_Pos] = (byte)oper;
            Header[ipcDefines.MessageHeader_CommandType_Pos] = (byte)GetCommandOperationType(type, oper);
            return Header;
        }

        public static byte[] GetTailForOperation(Type type, CommandOperation oper)
        {
            byte[] Header = new byte[ipcDefines.NET_UPLOAD_HEADER_BYTES];
            Header[ipcDefines.MessageHeader_Command_Pos] = (byte)oper;
            Header[ipcDefines.MessageHeader_CommandType_Pos] = (byte)GetCommandOperationType(type, oper);
            return Header;
        }

        public static byte[] GetMessageBodyForOperation(Type type, CommandOperation oper)
        {
            byte[] Header = new byte[ipcDefines.NET_UPLOAD_HEADER_BYTES];
            Header[ipcDefines.MessageHeader_Command_Pos] = (byte)oper;
            Header[ipcDefines.MessageHeader_CommandType_Pos] = (byte)GetCommandOperationType(type,oper);
            return Header;
        }


    }

}
