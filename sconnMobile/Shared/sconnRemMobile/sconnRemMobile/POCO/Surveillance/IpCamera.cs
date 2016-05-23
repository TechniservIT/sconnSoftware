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


        [DataMember]
        public virtual List<SurveillanceEvent> Events { get; set; }

        [DataMember]
        public virtual List<SurveillanceAnalysisConfig> Analysis { get; set; }
        
        [DataMember]
        public virtual List<SurveillanceRecordingSetup> RecordingSetup { get; set; }

        [DataMember]
        public virtual List<SurveillanceRecording> Recordings { get; set; }


        [DataMember]
        public virtual Site Site { get; set; }

    }
}
