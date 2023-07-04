using JsonSqlConfigDb.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JsonSqlConfigDb.Provider
{
    public class JsonSqlConfigProvider : ConfigurationProvider
    {
        private readonly IServiceProvider _services;

        public JsonSqlConfigProvider(IServiceProvider services) 
        {
            _services = services;
        }

        public override void Load()
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetService<JsonSqlContext>();
            var dict = context.JsonUnits.Where(u => u.Value != null).ToDictionary<JsonUnit, string, string>(u => u.Path, u => u.Value, StringComparer.OrdinalIgnoreCase);
            if (dict != null) Data = dict;
        }

        //public override bool TryGet(string key, out string value)
        //{
        //    return base.TryGet(key, out value);
        //}
    }
}
