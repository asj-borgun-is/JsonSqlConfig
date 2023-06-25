using System.Text.Json;
using System.Text;
using JsonSqlConfigDb.Model;
using JsonSqlConfigDb;

namespace JsonSqlConfig.Experiments
{
    public class JsonParser : IJsonParser
    {
        private JsonSqlConfigContext _context;
        private ILogger _logger;

        public JsonParser(
            JsonSqlConfigContext context,
            ILogger<JsonParser> logger) 
        {
            _context = context;
            _logger = logger;
        }

        public string Store(string jsonString)
        {
            using var jdoc = JsonDocument.Parse(jsonString, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip });
            return Store(jdoc.RootElement);
        }

        public string Store(JsonElement element)
        {
            var builder = new StringBuilder();
            var rootUnit = new JsonUnit();

            StoreElement(element, rootUnit, builder);

            _context.JsonUnits.Add(rootUnit);
            var debugview = _context.ChangeTracker.DebugView.ShortView;
            _logger.LogInformation(debugview);
            _context.SaveChanges();

            var dump = builder.ToString();
            _logger.LogInformation(dump);

            return dump;
        }

        private void StoreElement(JsonElement element, JsonUnit unit, StringBuilder builder, string indent = "")
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                unit.CompositeType = JsonUnitCompositeType.Object;
                foreach (var p in element.EnumerateObject())
                {
                    StoreProperty(p, CreateChild(unit), builder, indent);
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                unit.CompositeType = JsonUnitCompositeType.Array;
                foreach (var e in element.EnumerateArray())
                {
                    StoreElement(e, CreateChild(unit), builder, indent);
                }
            }
            else if (element.ValueKind != JsonValueKind.Undefined)
            {
                builder.AppendLine($"{indent}{StoreValue(element, unit)}");
            }
        }

        private void StoreProperty(JsonProperty property, JsonUnit unit, StringBuilder builder, string indent = "")
        {
            if (property.Value.ValueKind == JsonValueKind.Object) StoreObjectProperty(property, unit, builder, indent);
            else if (property.Value.ValueKind == JsonValueKind.Array) StoreArrayProperty(property, unit, builder, indent);
            else StoreSimpleProperty(property, unit, builder, indent);
        }

        private void StoreSimpleProperty(JsonProperty property, JsonUnit unit, StringBuilder builder, string indent = "")
        {
            unit.Name = property.Name;
            var displayValue = StoreValue(property.Value, unit);

            var displayName = property.Name ?? "(null)";
            var displayKind = property.Value.ValueKind.ToString();
            builder.AppendLine($"{indent}Name: {displayName}, Kind: {displayKind}, Value: {displayValue}");
        }

        private void StoreObjectProperty(JsonProperty property, JsonUnit unit, StringBuilder builder, string indent = "")
        {
            unit.Name = property.Name;
            unit.CompositeType = JsonUnitCompositeType.Object;

            var displayName = property.Name ?? "(null)";
            var displayKind = property.Value.ValueKind.ToString();
            builder.AppendLine($"{indent}Name: {displayName}, Kind: {displayKind}");
            indent += "\t";
            foreach (var p in property.Value.EnumerateObject())
            {
                StoreProperty(p, CreateChild(unit), builder, indent);
            }
        }

        private void StoreArrayProperty(JsonProperty property, JsonUnit unit, StringBuilder builder, string indent = "")
        {
            unit.Name = property.Name;
            unit.CompositeType = JsonUnitCompositeType.Array;

            var displayName = property.Name ?? "(null)";
            var displayKind = property.Value.ValueKind.ToString();
            builder.AppendLine($"{indent}Name: {displayName}, Kind: {displayKind}");
            indent += "\t";
            foreach (var e in property.Value.EnumerateArray())
            {
                StoreElement(e, CreateChild(unit), builder, indent);
            }
        }

        private string StoreValue(JsonElement element, JsonUnit unit)
        {
            if (element.ValueKind == JsonValueKind.String)
            {
                unit.SimpleType = JsonUnitSimpleType.String;
                unit.Value = element.GetRawText();
            }
            else if (element.ValueKind == JsonValueKind.Number)
            {
                unit.SimpleType = JsonUnitSimpleType.Number;
                unit.Value = element.GetRawText();
            }
            else if (element.ValueKind == JsonValueKind.True || element.ValueKind == JsonValueKind.False)
            {
                unit.SimpleType = JsonUnitSimpleType.Boolean;
                unit.Value = element.GetRawText();
            }
            else if (element.ValueKind == JsonValueKind.Null)
            {
                unit.SimpleType = JsonUnitSimpleType.Null;
                unit.Value = element.GetRawText();
            }

            string displayValue = string.Empty;
            if (element.ValueKind == JsonValueKind.String) displayValue = $"'{element.GetString()}'";
            else if (element.ValueKind == JsonValueKind.Number) displayValue = element.GetString();
            else displayValue = element.ValueKind.ToString();

            return displayValue;
        }

        private JsonUnit CreateChild(JsonUnit unit)
        {
            var child = new JsonUnit();
            unit.Child.Add(child);
            child.Parent = unit;
            return child;
        }
    }
}
