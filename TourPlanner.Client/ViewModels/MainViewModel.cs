﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TourPlanner.Client.BL;
using TourPlanner.Client.BL.ReportGeneration;
using TourPlanner.Client.ExportImport;
using TourPlanner.Shared.Models;
using static TourPlanner.Shared.Delegates;

namespace TourPlanner.Client.ViewModels
{
    internal class MainViewModel : BaseViewModel, ICloseWindow
    {
        private readonly ITourManager _tourManager;
        private readonly ILogManager _logManager;
        private readonly IReportGenerator _reportGenerator;
        private readonly IFilenameFetch _filenameFetch;
        public TourUserInformation? NewTour { get; set; } 
        public TourListViewModel TourListViewModel { get; }
        public TourDescriptionViewModel TourDescriptionViewModel { get; }
        public MapImageViewModel MapImageViewModel { get; }
        public TourLogsListViewModel TourLogsListViewModel { get; }
        public ICommand ImportCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand GenerateTourReportCommand { get; set; }
        public ICommand GenerateSummaryReportCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand OpenAddTourDialogCommand { get; set; }
        public ICommand OpenEditTourDialogCommand { get; set; }
        public ICommand OpenAddTourLogDialogCommand { get; set; }
        public ICommand OpenEditTourLogDialogCommand { get; set; }
        public ICommand DeleteLogCommand { get; set; }
        public Action? Close { get; set; }
        public AddCreatedTourToListDelegate Handler { get; set; }

        public MainViewModel(ITourManager tourManager, ILogManager logManager, IReportGeneratorFactory reportGeneratorFactory, IFilenameFetch filenameFetch,
            TourListViewModel tourListViewModel, TourDescriptionViewModel tourDescriptionViewModel, MapImageViewModel mapImageViewModel, TourLogsListViewModel tourLogsListViewModel)
        {
            _tourManager = tourManager;
            _logManager = logManager;
            _reportGenerator = reportGeneratorFactory.CreateReportGenerator();
            _filenameFetch = filenameFetch;
            TourListViewModel = tourListViewModel;
            TourDescriptionViewModel = tourDescriptionViewModel;
            MapImageViewModel = mapImageViewModel;
            TourLogsListViewModel = tourLogsListViewModel;
            SearchTours(null);

            ImportCommand = new RelayCommand(_ =>
            {
                var filename = _filenameFetch.FetchFilename("Document", FileExtension.CSV);
                if (filename == null || TourListViewModel.SelectedItem == null)
                    return;
                _tourManager.ImportTourAsync(filename);
            });

            ExportCommand = new RelayCommand(_ =>
            {
                var filename = _filenameFetch.FetchFilename("Document", FileExtension.CSV);
                if (filename == null || TourListViewModel.SelectedItem == null)
                    return;
               _tourManager.ExportTourAsync(TourListViewModel.SelectedItem, filename);
            }, _ => TourListViewModel.SelectedItem != null);

            GenerateTourReportCommand = new RelayCommand(_ =>
            {
                var filename = _filenameFetch.FetchFilename("Document", FileExtension.PDF);
                if (filename == null || TourListViewModel.SelectedItem == null)
                    return;
                _reportGenerator.GenerateTourReport(TourListViewModel.SelectedItem, filename);
            }, _ => TourListViewModel.SelectedItem != null);

            GenerateSummaryReportCommand = new RelayCommand(_ =>
            {
                var filename = _filenameFetch.FetchFilename("Document", FileExtension.PDF);
                if (filename == null)
                    return;
                _reportGenerator.GenerateSummaryReport(TourListViewModel.Items, filename);
            }, _ => TourListViewModel.Items.Count > 0);

            CloseCommand = new RelayCommand(_ =>
            {
                Close?.Invoke();
            });

            Handler = () => SearchTours(null);

            OpenAddTourDialogCommand = new RelayCommand(async _ =>
            {
                var viewModel = new AddTourDialogViewModel(_tourManager, this);
                var result = NavigationService?.NavigateTo(viewModel);
                if (result == true && NewTour != null)
                {
                    AddTour(await _tourManager.AddTourAsync(NewTour));
                }
            });

            OpenEditTourDialogCommand = new RelayCommand(async _ =>
            {
                var viewModel = new EditTourDialogViewModel(_tourManager, TourListViewModel.SelectedItem);
                var result = NavigationService?.NavigateTo(viewModel);
                if(result == true)
                {
                    LoadTour(await _tourManager.EditTourAsync(TourListViewModel.SelectedItem));
                }
            }, _ => TourListViewModel.SelectedItem != null);


            TourListViewModel.SearchTextChanged += (_, searchText) =>
            {
                SearchTours(searchText);
            };

            TourListViewModel.SelectedItemChanged += (_, selectedItem) =>
            {
                LoadTour(selectedItem);
            };

            TourListViewModel.OpenAddTourDialogCommand = OpenAddTourDialogCommand;
            TourListViewModel.OpenEditTourDialogCommand = OpenEditTourDialogCommand;

            OpenAddTourLogDialogCommand = new RelayCommand(_ =>
            {
                var viewModel = new AddTourLogDialogViewModel(TourLogsListViewModel);
                var result = NavigationService?.NavigateTo(viewModel);
                if (result == true)
                {
                    _logManager.AddTourLogAsync(TourListViewModel.SelectedItem.Id, TourLogsListViewModel.AddedTourLog);
                    TourLogsListViewModel.Items.Add(TourLogsListViewModel.AddedTourLog);
                    TourListViewModel.SelectedItem.Logs.Add(TourLogsListViewModel.AddedTourLog);
                }
            }, _ => TourListViewModel.SelectedItem != null);

            OpenEditTourLogDialogCommand = new RelayCommand(_ =>
            {
                var viewModel = new EditTourLogDialogViewModel(TourLogsListViewModel.SelectedItem);
                var result = NavigationService?.NavigateTo(viewModel);
                if (result == true)
                {
                    _logManager.EditTourLogAsync(TourLogsListViewModel.SelectedItem);
                    LoadTour(TourListViewModel.SelectedItem);
                }
            }, _ => TourListViewModel.SelectedItem != null && TourLogsListViewModel.SelectedItem != null);

            DeleteLogCommand = new RelayCommand(async _ =>
            {
                await _logManager.DeleteTourLogAsync(TourLogsListViewModel.SelectedItem.Id);
                TourListViewModel.SelectedItem.Logs.Remove(TourLogsListViewModel.SelectedItem);
                TourLogsListViewModel.Items.Remove(TourLogsListViewModel.SelectedItem);
                TourLogsListViewModel.SelectedItem = null;
            }, _ => TourLogsListViewModel.SelectedItem != null);

            TourLogsListViewModel.OpenAddTourLogDialogCommand = OpenAddTourLogDialogCommand;
            TourLogsListViewModel.OpenEditTourLogDialogCommand = OpenEditTourLogDialogCommand;
            TourLogsListViewModel.DeleteItemCommand = DeleteLogCommand;
        }

        private async void SearchTours(string? searchText)
        {
            IEnumerable<Tour>? tours;
            if (string.IsNullOrEmpty(searchText))
            {
                tours = await _tourManager.GetAllToursAsync();
            }
            else
            {
                tours = await _tourManager.GetMatchingToursAsync(searchText);
            }
            if (tours == null)
                return;
            foreach(var tour in tours)
            {
                var logs = await _logManager.GetAllTourLogsAsync(tour.Id);
                tour.Logs = logs?.ToList();
            }
          
            TourListViewModel.SetItems(tours);
        }

        private async void LoadTour(Tour? tour)
        {
            TourDescriptionViewModel.LoadItem(tour);
            if(tour == null)
            {
                MapImageViewModel.LoadImage(null);
                TourLogsListViewModel.LoadLogs(null);
                return;
            }
            var imageUri = await _tourManager.GetImageAsync(tour.ImageFileName);
            MapImageViewModel.LoadImage(imageUri);
            TourLogsListViewModel.LoadLogs(tour.Logs);
        }

        private void AddTour(Tour? tour)
        {
            if(tour != null)
                TourListViewModel.Items.Add(tour);
            LoadTour(tour);
        }

        public bool OnClosing()
        {
            bool close = false;
            _tourManager.ClearCache();
            // return false to close window
            // true to cancel closing
            return close;
        }
    }
}
