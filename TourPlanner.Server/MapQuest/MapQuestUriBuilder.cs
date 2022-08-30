using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Server.MapQuest
{
    public class MapQuestUriBuilder
    {
        private readonly StringBuilder _stringBuilder;
        private readonly CultureInfo _cultureInfo;

        public MapQuestUriBuilder(string apiKey)
        {
            _stringBuilder = new();
            _cultureInfo = new("en-US");
            _stringBuilder.Append("?key=");
            _stringBuilder.Append(apiKey);
        }

        public MapQuestUriBuilder BoundingBox(Coordinates upperLeft, Coordinates lowerRight)
        {
            _stringBuilder.Append($"&boundingBox={upperLeft.Latitude.ToString(_cultureInfo)},{upperLeft.Longitude.ToString(_cultureInfo)},{lowerRight.Latitude.ToString(_cultureInfo)},{lowerRight.Longitude.ToString(_cultureInfo)}");
            return this;
        }

        public MapQuestUriBuilder Direction(string from, string to)
        {
            _stringBuilder.Append($"&from={from}&to={to}");
            return this;
        }
        public MapQuestUriBuilder Route(string start, string end)
        {
            _stringBuilder.Append($"&start={start}&end={end}");
            return this;
        }

        public MapQuestUriBuilder Size(int width, int height)
        {
            _stringBuilder.Append($"&size={width},{height}");
            return this;
        }

        public string Build()
        {
            return _stringBuilder.ToString();
        }
    }
}
