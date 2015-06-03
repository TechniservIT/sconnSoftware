using iotDbConnector.DAL;
using iotRestfulService.Security.Tokenizer;
using MongoDB.Driver;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace iotDeviceService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class DeviceRestfulService : IDeviceRestfulService
    {

        private Logger _logger;

        public DeviceRestfulService()
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();
        }

        /*************   AUTH ***************/

        public string GetAuthTokenPublic()
        {
            try
            {
                TokenProvider provider = new TokenProvider();
                return provider.CreateAuthenticationTokenMs();
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ex.Message, ex);
                return null;
            }

        }

        public string GetAuthToken(string uname, string upass)
        {
            try
            {
                TokenProvider provider = new TokenProvider();
                return provider.CreateAuthenticationTokenAuth(uname);
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ex.Message, ex);
                return null;
            }


        }


        public string GetProtectedData(string token)
        {
            try
            {
                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("DeviceTokens");
                var collection = database.GetCollection<string>("tokens");

                return "";
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ex.Message, ex);
                return null;
            }

        }




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
            try
            {

            }
            catch (Exception ex)
            {
                _logger.ErrorException(ex.Message, ex);
            }
            iotRepository<Device> repo = new iotRepository<Device>();
            Device dev = repo.GetById(52);
            return JsonConvert.SerializeObject(dev);
        }


        public String GetSomeXml()
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.ErrorException(ex.Message, ex);
            }
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




    }
}
