using AlarmSystemManagmentService;
using sconnRem.ViewModel.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sconnConnector.Config;
using sconnConnector.POCO.Config.Abstract;
using System.Windows.Input;
using sconnConnector.POCO.Config.sconn;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;

namespace sconnRem.ViewModel.Alarm
{
    public class AlarmAuthConfigViewModel : ObservableObject, IPageViewModel    //  :  ViewModelBase<IGridNavigatedView>
    {
        public ObservableCollection<sconnAuthorizedDevice> AuthorizedDevices { get; set; }
        private AuthorizedDevicesConfigurationService _Provider;

        [Dependency]
        public AlarmSystemConfigManager _Manager { get; set; }
        

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
        }
        
        private ICommand _getDataCommand;
        private ICommand _saveDataCommand;

        private void GetData()
        {
            AuthorizedDevices.Clear();
            var retr = _Provider.GetAll();
            foreach (var item in retr)
            {
                AuthorizedDevices.Add(item);
            }
        }

        private void SaveData()
        {
            _Provider.SaveChanges();
        }

        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/lista2.png"; }
        }



        public AlarmAuthConfigViewModel()
        {
            _Name = "Auth";
            AuthorizedDevices = new ObservableCollection<sconnAuthorizedDevice>();
            AuthorizedDevices.Add(new sconnAuthorizedDevice());
            AuthorizedDevices.Add(new sconnAuthorizedDevice());
            AuthorizedDevices.Add(new sconnAuthorizedDevice());
            AuthorizedDevices.Add(new sconnAuthorizedDevice());
            this._Provider = new AuthorizedDevicesConfigurationService(_Manager);
        }

        public AlarmAuthConfigViewModel(AlarmSystemConfigManager Manager)
        {
            AuthorizedDevices = new ObservableCollection<sconnAuthorizedDevice>();
            AuthorizedDevices.Add(new sconnAuthorizedDevice());
            AuthorizedDevices.Add(new sconnAuthorizedDevice());
            _Manager = Manager;
            _Name = "Auth";
            this._Provider = new AuthorizedDevicesConfigurationService(_Manager);
          //  GetData();
            

        }
        
    }



    //public class NameList : ObservableCollection<PersonName>
    //{
    //    public NameList() : base()
    //    {
    //        Add(new PersonName("Willa", "Cather"));
    //        Add(new PersonName("Isak", "Dinesen"));
    //        Add(new PersonName("Victor", "Hugo"));
    //        Add(new PersonName("Jules", "Verne"));
    //    }
    //}

    //public class PersonName
    //{
    //    private string firstName;
    //    private string lastName;

    //    public PersonName(string first, string last)
    //    {
    //        this.firstName = first;
    //        this.lastName = last;
    //    }

    //    public string FirstName
    //    {
    //        get { return firstName; }
    //        set { firstName = value; }
    //    }

    //    public string LastName
    //    {
    //        get { return lastName; }
    //        set { lastName = value; }
    //    }
    //}



}
