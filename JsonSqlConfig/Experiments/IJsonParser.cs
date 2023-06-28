using JsonSqlConfigDb.Model;
using System.Text.Json;

namespace JsonSqlConfig.Experiments
{
    public interface IJsonParser
    {
        JsonUnit Store(string jsonString, string group = "");
        JsonUnit Store(JsonElement element, string group = "");
        string GetJsonString(JsonUnit unit);
    }
}