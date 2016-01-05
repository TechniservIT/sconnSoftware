using System;
using Microsoft.Owin;
using NLog;
using Owin;

[assembly: OwinStartupAttribute(typeof(iotDash.Startup))]
namespace iotDash
{
    public partial class Startup
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        //private void LoadLoggerSample()
        //{
        //    _logger.Info("Testing Logger");

        //    try
        //    {
        //    //    throw new DivideByZeroException();
        //        throw new System.IO.IOException();
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.Error(e, e.Message);
        //    }
           
        //}

        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
            
        }
    }
}

