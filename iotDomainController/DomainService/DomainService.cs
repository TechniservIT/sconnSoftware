using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDomainController.DomainService
{
    public class DomainService
    {
        DomainEventService EventsServ;

        DomainUpdateService UpdateServ;

        public DomainService()
        {
            EventsServ = new DomainEventService();
            UpdateServ = new DomainUpdateService();


        }


    }
}
