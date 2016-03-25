using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotData.POCO.Surveillance.Analysis
{
    public class SurveillanceAnalysisDetectionFeature : ISurveillanceAnalysisConfig
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public bool Enabled
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
