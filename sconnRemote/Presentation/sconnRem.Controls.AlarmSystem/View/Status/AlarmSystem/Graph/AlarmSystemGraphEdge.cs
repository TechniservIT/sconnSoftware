using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;

namespace sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.Graph
{

    public    class AlarmSystemGraphEdge : Edge<AlarmSystemGraphVertex>, INotifyPropertyChanged
    {

        private string id;

        public string ID
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged("ID");
            }
        }

        public AlarmSystemGraphEdge(string id, AlarmSystemGraphVertex source, AlarmSystemGraphVertex target): base(source, target)
        {
            ID = id;
        }


        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion


    }

}
