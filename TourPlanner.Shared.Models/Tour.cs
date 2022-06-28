using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Shared.Models
{
    public class Tour
    {
        public Guid? Id { get; set; }
        public TourUserInformation? UserInformation { get; set; }
        public TourMapquestInformation? MapquestInformation { get; set; }

        public Tour(Guid? id, TourUserInformation? userInformation, TourMapquestInformation? mapquestInformation)
        {
            Id = id;
            UserInformation = userInformation;
            MapquestInformation = mapquestInformation;
        }
    }

    public class TourUserInformation
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartLocation { get; set; }
        public string TargetLocation { get; set; }
        public string TransportType { get; set; }
        public string RouteInformation { get; set; }

        public TourUserInformation(string name, string description, string startLocation, string targetLocation, string transportType, string routeInformation)
        {
            Name = name;
            Description = description;
            StartLocation = startLocation;
            TargetLocation = targetLocation;
            TransportType = transportType;
            RouteInformation = routeInformation;
        }
    }

    public class TourMapquestInformation
    {
        public string Distance { get; set; }
        public string EstimatedTime { get; set; }

        public TourMapquestInformation(string distance, string estimatedTime)
        {
            Distance = distance;
            EstimatedTime = estimatedTime;
        }
    }
}
