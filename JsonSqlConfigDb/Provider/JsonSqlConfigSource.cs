using Microsoft.Extensions.Configuration;

namespace JsonSqlConfigDb.Provider
{
    internal class JsonSqlConfigSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            JsonSqlConfigProvider.Instance = new JsonSqlConfigProvider();
            return JsonSqlConfigProvider.Instance;
        }
    }
}
