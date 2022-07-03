using TourPlanner.Server.DAL;
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
            //TODO
        }
    }
}
