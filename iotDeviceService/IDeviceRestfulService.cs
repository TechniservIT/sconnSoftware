using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using iotDbConnector.DAL;

namespace iotDeviceService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IDeviceRestfulService
    {
        [OperationContract]
        [WebGet(UriTemplate = "Auth/Public")]
        string GetAuthTokenPublic();

        [OperationContract]
        [WebGet(UriTemplate = "Auth/Private/{uname}:{upass}")]
        string GetAuthToken(string uname, string upass);

        [OperationContract]
        [WebGet(UriTemplate = "Data/{token}")]
        string GetProtectedData(string token);



        [OperationContract]
        [WebGet(UriTemplate = "Device/{id}")]
        String GetDevice(string id);

        [WebGet(UriTemplate = "Devices")]
        [OperationContract]
        String GetAllDevices();


        [OperationContract]
        [WebGet(UriTemplate = "Device/GetSomeJson")]
        String GetSomeJson();


        [OperationContract]
        [WebGet(UriTemplate = "Device/GetSomeXml")]
        String GetSomeXml();


        [OperationContract]
        [WebInvoke(UriTemplate = "Device/Add", Method = "POST")]
        int AddDevice(Device Device);

        [OperationContract]
        [WebInvoke(UriTemplate = "Device/Update", Method = "PUT")]
        bool UpdateDevice(Device Device);

        [OperationContract]
        [WebInvoke(UriTemplate = "Device/Delete/{id}", Method = "DELETE")]
        bool DeleteDevice(string Id);



        /****** DEV *********/
        [OperationContract]
        [WebInvoke(UriTemplate = "Dbo/Clear", Method = "GET")]
        bool ClearDatabase();


       


    }

   
}
