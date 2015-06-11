using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace iotNoSqlDatabase
{
    public class RedisNoSqlDataSource<T> : INoSqlDataSource<T> where T : class
    {

        private IDatabase db;

        public RedisNoSqlDataSource()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
            db = redis.GetDatabase();
        }

        /********* Serialize/deserialize ********/
        private static List<HashEntry> ConvertToHashEntryList(object instance)
        {
            var propertiesInHashEntryList = new List<HashEntry>();
            foreach (var property in instance.GetType().GetProperties())
            {
                if (!property.Name.Equals("ObjectAdress"))
                {
                    if ( property.GetValue(instance) != null)
                    {
                        propertiesInHashEntryList.Add(new HashEntry(property.Name, instance.GetType().GetProperty(property.Name).GetValue(instance).ToString()));
                    }
                    
                }
                else
                {
                    var subPropertyList = ConvertToHashEntryList(instance.GetType().GetProperty(property.Name).GetValue(instance));
                    propertiesInHashEntryList.AddRange(subPropertyList);
                }
            }
            return propertiesInHashEntryList;
        }

        private static T GetFromEntryList(object instance)
        {
            T parsedEntity = (T)Activator.CreateInstance(typeof(T));
            foreach (var property in instance.GetType().GetProperties())
            {
                
            }
            return parsedEntity;
        }



        public Device GetDevice()
        {
            throw new NotImplementedException();
        }


        public virtual IQueryable<T> GetAll()
        {
            var hashes = db.HashGetAll("");
            return null;
           // db.SetAdd(Guid.NewGuid().ToString(), JsonConvert.SerializeObject(entity));
         
        }

        public virtual T GetById(string id)
        {
            var hashes = db.HashGetAll(id);
            return GetFromEntryList(hashes);
        }
        

        public virtual string Add(T entity)
        {
            var propertyList = ConvertToHashEntryList(entity);
            string id = Guid.NewGuid().ToString();
            db.HashSet(id, propertyList.ToArray());
            return id;

           // db.SetAdd(Guid.NewGuid().ToString(), JsonConvert.SerializeObject(entity));
        }

        public virtual void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(string id)
        {
            throw new NotImplementedException();
        }

    }
}
