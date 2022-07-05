using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Client.BL.ReportGeneration
{
    static public class QuestPdfExtension
    {
        private static IContainer Cell(this IContainer container, bool dark)
        {
            return container
                .Border(1)
                .Background(dark ? Colors.Grey.Lighten2 : Colors.White)
                .Padding(5)
                .AlignMiddle();
        }

        public static void LabelCell(this IContainer container, string text) => container.Cell(true).Text(text).Medium();

        public static IContainer ValueCell(this IContainer container) => container.Cell(false);
    }
}
