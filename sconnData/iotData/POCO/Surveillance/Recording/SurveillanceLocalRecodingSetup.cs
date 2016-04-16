using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotData.POCO.Surveillance.Recording
{
    public class SurveillanceLocalRecodingSetup : ISurveillanceRecordingSetup
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime From
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

        [Required]
        public SurveillanceRecordingType Type
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

        [Required]
        public DateTime Until
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
