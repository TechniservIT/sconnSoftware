﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iotDbConnector.DAL
{
    public interface IiotRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetById(int id);
        int Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
       
    }

   
}
