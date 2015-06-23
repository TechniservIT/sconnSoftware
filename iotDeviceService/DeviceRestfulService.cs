using iotDatabaseConnector.DAL.Repository.Runtime;
using iotDatabaseConnector.Runtime;
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                return cont.GetById(name);  //InMemoryContext.GetDomainForName(name); 
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                return cont.GetById(id);
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                domain.Id = domain.DomainName;
                cont.Add(domain);
                return true;
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);   //db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    //add to devices
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    site.Devices.Add(dev);
                    cont.UpdateWithHistory(domain);  //update domain
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    if (domain.Sites == null)
                    {
                        domain.Sites = new AIList<Site>();
                    }
                    domain.Sites.Add(site);
                    cont.UpdateWithHistory(domain);        //db.UpdateById(domain,domain.DomainName);  //update domain
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


        public bool AddActionToDeviceAtSiteInDomain(DeviceAction action, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceAction> propCont = new iotSharedEntityContext<DeviceAction>();
                    propCont.Add(action);
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


        public bool PropertyAdd(DeviceProperty prop, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceProperty> propCont = new iotSharedEntityContext<DeviceProperty>();
                    propCont.Add(prop);
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
               iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)    
                {
                    if (domain.Locations == null)
                    {
                        domain.Locations = new AIList<Location>();
                    }
                    domain.Locations.Add(loc);
                    cont.UpdateWithHistory(domain);   //db.UpdateById(domain, domain.DomainName);  //update domain
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceParameter> propCont = new iotSharedEntityContext<DeviceParameter>();
                    DeviceParameter stored = propCont.GetById(param.Id);
                    stored = param;
                    propCont.UpdateWithHistory(stored);
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


        public bool PropertyResultParamAdd(DeviceParameter param, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceParameter> propCont = new iotSharedEntityContext<DeviceParameter>();
                    DeviceParameter stored = propCont.GetById(param.Id);
                    stored = param;
                    propCont.UpdateWithHistory(stored);
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



        public bool ActionRequiredParamAdd(ActionParameter param, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<ActionParameter> propCont = new iotSharedEntityContext<ActionParameter>();
                    ActionParameter stored = propCont.GetById(param.Id);
                    stored = param;
                    propCont.UpdateWithHistory(stored);
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  
                if (domain != null)
                {
                    if (domain.DeviceTypes == null)
                    {
                        domain.DeviceTypes = new AIList<DeviceType>();
                    }
                    domain.DeviceTypes.Add(type);
                    cont.UpdateWithHistory(domain);  
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


        public bool SetDeviceCredentials(DeviceCredentials creds, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceCredentials> propCont = new iotSharedEntityContext<DeviceCredentials>();
                    DeviceCredentials stored = propCont.GetById(creds.Id);
                    if (stored != null)
                    {
                        stored = creds;
                        propCont.UpdateWithHistory(stored);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }

        public bool SetDeviceEndpoint(EndpointInfo endp, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<EndpointInfo> propCont = new iotSharedEntityContext<EndpointInfo>();
                    EndpointInfo stored = propCont.GetById(endp.Id);
                    if (stored != null)
                    {
                        stored = endp;
                        propCont.UpdateWithHistory(stored);
                        return true;
                    }
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);     //fetch domain from DB
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


        public List<Device> Devices(string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<Device> propCont = new iotSharedEntityContext<Device>();
                    List<Device> stored = propCont.GetAll().ToList();
                    if (stored != null)
                    {
                        return stored;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<Device>();
            }
        }


        public List<DeviceAction> DeviceActions(string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceAction> propCont = new iotSharedEntityContext<DeviceAction>();
                    List<DeviceAction> stored = propCont.GetAll().ToList();
                    if (stored != null)
                    {
                        return stored;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<DeviceAction>();
            }
        }


        public List<DeviceProperty> DeviceProperties(string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceProperty> propCont = new iotSharedEntityContext<DeviceProperty>();
                    List<DeviceProperty> stored = propCont.GetAll().ToList();
                    if (stored != null)
                    {
                        return stored;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<DeviceProperty>();
            }
        }


        public List<DeviceParameter> DeviceParameters(string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceParameter> propCont = new iotSharedEntityContext<DeviceParameter>();
                    List<DeviceParameter> stored = propCont.GetAll().ToList();
                    if (stored != null)
                    {
                        return stored;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<DeviceParameter>();
            }
        }


        public List<Location> Locations(string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);
                if (domain != null)
                {
                    return domain.DeviceTypes.ToList();
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<DeviceType>();
            }
        }


        public List<ActionParameter> ActionParams(string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<ActionParameter> propCont = new iotSharedEntityContext<ActionParameter>();
                    List<ActionParameter> stored = propCont.GetAll().ToList();
                    if (stored != null)
                    {
                        return stored;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<ActionParameter>();
            }
        }


        public DeviceCredentials DeviceCredentials(int DeviceId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceCredentials> propCont = new iotSharedEntityContext<DeviceCredentials>();
                    DeviceCredentials stored = propCont.GetById(DeviceId);
                    if (stored != null)
                    {
                        return stored;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceCredentials();
            }
        }

        public EndpointInfo DeviceEndpoint(int EndpointId,  string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<EndpointInfo> propCont = new iotSharedEntityContext<EndpointInfo>();
                    EndpointInfo stored = propCont.GetById(EndpointId);
                    if (stored != null)
                    {
                        return stored;
                    }
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);     //fetch domain from DB
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


        public Device DeviceWithId(int DeviceId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<Device> propCont = new iotSharedEntityContext<Device>();
                    Device stored = propCont.GetById(DeviceId);
                    if (stored != null)
                    {
                        return stored;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new Device();
            }
        }


        public DeviceAction DeviceActionWithId(int ActionId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceAction> propCont = new iotSharedEntityContext<DeviceAction>();
                    DeviceAction stored = propCont.GetById(ActionId);
                    if (stored != null)
                    {
                        return stored;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceAction();
            }
        }


        public DeviceProperty DevicePropertieWithId(int PropertyId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceProperty> propCont = new iotSharedEntityContext<DeviceProperty>();
                    DeviceProperty stored = propCont.GetById(PropertyId);
                    if (stored != null)
                    {
                        return stored;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceProperty();
            }
        }


        public DeviceParameter DeviceParameterWithId(int ParamId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceParameter> propCont = new iotSharedEntityContext<DeviceParameter>();
                    DeviceParameter stored = propCont.GetById(ParamId);
                    if (stored != null)
                    {
                        return stored;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceParameter();
            }
        }


        public iotDomain DomainWithId(string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);  
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);   
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


        public ActionParameter ActionParamWithId(int ActionId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<ActionParameter> propCont = new iotSharedEntityContext<ActionParameter>();
                    ActionParameter stored = propCont.GetById(ActionId);
                    if (stored != null)
                    {
                        return stored;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new ActionParameter();
            }
        }

        public DeviceCredentials DeviceCredentialWithId(int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain updated = cont.GetById(domain.Id);     //db.GetById(domain.DomainName);
                if (domain != null)
                {
                    updated = domain;
                    cont.UpdateWithHistory(domain);   //db.UpdateById(domain, domain.DomainName);
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


        public bool DeviceUpdate(Device device, int SiteId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<Device> propCont = new iotSharedEntityContext<Device>();
                    Device stored = propCont.GetById(device.Id);
                    if (stored != null)
                    {
                        propCont.UpdateWithHistory(stored);
                        return true;
                    }
                }
                return false;
               // Task updateTask = Task.Factory.StartNew(() => { DeviceUpdateEventService.SendDeviceUpdate(device); }); //Dispatch update notify
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool SiteUpdate(Site site , string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);
                if (domain != null)
                {
                    Site nsite = domain.Sites.First(s => s.Id == site.Id);
                    if (site != null)
                    {
                        nsite = site;
                        cont.UpdateWithHistory(domain);   //db.UpdateById(domain, domain.DomainName);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ActionUpdate(DeviceAction action, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);
                if (domain != null)
                {
                    
                }
                return false;
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    Device dev = site.Devices.First(d => d.Id == DeviceId);
                    DeviceProperty prop = dev.Properties.First(p => p.Id == Property.Id);
                    prop = Property;
                    cont.UpdateWithHistory(domain); //db.UpdateById(domain, domain.DomainName);
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<Location> propCont = new iotSharedEntityContext<Location>();
                    Location stored = propCont.GetById(Location.Id);
                    if (stored != null)
                    {
                        propCont.UpdateWithHistory(stored);
                        return true;
                    }
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);
                if (domain != null)
                {
                    if (param.Action != null)
                    {
                       // ActionParameter param =this.DeviceActionWithId(param.Action.Id);
                      //  repo.UpdateById(domain,domain.DomainName);
                    }
                    else
                    {
                     //   DeviceParameter param = domain..First(p => p.Id == Property.Id);
                    //    repo.UpdateById(domain,domain.DomainName);
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
        //        repo.UpdateById(domain,domain.DomainName);
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceType> propCont = new iotSharedEntityContext<DeviceType>();
                    DeviceType stored = propCont.GetById(type.Id);
                    if (stored != null)
                    {
                        propCont.UpdateWithHistory(stored);
                        return true;
                    }
                }
                return false;
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceCredentials> propCont = new iotSharedEntityContext<DeviceCredentials>();
                    DeviceCredentials stored = propCont.GetById(creds.Id);
                    if (stored != null)
                    {
                        propCont.UpdateWithHistory(stored);
                        return true;
                    }
                }
                return false;
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<EndpointInfo> propCont = new iotSharedEntityContext<EndpointInfo>();
                    EndpointInfo dev = propCont.GetById(endp.Id);
                    if (dev != null)
                    {
                        propCont.UpdateWithHistory(endp);
                        return true;
                    }
                }
                return false;
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain stored = cont.GetById(domain.Id);
                if (stored != null)
                {
                    cont.Delete(stored);
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool DeviceRemove(Device device, int SiteId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<Device> propCont = new iotSharedEntityContext<Device>();
                    Device dev = propCont.GetById(device.Id);
                    if (dev != null)
                    {
                        propCont.Delete(dev);
                        return true;
                    }
                }
                return false;
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);  //db.GetById(DomainId);
                if (domain != null)
                {
                    Site site = domain.Sites.First(s => s.Id == Site.Id);
                    domain.Sites.Remove(site);
                    cont.UpdateWithHistory(domain);   //db.UpdateById(domain, domain.DomainName);
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


        public bool ActionRemove(DeviceAction action, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId); 
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceAction> actCont = new iotSharedEntityContext<DeviceAction>();
                    DeviceAction act = actCont.GetById(action.Id);
                    if (act != null)
                    {
                        actCont.Delete(act);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool PropertyRemove(DeviceProperty property, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceProperty> propCont = new iotSharedEntityContext<DeviceProperty>();
                    DeviceProperty prop = propCont.GetById(property.Id);
                    if (prop != null)
                    {
                        propCont.Delete(prop);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool LocationRemove(Location location,  string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<Location> locCont = new iotSharedEntityContext<Location>();
                    Location loc = locCont.GetById(location.Id);
                    if (loc != null)
                    {
                        locCont.Delete(loc);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ResParamRemove(DeviceParameter param, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceParameter> locCont = new iotSharedEntityContext<DeviceParameter>();
                    DeviceParameter loc = locCont.GetById(param.Id);
                    if (loc != null)
                    {
                        locCont.Delete(loc);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ReqParamRemove(ActionParameter actparam, int DeviceId, int SiteId, string DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<ActionParameter> locCont = new iotSharedEntityContext<ActionParameter>();
                    ActionParameter loc = locCont.GetById(actparam.Id);
                    if (loc != null)
                    {
                        locCont.Delete(loc);
                        return true;
                    }
                }
                return false;
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceType> locCont = new iotSharedEntityContext<DeviceType>();
                    DeviceType loc = locCont.GetById(type.Id);
                    if (loc != null)
                    {
                        locCont.Delete(loc);
                        return true;
                    }
                }
                return false;
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<DeviceCredentials> locCont = new iotSharedEntityContext<DeviceCredentials>();
                    DeviceCredentials loc = locCont.GetById(creds.Id);
                    if (loc != null)
                    {
                        locCont.Delete(loc);
                        return true;
                    }
                }
                return false;
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    iotSharedEntityContext<EndpointInfo> locCont = new iotSharedEntityContext<EndpointInfo>();
                    EndpointInfo loc = locCont.GetById(endp.Id);
                    if (loc != null)
                    {
                        locCont.Delete(loc);
                        return true;
                    }
                }
                return false;
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

        public bool PerformDeviceAction(DeviceAction act, string DomainId)
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                        iotSharedEntityContext<iotDomain> devCont = new iotSharedEntityContext<iotDomain>();
                        Site site = domain.Sites.First(s => s.Devices.Where(d => d.EndpInfo.Id == endp.Id).Count() > 0);
                        if (site != null)
                        {
                            Device dev = site.Devices.First(d => d.EndpInfo.Id == endp.Id);
                            return dev;
                        }
                }
                return null;
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
                iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotDomain domain = cont.GetById(DomainId);
                if (domain != null)
                {
                    int LocId = int.Parse(Loc);
                    int DevTypeId = int.Parse(Type);
                    Device ndev = new Device();
                    ndev.DeviceName = Name;
                    List<DeviceType> types = domain.DeviceTypes.ToList();
                    DeviceType type = (from t in types
                                       where t.Id == DevTypeId
                                       select t).First();
                    List<Location> locs = domain.Locations.ToList();
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

                    iotSharedEntityContext<DeviceCredentials> credCont = new iotSharedEntityContext<DeviceCredentials>();
                    int credId = credCont.Add(cred);
                    DeviceCredentials storedCredentials = credCont.GetById(credId);

                    EndpointInfo info = new EndpointInfo();
                    info.Hostname = Host;
                    info.Port = int.Parse(Port);

                    //TODO
                    //CommProtocolType protocol = (CommProtocolType)Prot; //int.Parse(Prot);
                    //info.EnableProtocolSupport(protocol);
                    info.SupportsSconnProtocol = true;
                    iotSharedEntityContext<EndpointInfo> endpCont = new iotSharedEntityContext<EndpointInfo>();
                    int endpId = endpCont.Add(info);
                    EndpointInfo storedInfo = endpCont.GetById(endpId);
                    if (storedInfo == null)
                    {
                        //err
                        return null;
                    }

                    int siteIdNum = int.Parse(SiteId);
                    Site siteToAppend = domain.Sites.First(s => s.Id == siteIdNum);

                    ndev.Site = siteToAppend;
                    ndev.Credentials = storedCredentials;
                    ndev.EndpInfo = storedInfo;
                    
                    iotSharedEntityContext<Device> devCont = new iotSharedEntityContext<Device>();
                    int devId = devCont.Add(ndev);
                    Device stored = devCont.GetById(devId);

                    //update device
                    UpdateDeviceProperties(stored, stored.Site.Id, DomainId);

                    return stored;

                }
                return null;

            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return null;
            }
        }



    }
}
