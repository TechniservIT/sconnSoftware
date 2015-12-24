using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;

namespace iotDatabaseConnector.DAL.Repository
{
    static public class iotGenericGlobalContext<T> where T : class
    {
        static public DbContext DbContext;
        static public DbSet<T> DbSet;

        static iotGenericGlobalContext()
        {
                DbContext = (DbContext) iotGlobalContext.context;
                DbSet = DbContext.Set<T>();
        }
    }

    static public class iotGlobalContext
    {
        static public IIotContextBase context;

        static iotGlobalContext()
        {
            context = new iotContext();
        }

    }

}
