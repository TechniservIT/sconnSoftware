using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DeviceRestfulService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IDeviceService
    {

        [OperationContract]
        [WebGet(UriTemplate = "Device/{id}")]
        Device GetDevice(string id);

        [OperationContract]
        [WebGet]
        List<Device> GetAllDevice();

        [OperationContract]
        [WebInvoke(UriTemplate = "Device/Add", Method = "POST")]
        int AddDevice(Device Device);

        [OperationContract]
        [WebInvoke(UriTemplate = "Device/Update", Method = "PUT")]
        bool UpdateDevice(Device Device);

        [OperationContract]
        [WebInvoke(UriTemplate = "Device/Delete/{id}", Method = "DELETE")]
        bool DeleteDevice(string Id);
    }


}
