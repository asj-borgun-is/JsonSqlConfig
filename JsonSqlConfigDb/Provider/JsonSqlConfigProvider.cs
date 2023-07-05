using JsonSqlConfigDb.Model;
using Microsoft.Extensions.Configuration;

namespace JsonSqlConfigDb.Provider
{
    public class JsonSqlConfigProvider : ConfigurationProvider
    {
        public override void Load()
        {
            using var context = new JsonSqlContext(JsonSqlContext.Options);
            var dict = context.JsonUnits.Where(u => u.Value != null && u.SimpleType > JsonUnitSimpleType.None)
                .ToDictionary<JsonUnit, string, string>(u => u.Path, u => u.Value, StringComparer.OrdinalIgnoreCase);
            if (dict != null) Data = dict;
        }

        //public override bool TryGet(string key, out string value)
        //{
        //    return base.TryGet(key, out value);
        //}
    }
}
