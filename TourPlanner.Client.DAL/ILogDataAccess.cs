using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.DAL
{
    public interface ILogDataAccess
    {
        Task AddTourLogAsync(Guid tourId, TourLog? log);
        Task DeleteTourLogAsync(Guid? id);
        Task EditTourLogAsync(TourLog? log);
        Task<IEnumerable<TourLog>> GetAllTourLogsAsync(Guid tourId);
    }
}
