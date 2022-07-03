using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Client.BL;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.ViewModels
{
    internal class MainViewModel : BaseViewModel, ICloseWindow
    {
        private readonly ITourManager _tourManager;
        private readonly ILogManager _logManager;
        public TourListViewModel TourListViewModel { get; }
        public TourDescriptionViewModel TourDescriptionViewModel { get; }
        public MapImageViewModel MapImageViewModel { get; }
        public TourLogsListViewModel TourLogsListViewModel { get; }
        public ICommand ImportCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand OpenAddTourDialogCommand { get; set; }
        public ICommand OpenEditTourDialogCommand { get; set; }
        public ICommand OpenAddTourLogDialogCommand { get; set; }
        public ICommand OpenEditTourLogDialogCommand { get; set; }
        public ICommand DeleteLogCommand { get; set; }
        public Action? Close { get; set; }

        public MainViewModel(ITourManager tourManager, ILogManager logManager,
            TourListViewModel tourListViewModel, TourDescriptionViewModel tourDescriptionViewModel, MapImageViewModel mapImageViewModel, TourLogsListViewModel tourLogsListViewModel)
        {
            _tourManager = tourManager;
            _logManager = logManager;
            TourListViewModel = tourListViewModel;
            TourDescriptionViewModel = tourDescriptionViewModel;
            MapImageViewModel = mapImageViewModel;
            TourLogsListViewModel = tourLogsListViewModel;
            SearchTours(null);

            ImportCommand = new RelayCommand(_ =>
            {
                // TODO getout
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    FileName = "Documents",
                    DefaultExt = ".csv",
                    Filter = "CSV Files (.csv)|*.csv"
                };
                if (dialog.ShowDialog() == true)
                {
                    var filename = dialog.FileName;
                    _tourManager.ImportTourAsync(filename);
                }
            });

            ExportCommand = new RelayCommand(_ =>
            {
                var dialog = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = "Documents",
                    DefaultExt = ".csv",
                    Filter = "CSV Files (.csv)|*.csv"
                };
                if(dialog.ShowDialog() == true)
                {
                    string filename = dialog.FileName;
                    _tourManager.ExportTourAsync(TourListViewModel.SelectedItem, filename);
                }
            }, _ => TourListViewModel.SelectedItem != null);

            CloseCommand = new RelayCommand(_ =>
            {
                Close?.Invoke();
            });

            OpenAddTourDialogCommand = new RelayCommand(_ =>
            {
                NavigationService?.NavigateTo<AddTourDialogViewModel>();
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
                    //_logManager.AddTourLogAsync(TourListViewModel.SelectedItem.Id, TourLogsListViewModel.AddedTourLog);
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
                    //_logManager.EditTourLogAsync(TourListViewModel.SelectedItem.Id, TourLogsListViewModel.SelectedItem);
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
            TourListViewModel.SetItems(tours);
        }

        private async void LoadTour(Tour? tour)
        {
            TourDescriptionViewModel.LoadItem(tour);
            if(tour != null)
            {
                var imageUri = await _tourManager.GetImageAsync(tour.ImageFileName);
                MapImageViewModel.LoadImage(imageUri);
                TourLogsListViewModel.LoadLogs(tour.Logs);
                return;
            }
            MapImageViewModel.LoadImage(null);
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
