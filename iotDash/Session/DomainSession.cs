using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iotDash.Identity;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using iotDbConnector.DAL;
using sconnConnector.Config;

namespace iotDash.Session
{

    /************ DomainSession *********/
    /* 
    This is a service locator for session related objects including entity context.
    Objects are extracted from provided httpContext and if needed, created.    
    */
    static public class DomainSession
    {
        
        /********** Domain ***********/
        static public string GetContextDomain(HttpContextBase httpContext)
        {
            string url = httpContext.Request.RawUrl;
            var urlcomponents = url.Split('/');
            string appdomain = urlcomponents[1];

            return appdomain;
        }

        static public iotDomain GetDomainForHttpContext(HttpContextBase hcontext)
        {
            try
            {
                var cont = (iotContext)hcontext.Session["iotcontext"];
                string domainId = DomainSession.GetContextDomain(hcontext);
                iotDomain d = cont.Domains.First(dm => dm.DomainName.Equals(domainId));
                return d;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /********** Context ***********/
        static public void CreateDataContextForUserContext(HttpContextBase httpContext)
        {
            IIotContextBase icont = UserIotContextFactory.GetDataContextForUserHttpContext(httpContext);
            System.Web.HttpContext.Current.Session["iotcontext"] = icont;
        }

        static public void LoadDataContextForUserContext(HttpContextBase httpContext)
        {
            IIotContextBase cont = (IIotContextBase)httpContext.Session["iotcontext"];
            if (cont == null)
            {
                IIotContextBase icont = UserIotContextFactory.GetDataContextForUserHttpContext(httpContext);
                System.Web.HttpContext.Current.Session["iotcontext"] = icont;
            }
        }

        static public IIotContextBase GetDataContextForUserContext(HttpContextBase httpContext)
        {
            IIotContextBase cont = (IIotContextBase)httpContext.Session["iotcontext"];
            if (cont == null)
            {
                IIotContextBase icont = UserIotContextFactory.GetDataContextForUserHttpContext(httpContext);
                System.Web.HttpContext.Current.Session["iotcontext"] = icont;
                return icont;
            }
            else
            {
                return cont;
            }
        }


        /********** Alarm system ***********/
        static public AlarmSystemConfigManager GetAlarmConfigForContextWithDevice(HttpContextBase cont, Device dev)
        {
            AlarmSystemConfigManager man = (AlarmSystemConfigManager)cont.Session["alarmSysCfg"];
            cont.Session["alarmDevice"] = dev;
            if (man != null)
            {
                return man;
            }
            else
            {
                man = new AlarmSystemConfigManager(dev.EndpInfo,dev.Credentials);
                cont.Session["alarmSysCfg"] = man;
            }
            return man;
        }

        static public AlarmSystemConfigManager GetAlarmConfigForContextWithDeviceId(HttpContextBase cont, int devid)
        {
            IIotContextBase Icont = (IIotContextBase)cont.Session["iotcontext"];
            Device alrmSysDev = Icont.Devices.First(d => d.Id == devid);
            if (alrmSysDev != null)
            {
                cont.Session["alarmDevice"] = alrmSysDev;
                AlarmSystemConfigManager man = (AlarmSystemConfigManager)cont.Session["alarmSysCfg"];
                if (man != null)
                {
                    return man;
                }
                else
                {
                    man = new AlarmSystemConfigManager(alrmSysDev.EndpInfo, alrmSysDev.Credentials);
                    cont.Session["alarmSysCfg"] = man;
                }
                return man;
            }
            else
            {
                return null;
            }
        }

        static public AlarmSystemConfigManager GetAlarmConfigForContextSession(HttpContextBase cont)
        {
            IIotContextBase Icont = (IIotContextBase)cont.Session["iotcontext"];
            Device alrmSysDev = (Device)cont.Session["alarmDevice"];
            if (alrmSysDev != null)
            {
                cont.Session["alarmDevice"] = alrmSysDev;
                AlarmSystemConfigManager man = (AlarmSystemConfigManager)cont.Session["alarmSysCfg"];
                if (man != null)
                {
                    return man;
                }
                else
                {
                    man = new AlarmSystemConfigManager(alrmSysDev.EndpInfo, alrmSysDev.Credentials);
                    cont.Session["alarmSysCfg"] = man;
                }
                return man;
            }
            else
            {
                return null;
            }
        }

}
}