﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sconnConnector.POCO.Config.Abstract.IO
{
    public abstract class AlarmSystemOutput
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public int Value { get; set; }


        public bool Enabled { get; set; }

        public string Name { get; set; }
    }

}
