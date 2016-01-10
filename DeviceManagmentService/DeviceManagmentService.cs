using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using NLog;

namespace DeviceManagmentService
{
    public class DeviceProvider : IDeviceManagmentService
    {
        private IIotContextBase context;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public DeviceProvider(IIotContextBase cont)
        {
            this.context = cont;
        }

        public bool Add(Device entity, int LocationId, int TypeId, int SiteId)
        {
            try
            {
                Location loc = context.Locations.FirstOrDefault(l => l.Id == LocationId);
                DeviceType type = context.Types.FirstOrDefault(t => t.Id == TypeId);
                Site site = context.Sites.FirstOrDefault(s => s.Id == SiteId);
                if (loc != null && type != null && entity != null && site != null)
                {
                    var endp = context.Endpoints.Add(entity.EndpInfo);
                    context.SaveChanges();
                    entity.EndpInfo = context.Endpoints.FirstOrDefault(e=>e.Id == endp.Id);
                    var cred = context.Credentials.Add(entity.Credentials);
                    context.SaveChanges();
                    entity.Credentials = context.Credentials.FirstOrDefault(e => e.Id == cred.Id);

                    entity.Site = site;
                    entity.DeviceLocation = loc;
                    entity.Type = type;
                    var added = context.Devices.Add(entity);
                    context.SaveChanges();
                    return context.Devices.FirstOrDefault(d=>d.Id == added.Id) != null;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }
        }

        public bool Add(Device entity)
        {
            try
            {
                var added = context.Devices.Add(entity);
                context.SaveChanges();
                return context.Devices.Contains(added);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }
        }

        public bool Remove(Device entity)
        {
            try
            {
                var added = context.Devices.Remove(entity);
                context.SaveChanges();
                return !context.Devices.Contains(added);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }
                
        }

        public List<Device> GetAll()
        {
            try
            {
                var res = context.Devices.ToList();
                return res;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

        public bool Update(Device entity)
        {
            try
            {
                var device = context.Devices.FirstOrDefault(d => d.Id == entity.Id);
                if (device != null)
                {
                    device.Load(entity);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }
        }

        public Device GetById(int Id)
        {
            try
            {
                var device = context.Devices.FirstOrDefault(d => d.Id == Id);
                return device;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

        public bool RemoveById(int Id)
        {
            try
            {
                var dev = this.GetById(Id);
                if (dev != null)
                {
                    return this.Remove(dev);
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }
    }
}
