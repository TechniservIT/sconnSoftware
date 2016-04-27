using System;
using System.Windows;
//using NLog;
//using Prism.Logging;
//using Prism.Mef;
//using Prism.Modularity;
//using sconnConnector.Config;
//using sconnPrismGenerics.Boostrapper;
//using sconnPrismGenerics.Logging;
//using sconnRem.View.Config.AlarmSystem.Comm;

//using System.ComponentModel.Composition;
//using System.ComponentModel.Composition.Hosting;

namespace sconnRem.Shells.Config
{

    //public class ConfigNavBootstrapper : MefBootstrapper, IVerifiableBootstraper
    //{

    //    private const string ModuleCatalogUri = "/sconnRem;component/Wnd/Config/Shell/wndConfigureSiteShell.xaml";
    //    private IAlarmConfigManager _manager;
    //    private Logger _nlogger = LogManager.GetCurrentClassLogger();

    //    public ConfigNavBootstrapper()
    //    {
                
    //    }

    //    public ConfigNavBootstrapper(IAlarmConfigManager manager) :this()
    //    {
    //        _manager = manager;
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
    //            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ConfigNavBootstrapper).Assembly));
    //            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(AlarmCommConfigModule).Assembly));
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
    //            var repoPart = batch.AddExportedValue<IAlarmConfigManager>(_manager);
    //            this.Container.Compose(batch);
    //        }
    //        catch (Exception ex)
    //        {
    //            _nlogger.Error(ex, ex.Message);
    //        }

    //        // repo will now be injected on any matching [Import] or [ImportingConstructor]

    //        return this.Container.GetExportedValue<WndConfigureSiteShell>();
    //    }

    //    protected override void InitializeShell()
    //    {
    //        try
    //        {
    //            base.InitializeShell();
    //            Application.Current.MainWindow = (Window)this.Shell;
    //            Application.Current.MainWindow.Show();
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

    //}

}
 