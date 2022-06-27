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

        public ICommand AddItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand EditItemCommand { get; }

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

            AddItemCommand = new RelayCommand(_ =>
            {
                // TODO
                throw new NotImplementedException();
            });

            DeleteItemCommand = new RelayCommand(async _ =>
            {
                if(SelectedItem == null)
                {
                    return;
                }
                await _tourManager.DeleteTourAsync(SelectedItem.Id);
                Items.Remove(SelectedItem);
            });

            EditItemCommand = new RelayCommand(_ =>
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
