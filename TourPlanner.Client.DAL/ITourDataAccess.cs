using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.DAL
{
    public interface ITourDataAccess
    {
        Task AddTourAsync(TourUserInformation tour);
        Task DeleteTourAsync(Guid? id);
        Task EditTourAsync(Tour tour);
        Task<ICollection<Tour>?> GetAllToursAsync();
        Task<Tour?> GetTourById(Guid? id);
        Task<byte[]?> GetImageAsync(Guid? imageId);
        Task ImportTourAsync(Tour tour);
    }
}
