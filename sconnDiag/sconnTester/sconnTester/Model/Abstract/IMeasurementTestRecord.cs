using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnTester.Model.Abstract;

namespace sconnTester.Model.Test
{
    public interface IMeasurmentTestRecord
    {
        IMeasurmentTestParameter Definition { get; set; }
        IMeasurementTestResult Result { get; set; }
    }

}
