using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using sconnConnector.POCO.Config.sconn;

namespace sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.Graph
{

    public class VertexEventArgs : EventArgs
    {
        public sconnAlarmZone Zone { get; set; }
        public sconnDevice Device { get; set; }
    }

    public class AlarmSystemGraphZoneVertex : AlarmSystemGraphVertex
    {
        public sconnAlarmZone Config { get; set; }

       // protected override void OnVertexClicked()
       // {
       //     VertexEventArgs args=  new VertexEventArgs();
       //     args.Zone = Config;
       ////     VertexClicked?.Invoke(this, new VertexEventArgs());  
       // }

        public AlarmSystemGraphZoneVertex() : base()
        {
                
        }

        public AlarmSystemGraphZoneVertex(string name, string uri) : base(name,uri)
        {
           
        }
    }

    public  class AlarmSystemGraphVertex
    {
        public string Name { get; set; }
        public string IconUri { get; set; }
        public event EventHandler<VertexEventArgs> VertexClicked;

        public ICommand VertexClickedCommand { get; set; }

        public AlarmSystemGraphVertex()
        {
          //  VertexClicked = new EventHandler<VertexEventArgs>();
            VertexClickedCommand = new DelegateCommand(OnVertexClicked);
        }

        public AlarmSystemGraphVertex(string name, string uri) : this()
        {
            Name = name;
            IconUri = uri;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", Name, Name);
        }

        protected virtual void OnVertexClicked()
        {
            VertexClicked?.Invoke(this, new VertexEventArgs());
        }
    }

}
