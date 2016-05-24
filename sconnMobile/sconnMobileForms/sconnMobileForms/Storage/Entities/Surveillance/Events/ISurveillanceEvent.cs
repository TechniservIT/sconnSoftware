using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iotData.POCO.Surveillance.Events
{
    public enum SurveillanceEventType
    {
        CctvEventConnectionLoss,
        CctvEventMotionDetection,
        CctvEventObjectDetection,
        CCtvEventVideoLoss
    }

    public interface ISurveillanceEvent
    {
        IpCamera Source { get; set; }
        DateTime Time { get; set; }
    }


    public class SurveillanceEvent
    {
         
        [Key]
        [Required]
        public int Id { get; set; }
        

        public SurveillanceEventType Type { get; set; }

         
        public DateTime Time { get; set; }

         
        public virtual IpCamera Source { get; set; }
    }

}
