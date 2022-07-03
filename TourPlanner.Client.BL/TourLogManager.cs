using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Client.DAL;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.BL
{
    public class TourLogManager : ILogManager
    {
        private readonly ILogDataAccess _logDataAccess;

        public TourLogManager(ILogDataAccess logDataAccess)
        {
            _logDataAccess = logDataAccess;
        }
        public async void AddTourLogAsync(Guid tourId, TourLog? log)
        {
            await _logDataAccess.AddTourLogAsync(tourId, log);
        }

        public async Task DeleteTourLogAsync(Guid? id)
        {
            await _logDataAccess.DeleteTourLogAsync(id);
        }

        public async void EditTourLogAsync(Guid tourId, TourLog? log)
        {
            await _logDataAccess.EditTourLogAsync(tourId, log);
        }
    }
}
