using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.BL.ReportGeneration
{
    public interface IReportGenerator
    {
        void GenerateTourReport(Tour tour, string filename);
        void GenerateSummaryReport(IEnumerable<Tour> tours, string filename);
    }
}
