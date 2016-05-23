using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.Graph
{
    public  class AlarmSystemGraphVertex
    {
        public string ID { get; private set; }
        public bool IsMale { get; private set; }

        public AlarmSystemGraphVertex(string id, bool isMale)
        {
            ID = id;
            IsMale = isMale;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", ID, IsMale);
        }
    }

}
