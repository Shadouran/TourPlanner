namespace TourPlanner.Server.Modules
{
    public static class ModuleExtensions
    {
        private static readonly List<IModule> _registeredModules = new();
        
        public static IServiceCollection RegisterModules(this IServiceCollection services)
        {
            var modules = DiscoverModules();
            foreach(var module in modules)
            {
                module.RegisterModule(services);
                _registeredModules.Add(module);
            }
            return services;
        }

        public static WebApplication MapEndpoints(this WebApplication app)
        {
            foreach(var module in _registeredModules)
            {
                module.MapEndpoints(app);
            }
            return app;
        }

        private static IEnumerable<IModule> DiscoverModules()
        {
            return typeof(IModule).Assembly
                .GetTypes()
                .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
                .Select(Activator.CreateInstance)
                .Cast<IModule>();
        }
    }
}