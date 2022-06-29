using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Shared.Models
{
    public class MapDirections
    {
        public float Distance { get; set; }
        public int EstimatedTime { get; set; }
        public Coordinates UpperLeft { get; set; }
        public Coordinates LowerRight { get; set; }

        public MapDirections(float distance, int estimatedTime, Coordinates upperLeft, Coordinates lowerRight)
        {
            Distance = distance;
            EstimatedTime = estimatedTime;
            UpperLeft = upperLeft;
            LowerRight = lowerRight;
        }
    }

    public struct Coordinates
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public Coordinates(float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
