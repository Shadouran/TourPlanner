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
    internal class TourLogsListViewModel : BaseViewModel
    {

        public TourLog AddedTourLog { get; set; }

        private TourLog? _selectedItem;
        public TourLog? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<TourLog> Items { get; } = new();
        public ICommand OpenAddTourLogDialogCommand { get; set; }
        public ICommand OpenEditTourLogDialogCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }

        public TourLogsListViewModel()
        {

        }

        public void LoadLogs(IEnumerable<TourLog>? logs)
        {
            Items.Clear();
            if(logs != null)
            {
                logs.ToList().ForEach(e => Items.Add(e));
            }
        }
    }
}
