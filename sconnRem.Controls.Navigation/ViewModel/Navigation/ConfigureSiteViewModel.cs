using sconnConnector.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using sconnRem.Infrastructure;

namespace sconnRem.ViewModel.Navigation
{
    public class ConfigureSiteViewModel : ObservableObject
    {

        private AlarmSystemConfigManager _manager;


        #region Fields

        private ICommand _changePageCommand;

        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;

        #endregion

        private void LoadConfigPages()
        {

            // Add available pages
            _pageViewModels = new List<IPageViewModel>();
            //PageViewModels.Add(new AlarmAuthConfigViewModel(_Manager));
            //PageViewModels.Add(new AlarmGlobalConfigViewModel(_Manager));
            //PageViewModels.Add(new AlarmDeviceConfigViewModel(_Manager));
            //PageViewModels.Add(new AlarmGsmConfigViewModel(_Manager));
            //PageViewModels.Add(new AlarmZoneConfigViewModel(_Manager));
            //PageViewModels.Add(new AlarmUsersConfigViewModel(_Manager));

            // Set starting page
            CurrentPageViewModel = PageViewModels[0];
        }

        public ConfigureSiteViewModel()
        {
            LoadConfigPages();
        }

        public ConfigureSiteViewModel(AlarmSystemConfigManager manager)
        {
            _manager = manager;
            LoadConfigPages();
        }



        #region Properties / Commands


        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }

                return _changePageCommand;
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }

        #endregion

        #region Methods

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        #endregion

    }
}
