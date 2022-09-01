using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Server.DAL.Records
{
    public record Tour(Guid Id, string Name, string Description, string StartLocation, string TargetLocation, TransportType TransportType, string RouteInformation, float Distance, int EstimatedTime, Guid ImageFileName);
    public record TourUserInformation(Guid Id, string Name, string Description, string StartLocation, string TargetLocation, TransportType TransportType, string RouteInformation);
    public record TourLog(Guid Id, DateTime Date, string Time, string TotalTime, Difficulty Difficulty, int Rating, string Comment);
}
