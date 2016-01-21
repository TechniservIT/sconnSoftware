using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDbConnector.DAL;
using NLog;
using sconnConnector.Config.Abstract;
using sconnConnector.POCO.Config.sconn;

namespace sconnConnector.Config.Manager
{
    
    /********* Config manager for names *********/
    /*
    *
    *   TX/RX Names for all applicable alarm configuration types
    *   Names transfered one by one , number determined by configuration type.
    *
    *
    */

    public class AlarmGenericNamesConfigManager<T> where T : new()
    {
        protected IAlarmSystemNamedConfigurationEntity Entity;
        protected static Logger _logger = LogManager.GetCurrentClassLogger();
        protected SconnClient client;
        protected int EntityId;

        public AlarmGenericNamesConfigManager(IAlarmSystemNamedConfigurationEntity entity, Device device)
        {
            Entity = entity;
            client = new SconnClient(device.EndpInfo.Hostname, device.EndpInfo.Port, device.Credentials.Password, true);
        }

        public AlarmGenericNamesConfigManager(IAlarmSystemNamedConfigurationEntity entity, Device device, int EntityParam)
        {
            Entity = entity;
            EntityId = EntityParam;
            client = new SconnClient(device.EndpInfo.Hostname, device.EndpInfo.Port, device.Credentials.Password, true);
        }

        public bool Upload()
        {
            try
            {
                if (!client.Connect())
                {
                    client.Disconnect();
                    return false;
                }

                byte[] data = this.Entity.Serialize();
                byte[] msgHeader = CommandManager.GetHeaderForOperationParametrized(typeof(T), CommandOperation.Set, EntityId);
                byte[] messageTail = CommandManager.GetTailForOperation(typeof(T), CommandOperation.Set);
                byte[] uploadMsg = new byte[data.Length + msgHeader.Length + messageTail.Length];

                msgHeader.CopyTo(uploadMsg, 0);
                data.CopyTo(uploadMsg, ipcDefines.NET_UPLOAD_HEADER_BYTES);
                messageTail.CopyTo(uploadMsg, ipcDefines.NET_UPLOAD_HEADER_BYTES + data.Length);

                var res = this.SendMessage(uploadMsg);
                if (IsResultSuccessForOperation(res, CommandOperation.Set))
                {
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

                byte[] header = CommandManager.GetHeaderForOperation(typeof(T), CommandOperation.Get);
                var res = this.SendMessage(header);
                if (IsResultSuccessForOperation(res, CommandOperation.Get))
                {
                    byte[] msgBody = GetResultMessageForOperationResult(res, CommandOperation.Get);
                    Entity.DeserializeNames(msgBody);
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
        

    }


}
