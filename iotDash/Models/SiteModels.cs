using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using iotDbConnector.DAL;

namespace iotDash.Models
{

    public class ShowSitesViewModel
    {
        public List<Site> Sites { get; set; }
        public ShowSitesViewModel()
        {

        }

        public ShowSitesViewModel(List<Site> sites)
        {
            Sites = sites;
        }

    }


    public class AddSiteViewModel
    {
        public List<Location> Locations { get; set; }

        public AddSiteViewModel()
        {

        }

        public AddSiteViewModel(List<Location> locs)
        {
            Locations = locs;
        }

        [Required]
        [Display(Name = "Name")]
        public string SiteName { get; set; }

        [Required]
        [Display(Name = "Location")]
        public Location SiteLocation { get; set; }



    }

}