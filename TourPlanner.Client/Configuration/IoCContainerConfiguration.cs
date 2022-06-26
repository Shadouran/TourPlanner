using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TourPlanner.Shared.Log4Net;
using TourPlanner.Shared.Logging;

namespace TourPlanner.Client.IoCConfiguration
{
    internal class IoCContainerConfiguration
    {
        private readonly IServiceProvider _serviceProvider;

        public IoCContainerConfiguration()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ILoggerFactory, Log4NetFactory>(s =>
            {
                return new Log4NetFactory("log4net.config");
            });

            _serviceProvider = services.BuildServiceProvider();
        }
    }
}
