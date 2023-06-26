using JsonSqlConfigDb.Model;
using System.Text.Json;

namespace JsonSqlConfig.Experiments
{
    public interface IJsonParser
    {
        JsonUnit Store(string jsonString);
        JsonUnit Store(JsonElement element);
        string GetJsonString(JsonUnit unit);
    }
}