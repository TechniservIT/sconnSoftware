using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iotDbConnector.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Diagnostics;
using System.Data.Entity;

namespace iotDatabaseTester
{
    [TestClass]
    public class DatabaseContextUnitTest
    {

        [TestMethod]
        public void TestContextSelect()
        {
            iotContext cont = new iotContext();
            List<iotDomain> domains = (from d in cont.Domains
                                       select d).ToList();
            Assert.IsTrue(domains != null);
        }

        [TestMethod]
        public void TestDeviceParamQueryTime()
        {
            iotContext cont = new iotContext();
            int ReadTestInterations = 50;
            int maxQueryTimeMs = 25;
            Stopwatch watch = new Stopwatch();
            DeviceProperty prop = cont.Properties.FirstOrDefault();  //load initialy
            //DeviceParameter tparam = (from r in prop.ResultParameters
            //                         select r).Single();  //load initialy

            watch.Start();
            for (int i = 0; i < ReadTestInterations; i++)
            {
                DeviceParameter param =  cont.Parameters
                                        .Include(p=>p.sconnMappers)
                                        .FirstOrDefault();

            }
            watch.Stop();
            double TimePerQueryMs = watch.ElapsedMilliseconds / ReadTestInterations;
            Assert.IsTrue(TimePerQueryMs < maxQueryTimeMs);
        }


        [TestMethod]
        public void TestDevicePropQueryTime()
        {

            iotContext cont = new iotContext();
              int ReadTestInterations = 50;
              int maxQueryTimeMs = 2;
              Stopwatch watch = new Stopwatch();
              DeviceProperty inprop = cont.Properties.AsNoTracking().FirstOrDefault();  //load initialy
              watch.Start();
                for (int i = 0; i < ReadTestInterations; i++)
                {
                    DeviceProperty prop = (from r in cont.Properties.AsNoTracking()
                                             select r).FirstOrDefault();
                }
                watch.Stop();
                double TimePerQueryMs = watch.ElapsedMilliseconds / ReadTestInterations;
                Assert.IsTrue(TimePerQueryMs < maxQueryTimeMs);
        }



        [TestMethod]
        public void TestConnectorSelect()
        {
            iotConnector conn = new iotConnector();
            List<iotDomain> domains = conn.DomainList();
            var task = conn.DomainList();
            Assert.IsTrue(domains != null && domains != null);
        }


        [TestMethod]
        public void TestConnectorRepo()
        {
            iotRepository<iotDomain> repo = new iotRepository<iotDomain>();
            List<iotDomain> domains = repo.GetAll().ToList(); 
            Assert.IsTrue(domains != null );
        }

        /* CRUD */
        [TestMethod]
        public void TestRepoCreate()
        {
            iotRepository<iotDomain> locrepo = new iotRepository<iotDomain>();
            iotDomain dm = new iotDomain();
            dm.DomainName = Guid.NewGuid().ToString();
            dm.Sites = new AIList<Site>();
            locrepo.Add(dm);

            List<iotDomain> locs = locrepo.GetAll().ToList();
            Assert.IsTrue(locs.Contains(dm));

        }

        [TestMethod]
        public void TestRepoRead()
        {
            iotRepository<Location> repo = new iotRepository<Location>();
            List<Location> locs = repo.GetAll().ToList();
            Assert.IsTrue(locs != null);
        }

      
        public bool TestRepoSingleRead(string id)
        {
            try
            {
                iotRepository<Location> repo = new iotRepository<Location>();
                Location locs = repo.GetById(int.Parse(id));
                return locs != null;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }

        [TestMethod]
        public void TestRepoUpdate()
        {
            iotRepository<Location> repo = new iotRepository<Location>();
            //get
            List<Location> locs = repo.GetAll().ToList();
            //add if required
            Location loc =  new Location();
            if (locs != null)
            {
                    loc.LocationName = Guid.NewGuid().ToString();
                    loc.Lat = 0;
                    loc.Lng = 0;
                    repo.Add(loc);
                //update
                locs = repo.GetAll().ToList();
                loc = locs.Where(l => { return l.LocationName == loc.LocationName; }).First();
                Location StoredBefore = repo.GetById(loc.Id);
                StoredBefore.Lat = 53.325241;
                repo.Update(StoredBefore);
                //verify
                Location stored = repo.GetById(loc.Id);
                Assert.IsTrue(loc.Lat == stored.Lat);
            }
        }

        [TestMethod]
        public void TestRepoDelete()
        {
            iotRepository<iotDomain> repo = new iotRepository<iotDomain>();


        }

        /****  LOAD ****/

        [TestMethod]
        public void TestEntityDbWriteLoad()
        {
            try
            {
                int ReadTestInterations = 5;
                int maxQueryTimeMs = 25;

                Stopwatch watch = new Stopwatch();

                watch.Start();
                for (int i = 0; i < ReadTestInterations; i++)
                {
                    iotRepository<Location> locrepo = new iotRepository<Location>();
                    Location loc = new Location();
                    loc.LocationName = Guid.NewGuid().ToString();
                    loc.Lat = 0;
                    loc.Lng = 0;
                    locrepo.Add(loc);
                }
                watch.Stop();

                double TimePerQueryMs = watch.ElapsedMilliseconds / ReadTestInterations;
                Assert.IsTrue(TimePerQueryMs < maxQueryTimeMs);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestEntityDbReadLoad()
        {
            try
            {
                int ReadTestInterations = 250;
                int maxQueryTimeMs = 25;
                bool ReadSuccess = false;
                int FailCount = 0;

                TestRepoCreate();

                Stopwatch watch = new Stopwatch();

                iotRepository<iotDomain> repo = new iotRepository<iotDomain>();
                List<iotDomain> domains = repo.GetAll().ToList();
                iotDomain domain = domains.First();


                watch.Start();
                for (int i = 0; i < ReadTestInterations; i++)
                {

                    ReadSuccess = TestRepoSingleRead(domain.Id);
                    if (!ReadSuccess) { FailCount++; }

                }
                watch.Stop();

                double TimePerQueryMs = watch.ElapsedMilliseconds / ReadTestInterations;
                Assert.IsTrue(TimePerQueryMs < maxQueryTimeMs);
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
           
        }

    }




    //[TestClass]
    //public class NoSqlDatabaseUnitTest
    //{

    //    [TestMethod]
    //    void TestRavenDbCRUD()
    //    {
    //        RavenNoSqlDataSource<string> db = new RavenNoSqlDataSource<string>();
    //        db.Add("sample");
    //        var res = db.GetAll();
    //        Assert.IsTrue(res.Contains("sample"));
    //    }

    //}


}
