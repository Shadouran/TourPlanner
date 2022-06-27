using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Client.BL;
using TourPlanner.Client.DAL;
using TourPlanner.Client.DAL.Endpoint;
using TourPlanner.Client.Navigation;
using TourPlanner.Client.ViewModels;
using TourPlanner.Shared.Log4Net;
using TourPlanner.Shared.Logging;

namespace TourPlanner.Client.IoCConfiguration
{
    internal class IoCContainerConfiguration
    {
        private readonly IServiceProvider _serviceProvider;

        public IoCContainerConfiguration()
        {
            var services = new ServiceCollection();

            // Logging Setup
            services.AddSingleton<ILoggerFactory, Log4NetFactory>(s =>
            {
                return new Log4NetFactory(@"C:\__Technikum\4.Semester\SWEN2\TourPlanner\TourPlanner.Client\log4net.config");
            });

            // DAL Setup
            services.AddSingleton<ITourDataAccess, TourDataAccessEndpoint>();

            // BL Setup
            services.AddSingleton<ITourManager, TourManager>();

            // UI Setup
            services.AddSingleton<INavigationService, NavigationService>(s =>
            {
                var navigationService = new NavigationService(s);
                navigationService.RegisterNavigation<MainViewModel, MainWindow>((viewModel, window) =>
                {
                    window.TourList.DataContext = viewModel.TourListViewModel;
                });

                return navigationService;
            });
            services.AddTransient<TourListViewModel>();
            services.AddTransient<MainViewModel>();


            _serviceProvider = services.BuildServiceProvider();
        }

        public MainViewModel MainViewModel => _serviceProvider.GetRequiredService<MainViewModel>();
        public TourListViewModel TourListViewModel => _serviceProvider.GetRequiredService<TourListViewModel>();
        public ILoggerFactory LoggerFactory => _serviceProvider.GetRequiredService<ILoggerFactory>();
        public INavigationService NavigationService => _serviceProvider.GetRequiredService<INavigationService>();
    }
}
