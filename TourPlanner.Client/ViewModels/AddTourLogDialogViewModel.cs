using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.ViewModels
{
    internal class AddTourLogDialogViewModel : BaseViewModel, ICloseWindow
    {
        private readonly TourLogsListViewModel _tourLogsListViewModel;
        public DateTime Date { get; set; } = DateTime.Today;
        private string _time = DateTime.Now.ToString("HH:mm");
        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }
        private string _totalTime = new DateTime(1, 1, 1, 0, 0, 0).ToString("HH:mm");
        public string TotalTime
        {
            get => _totalTime;
            set
            {
                _totalTime = value;
                OnPropertyChanged();
            }
        }
        public Difficulty Difficulty { get; set; }
        public Rating Rating { get; set; }
        public string Comment { get; set; }

        public ICommand AddTourLogCommand { get; set; }
        public Action? Close { get; set; }

        public AddTourLogDialogViewModel(TourLogsListViewModel tourLogsListViewModel)
        {
            _tourLogsListViewModel = tourLogsListViewModel;

            AddTourLogCommand = new RelayCommand(_ =>
            {
                if (Comment == null)
                    Comment = "";
                tourLogsListViewModel.AddedTourLog = new(null, Date, Time, Difficulty, TotalTime, Comment, (int)Rating);
                Close?.Invoke();
            }, _ => ValidateAll());
        }

        private bool ValidateAll()
        {
            const string pattern = @"^([1-9]+\d*:)?(2[0-3]|[01][0-9]):([0-5][0-9])$";
            Regex regex = new(pattern);
            return regex.IsMatch(Time) && regex.IsMatch(TotalTime);
        }

        public bool OnClosing()
        {
            return false;
        }
    }
}