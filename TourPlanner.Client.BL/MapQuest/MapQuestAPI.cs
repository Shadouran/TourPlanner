using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Logging;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client.BL.MapQuest
{
    public class MapQuestAPI : IMapAPI
    {
        private readonly HttpClient _directionApi;
        private readonly HttpClient _staticMapApi;
        private readonly ILogger _logger;

        public MapQuestAPI(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _directionApi = new()
            {
                BaseAddress = new Uri(configuration.GetSection("mapAPI").GetSection("BaseAddresses")["directionBaseAddress"])
            };
            _staticMapApi = new()
            {
                BaseAddress = new Uri(configuration.GetSection("mapAPI").GetSection("BaseAddresses")["staticMapBaseAddress"])
            };
            _logger = loggerFactory.CreateLogger<MapQuestAPI>();
        }

        public async Task<MapDirections?> GetDirections(string uri)
        {
            try
            {
                var response = await _directionApi.GetStringAsync(uri);
                dynamic? deserialized = JsonConvert.DeserializeObject(response);
                var ci = new CultureInfo("en-US");
                float ullat = float.Parse(deserialized?.route.boundingBox.ul.lat.ToString(ci), ci);
                float ullng = float.Parse(deserialized?.route.boundingBox.ul.lng.ToString(ci), ci);
                float lrlat = float.Parse(deserialized?.route.boundingBox.lr.lat.ToString(ci), ci);
                float lrlng = float.Parse(deserialized?.route.boundingBox.lr.lng.ToString(ci), ci);
                float distance = float.Parse(deserialized?.route.distance.ToString(ci), ci);
                int time = int.Parse(deserialized?.route.time.ToString());
                return new(distance, time, new(ullat, ullng), new(lrlat, lrlng));
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
            }
            return null;
        }

        public async Task<byte[]?> GetMapImage(string uri)
        {
            try
            {
                return await _staticMapApi.GetByteArrayAsync(uri);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
            }
            return null;
        }

    }
}
