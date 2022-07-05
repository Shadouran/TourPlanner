using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.BL
{
    public interface ILogManager
    {
        Task<IEnumerable<TourLog>?> GetAllTourLogsAsync(Guid tourId);
        void EditTourLogAsync(TourLog? log);
        Task DeleteTourLogAsync(Guid? id);
        void AddTourLogAsync(Guid tourId, TourLog? log);
    }
}
