using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Shared.Models
{
    public class Tour
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? StartLocation { get; set; }
        public string? TargetLocation { get; set; }
        public string? TransportType { get; set; }
        public string? Distance { get; set; }
        public string? EstimatedTime { get; set; }
        public string? RouteInformation { get; set; }

        public Tour(string? id, string? name, string? description, string? startLocation, string? targetLocation, string? transportType, string? distance, string? estimatedTime, string? routeInformation)
        {
            Id = id;
            Name = name;
            Description = description;
            StartLocation = startLocation;
            TargetLocation = targetLocation;
            TransportType = transportType;
            Distance = distance;
            EstimatedTime = estimatedTime;
            RouteInformation = routeInformation;
        }
    }
}
