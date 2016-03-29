using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iotData.POCO.Surveillance.Recording
{
    public class SurveillanceRecording
    {
        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        public DateTime From { get; set; }

        [DataMember]
        public DateTime Until { get; set; }

        [DataMember]
        public  virtual SurveillanceRecordingSetup RecordingSetup { get; set; }

        [DataMember]
        public virtual IpCamera Source { get; set; }
    }
}
