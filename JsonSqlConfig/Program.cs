
using JsonSqlConfigDb.Service;
using JsonSqlConfigDb;
using Microsoft.EntityFrameworkCore;
using JsonSqlConfig.Settings;

namespace JsonSqlConfig
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            JsonSqlConfigSettings.CreateInstance(builder.Configuration);

            ConfigureServices(builder.Services);

            var app = builder.Build();

            LightDbTest(app);

            Configure(app);

            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<JsonSqlConfigContext>(options => options
                .UseSqlServer(JsonSqlConfigSettings.Instance.GetConnectionString())
                // When an Ilogger is configured the LogTo method is not strictly necessary
                //.LogTo(m => Console.WriteLine(m), new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                .EnableSensitiveDataLogging(true)
                );

            services.AddScoped<IJsonSqlService, JsonSqlService>();
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
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<JsonSqlConfigContext>();
                var unit = context.JsonUnits.OrderBy(u => u.JsonUnitId).FirstOrDefault();
            }
        }
    }
}
   