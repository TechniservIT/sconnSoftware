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
        protected IAlarmSystemEntityConfig Entity;
        protected static Logger _logger = LogManager.GetCurrentClassLogger();
        protected SconnClient client;
        public bool SingleUpload { get; set; }
        public bool Parametrized { get; set; }

        protected int EntityId;

        public AlarmGenericConfigManager(IAlarmSystemEntityConfig entity, Device device)
        {
            SingleUpload = true;
            Entity = entity;
            client = new SconnClient(device.EndpInfo.Hostname, device.EndpInfo.Port, device.Credentials.Password, true);
        }

        public AlarmGenericConfigManager(IAlarmSystemEntityConfig entity, Device device, int entityParam)
            : this(entity, device)
        {
            Parametrized = true;
            EntityId = entityParam;
        }

        public int AlarmGenericConfigManager_GetRemoteEntityCount()
        {
            byte[] data = new byte[1];
            data[0] = (byte)CommandManager.CommandManager_GetConfigTypeForEntity(typeof(T));  
            byte[] msgHeader = CommandManager.GetHeaderForOperationSingleQuery(CommandOperation.Einfo);
            byte[] messageTail = CommandManager.GetTailForOperation(typeof(T), CommandOperation.Einfo);
            byte[] uploadMsg = new byte[data.Length + msgHeader.Length + messageTail.Length];

            msgHeader.CopyTo(uploadMsg, 0);
            data.CopyTo(uploadMsg, msgHeader.Length);
            messageTail.CopyTo(uploadMsg, msgHeader.Length + data.Length);

            var res = this.SendMessage(uploadMsg);
            if (!IsResultSuccessForOperation(res, CommandOperation.Einfo)) //change to fail on any upload fail
            {
                return 0;
            }
            return res[1];
        }

        public bool Upload()
        {
            try
            {
                bool UploadSuccess = true;
                if (!client.Connect())
                {
                    client.Disconnect();
                    return false;
                }

                int entities = Entity.GetEntityCount();
                int remoteEntityCount = AlarmGenericConfigManager_GetRemoteEntityCount();
                //set remote entity info to match 
                if (entities != remoteEntityCount)
                {
                    byte[] msgHeader = CommandManager.GetHeaderForOperationParametrized(typeof(T), CommandOperation.SetInfo, entities);
                    byte[] messageTail = CommandManager.GetTailForOperation(typeof(T), CommandOperation.SetInfo);
                    byte[] uploadMsg = new byte[ msgHeader.Length + messageTail.Length];
                    msgHeader.CopyTo(uploadMsg, 0);
                    messageTail.CopyTo(uploadMsg, msgHeader.Length);
                    var res = this.SendMessage(uploadMsg);
                    if (!IsResultSuccessForOperation(res, CommandOperation.SetInfo))    //change to fail on any upload fail
                    {
                        return false;
                    }
                }

                //update remote entities
                for (int i = 0; i < entities; i++)
                {

                    byte[] data = this.Entity.SerializeEntityWithId(i);
                    byte[] msgHeader = CommandManager.GetHeaderForOperationParametrized(typeof(T), CommandOperation.Set,  i);
                    byte[] messageTail = CommandManager.GetTailForOperation(typeof(T), CommandOperation.Set);
                    byte[] uploadMsg = new byte[data.Length + msgHeader.Length + messageTail.Length];

                    msgHeader.CopyTo(uploadMsg, 0);
                    data.CopyTo(uploadMsg, msgHeader.Length);
                    messageTail.CopyTo(uploadMsg, msgHeader.Length + data.Length);

                    var res = this.SendMessage(uploadMsg);
                    if (!IsResultSuccessForOperation(res, CommandOperation.Set))    //change to fail on any upload fail
                    {
                        return false;
                    }
                }
                if (CommandManager.IsConfigEntityNamed(typeof(T)))
                {
                    UploadSuccess = UploadNames();                 //now send names config
                }
                client.Disconnect();
                return UploadSuccess;
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
                int entities = AlarmGenericConfigManager_GetRemoteEntityCount();
                Entity.Clear();
                for (int i = 0; i < entities; i++)
                {
                    byte[] header = CommandManager.GetHeaderForOperationParametrized(typeof(T), CommandOperation.Get, i);
                    var res = this.SendMessage(header);
                    if (IsResultSuccessForOperation(res, CommandOperation.Get))
                    {
                        byte[] msgBody = GetResultMessageForOperationResult(res, CommandOperation.Get);
                        Entity.DeserializeEntityWithId(msgBody);
                        if (CommandManager.IsConfigEntityNamed(typeof (T)))
                        {
                            this.DownloadNames();
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                client.Disconnect();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                client.Disconnect();
                return false;
            }
        }

        //public bool Download(byte Id)
        //{
        //    try
        //    {
        //        if (!client.Connect())
        //        {
        //            client.Disconnect();
        //            return false;
        //        }

        //        byte[] header = CommandManager.GetHeaderForOperationParametrized(typeof(T), CommandOperation.Get,Id);
        //        var res = this.SendMessage(header);
        //        if (IsResultSuccessForOperation(res, CommandOperation.Get))
        //        {
        //            byte[] msgBody = GetResultMessageForOperationResult(res, CommandOperation.Get);
        //            Entity.Deserialize(msgBody);
        //            if (CommandManager.IsConfigEntityNamed(typeof(T)))
        //            {
        //                this.DownloadNames();
        //            }
        //            client.Disconnect();
        //            return true;
        //        }

        //        client.Disconnect();
        //        return false;
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.Error(e, e.Message);
        //        client.Disconnect();
        //        return false;
        //    }

        //}


        private bool IsResultSuccessForOperation(byte[] result, CommandOperation oper)
        {
            if (oper == CommandOperation.Set)
            {
                return result.Contains(ipcCMD.TRUE);
            }
            else if (oper == CommandOperation.Get)
            {
                return result.Contains(ipcCMD.SVAL);
            }
            else if (oper == CommandOperation.Einfo)
            {
                return result.Contains(ipcCMD.SVAL);
            }
            else if (oper == CommandOperation.SetInfo)
            {
                return result.Contains(ipcCMD.SVAL);
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
                var hostNamedEntity = (IAlarmSystemNamedEntityConfig) this.Entity;

                if (client.client.Connected)
                {
                    int entities = hostNamedEntity.GetEntityCount();
                    int names = CommandManager.GetNamesNumberForEntity(typeof(T));
                    for (int l = 0; l < entities; l++)
                    {
                        byte[] namesCfgBuffer = new byte[ipcDefines.RAM_NAME_SIZE * names];
                        int entityNamesStartPos = CommandManager.GetNameStartPossitionForEntity(typeof(T));
                        for (int i = 0; i < names; i++)
                        {
                            byte[] header = CommandManager.GetHeaderForOperationParametrized(
                                typeof(T),
                                CommandOperation.Get,
                                entityNamesStartPos + (l* names) +i );
                            var res = this.SendMessage(header);
                            if (IsResultSuccessForOperation(res, CommandOperation.Get))
                            {
                                byte[] msgBody = GetResultMessageForOperationResult(res, CommandOperation.Get);
                                byte[] msgBytesName = new byte[ipcDefines.RAM_NAME_SIZE];
                                for (int j = 0; j < ipcDefines.RAM_NAME_SIZE; j++)
                                {
                                    msgBytesName[j] = msgBody[j];
                                }
                                msgBytesName.CopyTo(namesCfgBuffer, i * ipcDefines.RAM_NAME_SIZE);
                            }
                        }

                        hostNamedEntity.DeserializeEntityNames(l,namesCfgBuffer);
                        return true;
                    }
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
                    var hostNamedEntity = (IAlarmSystemNamedEntityConfig)this.Entity;
                    int entities = hostNamedEntity.GetEntityCount();
                    int names = CommandManager.GetNamesNumberForEntity(typeof(T));
                    for (int l = 0; l < entities; l++)
                    {
                        byte[] namesCfgBuffer = hostNamedEntity.SerializeEntityNames(l);
                        int entityNamesStartPos = CommandManager.GetNameStartPossitionForEntity(typeof(T));
                        for (int i = 0; i < names; i++)
                        {
                            byte[] header = CommandManager.GetHeaderForOperationParametrized(
                                typeof(T),
                                CommandOperation.Set,
                                entityNamesStartPos + (l*names+i));

                            //copy message body
                            byte[] nameconfig = new byte[ipcDefines.RAM_NAME_SIZE];
                            for (int j = 0; j < ipcDefines.RAM_NAME_SIZE; j++)
                            {
                                nameconfig[j] = namesCfgBuffer[i * ipcDefines.RAM_NAME_SIZE + j];
                            }
                            byte[] packetdaBytes = new byte[ipcDefines.RAM_NAME_SIZE + header.Length];
                            header.CopyTo(packetdaBytes, 0);
                            nameconfig.CopyTo(packetdaBytes, header.Length);
                            var res = this.SendMessage(packetdaBytes);
                            if (!IsResultSuccessForOperation(res, CommandOperation.Set))
                            {
                                //error during send - stop and log
                                _logger.Error("Alarm system data TRX error - name send failed.");
                                return false;
                            }
                        }
                        return true;

                    }
                   
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
