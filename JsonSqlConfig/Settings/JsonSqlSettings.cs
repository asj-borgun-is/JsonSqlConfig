using Microsoft.Data.SqlClient;

namespace JsonSqlConfig.Settings
{
    public class JsonSqlSettings
    {
        public string SqlDataSource { get; set; } = string.Empty;
        public string SqlInitialCatalog { get; set; }
        public string SqlUserID { get; set; }
        public string SqlPassword { get; set; }
        public bool SqlTrustServerCertificate { get; set; }
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
            if (Instance.SqlInitialCatalog is not null) builder.InitialCatalog = Instance.SqlInitialCatalog;
            if (Instance.SqlTrustServerCertificate) builder.TrustServerCertificate = Instance.SqlTrustServerCertificate;
            if (Instance.SqlUserID is not null) builder.UserID = Instance.SqlUserID;
            if (Instance.SqlPassword is not null) builder.Password = Instance.SqlPassword;
            if (Instance.SqlUserID is null && Instance.SqlPassword is null) builder.IntegratedSecurity = true;
            return builder.ConnectionString;
        }
    }
}
