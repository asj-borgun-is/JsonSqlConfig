using System.Text.Json;
using System.Text;
using JsonSqlConfigDb.Model;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;
using JsonSqlConfigDb.Provider;

namespace JsonSqlConfigDb.Service
{
    public class JsonSqlService : IJsonSqlService
    {
        private readonly JsonSqlContext _context;
        private readonly ILogger _logger;
        private readonly JsonSqlConfigProvider _provider;

        public JsonSqlService(
            JsonSqlContext context,
            ILogger<JsonSqlService> logger,
            JsonSqlConfigProvider provider)
        {
            _context = context;
            _logger = logger;
            _provider = provider;
        }

        public DatabaseFacade Database { get => _context.Database; }

        public async Task<JsonUnit> Store(string jsonString, string group = "")
        {
            using var jdoc = JsonDocument.Parse(jsonString, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip });
            return await Store(jdoc.RootElement, group);
        }

        public async Task<JsonUnit> Store(JsonElement element, string group = "")
        {
            var rootUnit = new JsonUnit();

            StoreElement(element, rootUnit);
            AssignIndex(rootUnit);
            AssignGroup(rootUnit, group);
            AssignPath(rootUnit);
            CheckDuplicatePathInUnit(rootUnit);
            await CheckDuplicatePathInDb(rootUnit);

            _context.JsonUnits.Add(rootUnit);

            var debugview = _context.ChangeTracker.DebugView.ShortView;
            _logger.LogDebug(debugview);

            await _context.SaveChangesAsync();

            return rootUnit;
        }

        public async Task<bool> Exists(string group)
        {
            return (await _context.JsonUnits.FirstOrDefaultAsync(u => u.Group == group)) != null;
        }

        public async Task<string> Get(string group)
        {
            // Load the group and get the root unit
            var rootUnit = await LoadGroup(group);
            if (rootUnit == null) return null;
            return Get(rootUnit);
        }

        public async Task Delete(string group)
        {
            var query = _context.JsonUnits.Where(u => u.Group == group);

            // Remove all units in group
            await foreach (var unit in query.AsAsyncEnumerable())
            {
                _context.JsonUnits.Remove(unit);
            }
            await _context.SaveChangesAsync();
        }

        public void LoadProvider()
        {
            _provider.Load();
        }

        private string Get(JsonUnit unit)
        {
            var sb = new StringBuilder();
            BuildJsonString(unit, sb);
            return sb.ToString();
        }

        private void StoreElement(JsonElement element, JsonUnit unit)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                unit.CompositeType = JsonUnitCompositeType.Object;
                foreach (var p in element.EnumerateObject())
                {
                    StoreProperty(p, CreateChild(unit));
                }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                unit.CompositeType = JsonUnitCompositeType.Array;
                foreach (var e in element.EnumerateArray())
                {
                    StoreElement(e, CreateChild(unit));
                }
            }
            else if (element.ValueKind != JsonValueKind.Undefined)
            {
                StoreValue(element, unit);
            }
        }

        private void StoreProperty(JsonProperty property, JsonUnit unit)
        {
            if (property.Value.ValueKind == JsonValueKind.Object) StoreObjectProperty(property, unit);
            else if (property.Value.ValueKind == JsonValueKind.Array) StoreArrayProperty(property, unit);
            else StoreSimpleProperty(property, unit);
        }

        private void StoreSimpleProperty(JsonProperty property, JsonUnit unit)
        {
            unit.Name = property.Name;
            var displayValue = StoreValue(property.Value, unit);
        }

        private void StoreObjectProperty(JsonProperty property, JsonUnit unit)
        {
            unit.Name = property.Name;
            unit.CompositeType = JsonUnitCompositeType.Object;

            foreach (var p in property.Value.EnumerateObject())
            {
                StoreProperty(p, CreateChild(unit));
            }
        }

        private void StoreArrayProperty(JsonProperty property, JsonUnit unit)
        {
            unit.Name = property.Name;
            unit.CompositeType = JsonUnitCompositeType.Array;

            foreach (var e in property.Value.EnumerateArray())
            {
                StoreElement(e, CreateChild(unit));
            }
        }

        private string StoreValue(JsonElement element, JsonUnit unit)
        {
            if (element.ValueKind == JsonValueKind.String)
            {
                unit.SimpleType = JsonUnitSimpleType.String;
                unit.Value = element.GetString();
            }
            else if (element.ValueKind == JsonValueKind.Number)
            {
                unit.SimpleType = JsonUnitSimpleType.Number;
                unit.Value = element.GetDecimal().ToString(NumberFormatInfo.InvariantInfo);
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
            else if (element.ValueKind == JsonValueKind.Number) displayValue = element.GetDecimal().ToString(NumberFormatInfo.InvariantInfo);
            else displayValue = element.ValueKind.ToString();

            return displayValue;
        }

        private void BuildJsonString(JsonUnit unit, StringBuilder sb, string indent = "")
        {
            if (unit.CompositeType == JsonUnitCompositeType.Object)
            {
                var name = unit.Name == null ? string.Empty : $"\"{unit.Name}\": ";
                sb.AppendLine($"{indent}{name}{{");
                var nextIndent = indent + "\t";
                var delimit = false;
                foreach (var child in unit.Child)
                {
                    if (delimit) sb.AppendLine(",");
                    delimit = true;

                    BuildJsonString(child, sb, nextIndent);
                }
                if (delimit) sb.AppendLine();
                sb.Append($"{indent}}}");
            }
            else if (unit.CompositeType == JsonUnitCompositeType.Array)
            {
                var name = unit.Name == null ? string.Empty : $"\"{unit.Name}\": ";
                sb.AppendLine($"{indent}{name}[");
                var nextIndent = indent + "\t";
                var delimit = false;
                foreach (var child in unit.Child)
                {
                    if (delimit) sb.AppendLine(",");
                    delimit = true;

                    BuildJsonString(child, sb, nextIndent);
                }
                if (delimit) sb.AppendLine();
                sb.Append($"{indent}]");
            }
            else
            {
                var name = unit.Name == null ? string.Empty : $"\"{unit.Name}\": ";
                string value;
                if (unit.SimpleType == JsonUnitSimpleType.String) value = $"\"{unit.Value}\"";
                else value = unit.Value;

                sb.Append($"{indent}{name}{value}");
            }
        }

        private JsonUnit CreateChild(JsonUnit unit)
        {
            var child = new JsonUnit();
            unit.Child.Add(child);
            child.Parent = unit;
            return child;
        }

        private void AssignIndex(JsonUnit unit)
        {
            if (unit.CompositeType == JsonUnitCompositeType.Array)
            {
                var index = 0;
                foreach (var child in unit.Child)
                {
                    child.Index = index;
                    index++;
                    AssignIndex(child);
                }
            }
            else if (unit.CompositeType == JsonUnitCompositeType.Object)
            {
                foreach (var child in unit.Child)
                {
                    AssignIndex(child);
                }
            }
        }

        private void AssignPath(JsonUnit unit, string path = "")
        {
            var delimiter = string.IsNullOrWhiteSpace(path) ? string.Empty : ":";
                    
            if (unit.Name != null) path += (delimiter + unit.Name.Trim());
            else if (unit.Index != null) path += (delimiter + unit.Index.ToString());

            if (!string.IsNullOrWhiteSpace(path)) unit.Path = path;
            else unit.Path = $"({unit.Group})";

            foreach (var child in unit.Child)
            {
                AssignPath(child, path);
            }
        }

        private void CheckDuplicatePathInUnit(JsonUnit unit, Dictionary<string, JsonUnit> dict = null)
        {
            dict ??= new Dictionary<string, JsonUnit>();

            if (dict.ContainsKey(unit.Path))
            {
                throw new JsonSqlException($"There is a duplicate property ({unit.Path}) in this group");
            }

            dict.Add(unit.Path, unit);
            foreach (var child in unit.Child)
            {
                CheckDuplicatePathInUnit(child, dict);
            }
        }

        private async Task CheckDuplicatePathInDb(JsonUnit unit)
        {
            var paths = ListUnits(unit).Where(u => u.Parent != null).Select(u => u.Path).ToList();
            var dupPath = await _context.JsonUnits.Where(u => u.Group != unit.Group && paths.Contains(u.Path)).FirstOrDefaultAsync();
            if (dupPath != null)
            {
                throw new JsonSqlException($"There is a duplicate property ({dupPath.Path}) in  group {dupPath.Group}");
            }
        }

        private List<JsonUnit> ListUnits(JsonUnit unit, List<JsonUnit> list = null)
        {
            list ??= new List<JsonUnit>();

            list.Add(unit);
            foreach (var child in unit.Child)
                ListUnits(child, list);

            return list;
        }

        private void AssignGroup(JsonUnit unit, string group)
        {
            unit.Group = group; 
            foreach (var child in unit.Child)
            {
                AssignGroup(child, group);
            }
        }

        private async Task<JsonUnit> LoadGroup(string group)
        {
            var query = _context.JsonUnits.Where(u => u.Group == group);

            // Load the whole tree and also get the root unit
            JsonUnit rootUnit = default;
            await foreach (var unit in query.AsAsyncEnumerable())
            {
                if (unit.ParentId == null) rootUnit = unit;
            }
            return rootUnit;
        }
    }
}
