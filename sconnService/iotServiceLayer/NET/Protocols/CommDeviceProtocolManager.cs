using iotDbConnector.DAL;
using iotSP.Net.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotServiceProvider.NET.Protocols
{
    public class CommDeviceProtocolManager
    {
        private Device _Device;


        public CommDeviceProtocolManager(Device dev)
        {
            _Device = dev;
        }


        private ICommProtocol GetProtocolDelegateForType(CommProtocolType protocol)
        {
            if (protocol == CommProtocolType.CommSconnProtocol)
            {
                return new CommSconnProtocol(_Device);
            }
            return null;
        }

        private CommProtocolType GetDeviceQueryProtcol()
        {
            if (_Device.EndpInfo.SupportsSconnProtocol)
            {
                return CommProtocolType.CommSconnProtocol;
            }
            return 0;
        }


        public Boolean PerformAction(DeviceAction action)
        {
            try
            {
                CommProtocolType protType = GetDeviceQueryProtcol();
                ICommProtocol protocol = GetProtocolDelegateForType(protType);
                if (protocol.ProtocolDeviceQueryAble())
                {
                    return protocol.PerformAction(action);
                }
                else
                {
                    return false;
                }    
            }
            catch (Exception e)
            {
                return false;              
            }

        }


        public Task PerformActionAsync(DeviceAction action)
        {
            try
            {
                CommProtocolType protType = GetDeviceQueryProtcol();
                ICommProtocol protocol = GetProtocolDelegateForType(protType);
                if (protocol.ProtocolDeviceQueryAble())
                {
                    return protocol.PerformActionAsync(action);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }


        public  Boolean QueryDeviceActions()
       {
           try
           {
               CommProtocolType protType = GetDeviceQueryProtcol();
               ICommProtocol protocol = GetProtocolDelegateForType(protType);
               if (protocol.ProtocolDeviceQueryAble())
               {
                   return protocol.LoadDeviceActions(_Device);
               }             
           }
           catch (Exception e)
           {  
           }
           return false;
        }
        public Boolean QueryDeviceProperties()
        {
            CommProtocolType protType = GetDeviceQueryProtcol();
            ICommProtocol protocol = GetProtocolDelegateForType(protType);

            if (protocol.ProtocolDeviceQueryAble())
            {
                return protocol.LoadDeviceProperties(_Device);
            }
            return false;
        }


        public Boolean UpdateDevice()
        {
            CommProtocolType protType = GetDeviceQueryProtcol();
            ICommProtocol protocol = GetProtocolDelegateForType(protType);

            if (protocol.ProtocolDeviceQueryAble())
            {
                return protocol.LoadDeviceProperties(_Device);
            }
            return false;
        }

    }

}
