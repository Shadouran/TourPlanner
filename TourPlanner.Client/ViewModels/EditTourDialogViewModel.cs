using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Client.BL;
using System.Windows;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.ViewModels
{
    internal class EditTourDialogViewModel : BaseViewModel, IDataErrorInfo, ICloseWindow
    {
        private readonly ITourManager _tourManager;


        private string? _name;
        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        private string? _description;
        public string? Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private string? _startLocation; 
        public string? StartLocation
        {
            get => _startLocation;
            set
            {
                _startLocation = value;
                OnPropertyChanged();
            }
        }

        private string? targetLocation;
        public string? TargetLocation
        {
            get => targetLocation;
            set
            {
                targetLocation = value;
                OnPropertyChanged();
            }
        }

        private string? _transportType;
        public string? TransportType
        {
            get => _transportType;
            set
            {
                _transportType = value;
                OnPropertyChanged();
            }
        }

        private string? _routeInformation;
        public string? RouteInformation
        {
            get => _routeInformation;
            set
            {
                _routeInformation = value;
                OnPropertyChanged();
            }
        }

        public Tour Tour { get; set; }

        public ICommand EditTourCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public Action? Close { get; set; }

        public EditTourDialogViewModel(ITourManager tourManager, Tour tour)
        {
            _tourManager = tourManager;
            Tour = tour;
            Name = Tour.Name;
            Description = Tour.Description;
            StartLocation = Tour.StartLocation;
            TargetLocation = Tour.TargetLocation;
            TransportType = Tour.TransportType;
            RouteInformation = Tour.RouteInformation;


            CloseCommand = new RelayCommand(_ =>
            {
                Close?.Invoke();
            });

            EditTourCommand = new RelayCommand(_ =>
            {
                if (ValidateAll())
                {
                    Tour.Name = Name;
                    Tour.Description = Description;
                    Tour.StartLocation = StartLocation;
                    Tour.TargetLocation = TargetLocation;
                    Tour.TransportType = TransportType;
                    Tour.RouteInformation = RouteInformation;
                    Close?.Invoke();
                }
            });
        }

        private bool ValidateAll()
        {
            if (string.IsNullOrEmpty(Name) ||
                string.IsNullOrEmpty(Description) ||
                string.IsNullOrEmpty(StartLocation) ||
                string.IsNullOrEmpty(TargetLocation) ||
                string.IsNullOrEmpty(TransportType) ||
                string.IsNullOrEmpty(RouteInformation))
            {
                return false;
            }
            return true;
        }

        #region IDataErrorInfo
        public string Error => null;
        public string this[string columnName] => Validate(columnName);

        private string Validate(string propertyName)
        {
            string validationMessage = string.Empty;
            switch(propertyName)
            {
                case "Name":
                    if(string.IsNullOrEmpty(Name))
                    {
                        validationMessage = "Field can not be empty.";
                    }
                    break;
                case "Description":
                    if (string.IsNullOrEmpty(Description))
                    {
                        validationMessage = "Field can not be empty.";
                    }
                    break;
                case "StartLocation":
                    if (string.IsNullOrEmpty(StartLocation))
                    {
                        validationMessage = "Field can not be empty.";
                    }
                    break;
                case "TargetLocation":
                    if (string.IsNullOrEmpty(TargetLocation))
                    {
                        validationMessage = "Field can not be empty.";
                    }
                    break;
                case "TransportType":
                    if (string.IsNullOrEmpty(TransportType))
                    {
                        validationMessage = "Field can not be empty.";
                    }
                    break;
                case "RouteInformation":
                    if (string.IsNullOrEmpty(RouteInformation))
                    {
                        validationMessage = "Field can not be empty.";
                    }
                    break;

            }
            return validationMessage;
        }

        public bool OnClosing()
        {
            return false;
        }
        #endregion
    }
}
