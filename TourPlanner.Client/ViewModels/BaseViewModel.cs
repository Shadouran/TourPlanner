using System.ComponentModel;
using System.Runtime.CompilerServices;
using TourPlanner.Client.Navigation;

namespace TourPlanner.Client.ViewModels
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        public INavigationService? NavigationService { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
