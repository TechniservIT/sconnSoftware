using iotServiceProvider;
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


        private bool VerifyChannel(ChannelFactory<IiotDomainService> fact)
        {
            return false;
        }

        public IiotDomainService GetDomainClient()
        {
            dbServiceFactory = new ChannelFactory<IiotDomainService>("iotDbServiceNamedPipe");
            return dbServiceFactory.CreateChannel();        
        }

        public IDeviceEventService GetEventClient()
        {
            InstanceContext context = new InstanceContext(this);
            EventServiceFactory = new DuplexChannelFactory<IDeviceEventService>(context, "iotDeviceEventServicePipe");
            return EventServiceFactory.CreateChannel();
        }


    }


}