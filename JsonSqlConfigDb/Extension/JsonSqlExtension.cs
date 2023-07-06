using JsonSqlConfigDb.Provider;
using JsonSqlConfigDb.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JsonSqlConfigDb.Extension
{
    public static class JsonSqlExtension
    {
        public static IServiceCollection AddJsonSqlConfigDb(this IServiceCollection services, IConfiguration config)
        {
            JsonSqlSettings.CreateInstance(config);

            Action<DbContextOptionsBuilder> optionsBuilderAction = ob => ob
                .UseSqlServer(JsonSqlSettings.Instance.GetConnectionString())
                // When an Ilogger is configured the LogTo method is not strictly necessary
                //.LogTo(m => Console.WriteLine(m), new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                .EnableSensitiveDataLogging(JsonSqlSettings.Instance.SensitiveLogging);

            services.AddDbContext<JsonSqlContext>(optionsBuilderAction);

            // Store options action
            JsonSqlContext.OptionsAction = optionsBuilderAction;

            return services;
        }

        public static IConfigurationBuilder AddJsonSqlConfigProvider(this IConfigurationBuilder builder) 
        {
            return builder.Add(new JsonSqlConfigSource());
        }
    }
}
