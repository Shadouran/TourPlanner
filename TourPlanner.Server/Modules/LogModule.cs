using Microsoft.AspNetCore.Mvc;
using TourPlanner.Server.DAL;
using TourPlanner.Server.DAL.Records;
using TourPlanner.Server.DAL.Repositories;
using TourPlanner.Shared.Log4Net;
using ILoggerFactory = TourPlanner.Shared.Logging.ILoggerFactory;

namespace TourPlanner.Server.Modules
{
    public class LogModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, Log4NetFactory>(s =>
            {
                return new Log4NetFactory("log4net.config");
            });
            services.AddSingleton<INpgsqlDatabase, NpgsqlDatabase>();
            services.AddSingleton<ILogRepository, LogRepositoryPostgreSQL>();
            return services;
        }

        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/tours/logs/", () =>
            {
                return Results.Ok(DateTime.Now);
            });

            endpoints.MapGet("/tours/logs/{tourId}", async ([FromServices] ILogRepository logRepository, Guid tourId) =>
            {
                var logs = await logRepository.GetAllLogsAsync(tourId);
                return Results.Ok(logs);
            });

            endpoints.MapPost("/tours/logs/{tourId}", async ([FromServices] ILogRepository logRepository, Guid tourId, TourLog log) =>
            {
                await logRepository.CreateAsync(log, tourId);
                return Results.Created($"/tours/logs/{tourId}", log);
            });

            endpoints.MapPut("/tours/logs", async ([FromServices] ILogRepository logRepository, TourLog log) =>
            {
                await logRepository.UpdateAsync(log);
                return Results.Ok(log);
            });

            endpoints.MapDelete("/tours/logs/{id}", async ([FromServices] ILogRepository logRepository, Guid id) =>
            {
                await logRepository.DeleteAsync(id);
                return Results.Ok(id);
            });

            return endpoints;
        }
    }
}
