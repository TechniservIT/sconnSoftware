using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace iotDbConnector.DAL
{
    

    public class iotDomain : IFakeAbleEntity
    {

         
        [Key]
        [Required]
        public int Id { get; set; }

         
        [Required]
        [DisplayName("Name")]
        public string DomainName { get; set; }

         
        public virtual List<Site> Sites { get; set; }

         
        public virtual List<Location> Locations { get; set; }

         
        public virtual List<DeviceType> DeviceTypes { get; set; }


         
        public virtual List<EndpointInfo> Endpoints { get; set; }


        public iotDomain()
        {
            if (this.Locations == null)
            {
                this.Locations = new List<Location>();
            }
            if (this.DeviceTypes == null)
            {
                this.DeviceTypes = new List<DeviceType>();
            }
            if (this.Sites == null)
            {
                this.Sites = new List<Site>();
            }
            if (this.Endpoints == null)
            {
                this.Endpoints = new List<EndpointInfo>();
            }
        }

        public void Fake()
        {
            this.DomainName = Guid.NewGuid().ToString();
        }
    }

    public interface IFakeAbleEntity
    {
        void Fake();
    }
}