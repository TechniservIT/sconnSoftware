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
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);

                GlobalNavigationContext.NavigateRegionToContractWithParam(
                   GlobalViewRegionNames.MainGridContentRegion,
                    contractName,
                    parameters
                    );
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
                param.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);
                GlobalNavigationContext.NavigateRegionToContractWithParam(
                   GlobalViewRegionNames.MainGridContentRegion,
                    contractName,
                    param
                    );
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);

            }
        }
    }


}
