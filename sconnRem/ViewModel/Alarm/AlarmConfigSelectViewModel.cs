using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnRem.ViewModel.Generic;

namespace sconnRem.ViewModel.Alarm
{
    public class AlarmConfigSelectViewModel : ObservableObject, IPageViewModel   /*: ViewModelBase<IGridNavigatedView>*/
    {
        //public AlarmConfigSelectViewModel(IGridNavigatedView view) : base(view)
        //{
        //}

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/config1.png"; }
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
        }

        public AlarmConfigSelectViewModel()
        {
                
        }
    }

}
