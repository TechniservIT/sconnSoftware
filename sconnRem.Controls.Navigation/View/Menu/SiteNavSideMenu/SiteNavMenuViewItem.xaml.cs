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
using sconnConnector.POCO.Config;
using sconnRem.Navigation;

namespace sconnRem.Controls.Navigation.View.Menu.SiteNavSideMenu
{
    
    [Export]
    [ViewSortHint("01")]
    public partial class SiteNavMenuViewItem : UserControl, IPartImportsSatisfiedNotification
    {
        private Logger _nlogger = LogManager.GetCurrentClassLogger();
        private static Uri _authViewUri = new Uri("AuthConfigView", UriKind.Relative);

        public sconnSite Site { get; set; }

        [Import]
        public IRegionManager RegionManager;

        public SiteNavMenuViewItem()
        {
            InitializeComponent();
        }

        [ImportingConstructor]
        public SiteNavMenuViewItem(sconnSite site)
        {
            Site = site;
            InitializeComponent();

        }
        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            IRegion mainContentRegion = this.RegionManager.Regions[""];
            if (mainContentRegion != null && mainContentRegion.NavigationService != null)
            {
                mainContentRegion.NavigationService.Navigated += this.MainContentRegion_Navigated;
            }
        }

        public void MainContentRegion_Navigated(object sender, RegionNavigationEventArgs e)
        {

        }

        private void Nav_Button_Click(object sender, RoutedEventArgs e)
        {
            //this.RegionManager.RequestNavigate(GlobalViewRegionNames.MainContentRegion, _authViewUri
            //    ,
            //    (NavigationResult nr) =>
            //    {
            //        var error = nr.Error;
            //        var result = nr.Result;
            //        if (error != null)
            //        {
            //            _nlogger.Error(error);
            //        }
            //    });
        }


    }

    
}
