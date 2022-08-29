using Microsoft.Extensions.Configuration;
using Moq;
using TourPlanner.Server.MapQuest;
using TourPlanner.Shared.Logging;
using TourPlanner.Shared.Models;

namespace TourPlanner.Server.Test
{
    internal class MapQuestApiTests
    {
        [Test]
        public async Task GetDirections_InputValidStringValidRoute_ReturnsFilledObject()
        {
            // Arrange
            //var configMock = new Mock<IConfiguration>();
            //configMock.Setup(config => It.IsAny<IConfigurationSection>()["DirectionBaseAddress"]).Returns("http://www.mapquestapi.com/directions/v2/route");
            //configMock.Setup(config => It.IsAny<IConfigurationSection>()["StaticMapBaseAddress"]).Returns("http://www.mapquestapi.com/staticmap/v5/map");
            //var config = configMock.Object;
            var config = new ConfigurationBuilder().AddJsonFile(@"C:\Repositories\TourPlanner\TourPlanner.Server\appsettings.json", false, true).Build();

            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(logger => logger.Error(It.IsAny<string>()));
            var logger = loggerMock.Object;

            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(factory => factory.CreateLogger<MapQuestAPI>()).Returns(logger);
            var loggerFactory = loggerFactoryMock.Object;

            const string uri = "?key=fDicPGcYdYvvdZ4zKsXW7nAM9gi3789f&from=Wien&to=Graz";

            MapQuestAPI api = new(config, loggerFactory);

            // Act
            var result = await api.GetDirections(uri);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Distance, Is.GreaterThan(0.0f));
                Assert.That(result.EstimatedTime, Is.GreaterThan(0));
                Assert.That(result.UpperLeft.Longitude, Is.GreaterThan(0.0f));
                Assert.That(result.UpperLeft.Latitude, Is.GreaterThan(0.0f));
                Assert.That(result.LowerRight.Longitude, Is.GreaterThan(0.0f));
                Assert.That(result.LowerRight.Latitude, Is.GreaterThan(0.0f));
            });
        }

        [Test]
        public async Task GetDirections_InputInvalidString_ReturnsZeroFilledObject()
        {
            // Arrange
            //var configMock = new Mock<IConfiguration>();
            //configMock.Setup(config => It.IsAny<IConfigurationSection>()["DirectionBaseAddress"]).Returns("http://www.mapquestapi.com/directions/v2/route");
            //configMock.Setup(config => It.IsAny<IConfigurationSection>()["StaticMapBaseAddress"]).Returns("http://www.mapquestapi.com/staticmap/v5/map");
            //var config = configMock.Object;
            var config = new ConfigurationBuilder().AddJsonFile(@"C:\Repositories\TourPlanner\TourPlanner.Server\appsettings.json", false, true).Build();

            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(logger => logger.Error(It.IsAny<string>()));
            var logger = loggerMock.Object;

            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(factory => factory.CreateLogger<MapQuestAPI>()).Returns(logger);
            var loggerFactory = loggerFactoryMock.Object;

            const string uri = "?key=fDicPGcYdYvvdZ4zKsXW7nAM9gi3789f&to=Graz";

            MapQuestAPI api = new(config, loggerFactory);

            // Act
            var result = await api.GetDirections(uri);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Distance, Is.EqualTo(0.0f));
                Assert.That(result.EstimatedTime, Is.EqualTo(0));
                Assert.That(result.UpperLeft.Longitude, Is.EqualTo(0.0f));
                Assert.That(result.UpperLeft.Latitude, Is.EqualTo(0.0f));
                Assert.That(result.LowerRight.Longitude, Is.EqualTo(0.0f));
                Assert.That(result.LowerRight.Latitude, Is.EqualTo(0.0f));
            });
        }

        [Test]
        public async Task GetDirections_InputValidStringValidRouteInvalidKey_ThrowsException()
        {
            // Arrange
            //var configMock = new Mock<IConfiguration>();
            //configMock.Setup(config => It.IsAny<IConfigurationSection>()["DirectionBaseAddress"]).Returns("http://www.mapquestapi.com/directions/v2/route");
            //configMock.Setup(config => It.IsAny<IConfigurationSection>()["StaticMapBaseAddress"]).Returns("http://www.mapquestapi.com/staticmap/v5/map");
            //var config = configMock.Object;
            var config = new ConfigurationBuilder().AddJsonFile(@"C:\Repositories\TourPlanner\TourPlanner.Server\appsettings.json", false, true).Build();

            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(logger => logger.Error(It.IsAny<string>()));
            var logger = loggerMock.Object;

            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(factory => factory.CreateLogger<MapQuestAPI>()).Returns(logger);
            var loggerFactory = loggerFactoryMock.Object;

            const string uri = "?key=12345&from=Wien&to=Graz";

            MapQuestAPI api = new(config, loggerFactory);

            // Act
            var result = await api.GetDirections(uri);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Distance, Is.EqualTo(0.0f));
                Assert.That(result.EstimatedTime, Is.EqualTo(0));
                Assert.That(result.UpperLeft.Longitude, Is.EqualTo(0.0f));
                Assert.That(result.UpperLeft.Latitude, Is.EqualTo(0.0f));
                Assert.That(result.LowerRight.Longitude, Is.EqualTo(0.0f));
                Assert.That(result.LowerRight.Latitude, Is.EqualTo(0.0f));
            });
        }
    }
}