using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Logging;
using TourPlanner.Shared.Models;

namespace TourPlanner.Shared.Filesystem
{
    public class Filesystem : IFilesystem
    {
        private readonly ILogger _logger;
        private readonly string _path;
        public string FilesystemPath => _path;

        public Filesystem(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<Filesystem>();
            _path = configuration["ImageDirectory"];
        }

        public bool ImageInCache(Guid? id)
        {
            var path = Path.Combine(_path, id.ToString());
            path = Path.ChangeExtension(path, "jpeg");
            if (!File.Exists(path))
                return false;
            return true;
        }

        public byte[]? LoadImage(Guid? id)
        {
            if (id == null)
                return null;
            var path = Path.Combine(_path, id.ToString());
            path = Path.ChangeExtension(path, "jpeg");
            if (!File.Exists(path))
                return null;

            using var image = Image.FromFile(path);
            using var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            var array = ms.ToArray();
            return array;
        }

        public Guid SaveImage(byte[] bytes, Guid? imageId = null)
        {
            using var memoryStream = new MemoryStream(bytes);
            var image = Image.FromStream(memoryStream);
            var id = imageId ?? Guid.NewGuid();

            if(!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
            var path = Path.Combine(_path, id.ToString());

            image.Save(Path.ChangeExtension(path, "jpeg"), ImageFormat.Jpeg);
            return id;
        }

        public async Task ClearDirectoryAsync()
        {
            await Task.Run(() =>
            {
                var files = Directory.EnumerateFiles(_path);
                foreach (var file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e.Message);
                    }
                }
            });
        }

        public void DeleteImage(Guid id)
        {
            var path = Path.Combine(_path, id.ToString());
            path = Path.ChangeExtension(path, "jpeg");
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
            }
        }
        
        public Tour ImportTour(string filename)
        {
            var uri = new Uri(filename, UriKind.Absolute);
            using var reader = new StreamReader(uri.AbsolutePath);
            using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);
            try
            {
                var records = csv.GetRecords<Tour>();
                return records.First();
            }
            catch (Exception)
            {
                throw new InvalidFileException("CSV file content is invalid");
            }
        }

        public async Task ExportTourAsync(Tour tour, string filename)
        {
            await Task.Run(() =>
            {
                using var writer = new StreamWriter(filename);
                using var csv = new CsvWriter(writer, CultureInfo.CurrentCulture);
                var list = new List<Tour>
                {
                    tour
                };
                try
                {
                    csv.WriteRecords(list);
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                }
            });
        }

        public async void SaveReport(byte[] report, string filename, FileExtension extension)
        {
            await Task.Run(() =>
            {
                switch(extension)
                {
                    case FileExtension.PDF:
                        Path.ChangeExtension(filename, "pdf");
                        break;
                }
                try
                {
                    File.WriteAllBytes(filename, report);
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                }
            });
        }
    }
}
