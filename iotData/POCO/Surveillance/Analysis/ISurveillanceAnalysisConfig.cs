using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iotData.POCO.Surveillance.Analysis
{

    public enum SurveillanceAnalysisType
    {
        SurveillanceAnalysisFeatureDetection,
        SurveillanceAnalysisMotionDetection,
        SurveillanceAnalysisObjectDetection,
        SurveillanceAnalysisFaceDetection,
        SurveillanceAnalysisObjectCountDetection,
        SurveillanceAnalysisSequenceDetection
    }

    public interface ISurveillanceAnalysisConfig
    {
        bool Enabled { get; set; }
    }

    public class SurveillanceAnalysisConfig
    {
        [DataMember]
        [Key]
        [Required]
        public int Id { get; set; }

        [DataMember]
        bool Enabled { get; set; }

        [DataMember]
        public DateTime From { get; set; }

        [DataMember]
        public DateTime Until { get; set; }

        [DataMember]
        public SurveillanceAnalysisType Type { get; set; }

        [DataMember]
        public virtual IpCamera Source { get; set; }
    }

}
