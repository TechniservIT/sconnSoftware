using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmSystemManagmentService.Abstract
{
    public interface IEntityService<T>
    {
        bool Add(T entity);
        bool Remove(T entity);
        List<T> GetAll();
        bool Update(T entity);
        T GetById(int Id);
        bool RemoveById(int Id);
    }

}

