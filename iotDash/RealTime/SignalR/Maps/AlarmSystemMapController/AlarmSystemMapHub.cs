using System.Collections.Generic;
using System.Linq;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;


namespace iotDash.RealTime.SignalR.Maps.AlarmSystemMapController
{
    public class DeviceStatusUpdaterHub : Hub
    {
        private IIotContextBase cont;


        public void PublishActionUpdate(DeviceActionResult param)
        {
            IIotContextBase cont = new iotContext();
            DeviceActionResult toUpdate = cont.ActionResultParameters.Include("Action").FirstOrDefault(e => e.Id == param.Id);
            
            string jsonParam = JsonConvert.SerializeObject(toUpdate);
            Clients.All.updateParam(jsonParam);

        }


        public void PublishParamUpdate(DeviceParameter param)
        {
            IIotContextBase cont = new iotContext();

            DeviceParameter toUpdate = cont.Parameters.Include("Property").FirstOrDefault(e => e.Id == param.Id);
            
            string jsonParam = JsonConvert.SerializeObject(toUpdate);
            Clients.All.updateParam(jsonParam);

        }


    }
}

