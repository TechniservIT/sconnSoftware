using iotData.POCO.Surveillance.Analysis;
using iotData.POCO.Surveillance.Events;
using iotData.POCO.Surveillance.Recording;
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

         
        [Key]
        [Required]
        public int Id { get; set; }

         
        public string Name { get; set; }

         
        public string AccessUrl { get; set; }

         
        public bool RequiresAuth { get; set; }
        
         
        public virtual Location Location { get; set; }

         
        public virtual EndpointInfo Endpoint { get; set; }

        
        public virtual List<SurveillanceEvent> Events { get; set; }

         
        public virtual List<SurveillanceAnalysisConfig> Analysis { get; set; }
        
         
        public virtual List<SurveillanceRecordingSetup> RecordingSetup { get; set; }

         
        public virtual List<SurveillanceRecording> Recordings { get; set; }


         
        public virtual Site Site { get; set; }

    }
}
