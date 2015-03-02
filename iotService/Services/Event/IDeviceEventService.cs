using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace iotServiceProvider
{
    public interface IDeviceEventCallback
    {

        [OperationContract(IsOneWay = true)]
        [ApplyDataContractResolver]
        void OnDeviceUpdated(Device dev);

    }


    [ServiceContract(SessionMode = SessionMode.Required,
    CallbackContract = typeof(IDeviceEventCallback))] 

    public interface IDeviceEventService
    {
        [OperationContract(IsOneWay = true)]
        void Subscribe();

        [OperationContract(IsOneWay = true)]
        void Unsubscribe(); 

    }
}
