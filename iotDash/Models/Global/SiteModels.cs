using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using iotDbConnector.DAL;

namespace iotDash.Models
{

    public class ShowSitesViewModel : IAsyncStatusModel
    {
        public List<Site> Sites { get; set; }
        public string Result { get; set; }

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

        [Required]
        [DisplayName("Location")]
        public int LocationId { get; set; }

        [Required]
        [DisplayName("Site")]
        public Site site { get; set; }

        public string Result { get; set; }

        public AddSiteViewModel()
        {
            this.site = new Site();
        }

        public AddSiteViewModel(List<Location> locs) : this()
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