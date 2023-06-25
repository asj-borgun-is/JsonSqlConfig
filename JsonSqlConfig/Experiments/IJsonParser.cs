using System.Text.Json;

namespace JsonSqlConfig.Experiments
{
    public interface IJsonParser
    {
        string Store(string jsonString);
        string Store(JsonElement element);
    }
}