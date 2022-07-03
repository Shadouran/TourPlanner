using Microsoft.AspNetCore.Mvc;
using TourPlanner.Server.DAL;
using TourPlanner.Server.DAL.Records;
using TourPlanner.Server.MapQuest;
using TourPlanner.Shared.Filesystem;
using TourPlanner.Shared.Log4Net;
using ILoggerFactory = TourPlanner.Shared.Logging.ILoggerFactory;

namespace TourPlanner.Server.Modules
{
    internal class TourModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, Log4NetFactory>(s =>
            {
                return new Log4NetFactory("log4net.config");
            });
            services.AddSingleton<INpgsqlDatabase, NpgsqlDatabase>();
            services.AddSingleton<ITourRepository, TourRepositoryPostgreSQL>();
            services.AddSingleton<IFilesystem, Filesystem>();
            services.AddSingleton<IMapAPI, MapQuestAPI>();
            return services;
        }

        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/tours", async ([FromServices] ITourRepository tourRepository) =>
            {
                var tours = await tourRepository.GetAllAsync();
                return Results.Ok(tours);
            });

            //endpoints.MapGet("/tours", async([FromServices] ITourRepository tourRepository, [FromQuery]string searchText) =>
            //{
            //    var tours = await tourRepository.GetMatchingAsync(searchText);
            //    return Results.Ok(tours);
            //});

            endpoints.MapGet("/tours/{id}", async ([FromServices] ITourRepository tourRepository, Guid id) =>
            {
                var tour = await tourRepository.GetByIdAsync(id);
                return tour is not null ? Results.Ok(tour) : Results.NotFound();
            });

            endpoints.MapGet("/tours/image/{id}", ([FromServices] IFilesystem filesystem, Guid id) =>
            {
                var image = filesystem.LoadImage(id);
                return image is not null ? Results.Bytes(image) : Results.NotFound();
            });

            endpoints.MapPost("/tours", async([FromServices] ITourRepository tourRepository,
                                            [FromServices] IConfiguration configuration,
                                            [FromServices] IMapAPI mapApi,
                                            [FromServices] IFilesystem filesystem,
                                            TourUserInformation tourUserInfo) =>
            {
                var apiKey = configuration.GetRequiredSection("MapAPI")["Key"];
                var uriBuilder = new MapQuestUriBuilder(apiKey);
                uriBuilder.Direction(tourUserInfo.StartLocation, tourUserInfo.TargetLocation);
                var uri = uriBuilder.Build();

                var info = await mapApi.GetDirections(uri);
                float Distance = info.Distance;
                int EstimatedTime = info.EstimatedTime;

                uriBuilder = new MapQuestUriBuilder(apiKey);
                uriBuilder.BoundingBox(info.UpperLeft, info.LowerRight);
                uriBuilder.Route(tourUserInfo.StartLocation, tourUserInfo.TargetLocation);
                uriBuilder.Size(800, 800);
                uri = uriBuilder.Build();

                var mapImageBytes = await mapApi.GetMapImage(uri);
                var id = filesystem.SaveImage(mapImageBytes);

                var tour = new Tour(tourUserInfo.Id, tourUserInfo.Name, tourUserInfo.Description, tourUserInfo.StartLocation, tourUserInfo.TargetLocation, tourUserInfo.TransportType, tourUserInfo.RouteInformation, Distance, EstimatedTime, id);

                await tourRepository.CreateAsync(tour);
                return Results.Created($"/tours/{tour.Id}", tour);
            });

            endpoints.MapPost("/tour/import", async([FromServices] ITourRepository tourRepository,
                                                    [FromServices] IConfiguration configuration,
                                                    [FromServices] IMapAPI mapApi,
                                                    [FromServices] IFilesystem filesystem,
                                                    Tour tour) =>
            {
                var apiKey = configuration.GetRequiredSection("MapAPI")["Key"];
                var uriBuilder = new MapQuestUriBuilder(apiKey);
                uriBuilder.Direction(tour.StartLocation, tour.TargetLocation);
                var uri = uriBuilder.Build();
                var info = await mapApi.GetDirections(uri);

                uriBuilder = new MapQuestUriBuilder(apiKey);
                uriBuilder.BoundingBox(info.UpperLeft, info.LowerRight);
                uriBuilder.Route(tour.StartLocation, tour.TargetLocation);
                uriBuilder.Size(800, 800);
                uri = uriBuilder.Build();

                var mapImageBytes = await mapApi.GetMapImage(uri);
                var id = filesystem.SaveImage(mapImageBytes);

                var newTour = new Tour(tour.Id, tour.Name, tour.Description, tour.StartLocation, tour.TransportType, tour.TargetLocation, tour.RouteInformation, tour.Distance, tour.EstimatedTime, id);

                await tourRepository.CreateAsync(newTour);
                return Results.Created($"/tours/{newTour.Id}", newTour);
            });

            endpoints.MapPut("/tours", async ([FromServices] ITourRepository tourRepository, Tour tour) =>
            {
                await tourRepository.UpdateAsync(tour);
                return Results.Ok(tour);
            });

            endpoints.MapDelete("/tours/{id}", async ([FromServices] ITourRepository tourRepository, Guid id) =>
            {
                await tourRepository.DeleteAsync(id);
                return Results.Ok(id);
            });

            return endpoints;
        }

    }
}