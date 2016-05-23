using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphSharp.Controls;
using QuickGraph;

namespace sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.Graph
{
    public class AlarmSystemGraphLayout : GraphLayout<AlarmSystemGraphVertex, AlarmSystemGraphEdge, AlarmSystemGraph> { }

    public class AlarmSystemGraph : BidirectionalGraph<AlarmSystemGraphVertex, AlarmSystemGraphEdge>
    {
        public AlarmSystemGraph() { }

        public AlarmSystemGraph(bool allowParallelEdges)
            : base(allowParallelEdges) { }

        public AlarmSystemGraph(bool allowParallelEdges, int vertexCapacity)
            : base(allowParallelEdges, vertexCapacity) { }
    }

}
