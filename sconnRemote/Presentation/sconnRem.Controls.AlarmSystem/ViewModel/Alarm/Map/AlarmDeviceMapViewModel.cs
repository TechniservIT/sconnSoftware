using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AlarmSystemManagmentService;
using AlarmSystemManagmentService.Device;
using Prism.Commands;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config.sconn;
using sconnRem.Controls.AlarmSystem.View.Status.AlarmSystem.Graph;
using sconnRem.Infrastructure.Navigation;
using sconnRem.Navigation;
using sconnRem.ViewModel.Generic;

namespace sconnRem.Controls.AlarmSystem.ViewModel.Alarm.Map
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AlarmDeviceMapViewModel : GenericAlarmConfigViewModel
    {

        #region Public Properties

        public AlarmSystemGraph Graph
        {
            get { return graph; }
            set
            {
                graph = value;
                OnPropertyChanged();
            }
        }

        #endregion


        private ObservableCollection<sconnDevice> _config;
        public ObservableCollection<sconnDevice> Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged();
            }
        }

        private AlarmDevicesConfigService _provider;
        private AlarmSystemConfigManager _manager;
        public ICommand ConfigureZoneCommand { get; set; }
        public ICommand GraphEntitySelectedCommand { get; set; }
        private AlarmSystemGraph graph;
        private int count;

        public void CreateGraph()
        {
            LoadZoneGraph();
        }

        public void LoadZoneGraph()
        {
            try
            {
                var g = new AlarmSystemGraph();
                string[] vertices = new string[Config.Count + 1];

                vertices[0] = Config.FirstOrDefault().Name;    //create root node
                AlarmSystemGraphVertex v1 = new AlarmSystemGraphVertex(vertices[0], Config.FirstOrDefault().imageIconUri, Config.FirstOrDefault().UUID);
                g.AddVertex(v1);

                for (int i = 0; i < Config.Count; i++)
                {
                    int currentVertIndex = i + 1;
                    vertices[currentVertIndex] = Config[i].Name;
                    AlarmSystemGraphVertex v = new AlarmSystemGraphVertex(vertices[currentVertIndex], Config[i].imageIconUri, Config[i].UUID);
                    v.VertexClicked += V_VertexClicked;
                    g.AddVertex(v);
                    g.AddEdge(new AlarmSystemGraphEdge(v1.Name, v1, v));    //connect child to parent
                }

                Graph = g;
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }
        }

        public void OpenDeviceContextForDevice(sconnDevice device)
        {
            if (device != null && Config.Count > 0)
            {
                //navigate context  

                NavigationParameters parameters = new NavigationParameters();
                parameters.Add(GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name, siteUUID);
                parameters.Add(AlarmSystemMapContractNames.Alarm_Contract_Map_Device_Edit_Context_Key_Name, device.Id);

                GlobalNavigationContext.NavigateRegionToContractWithParam(
                   GlobalViewRegionNames.RNavigationRegion,
                    GlobalViewContractNames.Global_Contract_Menu_RightSide_AlarmDeviceEditMapContext,
                    parameters
                    );
            }

        }

        public void VertexWithIdSelected(string Id)
        {
            //find out the model data corresponding with id
            var zone = Config.FirstOrDefault(z => z.UUID.Equals(Id));
            if (zone != null)
            {
                OpenDeviceContextForDevice(zone);
            }
        }

        public void V_VertexClicked(object sender, VertexEventArgs e)
        {

        }

        public void LoadDevicesGraph()
        {

        }


        public override void GetData()
        {
            try
            {
                Config = new ObservableCollection<sconnDevice>(_provider.GetAll());
                CreateGraph();
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

        public void EditZone(sconnAlarmZone zone)
        {

        }

        public AlarmDeviceMapViewModel()
        {
            _name = "Zones";
            this._provider = new AlarmDevicesConfigService(_manager);
        }


        public void SetupCmd()
        {
            ConfigureZoneCommand = new DelegateCommand<sconnAlarmZone>(EditZone);
            // GraphEntitySelectedCommand = new DelegateCommand<sconnAlarmZone>();
        }

        [ImportingConstructor]
        public AlarmDeviceMapViewModel(IRegionManager regionManager)
        {
            Graph = new AlarmSystemGraph();
            Config = new ObservableCollection<sconnDevice>();
            this._manager = SiteNavigationManager.alarmSystemConfigManager;
            this._provider = new AlarmDevicesConfigService(_manager);
            this._regionManager = regionManager;
        }


        private AlarmSystemGraphEdge AddNewGraphEdge(AlarmSystemGraphVertex from, AlarmSystemGraphVertex to)
        {
            string edgeString = string.Format("{0}-{1} Connected", from.Name, to.Name);

            AlarmSystemGraphEdge newEdge = new AlarmSystemGraphEdge(edgeString, from, to);
            Graph.AddEdge(newEdge);
            return newEdge;
        }


        public string DisplayedImagePath
        {
            get { return "pack://application:,,,/images/strefy1.png"; }
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
                    OpenDeviceContextForDevice(Config.FirstOrDefault());
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
            try
            {
                var targetsiteUuid = (string)navigationContext.Parameters[GlobalViewContractNames.Global_Contract_Nav_Site_Context__Key_Name];
                if (targetsiteUuid != siteUUID)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
                return false;
            }

        }




    }


}
