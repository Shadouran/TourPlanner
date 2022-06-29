using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Client.BL;
using TourPlanner.Client.BL.MapQuest;
using TourPlanner.Client.DAL;
using TourPlanner.Client.DAL.Endpoint;
using TourPlanner.Client.Navigation;
using TourPlanner.Client.ViewModels;
using TourPlanner.Client.Views;
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
                return new Log4NetFactory("log4net.config");
            });

            // Configuration Setup
            services.AddSingleton<IConfiguration>(s =>
            {
                return new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", false, true)
                            .Build();
            });

            // DAL Setup
            services.AddSingleton<IFilesystem, Filesystem>();
            services.AddSingleton<ITourDataAccess, TourDataAccessEndpoint>();

            // BL Setup
            services.AddSingleton<IMapAPI, MapQuestAPI>();
            services.AddSingleton<ITourManager, TourManager>();

            // UI Setup
            services.AddSingleton<INavigationService, NavigationService>(s =>
            {
                var navigationService = new NavigationService(s);
                navigationService.RegisterNavigation<AddTourDialogViewModel, AddTourDialog>();
                navigationService.RegisterNavigation<EditTourDialogViewModel, EditTourDialog>();
                navigationService.RegisterNavigation<MainViewModel, MainWindow>((viewModel, window) =>
                {
                    window.TourList.DataContext = viewModel.TourListViewModel;
                });

                return navigationService;
            });
            services.AddTransient<TourListViewModel>();
            services.AddTransient<AddTourDialogViewModel>();
            services.AddTransient<EditTourDialogViewModel>();
            services.AddTransient<MainViewModel>();


            _serviceProvider = services.BuildServiceProvider();
        }

        public MainViewModel MainViewModel => _serviceProvider.GetRequiredService<MainViewModel>();
        public TourListViewModel TourListViewModel => _serviceProvider.GetRequiredService<TourListViewModel>();
        public AddTourDialogViewModel AddTourDialogViewModel => _serviceProvider.GetRequiredService<AddTourDialogViewModel>();
        public EditTourDialogViewModel EditTourDialogViewModel => _serviceProvider.GetRequiredService<EditTourDialogViewModel>();
        public ILoggerFactory LoggerFactory => _serviceProvider.GetRequiredService<ILoggerFactory>();
        public INavigationService NavigationService => _serviceProvider.GetRequiredService<INavigationService>();
    }
}
