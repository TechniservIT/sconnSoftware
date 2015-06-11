using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotNoSqlDatabase
{

    public interface INoSqlDataSource<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(string id);
        string Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(string id);
    }

}
