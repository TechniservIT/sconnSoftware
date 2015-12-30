using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmSystemManagmentService
{
    public interface IAlarmSystemConfigurationService<T> 
    {
         bool Add(T entity);
         bool Remove(T entity);
         List<T> GetAll();
         bool Update(T entity);
         T GetById(int Id);
         bool RemoveById(int Id);
    }
}
