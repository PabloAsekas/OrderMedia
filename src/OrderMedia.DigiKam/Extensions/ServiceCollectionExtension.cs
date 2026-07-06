using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MySqlConnector;
using OrderMedia.DigiKam.Configuration;
using OrderMedia.DigiKam.Interfaces;
using OrderMedia.DigiKam.Repositories;
using OrderMedia.DigiKam.Services;

namespace OrderMedia.DigiKam.Extensions;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds DigiKam services.
    /// </summary>
    /// <param name="services">Service Collection.</param>
    /// <param name="configure">Configuration.</param>
    /// <returns><see cref="IServiceCollection"/> with all the services needed by the project.</returns>
    public static IServiceCollection AddDigiKam(
        this IServiceCollection services,
        Action<DigiKamOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);
        
        services
            .AddOptions<DigiKamOptions>()
            .Configure(configure)
            .ValidateDataAnnotations()
            .Validate(options => !string.IsNullOrWhiteSpace(options.ConnectionString),
                $"{nameof(DigiKamOptions.ConnectionString)} is required.")
            .ValidateOnStart();

        services.AddSingleton(sp =>
        {
            var digiKam = sp.GetRequiredService<IOptions<DigiKamOptions>>().Value;

            var builder = new MySqlDataSourceBuilder(digiKam.ConnectionString);
            return builder.Build();
        });

        services.AddScoped<IRepository, MariaDbRepository>();
        services.AddScoped<IDigiKamService, DigiKamService>();
        
        return services;
    }
}