using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.Config.Abstract
{
    public enum CommandOperation
    {
        Get = 1,
        Set,
        Push,
        PushFin,
        Fin,
        Ack,
        Alrm

    }

    public static class CommandManager
    {

        private static byte? SetCommandForConfigType(Type type)
        {
            if (type == typeof(sconnAlarmZoneConfig))
            {
                return ipcCMD.setGsmRcptCfg;
            }
            else if (type == typeof(sconnGsmConfig))
            {
                return ipcCMD.setGsmRcptCfg;
            }
            return null;
        }

        private static byte? GetCommandForConfigType(Type type)
        {

            return null;
        }

        private static byte? GetCommandOperationType(Type type, CommandOperation oper)
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

            return null;
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
