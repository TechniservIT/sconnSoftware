using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnPrismGenerics.Boostrapper
{
    public interface IVerifiableBootstraper
    {
        CompositionContainer GetContainer();
        AggregateCatalog GetAggregateCatalog();
    }


}
