using JsonSqlConfigDb.Model;
using System.Text.Json;

namespace JsonSqlConfig.Support
{
    public interface IJsonSupport
    {
        Task<JsonUnit> Store(string jsonString, string group = "");
        Task<JsonUnit> Store(JsonElement element, string group = "");
        Task<string> Get(string group);
        string Get(JsonUnit unit);
        Task<bool> Exists(string group);
        Task Delete(string group);
    }
}