using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Client.DAL;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.BL
{
    public class TourManager : ITourManager
    {
        //TODO: add validation here somewhere
        private readonly ITourDataAccess _tourDataAccess;
        public TourManager(ITourDataAccess tourDataAcess)
        {
            _tourDataAccess = tourDataAcess;
        }
        public async Task AddTourAsync(Tour tour)
        {
            await _tourDataAccess.AddTourAsync(tour);
        }

        public async Task DeleteTourAsync(Guid id)
        {
            await _tourDataAccess.DeleteTourAsync(id);
        }

        public async Task EditTourAsync(Tour tour)
        {
            await _tourDataAccess.EditTourAsync(tour);
        }

        public async Task<IEnumerable<Tour>?> GetAllTourAsync()
        {
            return await _tourDataAccess.GetAllToursAsync();
        }

        public async Task<Tour?> GetTourAsync(Guid id)
        {
            return await _tourDataAccess.GetTourById(id);
        }
    }
}
