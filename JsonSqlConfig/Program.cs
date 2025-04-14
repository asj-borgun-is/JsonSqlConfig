using JsonSqlConfigDb;
using JsonSqlConfigDb.Extension;

namespace JsonSqlConfig
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            LightDbTest(app);

            Configure(app);

            app.Run();
        }

        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddApplicationInsightsTelemetry();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddJsonSqlConfigDb(builder.Configuration);
            builder.Configuration.AddJsonSqlConfigProvider();
        }

        public static void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.MapControllers();
        }

        private static void LightDbTest(WebApplication app)
        {
            var logger = app.Services.GetService<ILogger<Program>>();

            // Test Db
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<JsonSqlContext>();
            //context.Database.EnsureCreated(); // Rebuild db if needed, can also be done in CLI
            var unit = context.JsonUnits.OrderBy(u => u.JsonUnitId).FirstOrDefault();
            logger.LogDebug("First JsonUnit has id {id}", unit?.JsonUnitId);

            // Indirect test by getting a property stored in Db
            var propertyName = "TESTARRAYA:0";
            var property = app.Configuration[propertyName];
            logger.LogDebug("Property {propertyname} has value {value}", propertyName, property);
        }
    }
}
   