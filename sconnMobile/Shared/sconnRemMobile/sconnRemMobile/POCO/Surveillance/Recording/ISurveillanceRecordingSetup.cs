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
        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        public DateTime From { get; set; }

        [DataMember]
        public DateTime Until { get; set; }

        [DataMember]
        public SurveillanceRecordingType Type { get; set; }

        [DataMember]
        public virtual IpCamera Source { get; set; }
    }

}
