using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDatabaseConnector.DAL.Repository
{
    static public class iotGenericGlobalContext<T> where T : class
    {
        static public DbContext DbContext;
        static public DbSet<T> DbSet;

        static iotGenericGlobalContext()
        {
                DbContext = iotGlobalContext.context;
                DbSet = DbContext.Set<T>();
        }
    }

    static public class iotGlobalContext
    {
        static public iotContext context;

        static iotGlobalContext()
        {
            context = new iotContext();
        }

    }

}
