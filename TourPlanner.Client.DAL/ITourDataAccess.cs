using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.DAL
{
    interface ITourDataAccess
    {
        void AddTour(Tour tour);
        void RemoveTour(Tour tour);
        void EditTour(Tour tour);
        ICollection<Tour> GetTours();
        Tour GetTour(string? id);
    }
}
