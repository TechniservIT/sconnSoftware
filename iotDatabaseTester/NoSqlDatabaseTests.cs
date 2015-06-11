using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iotDbConnector.DAL;
using System.Linq;
using System.Diagnostics;
using iotNoSqlDatabase;

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

        [TestMethod]
        public void TestRedisDbWriteLoad()
        {
            int ReadTestInterations = 250;
            int maxQueryTimeMs = 25;
            RedisNoSqlDataSource<Device> db = new RedisNoSqlDataSource<Device>();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < ReadTestInterations; i++)
            {
                var res = db.Add(new Device());
            }
            watch.Stop();
            double TimePerQueryMs = watch.ElapsedMilliseconds / ReadTestInterations;
            Assert.IsTrue(TimePerQueryMs < maxQueryTimeMs);
        }

        [TestMethod]
        public void TestRedisDbRWLoad()
        {
            int ReadTestInterations = 250;
            int maxQueryTimeMs = 25;
            RedisNoSqlDataSource<Device> db = new RedisNoSqlDataSource<Device>();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < ReadTestInterations; i++)
            {
                string id = db.Add(new Device{ DeviceName = Guid.NewGuid().ToString() });
                var stored = db.GetById(id);
                if (stored == null)
                {
                    Assert.Fail();
                }
            }
            watch.Stop();
            double TimePerQueryMs = watch.ElapsedMilliseconds / ReadTestInterations;
            Assert.IsTrue(TimePerQueryMs < maxQueryTimeMs);
        }



    }
}
