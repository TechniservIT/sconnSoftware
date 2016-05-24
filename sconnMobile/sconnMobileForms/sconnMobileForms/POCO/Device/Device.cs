using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace sconnConnector.POCO.Device
{
 
    public class Device
    {
        private int _Id;
        
        [Required]
        [Key]
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }

        public void Load(Device dev)
        {
            this.Credentials = dev.Credentials;
            this.DeviceName = dev.DeviceName;
            this.EndpInfo = dev.EndpInfo;
        }

         
        [Required]
        [DisplayName("Name")]
        public string DeviceName { get; set; }

         
        [Required]
        [DisplayName("Endpoint")]
        public virtual EndpointInfo EndpInfo { get; set; }

         
        [DisplayName("Credentials")]
        public virtual DeviceCredentials Credentials { get; set; }
        

         
        [DisplayName("Virtual")]
        public bool IsVirtual { get; set; }


        public void Fake()
        {
            DeviceName = Guid.NewGuid().ToString();
        }
    }
}