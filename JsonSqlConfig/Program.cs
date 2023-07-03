
using JsonSqlConfigDb.Service;
using JsonSqlConfigDb;
using Microsoft.EntityFrameworkCore;
using JsonSqlConfigDb.Settings;
using JsonSqlConfigDb.Extension;

namespace JsonSqlConfig
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            LightDbTest(app);

            Configure(app);

            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddJsonSqlConfigDb(config);
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
                var context = scope.ServiceProvider.GetService<JsonSqlContext>();
                var unit = context.JsonUnits.OrderBy(u => u.JsonUnitId).FirstOrDefault();
            }
        }
    }
}
   