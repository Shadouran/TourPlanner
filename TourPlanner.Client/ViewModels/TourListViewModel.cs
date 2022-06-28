using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Client.BL;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.ViewModels
{
    internal class TourListViewModel : BaseViewModel
    {
        private readonly ITourManager _tourManager;

        public event EventHandler<Tour?>? SelectedItemChanged;
        public ObservableCollection<Tour> Items { get; } = new();

        public ICommand OpenAddTourDialogCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand OpenEditTourDialogCommand { get; }

        private Tour? _selectedItem;
        public Tour? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                OnSelectedItemChanged();
            }
        }

        public TourListViewModel(ITourManager tourManager)
        {
            _tourManager = tourManager;

            SetItems();

            OpenAddTourDialogCommand = new RelayCommand(_ =>
            {
                NavigationService?.NavigateTo<AddTourDialogViewModel>();
            });

            DeleteItemCommand = new RelayCommand(async _ =>
            {
                if(SelectedItem == null || SelectedItem.Id == null)
                {
                    return;
                }
                await _tourManager.DeleteTourAsync(SelectedItem.Id);
                Items.Remove(SelectedItem);
            });

            OpenEditTourDialogCommand = new RelayCommand(_ =>
            {
                // TODO
                throw new NotImplementedException();
            });
        }

        private async void SetItems()
        {
            var tours = await _tourManager.GetAllTourAsync();
            Items.Clear();
            tours?.ToList().ForEach(e => Items.Add(e));
        }

        private void OnSelectedItemChanged()
        {
            SelectedItemChanged?.Invoke(this, SelectedItem);
        }
    }
}
