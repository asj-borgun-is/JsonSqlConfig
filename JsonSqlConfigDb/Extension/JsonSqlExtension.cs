﻿using JsonSqlConfigDb.Provider;
using JsonSqlConfigDb.Service;
using JsonSqlConfigDb.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace JsonSqlConfigDb.Extension
{
    public static class JsonSqlExtension
    {
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

            services.TryAddScoped<IJsonSqlService, JsonSqlService>();

            services.AddScoped<JsonSqlConfigProvider>();

            return services;
        }

        public static IConfigurationBuilder AddJsonSqlConfigProvider(this IConfigurationBuilder builder, IServiceCollection services) 
        {
            return builder.Add(new JsonSqlConfigSource(services));
        }
    }
}
