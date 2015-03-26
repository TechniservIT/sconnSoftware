using iotDatabaseConnector.DAL.POCO.Device.Notify;
using iotDbConnector.DAL;
using iotServiceProvider.NET.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace iotServiceProvider
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
  
    
    public class iotDomainService : IiotDomainService
    {

        public iotDomain GetDomainWithName(string name)
        {
            try
            {
                iotConnector connector = new iotConnector();
                return connector.DomainForDomainName(name);
            }
            catch (Exception e)
            {
                return new iotDomain();
            }
        }


        public iotDomain GetDomainWithId(int id) 
        {
            try
            {
                iotRepository<iotDomain> repo = new iotRepository<iotDomain>();
                return repo.GetById(id);
            }
            catch (Exception e)
            {
                return new iotDomain();
            }            
        }


        public bool DomainAdd(iotDomain domain)
        {
            try
            {
              
                iotRepository<iotDomain> repo = new iotRepository<iotDomain>();
                repo.Add(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        /******************* Add ***********************/

        public bool DeviceAdd(Device dev)
        {
            try
            {
                iotRepository<Device> repo = new iotRepository<Device>();
                repo.Add(dev);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        
        }


        public bool SiteAdd(Site site)
        {
            try
            {
                iotRepository<Site> repo = new iotRepository<Site>();
                repo.Add(site);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool ActionAdd(DeviceAction action)
        {
            try
            {
                iotRepository<DeviceAction> repo = new iotRepository<DeviceAction>();
                repo.Add(action);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool PropertyAdd(DeviceProperty prop)
        {
            try
            {
                iotRepository<DeviceProperty> repo = new iotRepository<DeviceProperty>();
                repo.Add(prop);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool LocationAdd(Location loc)
        {
            try
            {
                iotRepository<Location> repo = new iotRepository<Location>();
                repo.Add(loc);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool ResParamAdd(DeviceParameter param)
        {
            try
            {
                iotRepository<DeviceParameter> repo = new iotRepository<DeviceParameter>();
                repo.Add(param);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool ReqParamAdd(ActionParameter param)
        {
            try
            {
                iotRepository<ActionParameter> repo = new iotRepository<ActionParameter>();
                repo.Add(param);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool DeviceTypeAdd(DeviceType type)
        {
            try
            {
                iotRepository<DeviceType> repo = new iotRepository<DeviceType>();
                repo.Add(type);
                return true;        
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool DeviceCredentialsAdd(DeviceCredentials creds)
        {
            try
            {
                iotRepository<DeviceCredentials> repo = new iotRepository<DeviceCredentials>();
                repo.Add(creds);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool EndpointAdd(EndpointInfo endp)
        {
            try
            {
                iotRepository<EndpointInfo> repo = new iotRepository<EndpointInfo>();
                repo.Add(endp);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        /**************************** Get ****************************/


        public List<iotDomain> Domains()
        {
            try
            {
                iotRepository<iotDomain> repo = new iotRepository<iotDomain>();
                List<iotDomain> domains = repo.GetAll().ToList();
                return domains;
                
            }
            catch (Exception e)
            {
                return new List<iotDomain>();
            }
        }


        public List<Site> Sites()
        {
            try
            {
                iotContext cont = new iotContext();
                List<Site> sites = (from s in cont.Sites
                                    select s).ToList();
                return sites;
            }
            catch (Exception e)
            {
                return new List<Site>();
            }
        }


        public List<Device> Devices()
        {
            try
            {
                iotRepository<Device> repo = new iotRepository<Device>();
                List<Device> devs = repo.GetAll().ToList();
                return devs;
            }
            catch (Exception e)
            {
                return new List<Device>();
            }
        }


        public List<DeviceAction> DeviceActions()
        {
            try
            {
                iotRepository<DeviceAction> repo = new iotRepository<DeviceAction>();
                List<DeviceAction> acts = repo.GetAll().ToList();
                return acts;
            }
            catch (Exception e)
            {
                return new List<DeviceAction>();
            }
        }


        public List<DeviceProperty> DeviceProperties()
        {
            try
            {
                iotRepository<DeviceProperty> repo = new iotRepository<DeviceProperty>();
                List<DeviceProperty> props = repo.GetAll().ToList();
                return props;
            }
            catch (Exception e)
            {
                return new List<DeviceProperty>();
            }
        }


        public List<DeviceParameter> DeviceParameters()
        {
            try
            {
                iotRepository<DeviceParameter> repo = new iotRepository<DeviceParameter>();
                List<DeviceParameter> devpar = repo.GetAll().ToList();
                return devpar;
            }
            catch (Exception e)
            {
                return new List<DeviceParameter>();
            }
        }


        public List<Location> Locations()
        {
            try
            {
                iotRepository<Location> repo = new iotRepository<Location>();
                List<Location> locs = repo.GetAll().ToList();
                return locs;
            }
            catch (Exception e)
            {
                return new List<Location>();
            }
        }


        public List<DeviceType> DeviceTypes()
        {
            try
            {
                iotRepository<DeviceType> repo = new iotRepository<DeviceType>();
                List<DeviceType> devt = repo.GetAll().ToList();
                return devt;
            }
            catch (Exception e)
            {
                return new List<DeviceType>();
            }
        }


        public List<ActionParameter> ActionParams()
        {
            try
            {
                iotRepository<ActionParameter> repo = new iotRepository<ActionParameter>();
                List<ActionParameter> acts = repo.GetAll().ToList();
                return acts;
            }
            catch (Exception e)
            {
                return new List<ActionParameter>();
            }
        }


        public List<DeviceCredentials> DeviceCredentials()
        {
            try
            {
                iotRepository<DeviceCredentials> repo = new iotRepository<DeviceCredentials>();
                List<DeviceCredentials> creds = repo.GetAll().ToList();
                return creds;
            }
            catch (Exception e)
            {
                return new List<DeviceCredentials>();
            }
        }

        public List<EndpointInfo> Endpoints()
        {
            try
            {
                iotRepository<EndpointInfo> repo = new iotRepository<EndpointInfo>();
                List<EndpointInfo> endp = repo.GetAll().ToList();
                return endp;
            }
            catch (Exception e)
            {
                return new List<EndpointInfo>();
            }
        }


        /************** Get by id *****************/


        public Site SiteWithId(int id)
        {
            try
            {
                //iotRepository<Site> repo = new iotRepository<Site>();
                //return repo.GetById(id);
                iotConnector connector = new iotConnector();
                return connector.SiteList().Where(s => { return s.SiteId == id; }).First();
            }
            catch (Exception e)
            {
                return new Site();
            }
        }


        public Device DeviceWithId(int id)
        {
            try
            {
                /*
                iotRepository<Device> repo = new iotRepository<Device>();
                return repo.GetById(id);
                 */
                iotConnector connector = new iotConnector();
                return connector.DeviceList().Where(s => { return s.DeviceId == id; }).First();
            }
            catch (Exception e)
            {
                return new Device();
            }
        }


        public DeviceAction DeviceActionWithId(int id)
        {
            try
            {
                iotRepository<DeviceAction> repo = new iotRepository<DeviceAction>();
                return repo.GetById(id);
            }
            catch (Exception e)
            {
                return new DeviceAction();
            }
        }


        public DeviceProperty DevicePropertieWithId(int id)
        {
            try
            {
                iotRepository<DeviceProperty> repo = new iotRepository<DeviceProperty>();
                return repo.GetById(id);
            }
            catch (Exception e)
            {
                return new DeviceProperty();
            }
        }


        public DeviceParameter DeviceParameterWithId(int id)
        {
            try
            {
                iotRepository<DeviceParameter> repo = new iotRepository<DeviceParameter>();
                return repo.GetById(id);
            }
            catch (Exception e)
            {
                return new DeviceParameter();
            }
        }


        public iotDomain DomainWithId(int id)
        {
            try
            {
                iotRepository<iotDomain> repo = new iotRepository<iotDomain>();
                return repo.GetById(id);
            }
            catch (Exception e)
            {
                return new iotDomain();
            }
        }


        public Location LocationWithId(int id)
        {
            try
            {
                iotRepository<Location> repo = new iotRepository<Location>();
                return repo.GetById(id);
               // iotConnector connector = new iotConnector();
               // return connector.LocationList().Where(s => { return s.LocationId == id; }).First();
            }
            catch (Exception e)
            {
                return new Location();
            }
        }


        public DeviceType DeviceTypeWithId(int id)
        {
            try
            {
                iotRepository<DeviceType> repo = new iotRepository<DeviceType>();
                return repo.GetById(id);
            }
            catch (Exception e)
            {
                return new DeviceType();
            }
        }


        public ActionParameter ActionParamWithId(int id)
        {
            try
            {
                iotRepository<ActionParameter> repo = new iotRepository<ActionParameter>();
                return repo.GetById(id);
            }
            catch (Exception e)
            {
                return new ActionParameter();
            }
        }

        public DeviceCredentials DeviceCredentialWithId(int id)
        {
            try
            {
                iotRepository<DeviceCredentials> repo = new iotRepository<DeviceCredentials>();
                return repo.GetById(id);
            }
            catch (Exception e)
            {
                return new DeviceCredentials();
            }
        }

        public EndpointInfo EndpointWithId(int id)
        {
            try
            {
                iotRepository<EndpointInfo> repo = new iotRepository<EndpointInfo>();
                return repo.GetById(id);
            }
            catch (Exception e)
            {
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
                return false;
            }
        }

        
        public bool DeviceUpdate(Device device)
        {
            try
            {


                iotRepository<Device> repo = new iotRepository<Device>();
                repo.Update(device);
                Task updateTask = Task.Factory.StartNew(() => { DeviceUpdateEventService.SendDeviceUpdate(device); }); //Dispatch update notify
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool SiteUpdate(Site domain)
        {
            try
            {
                iotRepository<Site> repo = new iotRepository<Site>();
                repo.Update(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool ActionUpdate(DeviceAction domain)
        {
            try
            {
                iotRepository<DeviceAction> repo = new iotRepository<DeviceAction>();
                repo.Update(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool PropertyUpdate(DeviceProperty domain)
        {
            try
            {
                iotRepository<DeviceProperty> repo = new iotRepository<DeviceProperty>();
                repo.Update(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool LocationUpdate(Location domain)
        {
            try
            {
                iotRepository<Location> repo = new iotRepository<Location>();
                repo.Update(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool ResParamUpdate(DeviceParameter domain)
        {
            try
            {
                iotRepository<DeviceParameter> repo = new iotRepository<DeviceParameter>();
                repo.Update(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool ReqParamUpdate(ActionParameter domain)
        {
            try
            {
                iotRepository<ActionParameter> repo = new iotRepository<ActionParameter>();
                repo.Update(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool DeviceTypeUpdate(DeviceType type)
        {
            try
            {
                iotRepository<DeviceType> repo = new iotRepository<DeviceType>();
                repo.Update(type);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool DeviceCredentialsUpdate(DeviceCredentials creds)
        {
            try
            {
                iotRepository<DeviceCredentials> repo = new iotRepository<DeviceCredentials>();
                repo.Update(creds);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool EndpointUpdate(EndpointInfo endp)
        {
            try
            {
                iotRepository<EndpointInfo> repo = new iotRepository<EndpointInfo>();
                repo.Update(endp);
                return true;
            }
            catch (Exception e)
            {
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
                return false;
            }
        }

        
        public bool DeviceRemove(Device domain)
        {
            try
            {
                iotRepository<Device> repo = new iotRepository<Device>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool SiteRemove(Site domain)
        {
            try
            {
                iotRepository<Site> repo = new iotRepository<Site>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool ActionRemove(DeviceAction domain)
        {
            try
            {
                iotRepository<DeviceAction> repo = new iotRepository<DeviceAction>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool PropertyRemove(DeviceProperty domain)
        {
            try
            {
                iotRepository<DeviceProperty> repo = new iotRepository<DeviceProperty>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool LocationRemove(Location domain)
        {
            try
            {
                iotRepository<Location> repo = new iotRepository<Location>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool ResParamRemove(DeviceParameter domain)
        {
            try
            {
                iotRepository<DeviceParameter> repo = new iotRepository<DeviceParameter>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool ReqParamRemove(ActionParameter domain)
        {
            try
            {
                iotRepository<ActionParameter> repo = new iotRepository<ActionParameter>();
                repo.Delete(domain);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool DeviceTypeRemove(DeviceType type)
        {
            try
            {
                iotRepository<DeviceType> repo = new iotRepository<DeviceType>();
                repo.Delete(type);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool DeviceCredentialsRemove(DeviceCredentials creds)
        {
            try
            {
                iotRepository<DeviceCredentials> repo = new iotRepository<DeviceCredentials>();
                repo.Delete(creds);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        
        public bool EndpointRemove(EndpointInfo endp)
        {
            try
            {
                iotRepository<EndpointInfo> repo = new iotRepository<EndpointInfo>();
                repo.Delete(endp);
                return true;
            }
            catch (Exception e)
            {
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
                DeviceUpdateEventService.SendDeviceUpdate(act.Device);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }




        /************************  CUSTOM CROSS QUERY ********************/
        [OperationContract]
        [ApplyDataContractResolver]
        public Device DeviceWithEndpoint(EndpointInfo endp)
        {
            try
            {
                iotContext cont = new iotContext();
                Device stored = cont.Devices.Where(d => d.EndpInfo.Hostname.Equals(endp.Hostname) && d.EndpInfo.Port == endp.Port).FirstOrDefault();
                return stored;
            }
            catch (Exception e)
            {
                return new Device();
            }
        }


    }
}
