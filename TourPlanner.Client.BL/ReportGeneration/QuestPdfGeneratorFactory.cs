using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Filesystem;

namespace TourPlanner.Client.BL.ReportGeneration
{
    public class QuestPdfGeneratorFactory : IReportGeneratorFactory
    {
        private readonly IFilesystem _filesystem;

        public QuestPdfGeneratorFactory(IFilesystem filesystem)
        {
            _filesystem = filesystem;
        }

        public IReportGenerator CreateReportGenerator()
        {
            return new QuestPdfGenerator(_filesystem);
        }
    }
}
