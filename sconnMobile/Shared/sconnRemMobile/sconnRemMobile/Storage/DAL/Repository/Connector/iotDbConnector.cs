using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Data;
using System.Security.Policy;
using Mono.CSharp;
using sconnConnector.POCO.Device;

namespace iotDbConnector.DAL
{
    /* TODO specific context queries */

    public class iotConnector
    {

        private iotContext context;

        public iotConnector()
        {
            context = new iotContext();
        }

       /**************************** Query ****************************/


        /************** Domain ************/
        public iotDomain DomainForDomainName(string name)
        {
            try
            {
                iotContext db = new iotContext();
                
                    iotDomain domain = (from d in db.Domains
                                        where d.DomainName == name
                                        select d).First();
                    return domain;
                
            }
            catch (Exception e)
            {
                return new iotDomain();
            }

        }


        public iotDomain DomainForDomainId(int domainId)
        {
            try
            {
                    iotContext db = new iotContext();            
                    iotDomain domain = (from d in db.Domains
                                        where d.Id.Equals( domainId)
                                        select d).First();
                    return domain;     
            }
            catch (Exception e)
            {
                return new iotDomain();
            }

        }

        public   List<iotDomain> DomainList()
        {
            iotContext db = new iotContext();
            
                List<iotDomain> domains =   (from d in db.Domains
                                          select d).ToList();
                return domains;
            
        }

       


        /************** Site ************/
        public   List<Site> SiteList()
        {
            iotContext db = new iotContext();
            
                List<Site> sites =   (from d in db.Sites
                                                 select d).ToList();
                return sites;
            
        }

        /************** Device ************/
        public   List<Device> DeviceList()
        {
            iotContext db = new iotContext();
            
                List<Device> devs =   (from d in db.Devices
                                          select d).ToList();
                return devs;
            
        }


        /************** Action ************/
        public   List<DeviceAction> ActionList()
        {
            iotContext db = new iotContext();
            
                List<DeviceAction> actions =   (from d in db.Actions
                                           select d).ToList();
                return actions;
            
        }


        /************** Property ************/
        public   List<DeviceProperty> PropertyList()
        {
            iotContext db = new iotContext();
            
                List<DeviceProperty> props =   (from d in db.Properties
                                                    select d).ToList();
                return props;
            
        }


        /************** Parameter ************/
        public   List<DeviceParameter> ParameterList()
        {
            iotContext db = new iotContext();
            
                List<DeviceParameter> devparams =   (from d in db.Parameters
                                                    select d).ToList();
                return devparams;
            
        }

        /************** Location ************/
        public   List<Location> LocationList()
        {
            iotContext db = new iotContext();
            
                List<Location> locs =   (from d in db.Locations
                                                         select d).ToList();
                return locs;
            
        }


        /************** Endpoint ************/
        public   List<EndpointInfo> EndpInfoList()
        {
            iotContext db = new iotContext();
            
                List<EndpointInfo> endp =   (from d in db.Endpoints
                                             select d).ToList();
                return endp;
            
        }

        /************** Type ************/
        public   ParameterType TypeForName(string name)
        {
            try
            {
                iotContext db = new iotContext();
                List<ParameterType> types = (from d in db.ParamTypes
                                      where d.Name.Equals(name)
                                      select d).ToList();

                if (types.Count == 0)
                {
                    //add type
                    ParameterType type = new ParameterType();
                    type.Name = name;
                    ParameterType storedType =  db.ParamTypes.Add(type);
                    db.SaveChanges();
                    return storedType;
                }
                else
                {
                    return types.First();
                }

            }
            catch (Exception e)
            {
                return new ParameterType();
            
            }
          
        }

        public   List<ParameterType> ParamTypeList()
        {
            iotContext db = new iotContext();
            
                List<ParameterType> types =   (from d in db.ParamTypes
                                                 select d).ToList();
                return types;
            
        }


        /************** Mapper ************/
        public List<sconnActionResultMapper> MappersList()
        {
            iotContext db = new iotContext();

            List<sconnActionResultMapper> mappers = (from d in db.ActionResultMappers
                                                   select d).ToList();
                return mappers;
            
        }


        /************** Action params ************/
        public   List<ActionParameter> ActionParamList()
        {
            iotContext db = new iotContext();
            
                List<ActionParameter> types =   (from d in db.ActionParameters
                                                 select d).ToList();
                return types;
            
        }


        /************** Device types ************/
        public   List<DeviceType> DevTypesList()
        {
            iotContext db = new iotContext();
            
                List<DeviceType> mappers =   (from d in db.Types
                                                   select d).ToList();
                return mappers;
            
        }

        


        /**************************** Execute ****************************/



        /************** Domain ************/
        public  bool DomainAdd(iotDomain domain)
        {
            try
            {
                if (domain != null)
                {
                    iotContext db = new iotContext();
                    
                        db.Domains.Add(domain);
                          db.SaveChanges();
                        //TODO verify save
                    
                }

            }
            catch (Exception e)
            {
            }
            return true;
        }



        /************** Site ************/

        public bool SiteAdd(Site site)
        {
            try
            {
                iotContext db = new iotContext();

                iotDomain targetDomain = (from d in db.Domains
                                          where site.Domain.Id == d.Id
                                          select d).First();
                Location siteLocation = (from l in db.Locations
                                         where l.Id == site.siteLocation.Id
                                         select l).First();

                Site nsite = new Site();
                nsite.SiteName = site.SiteName;
                nsite.Domain = targetDomain;
                nsite.siteLocation = siteLocation;
                db.Sites.Add(nsite);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        /************** Device ************/
        public  bool DeviceAdd(Device device)
        {
            try
            {
                if (device != null)
                {
                    iotContext db = new iotContext();
                    
                        db.Devices.Add(device);
                          db.SaveChanges();
                        //TODO verify save
                    
                }

            }
            catch (Exception e)
            {
            }
            return true;
        }



        /************** Action ************/
        public  bool ActionAdd(DeviceAction action)
        {
            try
            {
                if (action != null)
                {
                    iotContext db = new iotContext();
                    
                        db.Actions.Add(action);
                          db.SaveChanges();
                        //TODO verify save
                    
                }

            }
            catch (Exception e)
            {
            }
            return true;
        }


        /************** Property ************/
        public  bool PropertyAdd(DeviceProperty property)
        {
            try
            {
                if (property != null)
                {
                    iotContext db = new iotContext();
                    
                        db.Properties.Add(property);
                          db.SaveChanges();
                        //TODO verify save
                    
                }

            }
            catch (Exception e)
            {
            }
            return true;
        }


        /************** Parameter ************/
        public  bool ParameterAdd(DeviceParameter param)
        {
            try
            {
                if (param != null)
                {
                    iotContext db = new iotContext();
                    
                        db.Parameters.Add(param);
                          db.SaveChanges();
                        //TODO verify save
                    
                }

            }
            catch (Exception e)
            {
            }
            return true;
        }


        /************** Location ************/
        public  bool LocationAdd(Location loc)
        {
            try
            {
                if (loc != null)
                {
                    iotContext db = new iotContext();
                    
                        db.Locations.Add(loc);
                          db.SaveChanges();
                        //TODO verify save
                    
                }

            }
            catch (Exception e)
            {
            }
            return true;
        }

        /************** Endpoint ************/
        public   bool EndpointAdd(EndpointInfo edp)
        {
            try
            {
                if (edp != null)
                {
                    iotContext db = new iotContext();
                    
                        db.Endpoints.Add(edp);
                          db.SaveChanges();
                        //TODO verify save
                    
                }

            }
            catch (Exception e)
            {
            }
            return true;
        }

        /************** Type ************/
        public  bool TypeAdd(DeviceType param)
        {
            try
            {
                if (param != null)
                {
                    iotContext db = new iotContext();
                    
                        db.Types.Add(param);
                          db.SaveChanges();
                        //TODO verify save
                    
                }

            }
            catch (Exception e)
            {
            }
            return true;
        }


        /************** Mapper ************/
        public bool MapperAdd(sconnActionResultMapper param)
        {
            try
            {
                if (param != null)
                {
                    iotContext db = new iotContext();
                    
                        db.ActionResultMappers.Add(param);
                          db.SaveChanges();
                        //TODO verify save
                    
                }

            }
            catch (Exception e)
            {
            }
            return true;
        }

        /************** Action param ************/
        public  bool ActionParamAdd(ActionParameter param)
        {
            try
            {
                if (param != null)
                {
                    iotContext db = new iotContext();
                    
                        db.ActionParameters.Add(param);
                          db.SaveChanges();
                       //TODO verify save
                    
                }

            }
            catch (Exception e)
            {
            }
            return true;
        }


        /**************************** Update ****************************/

        /************** Domain ************/

        /************** Site ************/

        /************** Device ************/

        /************** Action ************/

        /************** Property ************/

        /************** Parameter ************/

        /************** Location ************/

        /************** Endpoint ************/

        /************** Type ************/

        /* sconnMapper */

        /* Action Param */




    }
}