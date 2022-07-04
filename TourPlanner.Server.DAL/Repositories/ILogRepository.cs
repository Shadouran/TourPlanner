using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Server.DAL.Records;

namespace TourPlanner.Server.DAL.Repositories
{
    public interface ILogRepository
    {
        Task<IEnumerable<TourLog>> GetAllLogsAsync(Guid tourId);
        Task CreateAsync(TourLog log, Guid tourId);
        Task UpdateAsync(TourLog log);
        Task DeleteAsync(Guid tourId);
    }
}
