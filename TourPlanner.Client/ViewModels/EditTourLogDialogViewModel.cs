using System;
using System.Windows.Input;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.ViewModels
{
    internal class EditTourLogDialogViewModel : BaseViewModel, ICloseWindow
    {
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string TotalTime { get; set; }
        public Difficulty Difficulty { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public ICommand EditTourLogCommand { get; set; }
        public Action? Close { get; set; }

        public EditTourLogDialogViewModel(TourLog? selectedItem)
        {
            Date = selectedItem.Date;
            Time = selectedItem.Time;
            TotalTime = selectedItem.TotalTime;
            Difficulty = selectedItem.Difficulty;
            Rating = selectedItem.Rating;
            Comment = selectedItem.Comment;

            EditTourLogCommand = new RelayCommand(_ =>
            {
                if (ValidateAll())
                {
                    selectedItem.Date = Date;
                    selectedItem.Time = Time;
                    selectedItem.TotalTime = TotalTime;
                    selectedItem.Difficulty = Difficulty;
                    selectedItem.Rating = Rating;
                    selectedItem.Comment = Comment;
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