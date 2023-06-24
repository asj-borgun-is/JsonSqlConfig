using System.Text.Json;
using System.Text;

namespace JsonSqlConfig.Experiments
{
    public class JsonParser
    {
        public static string Store(string jsonString)
        {
            using var jdoc = JsonDocument.Parse(jsonString, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip });
            var jroot = jdoc.RootElement;
            var builder = new StringBuilder();

            StoreElement(jroot, builder);

            var dump = builder.ToString();           
            return dump;
        }

        private static void StoreElement(JsonElement element, StringBuilder builder, string indent = "")
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                foreach (var p in element.EnumerateObject())
                {
                    StoreProperty(p, builder, indent);
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                foreach (var e in element.EnumerateArray())
                {
                    StoreElement(e, builder, indent);
                }
            }
            else
            {
                builder.AppendLine($"{indent}{GetDisplayValue(element)}");
            }
        }

        private static void StoreProperty(JsonProperty property, StringBuilder builder, string indent = "")
        {
            if (property.Value.ValueKind == JsonValueKind.Object) StoreObjectProperty(property, builder, indent);
            else if (property.Value.ValueKind == JsonValueKind.Array) StoreArrayProperty(property, builder, indent);
            else StoreSimpleProperty(property, builder, indent);
        }

        private static void StoreSimpleProperty(JsonProperty property, StringBuilder builder, string indent = "")
        {
            var displayName = property.Name ?? "(null)";
            var displayKind = property.Value.ValueKind.ToString();
            var displayValue = GetDisplayValue(property.Value);
            builder.AppendLine($"{indent}Name: {displayName}, Kind: {displayKind}, Value: {displayValue}");
        }

        private static void StoreObjectProperty(JsonProperty property, StringBuilder builder, string indent = "")
        {
            var displayName = property.Name ?? "(null)";
            var displayKind = property.Value.ValueKind.ToString();
            builder.AppendLine($"{indent}Name: {displayName}, Kind: {displayKind}");
            indent += "\t";
            foreach (var p in property.Value.EnumerateObject())
            {
                StoreProperty(p, builder, indent);
            }
        }

        private static void StoreArrayProperty(JsonProperty property, StringBuilder builder, string indent = "")
        {
            var displayName = property.Name ?? "(null)";
            var displayKind = property.Value.ValueKind.ToString();
            builder.AppendLine($"{indent}Name: {displayName}, Kind: {displayKind}");
            indent += "\t";
            foreach (var e in property.Value.EnumerateArray())
            {
                StoreElement(e, builder, indent);
            }
        }

        private static string GetDisplayValue(JsonElement element)
        {
            string displayValue = string.Empty;
            if (element.ValueKind == JsonValueKind.String) displayValue = $"'{element.GetString()}'";
            else if (element.ValueKind == JsonValueKind.Number) displayValue = element.GetString();
            else displayValue = element.ValueKind.ToString();

            return displayValue;
        }
    }
}
