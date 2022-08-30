using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.ExportImport
{
    public interface IFilenameFetch
    {
        string? FetchFilename(string defaultName, FileExtension extension);
    }
}
