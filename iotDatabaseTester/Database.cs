using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using iotDbConnector.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


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
            iotRepository<Location> locrepo = new iotRepository<Location>();
            Location loc = new Location();
            loc.LocationName = Guid.NewGuid().ToString();
            loc.Lat = 0;
            loc.Lng = 0;
            locrepo.Add(loc);

            List<Location> locs = locrepo.GetAll().ToList();
            Assert.IsTrue(locs.Contains(loc));

        }

        [TestMethod]
        public void TestRepoRead()
        {
            iotRepository<Location> repo = new iotRepository<Location>();
            List<Location> locs = repo.GetAll().ToList();
            Assert.IsTrue(locs != null);
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
                Location StoredBefore = repo.GetById(loc.LocationId);
                StoredBefore.Lat = 53.325241;
                repo.Update(StoredBefore);
                //verify
                Location stored = repo.GetById(loc.LocationId);
                Assert.IsTrue(loc.Lat == stored.Lat);
            }
        }

        [TestMethod]
        public void TestRepoDelete()
        {
            iotRepository<iotDomain> repo = new iotRepository<iotDomain>();


        }
    }
}
