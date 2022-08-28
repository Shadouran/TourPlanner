using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Shared.Models
{
    public class TourLog
    {
        public Guid? Id { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public Difficulty Difficulty { get; set; }
        public string TotalTime { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }

        public TourLog(Guid? id, DateTime date, string time, Difficulty difficulty, string totalTime, string comment, int rating)
        {
            Id = id;
            Date = date;
            Time = time;
            Difficulty = difficulty;
            TotalTime = totalTime;
            Comment = comment;
            Rating = rating;
        }
    }
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public enum Rating
    {
        None,
        One,
        Two,
        Three,
        Four,
        Five
    }
}
