﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteManagmentService
{
    public interface IEntityStorage
    {
        void Save();
        void Load();
    }

}
