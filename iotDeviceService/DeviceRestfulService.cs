using iotDbConnector.DAL;
using iotNoSqlDatabase;
using iotRestfulService.Security.Tokenizer;
using iotServiceProvider.NET.Protocols;
using MongoDB.Driver;
using Newtonsoft.Json;
using NLog;
using Raven.Client.Embedded;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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

                var documentStore = new EmbeddableDocumentStore
                {
                    DataDirectory = "Data"
                };

                return "";
            }
            catch (Exception ex)
            {
                _logger.ErrorException(ex.Message, ex);
                return null;
            }

        }
        
        public Device GetDevice(string id)
        {
            try
            {
                int DeviceId = Convert.ToInt32(id);
                iotContext cont = new iotContext();
                cont.Configuration.ProxyCreationEnabled = false;
                cont.Configuration.LazyLoadingEnabled = false;
                Device dev = cont.Devices.AsNoTracking().Where(d => d.Id == DeviceId).SingleOrDefault(); //repo.GetById(DeviceId);
                return dev;
                //return JsonConvert.SerializeObject(dev);

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



        public List<Device> Devices()
        {
            try
            {
                iotRepository<Device> repo = new iotRepository<Device>();
                iotContext cont = new iotContext();
                cont.Configuration.ProxyCreationEnabled = false;
                cont.Configuration.LazyLoadingEnabled = false;
                List<Device> devs = cont.Devices.AsNoTracking().ToList(); //repo.GetById(DeviceId);
                return devs;
                //return JsonConvert.SerializeObject(devs);

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





        public bool ClearDatabase()
        {
            try
            {
                iotRepository<Device>.ClearIotRepository();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        /// <summary>
        /// Database access 
        /// </summary>

        private Logger nlogger = LogManager.GetCurrentClassLogger();
        
        public iotDomain GetDomainWithName(string name)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                return db.GetById(name);
                // return JsonConvert.SerializeObject(db.GetById(name));
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return null;
            }
        }


        public iotDomain GetDomainWithId(string id)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                return db.GetById(id);
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return null;
            }
        }


        public bool DomainAdd(iotDomain domain)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                return db.Add(domain).Length > 0;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }



        /******************* Add ***********************/

        public bool AddDeviceToDomainAtSite(Device dev, string DomainId, int SiteId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    //add to devices
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    site.Devices.Add(dev);
                    db.Update(domain);  //update domain
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }

        }


        public bool AddSiteToDomain(Site site, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    domain.Sites.Add(site);
                    db.Update(domain);  //update domain
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool AddActionToDeviceAtSiteInDomain(DeviceAction action, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(d => d.Id == DeviceId);
                    dev.Actions.Add(action);
                    db.Update(domain);  //update domain
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool PropertyAdd(DeviceProperty prop, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(d => d.Id == DeviceId);
                    dev.Properties.Add(prop);
                    db.Update(domain);  //update domain
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool LocationAdd(Location loc, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    domain.Locations.Add(loc);
                    db.Update(domain);  //update domain
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ActionResultParamAdd(DeviceParameter param, int ActionId,  int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(d => d.Id == DeviceId);
                    DeviceAction act = dev.Actions.First(a => a.Id == ActionId);
                    act.ResultParameters.Add(param);
                    db.Update(domain);  //update domain
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool PropertyResultParamAdd(DeviceParameter param, int PropertyId, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(d => d.Id == DeviceId);
                    DeviceProperty act = dev.Properties.First(a => a.Id == PropertyId);
                    act.ResultParameters.Add(param);
                    db.Update(domain);  //update domain
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }



        public bool ActionRequiredParamAdd(ActionParameter param, int ActionId, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(d => d.Id == DeviceId);
                    DeviceAction act = dev.Actions.First(a => a.Id == ActionId);
                    act.RequiredParameters.Add(param);
                    db.Update(domain);  //update domain
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }



        public bool DeviceTypeAdd(DeviceType type, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    domain.DeviceTypes.Add(type);
                    db.Update(domain);  //update domain
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool SetDeviceCredentials(DeviceCredentials creds, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(d => d.Id == DeviceId);
                    dev.Credentials = creds;
                    db.Update(domain);  //update domain
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }

        public bool SetDeviceEndpoint(EndpointInfo endp, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(d => d.Id == DeviceId);
                    dev.EndpInfo = endp;
                    db.Update(domain);  //update domain
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        /**************************** Get ****************************/

        public List<Site> GetSitesInDomain(string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    return domain.Sites.ToList();
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<Site>();
            }
        }


        public List<Device> Devices( int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    return site.Devices.ToList();
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<Device>();
            }
        }


        public List<DeviceAction> DeviceActions(int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);   
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(d => d.Id == DeviceId);
                    return dev.Actions.ToList();
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<DeviceAction>();
            }
        }


        public List<DeviceProperty> DeviceProperties(int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId); 
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(d => d.Id == DeviceId);
                    return dev.Properties.ToList();
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<DeviceProperty>();
            }
        }


        //public List<DeviceParameter> DeviceParameters(int DeviceId, int SiteId, string DomainId)
        //{
        //    try
        //    {
        //        RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
        //        iotDomain domain = db.GetById(DomainId);
        //        if (domain != null)
        //        {
        //            Site site = domain.Sites.First(s => s.Id == SiteId);
        //            Device dev = site.Devices.First(d => d.Id == DeviceId);
        //            return 
        //        }
        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        nlogger.ErrorException(e.Message, e);
        //        return new List<DeviceParameter>();
        //    }
        //}


        public List<Location> Locations(string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);
                if (domain != null)
                {
                    return domain.Locations.ToList();
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<Location>();
            }
        }


        public List<DeviceType> DeviceTypes(string DomainId)
        {
            try
            {
                iotRepository<DeviceType> repo = new iotRepository<DeviceType>();
                List<DeviceType> devt = repo.GetAll().ToList();
                return devt;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<DeviceType>();
            }
        }


        //public List<ActionParameter> ActionParams(int DeviceId, int SiteId, string DomainId)
        //{
        //    try
        //    {
        //        RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
        //        iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
        //        if (domain != null)
        //        {
        //            Site site = domain.Sites.First(s => s.Id == SiteId);
        //            Device dev = site.Devices.First(d => d.Id == DeviceId);
                    
        //        }
        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        nlogger.ErrorException(e.Message, e);
        //        return new List<ActionParameter>();
        //    }
        //}


        public DeviceCredentials DeviceCredentials(int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(d => d.Id == DeviceId);
                    return dev.Credentials;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceCredentials();
            }
        }

        public EndpointInfo DeviceEndpoint(int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(d => d.Id == DeviceId);
                    return dev.EndpInfo;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new EndpointInfo();
            }
        }


        /************** Get by id *****************/


        public Site SiteWithId(string DomainId, int SiteId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    return site;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new Site();
            }
        }


        public Device DeviceWithId(int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(s => s.Id == SiteId);
                    return dev;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new Device();
            }
        }


        public DeviceAction DeviceActionWithId(int ActionId, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(s => s.Id == SiteId);
                    DeviceAction act = dev.Actions.First(a => a.Id == ActionId);
                    return act;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceAction();
            }
        }


        public DeviceProperty DevicePropertieWithId(int PropertyId, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(s => s.Id == SiteId);
                    DeviceProperty prop = dev.Properties.First(a => a.Id == PropertyId);
                    return prop;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceProperty();
            }
        }


        //public DeviceParameter DeviceParameterWithId(int DeviceId, int SiteId, string DomainId)
        //{
        //    try
        //    {
        //        iotRepository<DeviceParameter> repo = new iotRepository<DeviceParameter>();
        //        return repo.GetById(id);
        //    }
        //    catch (Exception e)
        //    {
        //        nlogger.ErrorException(e.Message, e);
        //        return new DeviceParameter();
        //    }
        //}


        public iotDomain DomainWithId(string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);  
                if (domain != null)
                {
                    return domain;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new iotDomain();
            }
        }


        public Location LocationWithId(int id, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);
                if (domain != null)
                {
                    Location loc = domain.Locations.First(l => l.Id == id);
                    return loc;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new Location();
            }
        }


        public List<Device> GetAllDevices()
        {
            return null;
        }

        public DeviceType DeviceTypeWithId(int id, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);   
                if (domain != null)
                {
                    DeviceType type = domain.DeviceTypes.First(t => t.Id == id);
                    return type;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceType();
            }
        }


        //public ActionParameter ActionParamWithId(int DeviceId, int SiteId, string DomainId)
        //{
        //    try
        //    {
        //        RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
        //        iotDomain domain = db.GetById(DomainId);
        //        if (domain != null)
        //        {
        //            Site site = domain.Sites.First(s => s.Id == SiteId);
        //            Device dev = site.Devices.First(s => s.Id == SiteId);
        //            DeviceProperty prop = dev.Properties.First(a => a.Id == PropertyId);
        //            return prop;
        //        }
        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        nlogger.ErrorException(e.Message, e);
        //        return new ActionParameter();
        //    }
        //}

        public DeviceCredentials DeviceCredentialWithId(int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(s => s.Id == SiteId);
                    DeviceCredentials cred = dev.Credentials;
                    return cred;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceCredentials();
            }
        }

        public EndpointInfo EndpointWithId(int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(s => s.Id == SiteId);
                    EndpointInfo info = dev.EndpInfo;
                    return info;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new EndpointInfo();
            }
        }


        /*********************** Update ***********************/


        public bool DomainUpdate(iotDomain domain)
        {
            try
            {
                iotRepository<iotDomain> repo = new iotRepository<iotDomain>();
                repo.Update(domain);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool DeviceUpdate(Device device, int SiteId, string DomainId)
        {
            try
            {


                iotRepository<Device> repo = new iotRepository<Device>();
                repo.Update(device);
               // Task updateTask = Task.Factory.StartNew(() => { DeviceUpdateEventService.SendDeviceUpdate(device); }); //Dispatch update notify
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool SiteUpdate(Site domain , string DomainId)
        {
            try
            {
                iotRepository<Site> repo = new iotRepository<Site>();
                repo.Update(domain);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ActionUpdate(DeviceAction domain, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotRepository<DeviceAction> repo = new iotRepository<DeviceAction>();
                repo.Update(domain);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool PropertyUpdate(DeviceProperty Property, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(d => d.Id == DeviceId);
                    DeviceProperty prop = dev.Properties.First(p => p.Id == Property.Id);
                    prop = Property;
                    db.Update(domain);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool LocationUpdate(Location Location, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);
                if (domain != null)
                {
                    Location loc = domain.Locations.First(l => l.Id == Location.Id);
                    loc = Location;
                    db.Update(domain);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ResParamUpdate(DeviceParameter param, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);
                if (domain != null)
                {
                    if (param.Action != null)
                    {
                       // ActionParameter param =this.DeviceActionWithId(param.Action.Id);
                      //  repo.Update(domain);
                    }
                    else
                    {
                     //   DeviceParameter param = domain..First(p => p.Id == Property.Id);
                    //    repo.Update(domain);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        //public bool ReqParamUpdate(ActionParameter param, int DeviceId, int SiteId, string DomainId)
        //{
        //    try
        //    {
        //        iotRepository<ActionParameter> repo = new iotRepository<ActionParameter>();
        //        repo.Update(domain);
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        nlogger.ErrorException(e.Message, e);
        //        return false;
        //    }
        //}


        public bool DeviceTypeUpdate(DeviceType type , string DomainId)
        {
            try
            {
                iotRepository<DeviceType> repo = new iotRepository<DeviceType>();
                repo.Update(type);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool DeviceCredentialsUpdate(DeviceCredentials creds, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotRepository<DeviceCredentials> repo = new iotRepository<DeviceCredentials>();
                repo.Update(creds);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool EndpointUpdate(EndpointInfo endp, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotRepository<EndpointInfo> repo = new iotRepository<EndpointInfo>();
                repo.Update(endp);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }




        /*********************** Delete ***********************/


        public bool DomainRemove(iotDomain domain)
        {
            try
            {
                iotRepository<iotDomain> repo = new iotRepository<iotDomain>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool DeviceRemove(Device domain, int SiteId, string DomainId)
        {
            try
            {
                iotRepository<Device> repo = new iotRepository<Device>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool SiteRemove(Site Site, string DomainId)
        {
            try
            {
                RedisNoSqlDataSource<iotDomain> db = new RedisNoSqlDataSource<iotDomain>();
                iotDomain domain = db.GetById(DomainId);
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == Site.Id);
                    domain.Sites.Remove(site);
                    db.Update(domain);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ActionRemove(DeviceAction domain, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotRepository<DeviceAction> repo = new iotRepository<DeviceAction>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool PropertyRemove(DeviceProperty domain, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotRepository<DeviceProperty> repo = new iotRepository<DeviceProperty>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool LocationRemove(Location domain,  string DomainId)
        {
            try
            {
                iotRepository<Location> repo = new iotRepository<Location>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ResParamRemove(DeviceParameter domain, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotRepository<DeviceParameter> repo = new iotRepository<DeviceParameter>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ReqParamRemove(ActionParameter domain, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotRepository<ActionParameter> repo = new iotRepository<ActionParameter>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool DeviceTypeRemove(DeviceType type,  string DomainId)
        {
            try
            {
                iotRepository<DeviceType> repo = new iotRepository<DeviceType>();
                repo.Delete(type);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool DeviceCredentialsRemove(DeviceCredentials creds, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotRepository<DeviceCredentials> repo = new iotRepository<DeviceCredentials>();
                repo.Delete(creds);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool EndpointRemove(EndpointInfo endp, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotRepository<EndpointInfo> repo = new iotRepository<EndpointInfo>();
                repo.Delete(endp);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        /*********************************   IOT  QUERY / UPDATE    ********************************/

        public bool UpdateDeviceProperties(Device dev,  int SiteId, string DomainId)
        {
            try
            {
                CommDeviceProtocolManager man = new CommDeviceProtocolManager(dev);
                man.QueryDeviceProperties();
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }

        public bool UpdateDeviceActionState(Device dev, int SiteId, string DomainId)
        {
            try
            {
                CommDeviceProtocolManager man = new CommDeviceProtocolManager(dev);
                man.QueryDeviceActions();
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }

        public bool PerformDeviceAction(DeviceAction act, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                CommDeviceProtocolManager man = new CommDeviceProtocolManager(act.Device);
                //Task actTask = man.PerformActionAsync(act);
                //actTask.ContinueWith( t => { DeviceUpdateEventService.SendDeviceUpdate(act.Device); });
                man.PerformAction(act);

                //DeviceUpdateEventService.SendDeviceUpdate(act.Device);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }




        /************************  CUSTOM CROSS QUERY ********************/
        public Device DeviceWithEndpoint(EndpointInfo endp, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotContext cont = new iotContext();
                Device stored = cont.Devices.Where(d => d.EndpInfo.Hostname.Equals(endp.Hostname) && d.EndpInfo.Port == endp.Port).FirstOrDefault();
                return stored;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new Device();
            }
        }



        /************************  ADD/REMOVE WITH PARAM ********************/

        public Device DeviceAddWithParams(string SiteId, string Name, string Host, string Port, string Login, string Pass, string Type, string Loc, string Prot, string DomainId)
        {
            try
            {
                iotContext cont = new iotContext();
                int LocId = int.Parse(Loc);
                int DevTypeId = int.Parse(Type);
                Device ndev = new Device();
                ndev.DeviceName = Name;
                List<DeviceType> types = cont.Types.ToList();
                DeviceType type = (from t in types
                                   where t.Id == DevTypeId
                                   select t).First();
                List<Location> locs = cont.Locations.ToList();
                Location loc = (from l in locs
                                where l.Id == LocId
                                select l).First();
                ndev.Type = type;
                ndev.DeviceLocation = loc;
                DeviceCredentials cred = new DeviceCredentials();
                cred.PasswordExpireDate = DateTime.Now.AddYears(100);
                cred.PermissionExpireDate = DateTime.Now.AddYears(100);
                cred.Password = Pass;
                cred.Username = Login;
                cont.Credentials.Add(cred);
                cont.SaveChanges();

                List<DeviceCredentials> creds = cont.Credentials.ToList();
                DeviceCredentials storedCredentials = (from c in creds
                                                       where c.Username.Equals(cred.Username)
                                                       select c).FirstOrDefault();

                EndpointInfo info = new EndpointInfo();
                info.Hostname = Host;
                info.Port = int.Parse(Port);

                //TODO
                //CommProtocolType protocol = (CommProtocolType)Prot; //int.Parse(Prot);
                //info.EnableProtocolSupport(protocol);
                info.SupportsSconnProtocol = true;
                cont.Endpoints.Add(info);
                cont.SaveChanges();

                List<EndpointInfo> endps = cont.Endpoints.ToList();
                EndpointInfo storedInfo = (from i in endps
                                           where i.Hostname.Equals(info.Hostname) &&
                                           i.Port == info.Port
                                           select i).FirstOrDefault();
                if (storedInfo == null)
                {
                    //err
                    return null;
                }

                ndev.Credentials = storedCredentials;
                ndev.EndpInfo = storedInfo;

                int siteIdNum = int.Parse(SiteId);

                List<Site> sites = cont.Sites.ToList();
                Site siteToAppend = (from s in sites
                                     where s.Id == siteIdNum
                                     select s).FirstOrDefault();
                ndev.Site = siteToAppend;
                cont.Devices.Add(ndev);
                cont.SaveChanges();

                List<Device> devs = cont.Devices.ToList();
                Device stored = (from d in devs
                                 where d.DeviceName.Equals(ndev.DeviceName) &&
                                 d.EndpInfo.Hostname.Equals(ndev.EndpInfo.Hostname)
                                 select d).FirstOrDefault();

                //update device
                UpdateDeviceProperties(stored, stored.Site.Id, DomainId);

                return stored;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return null;
            }
        }



    }
}
