using Prism.Mef;
using Prism.Modularity;
using sconnRem.Wnd.Config;
using sconnRem.Wnd.Config.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sconnRem.Wnd.Navigation
{
    public class ConfigNavBootstrapper : MefBootstrapper
    {
        private const string ModuleCatalogUri = "/sconnRem;component/Wnd/Config/Shell/wndConfigureSiteShell.xaml";

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
