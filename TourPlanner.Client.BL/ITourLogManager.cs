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
        void EditTourLogAsync(Guid tourId, TourLog? log);
        Task DeleteTourLogAsync(Guid? id);
        void AddTourLogAsync(Guid tourId, TourLog? log);
    }
}
