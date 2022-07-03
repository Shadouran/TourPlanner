﻿using System;
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
        public TourListViewModel TourListViewModel { get; }
        public TourDescriptionViewModel TourDescriptionViewModel { get; }
        public MapImageViewModel MapImageViewModel { get; }
        public ICommand CloseCommand { get; set; }
        public ICommand OpenAddTourDialogCommand { get; set; }
        public ICommand OpenEditTourDialogCommand { get; set; }
        public Action? Close { get; set; }

        public MainViewModel(ITourManager tourManager, TourListViewModel tourListViewModel, TourDescriptionViewModel tourDescriptionViewModel, MapImageViewModel mapImageViewModel)
        {
            _tourManager = tourManager;
            TourListViewModel = tourListViewModel;
            TourDescriptionViewModel = tourDescriptionViewModel;
            MapImageViewModel = mapImageViewModel;
            SearchTours(null);

            CloseCommand = new RelayCommand(_ =>
            {
                Close?.Invoke();
            });

            OpenAddTourDialogCommand = new RelayCommand(_ =>
            {
                NavigationService?.NavigateTo<AddTourDialogViewModel>();
            });

            OpenEditTourDialogCommand = new RelayCommand(_ =>
            {
                var viewModel = new EditTourDialogViewModel(_tourManager, TourListViewModel.SelectedItem);
                NavigationService?.NavigateTo(viewModel);
                LoadTour(null);
                LoadTour(TourListViewModel.SelectedItem);
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
            if (tour == null)
                return;
            TourDescriptionViewModel.LoadItem(tour);
            var imageUri = await _tourManager.GetImageAsync(tour.ImageFileName);
            MapImageViewModel.LoadImage(imageUri);
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
