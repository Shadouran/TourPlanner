using Newtonsoft.Json;
using System.Globalization;
using TourPlanner.Shared.Models;
using ILogger = TourPlanner.Shared.Logging.ILogger;
using ILoggerFactory = TourPlanner.Shared.Logging.ILoggerFactory;

namespace TourPlanner.Server.MapQuest
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
                BaseAddress = new Uri(configuration.GetRequiredSection("MapAPI").GetRequiredSection("BaseAddresses")["DirectionBaseAddress"])
            };
            _staticMapApi = new()
            {
                BaseAddress = new Uri(configuration.GetRequiredSection("MapAPI").GetRequiredSection("BaseAddresses")["StaticMapBaseAddress"])
            };
            _logger = loggerFactory.CreateLogger<MapQuestAPI>();
        }

        public async Task<MapDirections> GetDirections(string uri)
        {
            try
            {
                var response = await _directionApi.GetStringAsync(uri);
                dynamic? deserialized = JsonConvert.DeserializeObject(response);
                return ParseDirections(deserialized);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
            }
            return new(0.0f, 0, new(0.0f, 0.0f), new(0.0f, 0.0f));
        }

        private static MapDirections ParseDirections(dynamic deserialized)
        {
            var ci = new CultureInfo("en-US");
            float ullat = float.Parse(deserialized?.route.boundingBox.ul.lat.ToString(ci), ci);
            float ullng = float.Parse(deserialized?.route.boundingBox.ul.lng.ToString(ci), ci);
            float lrlat = float.Parse(deserialized?.route.boundingBox.lr.lat.ToString(ci), ci);
            float lrlng = float.Parse(deserialized?.route.boundingBox.lr.lng.ToString(ci), ci);
            float distance = float.Parse(deserialized?.route.distance.ToString(ci), ci);
            int time = int.Parse(deserialized?.route.time.ToString());
            return new(distance, time, new(ullat, ullng), new(lrlat, lrlng));
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
