﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism;
using Prism.Regions;

namespace sconnPrismGenerics.View.Interface
{
    public interface INavigableView : IActiveAware, INavigationAware, IChangeTracking, INotifyPropertyChanged
    {

    }
}
