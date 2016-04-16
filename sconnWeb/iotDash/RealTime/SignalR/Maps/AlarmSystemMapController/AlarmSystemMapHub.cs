using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlarmSystemManagmentService.Device;
using iotDash.Areas.AlarmSystem.Models;
using iotDash.Helpers;
using iotDash.Identity.Attributes;
using iotDash.Session;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using NLog;
using sconnConnector.Config;


namespace iotDash.RealTime.SignalR.Maps.AlarmSystemMapController
{
    [DomainAuthorize]
    public class AlarmSystemMapHub : Hub
    {

        private Dictionary<string, AlarmSystemConfigManager> MapClientSessions;
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        
        public AlarmSystemMapHub()
        {
            MapClientSessions = new Dictionary<string, AlarmSystemConfigManager>();
        }

        public void GetMapData(int ServerId)
        {
            try
            {
                IIotContextBase cont = new iotContext();
                var serv = cont.Devices.FirstOrDefault(d => d.Id == ServerId);
                if (serv != null)   //correct server
                {
                    AlarmSystemMapEditModel model = new AlarmSystemMapEditModel();
                    string clientId = Context.ConnectionId;
                    if (DomainAuthHelper.UserHasDeviceAccess(serv, Context.User))   //authorized
                    {
                        var man = new AlarmSystemConfigManager(serv);
                        AlarmDevicesConfigService deviceprovider = new AlarmDevicesConfigService(man);
                        model = new AlarmSystemMapEditModel(deviceprovider.GetAll());
                        model.ServerId = ServerId;
                        var fMapDefinition = serv.DeviceMaps.FirstOrDefault();
                        fMapDefinition.Device = null;
                        foreach (var d in fMapDefinition.IoMapDefinitions)
                        {
                            d.Definition = null; //unbind
                        }
                        model.MapDefinition = fMapDefinition;
                        Clients.Client(Context.ConnectionId).UpdateMap(model);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }
        

        public JsonResult UpdateAlarmMap(AlarmSystemMapEditModel model)
        {
            try
            {
                IIotContextBase cont = new iotContext();
                Device dev = cont.Devices.FirstOrDefault(d => d.Id == model.ServerId);
                if (dev != null)
                {
                    if (DomainAuthHelper.UserHasDeviceAccess(dev, Context.User)) //authorized
                    {
                        if (dev.DeviceMaps.Count > 0)
                        {
                            //find adj map and update
                            var existing = dev.DeviceMaps.FirstOrDefault(m => m.Id == model.MapDefinition.Id);
                            if (existing != null)
                            {
                                //bind relations 
                                foreach (var iomd in model.MapDefinition.IoMapDefinitions)
                                {
                                    try
                                    {
                                        var existingMapDef = existing.IoMapDefinitions.FirstOrDefault(d => d.Id == iomd.Id);
                                        if (existingMapDef != null)
                                        {
                                            existingMapDef.Copy(iomd);
                                        }
                                        else
                                        {
                                            iomd.Definition = existing;
                                            cont.IoMapDefinitions.Add(iomd);
                                            cont.SaveChanges();
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.Error(e, e.Message);
                                    }
                                }
                                cont.SaveChanges();
                            }
                            else
                            {
                                //has map and new one sent ? todo : ovewrite conditionally
                            }
                        }
                        else
                        {
                            //add the map
                            dev.DeviceMaps.Add(model.MapDefinition);
                            cont.SaveChanges();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            } 
            return new JsonResult();
        }


        /*************  Alarm config session **************/

        private AlarmSystemConfigManager GetAlarmConfigForContextWithDeviceId(int devid)
        {
            try
            {
                IIotContextBase cont = new iotContext();
                Device alrmSysDev = cont.Devices.First(d => d.Id == devid);
                if (alrmSysDev != null)
                {
                    var man = new AlarmSystemConfigManager(alrmSysDev.EndpInfo, alrmSysDev.Credentials);
                    man.RemoteDevice = alrmSysDev;
                    return man;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }

        }
        
        public override Task OnDisconnected(bool stopCalled)
        {
            var clientId = Context.ConnectionId;
            if (MapClientSessions.ContainsKey(clientId))
            {
                MapClientSessions.Remove(clientId);
            }
            return base.OnDisconnected(stopCalled);
        }




    }
}

