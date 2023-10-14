using JsonSqlConfigDb.Model;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text.Json;

namespace JsonSqlConfigDb.Service
{
    public interface IJsonSqlService
    {
        DatabaseFacade Database { get; }
        Task<JsonUnit> Store(string jsonString, string group = "");
        Task<JsonUnit> Store(JsonElement element, string group = "");
        Task<string> Get(string group);
        Task<bool> Exists(string group);
        Task Delete(string group);
        void LoadProvider();
    }
}