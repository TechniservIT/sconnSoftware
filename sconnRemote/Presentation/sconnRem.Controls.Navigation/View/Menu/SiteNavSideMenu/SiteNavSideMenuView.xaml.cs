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
using sconnRem.Controls.Navigation.View.Menu.SiteNavSideMenu;
using sconnRem.Controls.Navigation.ViewModel.Navigation;
using sconnRem.Navigation;

namespace sconnRem.View.Menu.SiteNavSideMenu
{
    
    [Export]
    [ViewSortHint("01")]
    public partial class SiteNavSideMenuView : UserControl, IPartImportsSatisfiedNotification
    {
        private Logger _nlogger = LogManager.GetCurrentClassLogger();
        private static Uri _TargetNavUri = new Uri("AuthConfigView", UriKind.Relative);
        public List<SiteNavMenuViewItem> Items { get; set; }

        [Import]
        public IRegionManager RegionManager;

        public SiteNavSideMenuView()
        {
            Items = new List<SiteNavMenuViewItem>();
            InitializeComponent();
        }

        [ImportingConstructor]
        public SiteNavSideMenuView(SiteNavViewModel viewModel)
        {
            Items = new List<SiteNavMenuViewItem>();
            this.DataContext = viewModel;
            foreach (var site in ((SiteNavViewModel)this.DataContext).Sites)
            {
                this.Items.Add(new SiteNavMenuViewItem(site));
            }
            InitializeComponent();
        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            try
            {
                IRegion mainContentRegion = this.RegionManager.Regions[GlobalViewRegionNames.LNavigationRegion];
                if (mainContentRegion != null && mainContentRegion.NavigationService != null)
                {
                    mainContentRegion.NavigationService.Navigated += this.MainContentRegion_Navigated;
                }
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }

        }

        public void MainContentRegion_Navigated(object sender, RegionNavigationEventArgs e)
        {

        }

        private void Nav_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.RegionManager.RequestNavigate(GlobalViewRegionNames.MainGridContentRegion, _TargetNavUri
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

        private void OrdersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }


}
