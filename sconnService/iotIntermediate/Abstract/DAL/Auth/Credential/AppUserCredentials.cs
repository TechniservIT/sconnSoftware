﻿using iodash.Models.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iodash.Models.Auth.Credential
{
    public class AppUserCredentials
    {
        [Key]
        [Required]
        public int CredentialId { get; set; }

        [Required]
        public virtual User CredentialUser { get; set; }

        [Required]

        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public byte[] HashData { get; set; }

        public DateTime PermissionExpireDate { get; set; }

        public DateTime PasswordExpireDate { get; set; }


    }
}