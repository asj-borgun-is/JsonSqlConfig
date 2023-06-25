
using JsonSqlConfig.Experiments;
using JsonSqlConfig.Settings;
using JsonSqlConfigDb;
using Microsoft.EntityFrameworkCore;

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

            DbExperiment(app);

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

            services.AddScoped<IJsonParser, JsonParser>();
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

        private static void DbExperiment(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<JsonSqlConfigContext>();
                var unit = context.JsonUnits.FirstOrDefault();
            }
        }
    }
}
   