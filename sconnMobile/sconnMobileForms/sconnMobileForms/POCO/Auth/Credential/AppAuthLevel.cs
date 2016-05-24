using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iotDbConnector.DAL
{

    public enum AuthMethod
    {
        Password,
        Certificate
    }

    public class AppAuthLevel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public bool Write { get; set; }

        public bool Read { get; set; }

        public virtual List<DeviceCredentials> Credentials { get; set; }

    }

}