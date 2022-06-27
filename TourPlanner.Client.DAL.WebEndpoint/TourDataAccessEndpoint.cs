using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.DAL.Endpoint
{
    public class TourDataAccessEndpoint : ITourDataAccess
    {
        private readonly HttpClient _httpClient;
        public TourDataAccessEndpoint()
        {
            _httpClient = new()
            {
                BaseAddress = new Uri("http://localhost:3000/")
            };
        }
        public async Task AddTourAsync(Tour tour)
        {
            try
            {
                await _httpClient.PostAsJsonAsync("/tours", tour);
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }

        public async Task DeleteTourAsync(Guid id)
        {
            try
            {
                await _httpClient.DeleteAsync($"/tours/{id}");
            }
            catch (HttpRequestException e)
            {
                throw e;
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
                throw e;
            }
        }

        public async Task<ICollection<Tour>?> GetAllToursAsync()
        {
            try
            {
                var tours = await _httpClient.GetFromJsonAsync<ICollection<Tour>?>("/tours");
                return tours;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }

        }

        public async Task<Tour?> GetTourById(Guid id)
        {
            try
            {
                var tour = await _httpClient.GetFromJsonAsync<Tour?>($"/tours/{id}");
                return tour;
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
        }
    }
}
