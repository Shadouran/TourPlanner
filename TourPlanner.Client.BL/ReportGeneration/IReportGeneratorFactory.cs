using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Filesystem;

namespace TourPlanner.Client.BL.ReportGeneration
{
    public interface IReportGeneratorFactory
    {
        IReportGenerator CreateReportGenerator();
    }
}
