using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DeviceRestfulService
{
    public class DeviceService : IDeviceService
    {

        public Device GetDevice(string id)
        {
            try
            {
                int DeviceId = Convert.ToInt32(id);
                iotRepository<Device> repo = new iotRepository<Device>();
                return repo.GetById(DeviceId);

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public List<Device> GetAllDevice()
        {
            try
            {
                iotRepository<Device> repo = new iotRepository<Device>();
                return repo.GetAll().ToList();

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
                return Device.Id;

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
