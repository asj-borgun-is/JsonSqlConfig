using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JsonSqlConfigDb.Provider
{
    internal class JsonSqlConfigSource : IConfigurationSource
    {
        private readonly IServiceCollection _services;

        public JsonSqlConfigSource(IServiceCollection services)
        {
            _services = services;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            // "Sneek peek" at services, since this is still configuration time
            var serviceProvider = _services.BuildServiceProvider(); 

            return serviceProvider.GetRequiredService<JsonSqlConfigProvider>();
        }
    }
}
