using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Server.DAL.Records;

namespace TourPlanner.Server.DAL
{
    public interface ITourRepository
    {
        IEnumerable<Tour> GetAll();
        Tour? GetById(Guid id);
        void Create(Tour tour);
        void Update(Tour tour);
        void Delete(Guid tour);
        Task<IEnumerable<Tour>> GetAllAsync();
        Task<Tour?> GetByIdAsync(Guid id);
        Task CreateAsync(Tour tour);
        Task UpdateAsync(Tour tour);
        Task DeleteAsync(Guid id);
    }
}
