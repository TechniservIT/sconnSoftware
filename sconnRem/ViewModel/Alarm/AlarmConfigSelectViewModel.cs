using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnRem.ViewModel.Generic;
using Prism.Mvvm;
using System.ComponentModel.Composition;

namespace sconnRem.ViewModel.Alarm
{
    [Export]
    public class AlarmConfigSelectViewModel : BindableBase   
    {
      
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
