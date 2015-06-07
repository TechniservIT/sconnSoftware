using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iotDbConnector.DAL;
using System.Linq;
using System.Diagnostics;

namespace iotDatabaseTester
{
    [TestClass]
    public class NoSqlDboUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
        }


        [TestMethod]
        public void TestRavenDbCRUD()
        {
            RavenNoSqlDataSource<Device> db = new RavenNoSqlDataSource<Device>();
            db.Add(new Device());
            var res = db.GetDevice();
            Assert.IsTrue(res != null);
        }

        [TestMethod]
        public void TestRavenDbReadLoad()
        {
            int ReadTestInterations = 250;
            int maxQueryTimeMs = 25;
            RavenNoSqlDataSource<Device> db = new RavenNoSqlDataSource<Device>();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i<ReadTestInterations; i++)
			{
                var res = db.GetDevice();
            }
            watch.Stop();
            double TimePerQueryMs = watch.ElapsedMilliseconds / ReadTestInterations;
            Assert.IsTrue(TimePerQueryMs < maxQueryTimeMs);
        }

    }
}
