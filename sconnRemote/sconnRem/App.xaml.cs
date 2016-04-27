using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using sconnConnector;
using sconnConnector.Config;
using sconnRem.Wnd.Main;

namespace sconnRem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    /// 


    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer container = new UnityContainer();
            container.RegisterType<AlarmSystemConfigManager, AlarmSystemConfigManager>();



            GlobalWndBootstrapper bootstrapper = new GlobalWndBootstrapper();
            bootstrapper.Run();

            //ConfigNavBootstrapper bootstrapper = new ConfigNavBootstrapper();
            //bootstrapper.Run();

            //MainWindow = new MainWindow();
            //MainWindow.DataContext = new MainWindowViewModel(MainWindow);
            //MainWindow.ShowDialog();
        }
    }

}
