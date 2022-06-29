﻿using System;
using System.Collections.Generic;
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
                var viewModel = new EditTourDialogViewModel(_tourManager, this, TourListViewModel.SelectedItem);
                var result = NavigationService?.NavigateTo(viewModel);
                if (result == true)
                    SearchTours(TourListViewModel.SearchText);
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
                tours = await _tourManager.GetAllTourAsync();
            }
            else
            {
                // TODO implement getmatchingtours
                //tours = await _tourManager.GetMatchingToursAsync(searchString);
                tours = await _tourManager.GetAllTourAsync();
            }
            TourListViewModel.SetItems(tours);
        }

        private void LoadTour(Tour tour)
        {
            if (tour == null)
                return;
            //TourDescriptionViewModel.LoadItem(tour);
            var imageUri = _tourManager.GetFullImagePath(tour.ImageFileName);
            MapImageViewModel.ImagePath = imageUri;
        }
    }
}
