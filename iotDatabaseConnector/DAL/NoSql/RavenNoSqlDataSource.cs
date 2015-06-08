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

namespace iotDbConnector.DAL
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

        public virtual T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public virtual void Add(T entity)
        {
            using (IDocumentStore store = new DocumentStore
            {
                Url = "http://localhost:8080/"
            }.Initialize())
            {
               
                    PutResult  res =   store.DatabaseCommands
                        .Put(
                            "devices/2",
                            null,
                            RavenJObject.FromObject(new Device
                            {
                                DeviceName = "sconnGKP",
                                DeviceId = Guid.NewGuid().GetHashCode()
                            }),
                            new RavenJObject());
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

        public virtual void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }

}
