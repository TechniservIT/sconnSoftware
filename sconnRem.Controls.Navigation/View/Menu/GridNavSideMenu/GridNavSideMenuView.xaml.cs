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
using sconnPrismGenerics.View.Interface;
using sconnRem.Navigation;

namespace sconnRem.View.Menu.GridNavSideMenu
{
    //[ViewExport(RegionName = GlobalViewRegionNames.RNavigationRegion)]
    //[PartCreationPolicy(CreationPolicy.NonShared)]
    [Export]
    [ViewSortHint("01")]
    public partial class GridNavSideMenuView : UserControl, IPartImportsSatisfiedNotification, IAlarmSystemStatusView
    {
        private Logger _nlogger = LogManager.GetCurrentClassLogger();
        private static Uri _TargetNavUri = new Uri("AuthConfigView", UriKind.Relative);

        [Import]
        public IRegionManager RegionManager;

        public GridNavSideMenuView()
        {
            InitializeComponent();
        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            try
            {
                IRegion mainContentRegion = this.RegionManager.Regions[GlobalViewRegionNames.RNavigationRegion];
                if (mainContentRegion != null && mainContentRegion.NavigationService != null)
                {
                    mainContentRegion.NavigationService.Navigated += this.MainContentRegion_Navigated;
                }
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex,ex.Message);
            }

        }

        public void MainContentRegion_Navigated(object sender, RegionNavigationEventArgs e)
        {

        }

        private void Nav_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.RegionManager.RequestNavigate(GlobalViewRegionNames.RNavigationRegion, _TargetNavUri
                    ,
                    (NavigationResult nr) =>
                    {
                        var error = nr.Error;
                        var result = nr.Result;
                        if (error != null)
                        {
                            _nlogger.Error(error);
                        }
                    });
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }

        }

        

    }
}
