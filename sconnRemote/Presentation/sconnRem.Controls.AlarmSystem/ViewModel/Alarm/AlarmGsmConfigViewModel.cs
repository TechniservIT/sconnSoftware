﻿using AlarmSystemManagmentService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class AlarmGsmConfigViewModel : GenericAsyncConfigViewModel
    {
        public ObservableCollection<sconnGsmRcpt> Config { get; set; }
        private GsmConfigurationService _provider;
        private AlarmSystemConfigManager _manager;
      

        private ICommand _getDataCommand;
        private ICommand _saveDataCommand;

        public override void GetData()
        {
            try
            {
                Config = new ObservableCollection<sconnGsmRcpt>(_provider.GetAll());

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

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Uri.OriginalString.Equals(AlarmRegionNames.AlarmConfig_Contract_GsmRcptConfigView))
            {
                return true;    //singleton
            }
            return false;
        }

        [ImportingConstructor]
        public AlarmGsmConfigViewModel(IRegionManager regionManager)
        {
            Config = new ObservableCollection<sconnGsmRcpt>();
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new GsmConfigurationService(_manager);
            this._regionManager = regionManager;
            GetData();
        }
        
        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/tel.png"; }
        }

    }
}
