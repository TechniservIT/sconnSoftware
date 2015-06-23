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
        bool AddWithId(T entity, string Id);
        void Update(T entity);
        bool UpdateById(T entity, string Id);
        void Delete(T entity);
        void Delete(string id);
    }

}
