using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iotData.POCO.Surveillance
{
    public class IpCamera
    {

        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string AccessUrl { get; set; }

        [DataMember]
        public bool RequiresAuth { get; set; }



        [DataMember]
        public virtual Location Location { get; set; }

        [DataMember]
        public virtual EndpointInfo Endpoint { get; set; }
        
 



    }
}
