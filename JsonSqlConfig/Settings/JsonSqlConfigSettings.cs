using Microsoft.Data.SqlClient;

namespace JsonSqlConfigDb.Settings
{
    public class JsonSqlConfigSettings
    {
        public string SqlDataSource { get; set; } = string.Empty;
        public string SqlInitialCatalog { get; set; } = string.Empty;
        public string SqlUserID { get; set; } = string.Empty;
        public string SqlPassword { get; set; } = string.Empty;
        public bool SqlTrustServerCertificate { get; set; }

        public static JsonSqlConfigSettings Instance { get; set; }

        public static JsonSqlConfigSettings CreateInstance(IConfiguration configuration)
        {
            Instance = configuration.GetSection(nameof(JsonSqlConfigSettings)).Get<JsonSqlConfigSettings>() ?? new JsonSqlConfigSettings();
            return Instance;
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
