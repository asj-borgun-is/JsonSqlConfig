
using JsonSqlConfigDb.Service;
using JsonSqlConfigDb;
using JsonSqlConfigDb.Extension;
using JsonSqlConfig.Settings;
using Microsoft.EntityFrameworkCore;

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
            JsonSqlSettings.CreateInstance(builder.Configuration);
            
            builder.Services.AddApplicationInsightsTelemetry();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddJsonSqlConfigDb(ob => ob
                .UseSqlServer(JsonSqlSettings.Instance.GetConnectionString())
                // When an Ilogger is configured the LogTo method is not strictly necessary
                //.LogTo(m => Console.WriteLine(m), new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                .EnableSensitiveDataLogging(JsonSqlSettings.Instance.SensitiveLogging));
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
            var unit = context.JsonUnits.OrderBy(u => u.JsonUnitId).FirstOrDefault();
            logger.LogDebug("First JsonUnit has id {id}", unit?.JsonUnitId);

            // Indirect test by getting a property stored in Db
            var propertyName = "TESTARRAYA:0";
            var property = app.Configuration[propertyName];
            logger.LogDebug("Property {propertyname} has value {value}", propertyName, property);
        }
    }
}
   