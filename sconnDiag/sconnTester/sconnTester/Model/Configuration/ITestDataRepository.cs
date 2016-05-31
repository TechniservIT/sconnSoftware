using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnTester.Model.Test;

namespace sconnTester.Model.Configuration
{
    public interface ITestDataRepository
    {
        List<IMeasurementTest> GetAll();
        IMeasurementTest Get(int id);
        void Add(IMeasurementTest test);
        void Remove(IMeasurementTest test);
    }

}
