using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.ViewModels
{
    internal class TourDescriptionViewModel : BaseViewModel
    {
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

        private float? _distance;
        public float? Distance
        {
            get => _distance;
            set
            {
                _distance = value;
                OnPropertyChanged();
            }
        }

        private int? _estimatedTime;
        public int? EstimatedTime
        {
            get => _estimatedTime;
            set
            {
                _estimatedTime = value;
                OnPropertyChanged();
            }
        }

        public TourDescriptionViewModel()
        {

        }

        public void LoadItem(Tour tour)
        {
            Name = tour.Name;
            Description = tour.Description;
            StartLocation = tour.StartLocation;
            TargetLocation = tour.TargetLocation;
            TransportType = tour.TransportType;
            RouteInformation = tour.RouteInformation;
            Distance = tour.Distance;
            EstimatedTime = tour.EstimatedTime;
        }
    }
}
