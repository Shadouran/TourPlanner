using Microsoft.AspNetCore.Mvc;
using TourPlanner.Server.DAL;
using TourPlanner.Server.DAL.Records;

namespace TourPlanner.Server.Modules
{
    internal class TourModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection services)
        {
            services.AddSingleton<INpgsqlDatabase, NpgsqlDatabase>();
            services.AddSingleton<ITourRepository, TourRepositoryPostgreSQL>();
            return services;
        }

        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/tours", async ([FromServices] ITourRepository tourRepository) =>
            {
                var tours = await tourRepository.GetAllAsync();
                return Results.Ok(tours);
            });

            endpoints.MapGet("/tours/{id}", async ([FromServices] ITourRepository tourRepository, Guid id) =>
            {
                var tour = await tourRepository.GetByIdAsync(id);
                return tour is not null ? Results.Ok(tour) : Results.NotFound();
            });

            endpoints.MapPost("/tours", async ([FromServices] ITourRepository tourRepository, Tour tour) =>
            {
                await tourRepository.CreateAsync(tour);
                return Results.Created($"/tours/{tour.Id}", tour);
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