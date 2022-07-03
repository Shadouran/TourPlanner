using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Logging;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.DAL.Endpoint
{
    public class TourDataAccessEndpoint : ITourDataAccess
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        public TourDataAccessEndpoint(ILoggerFactory factory, IConfiguration configuration)
        {
            var section = configuration.GetRequiredSection("Endpoints");
            _httpClient = new()
            {
                BaseAddress = new Uri($"{section["baseAddress"]}:{section["port"]}")
            };
            _logger = factory.CreateLogger<TourDataAccessEndpoint>();
        }
        public async Task AddTourAsync(TourUserInformation tour)
        {
            try
            {
                await _httpClient.PostAsJsonAsync("/tours", tour);
            }
            catch (HttpRequestException e)
            {
                _logger.Error(e.Message);
            }
        }

        public async Task DeleteTourAsync(Guid? id)
        {
            try
            {
                await _httpClient.DeleteAsync($"/tours/{id}");
            }
            catch (HttpRequestException e)
            {
                _logger.Error(e.Message);
            }
        }

        public async Task EditTourAsync(Tour tour)
        {
            try
            {
                await _httpClient.PutAsJsonAsync($"/tours/{tour.Id}", tour);
            }
            catch (HttpRequestException e)
            {
                _logger.Error(e.Message);
            }
        }

        public async Task<ICollection<Tour>?> GetAllToursAsync()
        {
            try
            {
                var tours = await _httpClient.GetFromJsonAsync<ICollection<Tour>>("/tours");
                return tours;
            }
            catch (HttpRequestException e)
            {
                _logger.Error(e.Message);
            }
            return null;

        }

        public async Task<byte[]?> GetImageAsync(Guid? imageId)
        {
            try
            {
                var image = await _httpClient.GetByteArrayAsync($"/tours/image/{imageId}");
                return image;
            }
            catch (HttpRequestException e)
            {
                _logger.Error(e.Message);
            }
            return null;
        }

        public async Task<IEnumerable<Tour>?> GetMatchingToursAsync(string searchText)
        {
            try
            {
                var tours = await _httpClient.GetFromJsonAsync<IEnumerable<Tour>?>($"/tours?search={searchText}");
                return tours;
            }
            catch (HttpRequestException e)
            {
                _logger.Error(e.Message);
            }
            return null;
        }

        public async Task<Tour?> GetTourById(Guid? id)
        {
            try
            {
                var tour = await _httpClient.GetFromJsonAsync<Tour?>($"/tours/{id}");
                return tour;
            }
            catch (HttpRequestException e)
            {
                _logger.Error(e.Message);
            }
            return null;
        }

        public async Task ImportTourAsync(Tour tour)
        {
            try
            {
                await _httpClient.PostAsJsonAsync("/tour/import", tour);
            }
            catch (HttpRequestException e)
            {
                _logger.Error(e.Message);
            }
        }
    }
}
