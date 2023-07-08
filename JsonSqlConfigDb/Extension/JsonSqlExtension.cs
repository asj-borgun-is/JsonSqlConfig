using JsonSqlConfigDb.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JsonSqlConfigDb.Extension
{
    public static class JsonSqlExtension
    {
        public static IServiceCollection AddJsonSqlConfigDb(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.AddDbContext<JsonSqlContext>(optionsAction);

            // Store options action
            JsonSqlContext.OptionsAction = optionsAction;

            return services;
        }

        public static IConfigurationBuilder AddJsonSqlConfigProvider(this IConfigurationBuilder builder) 
        {
            return builder.Add(new JsonSqlConfigSource());
        }
    }
}
