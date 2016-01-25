using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iotDash.Models;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using Moq;
using NLog;

namespace iotDash.Identity
{
    public static class UserIotContextFactory
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();


        public static IIotContextBase GetContextForUser(ApplicationUser user)
        {
            try
            {
                IIotContextBase cont = new iotContext(user.DomainId);
                return cont;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

       

        public static IIotContextBase GetFakeContextForUserHttpContext(HttpContextBase context)
        {
            try
            {
                IIotContextBase icont = new Mock<iotContext>().Object;
                icont.Fake();
                return icont;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

        public static IIotContextBase GetDataContextForUserHttpContext(HttpContextBase context)
        {
            try
            {
                var cont = new ApplicationDbContext();
                var currentUser = (from u in cont.Users
                                   where u.UserName.Equals(context.User.Identity.Name)
                                   select u).First();
                IIotContextBase icont = new iotContext(currentUser.DomainId);
                return icont;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

    }

}