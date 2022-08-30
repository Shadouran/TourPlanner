using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Client.BL;
using TourPlanner.Client.BL.ReportGeneration;
using TourPlanner.Client.DAL;
using TourPlanner.Client.DAL.Endpoint;
using TourPlanner.Client.ExportImport;
using TourPlanner.Client.Navigation;
using TourPlanner.Client.ViewModels;
using TourPlanner.Client.Views;
using TourPlanner.Shared.Filesystem;
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
            services.AddSingleton<ILogDataAccess, TourDataAccessEndpoint>();

            // BL Setup
            services.AddSingleton<ITourManager, TourManager>();
            services.AddSingleton<ILogManager, TourLogManager>();
            services.AddSingleton<IReportGeneratorFactory, QuestPdfGeneratorFactory>();

            // UI Setup
            services.AddSingleton<IFilenameFetch, FilenameFetchDialog>();
            services.AddSingleton<INavigationService, NavigationService>(s =>
            {
                var navigationService = new NavigationService(s);
                navigationService.RegisterNavigation<AddTourDialogViewModel, AddTourDialog>();
                navigationService.RegisterNavigation<EditTourDialogViewModel, EditTourDialog>();
                navigationService.RegisterNavigation<AddTourLogDialogViewModel, AddTourLogDialog>();
                navigationService.RegisterNavigation<EditTourLogDialogViewModel, EditTourLogDialog>();
                navigationService.RegisterNavigation<MainViewModel, MainWindow>((viewModel, window) =>
                {
                    window.TourList.DataContext = viewModel.TourListViewModel;
                    window.MapImage.DataContext = viewModel.MapImageViewModel;
                    window.TourDescription.DataContext = viewModel.TourDescriptionViewModel;
                    window.TourLogs.DataContext = viewModel.TourLogsListViewModel;
                });

                return navigationService;
            });
            services.AddTransient<TourListViewModel>();
            services.AddTransient<TourDescriptionViewModel>();
            services.AddTransient<MapImageViewModel>();
            services.AddTransient<TourLogsListViewModel>();
            services.AddTransient<AddTourDialogViewModel>();
            services.AddTransient<EditTourDialogViewModel>();
            services.AddTransient<AddTourLogDialogViewModel>();
            services.AddTransient<EditTourLogDialogViewModel>();
            services.AddTransient<MainViewModel>();


            _serviceProvider = services.BuildServiceProvider();
        }

        public MainViewModel MainViewModel => _serviceProvider.GetRequiredService<MainViewModel>();
        public TourListViewModel TourListViewModel => _serviceProvider.GetRequiredService<TourListViewModel>();
        public TourDescriptionViewModel TourDescriptionViewModel => _serviceProvider.GetRequiredService<TourDescriptionViewModel>();
        public MapImageViewModel MapImageViewModel => _serviceProvider.GetRequiredService<MapImageViewModel>();
        public TourLogsListViewModel TourLogsListViewModel => _serviceProvider.GetRequiredService<TourLogsListViewModel>();
        public AddTourDialogViewModel AddTourDialogViewModel => _serviceProvider.GetRequiredService<AddTourDialogViewModel>();
        public EditTourDialogViewModel EditTourDialogViewModel => _serviceProvider.GetRequiredService<EditTourDialogViewModel>();
        public ILoggerFactory LoggerFactory => _serviceProvider.GetRequiredService<ILoggerFactory>();
        public INavigationService NavigationService => _serviceProvider.GetRequiredService<INavigationService>();
        public IFilenameFetch FilenameFetch => _serviceProvider.GetRequiredService<IFilenameFetch>();
    }
}
