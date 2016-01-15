using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AlarmSystemManagmentService.Device;
using iotDash.Areas.AlarmSystem.Models;
using iotDash.Identity.Attributes;
using iotDash.Session;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using NLog;
using sconnConnector.Config;


namespace iotDash.RealTime.SignalR.Maps.AlarmSystemMapController
{
    [DomainAuthorize]
    public class AlarmSystemMapHub : Hub
    {
        private IIotContextBase cont;
        private Dictionary<string, AlarmSystemConfigManager> MapClientSessions;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public AlarmSystemMapHub(IIotContextBase context)
        {
            this.cont = context;
        }

        public AlarmSystemMapEditModel GetMapEditModel(int ServerId)
        {
            AlarmSystemMapEditModel model = new AlarmSystemMapEditModel();
            try
            {
                string clientId = Context.ConnectionId;
                if (MapClientSessions.ContainsKey(clientId))
                {
                    AlarmSystemConfigManager cfg = new AlarmSystemConfigManager();
                    MapClientSessions.TryGetValue(clientId, out cfg);
                    if (cfg != null)
                    {
                        AlarmDevicesConfigService deviceprovider = new AlarmDevicesConfigService(cfg);
                        model = new AlarmSystemMapEditModel(deviceprovider.GetAll());
                    }
                }
                else
                {
                    //find server 
                    var serv = cont.Devices.FirstOrDefault(d => d.Id == ServerId);
                    if (serv != null)
                    {
                        //get alarm service 
                        AlarmSystemConfigManager cfg = GetAlarmConfigForContextWithDeviceId(ServerId);
                        AlarmDevicesConfigService deviceprovider = new AlarmDevicesConfigService(cfg);
                        model = new AlarmSystemMapEditModel(deviceprovider.GetAll());
                        MapClientSessions.Add(Context.ConnectionId, cfg);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
            return model;
        }


        public JsonResult UpdateAlarmMap(AlarmSystemMapEditModel model)
        {
            var clientId = Context.ConnectionId;
            if (MapClientSessions.ContainsKey(clientId))
            {
                AlarmSystemConfigManager cfg = new AlarmSystemConfigManager();
                MapClientSessions.TryGetValue(clientId, out cfg);
                if (cfg != null)
                {
                    AlarmDevicesConfigService deviceprovider = new AlarmDevicesConfigService(cfg);
                    //update map into database
                    if (model.Map != null)
                    {
                        var existingMap = cont.MapDefinitions.FirstOrDefault(m => m.Id == model.Map.Id);
                        if (existingMap != null)
                        {
                            existingMap.Device = model.Map.Device;
                            existingMap.DeviceMaps = model.Map.DeviceMaps;
                            existingMap.Url = model.Map.Url;
                        }
                        else
                        {
                            cont.MapDefinitions.Add(model.Map);
                        }
                        cont.SaveChanges();
                    }
                    model = new AlarmSystemMapEditModel(deviceprovider.GetAll());
                    MapClientSessions.Add(Context.ConnectionId, cfg);
                }
            }
            return new JsonResult();
        }

        public void PublishMapUpdated(DeviceActionResult param)
        {
            try
            {
                IIotContextBase cont = new iotContext();
                DeviceActionResult toUpdate = cont.ActionResultParameters.Include("Action").FirstOrDefault(e => e.Id == param.Id);

                string jsonParam = JsonConvert.SerializeObject(toUpdate);
                Clients.All.updateParam(jsonParam);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }



        /*************  Alarm config session **************/

        private AlarmSystemConfigManager GetAlarmConfigForContextWithDeviceId(int devid)
        {
            try
            {
                Device alrmSysDev = cont.Devices.First(d => d.Id == devid);
                if (alrmSysDev != null)
                {
                    var man = new AlarmSystemConfigManager(alrmSysDev.EndpInfo, alrmSysDev.Credentials);
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


        //public override Task OnConnected()
        //{
        //    var clientId = Context.ConnectionId;
        //    if (!MapClientSessions.ContainsKey(clientId))
        //    {
        //        AlarmSystemConfigManager cfg = new AlarmSystemConfigManager();
        //        MapClientSessions.Add(Context.ConnectionId, cfg);
        //    }
        //    return base.OnConnected();
        //}

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

