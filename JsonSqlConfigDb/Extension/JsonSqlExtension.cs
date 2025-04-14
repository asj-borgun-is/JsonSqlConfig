using JsonSqlConfigDb.Provider;
using JsonSqlConfigDb.Service;
using JsonSqlConfigDb.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JsonSqlConfigDb.Extension
{
    public static class JsonSqlExtension
    {
        public static IServiceCollection AddJsonSqlConfigDb(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, bool scopedService = true)
        {
            services.AddDbContext<JsonSqlContext>(optionsAction);
            if (scopedService) services.AddScoped<IJsonSqlService, JsonSqlService>();
            else services.AddTransient<IJsonSqlService, JsonSqlService>();

            // Store options action
            JsonSqlContext.OptionsAction = optionsAction;

            return services;
        }

        public static IServiceCollection AddJsonSqlConfigDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JsonSqlSettings>()
                .Configure<IConfiguration>(
                    (opt, conf) => conf.GetSection(nameof(JsonSqlSettings)).Bind(opt)
                );

            var settings = configuration.GetSection(nameof(JsonSqlSettings)).Get<JsonSqlSettings>();

            services.AddDbContext<JsonSqlContext>(ob => ob
                .UseSqlServer(settings.GetConnectionString())
                .EnableSensitiveDataLogging(settings.SensitiveLogging));

            services.AddScoped<IJsonSqlService, JsonSqlService>();

            return services;
        }

        public static IConfigurationBuilder AddJsonSqlConfigProvider(this IConfigurationBuilder builder) 
        {
            return builder.Add(new JsonSqlConfigSource());
        }
    }
}
