using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Client.ExportImport
{
    internal class FilenameFetchDialog : IFilenameFetch
    {
        private readonly Dictionary<FileExtension, Tuple<string, string>> _dialogConfig = new();

        public FilenameFetchDialog(IConfiguration configuration)
        {
            _dialogConfig.Add(FileExtension.CSV, new(".csv", "CSV Files (.csv)|*.csv"));
            _dialogConfig.Add(FileExtension.PDF, new(".pdf", "PDF Files (.pdf)|*.pdf"));
        }

        public string? FetchFilename(string defaultName, FileExtension extension)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = defaultName,
                DefaultExt = _dialogConfig[extension].Item1,
                Filter = _dialogConfig[extension].Item2
            };
            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            return null;
        }
    }
}
