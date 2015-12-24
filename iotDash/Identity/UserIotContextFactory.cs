using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iotDash.Models;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;

namespace iotDash.Identity
{
    public static class UserIotContextFactory
    {
        public static IIotContextBase GetContextForUser(ApplicationUser user)
        {
            try
            {
                IIotContextBase cont = new iotContext(user.DomainId);
                return cont;
            }
            catch (Exception)
            {
                    
                throw;
            }
        }

    }

}