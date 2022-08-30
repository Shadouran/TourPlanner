using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Filesystem;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.BL.ReportGeneration
{
    public class QuestPdfGenerator : IReportGenerator
    {
        private readonly IFilesystem _filesystem;

        public QuestPdfGenerator(IFilesystem filesystem)
        {
            _filesystem = filesystem;
        }

        public void GenerateTourReport(Tour tour, string filename)
        {
            var timeSpan = TimeSpan.FromMinutes(tour.EstimatedTime);
            var formattedTime = string.Format("{0:00}:{1:00}", timeSpan.TotalHours, timeSpan.Minutes);
            var document = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1, Unit.Centimetre);

                        page.Header()
                            .AlignCenter()
                            .Text("Tour Report")
                            .SemiBold().FontSize(36);

                        page.Content()
                            .Grid(grid =>
                            {
                                grid.VerticalSpacing(1, Unit.Centimetre);
                                grid.Columns(2);

                                grid.Item(1)
                                    .PaddingTop(40)
                                    .PaddingRight(20)
                                    .Grid(innerGrid =>
                                    {
                                        innerGrid.Columns(1);
                                        innerGrid.VerticalSpacing(1, Unit.Centimetre);
                                        innerGrid.Item().Text(tour.Name).FontSize(24);
                                        innerGrid.Item().Text(tour.Description);
                                    });
                                grid.Item(1).PaddingTop(40).MaxHeight(250).MaxWidth(250).Image(_filesystem.LoadImage(tour.ImageFileName));

                                grid.Item(2)
                                    .MinimalBox()
                                    .Border(1)
                                    .Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.ConstantColumn(90);
                                            columns.ConstantColumn(90);
                                            columns.ConstantColumn(90);
                                            columns.ConstantColumn(90);
                                            columns.ConstantColumn(90);
                                            columns.RelativeColumn();
                                        });

                                        table.Cell().LabelCell("Start");
                                        table.Cell().ColumnSpan(2).ValueCell().Text(tour.StartLocation);

                                        table.Cell().LabelCell("End");
                                        table.Cell().ColumnSpan(2).ValueCell().Text(tour.TargetLocation);

                                        table.Cell().LabelCell("Distance");
                                        table.Cell().ValueCell().AlignMiddle().Text(tour.Distance);

                                        table.Cell().LabelCell("Estimated Time");
                                        table.Cell().ValueCell().AlignMiddle().Text(formattedTime);

                                        table.Cell().LabelCell("Transport Type");
                                        table.Cell().ValueCell().AlignMiddle().Text(tour.TransportType);

                                        table.Cell().LabelCell("Route Information");
                                        table.Cell().ColumnSpan(5).ValueCell().Text(tour.RouteInformation);
                                    });

                                grid.Item(1)
                                    .Text("Logs")
                                    .FontSize(24);

                                foreach(var log in tour.Logs)
                                {
                                    grid.Item(2)
                                        .MinimalBox()
                                        .Border(1)
                                        .ShowEntire()
                                        .Table(table =>
                                        {
                                            table.ColumnsDefinition(columns =>
                                            {
                                                columns.RelativeColumn();
                                                columns.RelativeColumn();
                                                columns.RelativeColumn();
                                                columns.RelativeColumn();
                                            });

                                            table.Cell().LabelCell("Date / Time");
                                            table.Cell().ValueCell().Text(x =>
                                            {
                                                x.Span(log.Date.ToShortDateString());
                                                x.Span(" ");
                                                x.Span(log.Time);
                                            });

                                            table.Cell().LabelCell("Total Time");
                                            table.Cell().ValueCell().Text(log.TotalTime);

                                            table.Cell().LabelCell("Difficulty");
                                            table.Cell().ValueCell().Text(log.Difficulty.ToString());

                                            table.Cell().LabelCell("Rating");
                                            table.Cell().ValueCell().Text(log.Rating);

                                            table.Cell().LabelCell("Comment");
                                            table.Cell().ColumnSpan(3).ValueCell().Text(log.Comment);
                                        });
                                }
                            });
                        page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                    });

                });
            var pdf = document.GeneratePdf();
            _filesystem.SaveReport(pdf, filename, FileExtension.PDF);
        }

        public void GenerateSummaryReport(IEnumerable<Tour> tours, string filename)
        {
            var document = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);

                    page.Header()
                        .AlignCenter()
                        .Text("Summary Report")
                        .SemiBold().FontSize(36);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(1, Unit.Centimetre);

                            foreach(var tour in tours)
                            {
                                TimeSpan averageTime = new();
                                int averageRating = 0;
                                foreach(var log in tour.Logs)
                                {
                                    averageTime += TimeSpan.Parse(log.TotalTime);
                                    averageRating += log.Rating;
                                }
                                averageRating =  tour.Logs.Count <= 0 ? averageRating : averageRating / tour.Logs.Count;
                                averageTime = tour.Logs.Count <= 0 ? averageTime : averageTime.Divide(tour.Logs.Count);

                                column.Item()
                                    .MinimalBox()
                                    .Border(1)
                                    .ShowEntire()
                                    .Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                        });

                                        table.Cell().LabelCell("Name");
                                        table.Cell().ColumnSpan(5).ValueCell().Text(tour.Name);

                                        table.Cell().LabelCell("Average Rating");
                                        table.Cell().ValueCell().Text(averageRating);

                                        table.Cell().LabelCell("Average Time");
                                        table.Cell().ValueCell().Text(string.Format("{0}:{1:00}:{2:00}", averageTime.Days, averageTime.Hours, averageTime.Minutes));

                                        table.Cell().LabelCell("Distance");
                                        table.Cell().ValueCell().Text(tour.Distance);
                                    });
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });

                });
            });
            var pdf = document.GeneratePdf();
            _filesystem.SaveReport(pdf, filename, FileExtension.PDF);
        }
    }
}
