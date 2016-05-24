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
         
        [Key]
        [Required]
        public int Id { get; set; }
        
        public DateTime From { get; set; }

         
        public DateTime Until { get; set; }

         
        public  virtual SurveillanceRecordingSetup RecordingSetup { get; set; }

         
        public virtual IpCamera Source { get; set; }
    }
}
