using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;
using NLog;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.Config.Abstract
{
    public abstract class AlarmGenericConfigManager<T> where T: new()
    {
        protected IAlarmSystemConfigurationEntity Entity;
        protected static Logger _logger = LogManager.GetCurrentClassLogger();
        protected SconnClient client;

        public AlarmGenericConfigManager(IAlarmSystemConfigurationEntity entity, Device device)
        {
            Entity =  entity;
            client = new SconnClient(device.EndpInfo.Hostname, device.EndpInfo.Port, device.Credentials.Password, true);
        }

        public bool Upload()
        {
            try
            {
                if (!client.Connect())
                {
                    return false;
                }

                if (Upload_Send_Start_Message())
                {
                    Upload_Send_Config();
                    if (Upload_Send_Config())
                    {
                        return Upload_Signal_Finish();
                    }
                }
                client.Disconnect();
                return false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                client.Disconnect();
                return false;
            }
            
        }

        private bool Upload_Send_Start_Message()
        {
            try
            {
                byte[] header = CommandManager.GetHeaderForOperation(typeof(T),CommandOperation.Set);
                var res = this.SendMessage(header);
                if (IsResultSuccessForOperation(res, CommandOperation.Set))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        private bool Upload_Send_Config()
        {
            try
            {
                byte[] configBody = this.Entity.Serialize();
                int cfgPackets = (configBody.Length / ipcDefines.NET_MAX_TX_SIZE);
                int partialPacketBytes = configBody.Length % ipcDefines.NET_MAX_TX_SIZE;
                for (int i = 0; i < cfgPackets; i++)
                {
                    byte[] packetBytes = new byte[ipcDefines.NET_PACKET_TX_PAYLOAD_SIZE];
                    for (int j = 0; j < ipcDefines.NET_MAX_TX_SIZE; j++)
                    {
                        packetBytes[j] = configBody[i * ipcDefines.NET_MAX_TX_SIZE + j];
                    }
                    if (!Upload_Send_Config_Packet(packetBytes))
                    {
                        return false;
                    }
                }
                if (partialPacketBytes > 0)
                {
                    byte[] packetBytes = new byte[partialPacketBytes];
                    for (int j = 0; j < partialPacketBytes; j++)
                    {
                        packetBytes[j] = configBody[cfgPackets * ipcDefines.NET_MAX_TX_SIZE + j];
                    }
                    if (!Upload_Send_Config_Packet(packetBytes))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }


        }

        private bool Upload_Send_Config_Packet(byte [] data)
        {
            try
            {
                byte[] msgHeader = CommandManager.GetHeaderForOperation(typeof(T),CommandOperation.Push);
                byte[] messageTail = CommandManager.GetHeaderForOperation(typeof(T), CommandOperation.Push);
                byte[] uploadMsg = new byte[ipcDefines.NET_MAX_TX_SIZE];

                msgHeader.CopyTo(uploadMsg, 0);
                data.CopyTo(uploadMsg, ipcDefines.NET_UPLOAD_HEADER_BYTES);
                messageTail.CopyTo(uploadMsg, ipcDefines.NET_UPLOAD_HEADER_BYTES + data.Length);

                var res = this.SendMessage(uploadMsg);
                if (IsResultSuccessForOperation(res, CommandOperation.Push))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }


        }

        private bool Upload_Signal_Finish()
        {
            try
            {
                byte[] header = CommandManager.GetHeaderForOperation(typeof(T), CommandOperation.PushFin);
                var res = this.SendMessage(header);
                if (IsResultSuccessForOperation(res, CommandOperation.PushFin))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public Task UploadAsync()
        {
            return null;
        }

        public bool Download()
        {
            try
            {
                if (!client.Connect())
                {
                    return false;
                }

                byte[] header = CommandManager.GetHeaderForOperation(typeof(T), CommandOperation.Get);
                var res = this.SendMessage(header);
                if (IsResultSuccessForOperation(res, CommandOperation.Get))
                {
                    byte[] msgBody = GetResultMessageForOperationResult(res, CommandOperation.Get);
                    Entity.Deserialize(res);
                    return true;
                }
                return client.Disconnect();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                client.Disconnect();
                return false;
            }

        }

        private bool IsResultSuccessForOperation(byte[] result, CommandOperation oper)
        {
            if (oper == CommandOperation.Set)
            {
                return result.Contains(ipcCMD.ACK);
            }
            else if (oper == CommandOperation.Get)
            {
                return result.Contains(ipcCMD.ACK);
            }
            else if (oper == CommandOperation.Push)
            {
                return result.Contains(ipcCMD.PSHNXT);
            }
            else if (oper == CommandOperation.PushFin)
            {
                return result.Contains(ipcCMD.ACKFIN);
            }
            else if (oper == CommandOperation.Fin)
            {
                return result.Contains(ipcCMD.ACKFIN);
            }
            return false;
        }

        private byte[] GetResultMessageForOperationResult(byte[] result, CommandOperation oper)
        {
            return null;
        }
        

        public Task DownloadAsync()
        {
            return null;
        }

        public void Sync()
        {
            
        }
        
        private byte[] SendMessage(byte [] Message)
        {
             return  this.client.berkeleySendMsg(Message);
        }



    }

}
