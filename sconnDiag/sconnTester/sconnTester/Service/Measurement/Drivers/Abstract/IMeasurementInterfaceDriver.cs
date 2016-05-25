using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnTester.Model.Test;

namespace sconnTester.Service.Measurement.Drivers
{
    public interface IMeasurementInterfaceDriver
    {
        bool Connect();
        bool Connected { get; set; }

        int Channels { get; set; }
        double GetValue(int channel, ElectricMeasurementType type);
        void SetValue(int channel, double value, ElectricMeasurementType type);

    }

}
