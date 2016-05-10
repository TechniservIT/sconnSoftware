using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism;
using Prism.Mvvm;
using Prism.Regions;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Generic
{
    public interface IAsyncConfigViewModel : IActiveAware, INavigationAware, IChangeTracking, INotifyPropertyChanged
    {
        void GetData();
        void SaveData();
    }

}
