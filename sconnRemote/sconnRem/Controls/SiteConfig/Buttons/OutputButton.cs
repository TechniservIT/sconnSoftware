﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Xml.Linq;
using System.Xml;

namespace sconnRem
{
    public class OutputButton : Button
    {
        private int _outputNumber;

        public int OutputNumber
        {
            get { return _outputNumber; }
            set { if (value > 0) { _outputNumber = value; } }
        }

    }
}
