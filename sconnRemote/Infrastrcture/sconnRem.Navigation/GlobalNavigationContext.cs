using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Prism.Regions;

namespace sconnRem.Navigation
{
    public static class GlobalNavigationContext
    {
        public static IRegionManager Manager;
        public static Logger _nlogger = LogManager.GetCurrentClassLogger();
        private static object syncRoot = new Object();

        public  static  void NavigateDefaults()
        {
            GlobalNavigationContext.NavigateRegionToContract("","");
        }

        public  static  void NavigateRegionToContract(string regionName, string contractName)
        {
            try
            {
                lock (syncRoot)
                {
                    Manager.RequestNavigate(regionName, contractName
                       ,
                       (NavigationResult nr) =>
                       {
                           var error = nr.Error;
                           var result = nr.Result;
                           if (error != null)
                           {
                               _nlogger.Error(error);
                           }
                       });
                }
    
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);

            }
        }


        public static void NavigateRegionToContractWithParam(string regionName, string contractName, NavigationParameters param)
        {
            try
            {
                lock (syncRoot)
                {
                    Manager.RequestNavigate(regionName, new Uri(contractName + param, UriKind.Relative)
                     ,
                     (NavigationResult nr) =>
                     {
                         var error = nr.Error;
                         var result = nr.Result;
                         if (error != null)
                         {
                             _nlogger.Error(error);
                         }
                     });
                }
 
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);

            }

        }
    }
}
