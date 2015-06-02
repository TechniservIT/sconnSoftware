using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections.Specialized;
using iotRestfulService.Security.Tokenizer;

namespace iotRestfulService
{
    public class DeviceService : IDeviceService
    {

        public String GetDevice(string id)
        {
            try
            {
                int DeviceId = Convert.ToInt32(id);
                iotContext cont = new iotContext();
                cont.Configuration.ProxyCreationEnabled = false;
                cont.Configuration.LazyLoadingEnabled = false;
                Device dev = cont.Devices.AsNoTracking().Where(d => d.DeviceId == DeviceId).SingleOrDefault(); //repo.GetById(DeviceId);
                return JsonConvert.SerializeObject(dev);

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }



        public String GetSomeJson()
        {
            iotRepository<Device> repo = new iotRepository<Device>();
            Device dev = repo.GetById(52);
            return JsonConvert.SerializeObject(dev);
        }


        public String GetSomeXml()
        {
            iotRepository<Device> repo = new iotRepository<Device>();
            Device dev = repo.GetById(52);
            return JsonConvert.SerializeObject(dev);
        }



        public String GetAllDevices()
        {
            try
            {
                iotRepository<Device> repo = new iotRepository<Device>();
                iotContext cont = new iotContext();
                cont.Configuration.ProxyCreationEnabled = false;
                cont.Configuration.LazyLoadingEnabled = false;
                List<Device> devs = cont.Devices.AsNoTracking().ToList(); //repo.GetById(DeviceId);
                return JsonConvert.SerializeObject(devs);

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }



        public int AddDevice(Device Device)
        {
            try
            {
                iotRepository<Device> repo = new iotRepository<Device>();
                repo.Add(Device);
                return Device.DeviceId;

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool UpdateDevice(Device Device)
        {
            try
            {
                iotRepository<Device> repo = new iotRepository<Device>();
                repo.Update(Device);
                return true;

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public bool DeleteDevice(string id)
        {
            try
            {

                int DeviceId = Convert.ToInt32(id);
                iotRepository<Device> repo = new iotRepository<Device>();
                Device dev = repo.GetById(DeviceId);
                repo.Delete(dev);
                return true;

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }




        /*************   AUTH ***************/

        public string GetAuthTokenPublic()
        {
            TokenProvider provider = new TokenProvider();
            return provider.CreateAuthenticationTokenMs();
        }

        public string GetAuthToken(string uname, string upass)
        {
            TokenProvider provider = new TokenProvider();
            return provider.CreateAuthenticationTokenMs();
        }



    }
}
