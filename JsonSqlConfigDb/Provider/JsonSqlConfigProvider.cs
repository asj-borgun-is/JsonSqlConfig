using Microsoft.Extensions.Configuration;

namespace JsonSqlConfigDb.Provider
{
    public class JsonSqlConfigProvider : ConfigurationProvider
    {
        public override void Load()
        {
        }

        public override bool TryGet(string key, out string value)
        {
            return base.TryGet(key, out value);
        }
    }
}
