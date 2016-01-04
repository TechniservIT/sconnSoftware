using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iotDash.Models
{
    public class LocationListViewModel : IAsyncStatusModel
    {
        public List<Location> Locations { get; set; }
        public string Result { get; set; }

        public LocationListViewModel(List<Location> locations)
        {
            Locations = locations;
        }
    }

    public class LocationEditViewModel : IAsyncStatusModel
    {
        public Location Location { get; set; }
        public string Result { get; set; }

        public LocationEditViewModel(Location loc)
        {
            Location = loc;
        }
    }

}