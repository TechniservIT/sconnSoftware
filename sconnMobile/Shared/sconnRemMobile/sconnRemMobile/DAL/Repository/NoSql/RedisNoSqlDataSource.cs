using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Reflection;
using StackExchange.Redis.Extensions.Newtonsoft;
using StackExchange.Redis.Extensions.Core;

namespace iotNoSqlDatabase
{
    public class RedisNoSqlDataSource<T> : INoSqlDataSource<T> where T : class
    {

        private IDatabase db;

        private NewtonsoftSerializer serializer = new NewtonsoftSerializer();
        private StackExchangeRedisCacheClient cacheClient;

        public RedisNoSqlDataSource()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
            db = redis.GetDatabase();
            cacheClient = new StackExchangeRedisCacheClient(serializer);
        }

        /********* Serialize/deserialize ********/
        private static List<HashEntry> ConvertToHashEntryList(object instance)
        {
            var propertiesInHashEntryList = new List<HashEntry>();
            foreach (var property in instance.GetType().GetProperties())
            {
                if (!property.Name.Equals("ObjectAdress"))
                {
                    //is a virtual property - evaluate tree 
                    if (property.GetMethod.IsVirtual)
                    {

                    }

                    //
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

        private static T GetFromEntryList(List<HashEntry> Entries)
        {
            T parsedEntity = (T)Activator.CreateInstance(typeof(T));
            foreach (var property in parsedEntity.GetType().GetProperties())
            {
                //try to find matching property in resultset
                foreach (var entry in Entries)
                {
                    if(entry.Name.Equals(property.Name) )
                    {
                        try
                        {
                            property.SetValue(parsedEntity, Convert.ChangeType(entry.Value, property.PropertyType));
                        }
                        catch (Exception ex)
                        {

                        }
                       
                    }
                }
            }
            return parsedEntity;
        }

        public string StoreEntityExt(T entity)
        {
            string id = Guid.NewGuid().ToString();
            bool added = cacheClient.Add(id, entity, DateTimeOffset.MaxValue);
            return id;
        }

        public bool StoreEntityWithId(T entity, string Id)
        {
            bool added = cacheClient.Add(Id, entity, DateTimeOffset.MaxValue);
            return added;
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
            var resultset = cacheClient.Get<T>(id);
            return resultset;
            /*
            var hashes = db.HashGetAll(id);
            return GetFromEntryList(hashes.ToList());
            */
        }
        

        public virtual string Add(T entity)
        {
            //string id = Guid.NewGuid().ToString();
            //var propertyList = ConvertToHashEntryList(entity);
            //db.HashSet(id, propertyList.ToArray());
            //return id;

            // db.SetAdd(Guid.NewGuid().ToString(), JsonConvert.SerializeObject(entity));

            return StoreEntityExt(entity);
        }

        public virtual bool AddWithId(T entity, string Id)
        {
            return StoreEntityWithId(entity,Id);
        }

        public virtual void Update(T entity)
        {
           
        }

        public virtual bool UpdateById(T entity, string Id)
        {
            bool replaced = cacheClient.Replace(Id, entity);
            return replaced;
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
