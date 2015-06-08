using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace iotServiceProvider
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public  class DeviceUpdateEventService : IDeviceEventService
    {
        public delegate void CallbackDelegate<T>(T t);
        public static CallbackDelegate<Device> DeviceUpdated;

        #region IEventSystem Members

        public void Subscribe()
        {
            IDeviceEventCallback callback =
              OperationContext.Current.GetCallbackChannel<IDeviceEventCallback>();
            DeviceUpdated += callback.OnDeviceUpdated;
            ICommunicationObject obj = (ICommunicationObject)callback;

            obj.Closed += new EventHandler(EventService_Closed);
            obj.Closing += new EventHandler(EventService_Closing);
        }

        void EventService_Closing(object sender, EventArgs e)
        {
            //Console.WriteLine("Client Closing...");
        }

        void EventService_Closed(object sender, EventArgs e)
        {
            DeviceUpdated -= ((IDeviceEventCallback)sender).OnDeviceUpdated;
            //Console.WriteLine("Closed Client Removed!");
        }

        public void Unsubscribe()
        {
            IDeviceEventCallback callback =
              OperationContext.Current.GetCallbackChannel<IDeviceEventCallback>();
            DeviceUpdated -= callback.OnDeviceUpdated;
        }

        #endregion

        public static void SendDeviceUpdate(Device dev)
        {
            try
            {
                //get act device 
                iotContext cont = new iotContext();
                Device dbDevice = (from d in cont.Devices
                                   where d.DeviceId == dev.DeviceId
                                   select d).First();
                DeviceUpdated(dbDevice);
            }
            catch (Exception e)
            {
       
            
            }
       
        }

     
    }

}
