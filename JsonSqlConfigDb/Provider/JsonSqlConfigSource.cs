using Microsoft.Extensions.Configuration;

namespace JsonSqlConfigDb.Provider
{
    internal class JsonSqlConfigSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new JsonSqlConfigProvider();
        }
    }
}
