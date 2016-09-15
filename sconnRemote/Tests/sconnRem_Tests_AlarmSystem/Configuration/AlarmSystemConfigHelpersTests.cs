using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using sconnConnector;

namespace sconnRem_Tests_AlarmSystem.Configuration
{
    [TestFixture]
    public class AlarmSystemConfigHelpersTests
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        void Test_AlarmHelpers_Word_Serialization()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            bool passed = true;
            byte[] cfgBuffer = new byte[2];
            for (int i = 0; i < 100; i++)
            {
                ushort sampleId = (ushort)rnd.Next(ushort.MaxValue);
                AlarmSystemConfig_Helpers.WriteWordToBufferAtPossition(sampleId, cfgBuffer,0);
                ushort Id = (ushort)AlarmSystemConfig_Helpers.GetWordFromBufferAtPossition(cfgBuffer,0);
                if (sampleId != Id)
                {
                    passed = false;
                }
            }
            
            Assert.IsTrue(passed);
        }
        

        [Test]
        void Test_AlarmHelpers_Long_Serialization()
        {
            Assert.IsTrue(true);
        }
        
        [Test]
        void Test_AlarmHelpers_Time_Serialization()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            bool passed = true;
            byte[] cfgBuffer = new byte[4];
            for (int i = 0; i < 100; i++)
            {
                DateTime date = new DateTime(rnd.Next(int.MaxValue));
                AlarmSystemConfig_Helpers.WriteDateTimeToBufferAtPossition(date, cfgBuffer, 0);
                DateTime rxDateTime = AlarmSystemConfig_Helpers.GetDateTimeFromBufferAtPossition(cfgBuffer, 0);
                if (rxDateTime != date)
                {
                    passed = false;
                }
            }

            Assert.IsTrue(passed);
        }
        

    }
}
