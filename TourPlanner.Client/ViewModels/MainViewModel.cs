using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Client.BL;

namespace TourPlanner.Client.ViewModels
{
    internal class MainViewModel : BaseViewModel, ICloseWindow
    {
        private readonly ITourManager _tourManager;
        public TourListViewModel TourListViewModel { get; }
        public ICommand CloseCommand { get; set; }
        public ICommand OpenAddTourDialog { get; set; } 
        public Action? Close { get; set; }

        public MainViewModel(ITourManager tourManager, TourListViewModel tourListViewModel)
        {
            _tourManager = tourManager;
            TourListViewModel = tourListViewModel;

            CloseCommand = new RelayCommand(_ =>
            {
                Close?.Invoke();
            });

            OpenAddTourDialog = new RelayCommand(_ =>
            {
                NavigationService?.NavigateTo<AddTourDialogViewModel>();
            });
        }
    }
}
