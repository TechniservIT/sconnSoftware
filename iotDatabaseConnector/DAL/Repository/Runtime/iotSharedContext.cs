using iotDatabaseConnector.DAL.POCO.Device.Notify;
using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDatabaseConnector.DAL.Repository.Runtime
{
    public class iotSharedEntityContext<T> : IiotRepository<T> where T : class
    {

        public iotSharedEntityContext()
        {

        }


        public virtual IQueryable<T> GetAll()
        {
            try
            {
                return iotGenericGlobalContext<T>.DbContext.Set<T>();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public virtual T GetById(int id)
        {
            try
            {
                return iotGenericGlobalContext<T>.DbSet.Find(id);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        //public virtual T GetById(string id)
        //{
        //    try
        //    {
        //        return iotGenericGlobalContext<T>.DbSet.Find(id);
        //    }
        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}


        public virtual int Add(T entity)
        {
            try
            {
                DbEntityEntry dbEntityEntry = iotGenericGlobalContext<T>.DbContext.Entry(entity);
                if (dbEntityEntry.State != EntityState.Detached)
                {
                    dbEntityEntry.State = EntityState.Added;
                }
                else
                {
                    var res = iotGenericGlobalContext<T>.DbSet.Add(entity);
                    iotGenericGlobalContext<T>.DbContext.SaveChanges();
                    System.Reflection.PropertyInfo pi = res.GetType().GetProperty("Id");
                    if (pi != null)
                    {
                        int id = (int)(pi.GetValue(res, null));
                        return id;
                    }
                }
                return -1;
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

                DbEntityEntry dbEntityEntry = iotGenericGlobalContext<T>.DbContext.Entry(entity);
                if (dbEntityEntry.State == EntityState.Detached)
                {
                    iotGenericGlobalContext<T>.DbSet.Attach(entity);
                }
                dbEntityEntry.State = EntityState.Modified;
                //iotGenericGlobalContext<T>.DbContext.SaveChanges();
            }
            catch (Exception exc)
            {
                throw;
            }

        }


        public virtual void Update(T entity)
        {
            try
            {
                if (entity.GetType() == typeof(Device) || entity.GetType() == typeof(DeviceAction) || entity.GetType() == typeof(DeviceProperty))
                {
                    UpdateWithHistory(entity);
                }
                else
                {
                    DbEntityEntry dbEntityEntry = iotGenericGlobalContext<T>.DbContext.Entry(entity);
                    if (dbEntityEntry.State == EntityState.Detached)
                    {
                        iotGenericGlobalContext<T>.DbSet.Attach(entity);
                    }
                    dbEntityEntry.State = EntityState.Modified;
                }
                //iotGenericGlobalContext<T>.DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public virtual void Delete(T entity)
        {
            try
            {
                DbEntityEntry dbEntityEntry = iotGenericGlobalContext<T>.DbContext.Entry(entity);
                if (dbEntityEntry.State != EntityState.Deleted)
                {
                    dbEntityEntry.State = EntityState.Deleted;
                }
                else
                {
                    iotGenericGlobalContext<T>.DbSet.Attach(entity);
                    iotGenericGlobalContext<T>.DbSet.Remove(entity);
                }
            }
            catch (Exception e)
            {
                throw;
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
