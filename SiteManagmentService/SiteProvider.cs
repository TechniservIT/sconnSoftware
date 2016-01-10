using iotDatabaseConnector.DAL.Repository;
using iotDbConnector.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iotDatabaseConnector.DAL.Repository.Connector.Entity;
using NLog;

namespace SiteManagmentService
{
    public class SiteProvider : ISiteManagmentService
    {
        private IIotContextBase context;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public SiteProvider(IIotContextBase cont)
        {
            this.context = cont;
        }


        public bool Add(Site entity)
        {
            try
            {
                var added = context.Sites.Add(entity);
                context.SaveChanges();
                return context.Sites.Contains(added);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }
        }

        public bool Remove(Site entity)
        {
            try
            {
                var added = context.Sites.Remove(entity);
                context.SaveChanges();
                return !context.Sites.Contains(added);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }

        public List<Site> GetAll()
        {
            try
            {
                var res = context.Sites.ToList();
                return res;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

        public bool Update(Site entity)
        {
            try
            {
                var Site = context.Sites.FirstOrDefault(d => d.Id == entity.Id);
                if (Site != null)
                {
                    Site.Load(entity);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }
        }

        public Site GetById(int Id)
        {
            try
            {
                var Site = context.Sites.FirstOrDefault(d => d.Id == Id);
                return Site;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return null;
            }
        }

        public bool RemoveById(int Id)
        {
            try
            {
                var dev = this.GetById(Id);
                if (dev != null)
                {
                    return this.Remove(dev);
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return false;
            }

        }
        public bool RemoveSite(int id)
        {
            try
            {
                Site dev = context.Sites.First(s => s.Id == id);
                if (dev != null)
                {
                    context.Sites.Remove(dev);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

       
    }
}
