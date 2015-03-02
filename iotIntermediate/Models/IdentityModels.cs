using iodash.Models.Application.Users;
using iodash.Models.Auth.Credential;
using iodash.Models.Common;
using iotDash.DAL.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace iotDash.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public virtual iotDomain Domain { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Location> Locations { get; set; }

        public DbSet<EndpointInfo> Endpoints { get; set; }

        public DbSet<DeviceParameter> Parameters { get; set; }

        public DbSet<DeviceProperty> Properties { get; set; }

        public DbSet<DeviceAction> Actions { get; set; }

        public DbSet<DeviceType> Types { get; set; }

        public DbSet<DeviceCredentials> Credentials { get; set; }

        public DbSet<Device> Devices { get; set; }

        public DbSet<Site> Sites { get; set; }

        public DbSet<iotDomain> Domains { get; set; }

        public DbSet<UserPermission> Permissions { get; set; }


    }
}