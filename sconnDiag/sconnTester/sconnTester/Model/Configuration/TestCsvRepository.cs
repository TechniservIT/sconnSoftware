using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnTester.Model.Test;

namespace sconnTester.Model.Configuration
{
    public class TestCsvRepository : ITestDataRepository
    {
        public List<IMeasurementTest> GetAll()
        {
            throw new NotImplementedException();
        }

        public IMeasurementTest Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Add(IMeasurementTest test)
        {
            throw new NotImplementedException();
        }

        public void Remove(IMeasurementTest test)
        {
            throw new NotImplementedException();
        }
    }
}
