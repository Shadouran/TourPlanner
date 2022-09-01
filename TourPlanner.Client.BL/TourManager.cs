using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Client.DAL;
using TourPlanner.Shared.Filesystem;
using TourPlanner.Shared.Models;
using static TourPlanner.Shared.Delegates;

namespace TourPlanner.Client.BL
{
    public class TourManager : ITourManager
    {
        private readonly ITourDataAccess _tourDataAccess;
        private readonly IFilesystem _filesystem;

        public TourManager(ITourDataAccess tourDataAcess, IFilesystem filesystem)
        {
            _tourDataAccess = tourDataAcess;
            _filesystem = filesystem;
        }

        public async Task<Tour?> AddTourAsync(TourUserInformation tour)
        {
            tour.Id = Guid.NewGuid();
            return await _tourDataAccess.AddTourAsync(tour);
        }

        public async Task DeleteTourAsync(Guid? id)
        {
            await _tourDataAccess.DeleteTourAsync(id);
        }

        public async Task<Tour?> EditTourAsync(Tour tour)
        {
            _filesystem.DeleteImage(tour.ImageFileName);
            return await _tourDataAccess.EditTourAsync(tour);
        }

        public async Task<IEnumerable<Tour>?> GetAllToursAsync()
        {
            return await _tourDataAccess.GetAllToursAsync();
        }

        public async Task<Tour?> GetTourAsync(Guid? id)
        {
            return await _tourDataAccess.GetTourById(id);
        }

        public async Task<Uri?> GetImageAsync(Guid imageId)
        {
            Guid cacheImageId;
            if(!_filesystem.ImageInCache(imageId))
            {
                var image = await _tourDataAccess.GetImageAsync(imageId);
                if(image == null)
                    return null;
                cacheImageId = _filesystem.SaveImage(image, imageId);
            }
            else
            {
                cacheImageId = imageId;
            }
            var imagePath = Path.Combine(_filesystem.FilesystemPath, Path.ChangeExtension(cacheImageId.ToString(), "jpeg"));
            var uri = new Uri(imagePath, UriKind.RelativeOrAbsolute);
            return uri;
        }

        public async Task ClearCache()
        {
            await _filesystem.ClearDirectoryAsync();
        }

        public async Task<IEnumerable<Tour>?> GetMatchingToursAsync(string searchText)
        {
            var tours = await _tourDataAccess.GetAllToursAsync();
            var matchingTours = tours.Where(t => t.Name.Contains(searchText) || t.Description.Contains(searchText));
            return matchingTours;
        }
        
        public async void ImportTourAsync(string filename)
        {
            Tour tour = _filesystem.ImportTour(filename);
            tour.Id = Guid.NewGuid();
            await _tourDataAccess.ImportTourAsync(tour);
        }

        public async void ExportTourAsync(Tour tour, string filename)
        {
            await _filesystem.ExportTourAsync(tour, filename);
        }
    }
}
