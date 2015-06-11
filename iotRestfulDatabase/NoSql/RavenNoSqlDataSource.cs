using iotDbConnector.DAL;
using Newtonsoft.Json;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Connection;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Database.Server;
using Raven.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotNoSqlDatabase
{

    public class RavenNoSqlDataSource<T> : INoSqlDataSource<T> where T : class
    {
        private EmbeddableDocumentStore dsrc;
        private IDocumentStore dstore;
        public IDocumentSession RavenSession { get; protected set; }

        public RavenNoSqlDataSource()   
        {
            //dsrc = new EmbeddableDocumentStore();
            //dsrc.Configuration.RunInUnreliableYetFastModeThatIsNotSuitableForProduction = true;
            //dsrc.Configuration.VirtualDirectory = "/db";
            //dsrc.RunInMemory = true;

            //NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8080);

            //dsrc.Configuration.Storage.Voron.AllowOn32Bits = true;
            //dsrc.DataDirectory = "nsqlDbo";
            //dsrc.Configuration.Port = 8080;
            //dsrc.UseEmbeddedHttpServer = true;

            //dsrc.Initialize();
        }

        public Device GetDevice()
        {
            using (IDocumentStore store = new DocumentStore
            {
                Url = "http://localhost:8080/"
            }.Initialize())
            {
                JsonDocument document = store.DatabaseCommands.Get("devices/2"); // null if does not exist
                var robj = document.ToJson();
                Device dev = JsonConvert.DeserializeObject<Device>(robj.ToString());
                return dev;
            }
        }


        public virtual IQueryable<T> GetAll()
        {
            using (IDocumentStore store = new DocumentStore
            {
                Url = "http://localhost:8080/"
            }.Initialize())
            {
                IDatabaseCommands commands = store.DatabaseCommands;
                using (IDocumentSession session = store.OpenSession())
                {
                    return session.Query<T>();
                }
            }
            
        }

        public virtual T GetById(string id)
        {
            throw new NotImplementedException();
        }

        public virtual string Add(T entity)
        {
            using (IDocumentStore store = new DocumentStore
            {
                Url = "http://localhost:8080/"
            }.Initialize())
            {
                int id = Guid.NewGuid().GetHashCode();

                    PutResult  res =   store.DatabaseCommands
                        .Put(
                            "devices/2",
                            null,
                            RavenJObject.FromObject(new Device
                            {
                                DeviceName = "sconnGKP",
                                DeviceId = id
                            }),
                            new RavenJObject());

                return id.ToString();
            }
          
            
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
