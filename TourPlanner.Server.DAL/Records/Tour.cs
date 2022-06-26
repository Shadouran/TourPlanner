using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Server.DAL.Records
{
    public record Tour(Guid Id, string Name, string Description, string StartLocation, string TargetLocation, string TransportType, string Distance, string EstimatedTime, string RouteInformation);
}
