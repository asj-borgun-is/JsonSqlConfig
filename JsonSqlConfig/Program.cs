
using JsonSqlConfigDb.Service;
using JsonSqlConfigDb;
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

            builder.Configuration.AddJsonSqlConfigProvider();
            LightDbTest(app);

            Configure(app);

            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddJsonSqlConfigDb(configuration);
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
            // Test Db
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<JsonSqlContext>();
            var unit = context.JsonUnits.OrderBy(u => u.JsonUnitId).FirstOrDefault();

            // Indirect test by getting a property stored in Db
            var property = app.Configuration["TESTARRAYA:0"];
        }
    }
}
   