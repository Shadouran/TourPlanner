﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;
using static TourPlanner.Shared.Delegates;

namespace TourPlanner.Client.DAL
{
    public interface ITourDataAccess
    {
        Task<Tour?> AddTourAsync(TourUserInformation tour);
        Task DeleteTourAsync(Guid? id);
        Task<Tour?> EditTourAsync(Tour tour);
        Task<ICollection<Tour>?> GetAllToursAsync();
        Task<Tour?> GetTourById(Guid? id);
        Task<byte[]?> GetImageAsync(Guid? imageId);
        Task<IEnumerable<Tour>?> GetMatchingToursAsync(string searchText);
        Task ImportTourAsync(Tour tour);
    }
}
