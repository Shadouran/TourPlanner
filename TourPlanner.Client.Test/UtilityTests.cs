using TourPlanner.Shared.Models;

namespace TourPlanner.Client.Test
{
    public class UtilityTests
    {
        [Test]
        public void CalculatePopularity_InputMinusOne_ReturnsCorrectString()
        {
            // Arrange
            const string expectation = "Error";

            // Act
            var result = Utility.CalculatePopularity(-1);

            // Assert
            Assert.That(result, Is.EqualTo(expectation));

        }

        [Test]
        public void CalculatePopularity_InputZero_ReturnsCorrectString()
        {
            // Arrange
            const string expectation = "No logs available";

            // Act
            var result = Utility.CalculatePopularity(0);

            // Assert
            Assert.That(result, Is.EqualTo(expectation));

        }

        [Test]
        public void CalculatePopularity_InputFive_ReturnsCorrectString()
        {
            // Arrange
            const string expectation = "Not popular";

            // Act
            var result = Utility.CalculatePopularity(5);

            // Assert
            Assert.That(result, Is.EqualTo(expectation));

        }

        [Test]
        public void CalculatePopularity_InputTen_ReturnsCorrectString()
        {
            // Arrange
            const string expectation = "Popular";

            // Act
            var result = Utility.CalculatePopularity(10);

            // Assert
            Assert.That(result, Is.EqualTo(expectation));

        }

        [Test]
        public void CalculatePopularity_InputFifteen_ReturnsCorrectString()
        {
            // Arrange
            const string expectation = "Very popular";

            // Act
            var result = Utility.CalculatePopularity(15);

            // Assert
            Assert.That(result, Is.EqualTo(expectation));

        }

        [Test]
        public void CalculateChildFriendliness_InputDistanceThreeAverageTimeOneAverageRatingThreePointFive_ReturnsYes()
        {
            // Arrange
            const string expectation = "Yes";
            var tour = new Tour
            {
                Distance = 3.0f
            };
            var log1 = new TourLog(null, new(1, 1, 1), "00:00", Difficulty.Easy, "01:00", "", 3);
            var log2 = new TourLog(null, new(1, 1, 1), "00:00", Difficulty.Easy, "01:00", "", 4);
            tour.Logs.Add(log1);
            tour.Logs.Add(log2);

            // Act
            var result = Utility.CalculateChildFriendliness(tour);

            // Assert
            Assert.That(result, Is.EqualTo(expectation));

        }
        
        [Test]
        public void CalculateChildFriendliness_InputDistanceThreeAverageTimeZeroPointFiveAverageRatingThreePointFive_ReturnsNo()
        {
            // Arrange
            const string expectation = "No";
            var tour = new Tour
            {
                Distance = 3.0f
            };
            var log1 = new TourLog(null, new(1, 1, 1), "00:00", Difficulty.Easy, "00:30", "", 3);
            var log2 = new TourLog(null, new(1, 1, 1), "00:00", Difficulty.Easy, "00:30", "", 4);
            tour.Logs.Add(log1);
            tour.Logs.Add(log2);

            // Act
            var result = Utility.CalculateChildFriendliness(tour);

            // Assert
            Assert.That(result, Is.EqualTo(expectation));

        }

        [Test]
        public void CalculateChildFriendliness_InputDistanceThreeAverageTimeTwoAverageRatingThreePointFive_ReturnsYes()
        {
            // Arrange
            const string expectation = "Yes";
            var tour = new Tour
            {
                Distance = 3.0f
            };
            var log1 = new TourLog(null, new(1, 1, 1), "00:00", Difficulty.Easy, "02:00", "", 3);
            var log2 = new TourLog(null, new(1, 1, 1), "00:00", Difficulty.Easy, "02:00", "", 4);
            tour.Logs.Add(log1);
            tour.Logs.Add(log2);

            // Act
            var result = Utility.CalculateChildFriendliness(tour);

            // Assert
            Assert.That(result, Is.EqualTo(expectation));

        }

        [Test]
        public void CalculateChildFriendliness_InputDistanceSixAverageTimeTwoAverageRatingThreePointFive_ReturnsYes()
        {
            // Arrange
            const string expectation = "Yes";
            var tour = new Tour
            {
                Distance = 6.0f
            };
            var log1 = new TourLog(null, new(1, 1, 1), "00:00", Difficulty.Easy, "02:00", "", 3);
            var log2 = new TourLog(null, new(1, 1, 1), "00:00", Difficulty.Easy, "02:00", "", 4);
            tour.Logs.Add(log1);
            tour.Logs.Add(log2);

            // Act
            var result = Utility.CalculateChildFriendliness(tour);

            // Assert
            Assert.That(result, Is.EqualTo(expectation));

        }

        [Test]
        public void CalculateChildFriendliness_InputDistanceThreeAverageTimeOneAverageRatingThree_ReturnsNo()
        {
            // Arrange
            const string expectation = "No";
            var tour = new Tour
            {
                Distance = 3.0f
            };
            var log1 = new TourLog(null, new(1, 1, 1), "00:00", Difficulty.Easy, "02:00", "", 2);
            var log2 = new TourLog(null, new(1, 1, 1), "00:00", Difficulty.Easy, "02:00", "", 4);
            tour.Logs.Add(log1);
            tour.Logs.Add(log2);

            // Act
            var result = Utility.CalculateChildFriendliness(tour);

            // Assert
            Assert.That(result, Is.EqualTo(expectation));

        }

        [Test]
        public void ValidateTime_Validates00_00_ReturnsTrue()
        {
            // Arrange
            const string timeString = "00:00";

            // Act
            var result = Utility.ValidateTime(timeString);

            // Assert
            Assert.That(result, Is.True);

        }

        [Test]
        public void ValidateTime_Validates01_01_ReturnsTrue()
        {
            // Arrange
            const string timeString = "01:01";

            // Act
            var result = Utility.ValidateTime(timeString);

            // Assert
            Assert.That(result, Is.True);

        }

        [Test]
        public void ValidateTime_Validates01_59_ReturnsTrue()
        {
            // Arrange
            const string timeString = "01:59";

            // Act
            var result = Utility.ValidateTime(timeString);

            // Assert
            Assert.That(result, Is.True);

        }

        [Test]
        public void ValidateTime_Validates23_59_ReturnsTrue()
        {
            // Arrange
            const string timeString = "23:59";

            // Act
            var result = Utility.ValidateTime(timeString);

            // Assert
            Assert.That(result, Is.True);

        }

        [Test]
        public void ValidateTime_Validates24_00_ReturnsFalse()
        {
            // Arrange
            const string timeString = "24:00";

            // Act
            var result = Utility.ValidateTime(timeString);

            // Assert
            Assert.That(result, Is.False);

        }

        [Test]
        public void ValidateTime_Validates23_60_ReturnsFalse()
        {
            // Arrange
            const string timeString = "23:60";

            // Act
            var result = Utility.ValidateTime(timeString);

            // Assert
            Assert.That(result, Is.False);

        }

        [Test]
        public void ValidateTime_Validates100_00_ReturnsFalse()
        {
            // Arrange
            const string timeString = "100:00";

            // Act
            var result = Utility.ValidateTime(timeString);

            // Assert
            Assert.That(result, Is.False);

        }

        [Test]
        public void ValidateTime_Validates100_00_00_ReturnsTrue()
        {
            // Arrange
            const string timeString = "100:00:00";

            // Act
            var result = Utility.ValidateTime(timeString);

            // Assert
            Assert.That(result, Is.True);

        }

        [Test]
        public void ValidateTime_Validates_00_00_ReturnsFalse()
        {
            // Arrange
            const string timeString = ":00:00";

            // Act
            var result = Utility.ValidateTime(timeString);

            // Assert
            Assert.That(result, Is.False);

        }

        [Test]
        public void ValidateTime_Validates0_12_15_ReturnsFalse()
        {
            // Arrange
            const string timeString = "0:12:15";

            // Act
            var result = Utility.ValidateTime(timeString);

            // Assert
            Assert.That(result, Is.False);

        }

        [Test]
        public void ValidateTime_Validates1_7_15_ReturnsFalse()
        {
            // Arrange
            const string timeString = "1:7:15";

            // Act
            var result = Utility.ValidateTime(timeString);

            // Assert
            Assert.That(result, Is.False);

        }

        [Test]
        public void ValidateTime_Validates1_07_15_ReturnsTrue()
        {
            // Arrange
            const string timeString = "1:07:15";

            // Act
            var result = Utility.ValidateTime(timeString);

            // Assert
            Assert.That(result, Is.True);

        }
    }
}