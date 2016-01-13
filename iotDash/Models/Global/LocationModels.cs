using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        [DisplayName("Location")]
        public Location Location { get; set; }

        public string Result { get; set; }

        public LocationEditViewModel(Location loc)
        {
            Location = loc;
        }
    }

    public class LocatioAddModel : IAsyncStatusModel
    {
        [Required]
        [DisplayName("Location")]
        public Location Location { get; set; }
        public string Result { get; set; }

        public LocatioAddModel()
        {
            Location = new Location();
        }
    }


}