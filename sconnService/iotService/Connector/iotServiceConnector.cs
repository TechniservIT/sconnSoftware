using iotServiceProvider;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace iotDash.Service
{

    /* TODO dynamic service connection & setting  */

    public class iotServiceConnector
    {
        private ChannelFactory<IiotDomainService> dbServiceFactory;

        private DuplexChannelFactory<IDeviceEventService> EventServiceFactory;

        private Logger nlogger = LogManager.GetCurrentClassLogger();


        private bool VerifyChannel(ChannelFactory<IiotDomainService> fact)
        {
            return false;
        }

        public IiotDomainService GetDomainClient()
        {
            try
            {
                dbServiceFactory = new ChannelFactory<IiotDomainService>("iotDbServiceNamedPipe");
                return dbServiceFactory.CreateChannel();      
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message,e);
                return null; 
            }
        }

        public IDeviceEventService GetEventClient(InstanceContext context)
        {
            try
            {
                //InstanceContext context = new InstanceContext(subscriber);
                EventServiceFactory = new DuplexChannelFactory<IDeviceEventService>(context, "iotDeviceEventServicePipe");
                return EventServiceFactory.CreateChannel();
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return null;           
            }
        }


    }


}