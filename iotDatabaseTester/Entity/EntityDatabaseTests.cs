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
using iotDatabaseConnector.DAL.Repository.Connector.Entity;

namespace iotDatabaseTester
{

    public class DatabaseContextUnitTest
    {

        private IIotContextBase context;

        public DatabaseContextUnitTest(IIotContextBase cont)
        {
            this.context = cont;
        }

        [TestMethod]
        public void TestContextSelect()
        {
            List<iotDomain> domains = (from d in context.Domains
                                       select d).ToList();
            Assert.IsTrue(domains != null);
        }


        [TestMethod]
        public void TestDeviceParamQueryTime()
        {
            int ReadTestInterations = 50;
            int maxQueryTimeMs = 25;
            Stopwatch watch = new Stopwatch();
            DeviceProperty prop = context.Properties.FirstOrDefault();  //load initialy

            watch.Start();
            for (int i = 0; i < ReadTestInterations; i++)
            {
                DeviceParameter param = context.Parameters
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
              int ReadTestInterations = 50;
              int maxQueryTimeMs = 2;
              Stopwatch watch = new Stopwatch();
              DeviceProperty inprop = context.Properties.AsNoTracking().FirstOrDefault();  //load initialy
              watch.Start();
                for (int i = 0; i < ReadTestInterations; i++)
                {
                    DeviceProperty prop = (from r in context.Properties.AsNoTracking()
                                             select r).FirstOrDefault();
                }
                watch.Stop();
                double TimePerQueryMs = watch.ElapsedMilliseconds / ReadTestInterations;
                Assert.IsTrue(TimePerQueryMs < maxQueryTimeMs);
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
            dm.Sites = new List<Site>();
            locrepo.Add(dm);
            List<iotDomain> locs = locrepo.GetAll().ToList();
            Assert.IsTrue(locs.Contains(dm));
            locrepo.Delete(dm);
        }

        [TestMethod]
        public void TestRepoRead()
        {
            iotRepository<Location> repo = new iotRepository<Location>();
            List<Location> locs = repo.GetAll().ToList();
            Assert.IsTrue(locs != null);
        }

      
        public bool TestRepoSingleRead(int id)
        {
            try
            {
                iotRepository<Location> repo = new iotRepository<Location>();
                Location locs = repo.GetById(id);
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
    

}
