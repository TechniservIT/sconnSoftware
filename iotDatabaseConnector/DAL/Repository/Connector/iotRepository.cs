using iotDatabaseConnector.DAL.POCO.Device.Notify;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDbConnector.DAL
{
    /// <summary>
    /// The EF-dependent, generic repository for data access
    /// </summary>
    /// <typeparam name="T">Type of entity for this Repository.</typeparam>
    public class iotRepository<T> : IiotRepository<T> where T : class
    {
       
        public iotRepository()
        {
            iotContext dbContext = new iotContext();
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
    
        }

        static public void ClearIotRepository()
        {
                iotContext dbContext = new iotContext();
                var tableNames = dbContext.Database.SqlQuery<string>("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME NOT LIKE '%Migration%'").ToList();
                foreach (var tableName in tableNames)
                {
                    dbContext.Database.ExecuteSqlCommand(string.Format("DELETE FROM {0}", tableName));
                }
                dbContext.SaveChanges();
        }


        public iotRepository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("Null DbContext");
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        protected DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        public virtual IQueryable<T> GetAll()
        {
            try
            {
                return DbContext.Set<T>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public virtual T GetById(int id)
        {
            try
            {
                return DbSet.Find(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public virtual T GetById(string id)
        {
            try
            {
                return DbSet.Find(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public virtual int Add(T entity)
        {
            try
            {
                DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
                if (dbEntityEntry.State != EntityState.Detached)
                {
                    dbEntityEntry.State = EntityState.Added;
                }
                //else
                //{
                //    var res = DbSet.Add(entity);
                //    DbContext.SaveChanges();
                //    System.Reflection.PropertyInfo pi = res.GetType().GetProperty("Id");
                //    if (pi != null)
                //    {
                //        int id =  (int)(pi.GetValue(res, null));
                //        return id;
                //    }
                //}
                return 0;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public virtual void UpdateWithHistory(T entity)
        {
            try
            {
                if (entity.GetType() == typeof(Device))
                {
                    Device edited = (Device)(object)entity;
                    iotRepository<DeviceParameter> repo = new iotRepository<DeviceParameter>();
                    iotRepository<Device> devrepo = new iotRepository<Device>();
                    Device devbefore = devrepo.GetById(edited.Id);

                    foreach (var item in edited.Actions)
                    {
                        foreach (var param in item.ResultParameters)
                        {
                            DeviceParameter stparam = repo.GetById(param.Id);
                            if (stparam != null)
                            {
                                if (!stparam.Value.Equals(param.Value))
                                {
                                    ParameterChangeHistory hist = new ParameterChangeHistory();
                                    hist.Date = DateTime.Now;
                                    hist.Property = param;
                                    hist.Value = param.Value;
                                    iotRepository<ParameterChangeHistory> histrepo = new iotRepository<ParameterChangeHistory>();
                                    histrepo.Add(hist);
                                }
                            }
                        }
                    }


                    foreach (var item in edited.Properties)
                    {
                        foreach (var param in item.ResultParameters)
                        {
                            DeviceParameter stparam = repo.GetById(param.Id);
                            if (stparam != null)
                            {
                                if (!stparam.Value.Equals(param.Value))
                                {
                                    ParameterChangeHistory hist = new ParameterChangeHistory();
                                    hist.Date = DateTime.Now;
                                    hist.Property = param;
                                    hist.Value = param.Value;
                                    iotRepository<ParameterChangeHistory> histrepo = new iotRepository<ParameterChangeHistory>();
                                    histrepo.Add(hist);
                                }
                            }
                        }
                    }
                }
                else if (entity.GetType() == typeof(DeviceAction))
                {
                    iotRepository<DeviceParameter> repo = new iotRepository<DeviceParameter>();
                    DeviceAction item = (DeviceAction)(object)entity;
                        foreach (var param in item.ResultParameters)
                        {
                            DeviceParameter stparam = repo.GetById(param.Id);
                            if (stparam != null)
                            {
                                if (!stparam.Value.Equals(param.Value))
                                {
                                    ParameterChangeHistory hist = new ParameterChangeHistory();
                                    hist.Date = DateTime.Now;
                                    hist.Property = param;
                                    hist.Value = param.Value;
                                    iotRepository<ParameterChangeHistory> histrepo = new iotRepository<ParameterChangeHistory>();
                                    histrepo.Add(hist);
                                }
                            }
                        }

                }
                else if (entity.GetType() == typeof(DeviceProperty))
                {
                    iotRepository<DeviceParameter> repo = new iotRepository<DeviceParameter>();
                    DeviceProperty item = (DeviceProperty)(object)entity;

                        foreach (var param in item.ResultParameters)
                        {
                            DeviceParameter stparam = repo.GetById(param.Id);
                            if (stparam != null)
                            {
                                if (!stparam.Value.Equals(param.Value))
                                {
                                    ParameterChangeHistory hist = new ParameterChangeHistory();
                                    hist.Date = DateTime.Now;
                                    hist.Property = param;
                                    hist.Value = param.Value;
                                    iotRepository<ParameterChangeHistory> histrepo = new iotRepository<ParameterChangeHistory>();
                                    histrepo.Add(hist);
                                }
                            }
                        }

                }
                                    
                    DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
                    if (dbEntityEntry.State == EntityState.Detached)
                    {
                        DbSet.Attach(entity);
                    }
                    dbEntityEntry.State = EntityState.Modified;
            }
            catch (Exception exc)
            {
                
           
            }

        }
        

        public virtual void Update(T entity)
        {
            try
            {
                if (entity.GetType() == typeof(Device) || entity.GetType() == typeof(DeviceAction) || entity.GetType() == typeof(DeviceProperty) )
                {
                    UpdateWithHistory(entity);
                }
                else
                {
                    DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
                    if (dbEntityEntry.State == EntityState.Detached)
                    {
                        DbSet.Attach(entity);
                    }
                    dbEntityEntry.State = EntityState.Modified;
                }

            }
            catch (Exception e)
            {
   
            }
        }

        public virtual void Delete(T entity)
        {
            try
            {
                DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
                if (dbEntityEntry.State != EntityState.Deleted)
                {
                    dbEntityEntry.State = EntityState.Deleted;
                }
                else
                {
                    DbSet.Attach(entity);
                    DbSet.Remove(entity);
                }
            }
            catch (Exception e)
            {

            }

        }

        public virtual void Delete(int id)
        {
            var entity = GetById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }


    }
}
