using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmSystemManagmentService.Device;
using Prism.Regions;
using sconnConnector.Config;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Navigation;

namespace sconnRem.ViewModel.Generic
{
    public  class GenericAlarmConfigViewModel : GenericAsyncConfigViewModel
    {
        protected   AlarmSystemConfigManager _manager;

        protected void NavigateToAlarmContract(string contractName)
        {
            try
            {
                this._regionManager.RequestNavigate(GlobalViewRegionNames.MainGridContentRegion, contractName
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
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);

            }
        }


        protected void NavigateToAlarmContractWithParam(string contractName, NavigationParameters param)
        {
            try
            {
                this._regionManager.RequestNavigate(GlobalViewRegionNames.MainGridContentRegion, new Uri(contractName + param, UriKind.Relative)
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
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);

            }
        }
    }


}
