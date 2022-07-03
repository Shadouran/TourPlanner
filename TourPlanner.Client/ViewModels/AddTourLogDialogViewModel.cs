using System;
using System.Windows.Input;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.ViewModels
{
    internal class AddTourLogDialogViewModel : BaseViewModel, ICloseWindow
    {
        private readonly TourLogsListViewModel _tourLogsListViewModel;
        public DateTime Date { get; set; } = DateTime.Today;
        public string Time { get; set; } = DateTime.Now.ToString("HH:mm");
        public string TotalTime { get; set; } = new DateTime(1, 1, 1, 0, 0, 0).ToString("HH:mm");
        public Difficulty Difficulty { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }

        public ICommand AddTourLogCommand { get; set; }
        public Action? Close { get; set; }

        public AddTourLogDialogViewModel(TourLogsListViewModel tourLogsListViewModel)
        {
            _tourLogsListViewModel = tourLogsListViewModel;

            AddTourLogCommand = new RelayCommand(_ =>
            {
                if(ValidateAll())
                {
                    tourLogsListViewModel.AddedTourLog = new(null, Date, Time, Difficulty, TotalTime, Comment, Rating);
                    Close?.Invoke();
                }
            });
        }

        private bool ValidateAll()
        {
            // TODO missing validation
            return true;
        }

        public bool OnClosing()
        {
            return false;
        }
    }
}