using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.POCO.Device.Notify;
using iotDbConnector.DAL;
using NLog;

namespace iotDatabaseConnector.DAL.Repository
{
    public class TestDbSet<TEntity> : DbSet<TEntity>, IQueryable, IEnumerable<TEntity>, IDbAsyncEnumerable<TEntity>
    where TEntity : class
    {

        ObservableCollection<TEntity> _data;
        IQueryable _query;

        public override TEntity Find(params object[] keyValues)
        {
            try
            {
                if (_data.Count > 0)
                {
                    var id = (int)keyValues.Single();
                    foreach (var item in _data)
                    {
                        PropertyInfo prop = item.GetType().GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanRead)
                        {
                            int propIdVal = (int)prop.GetValue(item);
                            if (propIdVal == id)
                            {
                                return item;
                            }
                        }

                        //var fakeables = _data.GetType().GetProperties();
                        //foreach (PropertyInfo prop in fakeables)
                        //{
                        //    if (prop.Name.Equals("Id"))
                        //    {
                        //        int propIdVal = (int)prop.GetValue(item);
                        //        if (propIdVal == id)
                        //        {
                        //            return item;
                        //        }
                        //    }
                        //}
                    }
                }
                return null;
            }
            catch (Exception e)
            {

                return null;
            }
            //return _data.SingleOrDefault(b => b.Id == id);
        }

        public TestDbSet()
        {
            _data = new ObservableCollection<TEntity>();
            _query = _data.AsQueryable();
        }

        public override TEntity Add(TEntity item)
        {
            _data.Add(item);
            return item;
        }

        public override TEntity Remove(TEntity item)
        {
            _data.Remove(item);
            return item;
        }

        public override TEntity Attach(TEntity item)
        {
            _data.Add(item);
            return item;
        }

        public override TEntity Create()
        {
            return Activator.CreateInstance<TEntity>();
        }

        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public override ObservableCollection<TEntity> Local
        {
            get { return _data; }
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<TEntity>(_query.Provider); }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IDbAsyncEnumerator<TEntity> IDbAsyncEnumerable<TEntity>.GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<TEntity>(_data.GetEnumerator());
        }
    }

    internal class TestDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestDbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }

    internal class TestDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<T>(this); }
        }
    }

    internal class TestDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }

    public class IotFakeRepository<T> : IiotRepository<T> where T : class
    {

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        protected TestDbSet<T> DbSet { get; set; }

        public IotFakeRepository()
        {
            DbSet = new TestDbSet<T>();

        }

        public void ClearIotRepository()
        {
            DbSet = new TestDbSet<T>();
        }


        public IotFakeRepository(DbContext dbContext)
        {
            DbSet = new TestDbSet<T>(); // DbContext.Set<T>();
        }
        

        public virtual IQueryable<T> GetAll()
        {
            try
            {
                return DbSet;
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
                DbSet.Add(entity);
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

                DbSet.Attach(entity);
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
                if (entity.GetType() == typeof(Device) || entity.GetType() == typeof(DeviceAction) || entity.GetType() == typeof(DeviceProperty))
                {
                    UpdateWithHistory(entity);
                }
                else
                {
                    DbSet.Attach(entity);
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
                DbSet.Remove(entity);
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
