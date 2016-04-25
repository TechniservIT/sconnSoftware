using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using NLog;
using sconnConnector.POCO.Config;
using sconnConnector.POCO.Config.Abstract.Auth;
using sconnConnector.POCO.Config.sconn;
using sconnConnector.POCO.Device;

namespace sconnConnector.Config.Abstract
{
    public class AlarmGenericConfigManager<T> where T : new()
    {
        protected IAlarmSystemConfigurationEntity Entity;
        protected static Logger _logger = LogManager.GetCurrentClassLogger();
        protected SconnClient client;
        public bool SingleUpload { get; set; }

        protected int EntityId;

        public AlarmGenericConfigManager(IAlarmSystemConfigurationEntity entity, Device device)
        {
            SingleUpload = true;
            Entity = entity;
            client = new SconnClient(device.EndpInfo.Hostname, device.EndpInfo.Port, device.Credentials.Password, true);
        }

        public AlarmGenericConfigManager(IAlarmSystemConfigurationEntity entity, Device device, int entityParam)
            : this(entity, device)
        {
            EntityId = entityParam;
        }

        public bool Upload()
        {
            try
            {
                bool UploadSuccess = false;
                if (!client.Connect())
                {
                    client.Disconnect();
                    return false;
                }

                if (SingleUpload)
                {
                    byte[] data = this.Entity.Serialize();
                    byte[] msgHeader = CommandManager.GetHeaderForOperationParametrized(typeof (T), CommandOperation.Set,
                        EntityId);
                    byte[] messageTail = CommandManager.GetTailForOperation(typeof (T), CommandOperation.Set);
                    byte[] uploadMsg = new byte[data.Length + msgHeader.Length + messageTail.Length];

                    msgHeader.CopyTo(uploadMsg, 0);
                    data.CopyTo(uploadMsg, ipcDefines.NET_UPLOAD_HEADER_BYTES);
                    messageTail.CopyTo(uploadMsg, ipcDefines.NET_UPLOAD_HEADER_BYTES + data.Length);

                    var res = this.SendMessage(uploadMsg);
                    if (IsResultSuccessForOperation(res, CommandOperation.Set))
                    {
                        UploadSuccess =  true;
                    }
                }
                else
                {
                    if (Upload_Send_Start_Message())
                    {
                        if (Upload_Send_Config(this.Entity.Serialize()))
                        {
                            if (Upload_Signal_Finish())
                            {
                                UploadSuccess =  true;
                            }
                        }
                    }
                }

                bool namesUploaded = UploadNames();                 //now send names config
                client.Disconnect();
                return UploadSuccess && namesUploaded;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                client.Disconnect();
                return false;
            }

        }



        private bool IsEntityParametrized(Type entity)
        {
            return false;
        }

        private bool Upload_Send_Start_Message()
        {
            try
            {
                byte[] header = CommandManager.GetHeaderForOperationParametrized(typeof (T), CommandOperation.Set,
                    EntityId);
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

        private bool Upload_Send_Config(byte[] configBody)
        {
            try
            {
                // byte[] configBody = this.Entity.Serialize();
                int cfgPackets = (configBody.Length/ipcDefines.NET_MAX_PACKET_DATA);
                int partialPacketBytes = configBody.Length%ipcDefines.NET_MAX_PACKET_DATA;
                for (int i = 0; i < cfgPackets; i++)
                {
                    byte[] packetBytes = new byte[ipcDefines.NET_MAX_PACKET_DATA];
                    for (int j = 0; j < ipcDefines.NET_MAX_PACKET_DATA; j++)
                    {
                        packetBytes[j] = configBody[i*ipcDefines.NET_MAX_PACKET_DATA + j];
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
                        packetBytes[j] = configBody[cfgPackets*ipcDefines.NET_MAX_PACKET_DATA + j];
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

        private bool Upload_Send_Config_Packet(byte[] data)
        {
            try
            {
                byte[] msgHeader = CommandManager.GetHeaderForOperation(typeof (T), CommandOperation.Push);
                byte[] messageTail = CommandManager.GetTailForOperation(typeof (T), CommandOperation.Push);
                byte[] uploadMsg = new byte[data.Length + msgHeader.Length + messageTail.Length];

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
                byte[] header = CommandManager.GetHeaderForOperation(typeof (T), CommandOperation.PushFin);
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
                    client.Disconnect();
                    return false;
                }

                byte[] header = CommandManager.GetHeaderForOperation(typeof (T), CommandOperation.Get);
                var res = this.SendMessage(header);
                if (IsResultSuccessForOperation(res, CommandOperation.Get))
                {
                    byte[] msgBody = GetResultMessageForOperationResult(res, CommandOperation.Get);
                    Entity.Deserialize(msgBody);
                    if (CommandManager.IsConfigEntityNamed(typeof (T)))
                    {
                        this.DownloadNames();
                    }
                    client.Disconnect();
                    return true;
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

        private bool IsResultSuccessForOperation(byte[] result, CommandOperation oper)
        {
            if (oper == CommandOperation.Set)
            {
                return result.Contains(ipcCMD.ACK);
            }
            else if (oper == CommandOperation.Get)
            {
                return result.Contains(ipcCMD.SVAL);
            }
            else if (oper == CommandOperation.Push)
            {
                return result.Contains(ipcCMD.ACKNXT);
            }
            else if (oper == CommandOperation.PushFin)
            {
                return result.Contains(ipcCMD.ACKFIN);
            }
            return false;
        }

        private byte[] GetResultMessageForOperationResult(byte[] result, CommandOperation oper)
        {
            try
            {
                if (result.Length > 1)
                {
                    byte[] resultMsg = new byte[result.Length - 1];
                    if (result[0] == ipcCMD.SVAL)
                    {
                        for (int i = 0; i < result.Length - 1; i++)
                        {
                            resultMsg[i] = result[i + 1];
                        }
                        return resultMsg;
                    }
                }
                return new byte[0];
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return new byte[0];
            }
        }


        public Task DownloadAsync()
        {
            return null;
        }

        public void Sync()
        {

        }

        private byte[] SendMessage(byte[] Message)
        {
            return this.client.berkeleySendMsg(Message);
        }
        

        private bool DownloadNames()
        {
            try
            {
                var hostNamedEntity = (IAlarmSystemNamedConfigurationEntity) this.Entity;
                if (client.client.Connected)
                {
                    int names = CommandManager.GetNamesNumberForEntity(typeof (T));
                    byte[] namesCfgBuffer = new byte[ipcDefines.RAM_NAME_SIZE*names];
                    int entityNamesStartPos = CommandManager.GetNameStartPossitionForEntity(typeof (T));
                    for (int i = 0; i < names; i++)
                    {
                        byte[] header = CommandManager.GetHeaderForOperationRegisterParametrized(CommandOperation.Get,
                            ipcCMD.getName, entityNamesStartPos + i);
                        var res = this.SendMessage(header);
                        if (IsResultSuccessForOperation(res, CommandOperation.Get))
                        {
                            byte[] msgBody = GetResultMessageForOperationResult(res, CommandOperation.Get);
                            byte[] msgBytesName = new byte[ipcDefines.RAM_NAME_SIZE];
                            for (int j = 0; j < ipcDefines.RAM_NAME_SIZE; j++)
                            {
                                msgBytesName[j] = msgBody[j];
                            }
                            msgBytesName.CopyTo(namesCfgBuffer, i*ipcDefines.RAM_NAME_SIZE);
                        }
                    }
                    hostNamedEntity.DeserializeNames(namesCfgBuffer);
                    return true;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
            return false;
        }

        private bool UploadNames()
        {
            try
            {
                if (client.client.Connected)
                {
                    var hostNamedEntity = (IAlarmSystemNamedConfigurationEntity)this.Entity;
                    int names = CommandManager.GetNamesNumberForEntity(typeof (T));
                    byte[] namesCfgBuffer = hostNamedEntity.SerializeNames();
                    int entityNamesStartPos = CommandManager.GetNameStartPossitionForEntity(typeof(T));
                    for (int i = 0; i < names; i++)
                    {
                        byte[] header = CommandManager.GetHeaderForOperationRegisterParametrized(CommandOperation.Set, ipcCMD.setName, entityNamesStartPos + i);

                        //copy message body
                        byte[] nameconfig = new byte[ipcDefines.RAM_NAME_SIZE];
                        for (int j = 0; j < ipcDefines.RAM_NAME_SIZE; j++)
                        {
                            nameconfig[j] = namesCfgBuffer[i*ipcDefines.RAM_NAME_SIZE + j];
                        }
                        byte[] packetdaBytes = new byte[ipcDefines.RAM_NAME_SIZE+header.Length];
                        header.CopyTo(packetdaBytes,0);
                        nameconfig.CopyTo(packetdaBytes,header.Length);
                        var res = this.SendMessage(packetdaBytes);
                        if ( !IsResultSuccessForOperation(res, CommandOperation.Set))
                        {
                            //error during send - stop and log
                            _logger.Error("Alarm system data TRX error - name send failed.");
                            return false;
                        }

                    }
                    return true;

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);

            }
            return false;
        }
    }

}
