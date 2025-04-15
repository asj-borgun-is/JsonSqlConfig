using JsonSqlConfigDb.Model;
using Microsoft.Extensions.Configuration;

namespace JsonSqlConfigDb.Provider
{
    public class JsonSqlConfigProvider : ConfigurationProvider
    {
        private JsonSqlContext _context;
        private IConfiguration _configuration;

        public JsonSqlConfigProvider(
            JsonSqlContext context
            , IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public override void Load()
        {
            var dict = _context.JsonUnits.Where(u => u.Value != null && u.SimpleType > JsonUnitSimpleType.None)
                .ToDictionary<JsonUnit, string, string>(u => u.Path, u => u.Value, StringComparer.OrdinalIgnoreCase);
            if (dict != null) Data = dict;
        }

        public void ReLoad()
        {
            var root = (IConfigurationRoot)_configuration;
            root.Reload();
        }
    }
}
