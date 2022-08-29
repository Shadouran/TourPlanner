using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TourPlanner.Shared.Models;

namespace TourPlanner.Client
{
    public static class Utility
    {
        public static string CalculatePopularity(int logSize)
        {
            switch(logSize)
            {
                case < 0:
                    return "Error";
                case 0:
                    return "No logs available";
                case <= 5:
                    return "Not popular";
                case <= 10:
                    return "Popular";
                case > 10:
                    return "Very popular";
            }
        }

        public static string CalculateChildFriendliness(Tour tour)
        {
            double totalTime = SumTotalTime(tour.Logs);

            var averageTime = totalTime / tour.Logs.Count;
            var distanceInKilometer = tour.Distance;
            var averageSpeed = distanceInKilometer / averageTime;

            if (averageSpeed <= 3.0f)
            {
                var totalRating = tour.Logs.Sum(log => log.Rating);
                var averageRating = totalRating / (tour.Logs.Count * 1.0f);
                if (averageRating >= 3.5f)
                    return "Yes";
            }
            return "No";
        }

        public static double SumTotalTime(IEnumerable<TourLog> logs)
        {
            return logs.Sum(log =>
            {
                if (log.TotalTime.Count(c => c.Equals(':')) > 1)
                    return TimeSpan.Parse(ReplaceFirstOccurence(log.TotalTime)).TotalHours;
                return TimeSpan.Parse(log.TotalTime).TotalHours;
            });
        }

        public static string ReplaceFirstOccurence(string str)
        {
            var stringBuilder = new StringBuilder(str);
            var index = str.IndexOf(':');
            stringBuilder.Remove(index, 1);
            stringBuilder.Insert(index, '.');
            var timeString = stringBuilder.ToString();
            return timeString;
        }

        // Validates strings in time format d*:HH:mm
        public static bool ValidateTime(string timeString)
        {
            const string pattern = @"^([1-9]+\d*:)?(2[0-3]|[01][0-9]):([0-5][0-9])$";
            Regex regex = new(pattern);
            return regex.IsMatch(timeString);
        }
    }
}
