using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnTester.Model.Test
{
    public interface IMeasurementTest
    {
        void Run();
        List<IMeasurmentTestRecord> Tests { get; set; }
        bool Running { get; set; }
        bool Success { get; set; }

    }

}
