using System;
using System.Collections.Generic;
using System.Linq;
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

}
