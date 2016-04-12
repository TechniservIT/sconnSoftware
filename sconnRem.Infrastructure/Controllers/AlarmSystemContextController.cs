using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using iotDbConnector.DAL;
using Prism.Events;
using Prism.Mef;
using Prism.Regions;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnPrismGenerics.View.Interface;
using sconnRem.Navigation;

namespace sconnRem.Infrastructure.Controllers
{


    //public class AlarmSystemContextController
    //{
    //    private readonly MefBootstrapper Bootstrapper;
    //    private readonly IRegionManager regionManager;
    //    private readonly IEventAggregator eventAggregator;
    //    private readonly sconnSite currentContextSconnSite;

        

    //    public AlarmSystemContextController(MefBootstrapper bootstrapper,
    //                                IRegionManager regionManager,
    //                                IEventAggregator eventAggregator,
    //                                sconnSite site)
    //    {
    //        if (bootstrapper == null) throw new ArgumentNullException("bootstrapper");
    //        if (regionManager == null) throw new ArgumentNullException("regionManager");
    //        if (eventAggregator == null) throw new ArgumentNullException("eventAggregator");
    //        if (site == null) throw new ArgumentNullException("dataService");

    //        this.Bootstrapper = bootstrapper;
    //        this.regionManager = regionManager;
    //        this.eventAggregator = eventAggregator;
    //        this.currentContextSconnSite = site;

    //        // Subscribe to the EmployeeSelectedEvent event.
    //        // This event is fired whenever the user selects an
    //        // employee in the EmployeeListView.
    //        this.eventAggregator.GetEvent<sconnSiteSelectedEvent>().Subscribe(this.SiteNavigated, true);
    //    }

    //    /// <summary>
    //    /// Called when a new employee is selected. This method uses
    //    /// view injection to programmatically 
    //    /// </summary>
    //    private void SiteNavigated(string id)
    //    {
    //        if (string.IsNullOrEmpty(id)) return;

    //        EndpointInfo info = new EndpointInfo();
    //        info.Hostname = currentContextSconnSite.serverIP;
    //        info.Port = currentContextSconnSite.serverPort;
    //        DeviceCredentials cred = new DeviceCredentials();
    //        cred.Password = currentContextSconnSite.authPasswd;
    //        cred.Username = "";
    //        AlarmSystemConfigManager manager = new AlarmSystemConfigManager(info, cred);
    //        Device alrmSysDev = new Device();
    //        alrmSysDev.Credentials = cred;
    //        alrmSysDev.EndpInfo = info;
    //        manager.RemoteDevice = alrmSysDev;

    //        IAlarmConfigManager man = new AlarmSystemConfigManager(manager);
            
    //        IRegion mainRegion = this.regionManager.Regions[AlarmRegionNames.AlarmConfig_Contract_AuthConfigView];
    //        if (mainRegion == null) return;

    //        Type ViewType;
    //        if (id.Equals(AlarmRegionNames.AlarmStatus_Contract_InputsView))
    //        {
                
    //        }


    //        // Check to see if we need to create an instance of the view.
    //        UserControl view = mainRegion.GetView(id) as UserControl;
    //        if (view == null)
    //        {
    //            // Create a new instance of the EmployeeDetailsView using the Unity container.
    //            view = this.container.Resolve<IAlarmSystemStatusView>();

    //            // Add the view to the main region. This automatically activates the view too.
    //            mainRegion.Add(view, id);
    //        }
    //        else
    //        {
    //            // The view has already been added to the region so just activate it.
    //            mainRegion.Activate(view);
    //        }

    //        // Set the current employee property on the view model.
    //        IAlarmSystemStatusView viewModel = view.DataContext as IAlarmSystemStatusView;
    //        if (viewModel != null)
    //        {
    //          //  viewModel.CurrentEmployee = selectedEmployee;
    //        }
    //    }
    //}


}
