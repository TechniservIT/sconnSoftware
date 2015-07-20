using iotDatabaseConnector.DAL.Repository.Runtime;
using iotDbConnector.DAL;
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

        public iotDomain DomainWithName(string name)
        {
            try
            {
               // iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotContext cont = new iotContext();
                return cont.Domains.First(d => d.DomainName.Equals(name)); 
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return null;
            }
        }


        public iotDomain GetDomainWithName(string name)
        {
            try
            {
                //iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotContext cont = new iotContext();
                return cont.Domains.First(d=>d.DomainName.Equals(name));  //InMemoryContext.GetDomainForName(name); 
                // return JsonConvert.SerializeObject(db.GetById(name));
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return null;
            }
        }


        public iotDomain GetDomainWithId(int id)
        {
            try
            {
                //iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotContext cont = new iotContext();
                return cont.Domains.First(d=> d.Id==id);
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
                //iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotContext cont = new iotContext();
                cont.Domains.Add(domain);
                cont.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }



        /******************* Add ***********************/

        public bool AddDeviceToDomainAtSite(Device dev, int DomainId, int SiteId)
        {
            try
            {
                //iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotContext cont = new iotContext();
                iotDomain domain = cont.Domains.First(d => d.Id == DomainId);   //db.GetById(DomainId);     //fetch domain from DB
                if (domain != null)
                {
                    //add to devices
                    Site site = domain.Sites.First(s => s.Id == SiteId);
                    site.Devices.Add(dev);
                    //cont.UpdateWithHistory(domain);  //update domain
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


        public bool AddSiteToDomain(Site site)
        {
            try
            {
                    iotSharedEntityContext<Site> cont = new iotSharedEntityContext<Site>();
                    cont.Add(site);
                    return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool AddActionToDeviceAtSiteInDomain(DeviceAction action)
        {
            try
            {
                iotSharedEntityContext<DeviceAction> cont = new iotSharedEntityContext<DeviceAction>();
                cont.Add(action);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool PropertyAdd(DeviceProperty prop)
        {
            try
            {
                iotSharedEntityContext<DeviceProperty> cont = new iotSharedEntityContext<DeviceProperty>();
                cont.Add(prop);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool LocationAdd(Location loc)
        {
            try
            {
                iotSharedEntityContext<Location> cont = new iotSharedEntityContext<Location>();
                cont.Add(loc);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ActionResultParamAdd(DeviceParameter param)
        {
            try
            {
                iotSharedEntityContext<DeviceParameter> cont = new iotSharedEntityContext<DeviceParameter>();
                cont.Add(param);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool PropertyResultParamAdd(DeviceParameter param)
        {
            try
            {
                iotSharedEntityContext<DeviceParameter> cont = new iotSharedEntityContext<DeviceParameter>();
                cont.Add(param);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }



        public bool ActionRequiredParamAdd(ActionParameter param)
        {
            try
            {
                iotSharedEntityContext<ActionParameter> cont = new iotSharedEntityContext<ActionParameter>();
                cont.Add(param);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }



        public bool DeviceTypeAdd(DeviceType type)
        {
            try
            {
                iotSharedEntityContext<DeviceType> cont = new iotSharedEntityContext<DeviceType>();
                cont.Add(type);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool SetDeviceCredentials(DeviceCredentials creds)
        {
            try
            {
                iotSharedEntityContext<DeviceCredentials> propCont = new iotSharedEntityContext<DeviceCredentials>();
                DeviceCredentials stored = propCont.GetById(creds.Id);
                if (stored != null)
                {
                    stored = creds;
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

        public bool SetDeviceEndpoint(EndpointInfo endp, int DomainId)
        {
            try
            {
                iotSharedEntityContext<EndpointInfo> propCont = new iotSharedEntityContext<EndpointInfo>();
                EndpointInfo stored = propCont.GetById(endp.Id);
                if (stored != null)
                {
                    stored = endp;
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


        /**************************** Get ****************************/

        public List<Site> GetSitesInDomain(int DomainId)
        {
            try
            {
                iotSharedEntityContext<Site> cont = new iotSharedEntityContext<Site>();
                List<Site> sites = cont.GetAll().Where(s => s.Domain.Id == DomainId).ToList();
                return sites;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<Site>();
            }
        }


        public List<Device> Devices()
        {
            try
            {
               iotSharedEntityContext<Device> propCont = new iotSharedEntityContext<Device>();
               List<Device> stored = propCont.GetAll().ToList();
               if (stored != null)
                    {
                        return stored;
                    }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<Device>();
            }
        }


        public List<DeviceAction> DeviceActions()
        {
            try
            {
                iotSharedEntityContext<DeviceAction> propCont = new iotSharedEntityContext<DeviceAction>();
                List<DeviceAction> stored = propCont.GetAll().ToList();
                if (stored != null)
                {
                    return stored;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<DeviceAction>();
            }
        }


        public List<DeviceProperty> DeviceProperties()
        {
            try
            {
                iotSharedEntityContext<DeviceProperty> propCont = new iotSharedEntityContext<DeviceProperty>();
                List<DeviceProperty> stored = propCont.GetAll().ToList();
                if (stored != null)
                {
                    return stored;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<DeviceProperty>();
            }
        }


        public List<DeviceParameter> DeviceParameters()
        {
            try
            {
                iotSharedEntityContext<DeviceParameter> propCont = new iotSharedEntityContext<DeviceParameter>();
                List<DeviceParameter> stored = propCont.GetAll().ToList();
                if (stored != null)
                {
                    return stored;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<DeviceParameter>();
            }
        }


        public List<Location> Locations()
        {
            try
            {
                iotSharedEntityContext<Location> propCont = new iotSharedEntityContext<Location>();
                List<Location> stored = propCont.GetAll().ToList();
                if (stored != null)
                {
                    return stored;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<Location>();
            }
        }


        public List<DeviceType> DeviceTypes()
        {
            try
            {
                iotSharedEntityContext<DeviceType> propCont = new iotSharedEntityContext<DeviceType>();
                List<DeviceType> stored = propCont.GetAll().ToList();
                if (stored != null)
                {
                    return stored;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<DeviceType>();
            }
        }


        public List<ActionParameter> ActionParams()
        {
            try
            {
                iotSharedEntityContext<ActionParameter> propCont = new iotSharedEntityContext<ActionParameter>();
                List<ActionParameter> stored = propCont.GetAll().ToList();
                if (stored != null)
                {
                    return stored;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new List<ActionParameter>();
            }
        }


        public DeviceCredentials DeviceCredentials(int CredId)
        {
            try
            {
                iotSharedEntityContext<DeviceCredentials> devCont = new iotSharedEntityContext<DeviceCredentials>();
                DeviceCredentials dev = devCont.GetById(CredId);
                if (dev != null)
                {
                    return dev;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceCredentials();
            }
        }

        public EndpointInfo DeviceEndpoint(int EndpointId)
        {
            try
            {
                iotSharedEntityContext<EndpointInfo> devCont = new iotSharedEntityContext<EndpointInfo>();
                EndpointInfo dev = devCont.GetById(EndpointId);
                if (dev != null)
                {
                    return dev;
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


        public Site SiteWithId(int SiteId)
        {
            try
            {
                iotSharedEntityContext<Site> devCont = new iotSharedEntityContext<Site>();
                Site dev = devCont.GetById(SiteId);
                if (dev != null)
                {
                    return dev;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new Site();
            }
        }


        public Device DeviceWithId(int DeviceId)
        {
            try
            {
                iotSharedEntityContext<Device> devCont = new iotSharedEntityContext<Device>();
                Device dev = devCont.GetById(DeviceId);
                if (dev != null)
                {
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


        public DeviceAction DeviceActionWithId(int ActionId)
        {
            try
            {
                iotSharedEntityContext<DeviceAction> devCont = new iotSharedEntityContext<DeviceAction>();
                DeviceAction dev = devCont.GetById(ActionId);
                if (dev != null)
                {
                    return dev;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceAction();
            }
        }


        public DeviceProperty DevicePropertieWithId(int PropertyId)
        {
            try
            {
                iotSharedEntityContext<DeviceProperty> devCont = new iotSharedEntityContext<DeviceProperty>();
                DeviceProperty dev = devCont.GetById(PropertyId);
                if (dev != null)
                {
                    return dev;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceProperty();
            }
        }


        public DeviceParameter DeviceParameterWithId(int ParamId)
        {
            try
            {
                iotSharedEntityContext<DeviceParameter> devCont = new iotSharedEntityContext<DeviceParameter>();
                DeviceParameter dev = devCont.GetById(ParamId);
                if (dev != null)
                {
                    return dev;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceParameter();
            }
        }


        public iotDomain DomainWithId(int DomainId)
        {
            try
            {
                iotSharedEntityContext<iotDomain> devCont = new iotSharedEntityContext<iotDomain>();
                iotDomain dev = devCont.GetById(DomainId);
                if (dev != null)
                {
                    return dev;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new iotDomain();
            }
        }


        public Location LocationWithId(int LocationId)
        {
            try
            {
                iotSharedEntityContext<Location> devCont = new iotSharedEntityContext<Location>();
                Location dev = devCont.GetById(LocationId);
                if (dev != null)
                {
                    return dev;
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
            try
            {
                iotSharedEntityContext<Device> propCont = new iotSharedEntityContext<Device>();
                List<Device> stored = propCont.GetAll().ToList();
                if (stored != null)
                {
                    return stored;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public DeviceType DeviceTypeWithId(int TypeId)
        {
            try
            {
                iotSharedEntityContext<DeviceType> devCont = new iotSharedEntityContext<DeviceType>();
                DeviceType dev = devCont.GetById(TypeId);
                if (dev != null)
                {
                    return dev;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceType();
            }
        }


        public ActionParameter ActionParamWithId(int ActionId)
        {
            try
            {
                iotSharedEntityContext<ActionParameter> devCont = new iotSharedEntityContext<ActionParameter>();
                ActionParameter dev = devCont.GetById(ActionId);
                if (dev != null)
                {
                    return dev;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new ActionParameter();
            }
        }

        public DeviceCredentials DeviceCredentialWithId(int CredId)
        {
            try
            {
                iotSharedEntityContext<DeviceCredentials> devCont = new iotSharedEntityContext<DeviceCredentials>();
                DeviceCredentials dev = devCont.GetById(CredId);
                if (dev != null)
                {
                    return dev;
                }
                return null;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new DeviceCredentials();
            }
        }

        public EndpointInfo EndpointWithId(int EndpId)
        {
            try
            {
                iotSharedEntityContext<EndpointInfo> devCont = new iotSharedEntityContext<EndpointInfo>();
                EndpointInfo dev = devCont.GetById(EndpId);
                if (dev != null)
                {
                    return dev;
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
                cont.UpdateWithHistory(domain);   //db.UpdateById(domain, domain.DomainName);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool DeviceUpdate(Device device)
        {
            try
            {
                iotSharedEntityContext<Device> propCont = new iotSharedEntityContext<Device>();
                propCont.UpdateWithHistory(device);
                return true;
               // Task updateTask = Task.Factory.StartNew(() => { DeviceUpdateEventService.SendDeviceUpdate(device); }); //Dispatch update notify
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool SiteUpdate(Site site)
        {
            try
            {
                iotSharedEntityContext<Site> propCont = new iotSharedEntityContext<Site>();
                propCont.UpdateWithHistory(site);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ActionUpdate(DeviceAction action)
        {
            try
            {
                iotSharedEntityContext<DeviceAction> propCont = new iotSharedEntityContext<DeviceAction>();
                propCont.UpdateWithHistory(action);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool PropertyUpdate(DeviceProperty Property)
        {
            try
            {
                iotSharedEntityContext<DeviceProperty> propCont = new iotSharedEntityContext<DeviceProperty>();
                propCont.UpdateWithHistory(Property);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool LocationUpdate(Location Location)
        {
            try
            {
                iotSharedEntityContext<Location> propCont = new iotSharedEntityContext<Location>();
                propCont.UpdateWithHistory(Location);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ResParamUpdate(DeviceParameter param)
        {
            try
            {
                iotSharedEntityContext<DeviceParameter> propCont = new iotSharedEntityContext<DeviceParameter>();
                propCont.UpdateWithHistory(param);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ReqParamUpdate(ActionParameter param)
        {
            try
            {
                iotSharedEntityContext<ActionParameter> propCont = new iotSharedEntityContext<ActionParameter>();
                propCont.UpdateWithHistory(param);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool DeviceTypeUpdate(DeviceType type)
        {
            try
            {
                iotSharedEntityContext<DeviceType> propCont = new iotSharedEntityContext<DeviceType>();
                propCont.UpdateWithHistory(type);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool DeviceCredentialsUpdate(DeviceCredentials creds)
        {
            try
            {
                iotSharedEntityContext<DeviceCredentials> propCont = new iotSharedEntityContext<DeviceCredentials>();
                propCont.UpdateWithHistory(creds);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool EndpointUpdate(EndpointInfo endp)
        {
            try
            {
                iotSharedEntityContext<EndpointInfo> propCont = new iotSharedEntityContext<EndpointInfo>();
                propCont.UpdateWithHistory(endp);
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
                iotSharedEntityContext<iotDomain> propCont = new iotSharedEntityContext<iotDomain>();
                propCont.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool DeviceRemove(Device device)
        {
            try
            {
                iotSharedEntityContext<Device> propCont = new iotSharedEntityContext<Device>();
                propCont.Delete(device);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool SiteRemove(Site Site)
        {
            try
            {
                iotSharedEntityContext<Site> propCont = new iotSharedEntityContext<Site>();
                propCont.Delete(Site);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ActionRemove(DeviceAction action)
        {
            try
            {
                iotSharedEntityContext<DeviceAction> propCont = new iotSharedEntityContext<DeviceAction>();
                propCont.Delete(action);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool PropertyRemove(DeviceProperty property)
        {
            try
            {
                iotSharedEntityContext<DeviceProperty> propCont = new iotSharedEntityContext<DeviceProperty>();
                propCont.Delete(property);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool LocationRemove(Location location)
        {
            try
            {
                iotSharedEntityContext<Location> propCont = new iotSharedEntityContext<Location>();
                propCont.Delete(location);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ResParamRemove(DeviceParameter param)
        {
            try
            {
                iotSharedEntityContext<DeviceParameter> propCont = new iotSharedEntityContext<DeviceParameter>();
                propCont.Delete(param);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool ReqParamRemove(ActionParameter actparam)
        {
            try
            {
                iotSharedEntityContext<ActionParameter> propCont = new iotSharedEntityContext<ActionParameter>();
                propCont.Delete(actparam);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool DeviceTypeRemove(DeviceType type)
        {
            try
            {
                iotSharedEntityContext<DeviceType> propCont = new iotSharedEntityContext<DeviceType>();
                propCont.Delete(type);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool DeviceCredentialsRemove(DeviceCredentials creds)
        {
            try
            {
                iotSharedEntityContext<DeviceCredentials> propCont = new iotSharedEntityContext<DeviceCredentials>();
                propCont.Delete(creds);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        public bool EndpointRemove(EndpointInfo endp)
        {
            try
            {
                iotSharedEntityContext<EndpointInfo> propCont = new iotSharedEntityContext<EndpointInfo>();
                propCont.Delete(endp);
                return true;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return false;
            }
        }


        /*********************************   IOT  QUERY / UPDATE    ********************************/

        public bool UpdateDeviceProperties(Device dev)
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

        public bool UpdateDeviceActionState(Device dev)
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

        public bool PerformDeviceAction(DeviceAction act)
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
        public Device DeviceWithEndpoint(EndpointInfo endp)
        {
            try
            {
               // iotSharedEntityContext<Device> devCont = new iotSharedEntityContext<Device>();
                iotContext cont = new iotContext();
                Device dev = cont.Devices.First(d => d.EndpInfo.Id == endp.Id);
                return dev;
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
                return new Device();
            }
        }



        /************************  ADD/REMOVE WITH PARAM ********************/

        public Device DeviceAddWithParams(string SiteId, string Name, string Host, string Port, string Login, string Pass, string Type, string Loc, string Prot, int DomainId)
        {
            try
            {
               // iotSharedEntityContext<iotDomain> cont = new iotSharedEntityContext<iotDomain>();
                iotContext cont = new iotContext();

                iotDomain domain = cont.Domains.First(d=>d.Id == DomainId);
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

                    DeviceCredentials storedCredentials = cont.Credentials.Add(cred);
                    cont.SaveChanges();

                    EndpointInfo info = new EndpointInfo();
                    info.Hostname = Host;
                    info.Port = int.Parse(Port);
                    info.Domain = domain;
                    //TODO
                    //CommProtocolType protocol = (CommProtocolType)Prot; //int.Parse(Prot);
                    //info.EnableProtocolSupport(protocol);
                    info.SupportsSconnProtocol = true;
                    EndpointInfo storedInfo = cont.Endpoints.Add(info);
                    if (storedInfo == null)
                    {
                        return null;
                    }

                    int siteIdNum = int.Parse(SiteId);
                    Site siteToAppend = domain.Sites.First(s => s.Id == siteIdNum);

                    ndev.Site = siteToAppend;
                    ndev.Credentials = storedCredentials;
                    ndev.EndpInfo = storedInfo;
                    
                    Device stored = cont.Devices.Add(ndev);

                    cont.SaveChanges();

                    //update device
                    UpdateDeviceProperties(stored);

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


        public Device DeviceAddWithParamsEntity(string SiteId, string Name, string Host, string Port, string Login, string Pass, string Type, string Loc, string Prot, int DomainId, bool IsVirtual)
        {
            try
            {
                iotContext context = new iotContext();
                iotDomain domain = context.Domains.First(d => d.Id == DomainId);
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

                    context.Credentials.Add(cred);
                    context.SaveChanges();
                    DeviceCredentials storedCredentials = context.Credentials.First(c => c.Username.Equals(cred.Username) && c.Password.Equals(cred.Password));

                    EndpointInfo info = new EndpointInfo();
                    info.Hostname = Host;
                    info.Port = int.Parse(Port);
                    info.Domain = domain;

                    //TODO
                    //CommProtocolType protocol = (CommProtocolType)Prot; //int.Parse(Prot);
                    //info.EnableProtocolSupport(protocol);
                    info.SupportsSconnProtocol = true;
                    context.Endpoints.Add(info);
                    context.SaveChanges();

                    EndpointInfo storedInfo = context.Endpoints.First(e => e.Hostname.Equals(info.Hostname) && e.Port == info.Port);
                    if (storedInfo == null)
                    {
                        return null;
                    }

                    int siteIdNum = int.Parse(SiteId);
                    Site siteToAppend = domain.Sites.First(s => s.Id == siteIdNum);

                    ndev.Site = siteToAppend;
                    ndev.Credentials = storedCredentials;
                    ndev.EndpInfo = storedInfo;

                    ndev.IsVirtual = IsVirtual;

                    context.Devices.Add(ndev);
                    context.SaveChanges();
                    Device stored = context.Devices.First(d => d.DeviceName.Equals(ndev.DeviceName) && d.EndpInfo.Hostname.Equals(ndev.EndpInfo.Hostname));

                    //update device
                    if (!ndev.IsVirtual)
                    {
                        UpdateDeviceProperties(stored);
                    }
                    else
                    {
                        DeviceFillWithSampleProperties(ndev);
                    }

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



        public void DeviceFillWithSampleProperties(Device updated)
        {

            try
            {
                iotContext cont = new iotContext();
                Device dev = cont.Devices.First(d => d.Id == updated.Id);

                dev.Properties = new List<DeviceProperty>();
                dev.Actions = new List<DeviceAction>();

                cont.SaveChanges();

                //add sample Actions
                ParameterType paramtype = new ParameterType();
                paramtype.Name = "ActionParam";
                ParameterType storedptype = cont.ParamTypes.Add(paramtype);
                cont.SaveChanges();

                for (int a = 0; a < 4; a++)
                {
                    DeviceAction act = new DeviceAction();
                    act.ActionName = "Act" + a.ToString();
                    act.Device = dev;

                    DeviceAction storedact = cont.Actions.Add(act);
                    cont.SaveChanges();
                    

                    //required params
                    ActionParameter param = new ActionParameter();
                    param.Type = storedptype;
                    param.Value = "0";
                    param.Action = act;
                    storedact.RequiredParameters = new List<ActionParameter>();
                    ActionParameter storedreqparam = cont.ActionParameters.Add(param); 
                    storedact.RequiredParameters.Add(storedreqparam);

                    cont.SaveChanges();

                    //result params
                    DeviceActionResult param2 = new DeviceActionResult();
                    param2.Type = storedptype;
                    param2.Value = "0";
                    param2.Action = act;
                    act.ResultParameters = new List<DeviceActionResult>();
                    DeviceActionResult storedresparam = cont.ActionResultParameters.Add(param2);
                    act.ResultParameters.Add(storedresparam);
                    cont.SaveChanges();

                    dev.Actions.Add(act);
                    cont.SaveChanges();
                }

                //add Sample Proprties
                for (int p = 0; p < 8; p++)
                {
                    //DeviceProperty prop = new DeviceProperty();
                    //prop.PropertyName = "Prop" + p.ToString();
                    //prop.Device = dev;

                    ////result params
                    //DeviceParameter param2 = new DeviceParameter();
                    //param2.Type = storedptype;
                    //param2.Value = "0";
                    //param2.Property = prop;
                    //prop.ResultParameters = new List<DeviceParameter>();
                    //prop.ResultParameters.Add(param2);

                    //dev.Properties.Add(prop);
                }

                cont.SaveChanges();
                
            }
            catch (Exception e)
            {
                nlogger.ErrorException(e.Message, e);
            }
        }


    }
}
