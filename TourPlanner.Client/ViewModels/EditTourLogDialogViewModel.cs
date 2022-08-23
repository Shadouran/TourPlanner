using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.ViewModels
{
    internal class EditTourLogDialogViewModel : BaseViewModel, ICloseWindow
    {
        public DateTime Date { get; set; }
        private string _time;
        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }
        private string _totalTime;
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
        public ICommand EditTourLogCommand { get; set; }
        public Action? Close { get; set; }

        public EditTourLogDialogViewModel(TourLog? selectedItem)
        {
            Date = selectedItem.Date;
            Time = selectedItem.Time;
            TotalTime = selectedItem.TotalTime;
            Difficulty = selectedItem.Difficulty;
            Rating = (Rating)selectedItem.Rating;
            Comment = selectedItem.Comment;

            EditTourLogCommand = new RelayCommand(_ =>
            {
                selectedItem.Date = Date;
                selectedItem.Time = Time;
                selectedItem.TotalTime = TotalTime;
                selectedItem.Difficulty = Difficulty;
                selectedItem.Rating = (int)Rating;
                selectedItem.Comment = Comment;
                Close?.Invoke();
            }, _ => ValidateAll());
        }
        private bool ValidateAll()
        {
            const string pattern = @"^\d{2,}:[0-5][0-9]$";
            Regex regex = new(pattern);
            return regex.IsMatch(Time) && regex.IsMatch(TotalTime);
        }

        public bool OnClosing()
        {
            return false;
        }
    }
}