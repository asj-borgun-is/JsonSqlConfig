using JsonSqlConfigDb.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json;

namespace JsonSqlConfig.Service
{
    public interface IJsonService
    {
        DatabaseFacade Database { get; }
        Task<JsonUnit> Store(string jsonString, string group = "");
        Task<JsonUnit> Store(JsonElement element, string group = "");
        Task<string> Get(string group);
        string Get(JsonUnit unit);
        Task<bool> Exists(string group);
        Task Delete(string group);
    }
}