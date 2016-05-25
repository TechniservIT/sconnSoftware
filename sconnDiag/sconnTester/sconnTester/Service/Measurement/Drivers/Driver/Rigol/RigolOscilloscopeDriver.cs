using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnTester.Model.Test;

namespace sconnTester.Service.Measurement.Drivers.Driver.Rigol
{
    public class RigolOscilloscopeDriver : INetworkInterfaceDriver
    {
        public bool Connect()
        {
            throw new NotImplementedException();
        }

        public bool Connected { get; set; }
        public int Channels { get; set; }
        public double GetValue(int channel, ElectricMeasurementType type)
        {
            throw new NotImplementedException();
        }

        public void SetValue(int channel, double value, ElectricMeasurementType type)
        {
            throw new NotImplementedException();
        }
    }
}
