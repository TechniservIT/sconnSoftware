using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NLog;
using Prism.Logging;
using Prism.Mef;
using Prism.Modularity;
using sconnConnector.Config;
using sconnConnector.POCO.Config;
using sconnPrismGenerics.Boostrapper;
using sconnPrismGenerics.Logging;
using sconnRem.Controls.SiteManagment.Wizard;
using sconnRem.Navigation;
using SiteManagmentService;

namespace sconnRem.Shells.Config
{

    //public class SiteConnectionWizardBootstrapper : MefBootstrapper, IVerifiableBootstraper, IDisposable
    //{
    //    private Logger _nlogger = LogManager.GetCurrentClassLogger();
    //    private sconnSite RemoteSite;

    //    public SiteConnectionWizardBootstrapper()
    //    {
    //        RemoteSite = new sconnSite();
    //    }

    //    public SiteConnectionWizardBootstrapper(sconnSite site)
    //    {
    //        RemoteSite = site;
    //        //Navigate to site edit view
    //    }
        
    //    protected override ILoggerFacade CreateLogger()
    //    {
    //        return new MvvmLogger();

    //    }

    //    protected override void ConfigureAggregateCatalog()
    //    {
    //        try
    //        {
    //            base.ConfigureAggregateCatalog();
    //            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(WndSiteConnectionWizard).Assembly));
    //            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(SiteConnectionWizardModule).Assembly));
    //            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ISiteRepository).Assembly));
    //        }
    //        catch (Exception ex)
    //        {
    //            _nlogger.Error(ex, ex.Message);
    //        }

    //    }

    //    protected override IModuleCatalog CreateModuleCatalog()
    //    {
    //        return new ConfigurationModuleCatalog();
    //    }

    //    protected override DependencyObject CreateShell()
    //    {
    //        try
    //        {
    //            var batch = new CompositionBatch();
    //            var repoPart = batch.AddExportedValue(RemoteSite);
    //            this.Container.Compose(batch);
    //            return this.Container.GetExportedValue<WndSiteConnectionWizard>();
    //        }
    //        catch (Exception ex)
    //        {
    //            _nlogger.Error(ex, ex.Message);
    //            return null;
    //        }
    //    }

    //    protected override void InitializeShell()
    //    {
    //        try
    //        {
    //            base.InitializeShell();
    //            var shellWnd = (Window)this.Shell;
    //            shellWnd.Show();    //Application.Current.MainWindow
    //        }
    //        catch (Exception ex)
    //        {
    //            _nlogger.Error(ex, ex.Message);
    //        }

    //    }

    //    public CompositionContainer GetContainer()
    //    {
    //        return this.Container;
    //    }

    //    public AggregateCatalog GetAggregateCatalog()
    //    {
    //        return this.AggregateCatalog;
    //    }

    //    public void Dispose()
    //    {
            
    //    }
    //}

}
