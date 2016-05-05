using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NLog;
using Prism.Logging;
using Prism.Mef;
using Prism.Modularity;
using Prism.Regions;
using sconnConnector;
using sconnConnector.Config;
using sconnPrismGenerics;
using sconnPrismGenerics.Boostrapper;
using sconnPrismGenerics.Logging;
using sconnRem.Controls.AlarmSystem.View.Status.Inputs;
using sconnRem.Controls.AlarmSystem.ViewModel.Alarm;
using sconnRem.Infrastructure.Navigation;
using sconnRem.View.Menu.GridNavSideMenu;
using sconnRem.View.Menu.SiteNavSideMenu;
using sconnRem.View.Menu.ToolTopMenu;
using sconnRem.View.Status;
using SiteManagmentService;

namespace sconnRem.Wnd.Main
{
    
    public class GlobalWndBootstrapper : MefBootstrapper, IVerifiableBootstraper
    {

        //private sconnDataSrc _configSource = new sconnDataSrc();
        private Logger _nlogger = LogManager.GetCurrentClassLogger();
        private const string ModuleCatalogUri = "/sconnRem;component/Wnd/Main/WndGlobalShell.xaml";

        //private ISiteRepository _repository;

        ////todo - inject from cfg bootstraper
        //private void LoadConfigFromDataStore()
        //{
        //    _configSource.LoadConfig(DataSourceType.xml);
        //}

        public GlobalWndBootstrapper()
        {
            //_repository = new SiteXmlRepository();
            //LoadConfigFromDataStore();
        }

        protected override ILoggerFacade CreateLogger()
        {
            return new MvvmLogger();

        }

        protected override void ConfigureAggregateCatalog()
        {
            try
            {
                base.ConfigureAggregateCatalog();
                this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(GlobalWndBootstrapper).Assembly));

                this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(GridNavSideMenuModule).Assembly));

                this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(SiteStatusGridViewModule).Assembly));

                this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(AlarmDeviceListViewModel).Assembly));

                this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ISiteRepository).Assembly));
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, ex.Message);
            }        

        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }


        //protected override Prism.Regions.IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        //{
        //    var factory = base.ConfigureDefaultRegionBehaviors();

        //    factory.AddIfMissing("AutoPopulateExportedViewsBehavior", typeof(AutoPopulateExportedViewsBehavior));

        //    return factory;
        //}

        protected override DependencyObject CreateShell()
        {
            SiteNavigationManager.SetNavigationContextContainer(this.Container);

            //var batch = new CompositionBatch();
            //var repoPart = batch.AddExportedValue<ISiteRepository>(this._repository);
            //this.Container.Compose(batch);

            return this.Container.GetExportedValue<WndGlobalShell>();
        }

        protected override void InitializeShell()
        {
            try
            {
                base.InitializeShell();
                Application.Current.MainWindow = (Window)this.Shell;
                Application.Current.MainWindow.Show();
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex,ex.Message);
            }

        }

        public CompositionContainer GetContainer()
        {
            return this.Container;
        }

        public AggregateCatalog GetAggregateCatalog()
        {
            return this.AggregateCatalog;
        }
    }




}
