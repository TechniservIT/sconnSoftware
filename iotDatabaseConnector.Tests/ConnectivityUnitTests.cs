using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using iotDbConnector.DAL;
using NUnit.Framework;

namespace iotDatabaseConnector.Tests
{
    /// <summary>
    /// Summary description for ConnectivityUnitTest
    /// </summary>
    [TestFixture]
    public class ConnectivityUnitTests
    {
        private iotContext _context;


        [SetUp]
        public void Setup()
        {
            this._context = new iotContext();
        }

        public ConnectivityUnitTests()
        {
        }

        [Test]
        public void Test_Database_Connect()
        {
            _context.Database.Connection.Open();
            Assert.IsTrue(this._context.Database.Connection.State == ConnectionState.Open);
        }

        [Test]
        public void Test_Database_Response_Time()
        {
            Assert.IsTrue(false);
        }


    }
}
