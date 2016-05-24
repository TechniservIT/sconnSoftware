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
         
        [Key]
        [Required]
        public int Id { get; set; }

         
        bool Enabled { get; set; }

         
        public DateTime From { get; set; }

         
        public DateTime Until { get; set; }

         
        public SurveillanceAnalysisType Type { get; set; }

         
        public virtual IpCamera Source { get; set; }
    }

}
