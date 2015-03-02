using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using iodash.Models.Common;
using iodash.Models.Application;
using iodash.Models.Application.Users;
using iotDash.DAL.Domain;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace iodash.DAL
{
    /*
 
        //ApplicationDbContext partial
    public  class  iotDeviceContext : DbContext
    {
        public iotDeviceContext()
            : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

     

        //public DbSet<User> Users { get; set; }    USE ASP IDENTITY INSTEAD


    }
     * */
}