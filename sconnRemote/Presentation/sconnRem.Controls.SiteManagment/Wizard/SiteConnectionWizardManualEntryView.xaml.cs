﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NLog;
using Prism.Regions;
using sconnRem.Navigation;

namespace sconnRem.Controls.SiteManagment.Wizard
{
    [Export]    //(SiteManagmentRegionNames.SiteConnectionWizard_Contract_ManualEntry_View)
    [ViewSortHint("01")]
    public partial class SiteConnectionWizardManualEntryView : UserControl
    {
        private Logger _nlogger = LogManager.GetCurrentClassLogger();

        [Import] public IRegionManager RegionManager;

        [ImportingConstructor]
        public SiteConnectionWizardManualEntryView(SiteConnectionWizardViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

        public SiteConnectionWizardManualEntryView()
        {
                    
        }
    }

}
