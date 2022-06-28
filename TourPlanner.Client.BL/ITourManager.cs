using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.BL
{
    public interface ITourManager
    {
        Task AddTourAsync(Tour tour);
        Task EditTourAsync(Tour tour);
        Task DeleteTourAsync(Guid? id);
        Task<IEnumerable<Tour>?> GetAllTourAsync();
        Task<Tour?> GetTourAsync(Guid? id);
    }
}
