using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.BL
{
    public interface ITourManager
    {
        Task AddTourAsync(TourUserInformation tour);
        Task<Tour?> EditTourAsync(Tour tour);
        Task DeleteTourAsync(Guid? id);
        Task<IEnumerable<Tour>?> GetAllToursAsync();
        Task<Tour?> GetTourAsync(Guid? id);
        Task<Uri?> GetImageAsync(Guid imageId);
        Task ClearCache();
        Task<IEnumerable<Tour>?> GetMatchingToursAsync(string searchText);
        void ImportTourAsync(string filename);
        void ExportTourAsync(Tour tour, string filename);
    }
}
