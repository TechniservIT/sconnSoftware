using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnTester.Model.Test
{
    public enum ElectricMeasurementType
    {
        Voltage = 1,
        Current,
        Frequency,
        Amplitude,
        RiseTime,
        FallTime,
        Average,
        Duty,
        Overshot,
        Undershot
    }

    public interface IMeasurmentTestParameter
    {
        string Name { get; set; }
        string Description { get; set; }
        double MinimumValue { get; set; }
        double MaximumValue { get; set; }
        double MeasurmentTimeMinMs { get; set; }
        double MeasurmentTimeMaxMs { get; set; }
        ElectricMeasurementType Type { get; set; }
    }

}
