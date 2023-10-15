using JsonSqlConfigDb.Provider;
using JsonSqlConfigDb.Service;
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

        public static IConfigurationBuilder AddJsonSqlConfigProvider(this IConfigurationBuilder builder) 
        {
            return builder.Add(new JsonSqlConfigSource());
        }
    }
}
