using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnTester.Model.Abstract
{

    public interface IMeasurementTestResult
    {
        double Value { get; set; }
        double Elapsed { get; set; }
    }

}
