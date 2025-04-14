using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JsonSqlConfigDb.Provider
{
    internal class JsonSqlConfigSource : IConfigurationSource
    {
        private readonly IServiceProvider _serviceProvider;

        public JsonSqlConfigSource(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return _serviceProvider.GetRequiredService<JsonSqlConfigProvider>();
        }
    }
}
