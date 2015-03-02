using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDomainController.Logic
{
    public interface ITimeDomainOperand
    {

        bool HasChangedSinceTime(DateTime time);

        int TimesChangedSinceTime(DateTime time);

        int AverageChangePeriodSeconds();



    }
}
