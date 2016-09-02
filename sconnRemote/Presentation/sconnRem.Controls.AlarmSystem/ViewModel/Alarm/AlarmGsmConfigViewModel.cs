﻿using AlarmSystemManagmentService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract;
using sconnRem.ViewModel.Generic;
using sconnConnector.POCO.Config;
using Prism.Mvvm;
using System.ComponentModel.Composition;
using System.Windows;
using NLog;
using Prism.Regions;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Controls.AlarmSystem.ViewModel.Generic;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;

namespace sconnRem.ViewModel.Alarm
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmGsmConfigViewModel : GenericAlarmConfigViewModel
    {

        public ObservableCollection<sconnGsmRcpt> _config { get; set; }
        public ObservableCollection<sconnGsmRcpt> Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged();
            }
        }

        private GsmConfigurationService _provider;
        private AlarmSystemConfigManager _manager;
        private IAlarmSystemNavigationService AlarmNavService { get; set; }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
                if (_selectedIndex < Config.Count)
                {
                    Application.Current.Dispatcher.Invoke(() => { OpenEntityEditContext(Config[_selectedIndex]); });
                }
            }
        }

        public ICommand EntitySelected;
        public ICommand ConfigureEntityCommand;

        public void OpenEntityEditContext(sconnGsmRcpt device)
        {
            NavigationParameters parameters = new NavigationParameters();
            if (device != null)
            {
                parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);
                parameters.Add(AlarmSystemEntityListContractNames.Alarm_Contract_Entity_GsmRcpt_Edit_Context_Key_Name, device.Id);
            }
            else
            {
                parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);
            }

            GlobalNavigationContext.NavigateRegionToContractWithParam(
                GlobalViewRegionNames.RNavigationRegion,
                GlobalViewContractNames.Global_Contract_Menu_RightSide_AlarmGsmRcptEditListItemContext,
                parameters
                );
        }

        public override void GetData()
        {
            try
            {

                if (AlarmNavService.Online)
                {

                    Config = new ObservableCollection<sconnGsmRcpt>(_provider.GetAll());
                }
                else
                {
                    Config = new ObservableCollection<sconnGsmRcpt>(AlarmNavService.alarmSystemConfigManager.Config.GsmConfig.Rcpts);
                }
                SelectedIndex = 0; //reset on refresh
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public override void SaveData()
        {
            foreach (var item in Config)
            {
                _provider.Update(item);
            }
        }

        public AlarmGsmConfigViewModel()
        {
            _name = "Gsm";
            this._provider = new GsmConfigurationService(_manager);
        }


        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                siteUUID = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
                this.navigationJournal = navigationContext.NavigationService.Journal;

                BackgroundWorker bgWorker = new BackgroundWorker();
                bgWorker.DoWork += (s, e) => {
                    GetData();
                };
                bgWorker.RunWorkerCompleted += (s, e) =>
                {

                    Loading = false;
                };

                Loading = true;

                bgWorker.RunWorkerAsync();

            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }


        }


        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmConfig_Contract_GsmRcptConfigView))
            {
                return true;    //singleton
            }
            return false;
        }

        [ImportingConstructor]
        public AlarmGsmConfigViewModel(IRegionManager regionManager, IAlarmSystemNavigationService NavService)
        {
            Config = new ObservableCollection<sconnGsmRcpt>();
            AlarmNavService = NavService;
            this._manager = AlarmNavService.alarmSystemConfigManager;
            this._provider = new GsmConfigurationService(_manager);
            this._regionManager = regionManager;
        }
        
        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/tel.png"; }
        }

    }
}
