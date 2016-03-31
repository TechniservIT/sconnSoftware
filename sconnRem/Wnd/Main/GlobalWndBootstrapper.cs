using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Logging;
using Prism.Mef;
using Prism.Modularity;
using Prism.Regions;
using sconnConnector.Config;
using sconnRem.Logging;
using sconnRem.View.Menu.GridNavSideMenu;
using sconnRem.View.Menu.SiteNavSideMenu;
using sconnRem.View.Menu.ToolTopMenu;
using sconnRem.View.Status;
using sconnRem.Wnd.Config;

namespace sconnRem.Wnd.Main
{
    public class GlobalWndBootstrapper : MefBootstrapper
    {

        private const string ModuleCatalogUri = "/sconnRem;component/Wnd/Main/WndGlobalShell.xaml";

        //todo - inject from cfg bootstraper
        private void LoadConfigFromDataStore()
        {

        }

        public GlobalWndBootstrapper()
        {
            LoadConfigFromDataStore();
        }

        protected override ILoggerFacade CreateLogger()
        {
            return new MvvmLogger();

        }

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(GlobalWndBootstrapper).Assembly));

            //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(GridNavSideMenuModule).Assembly));
            //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(SiteNavSideMenuModule).Assembly));
            //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ToolTopMenuModule).Assembly));
            //this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(SiteStatusGridViewModule).Assembly));
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
            return this.Container.GetExportedValue<WndGlobalShell>();
        }

        //protected override DependencyObject CreateShell()
        //{
        //    //model inject - todo - loaded config

        //    //var batch = new CompositionBatch();
        //    //var repoPart = batch.AddExportedValue<IAlarmConfigManager>(_manager);
        //    //this.Container.Compose(batch);

        //    return this.Container.GetExportedValue<WndGlobalShell>();
        //}

        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }
    }


    [Export(typeof(AutoPopulateExportedViewsBehavior))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AutoPopulateExportedViewsBehavior : RegionBehavior, IPartImportsSatisfiedNotification
    {
        protected override void OnAttach()
        {
            AddRegisteredViews();
        }

        public void OnImportsSatisfied()
        {
            AddRegisteredViews();
        }

        private void AddRegisteredViews()
        {
            if (this.Region != null)
            {
                foreach (var viewEntry in this.RegisteredViews)
                {
                    if (viewEntry.Metadata.RegionName == this.Region.Name)
                    {
                        var view = viewEntry.Value;

                        if (!this.Region.Views.Contains(view))
                        {
                            this.Region.Add(view);
                        }
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "MEF injected values"), ImportMany(AllowRecomposition = true)]
        public Lazy<object, IViewRegionRegistration>[] RegisteredViews { get; set; }
    }

    public interface IViewRegionRegistration
    {
        string RegionName { get; }
    }


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [MetadataAttribute]
    public sealed class ViewExportAttribute : ExportAttribute, IViewRegionRegistration
    {
        public ViewExportAttribute()
            : base(typeof(object))
        { }

        public ViewExportAttribute(string viewName)
            : base(viewName, typeof(object))
        { }

        public string ViewName { get { return base.ContractName; } }

        public string RegionName { get; set; }
    }


}
