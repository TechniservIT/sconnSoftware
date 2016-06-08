using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnRem.ViewModel.Generic;
using Prism.Mvvm;
using System.ComponentModel.Composition;
using Prism;
using Prism.Regions;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Navigation;

namespace sconnRem.ViewModel.Alarm
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmConfigSelectViewModel : GenericAsyncConfigViewModel
    {
      
        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config1.png"; }
        }



        public override void GetData()
        {
           
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            //if (navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmConfig_Contract_Device_Sensor_View))
            //{
            //    return true;    //singleton
            //}
            return false;
        }

      
    }

}
