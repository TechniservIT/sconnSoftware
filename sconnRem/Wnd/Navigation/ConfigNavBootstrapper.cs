using Prism.Logging;
using Prism.Mef;
using Prism.Modularity;
using sconnRem.Logging;
using sconnRem.Wnd.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using sconnConnector.Config;

namespace sconnRem.Wnd.Navigation
{
    public class ConfigNavBootstrapper : MefBootstrapper
    {

        private const string ModuleCatalogUri = "/sconnRem;component/Wnd/Config/Shell/wndConfigureSiteShell.xaml";
        private IAlarmConfigManager _Manager;

        public ConfigNavBootstrapper()
        {
                
        }

        public ConfigNavBootstrapper(IAlarmConfigManager Manager) :this()
        {
            _Manager = Manager;
        }

        protected override ILoggerFacade CreateLogger()
        {
            return new MvvmLogger();

        }

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ConfigNavBootstrapper).Assembly));
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override DependencyObject CreateShell()
        {
            var batch = new CompositionBatch();
            var repoPart = batch.AddExportedValue<IAlarmConfigManager>(_Manager);
            this.Container.Compose(batch);
            // repo will now be injected on any matching [Import] or [ImportingConstructor]

            return this.Container.GetExportedValue<wndConfigureSiteShell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }
    }
}
