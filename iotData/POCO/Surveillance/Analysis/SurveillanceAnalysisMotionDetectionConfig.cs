using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotData.POCO.Surveillance.Analysis
{
    public class SurveillanceAnalysisMotionDetectionConfig : ISurveillanceAnalysisConfig
    {
        [Key]
        [Required]
        public int Id { get; set; }
        
        public int MinimumMotionDurationMs { get; set; }

        public int MaximumMotionDurationMs { get; set; }

        public int RelativeSensitivity { get; set; }

        public bool Enabled { get; set; }

        [Required]
        public virtual IpCamera Source { get; set; }

    }
}
