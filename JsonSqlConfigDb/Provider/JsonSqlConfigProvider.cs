using JsonSqlConfigDb.Model;
using Microsoft.Extensions.Configuration;

namespace JsonSqlConfigDb.Provider
{
    public class JsonSqlConfigProvider : ConfigurationProvider
    {
        public static JsonSqlConfigProvider Instance { get; set; }

        public override void Load()
        {
            //using var context = new JsonSqlContext(JsonSqlContext.Options);
            //var dict = context.JsonUnits.Where(u => u.Value != null && u.SimpleType > JsonUnitSimpleType.None)
            //    .ToDictionary<JsonUnit, string, string>(u => u.Path, u => u.Value, StringComparer.OrdinalIgnoreCase);
            //if (dict != null) Data = dict;
        }
    }
}
