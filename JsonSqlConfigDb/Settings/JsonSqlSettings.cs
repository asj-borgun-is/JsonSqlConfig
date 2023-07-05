using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace JsonSqlConfigDb.Settings
{
    public class JsonSqlSettings
    {
        public string SqlDataSource { get; set; } = string.Empty;
        public string SqlInitialCatalog { get; set; } = string.Empty;
        public string SqlUserID { get; set; } = string.Empty;
        public string SqlPassword { get; set; } = string.Empty;
        public bool SqlTrustServerCertificate { get; set; } = false;
        public bool SensitiveLogging { get; set; } = false;

        private static JsonSqlSettings _instance;
        public static JsonSqlSettings Instance { get => _instance; }

        public static JsonSqlSettings CreateInstance(IConfiguration configuration)
        {
            _instance = configuration.GetSection(nameof(JsonSqlSettings)).Get<JsonSqlSettings>() ?? new JsonSqlSettings();
            return _instance;
        }

        public string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = Instance.SqlDataSource;
            builder.InitialCatalog = Instance.SqlInitialCatalog;
            builder.TrustServerCertificate = Instance.SqlTrustServerCertificate;
            builder.UserID = Instance.SqlUserID;
            builder.Password = Instance.SqlPassword;
            return builder.ConnectionString;
        }
    }
}
