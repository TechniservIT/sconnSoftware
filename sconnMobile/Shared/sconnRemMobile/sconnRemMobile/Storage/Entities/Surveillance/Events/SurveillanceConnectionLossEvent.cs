using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iotData.POCO.Surveillance.Events
{
    public class SurveillanceConnectionLossEvent : ISurveillanceEvent
    {
        [Key]
        [Required]
        [DataMember]
        public int Id { get; set; }


        [DataMember]
        public virtual IpCamera Source
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        [DataMember]
        public DateTime Time
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
