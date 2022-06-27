using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Client.IoCConfiguration;
using TourPlanner.Client.ViewModels;
using TourPlanner.Shared.Logging;

namespace TourPlanner.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ILogger? Logger { get; set; }
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var ioCConfig = new IoCContainerConfiguration();
            Logger = ioCConfig.LoggerFactory.CreateLogger<App>();

            Logger.Debug("Configuration setup complete");
            Logger.Debug("App started");

            ioCConfig.NavigationService.NavigateTo<MainViewModel>();
        }
    }
}
