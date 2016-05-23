﻿using iotDatabaseConnector.DAL.POCO.Device.Notify;
using NLog;
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

    public class iotRepository<T> : IiotRepository<T> where T : class
    {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

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
                _logger.Error(e, e.Message);
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
                _logger.Error(e, e.Message);
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
                _logger.Error(e, e.Message);
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
                return 0;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
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
                                    ActionChangeHistory hist = new ActionChangeHistory();
                                    hist.Date = DateTime.Now;
                                    hist.Property = param;
                                    hist.Value = param.Value;
                                    iotRepository<ActionChangeHistory> histrepo = new iotRepository<ActionChangeHistory>();
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
                                    ActionChangeHistory hist = new ActionChangeHistory();
                                    hist.Date = DateTime.Now;
                                    hist.Property = param;
                                    hist.Value = param.Value;
                                    iotRepository<ActionChangeHistory> histrepo = new iotRepository<ActionChangeHistory>();
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
            catch (Exception e)
            {
                _logger.Error(e, e.Message);

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
                _logger.Error(e, e.Message);
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
                _logger.Error(e, e.Message);
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
