using JsonSqlConfigDb.Model;
using System.Text.Json;

namespace JsonSqlConfig.Support
{
    public interface IJsonSupport
    {
        Task<JsonUnit> Store(string jsonString, string group = "");
        Task<JsonUnit> Store(JsonElement element, string group = "");
        Task<string> GetJsonString(string group);
        string GetJsonString(JsonUnit unit);
        Task<bool> GroupExists(string group);
    }
}