using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Client.BL.MapQuest;
using TourPlanner.Client.DAL;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.BL
{
    public class TourManager : ITourManager
    {
        //TODO: add validation here somewhere
        private readonly ITourDataAccess _tourDataAccess;
        private readonly IMapAPI _mapApi;
        private readonly IFilesystem _filesystem;
        private readonly string _apiKey;

        public TourManager(ITourDataAccess tourDataAcess, IMapAPI mapApi, IFilesystem filesystem, IConfiguration config)
        {
            _tourDataAccess = tourDataAcess;
            _mapApi = mapApi;
            _filesystem = filesystem;
            _apiKey = config.GetSection("MapAPI")["key"];
        }

        public async Task AddTourAsync(Tour tour)
        {
            tour.Id = Guid.NewGuid();

            var uriBuilder = new MapQuestUriBuilder(_apiKey);
            uriBuilder.Direction(tour.StartLocation, tour.TargetLocation);
            var uri = uriBuilder.Build();

            var info = await _mapApi.GetDirections(uri);
            tour.Distance = info?.Distance;
            tour.EstimatedTime = info?.EstimatedTime;

            uriBuilder = new MapQuestUriBuilder(_apiKey);
            uriBuilder.BoundingBox(info.UpperLeft, info.LowerRight);
            uriBuilder.Route(tour.StartLocation, tour.TargetLocation);
            uriBuilder.Size(800, 800);
            uri = uriBuilder.Build();

            var mapImageBytes = await _mapApi.GetMapImage(uri);
            var id = _filesystem.SaveImage(mapImageBytes);
            tour.ImageFileName = id;

            await _tourDataAccess.AddTourAsync(tour);
        }

        public async Task DeleteTourAsync(Guid? id)
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

        public async Task<Tour?> GetTourAsync(Guid? id)
        {
            return await _tourDataAccess.GetTourById(id);
        }
    }
}
