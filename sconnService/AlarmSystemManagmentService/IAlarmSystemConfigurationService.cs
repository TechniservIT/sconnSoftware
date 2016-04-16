using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IotServiceAbstract;

namespace AlarmSystemManagmentService
{
    public interface IAlarmSystemConfigurationService<T> : IEntityService<T>
    {
         bool Online { get; set; }
    }

    public interface IAlarmSystemSingleEntityConfigurationService<T>
    {
        bool Remove(T entity);
        T Get();
        bool Update(T entity);
        bool Online { get; set; }
    }

}
