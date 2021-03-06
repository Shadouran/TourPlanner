using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Server.MapQuest
{
    internal class MapQuestUriBuilder
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

        public void BoundingBox(Coordinates upperLeft, Coordinates lowerRight)
        {
            _stringBuilder.Append($"&boundingBox={upperLeft.Latitude.ToString(_cultureInfo)},{upperLeft.Longitude.ToString(_cultureInfo)},{lowerRight.Latitude.ToString(_cultureInfo)},{lowerRight.Longitude.ToString(_cultureInfo)}");
        }

        public void Direction(string from, string to)
        {
            _stringBuilder.Append($"&from={from}&to={to}");
        }
        public void Route(string start, string end)
        {
            _stringBuilder.Append($"&start={start}&end={end}");
        }

        public void Size(int width, int height)
        {
            _stringBuilder.Append($"&size={width},{height}");
        }

        public string Build()
        {
            return _stringBuilder.ToString();
        }
    }
}
