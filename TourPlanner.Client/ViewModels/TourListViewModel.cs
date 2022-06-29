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
        public event EventHandler<string?>? SearchTextChanged;
        public ObservableCollection<Tour> Items { get; } = new();

        public ICommand? OpenAddTourDialogCommand { get; set; }
        public ICommand? OpenEditTourDialogCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }
        public ICommand SearchCommand { get; set; }

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

        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public TourListViewModel(ITourManager tourManager)
        {
            _tourManager = tourManager;

            DeleteItemCommand = new RelayCommand(async _ =>
            {
                await _tourManager.DeleteTourAsync(SelectedItem.Id);
                SearchTextChanged?.Invoke(this, SearchText);
            }, _ => SelectedItem != null);

            SearchCommand = new RelayCommand(_ =>
            {
                SearchTextChanged?.Invoke(this, SearchText);
            });
        }

        public void SetItems(IEnumerable<Tour>? tours)
        {
            if (tours == null)
                return;
            Items.Clear();
            tours?.ToList().ForEach(e => Items.Add(e));
        }

        private void OnSelectedItemChanged()
        {
            SelectedItemChanged?.Invoke(this, SelectedItem);
        }
    }
}
