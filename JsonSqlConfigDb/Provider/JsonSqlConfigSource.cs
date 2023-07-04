using Microsoft.Extensions.Configuration;

namespace JsonSqlConfigDb.Provider
{
    internal class JsonSqlConfigSource : IConfigurationSource
    {
        private readonly IServiceProvider _services;

        public JsonSqlConfigSource(IServiceProvider services) 
        {
            _services = services;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new JsonSqlConfigProvider(_services);
        }
    }
}
