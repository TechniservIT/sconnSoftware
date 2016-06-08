using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AlarmSystemManagmentService;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmGraphEntityViewModel : BindableBase, IActiveAware, INavigationAware, IChangeTracking, INotifyPropertyChanged
    {
        public IAlarmSystemNamedEntityWithType Entity { get; set; }
        public string PreviousUrl { get; set; }
        public string NextUrl { get; set; }
        public bool IsActive { get; set; }
        public event EventHandler IsActiveChanged;
        public bool IsChanged { get; }

        public AlarmGraphEntityViewModel()
        {
            Entity= new sconnAlarmZone();
        }
        
        [ImportingConstructor]
        public AlarmGraphEntityViewModel(IRegionManager regionManager)
        {
           
        }

        [ImportingConstructor]
        public AlarmGraphEntityViewModel(IAlarmSystemNamedEntityWithType entity, IRegionManager regionManager)
        {
            Entity = entity;
        }

        public string DisplayedImagePath => Entity.imageIconUri;

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            throw new NotImplementedException();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmConfig_Contract_ZoneConfigView))
            {
                return true;    //singleton
            }
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
         
        }

        public void AcceptChanges()
        {
          
        }

        
    }


}
