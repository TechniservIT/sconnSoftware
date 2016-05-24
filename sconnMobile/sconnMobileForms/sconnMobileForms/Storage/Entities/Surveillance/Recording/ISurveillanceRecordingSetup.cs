using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iotData.POCO.Surveillance.Recording
{

    public enum SurveillanceRecordingType
    {
        ContinousRecording = 1,
        Triggered,
        Scheduled, 
        Manual
    }


    public interface ISurveillanceRecordingSetup
    {
        DateTime From { get; set; }
        DateTime Until { get; set; }
        SurveillanceRecordingType Type { get; set; }
    }

    public class SurveillanceRecordingSetup
    {
         
        [Key]
        [Required]
        public int Id { get; set; }

         
        public DateTime From { get; set; }

         
        public DateTime Until { get; set; }

         
        public SurveillanceRecordingType Type { get; set; }

         
        public virtual IpCamera Source { get; set; }
    }

}
