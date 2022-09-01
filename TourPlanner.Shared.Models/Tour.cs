using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Shared.Models
{
    public class Tour
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartLocation { get; set; }
        public string TargetLocation { get; set; }
        public TransportType TransportType { get; set; }
        public string RouteInformation { get; set; }
        public float Distance { get; set; }
        public int EstimatedTime { get; set; }
        public Guid ImageFileName { get; set; }
        public List<TourLog> Logs { get; set; } = new();

        public Tour()
        {

        }
        public Tour(Guid id, string name, string description, string startLocation, string targetLocation, TransportType transportType, string routeInformation, float distance, int estimatedTime, Guid imageFileName)
        {
            Id = id;
            Name = name;
            Description = description;
            StartLocation = startLocation;
            TargetLocation = targetLocation;
            TransportType = transportType;
            RouteInformation = routeInformation;
            Distance = distance;
            EstimatedTime = estimatedTime;
            ImageFileName = imageFileName;
        }
    }

    public class TourUserInformation
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartLocation { get; set; }
        public string TargetLocation { get; set; }
        public TransportType TransportType { get; set; }
        public string RouteInformation { get; set; }

        public TourUserInformation(Guid? id, string name, string description, string startLocation, string targetLocation, TransportType transportType, string routeInformation)
        {
            Id = id;
            Name = name;
            Description = description;
            StartLocation = startLocation;
            TargetLocation = targetLocation;
            TransportType = transportType;
            RouteInformation = routeInformation;
        }
    }
}