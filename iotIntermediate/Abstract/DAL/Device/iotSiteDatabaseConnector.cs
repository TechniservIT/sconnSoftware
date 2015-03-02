using iotDash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iodash.DAL
{
    public class iotSiteDatabaseConnector
    {
        private ApplicationDbContext context;
        public iotSiteDatabaseConnector()
        {
            context = new ApplicationDbContext();
        }

    }
}