using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Shared
{
    public static class Functions
    {
        public static string CalculatePopularity(int logSize)
        {
            switch(logSize)
            {
                case 0:
                    return "No logs available";
                case < 5:
                    return "Not popular";
                case < 10:
                    return "Popular";
                case >= 10:
                    return "Very popular";
            }
        }

        public static string CalculateChildFriendliness(Tour tour)
        {
            var totalTime = tour.Logs.Sum(log =>
            {
                if(log.TotalTime.Count(c => c.Equals(':')) > 1)
                {
                    var stringBuilder = new StringBuilder(log.TotalTime);
                    var index = log.TotalTime.IndexOf(':');
                    stringBuilder.Remove(index, 1);
                    stringBuilder.Insert(index, '.');
                    return TimeSpan.Parse(stringBuilder.ToString()).TotalHours;
                }
                return TimeSpan.Parse(log.TotalTime).TotalHours;
            });

            var averageTime = totalTime / tour.Logs.Count;
            var distanceInKilometer = tour.Distance * 1.609344;
            var averageSpeed = distanceInKilometer / averageTime;
            
            if(averageSpeed <= 3.0f)
            {
                var totalRating = tour.Logs.Sum(log => log.Rating);
                var averageRating = totalRating / tour.Logs.Count;
                if (averageRating >= 3.5f)
                    return "Yes";

            }

            return "No";
        }
    }
}
