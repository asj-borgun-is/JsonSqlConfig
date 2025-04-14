using Microsoft.Data.SqlClient;

namespace JsonSqlConfigDb.Settings
{
    public class JsonSqlSettings
    {
        public string SqlDataSource { get; set; } = string.Empty;
        public string SqlInitialCatalog { get; set; }
        public string SqlUserID { get; set; }
        public string SqlPassword { get; set; }
        public bool SqlTrustServerCertificate { get; set; }
        public bool SensitiveLogging { get; set; } = false;

        public string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = SqlDataSource;
            if (SqlInitialCatalog is not null) builder.InitialCatalog = SqlInitialCatalog;
            if (SqlTrustServerCertificate) builder.TrustServerCertificate = SqlTrustServerCertificate;
            if (SqlUserID is not null) builder.UserID = SqlUserID;
            if (SqlPassword is not null) builder.Password = SqlPassword;
            if (SqlUserID is null && SqlPassword is null) builder.IntegratedSecurity = true;
            return builder.ConnectionString; 
        }
    }
}
