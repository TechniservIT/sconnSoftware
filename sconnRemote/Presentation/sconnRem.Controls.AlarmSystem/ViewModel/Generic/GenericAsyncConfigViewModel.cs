using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmSystemManagmentService.Device;
using NLog;
using Prism.Mvvm;
using Prism.Regions;
using sconnConnector.Config;
using sconnRem.Navigation;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Generic
{

    public  class GenericAsyncConfigViewModel : BindableBase, IAsyncConfigViewModel
    {
        protected bool _loading;
        public bool Loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                this.OnPropertyChanged();
            }
        }


        protected string _name;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        protected IRegionManager _regionManager;
        protected Logger _nlogger = LogManager.GetCurrentClassLogger();

        public bool IsActive { get; set; }
        public event EventHandler IsActiveChanged;
        public bool IsChanged { get; set; }
        
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            BackgroundWorker bgWorker = new BackgroundWorker();
            bgWorker.DoWork += (s, e) => {
                GetData();
            };
            bgWorker.RunWorkerCompleted += (s, e) =>
            {

                Loading = false;
            };

            Loading = true;
            bgWorker.RunWorkerAsync();
        }

        public virtual void GetData()
        {
            
        }

        public virtual void SaveData()
        {
            
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public virtual void AcceptChanges()
        {
            
        }

    }

}
